using System.Collections.Generic;
using System.Linq;
using Components.MatchItem;
using Components.SelectionSquare;
using JetBrains.Annotations;
using Miscellaneous;
using Zenject;

namespace Controllers
{
    [UsedImplicitly]
    public class SelectionSquareManager
    
    {
        private readonly List<SelectionSquare> _selectionSquareList;

        [Inject]
        private SelectionSquareManager(SelectionSquare[] selectionSquares) => _selectionSquareList = selectionSquares.ToList();


        public void PlaceItemOnSelectionSquare(MatchItem item)
        {
            // Check if all SelectionSquares are occupied
            if (_selectionSquareList.All(square => square.IsOccupied))
            {
                return;
            }

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
            if (matchCount < Helpers.ItemNumberToMatch) return;
            
            int removeStartIndex = leftIndex + 1;
            int removeEndIndex = index;

            for (int i = removeStartIndex; i <= removeEndIndex; i++)
            {
                _selectionSquareList[i].ClearSlot();
                _selectionSquareList[i].SetOccupied(false);
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