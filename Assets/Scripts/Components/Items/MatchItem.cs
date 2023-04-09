using Abstract;
using UnityEngine;

namespace Components.Items
{
    public class MatchItem : MonoBehaviour, IHit
    {
        public void OnHit(RaycastHit hitInfo)
        {
            Debug.Log("Hit object: " + hitInfo.transform.name);
        }
    }
}