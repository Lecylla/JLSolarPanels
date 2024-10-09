using JLSolarPanels.Pages;

namespace JLSolarPanels;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new SolarPanelsPage());
    }
    
    protected override void OnStart()
    {
        // Handle when your app starts
        RequestPermissions();
    }

    private async void RequestPermissions()
    {
        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            // Handle permission denied
        }
    }
}