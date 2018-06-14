using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarEffects.Effects
{
    class Delay : IEffect
    {
        public int DelaySamples { get; private set; }
        public float Volume { get; private set; }

        public Queue<float> samples;
        public Delay(int bpm = 100, float volume = 0.7f)
        {
            double delayms = (double)(60 * 1000) / ((double)bpm * 2);
            this.DelaySamples = (int)((float)delayms * 44.1f);
            this.Volume = volume;
            this.samples = new Queue<float>();

            for (int i = 0; i < DelaySamples; i++) samples.Enqueue(0f);
        }


        public float ApplyEffect(float sample)
        {
            samples.Enqueue(sample);
            sample = sample + (samples.Dequeue()) * this.Volume;

            return Math.Max(-1, Math.Min(1, sample));
        }
    }
}
