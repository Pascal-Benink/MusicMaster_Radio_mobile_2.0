using System;
namespace MusicMaster_Radio_mobile_2._0.Interfaces
{
	public interface INotificationManager
	{
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, int notificationId);
        void CancelNotification(int notificationId);
    }
}

