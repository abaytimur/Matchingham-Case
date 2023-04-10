using DataHandler.GameDatas.Level;
using JetBrains.Annotations;
using UnityEngine.Events;

namespace Events.External
{
    [UsedImplicitly]
    public class GameSceneEvents
    {
        public UnityAction<LevelDataSo> OnLevelStart;
        public UnityAction<bool> OnLevelEnd;
        
        public UnityAction OnDataLoadCompleted;
    }
}