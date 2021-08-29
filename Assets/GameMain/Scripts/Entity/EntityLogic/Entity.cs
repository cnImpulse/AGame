using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public abstract class Entity : EntityLogic
    {
        [SerializeField]
        private EntityData m_EntityData = null;

        public int Id
        {
            get
            {
                return Entity.Id;
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_EntityData = userData as EntityData;
            if (m_EntityData == null)
            {
                Log.Error("Entity data is invalid!");
                return;
            }

            Name = Utility.Text.Format("[Entity {0}]", Id);
        }
    }
}
