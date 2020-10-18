using System;

namespace ReadRawAudioPhone
{
    public interface IRawAudioRecorder
    {
        void StartRecordingAsync();
        byte[] StopRecording(TimeSpan timeout); 
        bool IsRecording { get; }
    }
}