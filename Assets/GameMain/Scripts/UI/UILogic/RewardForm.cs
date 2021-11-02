using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class RewardForm : UIForm
    {
        private Button m_EnsureBtn = null;
        private UIListTemplate m_RewardList = null;

        private List<int> m_RewardDataList = null;
        private int m_SelectedRewardId = 0;

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_RewardList = GetChild<UIListTemplate>("m_RewardList");
            m_RewardList.AddListener(OnRewardListInit);

            m_EnsureBtn = GetChild<Button>("m_EnsureBtn");
            m_EnsureBtn.onClick.AddListener(OnClickEnsureBtn);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_SelectedRewardId = 0;
            m_RewardDataList = userData as List<int>;

            m_RewardList.InitList();
            m_RewardList.AddItems(m_RewardDataList.Count);
            GameEntry.GameTips.PlayTips("选择一个角色作为开山弟子。");
        }

        private void OnRewardListInit(int index, UIItemTemplate item)
        {
            var m_ItemImg = item.GetChild<Image>("m_ItemImg");
            var m_ItemName = item.GetChild<Text>("m_ItemName");
            var m_ItemBtn = item.GetChild<Button>("m_ItemBtn");

            int rewardId = m_RewardDataList[index];
            var cfg = GameEntry.Cfg.Tables.TblBattleUnit.Get(rewardId);

            m_ItemName.text = cfg.Name;
            m_ItemBtn.onClick.AddListener(()=> {
                GameEntry.GameTips.PlayTips(cfg.Desc);
                OnClickRewardItem(item, index);
            });
        }

        private void OnClickEnsureBtn()
        {
            if (m_SelectedRewardId == 0)
            {
                return;
            }

            GameEntry.Event.Fire(this, EnsureRewardEventArgs.Create(m_SelectedRewardId));
            Close();
        }

        private void OnClickRewardItem(UIItemTemplate item, int index)
        {
            InitRewardList();
            m_SelectedRewardId = m_RewardDataList[index];

            var m_ItemBtn = item.GetChild<Button>("m_ItemBtn");
            m_ItemBtn.image.color = new Color(0.09f, 0.09f, 0.09f);
        }

        private void InitRewardList()
        {
            var items = m_RewardList.GetAllItems();
            foreach (var item in items.Values)
            {
                var m_ItemBtn = item.GetChild<Button>("m_ItemBtn");
                m_ItemBtn.image.color = new Color(0.8f, 0.8f, 0.8f);
            }
        }
    }
}
