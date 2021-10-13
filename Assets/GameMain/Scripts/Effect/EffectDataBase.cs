using UnityEngine;

namespace SSRPG
{
    public class EffectDataBase : EntityData
    {
        [SerializeField]
        private Vector3 m_Position = Vector3.zero;

        public EffectDataBase(int id, int typeId, Vector3 position) : base(id, typeId)
        {
            m_Position = position;
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }
    }
}