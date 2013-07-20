using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Microsoft.Phone.Shell;

namespace Quran360.Helpers
{
    public static class LiveTileManager
    {

        public static void UpdateAllLiveTiles()
        {
            LiveTileHelper.CleanupUnpinnedTilesResources();

            /*
            //Calculate Savings, Incomes, Expenses Amount
            App.ViewModel.UpdateSumStats();

            try{
            //Update Savings Tile
                UpdateLiveTile("Savings", "Total Savings", "",
                    "/Views/MainPage.xaml", "savingstile_173x173.png", "tile_173x173_back.png");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            try{
                //Update Incomes Tile
                UpdateLiveTile("Incomes", "Total Incomes", "",
                    "/Views/AllIncomes.xaml", "incomestile_173x173.png", "tile_173x173_back.png");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            try{
                //Update Expenses Tile
                UpdateLiveTile("Expenses", "Total Expenses", "",
                "/Views/AllExpenses.xaml", "expensestile_173x173.png", "tile_173x173_back.png");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }

            //!ONHOLD: Update Category Tiles

            */
        }


        public static void CreateLiveTile(string Title, string BackTitle, string BackContent, string PageUrl, string frontImg, string backImg)
        {
            RadExtendedTileData extendedData = new RadExtendedTileData();

            //extendedData.VisualElement = LayoutRoot;
            //extendedData.BackgroundImage = new Uri("appdata:Images/tile_173x173.png");
            extendedData.BackgroundImage = new Uri("appdata:Images/" + frontImg);
            extendedData.Title = Title;
            //extendedData.Count = 5000;
            extendedData.BackTitle = BackTitle;
            //extendedData.BackBackgroundImage = new Uri("appdata:Images/tile_173x173_back.png");
            extendedData.BackBackgroundImage = new Uri("appdata:Images/" + backImg);
            extendedData.BackContent = BackContent;
            //this will create a tile looking exactly as your page if it is placed inside a layout panel named LayoutRoot

            LiveTileHelper.CreateOrUpdateTile(extendedData, new Uri(PageUrl, UriKind.RelativeOrAbsolute));
        }

        public static void UpdateLiveTile(string Title, string BackTitle, string BackContent, string PageUrl, string frontImg, string backImg)
        {
            RadExtendedTileData extendedData = new RadExtendedTileData();

            //extendedData.VisualElement = LayoutRoot;
            //extendedData.BackgroundImage = new Uri("appdata:Images/tile_173x173.png");
            extendedData.BackgroundImage = new Uri("appdata:Images/" + frontImg);
            extendedData.Title = Title;
            //extendedData.Count = 5000;
            extendedData.BackTitle = BackTitle;
            //extendedData.BackBackgroundImage = new Uri("appdata:Images/tile_173x173.png");
            extendedData.BackBackgroundImage = new Uri("appdata:Images/" + backImg);
            extendedData.BackContent = BackContent;
            //this will create a tile looking exactly as your page if it is placed inside a layout panel named LayoutRoot

            LiveTileHelper.UpdateTile(LiveTileHelper.GetTile(new Uri(PageUrl, UriKind.RelativeOrAbsolute)), extendedData, false);
        }


    }
}
