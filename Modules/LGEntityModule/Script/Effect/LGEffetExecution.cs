using System;

namespace lgu3d
{
    /// <summary>
    /// 实体事件
    /// </summary>
    public abstract class EffetExecution<E> : IEffetExecution<E>, IReference where E : ILGEntity
    {

        /// <summary>
        /// 状态类型
        /// </summary>
        protected LGEffect Effect { get; set; }
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Execution(E entity);
        /// <summary>
        /// 效果执行前
        /// </summary>
        protected virtual void PreProcess(E entity)
        {

        }
        /// <summary>
        /// 效果执行后
        /// </summary>
        protected virtual void PostProcess(E entity)
        {

        }
        /// <summary>
        /// 效果执行结束
        /// </summary>
        protected virtual void FinishAction(E entity)
        {

        }

        public virtual void Clear()
        {
            Effect = null;
        }
    }
}