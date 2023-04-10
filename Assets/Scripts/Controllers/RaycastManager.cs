using Abstract;
using Components.MatchItem;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class RaycastManager : MonoBehaviour
    {
        [SerializeField] private float raycastRange = 100.0f;
        [SerializeField] private LayerMask layerMask;
        private Camera _mainCamera;
        private SelectionSquareManager _selectionSquareManager;
        private MatchItem _heldItem;
        private Vector3 _originalPosition;
        [SerializeField] private float displayHeight = 0.75f;

        [Inject]
        private void Construct(Camera mainCamera, SelectionSquareManager selectionSquareManager)
        {
            _mainCamera = mainCamera;
            _selectionSquareManager = selectionSquareManager;
        }

        void Update()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            bool raycastHit = Physics.Raycast(ray, out var hit, raycastRange, layerMask);

            if (Input.GetMouseButtonDown(0))
            {
                if (!raycastHit) return;
                GameObject hitObject = hit.collider.gameObject;
                if (hitObject.transform.parent is null)
                    return;

                Transform tempParent = hitObject.transform.parent;
                if (!tempParent.TryGetComponent(out IHit hitScriptInParent)) return;

                hitScriptInParent.OnHit(hit);
                _heldItem = hitScriptInParent as MatchItem;

                if (_heldItem is not null)
                {
                    var position = _heldItem.transform.position;
                    _originalPosition = position;
                    _heldItem.SetRigidbody(true);
                    Vector3 direction = (_mainCamera.transform.position - _originalPosition).normalized;
                    position += direction * displayHeight;
                    _heldItem.transform.position = position;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_heldItem is not null && raycastHit)
                {
                    _selectionSquareManager.PlaceItemOnSelectionSquare(_heldItem);
                    _heldItem.SetRigidbody(true);
                    _heldItem = null;
                }
                else
                {
                    ResetHeldItem();
                }
            }
            else if (Input.GetMouseButton(0) && _heldItem is not null)
            {
                if (!raycastHit)
                {
                    ResetHeldItem();
                }
            }
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