using System;

namespace DspPlayground
{
    internal class Saw
    {
        float _phase = 0;
        readonly float _sampleRate;
        internal Saw(float sampleRate) => _sampleRate = sampleRate;

        internal float Next(float frequency)
        {
            float increment = frequency / _sampleRate;
            float result = _phase * 2 - 1 - Blep(increment);
            _phase += increment;
            _phase -= MathF.Floor(_phase);
            return result;
        }

        float Blep(float increment)
        {
            if (_phase < increment)
            {
                float blep = _phase / increment;
                return (2.0f - blep) * blep - 1.0f;
            }
            if (_phase >= 1.0f - increment)
            {
                float blep = (_phase - 1.0f) / increment;
                return (blep + 2.0f) * blep + 1.0f;
            }
            return 0.0f;
        }
    }
}