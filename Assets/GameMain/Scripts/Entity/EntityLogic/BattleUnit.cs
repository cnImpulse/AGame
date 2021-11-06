using GameFramework.Event;
using GameFramework.Fsm;
using GameFramework.Resource;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    /// <summary>
    /// 战斗单位。
    /// </summary>
    public class BattleUnit : GridUnit
    {
        private static Color enemyColor, playerColor;
        private SpriteRenderer m_SpriteRenderer = null;

        [SerializeField]
        private BattleUnitData m_Data = null;
        public new BattleUnitData Data => m_Data;

        public bool CanAction { get; private set; }
        public bool CanMove { get; private set; }

        private void InitSprite()
        {
            InternalSetVisible(false);
            switch (m_Data.CampType)
            {
                case CampType.Player: m_SpriteRenderer.color = playerColor; break;
                case CampType.Enemy: m_SpriteRenderer.color = enemyColor; break;
            }

            string path = AssetUtl.GetTileAsset("BattleUnit", m_Data.TypeId.ToString());
            if (GameEntry.Resource.HasAsset(path) == HasAssetResult.NotExist)
            {
                path = AssetUtl.GetTileAsset("BattleUnit", "default");
            }

            GameEntry.Resource.LoadAsset(path, typeof(Tile),
            (assetName, asset, duration, userData) =>
            {
                var tile = asset as Tile;
                m_SpriteRenderer.sprite = tile.sprite;
                InternalSetVisible(true);
            });
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            ColorUtility.TryParseHtmlString("#70FFF0", out playerColor);
            ColorUtility.TryParseHtmlString("#FF7070", out enemyColor);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = userData as BattleUnitData;

            InitSprite();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            m_Data = null;

            base.OnHide(isShutdown, userData);
        }

        #region 战斗单位状态

        public virtual void OnBattleStart()
        {

        }

        public virtual void OnRoundStart()
        {
            CanAction = true;
            CanMove = true;
        }

        public virtual void OnActionEnd()
        {
            if (!CanAction)
            {
                Log.Info("战斗单位重复行动!");
                return;
            }

            CanAction = false;
        }

        public virtual void OnRoundEnd()
        {
            if (CanAction)
            {
                OnActionEnd();
            }
        }

        public virtual void OnBattleEnd()
        {
            
        }

        #endregion

        //-----------------------------------------

        public void Move(Vector2Int destination)
        {
            CanMove = false;
            GridMap.MoveTo(this, destination);
            transform.position = GridMap.GridPosToWorldPos(destination);
        }

        public void Attack(GridData gridData)
        {
            if (gridData == null || gridData.GridUnit == null)
            {
                return;
            }

            DamageInfo damageInfo = new DamageInfo(m_Data.ATK, Id, gridData.GridUnit.Id);
            GameEntry.Event.Fire(this, EventName.GridUnitDamage, damageInfo);
        }
    }
}
