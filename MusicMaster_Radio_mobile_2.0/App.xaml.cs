namespace MusicMaster_Radio_mobile_2._0;

public partial class App : Application
{
    public static NotificationData AppData { get; set; }

    public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
