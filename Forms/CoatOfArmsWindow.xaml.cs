using LandConquestDB.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LandConquest.Forms
{
    /// <summary>
    /// Логика взаимодействия для CoatOfArmsWindow.xaml
    /// </summary>
    public partial class CoatOfArmsWindow : Window
    {   
        Player player;
        Image shield;
        Image pattern = new Image();
        Image atribute = new Image();
        Image shieldMask = new Image();
        Image selectedImage;
        int currentMenu = 0;

        public CoatOfArmsWindow(Player _player)
        {
            player = _player;
            InitializeComponent();
        }
        private void Img_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (currentMenu == 1) //shield form
            {
                if (!CoatOfArmsCanvas.Children.Contains(shield))
                {
                    shield = new Image();
                    Image buffer = (Image)sender;
                    shield.Source = buffer.Source.Clone();
                    logBox.Text = Convert.ToString(buffer.Source);

                    shield.Width = 250;
                    shield.Height = 250;

                    selectedImage = shield;

                    CoatOfArmsCanvas.Children.Add(shield);

                    Image shieldLayer = new Image();
                    shieldLayer.Source = buffer.Source.Clone();
                    shieldLayer.Height = 40;
                    shieldLayer.Width = 40;
                    CoatOfArmsLayersList.Items.Insert(0, shieldLayer);

                    VisualBrush b = new VisualBrush();
                    b.Visual = shield;
                    b.Stretch = Stretch.None;
                    CanvasContainer.OpacityMask = b;
                }
            }
            if (currentMenu == 2) //pattern
            {

                if (!CoatOfArmsCanvas.Children.Contains(pattern))
                {

                    Image buffer = (Image)sender;
                    pattern.Source = buffer.Source.Clone();
                    pattern.Width = 160;
                    pattern.Height = 160;

                    CoatOfArmsCanvas.Children.Add(pattern);

                    Image patternLayer = new Image();
                    patternLayer.Source = buffer.Source.Clone();
                    patternLayer.Height = 30;
                    patternLayer.Width = 30;
                    CoatOfArmsLayersList.Items.Insert(0, patternLayer);
                    selectedImage = pattern;
                    
                    pattern.MouseDown += Img_MouseDown;
                }
                
                pattern.AllowDrop = true;

                DataObject data = new DataObject(typeof(ImageSource), pattern.Source);
                DragDrop.DoDragDrop(pattern, data, DragDropEffects.Move);
            }
            if (currentMenu == 3) //additional items
            {
                if ( !CoatOfArmsCanvas.Children.Contains(atribute))
                {
                    Image buffer = (Image)sender;
                    atribute.Source = buffer.Source.Clone();

                    CoatOfArmsCanvas.Children.Add(atribute);
                    atribute.Width = 70;
                    atribute.Height = 70;
                    selectedImage = atribute;
                    atribute.AllowDrop = true;

                    Image atrLayer = new Image();
                    atrLayer.Source = buffer.Source.Clone();
                    atrLayer.Height = 30;
                    atrLayer.Width = 30;
                    CoatOfArmsLayersList.Items.Insert(0, atrLayer);
                    
                    atribute.MouseDown += Img_MouseDown;
                }
                DataObject data = new DataObject(typeof(ImageSource), atribute.Source);
                DragDrop.DoDragDrop(atribute, data, DragDropEffects.Move);
            }
        }

     

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            if (e.GetPosition(CoatOfArmsCanvas).X > 0 
             && e.GetPosition(CoatOfArmsCanvas).X < CoatOfArmsCanvas.Width 
             && e.GetPosition(CoatOfArmsCanvas).Y > 0
             && e.GetPosition(CoatOfArmsCanvas).Y < CoatOfArmsCanvas.Height)
            {

                Canvas.SetLeft(selectedImage, e.GetPosition(CoatOfArmsCanvas).X - selectedImage.Width / 2);
                Canvas.SetTop(selectedImage, e.GetPosition(CoatOfArmsCanvas).Y - selectedImage.Height / 2);
            }
        }


        

        private void FormsGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void buttonForm_Click(object sender, RoutedEventArgs e)
        {

            currentMenu = 1;
            
            form1.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/333.png", UriKind.Relative));
            form2.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/shield2.jpg", UriKind.Relative));
        }


        private void buttonPattern_Click(object sender, RoutedEventArgs e)
        {
            currentMenu = 2;
            form1.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/shield2/cross_red.png", UriKind.Relative));
            form2.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/shield1/circle_red.png", UriKind.Relative));
        }

       

        private void CoatOfArmsLayersList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CoatOfArmsLayersList.SelectedItem != null)
            {
                Image layerImage = (Image)CoatOfArmsLayersList.Items[CoatOfArmsLayersList.SelectedIndex];
                if(pattern!=null)
                if (layerImage.Source.ToString().Equals(pattern.Source.ToString()))
                {
                    selectedImage = pattern;
                    //logBox.Text = "equals";
                  
                    buttonPattern_Click(layerImage,new RoutedEventArgs());
                }
                if (atribute != null)
                if (layerImage.Source.ToString().Equals(atribute.Source.ToString()))
                {
                    selectedImage = atribute;
                    //logBox.Text = "equals";
                    buttonAtrs_Click(layerImage, new RoutedEventArgs());
                }
                if (shield != null)
                if (layerImage.Source.ToString().Equals(shield.Source.ToString()))
                {
                        selectedImage = shield;
                        //logBox.Text = "equals";
                        buttonForm_Click(layerImage, new RoutedEventArgs());
                }

            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            logBox.Text = "Saving...";
            string path = "/CoatOfArms.png";
            FileStream fs = new FileStream(path, FileMode.Create);
            double w = 300;
            double h = 300;
            double dpi = 300;
            double scale = dpi / 96;

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)(w * scale), (int)(h * scale), dpi, dpi,PixelFormats.Pbgra32);
            bmp.Render(CoatOfArmsCanvas);
            BitmapEncoder encoder = new PngBitmapEncoder();
            
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(fs);
            logBox.Text = "File saved: " + fs.Name;
            fs.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (CoatOfArmsCanvas.Children.Contains(selectedImage))
            {
                CoatOfArmsCanvas.Children.Remove(selectedImage);
                CoatOfArmsLayersList.Items.Remove(CoatOfArmsLayersList.SelectedItem);
            }
          
        }

        private void buttonAtrs_Click(object sender, RoutedEventArgs e)
        {
            currentMenu = 3;
            form1.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/atributes/tower.png", UriKind.Relative));
            form2.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/atributes/apple.png", UriKind.Relative));
        }

    }
}
