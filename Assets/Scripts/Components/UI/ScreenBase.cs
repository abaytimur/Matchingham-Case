using DG.Tweening;
using UnityEngine;

namespace Components.UI
{
    public abstract class ScreenBase : MonoBehaviour
    {
        public virtual void Show()
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        }

        public virtual void Hide()
        {
            transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(() => gameObject.SetActive(false));
        }
    }
}