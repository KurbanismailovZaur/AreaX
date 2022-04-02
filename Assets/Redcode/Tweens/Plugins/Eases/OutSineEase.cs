using UnityEngine;

namespace Redcode.Tweens.Eases
{
    public sealed class OutSineEase : Ease
    {
        public override float Remap(float value) => Mathf.Sin(value * (Mathf.PI / 2f));
    }
}