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

            // Vadim:
            // In the sence of the analog prototype it's better to put the gain before the integrator,
            // because then the integrator will smooth the jumps and further artifacts arising
            // out of the cutoff modulation
            
            // Well, that's another thing I *actually* understand.
            // Who knows i might have a shot at this stuff in a couple years.
            _integrate += x * whatIsThis;
            _s++;
            return _integrate / _s;
        }
    }
}