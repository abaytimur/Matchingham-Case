using Abstract;
using Components.Items;
using DataHandler.GameDatas.Level;
using DG.Tweening;
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
        private IHit _currentHitItem;
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
                _heldItem.transform.DOKill();
                _selectionSquareManager.PlaceItemOnSelectionSquare(_heldItem);
                _currentHitItem.OnHit(false);
                _heldItem.SetRigidbody(true);
                _heldItem = null;
                _currentHitItem = null;
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
                if (_heldItem is not null && tempParent.gameObject != _heldItem.gameObject)
                {
                    ResetHeldItem();
                }

                if (!tempParent.TryGetComponent(out IHit hitScriptInParent)) return;
                _currentHitItem = hitScriptInParent;
                MatchItem newHeldItem = _currentHitItem as MatchItem;

                if (_heldItem == newHeldItem) return;
                ResetHeldItem();

                _heldItem = newHeldItem;
                if (_heldItem != null) _originalPosition = _heldItem.transform.position;
                HoldItem();

                hitScriptInParent.OnHit(true);
            }
        }

        private void HoldItem()
        {
            if (_heldItem is null) return;
            _heldItem.SetRigidbody(true);
            Vector3 direction = (_mainCamera.transform.position - _originalPosition).normalized;
            Vector3 position = _originalPosition + direction * displayHeight;
            _heldItem.transform.DOMove(position, .3f);
        }

        private void ResetHeldItem()
        {
            if (_heldItem is null) return;
            _currentHitItem.OnHit(false);
            _heldItem.SetRigidbody(false);
            _heldItem.transform.DOMove(_originalPosition, .3f);
            _currentHitItem = null;
            _heldItem = null;
        }
    }
}