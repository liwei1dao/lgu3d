using System;

namespace lgu3d
{
    public abstract class BTActionBase : IBTNode
    {
        public IBlackboard Blackboard { get; set ; }

        public virtual bool DoEvaluate()
        {
            return true;
        }

        public virtual void Tick(){
            
        }
    }
}