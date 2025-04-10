using DeadLink.PowerUp.Components;
using UnityEngine;

namespace DeadLink.PowerUp
{
    public class PickUpPowerUp : MonoBehaviour
    {
        [SerializeField] private PowerUp powerUp;
        [SerializeField] private VisitableComponent visitableComponent;
        
        public void PickUp()
        {
            Debug.Log("Click jump");
            visitableComponent.Accept(powerUp);
        }
    }
}