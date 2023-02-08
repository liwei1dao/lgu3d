using UnityEngine;

namespace lgu3d
{
    public class LGAttributeRename : PropertyAttribute
    {
        public string PropertyName;
        public bool IsWrite;
        public LGAttributeRename(string name)
        {
            PropertyName = name;
            IsWrite = true;
        }

        public LGAttributeRename(string name, bool isWrite)
        {
            PropertyName = name;
            IsWrite = isWrite;
        }
    }

}
