using System;
 

namespace App1
{
    public interface IAudio
    {
        void PlayAudioFile(string fileName);
        void StopAudioFile(string fileName);
    }
}

 