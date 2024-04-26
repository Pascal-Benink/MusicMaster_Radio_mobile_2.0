using System;
using Android.Content;
using Android.Util;
using System.Diagnostics;
using MusicMaster_Radio_mobile_2._0.Platforms.Android;
using MusicMaster_Radio_mobile_2._0.Interfaces;

namespace MusicMaster_Radio_mobile_2._0.Platforms.Android
{
    [BroadcastReceiver]
	public class NotificationActionReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            Debug.WriteLine("OnReceive method called.");

            string action = intent.Action;
            Debug.WriteLine("Received action: " + action);

            if (action.Equals("STOP_MUSIC"))
            {
                Debug.WriteLine("Sent StopMusicMessage");
                MessagingCenter.Send<object>(new object(), "StopMusicMessage");
            }
            else if (action.Equals("DELETE_NOTIFICATION"))
            {
                Debug.WriteLine("Sent DeleteNotificationMessage");
                MessagingCenter.Send<object>(new object(), "StopMusicMessage");
            }
        }
    }
}
