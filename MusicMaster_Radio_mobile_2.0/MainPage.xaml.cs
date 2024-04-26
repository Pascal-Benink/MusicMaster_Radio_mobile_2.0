using System.Diagnostics;
using MusicMaster_Radio_mobile_2._0.Interfaces;

namespace MusicMaster_Radio_mobile_2._0;

public partial class MainPage : ContentPage
{
    private IAudioPlayer audioPlayer;

    private INotificationManager notificationManager;

    private const int notificationId = 100;


    public MainPage()
    {
        InitializeComponent();
        audioPlayer = DependencyService.Get<IAudioPlayer>();
        notificationManager = DependencyService.Get<INotificationManager>();

        if (notificationManager != null)
        {

            notificationManager.Initialize();
        }
        else
        {
            Debug.WriteLine("notificationManager is null");
            Debug.WriteLine(notificationManager);
        }

        MessagingCenter.Subscribe<object>(this, "StopMusicMessage", (sender) =>
        {
            StopButton_Clicked(this, EventArgs.Empty);
        });

        radioButton1.CheckedChanged += RadioButton_CheckedChanged;
        radioButton2.CheckedChanged += RadioButton_CheckedChanged;
        radioButton3.CheckedChanged += RadioButton_CheckedChanged;
        radioButton4.CheckedChanged += RadioButton_CheckedChanged;

        
        var App_Version = AppInfo.Current.VersionString;

        versionLabel.Text += App_Version;

        Debug.WriteLine("app version" + App_Version);
    }

    /*private void DisplayAlertUnauthorized()
    {
        Application.Current.MainPage.Dispatcher.Dispatch(() =>
        {
            Debug.WriteLine("await DisplayAlert");
            DisplayAlert("Unauthorized version detected", "If you are not a developer please delete this app and report how you got this app to support", "OK");
            Debug.WriteLine("end await DisplayAlert");
        });
    }*/

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var App_Version = AppInfo.Current.VersionString;

        Debug.WriteLine("app version" + App_Version);

        await Task.Delay(3000); // Delay for 1 second


        if (App_Version.Contains("dev"))
        {
            DisplayAlertUnauthorized();
        }
    }

    private void DisplayAlertUnauthorized()
    {
        Debug.WriteLine("await DisplayAlert");
        DisplayAlert("Unauthorized version detected", "If you are not a developer please delete this app and report how you got this app to support", "OK");
        Debug.WriteLine("end await DisplayAlert");
    }

    public bool playing = false;

    public string radiourl = "https://25343.live.streamtheworld.com/100PNL_MP3_SC";

    public string selectedStation = "100%NL";

    private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value && playing == false)
        {
            RadioButton selectedRadioButton = (RadioButton)sender;

            string imagePath = string.Empty;


            if (selectedRadioButton == radioButton1)
            {
                selectedStation = "100%NL";
                radiourl = "https://25343.live.streamtheworld.com/100PNL_MP3_SC";
                /*imagePath = "_100pnl.png";*/
            }
            else if (selectedRadioButton == radioButton2)
            {
                selectedStation = "Radio10";
                radiourl = "https://25333.live.streamtheworld.com/RADIO10.mp3";
                /*imagePath = "/radio10.png";*/
            }
            else if (selectedRadioButton == radioButton3)
            {
                selectedStation = "Skyradio";
                radiourl = "https://22723.live.streamtheworld.com/SKYRADIO.mp3";
                imagePath = "skyradio.png";
            }
            else if (selectedRadioButton == radioButton4)
            {
                selectedStation = "RTV Rijnmond";
                radiourl = "https://dcur8bjarl5c2.cloudfront.net/icecast/rijnmond/radio-mp3";
                /*imagePath = "rtvrijnmond.png";*/
            }
            Select.Text = $"Selected: {selectedStation}";
            /*stationImage.Source = imagePath;*/
        }
    }

    public async void SupportLabel_Tapped(object sender, EventArgs e)
    {
        await Browser.OpenAsync(new Uri("https://pascal-benink.github.io/Coding-enterprice-main/Musicmaster.html"), BrowserLaunchMode.SystemPreferred);
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
    }

    void Submit_Clicked(System.Object sender, System.EventArgs e)
    {
        radiourl = urlEntry.Text;
        selectedStation = NameEntry.Text;
        Select.Text = $"Selected: {selectedStation}";
        radioButton1.IsChecked = false;
        radioButton2.IsChecked = false;
        radioButton3.IsChecked = false;
        stationImage.Source = "other.png";
    }

    private void PlayButton_Clicked(object sender, EventArgs e)
    {
        if (playing == false)
        {
            Debug.WriteLine(radiourl);
            if (audioPlayer != null)
            {
                audioPlayer.PlayAudio(radiourl);
                playing = true;
                statusLabel.Text = $"Status: Playing {selectedStation}";

                notificationManager.SendNotification("Now Playing", $"{selectedStation} is playing", notificationId);
                App.AppData = new NotificationData
                {
                    SelectedStation = selectedStation,
                    NotificationId = notificationId
                };
            }
            else
            {
                Debug.WriteLine("audioPlayer is null");
                Debug.WriteLine(audioPlayer);
            }
        }
    }

    private void StopButton_Clicked(object sender, EventArgs e)
    {
        audioPlayer.StopAudio();
        playing = false;
        statusLabel.Text = "Status: Stopped";

        notificationManager.CancelNotification(notificationId);
    }
}

