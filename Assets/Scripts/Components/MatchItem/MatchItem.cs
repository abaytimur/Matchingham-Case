using Abstract;
using UnityEngine;
using Zenject;

namespace Components.MatchItem
{
    [SelectionBase]
    public class MatchItem : MonoBehaviour, IHit
    {
        [field: SerializeField] public string ItemName { get; private set; }
        [SerializeField] private new Rigidbody rigidbody;

        [Inject]
        public void Construct(Outline outline)
        {
            outline.OutlineWidth = 4;
            Debug.Log("Outline injected");
        }

        public void OnHit(RaycastHit hitInfo)
        {
            Debug.Log("Hit object: " + hitInfo.transform.name);
        }

        public void SetRigidbody(bool isKinematic) => rigidbody.isKinematic = isKinematic;

        public void DestroyThis()
        {
            // Implement your destroy logic here, for example:
            Destroy(gameObject);
        }

        private void Reset()
        {
            ItemName = name;
            rigidbody = GetComponent<Rigidbody>();
        }
    }
}