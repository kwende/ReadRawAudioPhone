using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ReadRawAudioPhone.Droid
{
    public class RawAudioRecorder
    {
        private bool _shouldRecord = false; 
        private Thread _thread; 
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
                            }
                        }
                        finally
                        {
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