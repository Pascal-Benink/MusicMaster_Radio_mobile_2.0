using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Media;
using Android.Support.V4.App;
using Android.Content;
using MusicMaster_Radio_mobile_2._0.Interfaces;
using MusicMaster_Radio_mobile_2._0.Platforms.Android;

[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessNetworkState)]
[assembly: UsesPermission(Android.Manifest.Permission.WakeLock)]
[assembly: UsesPermission(Android.Manifest.Permission.ForegroundService)]
namespace MusicMaster_Radio_mobile_2._0;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channelId = "musicmaster_radio_channel";
            var channelName = "MusicMaster Radio";
            var channelDescription = "Notifications for MusicMaster Radio";
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            var channel = new NotificationChannel(channelId, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };
            notificationManager.CreateNotificationChannel(channel);
        }

        DependencyService.Register<IAudioPlayer, AudioPlayer>();
    }
}
