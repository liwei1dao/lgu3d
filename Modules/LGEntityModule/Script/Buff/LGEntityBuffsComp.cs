using System;
using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 实体Buff管理
    /// </summary>
    public abstract class LGEntityBuffsComp<B> : LGEntityCompBase, ILGEntityBuffsComp<B> where B : Enum
    {
        /// <summary>
        /// Buff链表
        /// </summary>
        protected LinkedList<ILGBuff<B>> m_Buffs = new();

        /// <summary>
        /// 用于查找的——基于Buff生效方式
        /// </summary>
        protected Dictionary<B, ILGBuff<B>> m_BuffsForFind_BuffWorkType = new();

        protected LinkedListNode<ILGBuff<B>> m_Current, m_Next;

        /// <summary>
        /// 添加Buff到真实链表，禁止外部调用
        /// </summary>
        /// <param name="buff"></param>
        public virtual void AddBuff(ILGBuff<B> buff)
        {
            m_Buffs.AddLast(buff);

            this.m_BuffsForFind_BuffWorkType[buff.BuffType] = buff;
        }

        /// <summary>
        /// 移除Buff(下一帧才真正移除 TODO 考虑到有些Buff绕一圈下来可能会移除自己，需要做额外处理，暂时先放着)
        /// </summary>
        /// <param name="buffId">要移除的BuffId</param>
        public virtual void RemoveBuff(long buffId)
        {

        }

        #region 查询
        /// <summary>
        /// 通过作用方式查找Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        /// <returns></returns>
        public bool FindBuffByWorkType(B buffWorkTypes)
        {
            if (this.m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out ILGBuff<B> _temp))
            {
                return true;
            }

            return false;
        }

        #endregion

        #region 获取

        /// <summary>
        /// 通过作用方式获得Buff
        /// </summary>
        /// <param name="buffWorkTypes"></param>
        public ILGBuff<B> GetBuffByWorkType(B buffWorkTypes)
        {
            if (m_BuffsForFind_BuffWorkType.TryGetValue(buffWorkTypes, out ILGBuff<B> _temp))
            {
                return _temp;
            }

            //Log.Error($"查找{buffWorkTypes}失败");
            return null;
        }

        #endregion
    }
}