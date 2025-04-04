using System;
using UnityEngine;
using KinematicCharacterController;

namespace ProjectSecurity.Gameplay
{
    public class PlayerCharacterController : EntityCharacterController, ICharacterController
    {
        private InputBank inputBank;

        [Header("Movement")]
        public float maxMoveSpeed = 10f;
        public float movementSharpness = 15f;
        public float maxAirMoveSpeed = 15f;
        public float airMovementSharpness = 10f;
        public float airAccelerationSpeed = 15f;
        public float orientationSharpness = 10f;

        [Header("Jumping")]
        public float jumpUpSpeed = 10f;
        public float jumpMinimumTime = 0.1f;
        public float jumpPreGroundingGraceTime = 0f;
        public float jumpPostGroundingGraceTime = 0f;
        public bool allowJumpingWhenSliding = false;

        private bool inputsGathering = true;
        private Vector3 moveVector;
        private bool tryJump;
        private float timeSinceLastAbleToJump = 0f;

        public bool hasJumpedThisFrame = false;

        public static Action OnLand;

        private void Start()
        {
            Motor.CharacterController = this;

            inputBank = GetComponent<InputBank>();

            gravity = new Vector3(0f, -gravityScale, 0f);
        }

        private void Update()
        {
            GetInputs();
        }

        public void GetInputs()
        {
            if (inputsGathering)
            {
                moveVector = inputBank.CameraMoveInput;

                tryJump = true;
            }
            else
            {
                moveVector = Vector3.zero;
            }
        }

        public void EnableInputs()
        {
            inputsGathering = true;
        }

        public void DisableInputs()
        {
            inputsGathering = false;
        }

        void ICharacterController.AfterCharacterUpdate(float deltaTime)
        {
            tryJump = false;

            moveVector = Vector3.zero;

            if (allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
            {
                timeSinceLastAbleToJump = 0f;
            }
            else
            {
                // Keep track of time since we were last able to jump (for grace period)
                timeSinceLastAbleToJump += deltaTime;
            }

            if(gravityLockout)
            {
                timeSinceGravityLockout += deltaTime;

                if(timeSinceGravityLockout >= gravityLockoutTime)
                {
                    gravityLockout = false;
                    timeSinceGravityLockout = 0f;
                }
            }
        }

        void ICharacterController.BeforeCharacterUpdate(float deltaTime)
        {
            hasJumpedThisFrame = false;
            hasHitGroundThisFrame = false;

            wasGroundedLast = Motor.GroundingStatus.IsStableOnGround;
        }

        bool ICharacterController.IsColliderValidForCollisions(Collider coll)
        {
            Debug.Log(Physics.GetIgnoreLayerCollision(gameObject.layer, coll.gameObject.layer));

            if (Physics.GetIgnoreLayerCollision(gameObject.layer, coll.gameObject.layer))
                return false;
            return true;
        }

        void ICharacterController.OnDiscreteCollisionDetected(Collider hitCollider)
        {

        }

        void ICharacterController.OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            if (!wasGroundedLast)
            {
                OnLand?.Invoke();
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
            if (moveVector.magnitude != 0f)
                lastLookDirection = moveVector;

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

                if(overroteVelocity)
                {
                    overroteVelocity = false;
                    return;
                }
            }

            Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

            // Ground movement
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                if(inputsGathering)
                {
                    Vector3 targetMovementVelocity = moveVector * maxMoveSpeed;

                    // Smooth movement Velocity
                    currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-movementSharpness * deltaTime));
                }
                else
                {
                    currentVelocity *= (1f / (1f + (groundDrag * deltaTime)));
                }
            }
            else
            {
                // Add move input
                if (moveVector.sqrMagnitude > 0f)
                {
                    Vector3 addedVelocity = moveVector * airAccelerationSpeed * deltaTime;

                    currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < maxAirMoveSpeed)
                    {
                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity, maxAirMoveSpeed);
                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    }
                    else
                    {
                        // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                        {
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                        }
                    }

                    // Prevent air-climbing sloped walls
                    if (Motor.GroundingStatus.FoundAnyGround)
                    {
                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                        {
                            Vector3 perpenticularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                        }
                    }

                    // Apply added velocity
                    currentVelocity += addedVelocity;
                }
                // Return to neutral on no input
                else
                {
                    currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                    Vector3 decreasedVelocity = Vector3.Lerp(currentVelocityOnInputsPlane, Vector3.zero, 1f - Mathf.Exp(-airMovementSharpness * deltaTime));

                    currentVelocity = new Vector3(decreasedVelocity.x, currentVelocity.y, decreasedVelocity.z);
                }

                // Gravity
                if(!gravityLockout)
                {
                    if(currentVelocity.y >= 0)
                        currentVelocity += gravity * deltaTime;
                    else
                    {
                        currentVelocity += (gravity + new Vector3(0f, currentVelocity.y, 0f)) * deltaTime;
                    }
                }

                // Drag
                currentVelocity *= (1f / (1f + (airDrag * deltaTime)));
            }

            if (!tryJump) return;

            if (inputBank.LastButtonInput == ButtonInput.Jump)
            {
                // See if we actually are allowed to jump
                if (((allowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || timeSinceLastAbleToJump <= jumpPostGroundingGraceTime))
                {
                    hasJumpedThisFrame = true;

                    // Calculate jump direction before ungrounding
                    Vector3 jumpDirection = Motor.CharacterUp;
                    if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                    {
                        jumpDirection = Motor.GroundingStatus.GroundNormal;
                    }

                    // Makes the character skip ground probing/snapping on its next update. 
                    // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                    Motor.ForceUnground();

                    // Add to the return velocity and reset jump state
                    currentVelocity += (jumpDirection * jumpUpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);

                    // currentVelocity += (moveInputVector * JumpScalableForwardSpeed);

                    inputBank.ConsumeLastButtonInput();

                    // Clamp velocity to max air speed
                    if (currentVelocityOnInputsPlane.magnitude > maxAirMoveSpeed)
                    {
                        Vector3 clampedCurrentVelocity = Vector3.ClampMagnitude(currentVelocityOnInputsPlane, maxAirMoveSpeed);
                        clampedCurrentVelocity.y = currentVelocity.y;

                        currentVelocity = clampedCurrentVelocity;

                        currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);
                    }
                }
            }
            else if (!inputBank.JumpHeld)
            {
                // See are in the air after a jump with an upward velocity after a short time
                if (!Motor.GroundingStatus.FoundAnyGround && currentVelocity.y > 0f && timeSinceLastAbleToJump >= jumpMinimumTime)
                {
                    // Resets upward velocity after jump release
                    currentVelocity.y = 0f;
                }
            }
        }

        public void DisableEnemyCollision()
        {
            Physics.IgnoreLayerCollision(6, 7);
        }

        public void EnableEnemyCollision()
        {
            Physics.IgnoreLayerCollision(6, 7, false);
        }
    }
}
