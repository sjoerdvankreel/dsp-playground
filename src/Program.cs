using System;
using System.Threading;
using Xt;

namespace DspPlayground
{
    internal class Program
    {
        const float SawPitchMIDIEnd = 127;
        const float SawPitchMIDIStart = 17;
        const int SawSweepTimeSeconds = 10;

        const int Channels = 2;
        const int SampleRate = 48000;
        const int BufferSizeMillis = 10;
        const XtSample Format = XtSample.Float32;
        const XtSetup Setup = XtSetup.ConsumerAudio;

        [STAThread]
        static void Main(string[] args)
        {
            using var xtAudio = XtAudio.Init(nameof(DspPlayground), 0);
            var setup = xtAudio.SetupToSystem(XtSetup.SystemAudio);
            var service = xtAudio.GetService(setup);
            var id = service.GetDefaultDeviceId(true);
            using var device = service.OpenDevice(id);
            var @params = new XtDeviceStreamParams();
            @params.stream.interleaved = false;
            @params.stream.onBuffer = OnBuffer;
            @params.bufferSize = BufferSizeMillis;
            @params.format.mix.sample = Format;
            @params.format.mix.rate = SampleRate;
            @params.format.channels.outputs = Channels;
            using var stream = device.OpenStream(in @params, MakeUserData());
            using var buffer = XtSafeBuffer.Register(stream);
            stream.Start();
            Thread.Sleep(SawSweepTimeSeconds * 1000);
            stream.Stop();
        }

        static UserData MakeUserData()
        {
            var result = new UserData();
            result.Filter = new NoFilter();
            result.Saw = new Saw(SampleRate);
            return result;
        }

        static float PitchToFrequency(float pitch)
        {
            return 440.0f * MathF.Pow(2.0f, (pitch - 69.0f) / 12.0f);
        }

        static int OnBuffer(XtStream stream, in XtBuffer buffer, object user)
        {
            var data = (UserData)user;
            const float lengthSamples = SawSweepTimeSeconds * SampleRate;
            const float sweepRangePitch = SawPitchMIDIEnd - SawPitchMIDIStart;

            var saw = data.Saw;
            var filter = data.Filter;
            var safe = XtSafeBuffer.Get(stream);
            safe.Lock(buffer);
            var output = (float[][])safe.GetOutput();
            for (int f = 0; f < buffer.frames; f++)
            {
                float pitch = SawPitchMIDIStart + sweepRangePitch * (data.StreamPosition / (float)lengthSamples);
                float frequency = PitchToFrequency(pitch);
                output[0][f] = output[1][f] = filter.Next(0, 0, saw.Next(frequency));
                data.StreamPosition++;
            }
            safe.Unlock(buffer);
            return 0;
        }
    }
}