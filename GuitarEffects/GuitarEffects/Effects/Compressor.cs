using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarEffects.Effects
{
    enum CompressorType
    {
        Upward, Downward
    }
    class Compressor : IEffect
    {
        CompressorType compType;
        public float VolThresh { get; private set; }
        public double Gain { get; private set; }
        public float VolGate { get; private set; }

        public Compressor(int threshold = -10, double gain = 0.60, int gatedB = -25, CompressorType type = CompressorType.Upward)
        {
            this.compType = type;
            this.VolThresh = (float)Math.Pow(10.0f, threshold * 0.05f);
            this.VolGate = (float)Math.Pow(10.0f, gatedB * 0.05f);
            this.Gain = gain;
        }

        public float ApplyEffect(float sample)
        {
            if(sample < this.VolThresh && sample > this.VolGate)
            {
                double smpl = (double)this.VolThresh / (double)sample;

                sample = sample * (float)Math.Pow(smpl, this.Gain);
            }

            return Math.Max(-1, Math.Min(1, sample));
        }
    }
}
