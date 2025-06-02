using UnityEngine;

namespace DeadLink.Menus.Other
{
    public class LaserCrosshairOffset : CrosshairOffset
    {
        [SerializeField] private float maxSpeed;
        [SerializeField] private float speedMultiplier;
        [SerializeField] private float speedValue;

        private float currentSpeed;

        public override void FireOffset()
        {
            if (currentSpeed < maxSpeed)
            {
                currentSpeed += speedValue * speedMultiplier + currentSpeed * Time.deltaTime;
            }

            DoSizeDelta(Vector2.zero);
        }

        protected override void DoSizeDelta(Vector2 offset)
        {
            Vector3 targetRotation = new Vector3(0, 0, currentSpeed);
            if (currentSpeed > 0)
            {
                currentSpeed -= speedValue;
            }
            
            rt.rotation = Quaternion.Euler(targetRotation);
        }

        private void Update()
        {
            if (currentSpeed > 0)
            {
                currentSpeed -= speedValue * Time.deltaTime;
                Vector3 targetRotation = new Vector3(0, 0, currentSpeed);
                rt.rotation = Quaternion.Euler(targetRotation);
            }
        }
    }
}