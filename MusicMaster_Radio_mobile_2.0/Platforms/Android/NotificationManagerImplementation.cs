using Android.App;
using andriod_app = Android.App;
using anriod_provider = Android.Provider;
using andriod_net = Android.Net;
using Android.Content;
using Android.Support.V4.App;
using Android.OS;
using AndroidX.Core.App;
using Android.Content.PM;
using System;
using Android;
using MusicMaster_Radio_mobile_2._0.Platforms.Android;
using MusicMaster_Radio_mobile_2._0.Interfaces;

[assembly: Dependency(typeof(NotificationManagerImplementation))]
[assembly: Dependency(typeof(MusicMaster_Radio_mobile_2._0.Platforms.Android.NotificationManagerImplementation))]

namespace MusicMaster_Radio_mobile_2._0.Platforms.Android
{
    public class NotificationManagerImplementation : INotificationManager
    {
        const string channelId = "default_channel_id";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        bool initialized = false;
        //int notificationId = 0;

        public event EventHandler NotificationReceived;

        public void Initialize()
        {
            if (!initialized)
            {
                CreateNotificationChannel();
                initialized = true;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    if (!NotificationManagerCompat.From(andriod_app.Application.Context).AreNotificationsEnabled())
                    {
                        var intent = new Intent(anriod_provider.Settings.ActionAppNotificationSettings);
                        intent.PutExtra(anriod_provider.Settings.ExtraAppPackage, andriod_app.Application.Context.PackageName);
                        intent.AddFlags(ActivityFlags.NewTask);

                        andriod_app.Application.Context.StartActivity(intent);
                    }
                }
            }
        }

        public void SendNotification(string title, string message, int notificationId = 0)
        {
            var context = andriod_app.Application.Context;
            var intent = new Intent(context, typeof(MainActivity)); // Change to your main activity
            intent.AddFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTop);

            intent.PutExtra("notification_clicked", true);

            PendingIntent pendingIntent;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                try
                {
                    pendingIntent = PendingIntent.GetActivity(context, notificationId, intent, PendingIntentFlags.OneShot | PendingIntentFlags.Immutable);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("pendingIntent in if " + e);
                    throw;
                }
            }
            else
            {
                try
                {
                    pendingIntent = PendingIntent.GetActivity(context, notificationId, intent, PendingIntentFlags.OneShot);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("pendingIntent in else " + e);
                    throw;
                }
            }

            var stopIntent = new Intent(context, typeof(NotificationActionReceiver));
            stopIntent.SetAction("STOP_MUSIC");
            PendingIntent stopPendingIntent;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                try
                {
                    stopPendingIntent = PendingIntent.GetBroadcast(context, 0, stopIntent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("stopPendingIntent in if " + e);
                    throw;
                }
            }
            else
            {
                try
                {
                    stopPendingIntent = PendingIntent.GetBroadcast(context, 0, stopIntent, PendingIntentFlags.UpdateCurrent);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("stopPendingIntent in else " + e);
                    throw;
                }
            }

            var openAppIntent = new Intent(context, typeof(MainActivity)); // Replace with your main activity
            openAppIntent.AddFlags(ActivityFlags.ClearTop);
            PendingIntent openAppPendingIntent;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                try
                {
                    openAppPendingIntent = PendingIntent.GetActivity(context, 0, openAppIntent, PendingIntentFlags.OneShot | PendingIntentFlags.Immutable);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("openAppPendingIntent in if " + e);
                    throw;
                }
            }
            else
            {
                try
                {
                    openAppPendingIntent = PendingIntent.GetActivity(context, 0, openAppIntent, PendingIntentFlags.OneShot);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("openAppPendingIntent in else " + e);
                    throw;
                }
            }

            var deleteIntent = new Intent(context, typeof(NotificationActionReceiver));
            deleteIntent.SetAction("DELETE_NOTIFICATION");
            PendingIntent deletePendingIntent;
            try
            {
                deletePendingIntent = PendingIntent.GetBroadcast(context, 0, deleteIntent, PendingIntentFlags.CancelCurrent | PendingIntentFlags.Immutable);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("deletePendingIntent " + e);
                throw;
            }



            var notificationBuilder = new NotificationCompat.Builder(context, channelId)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetSmallIcon(Resource.Drawable.icon)
                .SetAutoCancel(true)
                .SetContentIntent(pendingIntent)
                .AddAction(Resource.Drawable.stop_icon, "Stop Music", stopPendingIntent)
                .SetDeleteIntent(deletePendingIntent)
                .SetPriority((int)NotificationPriority.High)
                .SetOngoing(true);


            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Notify(notificationId, notificationBuilder.Build());

            NotificationReceived?.Invoke(this, EventArgs.Empty);
        }

        public void CancelNotification(int id)
        {
            var context = andriod_app.Application.Context;
            var notificationManager = NotificationManagerCompat.From(context);
            notificationManager.Cancel(id);
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                return; // Notification channels are only supported on Android Oreo and higher
            }

            var context = andriod_app.Application.Context;
            var notificationManager = (NotificationManager)context.GetSystemService(Context.NotificationService);

            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default)
            {
                Description = channelDescription,
            };

            notificationManager.CreateNotificationChannel(channel);
        }
    }
}
