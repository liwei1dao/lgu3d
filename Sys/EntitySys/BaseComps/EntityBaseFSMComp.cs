using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  public abstract class FSMBaseState<E, T, S> where E : EntityBase where T : struct where S : struct
  {
    protected E Entity;
    protected Dictionary<T, S> mFSMStateIdDic = new Dictionary<T, S>();
    public FSMBaseState(E entity)
    {
      Entity = entity;
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
    public abstract void OnEntry(params object[] agrs);
    public abstract void OnExecute(float time);
    public abstract void OnExit();
    public abstract void TransitionReason(float time);
  }


  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="E">角色</typeparam>
  /// <typeparam name="T"><转换枚举(保留0 为无效状态)></转换枚举></typeparam>
  /// <typeparam name="S"><状态(保留0 为无效状态)></typeparam>
  public abstract class EntityBaseFSMComp<E, T, S> : EntityCompBase<E> where E : EntityBase where T : struct where S : struct
  {
    public S mCurrentStateID;
    protected FSMBaseState<E, T, S> mCurrentState;
    protected Dictionary<S, FSMBaseState<E, T, S>> mFSMStateDic;

    public override void Load(E entity, params object[] agrs)
    {
      mFSMStateDic = new Dictionary<S, FSMBaseState<E, T, S>>();
      base.Load(entity, agrs);
    }

    protected void SetDefaultState(S state)
    {
      mCurrentStateID = state;
      mCurrentState = mFSMStateDic[mCurrentStateID];
    }


    public void AddFSMSate(S stateID, FSMBaseState<E, T, S> state)
    {
      if (mFSMStateDic.ContainsKey(stateID))
      {
        Debug.LogError("加入状态机 重复" + stateID.ToString());
        return;
      }
      mFSMStateDic.Add(stateID, state);
    }

    public void RemoveFSMSate(S stateID)
    {
      if (!mFSMStateDic.ContainsKey(stateID))
      {
        Debug.LogError("移除状态机 不存在" + stateID.ToString());
        return;
      }
      mFSMStateDic.Remove(stateID);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="transition">转换</param>
    /// <param name="agrs"></param>
    public virtual void TransitionFSMState(T transition, params object[] agrs)
    {
      S stateID = mCurrentState.GetStateIdByTransition(transition);
      if (!stateID.Equals(default(S)))
      {
        mCurrentStateID = stateID;
        if (mCurrentState != null)
          mCurrentState.OnExit();
        mCurrentState = mFSMStateDic[stateID];
        mCurrentState.OnEntry(agrs);
      }
    }

    //更新（执行）系统
    private void Updata()
    {
      if (mCurrentState != null)
      {
        mCurrentState.OnExecute(Time.deltaTime);
        mCurrentState.TransitionReason(Time.deltaTime);
      }
    }
  }
}
