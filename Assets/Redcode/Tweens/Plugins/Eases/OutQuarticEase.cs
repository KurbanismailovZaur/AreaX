namespace Redcode.Tweens.Eases
{
    public sealed class OutQuarticEase : Ease
    {
        public override float Remap(float value)
        {
            var f = (value - 1f);
            return f * f * f * (1f - value) + 1f;
        }
    }
}