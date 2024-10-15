using JLSolarPanels.Pages;

namespace JLSolarPanels;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        MainPage = new NavigationPage(new SolarPanelsPage());
    }
}