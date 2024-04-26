using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Android.Media;
using Android.Support.V4.App;
using Android.Content;
using MusicMaster_Radio_mobile_2._0.Interfaces;
using MusicMaster_Radio_mobile_2._0.Platforms.Android;
using AndroidX.Core.App;

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

        DependencyService.Register<IAudioPlayer, AudioPlayer>();
        DependencyService.Register<INotificationManager, NotificationManagerImplementation>();

        RequestNotificationPermission();

        RequestBatteryOptimizationExemption();
    }

    private void RequestBatteryOptimizationExemption()
    {
        // Check and request battery optimization exemption
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            var packageName = Android.App.Application.Context.PackageName;
            var pm = (PowerManager)Android.App.Application.Context.GetSystemService(Context.PowerService);

            if (!pm.IsIgnoringBatteryOptimizations(packageName))
            {
                var intent = new Intent();
                intent.SetAction(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
                intent.SetData(Android.Net.Uri.Parse("package:" + packageName));

                // Add the FLAG_ACTIVITY_NEW_TASK flag
                intent.AddFlags(ActivityFlags.NewTask);

                Android.App.Application.Context.StartActivity(intent);
            }
        }
    }

    protected override void OnNewIntent(Intent intent)
    {
        base.OnNewIntent(intent);

        if (intent.HasExtra("notification_clicked"))
        {
            System.Diagnostics.Debug.WriteLine("ello");
            var notificationData = MusicMaster_Radio_mobile_2._0.App.AppData;
            System.Diagnostics.Debug.WriteLine(notificationData.SelectedStation);
            if (notificationData != null)
            {
                var selectedStation = notificationData.SelectedStation;
                var notificationId = notificationData.NotificationId;
                var notificationManager = DependencyService.Get<INotificationManager>();
                if (notificationManager != null)
                {
                    notificationManager.SendNotification("Now Playing", $"{selectedStation} is playing", notificationId);
                }
            }
        }
    }

    private void RequestNotificationPermission()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            var context = Android.App.Application.Context;
            var notificationManager = NotificationManagerCompat.From(context);

            if (!notificationManager.AreNotificationsEnabled())
            {
                var intent = new Intent(Android.Provider.Settings.ActionAppNotificationSettings);
                intent.PutExtra(Android.Provider.Settings.ExtraAppPackage, context.PackageName);
                intent.AddFlags(ActivityFlags.NewTask); // Add this line to specify FLAG_ACTIVITY_NEW_TASK
                StartActivity(intent);
            }
        }
    }
}

