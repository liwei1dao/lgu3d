using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// 实体基础对象
/// 申明 角色模块只是一个容器 切记切记
/// </summary>
namespace lgu3d
{

    /// <summary>
    /// 子弹对象
    /// </summary>
    public interface ILGBullet : ILGEntity
    {

    }

    /// <summary>
    /// 子弹实体
    /// </summary>
    public abstract class LGBullet : LGEntityBase, ILGBullet
    {

    }

}
