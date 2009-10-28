using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;

namespace TripleA_Map_Resizer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Main.CheckForIllegalCrossThreadCalls = false;
            CheckForUpdates();
        }
        private Version usersVersion = new Version(1, 0, 1, 1);
        public void CheckForUpdates()
        {
            Thread t = new Thread(new ThreadStart(update));
            t.Priority = ThreadPriority.Lowest;
            t.IsBackground = true;
            t.Start();
        }
        private void update()
        {
            WebClient client = new WebClient(); //http://tmapc.googlecode.com/files/TripleA%20Map%20Creator%20v1.0.0.8.zip
            Version currentCheckingVersion = usersVersion;
            Version newestVersionAvailable = usersVersion;
            bool doBreak = false;
            bool hasStartedFindingVersions = false;

            while (!doBreak)
            {
                try
                {
                    Stream s = client.OpenRead("http://tmapc.googlecode.com/files/TripleA%20Map%20Creator%20v" + currentCheckingVersion.ToString() + ".zip");
                    newestVersionAvailable = currentCheckingVersion;
                    if (currentCheckingVersion.Revision < 9)
                        currentCheckingVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor, currentCheckingVersion.Build, currentCheckingVersion.Revision + 1);
                    else if (currentCheckingVersion.Build < 9)
                        currentCheckingVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor, currentCheckingVersion.Build + 1, 0);
                    else if (currentCheckingVersion.Minor < 9)
                        currentCheckingVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor + 1, 0, 0);
                    else if (currentCheckingVersion.Major < 9)
                        currentCheckingVersion = new Version(currentCheckingVersion.Major + 1, 0, 0, 0);

                    s.Close();
                    hasStartedFindingVersions = true;
                }
                catch
                {
                    if (hasStartedFindingVersions)
                        break;
                    else
                    {
                        if (currentCheckingVersion.Revision < 9)
                            currentCheckingVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor, currentCheckingVersion.Build, currentCheckingVersion.Revision + 1);
                        else if (currentCheckingVersion.Build < 9)
                            currentCheckingVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor, currentCheckingVersion.Build + 1, 0);
                        else if (currentCheckingVersion.Minor < 9)
                            currentCheckingVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor + 1, 0, 0);
                        else if (currentCheckingVersion.Major < 9)
                            currentCheckingVersion = new Version(currentCheckingVersion.Major + 1, 0, 0, 0);
                    }
                }
            }
            if (Convert.ToInt32(usersVersion.ToString().Replace(".", "")) < Convert.ToInt32(newestVersionAvailable.ToString().Replace(".", "")))
            {
                MessageBox.Show("There is a newer version of the Map Creator available.\r\nYour version: " + usersVersion.ToString() + ".\r\nNewest Version: " + newestVersionAvailable.ToString() + ".\r\n\r\nTo download the latest version, please go to \"http://code.google.com/p/tmapc/downloads/list\" and click on the latest download.", "Checking For Updates");
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void browseButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Please select the folder that contains all the map files.";
            folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                mapFolderTB.Text = folderBrowser.SelectedPath;
                DirectoryInfo folder = new DirectoryInfo(folderBrowser.SelectedPath);
                if (MessageBox.Show("Do you want the program to try to populate all the map information files automattically?", "Auto-Populate Files",MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    foreach (Control cur in groupBox1.Controls)
                    {
                        if (cur is TextBox)
                        {
                            cur.Text = "";
                        }
                    }
                    mapFolderTB.Text = folderBrowser.SelectedPath;
                    foreach (DirectoryInfo directory in folder.GetDirectories())
                    {
                        if (directory.Name.ToLower() == "basetiles")
                        {
                            baseTilesFolderTB.Text = directory.FullName;
                        }
                        else if (directory.Name.ToLower() == "relieftiles")
                        {
                            reliefTilesFolderTB.Text = directory.FullName;
                        }
                    }
                    foreach (FileInfo file in folder.GetFiles())
                    {
                        if (file.Extension.ToLower() == ".txt")
                        {
                            if (file.Name.ToLower() == "centers.txt")
                            {
                                centersFileTB.Text = file.FullName;
                            }
                            else if (file.Name.ToLower() == "polygons.txt")
                            {
                                polygonsFileTB.Text = file.FullName;
                            }
                            else if (file.Name.ToLower() == "place.txt")
                            {
                                placementFileTB.Text = file.FullName;
                            }
                            else if (file.Name.ToLower() == "name_place.txt")
                            {
                                namePlacementFileTB.Text = file.FullName;
                            }
                            else if (file.Name.ToLower() == "vc.txt")
                            {
                                vcTB.Text = file.FullName;
                            }
                            else if (file.Name.ToLower() == "ipc_place.txt")
                            {
                                ipcPlacementTB.Text = file.FullName;
                            }
                            else if (file.Name.ToLower() == "decorations.txt")
                            {
                                decorationsFileTB.Text = file.FullName;
                            }
                            else if (file.Name.ToLower() == "capitols.txt")
                            {
                                capitolsFileTB.Text = file.FullName;
                            }
                            else if (file.Name.ToLower() == "kamikaze_place.txt")
                            {
                                KamikazePlacementFileTB.Text = file.FullName;
                            }
                        }
                        else if (file.Extension.ToLower() == ".properties")
                        {
                            propertiesFileTB.Text = file.FullName;
                            if(File.Exists(propertiesFileTB.Text))
                            {
                                Size mapSize = getMapSize(new FileInfo(propertiesFileTB.Text));
                                mapSizeTB.Text = mapSize.Width + ","+ mapSize.Height;
                            }
                        }
                        else if (file.Extension.ToLower() == ".png" || file.Extension.ToLower() == ".jpg" || file.Extension.ToLower() == ".bmp")
                        {
                            imageFileTB.Text = file.FullName;
                            Image image = Image.FromFile(file.FullName);
                            mapSizeTB.Text = image.Size.Width + "," + image.Size.Height; 
                            image.Dispose();
                        }
                    }
                }
            }

        }
        private void browseButton2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Please select the 'baseTiles' folder that contains all the map's base tiles.";
            folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                baseTilesFolderTB.Text = folderBrowser.SelectedPath;
            }
        }

        private void browseButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'centers.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                centersFileTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'polygons.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                polygonsFileTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton5_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Map Properties Files (*.properties)|*.properties";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'map.properties' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                propertiesFileTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Please select the 'reliefTiles' folder that contains all the map's relief tiles.";
            folderBrowser.RootFolder = Environment.SpecialFolder.Desktop;
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                reliefTilesFolderTB.Text = folderBrowser.SelectedPath;
            }
        }

        private void browseButton8_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Image Files (*.png,*.jpg,*.jpeg,*.bmp)|*.png;*.jpg;*.jpeg;*.bmp|PNG Files (*.png)|*.png|JPEG Files (*.jpg,*.jpeg)|*.jpg;*.jpeg|Bitmap Files (*.bmp)|*.bmp";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's image file.";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                imageFileTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton9_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'place.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                placementFileTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton10_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'name_place.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                namePlacementFileTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton11_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'vc.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                vcTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton12_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'ipc_place.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                ipcPlacementTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton13_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'decorations.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                decorationsFileTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton14_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'capitols.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                capitolsFileTB.Text = fileBrowser.FileName;
            }
        }

        private void browseButton15_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileBrowser = new OpenFileDialog();
            fileBrowser.CheckFileExists = true;
            fileBrowser.CheckPathExists = true;
            fileBrowser.Filter = "Text Files (*.txt)|*.txt";
            fileBrowser.Multiselect = false;
            fileBrowser.Title = "Please select the map's 'kamikaze_place.txt' file";
            if (fileBrowser.ShowDialog(this) == DialogResult.OK)
            {
                KamikazePlacementFileTB.Text = fileBrowser.FileName;
            }
        }
        MapPreviewWindow mapPreviewWindow = new MapPreviewWindow();
        bool needToReconstructMapImage = true;
        public Bitmap reconstructedMapImage = new Bitmap(1, 1);
        private void previewResizedMapBTN_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo propertiesFile = new FileInfo(propertiesFileTB.Text);
                if (!propertiesFile.Exists)
                {
                    MessageBox.Show("Unable to recreate the map image from the base tiles bacause the map properties file is invalid or does not exist.", "Unable To Recreate Map Image");
                    return;
                }
                Size oldSize = getMapSize(propertiesFile);
                if (needToReconstructMapImage)
                {
                    needToReconstructMapImage = false;
                    DirectoryInfo baseTilesFolder = new DirectoryInfo(baseTilesFolderTB.Text);
                    if (!baseTilesFolder.Exists)
                    {
                        MessageBox.Show("Unable to recreate the map image from the base tiles bacause the base tiles folder is invalid or does not exist.", "Unable To Recreate Map Image");
                        return;
                    }
                    reconstructedMapImage = ReconstructMapImageUsingBaseTiles(baseTilesFolder, oldSize);
                }
                Size newSize = getScaledSize(oldSize);
                Bitmap resizedBitmap = new Bitmap(newSize.Width, newSize.Height);
                Graphics grphx = Graphics.FromImage(resizedBitmap);
                if (useImageSmoothing.Checked)
                {
                    grphx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                }
                else
                {
                    grphx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                }
                grphx.DrawImage(reconstructedMapImage, new Rectangle(new Point(0, 0), newSize));
                mapPreviewWindow.DisplayImage(resizedBitmap);
                grphx.Dispose();
            }
            catch (FileNotFoundException ex)
            { MessageBox.Show("The map image file does not exist.", "File Not Found"); }
            catch (Exception ex) { if (MessageBox.Show("An error occured when trying to preview the resized map image. Do you want to view the error message?", "Error Previewing Resized Image", MessageBoxButtons.YesNoCancel) == DialogResult.Yes) { exceptionViewerWindow.ShowInformationAboutException(ex, true); } }
            //catch { }
        }
        ExceptionViewer exceptionViewerWindow = new ExceptionViewer();
        private Bitmap ReconstructMapImageUsingBaseTiles(DirectoryInfo baseTilesFolder,Size mapSize)
        {
            List<FileInfo> files = new List<FileInfo>(baseTilesFolder.GetFiles());
            List<FileInfo> images = new List<FileInfo>();
            foreach (FileInfo cur in files)
            {
                if (cur.Extension.ToLower() == ".png")
                {
                    images.Add(cur);
                }
            }
            Image fullImage = new Bitmap(mapSize.Width, mapSize.Height);
            Graphics grphx = Graphics.FromImage(fullImage);
            grphx.Clear(Color.White);
            foreach (FileInfo image in images)
            {
                try
                {
                    int x = Convert.ToInt32(image.Name.Substring(0, image.Name.IndexOf("_")));
                    int y = Convert.ToInt32(image.Name.Substring(image.Name.IndexOf("_") + 1, image.Name.Substring(image.Name.IndexOf("_") + 1).IndexOf(".")));
                    Image imageToPaste = Image.FromFile(image.FullName);
                    Point pasteLoc = new Point(x * 256, y * 256);
                    grphx.DrawImage(imageToPaste, pasteLoc);
                    imageToPaste.Dispose();
                }
                catch {}
            }
            return (Bitmap)fullImage;
        }

        private Size getMapSize(FileInfo mapPropertiesFile)
        {
            Size result = new Size();
            string[] lines = File.ReadAllLines(mapPropertiesFile.FullName);
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
            return result;
        }
        private Size getScaledSize(Size oldSize)
        {
            if (radioButton1.Checked)
            {
                float ratio = ((float)mapScaleUPDOWN.Value / 100F);
                return new Size((int)(oldSize.Width * ratio), (int)(oldSize.Height * ratio));
            }
            else
            {
                string text = mapSizeTB.Text.Trim();
                return new Size(Convert.ToInt32(text.Substring(0, text.IndexOf(","))), Convert.ToInt32(text.Substring(text.IndexOf(",") + 1)));
            }
        }
        private SizeF getScale(Size oldSize, Size newSize)
        {
            return new SizeF((float)1.00 + ((float)(newSize.Width - oldSize.Width) / (float)oldSize.Width), (float)1.00 + ((float)(newSize.Height - oldSize.Height) / (float)oldSize.Height));
        }

        private void resizeMapFilesBTN_Click(object sender, EventArgs e)
        {
            if (resizeMapThread == null || !resizeMapThread.IsAlive)
            {
                if (!ValidateMapFilesAndFolders())
                {
                    MessageBox.Show("Some of the map files or folders are invalid or do not exist. The program cannot resize the map until all the files and folders are valid.", "Invalid Files Or Folders");
                    return;
                }
                resizeMapThread = new Thread(resizeMapMethod);
                resizeMapThread.Start();
            }
            else
            {
                resizeMapThread.Suspend();
                if (MessageBox.Show("Are you sure you want to cancel the resizing of the map files and folders?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    resizeMapThread.Resume();
                    resizeMapThread.Abort();
                    StartAllowingEditing();
                }
                else
                {
                    resizeMapThread.Resume();
                }
            }
        }
        Thread resizeMapThread;
        public void resizeMapMethod()
        {
            try
            {
                StopAllowingEditing();
                FileInfo propertiesFile = new FileInfo(propertiesFileTB.Text);
                if (!propertiesFile.Exists)
                {
                    MessageBox.Show("Unable to recreate the map image from the base tiles bacause the map properties file is invalid or does not exist.", "Unable To Recreate Map Image");
                    return;
                }
                Size oldSize = getMapSize(propertiesFile);
                if (oldSize.Width == 0 || oldSize.Height == 0)
                {
                    MessageBox.Show("Unable to recreate the map image from the base tiles bacause the map properties file is invalid or does not exist.", "Unable To Recreate Map Image");
                    return;
                }
                if (needToReconstructMapImage)
                {
                    needToReconstructMapImage = false;
                    DirectoryInfo baseTilesFolder = new DirectoryInfo(baseTilesFolderTB.Text);
                    if (!baseTilesFolder.Exists)
                    {
                        MessageBox.Show("Unable to recreate the map image from the base tiles bacause the base tiles folder is invalid or does not exist.", "Unable To Recreate Map Image");
                        return;
                    }
                    reconstructedMapImage = ReconstructMapImageUsingBaseTiles(baseTilesFolder, oldSize);
                }
                Size newSize = getScaledSize(oldSize);
                Bitmap resizedBitmap = ResizeImage(reconstructedMapImage, newSize);
                SizeF scaleRatio = getScale(oldSize, newSize);
                DialogResult result = MessageBox.Show("Do you want to overwrite the original map data with the resized files and folders?", "Overwrite Map Files", MessageBoxButtons.YesNoCancel);
                DirectoryInfo mapFolderTW;
                if (result == DialogResult.Yes)
                {
                    mapFolderTW = new DirectoryInfo(mapFolderTB.Text);
                }
                else if (result == DialogResult.No)
                {
                    mapFolderTW = new DirectoryInfo(mapFolderTB.Text + @"\Resized Map Files");
                }
                else
                {
                    StartAllowingEditing();
                    statusLabel.Text = "Ready...";
                    progressBar.Value = 0;
                    return;
                }
                DirectoryInfo baseTilesFolderTW = new DirectoryInfo(mapFolderTW.FullName + @"\baseTiles");
                FileInfo centersFileTW = new FileInfo(mapFolderTW.FullName + @"\centers.txt");
                FileInfo polygonsFileTW = new FileInfo(mapFolderTW.FullName + @"\polygons.txt");
                FileInfo propertiesFileTW = new FileInfo(mapFolderTW.FullName + @"\map.properties");
                DirectoryInfo reliefTilesFolderTW = new DirectoryInfo(mapFolderTW.FullName + @"\reliefTiles");
                FileInfo imageFileTW = new FileInfo(mapFolderTW.FullName + @"\resizedMapImage.png");
                FileInfo placementFileTW = new FileInfo(mapFolderTW.FullName + @"\place.txt");
                FileInfo namePlacementFileTW = new FileInfo(mapFolderTW.FullName + @"\name_place.txt");
                FileInfo victoryCitiesFileTW = new FileInfo(mapFolderTW.FullName + @"\vc.txt");
                FileInfo ipcPlacementFileTW = new FileInfo(mapFolderTW.FullName + @"\ipc_place.txt");
                FileInfo decorationsFileTW = new FileInfo(mapFolderTW.FullName + @"\decorations.txt");
                FileInfo capitolsFileTW = new FileInfo(mapFolderTW.FullName + @"\capitols.txt");
                FileInfo kamikazePlacementFileTW = new FileInfo(mapFolderTW.FullName + @"\kamikaze_place.txt");

                Directory.CreateDirectory(mapFolderTW.FullName);
                if (baseTilesFolderTB.Text.Trim().Length > 0 && Directory.Exists(baseTilesFolderTB.Text))
                {
                    statusLabel.Text = "Processing Base Tiles...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    Directory.CreateDirectory(baseTilesFolderTW.FullName);
                    BreakDownImageAndSaveToDirectory(resizedBitmap, baseTilesFolderTW);
                }
                if (reliefTilesFolderTB.Text.Trim().Length > 0 && Directory.Exists(reliefTilesFolderTB.Text))
                {
                    statusLabel.Text = "Processing Relief Tiles...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    Directory.CreateDirectory(reliefTilesFolderTW.FullName);
                    BreakDownImageAndSaveToDirectory(ResizeImage((Bitmap)ReconstructImageUsingReliefTiles(new DirectoryInfo(reliefTilesFolderTB.Text),resizedBitmap.Size),newSize), reliefTilesFolderTW);
                }
                if (centersFileTB.Text.Trim().Length > 0 && File.Exists(centersFileTB.Text))
                {
                    statusLabel.Text = "Processing Centers File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, centersFileTB.Text, centersFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                if (polygonsFileTB.Text.Trim().Length > 0 && File.Exists(polygonsFileTB.Text))
                {
                    statusLabel.Text = "Processing Polygons File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, polygonsFileTB.Text, polygonsFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                if (propertiesFileTB.Text.Trim().Length > 0 && File.Exists(propertiesFileTB.Text))
                {
                    statusLabel.Text = "Processing Properties File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    UpdateMapSizeAndWriteResults(resizedBitmap.Size, propertiesFileTB.Text, propertiesFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                resizedBitmap.Save(imageFileTW.Open(FileMode.Create, FileAccess.Write), System.Drawing.Imaging.ImageFormat.Png);
                if (placementFileTB.Text.Trim().Length > 0 && File.Exists(placementFileTB.Text))
                {
                    statusLabel.Text = "Processing Placements File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, placementFileTB.Text, placementFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                if (namePlacementFileTB.Text.Trim().Length > 0 && File.Exists(namePlacementFileTB.Text))
                {
                    statusLabel.Text = "Processing Name Placements File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, namePlacementFileTB.Text, namePlacementFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                if (vcTB.Text.Trim().Length > 0 && File.Exists(vcTB.Text))
                {
                    statusLabel.Text = "Processing Victory Cities File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, vcTB.Text, victoryCitiesFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                if (ipcPlacementTB.Text.Trim().Length > 0 && File.Exists(ipcPlacementTB.Text))
                {
                    statusLabel.Text = "Processing Ipc Placements File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, ipcPlacementTB.Text, ipcPlacementFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                if (decorationsFileTB.Text.Trim().Length > 0 && File.Exists(decorationsFileTB.Text))
                {
                    statusLabel.Text = "Processing Decorations File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, decorationsFileTB.Text, decorationsFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                if (capitolsFileTB.Text.Trim().Length > 0 && File.Exists(capitolsFileTB.Text))
                {
                    statusLabel.Text = "Processing Capitols File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, capitolsFileTB.Text, capitolsFileTW.Open(FileMode.Create, FileAccess.Write));
                }
                if (KamikazePlacementFileTB.Text.Trim().Length > 0 && File.Exists(KamikazePlacementFileTB.Text))
                {
                    statusLabel.Text = "Processing Kamikaze Placements File...";
                    statusLabel.Invalidate();
                    statusStrip1.Update();
                    ScalePointsAndWriteResults(scaleRatio, KamikazePlacementFileTB.Text, kamikazePlacementFileTW.Open(FileMode.Create, FileAccess.Write));
                }
            }
            catch (FileNotFoundException ex)
            { MessageBox.Show("One of the files that was going to be processed does not exist. Cancelling resizing operation.", "File Not Found"); }
            catch (Exception ex) { if (MessageBox.Show("An error occured when trying to resize the map. Do you want to view the error message?", "Error Resizing Map", MessageBoxButtons.YesNoCancel) == DialogResult.Yes) { exceptionViewerWindow.ShowInformationAboutException(ex, true); } }
            StartAllowingEditing();
            statusLabel.Text = "Ready...";
            progressBar.Value = 0;
            //catch { }
        }

        private Bitmap ResizeImage(Bitmap image, Size newSize)
        {
            Bitmap result = new Bitmap(newSize.Width, newSize.Height);
            Graphics grphx = Graphics.FromImage(result);
            if (useImageSmoothing.Checked)
            {
                grphx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            }
            else
            {
                grphx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            }
            grphx.DrawImage(image, new Rectangle(new Point(0, 0), newSize));
            grphx.Dispose();
            return result;
        }

        private Image ReconstructImageUsingReliefTiles(DirectoryInfo reliefTilesFolder, Size mapSize)
        {
            List<FileInfo> files = new List<FileInfo>(reliefTilesFolder.GetFiles());
            List<FileInfo> images = new List<FileInfo>();
            foreach (FileInfo cur in files)
            {
                if (cur.Extension.ToLower() == ".png")
                {
                    images.Add(cur);
                }
            }
            Image fullImage = new Bitmap(mapSize.Width, mapSize.Height);
            Graphics grphx = Graphics.FromImage(fullImage);
            foreach (FileInfo image in images)
            {
                try
                {
                    int x = Convert.ToInt32(image.Name.Substring(0, image.Name.IndexOf("_")));
                    int y = Convert.ToInt32(image.Name.Substring(image.Name.IndexOf("_") + 1, image.Name.Substring(image.Name.IndexOf("_") + 1).IndexOf(".")));
                    Image imageToPaste = Image.FromFile(image.FullName);
                    Point pasteLoc = new Point(x * 256, y * 256);
                    grphx.DrawImage(imageToPaste, pasteLoc);
                    imageToPaste.Dispose();
                }
                catch { }
            }
            return (Bitmap)fullImage;
        }

        private void StopAllowingEditing()
        {
            resizeMapFilesBTN.Text = "Cancel";
            previewResizedMapBTN.Enabled = false;
            groupBox1.Enabled = false;
            useImageSmoothing.Enabled = false;
            mapScaleUPDOWN.Enabled = false;
            mapSizeTB.Enabled = false;
            radioButton1.Enabled = false;
            radioButton2.Enabled = false;
        }
        private void StartAllowingEditing()
        {
            progressBar.Value = 0;
            statusLabel.Text = "Ready...";
            resizeMapFilesBTN.Text = "Resize Map Files";
            previewResizedMapBTN.Enabled = true;
            groupBox1.Enabled = true;
            useImageSmoothing.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton1_CheckedChanged(new object(), new EventArgs());
        }

        private void UpdateMapSizeAndWriteResults(Size size, string readFileLocation, FileStream fileStream)
        {
            int index = 0;
            string[] lines = File.ReadAllLines(readFileLocation);
            StringBuilder textBuilder = new StringBuilder();
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = lines.Length;
            int lineIndex = 0;
            foreach (string line in lines)
            {
                string text = line.Trim();
                while (index < text.Length)
                {
                    string remainingTextToProcess = text.Substring(index);
                    int indexOfSpace = remainingTextToProcess.IndexOf(" ");
                    if (indexOfSpace <= 0)
                    {
                        textBuilder.AppendLine(GetEditedText(remainingTextToProcess,size));
                        progressBar.Value = lineIndex + 1;
                        break;
                    }
                    else
                    {
                        string cur = text.Substring(index, indexOfSpace);
                        textBuilder.Append(GetEditedText(cur, size));
                        index += cur.Length;
                    }
                }
                index = 0;
                lineIndex++;
            }
            progressBar.Value = 0;
            byte[] bytes = Encoding.ASCII.GetBytes(textBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }

        private string GetEditedText(string text,Size resizedMapSize)
        {
            if (text.Trim().ToLower().StartsWith("map.width="))
            {
                return "map.width=" + resizedMapSize.Width;
            }
            else if (text.Trim().ToLower().StartsWith("map.height="))
            {
                return "map.height=" + resizedMapSize.Height;
            }
            else
                return text;
        }
        private void BreakDownImageAndSaveToDirectory(Image image, DirectoryInfo baseTilesFolderTW)
        {
            try
            {
                foreach (FileInfo cur in baseTilesFolderTW.GetFiles())
                {
                    try
                    {
                        cur.Delete();
                    }
                    catch { }
                }
            }
            catch { }
            int xSquarePosition = 0;
            int ySquarePosition = 0;
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = ((image.Width / 256) + 1) * ((image.Height / 256) + 1);
            while (xSquarePosition < image.Width)
            {
                while (ySquarePosition < image.Height)
                {
                    progressBar.Value++;
                    Bitmap bitmap = new Bitmap(256, 256);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.DrawImage(image, new Rectangle(0, 0, 256, 256), new Rectangle(xSquarePosition, ySquarePosition, 256, 256), GraphicsUnit.Pixel);
                    Stream stream = File.Open(String.Concat(baseTilesFolderTW.FullName, @"\", (xSquarePosition / 256), "_", (ySquarePosition / 256), ".png"), FileMode.Create);
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Close();
                    stream.Dispose();
                    graphics.Dispose();
                    ySquarePosition += 256;
                }
                xSquarePosition += 256;
                ySquarePosition = 0;
            }
        }

        private void ScalePointsAndWriteResults(SizeF scaleRatio, string readFileLocation, FileStream fileStream)
        {
            int index = 0;
            string text = File.ReadAllText(readFileLocation);
            StringBuilder textBuilder = new StringBuilder();
            progressBar.Value = 0;
            progressBar.Minimum = 0;
            progressBar.Maximum = text.Length;
            while (index < text.Length)
            {
                string remainingTextToProcess = text.Substring(index);
                int indexOfPointEnd = remainingTextToProcess.IndexOf(")");
                if (indexOfPointEnd <= 0)
                {
                    textBuilder.Append(remainingTextToProcess);
                    progressBar.Value = 100;
                    break;
                }
                else
                {
                    string cur = text.Substring(index, indexOfPointEnd + 1);
                    textBuilder.Append(GetTransformedPointText(cur, scaleRatio));
                    index += cur.Length;
                    progressBar.Value = index;
                }
            }
            progressBar.Value = 0;
            byte[] bytes = Encoding.ASCII.GetBytes(textBuilder.ToString());
            fileStream.Write(bytes, 0, bytes.Length);
            fileStream.Close();
        }
        int indexOfPointStart;
        private string GetTransformedPointText(string cur,SizeF scaleRatio)
        {
            indexOfPointStart = cur.IndexOf("(");
            string prePointPart = cur.Substring(0,indexOfPointStart);
            string pointPart = cur.Substring(indexOfPointStart + 1, cur.Length - (indexOfPointStart + 2));
            Point oldPoint = new Point(Convert.ToInt32(pointPart.Substring(0, pointPart.IndexOf(","))), Convert.ToInt32(pointPart.Substring(pointPart.IndexOf(",") + 1)));
            Point newPoint = new Point((int)(oldPoint.X * scaleRatio.Width), (int)(oldPoint.Y * scaleRatio.Height));
            return String.Concat(prePointPart, "(", newPoint.X, ", ", newPoint.Y, ")");
        }
        private bool ValidateMapFilesAndFolders()
        {
            if (mapFolderTB.Text.Trim().Length > 0 && !Directory.Exists(mapFolderTB.Text))
                return false;
            if (baseTilesFolderTB.Text.Trim().Length > 0 && !Directory.Exists(baseTilesFolderTB.Text))
                return false;
            if (centersFileTB.Text.Trim().Length > 0 && !File.Exists(centersFileTB.Text))
                return false;
            if (polygonsFileTB.Text.Trim().Length > 0 && !File.Exists(polygonsFileTB.Text))
                return false;
            if (propertiesFileTB.Text.Trim().Length > 0 && !File.Exists(propertiesFileTB.Text))
                return false;
            if (reliefTilesFolderTB.Text.Trim().Length > 0 && !Directory.Exists(reliefTilesFolderTB.Text))
                return false;
            if (imageFileTB.Text.Trim().Length > 0 && !File.Exists(imageFileTB.Text))
                return false;
            if (placementFileTB.Text.Trim().Length > 0 && !File.Exists(placementFileTB.Text))
                return false;
            if (namePlacementFileTB.Text.Trim().Length > 0 && !File.Exists(namePlacementFileTB.Text))
                return false;
            if (vcTB.Text.Trim().Length > 0 && !File.Exists(vcTB.Text))
                return false;
            if (ipcPlacementTB.Text.Trim().Length > 0 && !File.Exists(ipcPlacementTB.Text))
                return false;
            if (decorationsFileTB.Text.Trim().Length > 0 && !File.Exists(decorationsFileTB.Text))
                return false;
            if (capitolsFileTB.Text.Trim().Length > 0 && !File.Exists(capitolsFileTB.Text))
                return false;
            if (KamikazePlacementFileTB.Text.Trim().Length > 0 && !File.Exists(KamikazePlacementFileTB.Text))
                return false;
            return true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            mapScaleUPDOWN.Enabled = radioButton1.Checked;
            mapSizeTB.Enabled = radioButton2.Checked;
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            PrepareForClose();
        }

        private void PrepareForClose()
        {
            GC.Collect();
        }

        private void baseTilesFolderTB_TextChanged(object sender, EventArgs e)
        {
            needToReconstructMapImage = true;
        }
    }
}
