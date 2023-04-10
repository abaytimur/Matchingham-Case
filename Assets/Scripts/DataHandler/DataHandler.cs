using System.Diagnostics;
using System.IO;
using DataHandler.DataModels;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DataHandler
{
    public class DataHandler : MonoBehaviour
    {
        [BoxGroup("Settings")] public SettingDataModel setting;
        [BoxGroup("Player")] public PlayerDataModel player;

        public void Awake()
        {
            setting = new SettingDataModel().Load();
            player = new PlayerDataModel().Load();
        }

        private void OnApplicationQuit()
        {
            player.Save();
            setting.Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!pauseStatus) return;

            player.Save();
            setting.Save();
        }


        [Button]
        public void ClearAllData()
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
            for (int i = 0; i < files.Length; i++)
            {
                File.Delete(files[i]);
            }

            PlayerPrefs.DeleteAll();

            if (Directory.GetFiles(Application.persistentDataPath, "*.json").Length == 0)
            {
                Debug.Log("Data Clear Succeeded");
            }
        }

#if UNITY_EDITOR

        [Button]
        public void OpenSaveDirectory() => Process.Start(Application.persistentDataPath);

#endif
    }
}