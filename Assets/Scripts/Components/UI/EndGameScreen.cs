using Controllers;
using UnityEngine;
using Zenject;

namespace Components.UI
{
    public class EndGameScreen : ScreenBase
    {
        [SerializeField] private GameObject successScreen;
        [SerializeField] private GameObject failScreen;
        private LevelManager _levelManager;

        [Inject]
        private void Construct(LevelManager levelManager) => _levelManager = levelManager;

        public void ShowScreen(bool isLevelCompleted)
        {
            successScreen.SetActive(isLevelCompleted);
            failScreen.SetActive(!isLevelCompleted);
        }

        public void NextButton() => _levelManager.PlayNextLevel();
        public void RetryButton() => _levelManager.PlayCurrentLevel();
    }
}