using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ReadRawAudioPhone
{
    public partial class App : Application
    {
        public App(IRawAudioRecorder rawAudioRecorder, IRawAudioPlayback rawAudioPlayback)
        {
            InitializeComponent();

            MainPage = new MainPage(rawAudioRecorder, rawAudioPlayback);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
