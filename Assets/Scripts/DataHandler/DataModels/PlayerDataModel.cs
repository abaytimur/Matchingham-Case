using System;

namespace DataHandler.DataModels
{
    [Serializable]
    public class PlayerDataModel : DataModel<PlayerDataModel>
    {
        public static PlayerDataModel Data;

        public int lastCompletedLevel;
        public int starsCollected;

        public PlayerDataModel Load()
        {
            if (Data == null)
            {
                Data = this;
                PlayerDataModel data = LoadJson();

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