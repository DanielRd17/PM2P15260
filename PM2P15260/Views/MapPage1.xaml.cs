using PM2P15260.Controllers;
using PM2P15260.Models;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
namespace PM2P15260.Views;

public partial class MapPage1 : ContentPage
{
    double LatitudeEntry, LongitudeEntry;
    public MapPage1(double xlatitude, double xlongitude)
	{
        LatitudeEntry = xlatitude;
        LongitudeEntry= xlongitude;
		InitializeComponent();
        InitializeMap();
    }

    private async void InitializeMap()
    {
        try
        {           

            if (LatitudeEntry != null)
            {
                // Centro del mapa en una ubicación específica
                map.MoveToRegion(MapSpan.FromCenterAndRadius(
                    new Location(LatitudeEntry, LongitudeEntry), Distance.FromMiles(10)));


               map.MoveToRegion(MapSpan.FromCenterAndRadius(
                   new Location(LatitudeEntry, LongitudeEntry),
                   Distance.FromMiles(1)));

                map.Pins.Add(new Pin
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

    private async void OnCaptureAndShareButtonClicked(object sender, EventArgs e)
    {
        var screenshot = await CaptureMapScreenshotAsync(map);

        if (screenshot != null)
        {
            var file = await SaveScreenshotAsync(screenshot);
            await ShareScreenshotAsync(file);
        }
    }

    private async Task<byte[]> CaptureMapScreenshotAsync(Microsoft.Maui.Controls.Maps.Map map)
    {
        // Capturar la pantalla del mapa
        var screenshot = await map.CaptureAsync();
        using (var stream = new MemoryStream())
        {
            await screenshot.CopyToAsync(stream);
            return stream.ToArray();
        }
    }

    private async Task<FileResult> SaveScreenshotAsync(byte[] screenshot)
    {
        // Guardar la captura de pantalla en un archivo temporal
        var fileName = Path.Combine(FileSystem.CacheDirectory, "map_screenshot.png");
        await File.WriteAllBytesAsync(fileName, screenshot);
        return new FileResult(fileName);
    }

    private async Task ShareScreenshotAsync(FileResult file)
    {
        // Compartir la captura de pantalla usando el plugin de compartir de .NET MAUI
        await Share.RequestAsync(new ShareFileRequest
        {
            Title = "Compartir captura de pantalla del mapa",
            File = new ShareFile(file.FullPath)
        });
    }


    private void OnCaptureMapClicked(object sender, EventArgs e)
    {

    }
}