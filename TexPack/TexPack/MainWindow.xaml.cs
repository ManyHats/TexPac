using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Drawing.Imaging;

namespace TexPack
{
    public partial class MainWindow : Window
    {
        private List<System.Drawing.Image> images = new List<System.Drawing.Image>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ImageList_Drop(object sender, DragEventArgs e)
        {
            var data = e.Data.GetData(DataFormats.FileDrop) as string[];
            spriteLabel.Opacity = 0;

            for (int i = 0; i < data.Length; i++)
            {
                // Create new image
                System.Drawing.Image img = System.Drawing.Image.FromFile(data[i]);

                // Add image to list of items
                images.Add(img);

                // Get file name for list
                string fileName = Path.GetFileName(data[i]);

                // Create new list item and add it to list
                ListBoxItem listItem = new ListBoxItem();
                listItem.Content = fileName;
                imageList.Items.Add(listItem);
            }
        }

        private void ImageList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ExportImage_Click(object sender, RoutedEventArgs e)
        {
            int exportWidth = 0;
            int exportHeight = 0;
            int padding = int.Parse(paddingBox.Text);

            // Calculate export image height and width
            foreach (System.Drawing.Image img in images)
            {
                exportWidth += img.Width;
                exportHeight = img.Height;
            }

            Bitmap exportImage = new Bitmap(exportWidth + (images.Count * padding), exportHeight);

            using (Graphics graphics = Graphics.FromImage(exportImage))
            {
                int x = 0;

                for (int i = 0; i < images.Count; i++)
                {
                    graphics.DrawImage(images[i], new Rectangle(x, 0, images[i].Width, images[i].Height));
                    x += padding;
                    x += images[i].Width;
                }
            }

            SaveFileDialog dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                exportImage.Save(dialog.FileName, ImageFormat.Png);
                exportImage.Dispose();
            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            images.Clear();
            imageList.Items.Clear();
        }
    }
}
