using Microsoft.Xna.Framework.Audio;

namespace Fun.Engine.Audio
{
    public class SoundEffectManager : IDisposable
    {
        private readonly Dictionary<object, SoundEffectInstance> _sounds = [];

        private bool _isDisposed;

        public void RegisterSound<TKey>(TKey key, Models.AudioTrack track) where TKey : Enum
        {
            _sounds[key] = track.CreateInstance();
        }

        public void PlaySound<TKey>(TKey key) where TKey : Enum
        {
            if (_sounds.TryGetValue(key, out var instance))
            {
                instance.Play();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // Dispose managed state (managed objects)
                    foreach (var instance in _sounds.Values)
                    {
                        instance.Dispose();
                    }

                    _sounds.Clear();
                }

                // TODO: Override finalizer (destructor) only if there is code to free unmanaged resources
                // Free unmanaged resources (unmanaged objects)
                // Set large fields to null

                _isDisposed = true;
            }
        }

        private static void ValidateEnum<TKey>(TKey value) where TKey : Enum
        {
            //if (!Enum.IsDefined(typeof(TKey), value))
            if (!typeof(TKey).IsDefined(typeof(Attributes.AudioTrackAttribute), false))
            {
                throw new ArgumentException($"Invalid enum value: {value}", nameof(value));
            }
        }
    }
}