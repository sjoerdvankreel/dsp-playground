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

            // More stuff from the book:
            // Actually, the number of
            // poles is always equal to the order of the filter or (which is the same) to the
            // number of integrators in the filter. Therefore it is common, instead of e.g.a
            // 4th - order filter to say a 4-pole filter

            // Well, that's another thing I *actually* understand.
            // Who knows i might have a shot at this stuff in a couple years.
            _integrate += x * whatIsThis;
            _s++;
            return _integrate / _s;

            // TODO add multimode LP/HP here
            // seems doable
        }
    }
}