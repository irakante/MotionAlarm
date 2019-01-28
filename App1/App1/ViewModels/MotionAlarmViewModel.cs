using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows.Input;
using App1.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace App1.ViewModels
{
    public class MotionAlarmViewModel : INotifyPropertyChanged
    {
        public ICommand PinForCompare_Completed { get; set; }
        public ICommand PinForExit_Completed { get; set; }

        private MotionAlarm motionAlarm;

        public MotionAlarmViewModel()
        {
            motionAlarm = new MotionAlarm();          
            PinForCompare_Completed = new Command(PinForCompare_Completed_Command);
            PinForExit_Completed = new Command(PinForExit_Completed_Command);
        }

        public bool IsPlaying
        {
            get { return motionAlarm.IsPlaying; }
            set
            {
                if (motionAlarm.IsPlaying != value)
                {
                    motionAlarm.IsPlaying = value;
                    OnPropertyChanged("isPlaying");
                }
            }
        }

        public string InitialAcceleration
        {
            get { return motionAlarm.InitialAcceleration; }
            set
            {
                if (motionAlarm.InitialAcceleration != value)
                {
                    motionAlarm.InitialAcceleration = value;
                    OnPropertyChanged("InitialAcceleration");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _pinForExit;
        public string PinForExit
        {
            set
            {
                if (_pinForExit != value)
                {
                    _pinForExit = value;
                    OnPropertyChanged("PinForExit");
                }
            }
            get
            {
                return _pinForExit;
            }
        }

        private string _pinForCompare;
        public string PinForCompare
        {
            set
            {
                if (_pinForCompare != value)
                {
                    _pinForCompare = value;
                    OnPropertyChanged("PinForCompare");
                }
            }
            get
            {
                return _pinForCompare;
            }
        }

        string _labelSatus;
        public string LabelSatus
        {
            set
            {
                if (_labelSatus != value)
                {
                    _labelSatus = value;
                    OnPropertyChanged("LabelSatus");
                }
            }
            get
            {
                return _labelSatus;
            }
        }


        private bool _stackForEnterIsVisible;
        public bool StackForEnterIsVisible
        {
            get
            {
                return _stackForEnterIsVisible;
            }
            set
            {
                _stackForEnterIsVisible = value;
                OnPropertyChanged("StackForEnterIsVisible");
            }
        }

        private bool _stackForExitIsVisible;
        public bool StackForExitIsVisible
        {
            get
            {
                return _stackForExitIsVisible;
            }
            set
            {
                _stackForExitIsVisible = value;
                OnPropertyChanged("StackForExitIsVisible");
            }
        }

        private bool _labelShowNoticeIsVisible;
        public bool LabelShowNoticeIsVisible
        {
            get
            {
                return _labelShowNoticeIsVisible;
            }
            set
            {
                _labelShowNoticeIsVisible = value;
                OnPropertyChanged("LabelShowNoticeIsVisible");
            }
        }

        private void InitializeAccelerometer()
        {
            try
            {
                LabelShowNoticeIsVisible = true;
                Accelerometer.Start(SensorSpeed.UI);
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
                LabelSatus = "X:" + data.Acceleration.X + " Y:" + data.Acceleration.Y + " Z:" + data.Acceleration.Z;
                if (InitialAcceleration == "")
                {
                    InitialAcceleration = LabelSatus;
                    LabelShowNoticeIsVisible= true;
                    Thread.Sleep(2000);
                    StackForExitIsVisible = true;
                    LabelShowNoticeIsVisible = false;
                }
            });
            if (InitialAcceleration != "")
            {
                if (InitialAcceleration != LabelSatus && !IsPlaying)
                {
                    DependencyService.Get<IAudio>().PlayAudioFile("alarm.wav");
                    IsPlaying = true;
                }
            }
        }


        private void PinForCompare_Completed_Command()
        {
            if (PinForCompare != "")
            {
                InitializeAccelerometer();
                StackForEnterIsVisible = false;
            }
                
        }

        private void PinForExit_Completed_Command()
        {
            if (PinForExit == PinForCompare)
            {
                if (Accelerometer.IsMonitoring)
                {
                    Accelerometer.Stop();
                    if (IsPlaying)
                    {
                        DependencyService.Get<IAudio>().StopAudioFile("alarm.wav");
                        IsPlaying = false;
                        InitialAcceleration = "";
                        PinForCompare = "";
                        PinForExit = "";
                        StackForEnterIsVisible = true;
                        StackForExitIsVisible = false;                       
                        LabelShowNoticeIsVisible = false;
                        Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
                    }
                }
            }
            else
            {
                try
                {
                    // Use default vibration length
                    Vibration.Vibrate();

                    // Or use specified time
                    var duration = TimeSpan.FromSeconds(1);
                    Vibration.Vibrate(duration);
                }
                catch (FeatureNotSupportedException ex)
                {
                    // Feature not supported on device
                }
                catch (Exception ex)
                {
                    // Other error has occurred.
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
