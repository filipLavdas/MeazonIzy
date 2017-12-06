using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            Title = L10n.Localize("AboutTitle");
        }

        private async void TapGestureRecognizer_OnTapped_Faq(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FaqPage());

        }

        private async void OnTapped_Privacy(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Privacy_Policy_Page());
        } 
    }
}
