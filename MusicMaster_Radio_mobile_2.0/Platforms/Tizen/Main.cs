using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace MusicMaster_Radio_mobile_2._0;

class Program : MauiApplication
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

	static void Main(string[] args)
	{
		var app = new Program();
		app.Run(args);
	}
}
