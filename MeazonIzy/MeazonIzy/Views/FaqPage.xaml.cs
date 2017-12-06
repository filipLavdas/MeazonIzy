using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeazonIzy.Localization;
using Xamarin.Forms;

namespace MeazonIzy.Views
{
    public partial class FaqPage : ContentPage
    {
        public FaqPage()
        {
            InitializeComponent();
            Title = L10n.Localize("FaqFull");
            IntializeFaq();

        }

        private void IntializeFaq()
        {

            Q1.Text = L10n.Localize("q1");
            A1.Text = L10n.Localize("a1");
            Q2.Text = L10n.Localize("q2");


            A2.Text = L10n.Localize("a2");


            Q3.Text = L10n.Localize("q3");

            A3.Text = L10n.Localize("a3");

            Q4.Text = L10n.Localize("q4");
            A4.Text = L10n.Localize("a4");
            Q5.Text = L10n.Localize("q5");
            A5.Text = L10n.Localize("a5");
            Q6.Text = L10n.Localize("q6");
            A6.Text = L10n.Localize("a6");
            Q7.Text = L10n.Localize("q7");
            A7.Text = L10n.Localize("a7");
            Q8.Text = L10n.Localize("q8");
            A8.Text = L10n.Localize("a8");
            Q9.Text = L10n.Localize("q9");
            A9.Text = L10n.Localize("a9");
            Q10.Text = L10n.Localize("q10");
            A10.Text = L10n.Localize("a10");



        }


        private async void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            var grid = sender as Grid;

            var childrens = grid.Children;
            var l1 = childrens[0] as Label;
            var l2 = childrens[1] as Label;


            double tempHeight;
            var gridRows = grid.RowDefinitions;
            var h = gridRows[1].Height;

            if (l2.Opacity == 1)
            {
                l2.FadeTo(0, 1000, Easing.Linear);
                l2.Opacity = 0;
               
                grid.HeightRequest = grid.Height / 2;
                gridRows[1].Height = new GridLength(0, GridUnitType.Absolute);
               

                await l2.TranslateTo(0, -l2.Height, 1000, Easing.SinIn);
                //await l2.LayoutTo(new Rectangle(0, -(l2.Height),l1.Width,l1.Height),1000,Easing.Linear);

                //counter[0].Height = new GridLength(1, GridUnitType.Star);

            }
            else
            {
              
                l2.Opacity = 1;
                grid.HeightRequest = grid.Height * 2;

                gridRows[1].Height = new GridLength(1, GridUnitType.Star);
                tempHeight = grid.HeightRequest/2;

                l2.FadeTo(1.0, 1000, Easing.Linear);
  
                await l2.TranslateTo(0, tempHeight, 1000, Easing.SinIn);

                //await l2.LayoutTo(new Rectangle(0, l2.Height, l1.Width, l1.Height), 1000, Easing.SinIn);


                //counter[0].Height = new GridLength(0, GridUnitType.Absolute);

            }

        }
    }
}
