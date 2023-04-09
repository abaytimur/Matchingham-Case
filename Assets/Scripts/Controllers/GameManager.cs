using UnityEngine;

namespace Controllers
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = -1;
        }
    }
}