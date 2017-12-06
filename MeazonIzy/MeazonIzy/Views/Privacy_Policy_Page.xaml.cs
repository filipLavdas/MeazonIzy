using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Resources;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class Privacy_Policy_Page : ContentPage
    {
        public Privacy_Policy_Page()
        {
            InitializeComponent();
            Title = L10n.Localize("PrivacyLabel");
            var resource=new PrivacyResources();
            PrivacyLabel.Text = resource.ReturnPrivacyText();
        }
    }
}
