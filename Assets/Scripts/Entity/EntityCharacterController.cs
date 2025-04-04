using System;
using UnityEngine;
using KinematicCharacterController;

namespace ProjectSecurity.Gameplay
{
    public class EntityCharacterController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor;

        [Header("Physics")]
        public float gravityScale = 25f;
        public float gravityLockoutTime = 0.2f;
        public float groundDrag = 0.5f;
        public float airDrag = 0.1f;

        protected Vector3 gravity;
        protected bool gravityLockout;
        protected float timeSinceGravityLockout = 0f;

        protected bool wasGroundedLast = false;
        protected bool hasHitGroundThisFrame = false;

        public Vector3 lastLookDirection;

        protected Vector3 overridingVelocity = Vector3.zero;
        protected bool overroteVelocity = false;
        protected Vector3 internalVelocityAdd = Vector3.zero;

        public Vector3 CharacterForward
        {
            get { return Motor.CharacterForward; }
        }

        public Vector3 CharacterRight
        {
            get { return Motor.CharacterRight; }
        }

        public Vector3 Velocity
        {
            get { return Motor.Velocity; }
        }

        public bool IsGrounded
        {
            get { return Motor.GroundingStatus.IsStableOnGround; }
        }

        public bool HasHitGroundThisFrame
        {
            get { return hasHitGroundThisFrame; }
        }

        private void Start()
        {
            Motor.CharacterController = this;

            gravity = new Vector3(0f, -gravityScale, 0f);
        }

        void ICharacterController.AfterCharacterUpdate(float deltaTime)
        {
            UpdateGravityLockout(deltaTime);
        }

        protected void UpdateGravityLockout(float deltaTime)
        {
            if (gravityLockout)
            {
                timeSinceGravityLockout += deltaTime;

                if (timeSinceGravityLockout >= gravityLockoutTime)
                {
                    gravityLockout = false;
                    timeSinceGravityLockout = 0f;
                }
            }
        }

        void ICharacterController.BeforeCharacterUpdate(float deltaTime)
        {
            ResetGroundingStatus();
        }

        protected void ResetGroundingStatus()
        {
            hasHitGroundThisFrame = false;

            wasGroundedLast = Motor.GroundingStatus.IsStableOnGround;
        }

        bool ICharacterController.IsColliderValidForCollisions(Collider coll)
        {
            if (Physics.GetIgnoreLayerCollision(gameObject.layer, coll.gameObject.layer))
                return false;
            return true;
        }

        void ICharacterController.OnDiscreteCollisionDetected(Collider hitCollider)
        {

        }

        void ICharacterController.OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            CheckHasHitGroundThisFrame();
        }

        protected void CheckHasHitGroundThisFrame()
        {
            if (!wasGroundedLast)
            {
                hasHitGroundThisFrame = true;
            }
        }

        void ICharacterController.OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {

        }

        void ICharacterController.PostGroundingUpdate(float deltaTime)
        {

        }

        void ICharacterController.ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {

        }

        void ICharacterController.UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            // Smoothly interpolate from current to the current forward direction
            // Vector3 smoothedForwardDirection = Vector3.Slerp(Motor.CharacterForward, lastMoveVector, 1 - Mathf.Exp(-orientationSharpness * deltaTime)).normalized;

            // Set the current rotation (which will be used by the KinematicCharacterMotor)
            currentRotation = Quaternion.LookRotation(lastLookDirection, Motor.CharacterUp);
        }

        void ICharacterController.UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (overridingVelocity.magnitude > 1 || overroteVelocity)
            {
                currentVelocity = overridingVelocity;
                overridingVelocity = Vector3.zero;

                if (overroteVelocity)
                {
                    overroteVelocity = false;
                    return;
                }
            }

            if(IsGrounded)
                currentVelocity *= (1f / (1f + (groundDrag * deltaTime)));
            else
                currentVelocity = HandleAirPhysics(currentVelocity, deltaTime);
        }

        protected Vector3 HandleAirPhysics(Vector3 currentVelocity, float deltaTime)
        {
            // Gravity
            if (!gravityLockout)
            {
                if (currentVelocity.y >= 0)
                    currentVelocity += gravity * deltaTime;
                else
                {
                    currentVelocity += (gravity + new Vector3(0f, currentVelocity.y, 0f)) * deltaTime;
                }
            }

            // Drag
            currentVelocity *= (1f / (1f + (airDrag * deltaTime)));

            return currentVelocity;
        }

        public void OverrideVelocity(Vector3 newVelocity, bool applyToCharacterForward)
        {
            if (applyToCharacterForward)
            {
                newVelocity = VectorUtility.OrientVectorHorizontal(newVelocity, lastLookDirection, CharacterRight);
            }

            if (newVelocity.y > 0f)
            {
                Motor.ForceUnground();
            }

            overridingVelocity = newVelocity;

            overroteVelocity = true;
        }

        public void OverrideVelocity(float magnitude)
        {
            overridingVelocity = lastLookDirection * magnitude;

            overroteVelocity = true;
        }

        public void OverrideHorizontalVelocity(Vector3 newVelocity, bool applyToCharacterForward)
        {
            if (applyToCharacterForward)
            {
                newVelocity = VectorUtility.OrientVectorHorizontal(newVelocity, lastLookDirection, CharacterRight);

                newVelocity.y = Velocity.y;
            }

            overridingVelocity = newVelocity;
        }

        public void OverrideHorizontalVelocity(float magnitude)
        {
            overridingVelocity = lastLookDirection * magnitude;
            overridingVelocity.y = Velocity.y;
        }

        public void OverrideRotation(Vector3 newForward)
        {
            lastLookDirection = newForward;
        }

        public void GravityLockout()
        {
            gravityLockout = true;
        }
    }
}
