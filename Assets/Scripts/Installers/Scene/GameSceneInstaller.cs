using Events.External;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        [TabGroup("Others")] [SerializeField] private Camera mainCamera;

        public override void InstallBindings()
        {
            Container.Bind<GameSceneEvents>().AsSingle();
            Container.BindInstance(mainCamera).AsSingle().NonLazy();
        }
    }
}