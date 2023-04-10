using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DataHandler.DataModels
{
    [Serializable]
    public class DataModel<T>
    {
        protected void SaveJson()
        {
            string json = JsonConvert.SerializeObject(this);
            Debug.Log(json);
            string filePath = Application.persistentDataPath + "/" + GetType().Name + ".json";
            File.WriteAllText(filePath, json);
        }
        
        protected T LoadJson()
        {
            string filePath = Application.persistentDataPath + "/" + GetType().Name + ".json";
    
            if (!File.Exists(filePath))
            {
                return default(T);
            }
    
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}