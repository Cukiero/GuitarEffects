using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuitarEffects.Effects
{
    class Reverb : IEffect
    {
        public int ReverbLength { get; private set; }
        public float ReverbDecay { get; private set; }

        public Queue<float> samples;
        public Reverb(int delay = 100, float decay = 0.7f)
        {
            this.ReverbLength = (int)((float)delay * 44.1f);
            this.ReverbDecay = decay;
            this.samples = new Queue<float>();

            for (int i = 0; i < ReverbLength; i++) samples.Enqueue(0f);
        }


        public float ApplyEffect(float sample)
        {
            float sample_main = (ReverbDecay * samples.Dequeue()) ;

            sample = sample + sample_main;
            samples.Enqueue(sample);

            return Math.Max(-1, Math.Min(1, sample));
        }

    }
}
