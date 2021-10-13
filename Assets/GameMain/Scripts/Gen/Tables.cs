
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;


namespace cfg
{
    public sealed class Tables
    {
        public Battle.TblBattleUnit TblBattleUnit {get; }
        public Battle.TblBattleUnitSkill TblBattleUnitSkill {get; }
        public UI.TblUIForm TblUIForm {get; }
        public Entity.TblEntity TblEntity {get; }
        public Effect.TblEffect TblEffect {get; }

        public Tables(System.Func<string, ByteBuf> loader)
        {
            var tables = new System.Collections.Generic.Dictionary<string, object>();
            TblBattleUnit = new Battle.TblBattleUnit(loader("battle_tblbattleunit")); 
            tables.Add("Battle.TblBattleUnit", TblBattleUnit);
            TblBattleUnitSkill = new Battle.TblBattleUnitSkill(loader("battle_tblbattleunitskill")); 
            tables.Add("Battle.TblBattleUnitSkill", TblBattleUnitSkill);
            TblUIForm = new UI.TblUIForm(loader("ui_tbluiform")); 
            tables.Add("UI.TblUIForm", TblUIForm);
            TblEntity = new Entity.TblEntity(loader("entity_tblentity")); 
            tables.Add("Entity.TblEntity", TblEntity);
            TblEffect = new Effect.TblEffect(loader("effect_tbleffect")); 
            tables.Add("Effect.TblEffect", TblEffect);

            TblBattleUnit.Resolve(tables); 
            TblBattleUnitSkill.Resolve(tables); 
            TblUIForm.Resolve(tables); 
            TblEntity.Resolve(tables); 
            TblEffect.Resolve(tables); 
        }

        public void TranslateText(System.Func<string, string, string> translator)
        {
            TblBattleUnit.TranslateText(translator); 
            TblBattleUnitSkill.TranslateText(translator); 
            TblUIForm.TranslateText(translator); 
            TblEntity.TranslateText(translator); 
            TblEffect.TranslateText(translator); 
        }
    }
}