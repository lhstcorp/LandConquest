using LandConquestDB.Entities;
using System;
using System.Collections.Generic;

using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        Image pattern;
        Image atribute;
        List<Image> addedElementList = new List<Image>();
        Image selectedImage;
        int currentMenu = 0;
        Boolean itemSelected;
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

                    shield.Width = 200;
                    shield.Height = 200;

                    addedElementList.Add(shield);

                    selectedImage = shield;
                    AllowDrop = true;
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

                if (!CoatOfArmsCanvas.Children.Contains((Image)sender))
                {

                    pattern = new Image();
                    Image buffer = (Image)sender;
                    pattern.Source = buffer.Source.Clone();
                    pattern.Opacity = 0.8;
                    pattern.Width = 500;
                    pattern.Height = 500;

                    VisualBrush colorBrush = new VisualBrush();



                    //                    Image myImage3 = new Image();
                    //System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)pattern.Source.Clone();

                    //changeElementColor(bmp, System.Drawing.Color.Red);
                    //pattern.Source = Bitmap2BitmapImage(bmp);

                    //System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(new System.Drawing.Image( ));
                    //CoatOfArmsCanvas.Background =  new SolidColorBrush(Colors.Blue);
                    CoatOfArmsCanvas.Children.Add(pattern);
                    
                    Image patternLayer = new Image();
                    patternLayer.Source = buffer.Source.Clone();
                    patternLayer.Height = 30;
                    patternLayer.Width = 30;
                    CoatOfArmsLayersList.Items.Insert(0, patternLayer);
                    selectedImage = pattern;
                    SizeSlider.Value = 1;
                    addedElementList.Add(pattern);
                    pattern.MouseDown += Img_MouseDown;
                }

                pattern.AllowDrop = true;

                DataObject data = new DataObject(typeof(ImageSource), pattern.Source);
                DragDrop.DoDragDrop(pattern, data, DragDropEffects.Move);
            }
            if (currentMenu == 3) //additional items
            {
                if (!CoatOfArmsCanvas.Children.Contains((Image)sender))
                {
                    atribute = new Image();
                    Image buffer = (Image)sender;
                    atribute.Source = buffer.Source.Clone();

                    CoatOfArmsCanvas.Children.Add(atribute);
                    atribute.Width = 70;
                    atribute.Height = 70;
                    selectedImage = atribute;
                    SizeSlider.Value = 1;
                    addedElementList.Add(atribute);
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

            form1.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/forms/circle.png", UriKind.Relative));
            form2.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/forms/shield1.png", UriKind.Relative));
            form3.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/forms/shield2.png", UriKind.Relative));
            form4.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/forms/shield3.png", UriKind.Relative));
            form5.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/forms/shield4.png", UriKind.Relative));
        }


        private void buttonPattern_Click(object sender, RoutedEventArgs e)
        {
            currentMenu = 2;
            form1.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/patterns/cross.png", UriKind.Relative));
            form2.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/patterns/cross_ang.png", UriKind.Relative));
            form3.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/patterns/half.png", UriKind.Relative));
            form4.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/patterns/half_hor.png", UriKind.Relative));
        }



        private void CoatOfArmsLayersList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (CoatOfArmsLayersList.SelectedItem != null)
            {


                // Image layerImage = (Image)CoatOfArmsLayersList.Items[CoatOfArmsLayersList.SelectedIndex];
                itemSelected = true;
                selectedImage = addedElementList[CoatOfArmsLayersList.Items.Count - CoatOfArmsLayersList.SelectedIndex - 1];
                SizeSlider.Value = selectedImage.Width / 500;
                logBox.Text = CoatOfArmsLayersList.SelectedIndex.ToString();
                //if(pattern!=null)
                //    if (layerImage.Source.ToString().Equals(pattern.Source.ToString()))
                //    {   
                //        selectedImage = pattern;
                //        //logBox.Text = "equals";

                //        buttonPattern_Click(layerImage,new RoutedEventArgs());
                //    }
                //if (atribute != null)
                //    if (layerImage.Source.ToString().Equals(atribute.Source.ToString()))
                //    {
                //        selectedImage = atribute;
                //        //logBox.Text = "equals";
                //        buttonAtrs_Click(layerImage, new RoutedEventArgs());
                //    }
                //if (shield != null)
                //    if (layerImage.Source.ToString().Equals(shield.Source.ToString()))
                //    {
                //            selectedImage = shield;
                //            //logBox.Text = "equals";
                //            buttonForm_Click(layerImage, new RoutedEventArgs());
                //    }

            }
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            logBox.Text = "Saving...";
            string path = "/CoatOfArms.png";
            FileStream fs = new FileStream(path, FileMode.Create);
            double w = 400;
            double h = 400;
            double dpi = 300;
            double scale = dpi / 96;

            RenderTargetBitmap bmp = new RenderTargetBitmap((int)(w * scale), (int)(h * scale), dpi, dpi, PixelFormats.Pbgra32);

            bmp.Render(CanvasContainer);
            BitmapEncoder encoder = new PngBitmapEncoder();

            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(fs);
            logBox.Text = "File saved: " + fs.Name;
            fs.Close();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (itemSelected)
            {
                if (CoatOfArmsCanvas.Children.Contains(selectedImage))
                {
                    addedElementList.Remove(selectedImage);
                    CoatOfArmsCanvas.Children.Remove(selectedImage);
                    CoatOfArmsLayersList.Items.Remove(CoatOfArmsLayersList.SelectedItem);
                    itemSelected = false;
                }
            }


        }

        private void buttonAtrs_Click(object sender, RoutedEventArgs e)
        {
            currentMenu = 3;
            form1.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/atributes/tower.png", UriKind.Relative));
            form2.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/atributes/apple.png", UriKind.Relative));
            form3.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/atributes/lion.png", UriKind.Relative));
            form4.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/atributes/scales.png", UriKind.Relative));
            form5.Source = new BitmapImage(new Uri("/Pictures/CoatOfArms/atributes/eagle.png", UriKind.Relative));

        }

        private void SizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (selectedImage != null)
            {
                if (currentMenu == 2)
                {
                    selectedImage.RenderTransform.Value.RotatePrepend(30);
                    selectedImage.Height = 500 * SizeSlider.Value;
                    selectedImage.Width = 500 * SizeSlider.Value;

                }
                if (currentMenu == 3)
                {
                    selectedImage.Height = 70 * SizeSlider.Value;
                    selectedImage.Width = 70 * SizeSlider.Value;
                }

            }
        }
        private void changeElementColor(System.Drawing.Bitmap _bmp, System.Drawing.Color _color)
        {
            for (int i = 0; i < _bmp.Size.Height; i++)
            {
                for (int j = 0; j < _bmp.Size.Width; j++)
                {
                    System.Drawing.Color pixel = _bmp.GetPixel(j, i);
                    if (!pixel.Equals(System.Drawing.Color.Transparent))
                    {
                        _bmp.SetPixel(j, i, _color);

                    }

                }
            }
        }
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private BitmapImage Bitmap2BitmapImage(System.Drawing.Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            BitmapImage retval;

            try
            {
                retval = (BitmapImage)Imaging.CreateBitmapSourceFromHBitmap(
                             hBitmap,
                             IntPtr.Zero,
                             Int32Rect.Empty,
                             BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
    }
    //public static class BitmapExtensions
    //{
    //    public static System.Drawing.Image ChangeColor(this System.Drawing.Image image, System.Drawing.Color fromColor, System.Drawing.Color toColor)
    //    {
    //        System.Drawing.Imaging.ImageAttributes attributes = new System.Drawing.Imaging.ImageAttributes();
    //        attributes.SetRemapTable(new ColorMap[]
    //        {
    //        new ColorMap()
    //        {
    //            OldColor = fromColor,
    //            NewColor = toColor,
    //        }
    //        }, ColorAdjustType.Bitmap);

    //        using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image))
    //        {
    //            System.Drawing.Point p = new System.Drawing.Point();
    //            g.DrawImage(image, new System.Drawing.Rectangle(p, image.Size), 0, 0, image.Width, image.Height, System.Drawing.GraphicsUnit.Pixel, attributes);
    //        }

    //        return image;
    //    }
    //}
}
