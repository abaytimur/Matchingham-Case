using Abstract;
using Components.MatchItem;
using DataHandler.GameDatas.Level;
using Events.External;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class RaycastManager : MonoBehaviour
    {
        [SerializeField] private float raycastRange = 100.0f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float displayHeight = 0.75f;

        private Camera _mainCamera;
        private SelectionSquareManager _selectionSquareManager;
        private GameSceneEvents _gameSceneEvents;
        private MatchItem _heldItem;
        private Vector3 _originalPosition;
        private bool _canUseRaycast;

        [Inject]
        private void Construct(GameSceneEvents gameSceneEvents, Camera mainCamera,
            SelectionSquareManager selectionSquareManager)
        {
            _gameSceneEvents = gameSceneEvents;
            _mainCamera = mainCamera;
            _selectionSquareManager = selectionSquareManager;
        }

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
            if (levelDataSo is null) return;
            _canUseRaycast = true;
        }

        private void OnLevelEnd(bool isLevelCompleted) => _canUseRaycast = false;

        void Update()
        {
            if (!_canUseRaycast) return;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            bool raycastHit = Physics.Raycast(ray, out var hit, raycastRange, layerMask);

            if (Input.GetMouseButtonUp(0))
            {
                ProcessMouseButtonUp(raycastHit);
            }
            else if (Input.GetMouseButton(0))
            {
                ProcessMouseButtonDown(raycastHit, hit);
            }
        }

        private void ProcessMouseButtonUp(bool raycastHit)
        {
            if (_heldItem is null) return;
            if (raycastHit)
            {
                _selectionSquareManager.PlaceItemOnSelectionSquare(_heldItem);
                _heldItem.SetRigidbody(true);
                _heldItem = null;
            }

            ResetHeldItem();
        }

        private void ProcessMouseButtonDown(bool raycastHit, RaycastHit hit)
        {
            if (!raycastHit)
            {
                ResetHeldItem();
            }
            else
            {
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.transform.parent is null) return;
                Transform tempParent = hitObject.transform.parent;
                if (!tempParent.TryGetComponent(out IHit hitScriptInParent)) return;
                MatchItem newHeldItem = hitScriptInParent as MatchItem;

                if (_heldItem == newHeldItem) return;
                ResetHeldItem();

                _heldItem = newHeldItem;
                if (_heldItem != null) _originalPosition = _heldItem.transform.position;
                HoldItem();

                // Call OnHit() method for each displayed MatchItem
                hitScriptInParent.OnHit(hit);
            }
        }

        // Hold and Reset Held Item methods
        private void HoldItem()
        {
            if (_heldItem is null) return;
            _heldItem.SetRigidbody(true);
            Vector3 direction = (_mainCamera.transform.position - _originalPosition).normalized;
            Vector3 position = _originalPosition + direction * displayHeight;
            _heldItem.transform.position = position;
        }

        private void ResetHeldItem()
        {
            if (_heldItem is null) return;
            _heldItem.SetRigidbody(false);
            _heldItem.transform.position = _originalPosition;
            _heldItem = null;
        }
    }
}