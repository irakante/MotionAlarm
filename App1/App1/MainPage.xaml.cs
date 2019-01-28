
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.SimpleAudioPlayer;
using System.Diagnostics;
using System;
using System.Threading;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        SensorSpeed speed = SensorSpeed.UI;
        bool isPlaying = false;      
        string InitialAcceleration = "";
        public MainPage()
        {
            InitializeComponent();
        }

        private void InitializeAccelerometer()
        {
            try
            {
                labelShowNotice.IsVisible = true;             
                labelShowNotice.IsVisible = false;
                Accelerometer.Start(speed);
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
            }
            catch (FeatureNotSupportedException)
            {
                Debug.WriteLine("Accelerometer Unavailable");
            }
        }


        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var data = e.Reading;
                labelSatus.Text = "X:" + data.Acceleration.X + " Y:" + data.Acceleration.Y + " Z:" + data.Acceleration.Z;
                if (InitialAcceleration == "")
                {
                    InitialAcceleration = labelSatus.Text;
                    Thread.Sleep(2000);
                }
            });
            if (InitialAcceleration != "")
            {
                if (InitialAcceleration != labelSatus.Text && !isPlaying)
                {
                    DependencyService.Get<IAudio>().PlayAudioFile("alarm.wav");
                    isPlaying = true;
                }
            }
        }


        private void ToggleAccelerometr_Toggled(object sender, ToggledEventArgs e)
        {
            try
            {
                if (Accelerometer.IsMonitoring)
                {
                    StackForExit.IsVisible = true;
                    StackForEnter.IsVisible = false;
                }
                else
                {
                    StackForExit.IsVisible = false;
                    StackForEnter.IsVisible = true;

                }
            }
            catch { }
        }

        private void Entry_PinForCompare_Completed(object sender, EventArgs e)
        {
            InitializeAccelerometer();
            StackForEnter.IsVisible = false;
            StackForExit.IsVisible = true;
        }

        private void Entry_PinForExit_Completed(object sender, EventArgs e)
        {
            if (PinForExit.Text == PinForCompare.Text)
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    if (isPlaying)
                    {
                        DependencyService.Get<IAudio>().StopAudioFile("alarm.wav");
                        isPlaying = false;
                        InitialAcceleration = "";
                        PinForCompare.Text = "";
                        PinForExit.Text = "";
                    }
                }
            }
        }
    }
}
