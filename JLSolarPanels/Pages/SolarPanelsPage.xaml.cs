using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLSolarPanels.Pages;

public partial class SolarPanelsPage : ContentPage
{
    public SolarPanelsPage()
    {
        InitializeComponent();
        GetOrientation();
        GetDirection();
        GetLocation();
    }
    
    void GetOrientation()
    {
        if (Accelerometer.IsMonitoring)
        {
            HorizontalLabel.Text = "Horizontal : Indisponible";
            VerticalLabel.Text = "Vertical : Indisponible";
            return;
        }

        Accelerometer.ReadingChanged += (s, e) =>
        {
            HorizontalLabel.Text = $"Horizontal (Gauche - Droite) : {Math.Round(e.Reading.Acceleration.Z, 2)}";
            VerticalLabel.Text = $"Vertical (Avant - Arrière) : {Math.Round(e.Reading.Acceleration.X, 2)}";
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
        while (true)
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.High,
                Timeout = TimeSpan.FromSeconds(30)
            });
        
            if (location != null)
            {
                LatitudeLabel.Text = $"Latitude : {location.Latitude}";
                LongitudeLabel.Text = $"Longitude : {location.Longitude}";
                AltitudeLabel.Text = $"Altitude : {location.Altitude}";
            }
        
            await Task.Delay(1000);
        }
    }
    
    // Barometer
}