namespace DspPlayground
{
    internal abstract class FilterBase
    {
        internal abstract float Next(float freq, float res, float x);
    }
}