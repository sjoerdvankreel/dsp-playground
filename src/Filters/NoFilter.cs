namespace DspPlayground
{
    internal class NoFilter : FilterBase
    {
        internal override float Next(float freq, float res, float x) => x;
    }
}