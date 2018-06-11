using System;


namespace GuitarEffects
{
    public interface IEffect
    {
        float ApplyEffect(float sample);

    }
}
