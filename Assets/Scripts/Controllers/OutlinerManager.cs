using System.Collections.Generic;
using LTX.Singletons;
using UnityEngine;

namespace RogueLike.Controllers
{
    public class OutlinerManager : MonoSingleton<OutlinerManager>
    {
        private List<GameObject> outlinedObjects;

        protected override void Awake()
        {
            base.Awake();
            outlinedObjects = new List<GameObject>();
        }
        
        public void AddOutline(GameObject obj)
        {
            if (!outlinedObjects.Contains(obj))
            {
                outlinedObjects.Add(obj);
                // Add outline to the object
            }
        }
        
        public void RemoveOutline(GameObject obj)
        {
            if (outlinedObjects.Contains(obj))
            {
                outlinedObjects.Remove(obj);
                // Remove outline from the object
            }
        }
    }
}