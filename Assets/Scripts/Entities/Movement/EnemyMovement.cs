using UnityEngine;

namespace DeadLink.Entities.Movement
{
    public class EnemyMovement : EntityMovement
    {
        protected override void Update()
        {
            base.Update();
            InputDirection = new Vector3(Random.value, 0, Random.value);
        }
    }
}