using System.Collections;
using System.Collections.Generic;
using DataHandler.GameDatas.Level;
using Events.External;
using UnityEngine;
using Zenject;

namespace Components.SelectionSquare
{
    public class RotateObjects : MonoBehaviour
    {
        private GameSceneEvents _gameSceneEvents;

        private readonly List<GameObject> _objectsList = new();
        [SerializeField] private  float rotationSpeed = 30f;
        [SerializeField] private float updateInterval = 0.01f; // Time interval between rotation updates
        private float _rotationTime;
        private float _rotationAngle;
        private bool _canRotate;

        [Inject]
        private void Construct(GameSceneEvents gameEventsSo) => _gameSceneEvents = gameEventsSo;

        private void OnEnable() => RegisterEvents();
        private void OnDisable() => UnRegisterEvents();

        private void RegisterEvents()
        {
            _gameSceneEvents.OnLevelStart += OnLevelStart;
            _gameSceneEvents.OnLevelEnd += OnLevelEnd;
        }

        private void UnRegisterEvents()
        {
            _gameSceneEvents.OnLevelStart -= OnLevelStart;
            _gameSceneEvents.OnLevelEnd -= OnLevelEnd;
        }

        private void OnLevelStart(LevelDataSo levelDataSo)
        {
            _canRotate = true;
            StartCoroutine(UpdateRotation());
        }

        private void OnLevelEnd(bool isLevelCompleted)
        {
            _canRotate = false;
            _objectsList.Clear();
            StopAllCoroutines();
        }

        private IEnumerator UpdateRotation()
        {
            while (_canRotate)
            {
                // Update rotation time
                _rotationTime += rotationSpeed * updateInterval;

                // Calculate the new rotation angle using Mathf.PingPong()
                _rotationAngle = Mathf.PingPong(_rotationTime, 60) - 30;

                // Apply the same rotation angle to all objects in the list
                for (var i = 0; i < _objectsList.Count; i++)
                {
                    var obj = _objectsList[i];
                    if (!obj.activeInHierarchy)
                    {
                        _objectsList.Remove(obj);
                        continue;
                    }

                    obj.transform.rotation = Quaternion.Euler(45, _rotationAngle, 0);
                }

                // Wait for the specified time interval before updating rotation again
                yield return new WaitForSeconds(updateInterval);
            }
        }

        public void AddObject(GameObject newObj)
        {
            if (_objectsList.Contains(newObj))
            {
                return;
            }

            newObj.transform.rotation = Quaternion.Euler(45, _rotationAngle, 0);
            _objectsList.Add(newObj);
        }
    }
}