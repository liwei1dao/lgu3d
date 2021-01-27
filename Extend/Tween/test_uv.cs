using UnityEngine;
using System.Collections;

namespace lgu3d
{
    public class test_uv : MonoBehaviour
    {
        public float xspeed = 0;
        public float yspeed = 0;
        private Vector2 v2;
        void Start()
        {
            v2 = Vector2.zero;
            //AssetBundleManager.GetBundle("dataconfig", getdata);
        }
        void Update()
        {
            v2.x += Time.fixedDeltaTime * xspeed;
            v2.y += Time.fixedDeltaTime * yspeed;
            GetComponent<Renderer>().materials[0].mainTextureOffset = v2;
        }


    }
}
