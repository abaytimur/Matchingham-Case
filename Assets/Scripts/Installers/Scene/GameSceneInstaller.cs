using Components.Pools;
using Events.External;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        [TabGroup("Others")] [SerializeField] private Camera mainCamera;
        [TabGroup("Others")] [SerializeField] private StringBasedPool stringBasedPool;

        public override void InstallBindings()
        {
            Container.Bind<GameSceneEvents>().AsSingle();
            Container.BindInstance(mainCamera).AsSingle().NonLazy();
            Container.BindInstance(stringBasedPool).AsSingle().NonLazy();
        }
    }
}