using System;

namespace DataHandler.DataModels
{
    [Serializable]
    public class SettingDataModel : DataModel<SettingDataModel>
    {
        public static SettingDataModel Data;

        public bool vibration;
        public bool sound;

        public SettingDataModel Load()
        {
            if (Data == null)
            {
                Data = this;
                SettingDataModel data = LoadJson();

                if (data != null)
                {
                    Data = data;
                }
            }

            return Data;
        }

        public void Save()
        {
            SaveJson();
        }
    }
}