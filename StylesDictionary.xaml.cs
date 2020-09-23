using LandConquest.Entities;
using LandConquest.Forms;
using LandConquest.Models;
using System;
using System.Collections.Generic;
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

                //greendend
                ///connection = new SqlConnection(@"Data Source=DESKTOP-3S39QPO\SQLEXPRESS;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
                //user-pass 
                connection = new SqlConnection(@"Data Source=DESKTOP-EQUN2R7;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
                //glandeil
                //connection = new SqlConnection(@"Data Source=DESKTOP-P19BATV\SQLEXPRESS;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
                //Kirill-Spesivtsev
                //connection = new SqlConnection(@"Data Source=KIR\SQLEXPRESS;Initial Catalog=LandConquestDB;Integrated Security=True;Pooling=False");
                //online connection link
                //connection = new SqlConnection(@"workstation id=LandConquest1.mssql.somee.com;packet size=4096;user id=LandConquest_SQLLogin_1;pwd=3xlofdewbj;data source=LandConquest1.mssql.somee.com;persist security info=False;initial catalog=LandConquest1");

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