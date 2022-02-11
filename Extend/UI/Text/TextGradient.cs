using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace lgu3d
{
    /// <summary>
    /// 文字上下渐变特效
    /// </summary>
    [AddComponentMenu("UI/Effects/Gradient")]
    [RequireComponent(typeof(Text))]
    public class TextGradient : BaseMeshEffect
    {
        [SerializeField]
        private Color32 topColor = Color.white;

        [SerializeField]
        private Color32 bottomColor = Color.black;

        private List<UIVertex> mVertexList;
        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
            {
                return;
            }

            if (mVertexList == null)
            {
                mVertexList = new List<UIVertex>();
            }

            vh.GetUIVertexStream(mVertexList);
            ApplyGradient(mVertexList);

            vh.Clear();
            vh.AddUIVertexTriangleStream(mVertexList);
        }

        private void ApplyGradient(List<UIVertex> vertexList)
        {
            for (int i = 0; i < vertexList.Count;)
            {
                ChangeColor(vertexList, i, topColor);
                ChangeColor(vertexList, i + 1, topColor);
                ChangeColor(vertexList, i + 2, bottomColor);
                ChangeColor(vertexList, i + 3, bottomColor);
                ChangeColor(vertexList, i + 4, bottomColor);
                ChangeColor(vertexList, i + 5, topColor);
                i += 6;
            }
        }

        private void ChangeColor(List<UIVertex> verList, int index, Color color)
        {
            UIVertex temp = verList[index];
            temp.color = color;
            verList[index] = temp;
        }
    }
}

