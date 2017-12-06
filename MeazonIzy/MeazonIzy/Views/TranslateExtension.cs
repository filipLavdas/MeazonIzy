using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Strings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeazonIzy
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
       
        const string ResourceId = "MeazonIzy.Strings.AppResources";

        public TranslateExtension()
        {
            ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
           

        }
        public string Text { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId
                                , typeof(TranslateExtension).GetTypeInfo().Assembly);
            var translation = resmgr.GetString(Text, ci);

            if (translation == null)
            {

                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER

            }
            return translation;

        }
    }
}
