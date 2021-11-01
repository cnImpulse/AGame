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

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_RewardList = GetChild<UIListTemplate>("m_RewardList");
            m_RewardList.AddListener(OnRewardListShow);

            m_EnsureBtn = GetChild<Button>("m_EnsureBtn");
            m_EnsureBtn.onClick.AddListener(OnClickEnsureBtn);
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_RewardDataList = userData as List<int>;
            m_RewardList.AddItems(m_RewardDataList.Count);
        }

        private void OnRewardListShow(int index)
        {
            var item = m_RewardList.GetItem(index);
            var m_ItemImg = item.GetChild<Image>("m_ItemImg");
            var m_ItemName = item.GetChild<Text>("m_ItemName");

            int rewardId = m_RewardDataList[index];
            var cfg = GameEntry.Cfg.Tables.TblBattleUnit.Get(rewardId);
            m_ItemName.text = cfg.Name;
        }

        private void OnClickEnsureBtn()
        {

        }
    }
}
