
using Xamarin.Forms;
using App1.ViewModels;

namespace App1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MotionAlarmViewModel
            {
                IsPlaying = false,
                InitialAcceleration = "",
                PinForCompare = "",
                PinForExit = "",
                StackForEnterIsVisible = true,
                StackForExitIsVisible = false,
                LabelShowNoticeIsVisible = false
            };
        }
    }
}
