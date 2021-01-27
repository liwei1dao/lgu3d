using UnityEngine;

namespace lgu3d
{
    public class MFWAttributeRename : PropertyAttribute
    {
        public string PropertyName;
        public MFWAttributeRename(string name)
        {
            PropertyName = name;
        }
    }

}
