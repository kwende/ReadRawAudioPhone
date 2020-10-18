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
    public class RawAudioPlayback : IRawAudioPlayback
    {
        private int _byteIndex; 
        private Thread _thread;
        public bool IsPlaying { get; private set; }
        private bool _shouldPlay = false;
        AudioTrack _play; 

        public event EventHandler<EventArgs> OnPlaybackStopped;

        public void StartPlaybackAsync(byte[] buffer)
        {
            lock (this)
            {
                if (_thread == null)
                {
                    _thread = new Thread(new ThreadStart(() =>
                    {
                        _shouldPlay = true; 
                        _byteIndex = 0; 

                        int numBytesToWrite = AudioTrack.GetMinBufferSize(44100,
                            ChannelOut.Mono,
                            Android.Media.Encoding.Pcm16bit);

                        _play = new AudioTrack(
                            // Stream type
                            Android.Media.Stream.Music,
                            // Frequency
                            44100,
                            // Mono or stereo
                            ChannelOut.Mono,
                            // Audio encoding
                            Android.Media.Encoding.Pcm16bit,
                            // Length of the audio clip.
                            numBytesToWrite,
                            // Mode. Stream or static.
                            AudioTrackMode.Stream);

                        float secondsOfPlayback = buffer.Length / 44100 / 2.0f; 

                        _play.Play();
                        while (_shouldPlay)
                        {
                            int bytesWritten = _play.Write(buffer, _byteIndex, numBytesToWrite);

                            _byteIndex += bytesWritten; 

                            if(_byteIndex + numBytesToWrite > buffer.Length)
                            {
                                numBytesToWrite = buffer.Length - _byteIndex; 
                            }
                        }

                    }));
                    _shouldPlay = true;
                    _thread.Start();
                }
            }
        }

        public void StopPlayback(TimeSpan timeout)
        {
            lock (this)
            {
                if (_thread != null)
                {
                    _shouldPlay = false;
                    _thread.Join(timeout);

                    IsPlaying = false;
                    _play.Stop();
                    _play.Release();
                    _play.Dispose();
                    _play = null;
                    OnPlaybackStopped?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}