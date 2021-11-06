using UnityEngine;

namespace SSRPG
{
    public class EffectData : EntityData
    {
        public EffectData(int id, int typeId, Vector3 position, float lifetime) : base(id, typeId)
        {
            Position = position;
            LifeTime = lifetime;
        }

        public Vector3 Position { get; set; }

        public float LifeTime { get; private set; }
    }
}