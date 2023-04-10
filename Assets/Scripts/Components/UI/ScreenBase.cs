using DG.Tweening;
using UnityEngine;

namespace Components.UI
{
    public abstract class ScreenBase : MonoBehaviour
    {
        [SerializeField] protected Canvas canvas;

        public void Show(bool instant = false)
        {
            if (instant)
            {
                transform.localScale = Vector3.one;
                canvas.enabled = true;
            }
            else
            {
                transform.localScale = Vector3.zero;
                canvas.enabled = true;
                transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
            }
        }

        public void Hide(bool instant = false)
        {
            if (instant)
            {
                canvas.enabled = false;
            }
            else
            {
                transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => canvas.enabled = false);
            }
        }

        private void Reset() => canvas = GetComponent<Canvas>();
    }
}