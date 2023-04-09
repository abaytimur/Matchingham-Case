using Abstract;
using Components.Items;
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

        [Inject]
        private void Construct(Camera mainCamera, SelectionSquareManager selectionSquareManager)
        {
            _mainCamera = mainCamera;
            _selectionSquareManager = selectionSquareManager;
        }

        void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, raycastRange, layerMask)) return;

            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.transform.parent is null)
                return;

            Transform tempParent = hitObject.transform.parent;
            if (!tempParent.TryGetComponent(out IHit hitScriptInParent)) return;
            // Debug.Log("Hit: " + hit.transform.name);
            hitScriptInParent.OnHit(hit);
            var matchItem = hitScriptInParent as MatchItem;
            _selectionSquareManager.PlaceItemOnSelectionSquare(matchItem);
        }
    }
}