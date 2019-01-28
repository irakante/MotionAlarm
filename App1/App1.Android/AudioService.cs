using System;
using Xamarin.Forms;

using Android.Media;
using Android.Content.Res;
using App1.Droid;
using System.Runtime.Remoting.Contexts;

[assembly: Dependency(typeof(AudioService))]
namespace App1.Droid
{
    public class AudioService : IAudio
    {
        private static MediaPlayer player = null;       

        public AudioService()
        {
        }
        const float V = 50.0f;
        public void PlayAudioFile(string fileName)
        {
             player = new MediaPlayer();
            var fd = global::Android.App.Application.Context.Assets.OpenFd(fileName);
            player.Prepared += (s, e) =>
            {
                player.Start();
                player.Looping =true;
                player.SetVolume(V, V);
            };
            player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
            player.Prepare();
        }

        public void StopAudioFile(string fileName)
        {           
            try
            {
                player.Reset();
                player.Prepare();
                player.Stop();
                player.Release();
                player = null;
            }
            catch (Exception e)
            {
               // e.Message();
            }
        }
    }
}