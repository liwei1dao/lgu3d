using System;

namespace lgu3d
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class LGSerializeAttribute : UnityEngine.PropertyAttribute
    {
        public bool IsWrite;
        public LGSerializeAttribute()
        {
            IsWrite = false;
        }
        public LGSerializeAttribute(bool _IsWrite)
        {
            IsWrite = _IsWrite;
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class LGSerializeNameAttribute : LGSerializeAttribute
    {
        public string Name;
        public LGSerializeNameAttribute(string _Name)
            : base()
        {
            Name = _Name;
        }
        public LGSerializeNameAttribute(string _Name, bool _IsWrite)
            : base(_IsWrite)
        {
            Name = _Name;
        }
    }
}
