using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using KBCore.Refs;
using UnityEngine;

namespace DeadLink.Menus.Other
{
    public class CrosshairOffset : MonoBehaviour
    {
        [field: SerializeField] public Vector2 MinOffset { get; private set; }
        [field: SerializeField] public Vector2 MaxOffset { get; private set; }
        [field: SerializeField] public float OffsetTime { get; private set; }
        [SerializeField, Self] private RectTransform rt;

        private void OnValidate() => this.ValidateRefs();

        public void FireOffset()
        {
            DoSizeDelta(MaxOffset).OnComplete(() => DoSizeDelta(MinOffset));
        }

        private TweenerCore<Vector2,Vector2,VectorOptions> DoSizeDelta(Vector2 offset)
        {
            return DOTween.To(
                () => rt.sizeDelta,
                value => rt.sizeDelta = value,
                offset,
                OffsetTime * 0.5f
            );
        }
    }
}