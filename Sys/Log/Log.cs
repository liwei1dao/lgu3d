using System;

namespace LG
{
    public static class Log
    {
        public static void Debug(string log)
        {
            if (log.Contains("OnceWaitTimer")) return;
            if (log.Contains("GameObjectComponent")) return;
            if (log.Contains("EventComponent")) return;
            if (log.Contains("ChildrenComponent")) return;
            UnityEngine.Debug.Log(log);
        }

        public static void Error(string log)
        {
            UnityEngine.Debug.LogError(log);
        }
        public static void Error(Exception e)
        {
            UnityEngine.Debug.LogException(e);
        }
    }
}