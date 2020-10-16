using Android.Media;
using System;
using System.Threading;

namespace ReadRawAudioPhone.Droid
{
    public class RawAudioRecorder : IRawAudioRecorder
    {
        private bool _shouldRecord = false; 
        private Thread _thread;
        private bool _recording = false; 

        public bool IsRecording => _recording;

        public void StartRecordingAsync()
        {
            lock(this)
            {
                if (_thread == null)
                {
                    _thread = new Thread(new ThreadStart(() =>
                    {
                        int bufferSize = AudioRecord.GetMinBufferSize(44100,
                            ChannelIn.Mono,
                            Android.Media.Encoding.Pcm16bit);

                        AudioRecord record = new AudioRecord(
                            AudioSource.Mic,
                            44100,
                            ChannelIn.Mono,
                            Android.Media.Encoding.Pcm16bit,
                            bufferSize);

                        byte[] buffer = new byte[bufferSize];

                        try
                        {
                            record.StartRecording();
                            while (_shouldRecord)
                            {
                                int bytesRead = record.Read(buffer, 0, buffer.Length);
                                _recording = true; 
                            }
                        }
                        finally
                        {
                            _recording = false; 
                            record.Stop();
                            record.Release();
                            record.Dispose();
                            record = null;
                        }

                    }));
                    _shouldRecord = true; 
                    _thread.Start();
                }
            }
        }

        public bool StopRecording(TimeSpan timeout)
        {
            bool ret = false; 
            lock(this)
            {
                if(_thread != null)
                {
                    _shouldRecord = false;
                    ret = _thread.Join(timeout); 
                }
            }
            return ret; 
        }
    }
}