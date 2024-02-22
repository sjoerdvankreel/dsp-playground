namespace DspPlayground
{
    internal class TestFilter : FilterBase
    {
        long _s = 1;
        float _integrate = 0;

        internal override float Next(float freq, float res, float x)
        {
            // higher values of this correspond to increased LPF cutoff
            float whatIsThis = 500;
            _integrate += x * whatIsThis;
            _s++;
            return _integrate / _s;
        }
    }
}