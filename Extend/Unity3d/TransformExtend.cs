using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace lgu3d
{

    public static class TransformExtend
    {
        /// <summary>
        /// 查找当前物体的子物体包含的所有材质球
        /// </summary>
        /// <param name="parent">需要查找的物体</param>
        /// <returns>返回获取到的所有材质球</returns>
        public static List<Material> FindChildMaterials(this Transform parent)
        {
            List<Material> t = new List<Material>();
            parent.Find(ref t);
            return t;
        }
        private static void Find(this Transform parent, ref List<Material> list)
        {
            if (parent.GetComponent<MeshRenderer>())
            {
                Material[] m = parent.GetComponent<MeshRenderer>().materials;
                if (m != null && m.Length > 0)
                {
                    foreach (var item in m)
                    {
                        list.Add(item);
                    }
                }
            }
            if (parent.GetComponent<SkinnedMeshRenderer>())
            {
                Material[] m = parent.GetComponent<SkinnedMeshRenderer>().materials;
                if (m != null && m.Length > 0)
                {
                    foreach (var item in m)
                    {
                        list.Add(item);
                    }
                }
            }
            int number = parent.childCount;
            while (number > 0)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    if (parent.GetChild(i).childCount > 0) parent.GetChild(i).Find(ref list);
                    number--;
                    if (parent.GetChild(i).GetComponent<MeshRenderer>())
                    {
                        Material[] m = parent.GetChild(i).GetComponent<MeshRenderer>().materials;
                        if (m != null && m.Length > 0)
                        {
                            foreach (var item in m)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    if (parent.GetChild(i).GetComponent<SkinnedMeshRenderer>())
                    {
                        Material[] m = parent.GetChild(i).GetComponent<SkinnedMeshRenderer>().materials;
                        if (m != null && m.Length > 0)
                        {
                            foreach (var item in m)
                            {
                                list.Add(item);
                            }
                        }
                    }
                }
            }
        }
    }
}