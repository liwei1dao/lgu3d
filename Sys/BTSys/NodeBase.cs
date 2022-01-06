using System;

namespace lgu3d
{
    public abstract class NodeBase : IBTNode
    {
        public IBTTree BTTree{ get; set; }

        public virtual bool DoEvaluate()
        {
            return true;
        }

        public void Tick()
        {
            
        }
    }
}