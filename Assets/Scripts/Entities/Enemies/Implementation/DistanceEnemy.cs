using DeadLink.PowerUpSystem.InterfacePowerUps;
using RogueLike;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DeadLink.Entities.Enemies
{
    public class DistanceEnemy : Enemy
    {
        [Header("Patrol Behavior")]
        [SerializeField] protected float patrolSweepAngle = 90f;
        [SerializeField] protected float patrolRotationSpeed = 1.0f;
        
        private Quaternion initialLocalRotation;
        private Vector3 initialForward;

        protected override void Start()
        {
            base.Start();
            initialLocalRotation = transform.localRotation;
            initialForward = transform.forward;
        }
        
        protected override void Shoot()
        {
            if (CurrentWeapon != null && CurrentWeapon.CurrentReloadTime >= CurrentWeapon.WeaponData.ReloadTime)
            {
                GameObject objectToHit = null;
                Vector3 direction = player.transform.position - transform.position + Vector3.Scale(transform.forward, Random.onUnitSphere) * 3;
                Debug.DrawRay(BulletSpawnPoint.position, direction * 50, Color.red);

                if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 500, GameMetrics.Global.BulletRayCast))
                {
                    objectToHit = hit.collider.gameObject;
                }
                CurrentWeapon.Fire(this, direction, objectToHit);
            }
            else
            {
                //Debug.LogError($"No equipped weapon for {gameObject.name}");
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            
        }

        protected override void HandleOrientation()
        {
            if (HasVisionOnPlayer() && inAggroRange)
            {
                // Orient towards player (Horizontal rotation only, typical for turrets)
                Vector3 directionToPlayer = player.transform.position - transform.position;

                // Project the direction onto the turret's local horizontal plane
                // (the plane perpendicular to its own up vector)
                Vector3 localUp = transform.up;
                Vector3 projectedDirection = Vector3.ProjectOnPlane(directionToPlayer, localUp);

                if (projectedDirection.sqrMagnitude > 0.001f) // Check if direction is not zero (e.g. player is not directly above/below)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(projectedDirection, localUp);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, chaseRotationSpeed * Time.deltaTime);
                }
                // If player is directly above/below, turret might not rotate, or you can define a default behavior.
            }
            else
            {
                // Patrol behavior: Rotate left and right smoothly
                // Mathf.Sin will oscillate between -1 and 1.
                // We multiply by half of the sweep angle to get a range from -patrolSweepAngle/2 to +patrolSweepAngle/2.
                float sweepRange = patrolSweepAngle / 2.0f;
                float currentAngleOffset = Mathf.Sin(Time.time * patrolRotationSpeed) * sweepRange;

                // Apply this offset to the initial rotation around the local Y-axis (up)
                // This ensures the patrol is relative to where the turret was initially facing.
                Quaternion patrolRotation = initialLocalRotation * Quaternion.AngleAxis(currentAngleOffset, Vector3.up);
                transform.localRotation = patrolRotation;
            }
        }

        public override void Unlock(IVisitor visitor)
        {
        }

        public override void UsePowerUp(InputAction.CallbackContext context)
        {
        }
        
    }
}