namespace Fun.Engine.Audio.Models
{
    public class AudioSettings(float volume, float pitch, float pan)
    {
        public float Volume { get; } = volume;

        public float Pitch { get; } = pitch;

        public float Pan { get; } = pan;
    }
}