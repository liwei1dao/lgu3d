namespace lgu3d
{
    /// <summary>
    /// LG实体组件
    /// </summary>
    public interface ILGEntityComponent
    {
        /// <summary>
        /// 状态类型
        /// </summary>
        ILGEntity Entity { get; }
        void LGInit(ILGEntity entity);
        void LGStart();
        void LGUpdate(float time);
        void Activation();
    }
}