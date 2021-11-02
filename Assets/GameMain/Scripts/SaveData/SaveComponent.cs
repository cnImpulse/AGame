using System.Collections.Generic;
using System.IO;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class SaveComponent : GameFrameworkComponent
    {
        private int m_SaveIndex = -1;
        private SaveData m_SaveData = null;

        public SaveData SaveData => m_SaveData;

        public bool Inited
        {
            get
            {
                return m_SaveData != null;
            }
        }

        public string DataPath
        {
            get
            {
                return AssetUtl.GetSaveDataPath(m_SaveIndex);
            }
        }

        public bool HasSave(int saveIndex)
        {
            string path = AssetUtl.GetSaveDataPath(saveIndex);
            return File.Exists(path);
        }

        public void InitSaveData(int saveIndex)
        {
            m_SaveIndex = saveIndex;
            if (HasSave(m_SaveIndex))
            {
                m_SaveData = AssetUtl.ReadData<SaveData>(DataPath);
            }
            else
            {
                m_SaveData = new SaveData();
            }
        }

        public void Save()
        {
            if (!Inited)
            {
                Log.Warning("没有存档数据!");
            }

            AssetUtl.SaveData(DataPath, m_SaveData);
        }

        public void Remove(int savaIndex)
        {
            File.Delete(AssetUtl.GetSaveDataPath(savaIndex));
        }
    }
}
