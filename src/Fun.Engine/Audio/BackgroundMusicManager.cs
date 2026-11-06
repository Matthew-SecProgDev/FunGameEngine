using Microsoft.Xna.Framework.Audio;

namespace Fun.Engine.Audio
{
    public class BackgroundMusicManager : IDisposable
    {
        private bool _isDisposed;
        private int _musicTrackIndex = -1;
        private SoundEffectInstance[] _musicTracks = [];

        public void SetBackgroundMusic(IEnumerable<Models.AudioTrack> tracks)
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            var newTracks = tracks
                .Select(track => track.CreateInstance())
                .ToArray();

            foreach (var track in _musicTracks)
            {
                track.Dispose();
            }

            _musicTracks = newTracks;
            _musicTrackIndex = 0;
        }

        public void PlayMusic()
        {
            ObjectDisposedException.ThrowIf(_isDisposed, this);

            if (_musicTracks.Length == 0)
            {
                return;
            }

            if (_musicTrackIndex >= 0 &&
                _musicTracks[_musicTrackIndex].State != SoundState.Stopped)
            {
                return;
            }

            _musicTrackIndex = (_musicTrackIndex + 1) % _musicTracks.Length;

            _musicTracks[_musicTrackIndex].Play();
        }

        public void StopMusic()
        {
            if (_isDisposed || _musicTrackIndex < 0)
            {
                return;
            }

            _musicTracks[_musicTrackIndex].Stop();
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
                    foreach (var track in _musicTracks)
                    {
                        track.Dispose();
                    }
                }

                _isDisposed = true;
            }
        }
    }
}