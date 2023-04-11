using System;
using System.Linq;
using Components.Items;
using Components.SelectionSquare;
using DataHandler.DataModels;
using DataHandler.GameDatas.Level;
using Events.External;
using JetBrains.Annotations;
using Miscellaneous;
using UnityEngine;
using Zenject;

namespace Controllers
{
    using System.Collections.Generic;

    [UsedImplicitly]
    public class SelectionSquareManager : IInitializable, IDisposable
    {
        private readonly GameSceneEvents _gameSceneEvents;
        private readonly List<SelectionSquare> _selectionSquareList;
        private readonly RotateObjects _rotateObjects;
        private int _totalSpawnedItemsCount;
        private int _matchCount;

        [Inject]
        private SelectionSquareManager(GameSceneEvents gameSceneEvents, SelectionSquare[] selectionSquares,
            RotateObjects rotateObjects)
        {
            _gameSceneEvents = gameSceneEvents;
            _selectionSquareList = selectionSquares.ToList();
            _rotateObjects = rotateObjects;
        }

        public void Initialize()
        {
            _gameSceneEvents.OnLevelStart += OnLevelStart;
            _gameSceneEvents.OnLevelEnd += OnLevelEnd;
            _gameSceneEvents.OnSendTotalSpawnedItemsCount += OnGetTotalSpawnedItemsCount;
        }

        public void Dispose()
        {
            _gameSceneEvents.OnLevelStart -= OnLevelStart;
            _gameSceneEvents.OnLevelEnd -= OnLevelEnd;
            _gameSceneEvents.OnSendTotalSpawnedItemsCount -= OnGetTotalSpawnedItemsCount;
        }

        private void OnLevelStart(LevelDataSo levelDataSo) => _matchCount = 0;

        private void OnLevelEnd(bool isLevelCompleted)
        {
            foreach (var item in _selectionSquareList)
            {
                item.ClearSlot();
                item.SetOccupied(false);
            }
        }

        private void OnGetTotalSpawnedItemsCount(int totalSpawnedItemsCount) =>
            _totalSpawnedItemsCount = Convert.ToInt32(totalSpawnedItemsCount / Helpers.ItemNumberToMatch);

        public void PlaceItemOnSelectionSquare(MatchItem item)
        {
            // Check if all SelectionSquares are occupied
            if (_selectionSquareList.All(square => square.IsOccupied)) return;

            int lastIndex = _selectionSquareList.FindLastIndex(square =>
                square.CurrentItem is not null && square.CurrentItem.ItemName == item.ItemName);

            if (lastIndex != -1)
            {
                // Place the new item after the lastIndex
                lastIndex++;
            }
            else
            {
                // If there is no item with the same name, place the item in the first available slot
                lastIndex = _selectionSquareList.FindIndex(square => square.CurrentItem is null);
            }

            // Shift items to the right of the lastIndex
            for (int i = _selectionSquareList.Count - 1; i > lastIndex; i--)
            {
                _selectionSquareList[i].SetCurrentItem(_selectionSquareList[i - 1].CurrentItem);
            }

            if (lastIndex >= 0)
            {
                _selectionSquareList[lastIndex].SetCurrentItem(item);
            }

            _rotateObjects.AddObject(item.gameObject);
            CheckForOccupation();

            // Check if the required number of consecutive items have the same name and remove them if necessary
            CheckAndRemoveConsecutiveItems(lastIndex);
        }

        private void CheckForOccupation()
        {
            for (int i = 0; i < _selectionSquareList.Count; i++)
            {
                if (_selectionSquareList[i].CurrentItem is not null)
                {
                    _selectionSquareList[i].PlaceItem(_selectionSquareList[i].CurrentItem);
                }
                else
                {
                    _selectionSquareList[i].SetOccupied(false);
                }
            }
        }

        private void CheckAndRemoveConsecutiveItems(int index)
        {
            int matchCount = 1;
            int leftIndex = index - 1;

            // Check for matches to the left of the current item
            while (leftIndex >= 0 && _selectionSquareList[leftIndex].CurrentItem.ItemName ==
                   _selectionSquareList[index].CurrentItem.ItemName)
            {
                matchCount++;
                leftIndex--;
            }

            // If there are matches equal to or greater than the required match count, remove the items and adjust the list
            if (matchCount < Helpers.ItemNumberToMatch)
            {
                if (_selectionSquareList.All(square => square.IsOccupied))
                {
                    _gameSceneEvents.OnLevelEnd?.Invoke(false);
                }

                return;
            }

            int removeStartIndex = leftIndex + 1;
            int removeEndIndex = index;

            for (int i = removeStartIndex; i <= removeEndIndex; i++)
            {
                _selectionSquareList[i].ClearSlot();
                _selectionSquareList[i].SetOccupied(false);
            }

            _gameSceneEvents.OnItemsMatched?.Invoke();
            _matchCount++;

            Debug.Log($"{_matchCount}: matchCount ");
            Debug.Log($"{_totalSpawnedItemsCount}: _totalSpawnedItemsCount ");

            if (_matchCount >= _totalSpawnedItemsCount)
            {
                Debug.Log("Level End");
                PlayerDataModel.Data.lastCompletedLevel++;
                _gameSceneEvents.OnLevelEnd?.Invoke(true);
            }

            AdjustListAfterRemoval(removeStartIndex);
        }

        private void AdjustListAfterRemoval(int startIndex)
        {
            for (int i = startIndex; i < _selectionSquareList.Count - Helpers.ItemNumberToMatch; i++)
            {
                _selectionSquareList[i].SetCurrentItem(_selectionSquareList[i + Helpers.ItemNumberToMatch].CurrentItem);
                _selectionSquareList[i + Helpers.ItemNumberToMatch].SetCurrentItem(null);
                _selectionSquareList[i].SetOccupied(_selectionSquareList[i].CurrentItem is not null);
                if (_selectionSquareList[i].CurrentItem is not null)
                {
                    _selectionSquareList[i].PlaceItem(_selectionSquareList[i].CurrentItem);
                }
            }

            CheckForOccupation();
        }
    }
}