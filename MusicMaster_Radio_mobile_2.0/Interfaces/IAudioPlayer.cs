using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicMaster_Radio_mobile_2._0.Interfaces
{
    internal interface IAudioPlayer
    {
        void PlayAudio(string audioUrl);
        void PauseAudio();
        void StopAudio();
    }
}
