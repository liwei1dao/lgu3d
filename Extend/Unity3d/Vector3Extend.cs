using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace lgu3d
{
    ///
    public static class Vector3Extend
    {
        public static float Angle(Vector3 from, Vector3 to)
        {
            float angle = Vector3.Angle(from, to);
            Vector3 nordir = Vector3.Cross(from, to);
            float dot = Vector3.Dot(nordir, Vector3.down);
            if (dot < 0)
            {
                angle *= -1;
                angle += 360;
            }
            return angle;
        }
    }
}