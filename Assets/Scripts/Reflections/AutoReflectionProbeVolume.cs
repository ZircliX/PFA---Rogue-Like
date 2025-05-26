using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace DeadLink.Reflections
{
    [ExecuteInEditMode]
    public class AutoReflectionProbeVolume : MonoBehaviour
    {
        [Header("Volume Settings")]
        public Vector3 volumeSize = new Vector3(50, 10, 50);
        public Vector3 probeSpacing = new Vector3(10, 10, 10);

        [Header("Reflection Probe Settings")]
        public Vector3 probeBoxSize = new Vector3(10, 10, 10);
        public float blendDistance = 5f;
        public ReflectionProbeMode probeMode = ReflectionProbeMode.Baked;
        public int cubemapResolution = 128;
        public bool useBoxProjection = true;

        [Header("Placement Filter")]
        public LayerMask probePlacementLayerMask = ~0; // Tout par défaut
        public QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Ignore;
        public float centerCheckRadius = 0.25f;

        // Buffer réutilisé pour éviter les allocations
        private readonly Collider[] overlapBuffer = new Collider[32];

        [ContextMenu("Generate Reflection Probes")]
        public void GenerateProbes()
        {
            ClearExistingProbes();

            Vector3 startPos = transform.position - volumeSize * 0.5f;

            int countX = Mathf.CeilToInt(volumeSize.x / probeSpacing.x);
            int countY = Mathf.CeilToInt(volumeSize.y / probeSpacing.y);
            int countZ = Mathf.CeilToInt(volumeSize.z / probeSpacing.z);

            for (int x = 0; x < countX; x++)
            {
                for (int y = 0; y < countY; y++)
                {
                    for (int z = 0; z < countZ; z++)
                    {
                        Vector3 pos = startPos + new Vector3(x * probeSpacing.x, y * probeSpacing.y, z * probeSpacing.z);

                        // Vérifie s'il y a au moins un mesh dans la box
                        int hitCount = Physics.OverlapBoxNonAlloc(
                            pos,
                            probeBoxSize * 0.5f,
                            overlapBuffer,
                            Quaternion.identity,
                            probePlacementLayerMask,
                            triggerInteraction
                        );

                        if (hitCount == 0)
                            continue;

                        // Vérifie que le centre du probe n'est pas dans un mesh
                        if (Physics.CheckSphere(pos, centerCheckRadius, probePlacementLayerMask, triggerInteraction))
                            continue;

                        // Crée le probe
                        GameObject probeObj = new GameObject($"ReflectionProbe_{x}_{y}_{z}");
                        probeObj.transform.parent = this.transform;
                        probeObj.transform.position = pos;

                        var probe = probeObj.AddComponent<ReflectionProbe>();
                        probe.mode = probeMode;
                        probe.size = probeBoxSize;
                        probe.blendDistance = blendDistance;
                        probe.resolution = cubemapResolution;
                        probe.boxProjection = useBoxProjection;
                        probe.clearFlags = ReflectionProbeClearFlags.Skybox;
                        probe.cullingMask = -1;
                        probe.shadowDistance = 100;
                        probe.refreshMode = probeMode == ReflectionProbeMode.Realtime
                            ? ReflectionProbeRefreshMode.EveryFrame
                            : ReflectionProbeRefreshMode.ViaScripting;
                    }
                }
            }

#if UNITY_EDITOR
            if (probeMode == ReflectionProbeMode.Baked)
            {
                Lightmapping.BakeAsync();
            }
#endif
        }

        [ContextMenu("Clear Existing Probes")]
        public void ClearExistingProbes()
        {
#if UNITY_EDITOR
            foreach (Transform child in transform)
            {
                if (PrefabUtility.IsPartOfPrefabInstance(child.gameObject)) continue;
                if (child.GetComponent<ReflectionProbe>())
                    DestroyImmediate(child.gameObject);
            }
#endif
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0.2f, 0.6f, 1f, 0.2f);
            Gizmos.DrawCube(transform.position, volumeSize);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(AutoReflectionProbeVolume))]
    public class AutoReflectionProbeVolumeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            AutoReflectionProbeVolume script = (AutoReflectionProbeVolume)target;

            if (GUILayout.Button("Generate Reflection Probes"))
            {
                script.GenerateProbes();
            }

            if (GUILayout.Button("Clear Existing Probes"))
            {
                script.ClearExistingProbes();
            }
        }
    }
#endif
}
