using Microsoft.Xna.Framework.Audio;

namespace Fun.Engine.Audio.Models
{
    public class AudioTrack
    {
        private readonly AudioSettings _settings;

        public SoundEffect Sound { get; }

        public AudioTrack(SoundEffect sound)
        {
            Sound = sound;
            _settings = new AudioSettings(1f, 0f, 0f);
        }

        public AudioTrack(SoundEffect sound, AudioSettings settings)
        {
            Sound = sound;
            _settings = settings;
        }

        public SoundEffectInstance CreateInstance()
        {
            var instance = Sound.CreateInstance();
            instance.Volume = _settings.Volume;
            instance.Pitch = _settings.Pitch;
            instance.Pan = _settings.Pan;

            return instance;
        }
    }
}