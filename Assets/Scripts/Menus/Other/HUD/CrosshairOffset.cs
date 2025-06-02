using DG.Tweening;
using KBCore.Refs;
using UnityEngine;

namespace DeadLink.Menus.Other
{
    public class CrosshairOffset : MonoBehaviour
    {
        [field: SerializeField] public Vector2 MinOffset { get; private set; }
        [field: SerializeField] public Vector2 MaxOffset { get; private set; }
        [field: SerializeField] public float OffsetTime { get; private set; }
        [SerializeField, Self] protected RectTransform rt;

        private void OnValidate() => this.ValidateRefs();

        public virtual void FireOffset()
        {
            DoSizeDelta(MaxOffset);
        }

        protected virtual void DoSizeDelta(Vector2 offset)
        {
            DOTween.To(
                () => rt.sizeDelta,
                value => rt.sizeDelta = value,
                offset,
                OffsetTime * 0.5f
            ).OnComplete(() => DoSizeDelta(MinOffset));
        }
    }
}