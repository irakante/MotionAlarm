
using Xamarin.Forms;
using Xamarin.Essentials;
using Plugin.SimpleAudioPlayer;
using System.Diagnostics;
using System;
using System.Threading;
using App1.Models;
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
                InitialAcceleration = ""
            }; 
        }

     
    }
}
