using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Maui.Audio;
using System.Timers;

namespace JLSolarPanels.Pages;

public partial class SolarPanelsPage : ContentPage
{
    private bool isDarkMode = true;
    
    private IAudioPlayer jour = AudioManager.Current.CreatePlayer("jour.mp3");
    private IAudioPlayer nuit = AudioManager.Current.CreatePlayer("nuit.mp3");
    private IAudioPlayer spam = AudioManager.Current.CreatePlayer("spam.mp3");
    
    private int spamLimit = 10;
    private int spamTimerInterval = 10000;
    private int spamCount = 0;
    private System.Timers.Timer spamTimer;
    private bool isTimerRunning = false;
    public SolarPanelsPage()
    {
        InitializeComponent();
        
        GetOrientation();
        GetDirection();
        GetLocation();
        
        Application.Current.UserAppTheme = AppTheme.Dark;
    }
    
    void ThemeToggleClicked(object sender, EventArgs e)
    {
        UpdateTimer();
        UpdateTheme();
    }
    
    async void UpdateTheme()
    {
        isDarkMode = !isDarkMode;
        await this.FadeTo(0, 250);
        if (isDarkMode)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            ThemeToggle.IconImageSource = "sun.png";
            nuit.Play();
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            ThemeToggle.IconImageSource = "moon.png";
            jour.Play();
        }
        await this.FadeTo(1, 250);
    }
    
    void UpdateTimer()
    {
        if (!isTimerRunning)
            StartSpamTimer();
        
        spamCount++;
        
        if (spamCount >= spamLimit)
            spam.Play();
    }
    
    void StartSpamTimer()
    {
        spamTimer = new System.Timers.Timer(spamTimerInterval)
        {
            AutoReset = false // Pas de redémarrage automatique
        };
        spamTimer.Elapsed += SpamTimerElapsed; // Événement déclenché à la fin du timer
        spamTimer.Start();
        isTimerRunning = true;
    }

    void SpamTimerElapsed(Object source, ElapsedEventArgs e)
    {
        spamTimer.Dispose(); // Libère les ressources et arrête le timer (pas besoin de Stop())
        spamCount = 0;
        isTimerRunning = false;
    }

    void GetOrientation()
    {
        if (Accelerometer.IsMonitoring)
        {
            HorizontalLabel.Text = "Horizontal : Indisponible";
            VerticalLabel.Text = "Vertical : Indisponible";
            return;
        }
        
        Accelerometer.ReadingChanged += (sender, e) =>
        {
            HorizontalLabel.Text = $"Horizontal : {Math.Round(e.Reading.Acceleration.Z, 2)}";
            VerticalLabel.Text = $"Vertical : {Math.Round(e.Reading.Acceleration.X, 2)}";
        };
        
        Accelerometer.Start(SensorSpeed.UI);
    }
    
    void GetDirection()
    {
        if (Compass.IsMonitoring)
        {
            HeadingLabel.Text = "Direction : Indisponible";
            return;
        }

        Compass.ReadingChanged += (s, e) =>
        {
            int direction = (int)Math.Round(e.Reading.HeadingMagneticNorth);
            HeadingLabel.Text = $"Direction : {direction}°";
            switch (direction)
            {
                case <= 25:
                    DirectionLabel.Text = "Rose des vents : Nord Nord-Est";
                    break;
                case <= 45:
                    DirectionLabel.Text = "Rose des vents : Nord-Est";
                    break;
                case <= 67:
                    DirectionLabel.Text = "Rose des vents : Est Nord-Est";
                    break;
                case <= 90:
                    DirectionLabel.Text = "Rose des vents : Est";
                    break;
                case <= 112:
                    DirectionLabel.Text = "Rose des vents : Est Sud-Est";
                    break;
                case <= 135:
                    DirectionLabel.Text = "Rose des vents : Sud-Est";
                    break;
                case <= 157:
                    DirectionLabel.Text = "Rose des vents : Sud Sud-Est";
                    break;
                case <= 180:
                    DirectionLabel.Text = "Rose des vents : Sud";
                    break;
                case <= 202:
                    DirectionLabel.Text = "Rose des vents : Sud Sud-Ouest";
                    break;
                case <= 225:
                    DirectionLabel.Text = "Rose des vents : Sud-Ouest";
                    break;
                case <= 247:
                    DirectionLabel.Text = "Rose des vents : Ouest Sud-Ouest";
                    break;
                case <= 270:
                    DirectionLabel.Text = "Rose des vents : Ouest";
                    break;
                case <= 292:
                    DirectionLabel.Text = "Rose des vents : Ouest Nord-Ouest";
                    break;
                case <= 315:
                    DirectionLabel.Text = "Rose des vents : Nord-Ouest";
                    break;
                case <= 337:
                    DirectionLabel.Text = "Rose des vents : Nord Nord-Ouest";
                    break;
                case <= 360:
                    DirectionLabel.Text = "Rose des vents : Nord";
                    break;
            }
        };

        Compass.Start(SensorSpeed.UI);
    }

    async void GetLocation()
    {
        try
        {
            LatitudeLabel.Text = "Latitude : Recherche...";
            LongitudeLabel.Text = "Longitude : Recherche...";
            AltitudeLabel.Text = "Altitude : Recherche...";
            while (true)
            {
                var location = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.High,
                    Timeout = TimeSpan.FromSeconds(5)
                });

                if (location != null)
                {
                    LatitudeLabel.Text = $"Latitude : {location.Latitude}";
                    LongitudeLabel.Text = $"Longitude : {location.Longitude}";
                    AltitudeLabel.Text = $"Altitude : {Math.Round(float.Parse(location.Altitude.ToString()), 7)}";
                }

                await Task.Delay(5000);
            }
        }
        catch (Exception ex)
        {
            LatitudeLabel.Text = "Latitude : Indisponible";
            LongitudeLabel.Text = "Longitude : Indisponible";
            AltitudeLabel.Text = "Altitude : Indisponible";
        }
    }
}