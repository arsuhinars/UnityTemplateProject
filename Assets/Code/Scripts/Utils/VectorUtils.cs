using UnityEngine;

namespace Game.Utils
{
    public static class VectorUtils
    {
        public static bool IsZero(this Vector2 v)
        {
            return Mathf.Approximately(v.x, 0f) && Mathf.Approximately(v.y, 0f);
        }

        public static bool IsZero(this Vector3 v)
        {
            return Mathf.Approximately(v.x, 0f) && Mathf.Approximately(v.y, 0f) && Mathf.Approximately(v.z, 0f);
        }
    }
}
