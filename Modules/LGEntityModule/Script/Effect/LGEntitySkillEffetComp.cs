using System;
using System.Collections.Generic;

namespace lgu3d
{
    /// <summary>
    /// 效果执行组件
    /// </summary>
    public abstract class LGEntityEffetExecutionComp<E> : LGEntityCompBase<E>, ILGEntityEffetExecutionComp where E : class, ILGEntity
    {
        public Queue<IEffetExecution<E>> ExecutionEffects { get; private set; } = new Queue<IEffetExecution<E>>();

        public virtual void AddEffect(ILGSkill source, LGEffect effect)
        {
            List<IEffetExecution<E>> effetExecutions = CreateExecution(source, effect);
            foreach (var item in effetExecutions)
            {
                ExecutionEffects.Enqueue(item);
            }
        }

        /// <summary>
        /// 一个技能效果可能需要多个执行单元
        /// </summary>
        /// <param name="effect"></param>
        /// <returns></returns>
        public abstract List<IEffetExecution<E>> CreateExecution(ILGSkill source, LGEffect effect);


        public override void LGUpdate(float time)
        {
            if (ExecutionEffects.Count > 0)
            {
                IEffetExecution<E> effetExecution = ExecutionEffects.Dequeue();
                effetExecution.Execution();
                ReferencePool.Release(effetExecution);
            }
        }

    }
}