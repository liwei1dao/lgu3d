using System;

namespace lgu3d
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class lgu3d_SerializeAttribute : UnityEngine.PropertyAttribute
    {
        public bool IsWrite;
        public lgu3d_SerializeAttribute()
        {
            IsWrite = false;
        }
        public lgu3d_SerializeAttribute(bool _IsWrite)
        {
            IsWrite = _IsWrite;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class lgu3d_SerializeNameAttribute : lgu3d_SerializeAttribute
    {
        public string Name;
        public lgu3d_SerializeNameAttribute(string _Name)
            : base()
        {
            Name = _Name;
        }
        public lgu3d_SerializeNameAttribute(string _Name, bool _IsWrite)
            : base(_IsWrite)
        {
            Name = _Name;
        }
    }
}
