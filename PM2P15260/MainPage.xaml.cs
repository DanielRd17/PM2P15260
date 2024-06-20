using PM2P15260.Views;

namespace PM2P15260
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {

            InitializeComponent();
        }

        private void btnPage1_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SitesPage1());
            if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                IsPresented = false;
        }

        private void btnPage2_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new SitesPage1());
            if (!((IFlyoutPageController)this).ShouldShowSplitMode)
                IsPresented = false;
        }

    }
}
