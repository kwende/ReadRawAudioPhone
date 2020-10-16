using System;

namespace ReadRawAudioPhone
{
    public interface IRawAudioRecorder
    {
        void StartRecordingAsync();
        bool StopRecording(TimeSpan timeout); 
        bool IsRecording { get; }
    }
}