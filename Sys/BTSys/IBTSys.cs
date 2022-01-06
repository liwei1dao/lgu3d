using System;

namespace lgu3d
{
    public enum BTNodeState
    {
       /// <summary>
        /// 无效的行为
        /// </summary>
        Invalid,

        /// <summary>
        /// 成功
        /// </summary>
        Susccess,

        /// <summary>
        /// 运行中
        /// </summary>
        Running,

        /// <summary>
        /// 执行返回失败
        /// </summary>
        Failure,

        /// <summary>
        /// 中断
        /// </summary>
        Aborted
    }

    /// <summary>
    /// 将所有节点组合起来的地方
    /// </summary>
    public interface IBTTree{
        IBlackboard Blackboard { get; set; }
        IBTNode Root { get; set; }
        void Tick();
    }

    /// <summary>
    /// 个存放共享数据的地方，可以看成是一个Key－Value的字典
    /// </summary>
    public interface IBlackboard{
        T GetData<T>(string key);
    }

    /// <summary>
    /// 基础节点
    /// </summary>
    public interface IBTNode {
        IBTTree BTTree{ get; set; }
        bool DoEvaluate();
        void Tick();
    }

    /// <summary>
    /// 条件判断节点
    /// </summary>
    public interface IConditionNode:IBTNode {
        
    }

    /// <summary>
    /// 行为节点，继承于BTNode。具体的游戏逻辑应该放在这个节点里面。
    /// </summary>
    public interface IActionNode:IBTNode {

    }

    /// <summary>
    /// 判断执行 Priority Selector逻辑节点，每次执行，先有序地遍历子节点，然后执行符合准入条件的第一个子结点。可以看作是根据条件来选择一个子结点的选择器。
    /// </summary>
    public interface IPrioritySelectorNode :IBTNode{

    }

    /// <summary>
    /// 顺序执行  Sequence逻辑节点，每次执行，有序地执行各个子结点，当一个子结点结束后才执行下一个。严格按照节点A、B、C的顺序执行，当最后的行为C结束后，BTSequence结束。
    /// </summary>
    public interface ISequence :IBTNode{

    }

    /// <summary>
    /// And条件逻辑 Parallel逻辑节点 同时执行各个子结点。每当任一子结点的准入条件失败，它就不会执行。
    /// </summary>
    public interface IParallelP :IBTNode{

    }

    /// <summary>
    /// Or条件逻辑 Parallel的一个变异，同时执行各个子节点。当所有子结点的准入条件都失败，它就不会执行。
    /// </summary>
    public interface IParallelFlexible :IBTNode{

    }


}