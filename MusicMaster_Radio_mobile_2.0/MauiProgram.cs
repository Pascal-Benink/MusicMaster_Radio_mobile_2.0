using MusicMaster_Radio_mobile_2._0.Interfaces;
using Microsoft.Maui.Controls.Hosting;

#if ANDROID
using MusicMaster_Radio_mobile_2._0.Platforms.Android;
#endif

namespace MusicMaster_Radio_mobile_2._0;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
          .UseMauiApp<App>()
          .ConfigureFonts(fonts =>
          {
              fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
              fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
          });

#if ANDROID
        builder.Services.AddTransient<IAudioPlayer, AudioPlayer>();
#endif

        return builder.Build();
    }
}
