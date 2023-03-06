using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace lgu3d
{
    ///
    public static class Vector2Extend
    {
        public static float Angle(Vector2 form, Vector2 to)
        {
            float x = to.x - form.x;
            float y = to.y - form.y;

            float hy = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2f));

            float cos = x / hy;
            float radian = Mathf.Acos(cos);

            float angle = 180 / (Mathf.PI / radian);
            if (y < 0) angle = 360 - angle;   // if (y < 0) angle = - angle;   //-180-180
            else if ((y == 0) && (x < 0)) angle = 180;

            return angle;
        }
    }
}