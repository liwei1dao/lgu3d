using UnityEngine;

namespace LG.EntitySys
{
    public class ComponentView : MonoBehaviour
    {
        public string Type;
        public object Component { get; set; }
    }
}