using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class GameTipsComponent : GameFrameworkComponent
    {
        public float TimeTnterval = 0.4f;

        private Queue<string> m_TipsList = null;
        private float m_LastPlayTime = 0f;

        private void Start()
        {
            m_TipsList = new Queue<string>();
        }

        private void Update()
        {
            if (Time.time - m_LastPlayTime > TimeTnterval)
            {
                PlayTips();
                m_LastPlayTime = Time.time;
            }
        }

        public void PlayTips(string tips)
        {
            m_TipsList.Enqueue(tips);
        }

        private void PlayTips()
        {
            if (m_TipsList.Count == 0)
            {
                return;
            }

            string tips = m_TipsList.Dequeue();
            GameEntry.UI.OpenUIForm(Cfg.UI.FormType.TipsForm, tips);
        }
    }
}
