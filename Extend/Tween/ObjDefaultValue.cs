using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 最初设计好的默认的属性
    /// </summary>
    public class ObjDefaultValue : MonoBehaviour
    {
        public RectTransform rectTransform;
        public Vector3 defaultLocalPos;
        public Vector3 defaultLocalScale;
        public Quaternion defaultLocalRotate;
        public Vector3 defaultlocalEulerAngles;
        public Vector3 defaultAnchoredPosition3D;

        private Dictionary<string, object> propValues = new Dictionary<string, object>();
        private Dictionary<string, Object> propObjectValues = new Dictionary<string, Object>();

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            defaultLocalPos = transform.localPosition;
            defaultLocalScale = transform.localScale;
            defaultLocalRotate = transform.localRotation;
            defaultlocalEulerAngles = transform.localEulerAngles;
            defaultAnchoredPosition3D = rectTransform.anchoredPosition3D;
        }

        /// <summary>
        /// 添加system.object值类型的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddValue(string key, object value)
        {
            propValues.ForceAddValue(key, value);
        }

        /// <summary>
        /// 添加UnityEngine.Object组件类型的参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddObject(string key, Object value)
        {
            propValues.ForceAddValue(key, value);
        }
    }
}