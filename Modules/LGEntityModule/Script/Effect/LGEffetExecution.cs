using System;

namespace lgu3d
{
    /// <summary>
    /// 实体事件
    /// </summary>
    public abstract class EffetExecution<E> : IEffetExecution<E> where E : class, ILGEntity
    {

        /// <summary>
        /// 状态类型
        /// </summary>
        public LGEffect Effect { get; set; }
        /// <summary>
        /// 效果来源技能
        /// </summary>
        public ILGSkill SourceEffetSkill { get; set; }
        /// <summary>
        /// 效果来源实体
        /// </summary>
        public E SourceEffetEntity => SourceEffetSkill.Entity as E;
        /// <summary>
        /// 效果目标实体
        /// </summary>
        public E TargetEntity { get; set; }
        /// <summary>
        /// 执行函数
        /// </summary>
        /// <param name="entity"></param>
        public abstract void Execution();
        /// <summary>
        /// 效果执行前
        /// </summary>
        protected virtual void PreProcess()
        {

        }
        /// <summary>
        /// 效果执行后
        /// </summary>
        protected virtual void PostProcess()
        {

        }
        /// <summary>
        /// 效果执行结束
        /// </summary>
        protected virtual void FinishAction()
        {

        }

        public virtual void Clear()
        {
            Effect = null;
            SourceEffetSkill = null;
            TargetEntity = null;
        }
    }
}