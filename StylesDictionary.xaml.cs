using LandConquest.Entities;
using LandConquest.Forms;
using LandConquest.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LandConquest
{
    public partial class StylesDictionary : ResourceDictionary
    {
        SqlConnection connection;
        List<Land> lands;

        Color colorOfResource1 = Color.FromRgb(255, 255, 255);
        Color colorOfResource2 = Color.FromRgb(0, 0, 0);
        public void ViewboxLoadedHandler(object sender, EventArgs e)
        {
            try
            {
                LandModel landModel = new LandModel();

                string cdb = ConfigurationManager.ConnectionStrings["greendend2"].ConnectionString;
                connection = new SqlConnection(cdb);
                connection.Open();

                const int landsCount = 11;

                lands = new List<Land>();
                for (int i = 0; i < landsCount; i++)
                {
                    lands.Add(new Land());
                }

                lands = landModel.GetLandsInfo(lands, connection);
                connection.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }
        public void PathLoadedHandler(object sender, EventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;

                Land land = new Land();
                land = lands.ElementAt(Convert.ToInt32(senderPath.Name.Replace("Land", "")) - 1);

                switch (land.ResourceType1)
                {
                    case 4:
                        {
                            colorOfResource1 = Color.FromRgb(216, 191, 216);
                            break;
                        }
                    case 5:
                        {
                            colorOfResource1 = Color.FromRgb(255, 215, 0);
                            break;
                        }
                    case 6:
                        {
                            colorOfResource1 = Color.FromRgb(255, 140, 0);
                            break;
                        }
                    case 7:
                        {
                            colorOfResource1 = Color.FromRgb(127, 255, 212);
                            break;
                        }
                    case 8:
                        {
                            colorOfResource1 = Color.FromRgb(139, 69, 19);
                            break;
                        }
                }

                switch (land.ResourceType2)
                {
                    case 4:
                        {
                            colorOfResource2 = Color.FromRgb(216, 191, 216);
                            break;
                        }
                    case 5:
                        {
                            colorOfResource2 = Color.FromRgb(255, 215, 0);
                            break;
                        }
                    case 6:
                        {
                            colorOfResource2 = Color.FromRgb(255, 140, 0);
                            break;
                        }
                    case 7:
                        {
                            colorOfResource2 = Color.FromRgb(127, 255, 212);
                            break;
                        }
                    case 8:
                        {
                            colorOfResource2 = Color.FromRgb(139, 69, 19);
                            break;
                        }
                }


                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                myLinearGradientBrush.StartPoint = new Point(0, 0.5);
                myLinearGradientBrush.EndPoint = new Point(0.5, 1);
                myLinearGradientBrush.GradientStops.Add(new GradientStop(colorOfResource1, 0.45));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(colorOfResource2, 0.55));

                senderPath.Fill = myLinearGradientBrush;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        public void PathEnterHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;

                GradientStopCollection color = ((LinearGradientBrush)senderPath.Fill).GradientStops;

                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                myLinearGradientBrush.StartPoint = new Point(0, 0.5);
                myLinearGradientBrush.EndPoint = new Point(0.5, 1);
                myLinearGradientBrush.GradientStops.Add(new GradientStop(color[1].Color, 0.45));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(color[0].Color, 0.55));

                senderPath.Fill = myLinearGradientBrush;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }

        public void PathLeaveHandler(object sender, RoutedEventArgs e)
        {
            try
            {
                Path senderPath = (Path)sender;


                GradientStopCollection color = ((LinearGradientBrush)senderPath.Fill).GradientStops;

                LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush();
                myLinearGradientBrush.StartPoint = new Point(0, 0.5);
                myLinearGradientBrush.EndPoint = new Point(0.5, 1);
                myLinearGradientBrush.GradientStops.Add(new GradientStop(color[1].Color, 0.45));
                myLinearGradientBrush.GradientStops.Add(new GradientStop(color[0].Color, 0.55));

                senderPath.Fill = myLinearGradientBrush;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButton.OK);
            }
        }
    }
}