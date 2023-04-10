using System.Collections.Generic;
using System.Linq;
using Components.Pools;
using Components.SelectionSquare;
using Controllers;
using Data.GameData.Level;
using Events.External;
using Miscellaneous;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        [TabGroup("Gameplay")] [SerializeField]
        private SelectionSquare[] selectionSquares;

        [TabGroup("Levels")] [InlineButton("GetAllLevelsFromPath")] [SerializeField]
        private List<LevelDataSo> levelList = new();

        [TabGroup("Other")] [SerializeField] private Camera mainCamera;
        [TabGroup("Other")] [SerializeField] private StringBasedPool stringBasedPool;

        public override void InstallBindings()
        {
            Container.Bind<GameSceneEvents>().AsSingle();
            Container.Bind<SelectionSquareManager>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().NonLazy();
            Container.BindInstance(mainCamera).AsSingle().NonLazy();
            Container.BindInstance(stringBasedPool).AsSingle().NonLazy();
            Container.BindInstance(selectionSquares).AsSingle();

            if (levelList.Count == 0)
                Debug.LogError($"There are no levels to lead." +
                               $" Please add levels to the {nameof(levelList)} list.");
            Container.BindInstance(levelList).AsSingle();
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().NonLazy();
        }

        private void GetAllLevelsFromPath()
        {
            levelList.Clear();
            levelList = Helpers.GetAllLevelDataSo().ToList();
        }
    }
}