using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Fsm;
using GameFramework.Event;
using UnityGameFramework.Runtime;
using System;

namespace SSRPG
{
    public abstract class BattleStateBase : FsmState<ProcedureBattle>
    {
        public IFsm<ProcedureBattle> Fsm { get; private set; }
        public ProcedureBattle Owner => Fsm == null ? null : Fsm.Owner;

        public GridMap m_GridMap => Owner.GridMap;
        public CampType m_ActiveCamp
        {
            get
            {
                return (CampType)(int)Fsm.GetData<VarInt32>("ActiveCamp");
            }
            set
            {
                Fsm.SetData<VarInt32>("ActiveCamp", (int)value);
            }
        }
        public bool IsAutoBattle
        {
            get
            {
                if (m_ActiveCamp != CampType.Player || GameEntry.Battle.AutoBattle)
                {
                    return true;
                }
                return false;
            }
        }

        private Type m_NextState = null;

        protected override void OnInit(IFsm<ProcedureBattle> fsm)
        {
            base.OnInit(fsm);
            Fsm = fsm;
        }

        protected override void OnEnter(IFsm<ProcedureBattle> fsm)
        {
            base.OnEnter(fsm);

            Log.Info("Enter {0}", GetType());
        }

        protected override void OnUpdate(IFsm<ProcedureBattle> fsm, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

            if (m_NextState != null)
            {
                ChangeState(fsm, m_NextState);
            }
        }

        protected override void OnLeave(IFsm<ProcedureBattle> fsm, bool isShutdown)
        {
            m_NextState = null;

            base.OnLeave(fsm, isShutdown);
        }

        protected override void OnDestroy(IFsm<ProcedureBattle> fsm)
        {
            Fsm = null;
            base.OnDestroy(fsm);
        }

        /// <summary>
        /// 会在下一帧切换状态
        /// </summary>
        public void ChangeState<T>()
            where T : BattleStateBase
        {
            m_NextState = typeof(T);
        }
    }
}