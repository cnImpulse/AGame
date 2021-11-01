using System.Collections.Generic;
using GameFramework;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class GameTipsComponent : GameFrameworkComponent
    {
        private TipsForm m_Form = null;

        private void Start()
        {
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenTipsForm);
        }

        public void PlayTips(string tips)
        {
            if(m_Form != null)
            {
                m_Form.Close(true);
            }

            GameEntry.UI.OpenUIForm(UIFormId.TipsForm, tips);
        }

        public void OnOpenTipsForm(object sender, GameEventArgs e)
        {
            var ne = (OpenUIFormSuccessEventArgs)e;

            if (ne.UIForm.Logic is TipsForm)
            {
                m_Form = ne.UIForm.Logic as TipsForm;
                m_Form.OnCloseForm += () => { m_Form = null; };
            }
        }
    }
}
