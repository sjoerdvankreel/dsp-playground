namespace DspPlayground
{
    // https://www.dsprelated.com/freebooks/filters/Simplest_Lowpass_Filter_I.html
    internal class SimpleLowPassFilter : FilterBase
    {
        float _x1;

        internal override float Next(float freq, float res, float x)
        {
            float y = (x + _x1) / 2;
            _x1 = x;
            return x;
        }
    }
}