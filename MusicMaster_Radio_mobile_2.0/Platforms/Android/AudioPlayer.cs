using Android.Content;
using Android.Media;
using Android.Media.Session;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndroidApp = Android.App.Application;
using Amedia = Android.Media;
using MusicMaster_Radio_mobile_2._0.Platforms.Android;
using MusicMaster_Radio_mobile_2._0.Interfaces;
using static Android.OS.PowerManager;

    [assembly: Dependency(typeof(AudioPlayer))]
    [assembly: Dependency(typeof(MusicMaster_Radio_mobile_2._0.Platforms.Android.AudioPlayer))]

namespace MusicMaster_Radio_mobile_2._0.Platforms.Android
{
    class AudioPlayer : IAudioPlayer
    {
        private MediaPlayer mediaPlayer;
        private readonly Context context;
        private AudioManager audioManager;
        private PowerManager powerManager;
        private WakeLock wakeLock;
        private MediaSession mediaSession;
        private bool isPlaying;
        private bool hasStarted;

        public AudioPlayer()
        {
            context = AndroidApp.Context;
            audioManager = (AudioManager)context.GetSystemService(Context.AudioService);

            mediaSession = new MediaSession(context, "AudioPlayerSession");
        }

        public void PlayAudio(string audioUrl)
        {
            try
            {
                mediaPlayer = new MediaPlayer();

                var audioAttributes = new AudioAttributes.Builder()
                   .SetUsage(AudioUsageKind.Media)
                   .SetContentType(AudioContentType.Music)
                   .Build();

                mediaPlayer.SetAudioAttributes(audioAttributes);

                int audioSessionId = mediaPlayer.AudioSessionId;

                mediaPlayer.SetDataSource(audioUrl);
                mediaPlayer.Prepare();
                mediaPlayer.Start();

                RequestAudioFocus();

                powerManager = (PowerManager)context.GetSystemService(Context.PowerService);
                wakeLock = powerManager.NewWakeLock(WakeLockFlags.Full, "AudioPlayerWakeLock");
                wakeLock.Acquire();

                System.Diagnostics.Debug.WriteLine("Wakelock" + wakeLock);

                mediaSession.Active = true;
                isPlaying = true;
                if (!hasStarted)
                {
                    hasStarted = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error playing audio: " + ex.Message);
            }
        }

        public void PauseAudio()
        {
            if (isPlaying)
            {
                if (mediaPlayer != null)
                {
                    mediaPlayer.Pause();
                }
                if (wakeLock != null && wakeLock.IsHeld)
                {
                    System.Diagnostics.Debug.WriteLine("Wakelock" + wakeLock);
                    wakeLock.Release();
                    System.Diagnostics.Debug.WriteLine("Wakelock" + wakeLock);
                }
                isPlaying = false;
            }
        }

        public void StopAudio()
        {
            // Stop and release MediaPlayer
            if (mediaPlayer != null)
            {
                mediaPlayer.Stop();
                mediaPlayer.Release();
                mediaPlayer = null;
            }

            if (wakeLock != null && wakeLock.IsHeld)
            {
                System.Diagnostics.Debug.WriteLine("Wakelock" + wakeLock);
                wakeLock.Release();
                System.Diagnostics.Debug.WriteLine("Wakelock" + wakeLock);
            }
            isPlaying = false;
            hasStarted = false;

            AbandonAudioFocus();
        }

        private void RequestAudioFocus()
        {
            if (audioManager != null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var audioAttributes = new AudioAttributes.Builder()
                       .SetUsage(AudioUsageKind.Media)
                       .SetContentType(AudioContentType.Music)
                       .Build();

                    var focusRequest = new AudioFocusRequestClass.Builder(AudioFocus.Gain)
                       .SetAudioAttributes(audioAttributes)
                       .Build();

                    var result = audioManager.RequestAudioFocus(focusRequest);

                    if (result == AudioFocusRequest.Granted)
                    {
                        // Audio focus granted
                    }
                    else
                    {
                        // Handle the case when audio focus is not granted
                        System.Diagnostics.Debug.WriteLine("OI There bud the auido focus has not been granted please fix");
                    }
                }
                else
                {
                    // Use the older audio focus request mechanism for older Android versions
                    var result = audioManager.RequestAudioFocus(null, Amedia.Stream.Music, AudioFocus.Gain);

                    if (result == AudioFocusRequest.Granted)
                    {
                        // Audio focus granted, start audio playback here
                    }
                    else
                    {
                        // Handle the case when audio focus is not granted
                    }
                }
            }
        }

        private void AbandonAudioFocus()
        {
            if (audioManager != null)
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    audioManager.AbandonAudioFocusRequest(new AudioFocusRequestClass.Builder(AudioFocus.Gain).Build());
                }
                else
                {
                    audioManager.AbandonAudioFocus(null);
                }
            }
        }

    }
}