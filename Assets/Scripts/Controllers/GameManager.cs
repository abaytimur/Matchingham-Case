using JetBrains.Annotations;
using UnityEngine;
using Zenject;

namespace Controllers
{
    [UsedImplicitly]
    public class GameManager : IInitializable
    {
        public void Initialize()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = -1;
        }
    }
}