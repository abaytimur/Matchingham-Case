using Components.Pools;
using Components.SelectionSquare;
using Controllers;
using Events.External;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        [TabGroup("Controller")] 
        [TabGroup("Gameplay")] [SerializeField] private SelectionSquare[] selectionSquares ;
        [TabGroup("Other")] [SerializeField] private Camera mainCamera;
        [TabGroup("Other")] [SerializeField] private StringBasedPool stringBasedPool;

        public override void InstallBindings()
        {
            Container.Bind<GameSceneEvents>().AsSingle();
            Container.Bind<SelectionSquareManager>().AsSingle().NonLazy();
            Container.BindInstance(mainCamera).AsSingle().NonLazy();
            Container.BindInstance(stringBasedPool).AsSingle().NonLazy();
            Container.BindInstance(selectionSquares).AsSingle();
        }
    }
}