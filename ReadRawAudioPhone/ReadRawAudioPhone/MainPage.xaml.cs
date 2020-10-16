using System;
using Xamarin.Forms;

namespace ReadRawAudioPhone
{
    public partial class MainPage : ContentPage
    {
        private IRawAudioRecorder _rawAudioRecorder; 

        public MainPage(IRawAudioRecorder rawAudioRecorder)
        {
            _rawAudioRecorder = rawAudioRecorder; 
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if(_rawAudioRecorder.IsRecording)
            {
                _rawAudioRecorder.StopRecording(TimeSpan.FromSeconds(1)); 
            }
            else
            {
                _rawAudioRecorder.StartRecordingAsync();
            }
        }
    }
}
