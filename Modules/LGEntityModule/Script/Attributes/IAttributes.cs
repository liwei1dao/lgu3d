using System;

namespace lgu3d
{

    public enum ModifierType : int
    {
        /// <summary>
        /// 加
        /// </summary>
        Add,
        /// <summary>
        /// 百分比加成
        /// </summary>
        PctAdd,
        FinalAdd,
        FinalPctAdd,
    }
    /// <summary>
    /// 属性管理
    /// </summary>
    public interface ILGEntityAttributesComp<A> : ILGEntityComponent
    {
        void OnNumericUpdate(FloatNumeric numeric);
    }
}