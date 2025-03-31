using UnityEngine;

namespace RogueLike.Controllers
{
    public static class CameraController
    {
        private static Camera cam;
        public static Camera MainCamera
        {
            get
            {
                if (!cam)
                    cam = Camera.main;
                return cam;
            }
        }
    }
}