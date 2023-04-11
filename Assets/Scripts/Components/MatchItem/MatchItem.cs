using Abstract;
using DG.Tweening;
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
        [SerializeField] private float rotationSpeed = .2f;
        [SerializeField] private int rotationLoops = -1;

        public void SetRigidbody(bool isKinematic) => rigidbody.isKinematic = isKinematic;

        public void OnHit(bool rayEnter)
        {
            if (rayEnter)
            {
                transform.DORotate(new Vector3(45.0f, 0.0f, 0.0f), .3f).OnComplete(() =>
                {
                    transform
                        .DORotate(new Vector3(0.0f, 360.0f, 0.0f), rotationSpeed, RotateMode.LocalAxisAdd).SetLoops(rotationLoops).SetEase(Ease.Linear);
                });

                outline.OutlineWidth = 4f;
            }
            else
            {
                transform.DOKill();
                outline.OutlineWidth = 0f;
            }
        }

        private void Reset()
        {
            ItemName = name;
            rigidbody = GetComponent<Rigidbody>();
            outline = GetComponent<Outline>();
        }
    }
}