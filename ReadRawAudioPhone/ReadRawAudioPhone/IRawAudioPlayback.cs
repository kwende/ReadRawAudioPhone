using System;
using System.Collections.Generic;
using System.Text;

namespace ReadRawAudioPhone
{
    public interface IRawAudioPlayback
    {
        event EventHandler<EventArgs> OnPlaybackStopped; 
        void StartPlaybackAsync(byte[] buffer);
        void StopPlayback(TimeSpan timeout); 
        bool IsPlaying { get; }
    }
}
