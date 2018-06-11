using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;


namespace GuitarEffects
{
    class EffectStream : WaveStream
    {
        public WaveStream SourceStream { get; set; }

        public List<IEffect> Effects { get; private set; }

        public EffectStream(WaveStream stream)
        {
            this.SourceStream = stream;
            this.Effects = new List<IEffect>();
        }

        public override long Length
        {
            get { return SourceStream.Length; }
        }

        public override long Position
        {
            get
            {
                return SourceStream.Position;
            }
            set
            {
                SourceStream.Position = value;
            }
        }

        public override WaveFormat WaveFormat
        {
            get { return SourceStream.WaveFormat; }
        }

        private int channel = 0;
        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = SourceStream.Read(buffer, offset, count);
            bool applyEffects = false;

            if(Effects.Count % WaveFormat.Channels == 0)
            {
                applyEffects = true;
            }

            for (int i = 0; i < read/4; i++)
            {
                float sample = BitConverter.ToSingle(buffer, i * 4);

                if (applyEffects == true)
                {
                    for(int j = channel; j < Effects.Count; j += WaveFormat.Channels)
                    {
                        sample = Effects[j].ApplyEffect(sample);
                    }
                    channel = (channel + 1) % WaveFormat.Channels;
                }

                byte[] bytes = BitConverter.GetBytes(sample);
                bytes.CopyTo(buffer, i * 4);
            }

            return read;
        }
    }
}
