using Events.External;
using Zenject;

namespace Installers.Scene
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<GameSceneEvents>().AsSingle();
        }
    }
}