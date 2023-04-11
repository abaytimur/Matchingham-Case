using Abstract;
using QuickOutline.Scripts;
using UnityEngine;

namespace Components.MatchItem
{
    [SelectionBase]
    public class MatchItem : MonoBehaviour, IHit
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Outline outline;

        public void SetRigidbody(bool isKinematic) => rigidbody.isKinematic = isKinematic;
        public void OnHit(bool rayEnter) => outline.OutlineWidth = rayEnter ? 4f : 0f;
        private void Reset()
        {
            ItemName = name;
            rigidbody = GetComponent<Rigidbody>();
            outline = GetComponent<Outline>();
        }
    }
}