using LTX.Singletons;
using UnityEngine;

namespace DeadLink.Menus
{
    public class UiTimeScaleManager : MonoBehaviour
    {
        [field: SerializeField] public Material[] Material { get; private set; }

        private void Update()
        {
            if (Material != null)
            {
                foreach (Material mat in Material)
                {
                    mat.SetFloat("_TimeUnscaled", Time.unscaledTime);
                    mat.SetFloat("_GlitchSpeed", Time.unscaledTime);
                }
            }
        }
    }
}