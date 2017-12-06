using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using MeazonIzy.Models;
using Xamarin.Forms;

namespace MeazonIzy.Resources
{
  public  class MenuResources
    {
        public ObservableCollection<MainMenuItems> GetNames()
        {


            ObservableCollection<MainMenuItems> Namelist = new ObservableCollection<MainMenuItems>
          {
              new MainMenuItems
              {
                  Name=L10n.Localize("DevicesTitle"),Color = Color.Purple,
                Image = "https://izy.meazon.com/app/content/img/icons/PlugMetro.png"
        },
              new MainMenuItems
              {
                  Name=L10n.Localize("ScheduleTitle"),Color = Color.Navy,
                   Image ="https://izy.meazon.com/app/content/img/icons/ScheduleMetro.png"
              },
              new MainMenuItems
              {
                  Name=L10n.Localize("UsersTitle"),Color = Color.Green,
                   Image = "https://izy.meazon.com/app/content/img/icons/UsersMetro.png"
              },
              new MainMenuItems
              {
                  Name=L10n.Localize("SettingsTitle"),Color = Color.Olive,
                   Image = "https://izy.meazon.com/app/content/img/icons/SettingsMetro.png"
              },
              new MainMenuItems
              {
                  Name=L10n.Localize("AboutTitle"),Color = Color.Maroon,
                   Image = "https://izy.meazon.com/app/content/img/icons/about-light.png"
              }

          };
            return Namelist;

        }

    }
}
