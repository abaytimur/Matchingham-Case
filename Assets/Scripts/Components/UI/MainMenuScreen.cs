using Components.StateMachine.GamesStates;
using Controllers;
using DataHandler.DataModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Components.UI
{
    public class MainMenuScreen : ScreenBase
    {
        [SerializeField] Button playButton;
        [SerializeField] TextMeshProUGUI playButtonText;
        private LevelManager _levelManager;

        [Inject]
        private void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
        }

        public void SetLevelData()
        {
            playButtonText.SetText($"Level {PlayerDataModel.Data.lastCompletedLevel + 1}");
            SetButton();
        }

        private void SetButton()
        {
            print(" set button");
            playButton.onClick.RemoveAllListeners();
            //todo: look at this
            playButton.onClick.AddListener(() => ButtonClicked(PlayerDataModel.Data.lastCompletedLevel));
        }

        private void ButtonClicked(int levelNumber)
        {
            print(" button clicked");
            _levelManager.StartLevel(levelNumber);
        }
    }
}