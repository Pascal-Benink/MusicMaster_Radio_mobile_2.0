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

        DependencyService.Register<IAudioPlayer, AudioPlayer>();
    }
}
