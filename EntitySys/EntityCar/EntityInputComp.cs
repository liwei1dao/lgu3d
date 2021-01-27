using UnityEngine;
using UnityEditor;

namespace EntitySys {
    public class EntityInputComp : MonoEntityCompBase<EntityCar>, IEntityCarInput
    {
        float m_Acceleration;
        float m_Steering;
        bool m_HopPressed;
        bool m_HopHeld;
        bool m_BoostPressed;
        bool m_FirePressed;

        public float Acceleration
        {
            get { return m_Acceleration; }
        }

        public bool HopPressed {
            get { return m_HopPressed; }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.W))
                m_Acceleration = 1f;
            else if (Input.GetKey(KeyCode.S))
                m_Acceleration = -1f;
            else
                m_Acceleration = 0f;
        }

        void FixedUpdate()
        {

        }
    }
}
