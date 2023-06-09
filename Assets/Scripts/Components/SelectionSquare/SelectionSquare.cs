using Components.Items;
using UnityEngine;

namespace Components.SelectionSquare
{
    public class SelectionSquare : MonoBehaviour
    {
        [field: SerializeField] public bool IsOccupied { get; private set; }
        [SerializeField] private Transform collectPoint;
        public MatchItem CurrentItem { get; private set; }
        
        public void PlaceItem()
        {
            SetOccupied(true);
            CurrentItem.SetRigidbody(true);
            Transform itemTransform = CurrentItem.transform;
            itemTransform.SetParent(collectPoint);
            itemTransform.localScale = Vector3.one / 2f;
            itemTransform.position = collectPoint.position;
            itemTransform.rotation = collectPoint.rotation;
        }
        
        public void SetOccupied(bool isOccupied) => IsOccupied = isOccupied;
        public void SetCurrentItem(MatchItem item) => CurrentItem = item;
        
        public void ClearSlot()
        {
            SendToPool();
            CurrentItem = null;
        }

        private void SendToPool()
        {
            if (CurrentItem is null)
                return;
            
            CurrentItem.SetRigidbody(false);
            Transform itemTransform = CurrentItem.transform;
            itemTransform.SetParent(null);
            itemTransform.localScale = Vector3.one;
            CurrentItem.gameObject.SetActive(false);
        }
    }
}