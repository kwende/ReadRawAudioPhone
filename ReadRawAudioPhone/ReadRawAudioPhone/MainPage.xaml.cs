using System;
using Xamarin.Forms;

namespace ReadRawAudioPhone
{
    public partial class MainPage : ContentPage
    {
        private IRawAudioRecorder _rawAudioRecorder;
        private IRawAudioPlayback _rawAudioPlayback; 

        public MainPage(IRawAudioRecorder rawAudioRecorder, IRawAudioPlayback rawAudioPlayback)
        {
            _rawAudioRecorder = rawAudioRecorder;
            _rawAudioPlayback = rawAudioPlayback;

            _rawAudioPlayback.OnPlaybackStopped += _rawAudioPlayback_OnPlaybackStopped;

            InitializeComponent();
        }

        private void _rawAudioPlayback_OnPlaybackStopped(object sender, EventArgs e)
        {
            if(Dispatcher.IsInvokeRequired)
            {
                Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    RecordButton.Text = "Record";
                }); 
            }
            else
            {
                RecordButton.Text = "Record";
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if(_rawAudioRecorder.IsRecording)
            {
                byte[] buffer = _rawAudioRecorder.StopRecording(TimeSpan.FromSeconds(1));
                _rawAudioPlayback.StartPlaybackAsync(buffer); 
                RecordButton.Text = "Stop Playback";
            }
            else if(_rawAudioPlayback.IsPlaying)
            {
                _rawAudioPlayback.StopPlayback(TimeSpan.FromSeconds(1));
                RecordButton.Text = "Record"; 
            }
            else if(!_rawAudioPlayback.IsPlaying && !_rawAudioRecorder.IsRecording)
            {
                _rawAudioRecorder.StartRecordingAsync();
                RecordButton.Text = "Stop & Playback";
            }
        }
    }
}
