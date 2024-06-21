using PM2P15260.Controllers;
using PM2P15260.Models;
using System.Collections.ObjectModel;

namespace PM2P15260.Views;

public partial class SitesPage1 : ContentPage
{
    private readonly DatabaseService _databaseService;
    private ObservableCollection<Sitios> _sitios;
    private ObservableCollection<Sitios> _filteredSitios;
    public SitesPage1()
    {
        InitializeComponent();
        _databaseService = new DatabaseService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DBSitios.db3"));
   //     LoadSitios();
        
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        var sitios = await _databaseService.GetSitiosAsync();
        _sitios = new ObservableCollection<Sitios>(sitios);
        _filteredSitios = new ObservableCollection<Sitios>(_sitios);
        SitiosCollectionView.ItemsSource = _filteredSitios;
    }

//    private async void LoadSitios()
//    {
//        var sitios = await _databaseService.GetSitiosAsync();
//        _sitios = new ObservableCollection<Sitios>(sitios);
//        _filteredSitios = new ObservableCollection<Sitios>(_sitios);
//        SitiosCollectionView.ItemsSource = _filteredSitios;
//    }

    private void OnMenuClicked(object sender, EventArgs e)
    {

    }

    private void OnAgregarClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EditPage1(null, _databaseService));
    }

    private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
    {
        var searchTerm = e.NewTextValue.ToLower();
        _filteredSitios.Clear();

        foreach (var sitios in _sitios)
        {
            if (sitios.Descripcion.ToLower().Contains(searchTerm) || sitios.Latitude.ToLower().Contains(searchTerm))
            {
                _filteredSitios.Add(sitios);
            }
        }

    }

    private void OnAutorSelected(object sender, SelectionChangedEventArgs e)
    {

    }

    private void OnAyudaClicked(object sender, EventArgs e)
    {

    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new EditPage1(null, _databaseService));
    }

    private void OnMapClicked(object sender, EventArgs e)
    {
        //Navigation.PushAsync(new MapPage1());
    }

    private void OnSitiosSelected(object sender, SelectionChangedEventArgs e)
    {
  
    }

    private void OnEditSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var sitio = swipeItem.BindingContext as Sitios;
        if (sitio != null)
        {
            Navigation.PushAsync(new EditPage1(sitio,_databaseService));
        }
    }

    private async void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var sitio = swipeItem.BindingContext as Sitios;
        if (sitio != null)
        {
            bool confirm = await DisplayAlert("Eliminar", $"Est'seguro de eliminar a: {sitio.Descripcion} {sitio.Latitude}?", "Si", "No");
            if (confirm)
            {
                await _databaseService.DeleteSitioAsync(sitio);
                _sitios.Remove(sitio);
            }
        }
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {

    }

    private void OnMapSwipeItemInvoked(object sender, EventArgs e)
    {
        var swipeItem = sender as SwipeItem;
        var sitio = swipeItem.BindingContext as Sitios;

        double latitud;
        double longitud;

        bool isLatitudValid = double.TryParse(sitio.Latitude, out latitud);
        bool isLongitudValid = double.TryParse(sitio.Longitude, out longitud);

        if (!isLatitudValid || !isLongitudValid)
        {
            DisplayAlert("Mensaje", "Error al convertir las coordenadas", "ok");
        }
        else
        {
            var page = new Views.MapPage1(latitud,longitud);
            Navigation.PushAsync(page);
           // Navigation.PushAsync(new MapPage1(latitud,longitud));
            //Navigation.PushAsync(page);
        }
    }
}