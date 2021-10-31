using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace SSRPG
{
    public class StoryForm : UIForm
    {
        private Text m_InfoTxt = null;
        
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_InfoTxt = GetChild<Text>("m_InfoTxt");
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_InfoTxt.text = "";
            Tweener tweener = m_InfoTxt.DOText("先人抚我顶，结发受长生。", 2);

            tweener.OnComplete(() =>
            {
                m_InfoTxt.text = "";
                m_InfoTxt.DOText("时来天地皆同力，运去英雄不自由。", 4);
            });
        }
    }
}
