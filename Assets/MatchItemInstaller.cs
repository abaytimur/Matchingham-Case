using QuickOutline.Scripts;
using UnityEngine;
using Zenject;

public class MatchItemInstaller  : MonoInstaller<MatchItemInstaller>
{
    [SerializeField] private Outline outline;

    public override void InstallBindings()
    {
        // Container.Bind<MatchItemInternalEvents>().AsSingle();
        Container.BindInstance(outline).AsSingle();
    }

    private void Reset() => outline = GetComponent<Outline>();
}
