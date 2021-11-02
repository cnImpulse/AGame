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

        public string CurrentSaveName
        {
            get
            {
                return GetSaveName(m_SaveIndex);
            }
        }

        public bool HasSave(int saveIndex)
        {
            return GameEntry.Setting.HasSetting(GetSaveName(saveIndex));
        }

        public void InitSaveData(int saveIndex)
        {
            m_SaveIndex = saveIndex;
            if (HasSave(m_SaveIndex))
            {
                m_SaveData = GetSaveData(m_SaveIndex);
            }
            else
            {
                m_SaveData = new SaveData();
            }
        }

        public SaveData GetSaveData(int index)
        {
            return GameEntry.Setting.GetObject<SaveData>(GetSaveName(index));
        }

        public void Save()
        {
            if (!Inited)
            {
                Log.Warning("没有存档数据!");
            }

            GameEntry.Setting.SetObject(CurrentSaveName, m_SaveData);
        }

        public void Delete(int savaIndex)
        {
            GameEntry.Setting.RemoveSetting(GetSaveName(savaIndex));
        }

        private string GetSaveName(int index)
        {
            return string.Format("SaveData_{0}", index);
        }

        #region EditSaveData

        public void AddDisciple(int id)
        {
            m_SaveData.DiscipleList.Add(id, id);
        }

        #endregion
    }
}
