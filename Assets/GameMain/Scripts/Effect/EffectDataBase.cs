using UnityEngine;
using GameFramework;
using UnityGameFramework.Runtime;

namespace SSRPG
{
    public class EffectDataBase : EntityData
    {
        public EffectDataBase(int id, int typeId, Vector3 position) : base(id, typeId)
        {
            Position = position;
        }
    }
}