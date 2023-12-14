using System;
using LG;

namespace lgu3d
{

    /// <summary>
    /// Buff对象
    /// </summary>
    public interface ILGBuff<B> : IReference where B : Enum
    {
        B BuffType { get; }
        void Excute(uint currentFrame);
        void Update(uint currentFrame);
        void Finished(uint currentFrame);
        void Refresh(uint currentFrame);
    }
    /// <summary>
    /// Buff管理
    /// </summary>
    public interface ILGEntityBuffsComp<B> : ILGEntityComponent where B : Enum
    {

    }
}