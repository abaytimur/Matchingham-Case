using Controllers;
using DataHandler.DataModels;
using TMPro;
using UnityEngine;
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
        private void Construct(LevelManager levelManager) => _levelManager = levelManager;

        public void SetLevelData()
        {
            playButtonText.SetText($"Level {PlayerDataModel.Data.lastCompletedLevel + 1}");
            SetButton();
        }

        private void SetButton()
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked() => _levelManager.StartLevel(PlayerDataModel.Data.lastCompletedLevel + 1);
    }
}