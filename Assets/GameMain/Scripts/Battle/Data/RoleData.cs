using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cfg.Battle;

namespace SSRPG
{
    public class RoleData
    {
        public int TypeId { get; private set; }
        public string Name { get; private set; }
        public int Level { get; private set; }
        public Talent Talent { get; private set; }
        public Weapon Weapon { get; private set; }
        public List<int> SkillList { get; private set; }
        public Attribute BaseAttribute { get; private set; }
        public Attribute EquipmentAddition => new Attribute(Weapon.Addition);
        public Attribute Attribute => BaseAttribute + EquipmentAddition;

        public RoleData(int typeId)
        {
            var cfg = GameEntry.Cfg.Tables.TblRole.Get(typeId);

            TypeId = typeId;
            Name = cfg.Name;
            Level = 0;
            SkillList = cfg.SkillList;
            Talent = GameEntry.Cfg.Tables.TblTalent.Get(cfg.TalentType);
            Weapon = GameEntry.Cfg.Tables.TblWeapon.Get(cfg.Weapon);
            BaseAttribute = new Attribute(cfg.BaseAttribute);
        }

        public void UpLevel(int level)
        {
            Level += level;
            BaseAttribute += new Attribute(Talent) * level;
        }
    }
}
