using System.Collections.Generic;
using UnityEngine.UI;
using UnityGameFramework.Runtime;
using DG.Tweening;

namespace SSRPG
{
    public class StoryForm : UIForm
    {
        private Button m_ContinueBtn = null;
        private Text m_InfoTxt = null;

        Queue<string> m_StoryQueue = null;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_StoryQueue = new Queue<string>();

            m_ContinueBtn = GetComponent<Button>();
            m_InfoTxt = GetChild<Text>("m_InfoTxt");

            m_ContinueBtn.onClick.AddListener(PlayStory);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_StoryQueue.Enqueue("有一个人前来卖瓜。");
            m_StoryQueue.Enqueue("生异形吗，你们哥俩。");
            m_StoryQueue.Enqueue("哥们儿，这瓜多少钱一斤啊。");
            m_StoryQueue.Enqueue("两块钱一斤。");

            PlayStory();
        }

        private void PlayStory()
        {
            if (m_StoryQueue.Count == 0)
            {
                List<int> rewardIdList = new List<int>();
                rewardIdList.Add(1001);
                rewardIdList.Add(1002);
                rewardIdList.Add(1003);
                GameEntry.UI.OpenUIForm(Cfg.UI.FormType.RewardForm, rewardIdList);
                Close();
                return;
            }

            string story = m_StoryQueue.Dequeue();
            m_InfoTxt.DOKill();
            m_InfoTxt.text = "";
            m_InfoTxt.DOText(story, story.Length * 0.2f);
        }
    }
}
