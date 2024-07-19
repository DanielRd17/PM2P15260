using PM2P15260.Controllers;
using PM2P15260.Models;

namespace PM2P15260.Views;

public partial class EditPage1 : ContentPage
{
    FileResult photo;
    private string _photoBase64;
    private readonly DatabaseService _databaseService;
    private Sitios _sitio;
    String Base64 = String.Empty;

    public EditPage1(Sitios sitio, DatabaseService databaseService)
    {
        InitializeComponent();
        _databaseService = databaseService;

        if (sitio != null)
        {
            _sitio = sitio;
            DescripcionEntry.Text = sitio.Descripcion;
            LatitudeEntry.Text = sitio.Latitude;
            LongitudeEntry.Text = sitio.Longitude;
            _photoBase64 = _sitio.Foto;
            FotoImageEntry.Source = ImageSource.FromStream(() => new MemoryStream(Convert.FromBase64String(_photoBase64)));
            //              EliminarButton.IsVisible = true;
        }

    }
    private async void OnAudioRecordClicked(object sender, EventArgs e)
    { }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await RequestLocationPermissionAsync();
        var location = await Geolocation.GetLastKnownLocationAsync();
        if (location != null) 
        {    
            LatitudeEntry.Text = location.Latitude.ToString();
            LongitudeEntry.Text = location.Longitude.ToString();
        }

    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        if (_sitio != null)
        {
            await _databaseService.DeleteSitioAsync(_sitio);
            await Navigation.PopAsync();
        }
    }

    private async void btnfoto_Clicked(object sender, EventArgs e)
    {
        try
        {
            photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                string Localizacion = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream imagenlocal = File.OpenWrite(Localizacion);

                FotoImageEntry.Source = ImageSource.FromStream(() => photo.OpenReadAsync().Result);

                await sourceStream.CopyToAsync(imagenlocal);

            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }
    }


    private void OnDeleteClicked(object sender, EventArgs e)
    {

    }

    private void OnUpdateClicked(object sender, EventArgs e)
    {

    }

    public String GetImage64()
    {
        String Base64 = String.Empty;

        if (photo != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Stream stream = photo.OpenReadAsync().Result;
                stream.CopyTo(ms);
                byte[] data = ms.ToArray();

                Base64 = Convert.ToBase64String(data);
                return Base64;
            }
        }

        return Base64;
    }


    public byte[] GetImageArray()
    {
        byte[] data = new Byte[] { };

        if (photo != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Stream stream = photo.OpenReadAsync().Result;
                stream.CopyTo(ms);
                data = ms.ToArray();

                return data;
            }
        }

        return data;
    }
    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_sitio == null)
        {
            _sitio = new Sitios
            {
                Descripcion = DescripcionEntry.Text,
                Latitude = LatitudeEntry.Text,
                Longitude = LongitudeEntry.Text,
                Foto = GetImage64()
            };
        }
        else
        {
            _sitio.Descripcion = DescripcionEntry.Text;
            _sitio.Latitude = LatitudeEntry.Text;
            _sitio.Longitude = LongitudeEntry.Text;
            _sitio.Foto = GetImage64();
        }
        await _databaseService.SaveSitioAsync(_sitio);
        Navigation.PopAsync();
        clearView();
    }

    private void clearView()
    {
        DescripcionEntry.Text = string.Empty;
        LatitudeEntry.Text = string.Empty;
        LongitudeEntry.Text = string.Empty;
        FotoImageEntry=null;

    }

    //Permisos de localizaci�n
    private async Task RequestLocationPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        if (status != PermissionStatus.Granted)
        {
            await DisplayAlert("Permiso", "No se pudo obtener el permiso de localización", "OK");
        }
    }
}