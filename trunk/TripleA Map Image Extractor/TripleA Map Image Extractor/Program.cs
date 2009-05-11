using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace TripleA_Map_Image_Extractor
{
    static class Program
    {
        static ProgressBarWindow barW = new ProgressBarWindow();
        [STAThread]
        static void Main(string[] args)
        {
            Size mapSize = new Size();
            FolderBrowserDialog open = new FolderBrowserDialog();
            open.ShowNewFolderButton = true;
            WriteLine("Please specify the folder containing the base tiles for the map.");
            open.Description = "Please specify the folder containing the base tiles for the map. You can usually find it in the 'baseTiles' folder in the folder where the map was placed.";
            if (open.ShowDialog() != DialogResult.Cancel)
            {
                mapSize = getMapSize(new DirectoryInfo(open.SelectedPath));
                barW.progressBar1.Value = 0;
                List<FileInfo> files = new List<FileInfo>(new DirectoryInfo(open.SelectedPath).GetFiles());
                List<FileInfo> images = new List<FileInfo>();
                foreach (FileInfo cur in files)
                {
                    if (cur.Extension.ToLower() == ".png")
                    {
                        images.Add(cur);
                    }
                }
                barW.Show();
                barW.progressBar1.Maximum = images.Count;
                WriteLine("The program will now form the base tiles into one image...");
                Image fullImage = new Bitmap(mapSize.Width, mapSize.Height);
                Graphics grphx = Graphics.FromImage(fullImage);
                foreach (FileInfo image in images)
                {
                    barW.progressBar1.Value++;
                    int x = Convert.ToInt32(image.Name.Substring(0, image.Name.IndexOf("_")));
                    int y = Convert.ToInt32(image.Name.Substring(image.Name.IndexOf("_") + 1, image.Name.Substring(image.Name.IndexOf("_") + 1).IndexOf(".")));
                    Image imageToPaste = Image.FromFile(image.FullName);
                    Point pasteLoc = new Point(x * 256, y * 256);
                    WriteLine("Drawing base tile " + x + "," + y + " to the map image...");
                    grphx.DrawImage(imageToPaste, pasteLoc);
                    imageToPaste.Dispose();
                }
                WriteLine("Done forming image. Please select save location.");
                SaveFileDialog save = new SaveFileDialog();
                save.DefaultExt = ".png";
                save.Filter = "Png Image Files|*.png|All files (*.*)|*.*";
                save.Title = "Please select location to save map image file to.";
                if (save.ShowDialog() != DialogResult.Cancel)
                {
                    fullImage.Save(save.FileName, System.Drawing.Imaging.ImageFormat.Png);
                    WriteLine("Map image saved to " + save.FileName);
                }
                else
                {
                    WriteLine("Save location not specified. The program will now shut down.");
                }
            }
            else
            {
                WriteLine("Folder not specified. The program will now shut down.");
            }
            barW.Hide();
        }
        public static Size getMapSize(DirectoryInfo baseTilesFolder)
        {
            Size result = new Size();
            if (File.Exists(baseTilesFolder.Parent.FullName + @"\map.properties"))
            {
                string[] lines = File.ReadAllLines(baseTilesFolder.Parent.FullName + @"\map.properties");
                foreach (string cur in lines)
                {
                    if (cur.ToLower().Contains("map.width="))
                    {
                        result.Width = Convert.ToInt32(cur.ToLower().Substring(cur.ToLower().IndexOf(".width=") + 7));
                    }
                    if (cur.ToLower().Contains("map.height="))
                    {
                        result.Height = Convert.ToInt32(cur.ToLower().Substring(cur.ToLower().IndexOf(".height=") + 8));
                    }
                }
            }
            else
            {
                OpenFileDialog open = new OpenFileDialog();
                open.CheckFileExists = true;
                open.DefaultExt = ".properties";
                open.Filter = "Map Properties Files|*.properties|All files (*.*)|*.*";
                open.InitialDirectory = baseTilesFolder.Parent.FullName;
                open.Multiselect = false;
                open.Title = "Please select the map.properties file for the map.";
                if (open.ShowDialog() != DialogResult.Cancel)
                {
                    string[] lines = File.ReadAllLines(open.FileName);
                    foreach (string cur in lines)
                    {
                        if (cur.Contains("map.width="))
                        {
                            result.Width = Convert.ToInt32(cur.ToLower().Substring(cur.ToLower().IndexOf(".width=") + 7));
                        }
                        if (cur.Contains("map.height="))
                        {
                            result.Height = Convert.ToInt32(cur.ToLower().Substring(cur.ToLower().IndexOf(".height=") + 8));
                        }
                    }
                }
            }
            return result;
        }
        public static void WriteLine(string line)
        {
        }
    }
}
