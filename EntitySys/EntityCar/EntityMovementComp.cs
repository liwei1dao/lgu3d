 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EntitySys {
    [RequireComponent(typeof(IEntityCarInput))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class EntityMovementComp : MonoEntityCompBase<EntityCar>
    {
        public Transform frontGroundRaycast;
        public Transform rightGroundRaycast;
        public Transform leftGroundRaycast;
        public Transform rearGroundRaycast;
        public LayerMask groundLayers;

        Rigidbody m_Rigidbody;
        CapsuleCollider m_Capsule;
        GroundInfo m_CurrentGroundInfo;
        RaycastHit[] m_RaycastHitBuffer = new RaycastHit[8];

        public UnityEvent OnBecomeAirborne;
        public UnityEvent OnBecomeGrounded;
        public UnityEvent OnHop;

        Vector3 m_Velocity;
        Vector3 m_RigidbodyPosition;
        Vector3 m_RepositionPositionDelta;
        Quaternion m_RepositionRotationDelta = Quaternion.identity;

        bool m_HasControl;
        bool m_IsGrounded;

        const float k_VelocityNormalAirborneDot = 0.5f;
        const float k_GroundToCapsuleOffsetDistance = 0.025f;

        protected  void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
        }


        void FixedUpdate()
        {
            if (Mathf.Approximately(Time.timeScale, 0f))
                return;
            if (m_RepositionPositionDelta.sqrMagnitude > float.Epsilon || m_RepositionRotationDelta != Quaternion.identity)
            {
                m_Rigidbody.MovePosition(m_Rigidbody.position + m_RepositionPositionDelta);
                m_Rigidbody.MoveRotation(m_RepositionRotationDelta * m_Rigidbody.rotation);
                m_RepositionPositionDelta = Vector3.zero;
                m_RepositionRotationDelta = Quaternion.identity;
                return;
            }
            m_RigidbodyPosition = m_Rigidbody.position;
            Quaternion rotationStream = m_Rigidbody.rotation;
            float deltaTime = Time.deltaTime;
            m_CurrentGroundInfo = CheckForGround(deltaTime, rotationStream, m_Velocity * deltaTime);
            Hop(rotationStream, m_CurrentGroundInfo);
            if (m_CurrentGroundInfo.isGrounded && !m_IsGrounded)
                OnBecomeGrounded.Invoke();
            if (!m_CurrentGroundInfo.isGrounded && m_IsGrounded)
                OnBecomeAirborne.Invoke();
            m_IsGrounded = m_CurrentGroundInfo.isGrounded;

        }

        /// <summary>
        /// 检测是否与地面接触
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <param name="rotationStream"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private GroundInfo CheckForGround(float deltaTime, Quaternion rotationStream, Vector3 offset) {
            GroundInfo groundInfo = new GroundInfo();
            Vector3 defaultPosition = offset + m_Velocity * deltaTime;
            Vector3 direction = rotationStream * Vector3.down;

            float capsuleRadius = m_Capsule.radius;
            float capsuleTouchingDistance = capsuleRadius + Physics.defaultContactOffset;
            float groundedDistance = capsuleTouchingDistance + k_GroundToCapsuleOffsetDistance;
            float closeToGroundDistance = Mathf.Max(groundedDistance + capsuleRadius, m_Velocity.y);

            int hitCount = 0;
            Ray ray = new Ray(defaultPosition + frontGroundRaycast.position, direction);
            bool didHitFront = GetNearestFromRaycast(ray, closeToGroundDistance, groundLayers, QueryTriggerInteraction.Ignore, out RaycastHit frontHit);
            if (didHitFront)
                hitCount++;

            ray.origin = defaultPosition + rightGroundRaycast.position;

            bool didHitRight = GetNearestFromRaycast(ray, closeToGroundDistance, groundLayers, QueryTriggerInteraction.Ignore, out RaycastHit rightHit);
            if (didHitRight)
                hitCount++;

            ray.origin = defaultPosition + leftGroundRaycast.position;

            bool didHitLeft = GetNearestFromRaycast(ray, closeToGroundDistance, groundLayers, QueryTriggerInteraction.Ignore, out RaycastHit leftHit);
            if (didHitLeft)
                hitCount++;

            ray.origin = defaultPosition + rearGroundRaycast.position;

            bool didHitRear = GetNearestFromRaycast(ray, closeToGroundDistance, groundLayers, QueryTriggerInteraction.Ignore, out RaycastHit rearHit);
            if (didHitRear)
                hitCount++;

            groundInfo.isCapsuleTouching = frontHit.distance <= capsuleTouchingDistance || rightHit.distance <= capsuleTouchingDistance || leftHit.distance <= capsuleTouchingDistance || rearHit.distance <= capsuleTouchingDistance;
            groundInfo.isGrounded = frontHit.distance <= groundedDistance || rightHit.distance <= groundedDistance || leftHit.distance <= groundedDistance || rearHit.distance <= groundedDistance;
            groundInfo.isCloseToGround = hitCount > 0;

            // No hits - normal = Vector3.up
            if (hitCount == 0)
            {
                groundInfo.normal = Vector3.up;
            }

            // 1 hit - normal = hit.normal
            else if (hitCount == 1)
            {
                if (didHitFront)
                    groundInfo.normal = frontHit.normal;
                else if (didHitRight)
                    groundInfo.normal = rightHit.normal;
                else if (didHitLeft)
                    groundInfo.normal = leftHit.normal;
                else if (didHitRear)
                    groundInfo.normal = rearHit.normal;
            }

            // 2 hits - normal = hits average
            else if (hitCount == 2)
            {
                groundInfo.normal = (frontHit.normal + rightHit.normal + leftHit.normal + rearHit.normal) * 0.5f;
            }

            // 3 hits - normal = normal of plane from 3 points
            else if (hitCount == 3)
            {
                if (!didHitFront)
                    groundInfo.normal = Vector3.Cross(rearHit.point - rightHit.point, leftHit.point - rightHit.point);

                if (!didHitRight)
                    groundInfo.normal = Vector3.Cross(rearHit.point - frontHit.point, leftHit.point - frontHit.point);

                if (!didHitLeft)
                    groundInfo.normal = Vector3.Cross(rightHit.point - frontHit.point, rearHit.point - frontHit.point);

                if (!didHitRear)
                    groundInfo.normal = Vector3.Cross(leftHit.point - rightHit.point, frontHit.point - rightHit.point);
            }

            // 4 hits - normal = average of normals from 4 planes
            else
            {
                Vector3 normal0 = Vector3.Cross(rearHit.point - rightHit.point, leftHit.point - rightHit.point);
                Vector3 normal1 = Vector3.Cross(rearHit.point - frontHit.point, leftHit.point - frontHit.point);
                Vector3 normal2 = Vector3.Cross(rightHit.point - frontHit.point, rearHit.point - frontHit.point);
                Vector3 normal3 = Vector3.Cross(leftHit.point - rightHit.point, frontHit.point - rightHit.point);

                groundInfo.normal = (normal0 + normal1 + normal2 + normal3) * 0.25f;
            }

            if (groundInfo.isGrounded)
            {
                float dot = Vector3.Dot(groundInfo.normal, m_Velocity.normalized);
                if (dot > k_VelocityNormalAirborneDot)
                {
                    groundInfo.isGrounded = false;
                }
            }

            return groundInfo;
        }

        /// <summary>
        /// Affects the velocity of the kart if it hops.
        /// </summary>
        void Hop(Quaternion rotationStream, GroundInfo currentGroundInfo)
        {
            if (currentGroundInfo.isGrounded && Entity.InputComp.HopPressed && m_HasControl)
            {
                m_Velocity += rotationStream * Vector3.up * Entity.AttributeComp.hopHeight;

                OnHop.Invoke();
            }
        }

        /// <summary>
        /// Gets information about the nearest object hit by a raycast.
        /// </summary>
        bool GetNearestFromRaycast(Ray ray, float rayDistance, int layerMask, QueryTriggerInteraction query, out RaycastHit hit)
        {
            int hits = Physics.RaycastNonAlloc(ray, m_RaycastHitBuffer, rayDistance, layerMask, query);

            hit = new RaycastHit();
            hit.distance = float.PositiveInfinity;

            bool hitSelf = false;
            for (int i = 0; i < hits; i++)
            {
                if (m_RaycastHitBuffer[i].collider == m_Capsule)
                {
                    hitSelf = true;
                    continue;
                }

                if (m_RaycastHitBuffer[i].distance < hit.distance)
                    hit = m_RaycastHitBuffer[i];
            }

            if (hitSelf)
                hits--;

            return hits > 0;
        }
    }
}
