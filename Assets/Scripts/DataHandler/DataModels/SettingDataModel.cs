using System;

namespace DataHandler.DataModels
{
    [Serializable]
    public class SettingDataModel : DataModel<SettingDataModel>
    {
        public static SettingDataModel Data;
        
        // todo:
        // Burada kullanıdğım bir veri olmamasına rağmen yeni veri eklemek istediğimizde,
        // nasıl yapıldığını göstermek için ekledim bu classı
        // Bu projede kullanıcı verileri şu an PlayerDataModel.cs içinde yönetiliyor.
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