using JetBrains.Annotations;
using UnityEngine.Events;

namespace Events.External
{
    [UsedImplicitly]
    public class GameSceneEvents
    {
        public UnityAction<int> OnLevelStart;
        public UnityAction<bool> OnLevelEnd;
    }
}