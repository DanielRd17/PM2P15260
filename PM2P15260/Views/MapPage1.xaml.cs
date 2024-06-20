using PM2P15260.Controllers;
using PM2P15260.Models;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace PM2P15260.Views;

public partial class MapPage1 : ContentPage
{
    private readonly DatabaseService _databaseService;
    private Sitios _sitio;
    public MapPage1(Sitios sitio, DatabaseService databaseService)
	{
		InitializeComponent();

        _databaseService = databaseService;

        if (sitio != null)
        {
            _sitio = sitio;
            InitializeMap();
        }

    }

    private async void InitializeMap()
    {
        try
        {           

            if (_sitio != null)
            {
                double LatitudeEntry = 3;
                double LongitudeEntry = 3;

                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location(LatitudeEntry, LongitudeEntry),
                    Distance.FromMiles(1)));

                MyMap.Pins.Add(new Pin
                {
                    Label = "Mi ubicación",
                    Address = "Ubicación actual",
                    Type = PinType.Place,
                    Location = new Location(LatitudeEntry, LongitudeEntry)
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No es posible mostrar ubicacion del sitio: {ex.Message}", "OK");
        }

    }
}