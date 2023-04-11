using Components.Pools;
using Components.SelectionSquare;
using Components.UI;
using Controllers;
using DataHandler.GameDatas.Level;
using Events.External;
using JetBrains.Annotations;
using Miscellaneous;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        [TabGroup("Gameplay")] [SerializeField]
        private SelectionSquare[] selectionSquares;

        [TabGroup("Levels")] [InlineButton("GetAllLevelsFromPath")] [SerializeField]
        private LevelDataSo[] levelDataSos;

        [TabGroup("Screens")] [SerializeField] private LoadingScreen loadingScreen;
        [TabGroup("Screens")] [SerializeField] private MainMenuScreen mainMenuScreen;
        [TabGroup("Screens")] [SerializeField] private GameScreen gameScreen;
        [TabGroup("Screens")] [SerializeField] private EndGameScreen endGameScreen;

        [TabGroup("Other")] [SerializeField] private Camera mainCamera;
        [TabGroup("Other")] [SerializeField] private StringBasedPool stringBasedPool;
        [TabGroup("Other")] [SerializeField] private RotateObjects rotateObjects;

        public override void InstallBindings()
        {
            Container.Bind<GameSceneEvents>().AsSingle();
            Container.BindInterfacesAndSelfTo<SelectionSquareManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.BindInstance(mainCamera).AsSingle().NonLazy();
            Container.BindInstance(stringBasedPool).AsSingle().NonLazy();
            Container.BindInstance(rotateObjects).AsSingle().NonLazy();
            Container.BindInstance(selectionSquares).AsSingle();
            Container.BindInstance(loadingScreen).AsSingle();
            Container.BindInstance(mainMenuScreen).AsSingle();
            Container.BindInstance(gameScreen).AsSingle();
            Container.BindInstance(endGameScreen).AsSingle();
            if (levelDataSos.IsNullOrEmpty())
                Debug.LogError($"There are no levels to lead." +
                               $" Please add levels to the {nameof(levelDataSos)} list.");
            Container.BindInstance(levelDataSos).AsSingle();
            Container.Bind<LevelManager>().AsSingle().NonLazy();
        }

        // InlineButton is using this method.
        [UsedImplicitly]
        private void GetAllLevelsFromPath() => levelDataSos = Helpers.GetAllLevelDataSo().ToArray();
    }
}