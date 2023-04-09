using UnityEngine;

namespace Abstract
{
    public interface IHit
    {
        void OnHit(RaycastHit hitInfo);
    }
}