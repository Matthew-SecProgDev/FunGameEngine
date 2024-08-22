using Microsoft.Xna.Framework.Audio;

namespace Fun.Engine.Audio
{
    public class BackgroundMusicManager : IDisposable
    {
        private bool _isDisposed;
        private int _musicTrackIndex;
        private SoundEffectInstance[] _musicTracks = [];

        public void SetBackgroundMusic(IEnumerable<Models.AudioTrack> tracks)
        {
            foreach (var track in _musicTracks)
            {
                track.Dispose();
            }

            _musicTracks = tracks.Select(i => i.CreateInstance()).ToArray();
            _musicTrackIndex = 0; // Initialize to the first track
        }

        public void PlayMusic()
        {
            var trackCount = _musicTracks.Length;

            if (trackCount == 0)
            {
                return;
            }

            var currentTrack = _musicTracks[_musicTrackIndex];
            if (currentTrack.State == SoundState.Stopped)
            {
                var nextTrackIndex = (_musicTrackIndex + 1) % trackCount;
                _musicTracks[nextTrackIndex].Play();
                _musicTrackIndex = nextTrackIndex;
            }
        }

        public void StopMusic()
        {
            if (_musicTracks.Length == 0)
            {
                return;
            }

            var currentTrack = _musicTracks[_musicTrackIndex];
            if (currentTrack.State == SoundState.Playing)
            {
                currentTrack.Stop();
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
                    foreach (var track in _musicTracks)
                    {
                        track.Dispose();
                    }
                }

                // TODO: Override finalizer (destructor) only if there is code to free unmanaged resources
                // Free unmanaged resources (unmanaged objects)
                // Set large fields to null

                _isDisposed = true;
            }
        }
    }
}