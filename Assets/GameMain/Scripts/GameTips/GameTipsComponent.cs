using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class GameTipsComponent : GameFrameworkComponent
    {
        Queue<int> m_TipsFormList = null;

        private void Start()
        {
            m_TipsFormList = new Queue<int>();
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenTipsForm);
        }

        public void PlayTips(string tips)
        {
            m_TipsFormList.Enqueue((int)GameEntry.UI.OpenUIForm(Cfg.UI.FormType.TipsForm, tips));
        }

        public void StopAllTips()
        {
            foreach (var id in m_TipsFormList)
            {
                GameEntry.UI.CloseUIForm(true, id);
            }
        }

        public void OnOpenTipsForm(object sender, GameEventArgs e)
        {
            var ne = (OpenUIFormSuccessEventArgs)e;

            if (ne.UIForm.Logic is TipsForm)
            {

            }
        }
    }
}
