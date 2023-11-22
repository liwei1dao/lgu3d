using UnityEngine;

namespace lgu3d
{

    public static class RectTransformExtend
    {
        /// <summary>
        /// 通过世界坐标设置UI的位置
        /// </summary>
        /// <param name="rectTransform"></param>
        /// <param name="worldPos"></param>
        public static void WorldPosToUIPos(this RectTransform rectTransform, Vector3 worldPos, Vector3 offset)
        {
            if (rectTransform == null)
                return;

            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            // Z小于0，代表在相机后面，此时X、Y反向；
            if (screenPos.z < 0)
            {
                screenPos.x *= -1;
                screenPos.y *= -1;
            }
            screenPos += offset;
            rectTransform.position = new Vector3(screenPos.x, screenPos.y, 0);
        }
    }
}