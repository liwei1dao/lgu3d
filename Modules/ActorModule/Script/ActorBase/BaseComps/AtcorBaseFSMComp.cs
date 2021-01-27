using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    public abstract class FSMBaseState<A , T, S> where T :  struct where S: struct
    {
        protected A Actor;
        protected Dictionary<T, S> mFSMStateIdDic = new Dictionary<T, S>();
        public FSMBaseState(A _Actor)
        {
            Actor = _Actor;
        }
        public void AddTransition(T transition, S stateID)
        {
            if (mFSMStateIdDic.ContainsKey(transition))
            {
                return;
            }
            mFSMStateIdDic.Add(transition, stateID);
        }

        public void RemoveTransition(T transition)
        {
            if (!mFSMStateIdDic.ContainsKey(transition))
            {
                return;
            }
            mFSMStateIdDic.Remove(transition);
        }

        public S GetStateIdByTransition(T transition)
        {
            if (!mFSMStateIdDic.ContainsKey(transition))
            {
                return default(S);
            }
            return mFSMStateIdDic[transition];
        }
        public abstract void OnEntry(params object[] _Agr);
        public abstract void OnExecute(float time);
        public abstract void OnExit();
        public abstract void TransitionReason(float time);

    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="A">角色</typeparam>
    /// <typeparam name="T"><转换枚举(保留0 为无效状态)></转换枚举></typeparam>
    /// <typeparam name="S"><状态(保留0 为无效状态)></typeparam>
    public abstract class AtcorBaseFSMComp<A,T,S> : ActorCompBase<A> where A: ActorBase where T : struct where S : struct
    {
        public S mCurrentStateID;
        protected FSMBaseState<A,T,S> mCurrentState;
        protected Dictionary<S, FSMBaseState<A,T,S>> mFSMStateDic;

        public override void Load(ActorBase _Actor, params object[] _Agr)
        {
            mFSMStateDic = new Dictionary<S, FSMBaseState<A, T, S>>();
            base.Load(_Actor, _Agr);
        }

        protected void SetDefaultState(S _State)
        {
            mCurrentStateID = _State;
            mCurrentState = mFSMStateDic[mCurrentStateID];
        }


        public void AddFSMSate(S StateID, FSMBaseState<A,T,S> State)
        {
            if (mFSMStateDic.ContainsKey(StateID))
            {
                Debug.LogError("加入状态机 重复"+ StateID.ToString());
                return;
            }
            mFSMStateDic.Add(StateID, State);
        }

        public void RemoveFSMSate(S StateID)
        {

            if (!mFSMStateDic.ContainsKey(StateID))
            {
                Debug.LogError("移除状态机 不存在"+ StateID.ToString());
                return;
            }
            mFSMStateDic.Remove(StateID);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="_Agr"></param>
        public virtual void TransitionFSMState(T transition, params object[] _Agr)
        {
            
            S stateID = mCurrentState.GetStateIdByTransition(transition);
            if (!stateID.Equals(default(S)))
            {
                mCurrentStateID = stateID;
                if(mCurrentState != null)
                    mCurrentState.OnExit();
                mCurrentState = mFSMStateDic[stateID];
                mCurrentState.OnEntry(_Agr);
            }
        }

        //更新（执行）系统
        public override void Updata(float time)
        {
            if (mCurrentState != null)
            {
                mCurrentState.OnExecute(time);
                mCurrentState.TransitionReason(time);
            }
        }

    }
}
