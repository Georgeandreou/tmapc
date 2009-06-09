using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace TripleA_Game_Creator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            oldSize = Size;
            tabControl1.Tag = tabControl1.Size;
            panel2.Tag = panel2.Location;
            RefreshSettings();
        }
        public void RefreshSettings()
        {
            try
            {
                if (File.Exists(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/Settings.inf") && File.ReadAllText(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/Settings.inf").Contains("Java Heap Size=\""))
                {
                    string[] lines = File.ReadAllLines(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/Settings.inf");
                    foreach (string cur in lines)
                    {
                        if (cur.Contains("Java Heap Size=\""))
                        {
                            Settings.JavaHeapSize = Convert.ToInt32(cur.Substring(cur.IndexOf("Heap Size=\"") + 11, cur.Substring(cur.IndexOf("Heap Size=\"") + 11).IndexOf("\"")));
                        }
                    }
                }
                else
                {
                    try
                    {
                        List<string> lines = new List<string>();
                        lines.Add("Display Current Units When Entering New Units=\"true\"");
                        lines.Add("Java Heap Size=\"1000\"");
                        File.WriteAllLines(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/Settings.inf",lines.ToArray());
                    }
                    catch { }
                }
            }
            catch
            {
                MessageBox.Show("An error occured when trying to load the settings file for the program. Please make sure the \"Settings.inf\" file contains no errors.", "Error loading settings file.");
            }
        }
        public static class Settings
        {
            public static int JavaHeapSize = 1000;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Back();
            DisplayStepTabPage();
            //MessageBox.Show(Step2Info.territories.Count.ToString() + "/" + Step3Info.connections.Count.ToString());
            //MessageBox.Show(TerritoryDefinitionsImageDrawer.Controls.Count.ToString() + "/" + TerritoryConnectionsImageDrawer.Controls.Count.ToString());
        }
        int stepIndex = 0;
        int oldStepIndex = 0;
        private void Back()
        {
            if (stepIndex == 5)
            {
                if (MessageBox.Show("WARNING!! If the 'Auto-Placement Finder' is still running, some files will be incomplete. Do you want to continue?", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    Control oldControl = getControl("label" + stepIndex);
                    oldStepIndex = stepIndex;
                    if (stepIndex > 0)
                        stepIndex--;
                    Control newControl = getControl("label" + stepIndex);
                    oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
                    newControl.Font = new Font(newControl.Font, FontStyle.Bold);
                    try
                    {
                        foreach (string cur in Directory.GetFiles(cutPath))
                        {
                            File.Copy(cur, textBox3.Text + "/" + new FileInfo(cur).Name, true);
                        }
                        Directory.Delete(cutPath);
                    }
                    catch { }
                }
            }
            else if (stepIndex == 7)
            {
                if (MessageBox.Show("WARNING!! If the 'Relief Image Breaker' is still running, some files will be incomplete. Do you want to continue?", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    Control oldControl = getControl("label" + stepIndex);
                    oldStepIndex = stepIndex;
                    if (stepIndex > 0)
                        stepIndex--;
                    Control newControl = getControl("label" + stepIndex);
                    oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
                    newControl.Font = new Font(newControl.Font, FontStyle.Bold);
                    try
                    {
                        Directory.Move(cutPath + "/reliefTiles/", textBox3.Text + "/reliefTiles");
                        Directory.Delete(cutPath);
                    }
                    catch { }
                }
            }
            else
            {
                Control oldControl = getControl("label" + stepIndex);
                oldStepIndex = stepIndex;
                if (stepIndex > 0)
                    stepIndex--;
                Control newControl = getControl("label" + stepIndex);
                oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
                newControl.Font = new Font(newControl.Font, FontStyle.Bold);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Next();
            DisplayStepTabPage();
            //MessageBox.Show(Step2Info.territories.Count.ToString() + "/" + Step3Info.connections.Count.ToString());
            //MessageBox.Show(TerritoryDefinitionsImageDrawer.Controls.Count.ToString() + "/" + TerritoryConnectionsImageDrawer.Controls.Count.ToString());
        }
        private void Next()
        {
            if (stepIndex == 5)
            {
                if (MessageBox.Show("WARNING!! If the 'Auto-Placement Finder' is still running, some files will be incomplete. Do you want to continue?", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    foreach (Control cur in tabControl1.TabPages[stepIndex].Controls)
                    {
                        if (cur is TextBox && !(cur.Text.Trim().Length > 0))
                        {
                            MessageBox.Show("Fill in all the fields before going to the next step.", "Complete Current Step");
                            return;
                        }
                    }
                    Control oldControl = getControl("label" + stepIndex);
                    oldStepIndex = stepIndex;
                    if (stepIndex < 10)
                        stepIndex++;
                    Control newControl = getControl("label" + stepIndex);
                    oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
                    newControl.Font = new Font(newControl.Font, FontStyle.Bold);
                    try
                    {
                        Directory.Delete(cutPath);
                    }
                    catch { }
                }
            }
            else if (stepIndex == 7)
            {
                if (MessageBox.Show("WARNING!! If the 'Relief Image Breaker' is still running, some files will be incomplete. Do you want to continue?", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                {
                    foreach (Control cur in tabControl1.TabPages[stepIndex].Controls)
                    {
                        if (cur is TextBox && !(cur.Text.Trim().Length > 0))
                        {
                            MessageBox.Show("Fill in all the fields before going to the next step.", "Complete Current Step");
                            return;
                        }
                    }
                    Control oldControl = getControl("label" + stepIndex);
                    oldStepIndex = stepIndex;
                    if (stepIndex < 10)
                        stepIndex++;
                    Control newControl = getControl("label" + stepIndex);
                    oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
                    newControl.Font = new Font(newControl.Font, FontStyle.Bold);
                    try
                    {
                        Directory.Move(cutPath + "/reliefTiles/", textBox3.Text + "/reliefTiles");
                        Directory.Delete(cutPath);
                    }
                    catch { }
                }
            }
            else if (stepIndex < 8)
            {
                foreach (Control cur in tabControl1.TabPages[stepIndex].Controls)
                {
                    if (cur is TextBox && !(cur.Text.Trim().Length > 0))
                    {
                        MessageBox.Show("Fill in all the fields before going to the next step.", "Complete Current Step");
                        return;
                    }
                }
                Control oldControl = getControl("label" + stepIndex);
                oldStepIndex = stepIndex;
                if (stepIndex < 10)
                    stepIndex++;
                Control newControl = getControl("label" + stepIndex);
                oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
                newControl.Font = new Font(newControl.Font, FontStyle.Bold);
            }
            else
            {
                Control oldControl = getControl("label" + stepIndex);
                oldStepIndex = stepIndex;
                if (stepIndex < 10)
                    stepIndex++;
                Control newControl = getControl("label" + stepIndex);
                oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
                newControl.Font = new Font(newControl.Font, FontStyle.Bold);
            }

        }
        public Control getControl(string name)
        {
            foreach (Control cur in this.Controls)
            {
                if (cur.Name == name)
                    return cur;
            }
            return new Control();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                e.Cancel = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayStepTabPage();
        }
        private void DisplayStepTabPage()
        {
            tabControl1.SelectedIndex = stepIndex;
        }
        Size oldSize;
        private void Form1_Resize(object sender, EventArgs e)
        {
            Size change = new Size(Size.Width - oldSize.Width, Size.Height - oldSize.Height);
            tabControl1.Size = ((Size)tabControl1.Tag) + change;
            panel2.Location = ((Point)panel2.Tag) + new Size(change.Width / 2, change.Height);
        }
        int change = 0;
        private void button6_Click(object sender, EventArgs e)
        {
            change += 25;
            button8.Location += new Size(0, 25);
            button11.Location += new Size(0, 25);
            TextBox t1 = new TextBox() { Size = textBox2.Size, Location = new Point(textBox2.Location.X, textBox2.Location.Y + change) };
            t1.TextChanged += new EventHandler(t1_TextChanged);
            t1.DoubleClick += new EventHandler(t1_DoubleClick);
            Button b1 = new Button { Font = button3.Font, Text = button3.Text, Size = button3.Size, Location = new Point(button3.Location.X, button3.Location.Y + change) };
            b1.Click += new EventHandler(b1_Click);
            b1.Tag = t1;
            toolTip1.SetToolTip(b1, "Select a color using the color chooser window.");
            tabPage2.Controls.AddRange(new Control[] { new TextBox() { Size = textBox1.Size, Location = new Point(textBox1.Location.X, textBox1.Location.Y + change) },t1,b1});
        }

        void t1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ((TextBox)sender).BackColor = ColorTranslator.FromHtml(((TextBox)sender).Text);
            }
            catch { ((TextBox)sender).BackColor = Color.FromKnownColor(KnownColor.Control); }
        }

        void b1_Click(object sender, EventArgs e)
        {
            ColorDialog chooser = new ColorDialog();
            chooser.AllowFullOpen = true;
            chooser.AnyColor = true;
            chooser.FullOpen = true;
            chooser.SolidColorOnly = true;
            try
            {
               chooser.Color = ColorTranslator.FromHtml(((Button)sender).Text);
            }
            catch { }
            if (chooser.ShowDialog() != DialogResult.Cancel)
            {
                ((TextBox)((Button)sender).Tag).Text = ColorTranslator.ToHtml(chooser.Color).ToString();
            }
        }
        void t1_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog chooser = new ColorDialog();
            chooser.AllowFullOpen = true;
            chooser.AnyColor = true;
            chooser.FullOpen = true;
            chooser.SolidColorOnly = true;
            try
            {
                chooser.Color = ColorTranslator.FromHtml(((TextBox)sender).Text);
            }
            catch { }
            if (chooser.ShowDialog() != DialogResult.Cancel)
            {
                ((TextBox)sender).Text = ColorTranslator.ToHtml(chooser.Color).ToString();
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (change > 0)
            {
                tabPage2.Controls.RemoveAt(tabPage2.Controls.Count - 1);
                tabPage2.Controls.RemoveAt(tabPage2.Controls.Count - 1);
                tabPage2.Controls.RemoveAt(tabPage2.Controls.Count - 1);
                change -= 25;
                button8.Location += new Size(0, -25);
                button11.Location += new Size(0, -25);
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (oldStepIndex < stepIndex)
            {
                if (oldStepIndex == 1)
                {
                    if (File.Exists(textBox3.Text + "/map.properties"))
                    {
                        if (MessageBox.Show("Another map.properties file was found in the maps folder. Do you want to overwrite it with a new file consisting of what you just entered?", "Overwrite?", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                            return;
                    }
                    string playerName = "";
                    List<String> lines = new List<string>();
                    foreach (Control cur in tabControl1.TabPages[oldStepIndex].Controls)
                    {
                        if (cur is TextBox && cur.Name != "textBox3" && cur.Name != "textBox4" && cur.Name != "textBox7")
                        {
                            if (playerName == "")
                                playerName = cur.Text;
                            else
                            {
                                lines.Add("color." + playerName + "=" + cur.Text.Replace("#",""));
                                playerName = "";
                            }
                        }
                    }
                    lines.Add("units.scale=" + textBox4.Text);
                    lines.Add("map.hasRelief=" + comboBox1.Text.ToLower());
                    lines.Add("map.showCapitolMarkers=false");
                    lines.Add("map.width=" + textBox7.Text.Substring(0, textBox7.Text.IndexOf(",")));
                    lines.Add("map.height=" + textBox7.Text.Substring(textBox7.Text.IndexOf(",") + 1));
                    lines.Add("map.scrollWrapX=" + comboBox2.Text.ToLower());
                    File.WriteAllLines(textBox3.Text + "/map.properties", lines.ToArray());
                }
                if (stepIndex == 1)
                {
                    comboBox1.SelectedIndex = 1;
                    comboBox2.SelectedIndex = 1;
                }
            }
        }
        FolderBrowserDialog d = new FolderBrowserDialog();
        private void button12_Click(object sender, EventArgs e)
        {
            d.ShowNewFolderButton = true;
            if (d.ShowDialog() == DialogResult.OK)
                textBox3.Text = d.SelectedPath;
        }
        string directory = "";
        string file = "";
        private void button13_Click(object sender, EventArgs e)
        {
            directory = "";
            if (Directory.Exists("C:/Program Files/TripleA/") && new DirectoryInfo("C:/Program Files/TripleA/").GetDirectories().Length > 0)
                directory = "C:/Program Files/TripleA/";
            else
            {
                FolderBrowserDialog od = new FolderBrowserDialog();
                od.Description = "Please locate the TripleA Program's folder. (Where you installed TripleA)";
                od.ShowNewFolderButton = true;
                if (Directory.Exists(@"C:\Program Files\"))
                    od.SelectedPath = @"C:\Program Files\";
                if (od.ShowDialog() != DialogResult.Cancel)
                    directory = od.SelectedPath;
                else return;
            }
            bool isBaseDirectory = false;
            foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
            {
                if (cur.Name.ToLower() == "bin")
                {
                    isBaseDirectory = true;
                }
            }
            if (isBaseDirectory == false)
            {
                DirectoryInfo newestTripleAVersion = new DirectoryInfo(directory).GetDirectories()[0];
                foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                {
                    try
                    {
                        if (Convert.ToInt32(cur.Name.Substring(cur.Name.IndexOf("_")).Replace("_", "")) > Convert.ToInt32(newestTripleAVersion.Name.Substring(newestTripleAVersion.Name.IndexOf("_")).Replace("_", "")) && File.Exists(cur.FullName + "/bin/triplea.jar"))
                            newestTripleAVersion = cur;
                    }
                    catch { }
                }
                directory = newestTripleAVersion.FullName;
            }
            file = directory + "/bin/triplea.jar";
            if (!File.Exists(file))
            {
                OpenFileDialog d2 = new OpenFileDialog();
                d2.Multiselect = false;
                d2.InitialDirectory = directory;
                d2.Title = "Please locate the 'triplea.jar' file. (It should be in the TripleA Home folder, under a folder named 'bin'.)";
                if (d2.ShowDialog() == DialogResult.OK)
                    file = d2.FileName;
            }
            if(File.Exists(file))
            System.Diagnostics.Process.Start("java", "-Xmx" + Settings.JavaHeapSize + "m" + " -classpath \"" + file + "\" util/image/CenterPicker");
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            if (!File.Exists(file))
            {
                directory = "";
                if (Directory.Exists("C:/Program Files/TripleA/") && new DirectoryInfo("C:/Program Files/TripleA/").GetDirectories().Length > 0)
                    directory = "C:/Program Files/TripleA/";
                else
                {
                    FolderBrowserDialog od = new FolderBrowserDialog();
                    od.Description = "Please locate the TripleA Program's folder. (Where you installed TripleA)";
                    od.ShowNewFolderButton = true;
                    if (Directory.Exists(@"C:\Program Files\"))
                        od.SelectedPath = @"C:\Program Files\";
                    if (od.ShowDialog() != DialogResult.Cancel)
                        directory = od.SelectedPath;
                    else return;
                }
                bool isBaseDirectory = false;
                foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                {
                    if (cur.Name.ToLower() == "bin")
                    {
                        isBaseDirectory = true;
                    }
                }
                if (isBaseDirectory == false)
                {
                    DirectoryInfo newestTripleAVersion = new DirectoryInfo(directory).GetDirectories()[0];
                    foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                    {
                        try
                        {
                            if (Convert.ToInt32(cur.Name.Substring(cur.Name.IndexOf("_")).Replace("_", "")) > Convert.ToInt32(newestTripleAVersion.Name.Substring(newestTripleAVersion.Name.IndexOf("_")).Replace("_", "")) && File.Exists(cur.FullName + "/bin/triplea.jar"))
                                newestTripleAVersion = cur;
                        }
                        catch { }
                    }
                    directory = newestTripleAVersion.FullName;
                }
                file = directory + "/bin/triplea.jar";
                if (!File.Exists(file))
                {
                    OpenFileDialog d2 = new OpenFileDialog();
                    d2.Multiselect = false;
                    d2.InitialDirectory = directory;
                    d2.Title = "Please locate the 'triplea.jar' file. (It should be in the TripleA Home folder, under a folder named 'bin'.)";
                    if (d2.ShowDialog() == DialogResult.OK)
                        file = d2.FileName;
                }
            }
            if (File.Exists(file))
                System.Diagnostics.Process.Start("java", "-Xmx" + Settings.JavaHeapSize + "m" + " -classpath \"" + file + "\" util/image/PolygonGrabber");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (!File.Exists(file))
            {
                directory = "";
                if (Directory.Exists("C:/Program Files/TripleA/") && new DirectoryInfo("C:/Program Files/TripleA/").GetDirectories().Length > 0)
                    directory = "C:/Program Files/TripleA/";
                else
                {
                    FolderBrowserDialog od = new FolderBrowserDialog();
                    od.Description = "Please locate the TripleA Program's folder. (Where you installed TripleA)";
                    od.ShowNewFolderButton = true;
                    if (Directory.Exists(@"C:\Program Files\"))
                        od.SelectedPath = @"C:\Program Files\";
                    if (od.ShowDialog() != DialogResult.Cancel)
                        directory = od.SelectedPath;
                    else return;
                }
                bool isBaseDirectory = false;
                foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                {
                    if (cur.Name.ToLower() == "bin")
                    {
                        isBaseDirectory = true;
                    }
                }
                if (isBaseDirectory == false)
                {
                    DirectoryInfo newestTripleAVersion = new DirectoryInfo(directory).GetDirectories()[0];
                    foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                    {
                        try
                        {
                            if (Convert.ToInt32(cur.Name.Substring(cur.Name.IndexOf("_")).Replace("_", "")) > Convert.ToInt32(newestTripleAVersion.Name.Substring(newestTripleAVersion.Name.IndexOf("_")).Replace("_", "")) && File.Exists(cur.FullName + "/bin/triplea.jar"))
                                newestTripleAVersion = cur;
                        }
                        catch { }
                    }
                    directory = newestTripleAVersion.FullName;
                }
                file = directory + "/bin/triplea.jar";
                if (!File.Exists(file))
                {
                    OpenFileDialog d2 = new OpenFileDialog();
                    d2.Multiselect = false;
                    d2.InitialDirectory = directory;
                    d2.Title = "Please locate the 'triplea.jar' file. (It should be in the TripleA Home folder, under a folder named 'bin'.)";
                    if (d2.ShowDialog() == DialogResult.OK)
                        file = d2.FileName;
                }
            }
            if (File.Exists(file))
                System.Diagnostics.Process.Start("java", "-Xmx" + Settings.JavaHeapSize + "m" + " -classpath \"" + file + "\" util/image/PlacementPicker");
        }
        string cutPath = "";
        private void button16_Click(object sender, EventArgs e)
        {
            if (!File.Exists(file))
            {
                directory = "";
                if (Directory.Exists("C:/Program Files/TripleA/") && new DirectoryInfo("C:/Program Files/TripleA/").GetDirectories().Length > 0)
                    directory = "C:/Program Files/TripleA/";
                else
                {
                    FolderBrowserDialog od = new FolderBrowserDialog();
                    od.Description = "Please locate the TripleA Program's folder. (Where you installed TripleA)";
                    od.ShowNewFolderButton = true;
                    if (Directory.Exists(@"C:\Program Files\"))
                        od.SelectedPath = @"C:\Program Files\";
                    if (od.ShowDialog() != DialogResult.Cancel)
                        directory = od.SelectedPath;
                    else return;
                }
                bool isBaseDirectory = false;
                foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                {
                    if (cur.Name.ToLower() == "bin")
                    {
                        isBaseDirectory = true;
                    }
                }
                if (isBaseDirectory == false)
                {
                    DirectoryInfo newestTripleAVersion = new DirectoryInfo(directory).GetDirectories()[0];
                    foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                    {
                        try
                        {
                            if (Convert.ToInt32(cur.Name.Substring(cur.Name.IndexOf("_")).Replace("_", "")) > Convert.ToInt32(newestTripleAVersion.Name.Substring(newestTripleAVersion.Name.IndexOf("_")).Replace("_", "")) && File.Exists(cur.FullName + "/bin/triplea.jar"))
                                newestTripleAVersion = cur;
                        }
                        catch { }
                    }
                    directory = newestTripleAVersion.FullName;
                }
                file = directory + "/bin/triplea.jar";
                if (!File.Exists(file))
                {
                    OpenFileDialog d2 = new OpenFileDialog();
                    d2.Multiselect = false;
                    d2.InitialDirectory = directory;
                    d2.Title = "Please locate the 'triplea.jar' file. (It should be in the TripleA Home folder, under a folder named 'bin'.)";
                    if (d2.ShowDialog() == DialogResult.OK)
                        file = d2.FileName;
                }
            }
            if (File.Exists(file))
            {
                cutPath = Directory.GetParent(file.Substring(0, file.LastIndexOf("/"))).FullName + "/maps/temp/";
                File.Create(textBox3.Text + "/place.txt").Close();
                Directory.CreateDirectory(cutPath);
                foreach (string cur in Directory.GetFiles(textBox3.Text))
                {
                    File.Copy(cur, cutPath + new FileInfo(cur).Name, true);
                }
                if (Directory.Exists(textBox3.Text + @"\misc"))
                {
                    Directory.CreateDirectory(cutPath + @"\misc");
                    foreach (FileInfo cur in new DirectoryInfo(textBox3.Text + @"\misc").GetFiles())
                    {
                        File.Copy(cur.FullName, cutPath + @"\misc\" + cur.Name);
                    }
                }
                System.Diagnostics.Process.Start("java", "-Xmx" + Settings.JavaHeapSize + "m" + " -classpath \"" + file + "\" util/image/AutoPlacementFinder " + getUnitScale(new DirectoryInfo(textBox3.Text)).ToString());
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (!File.Exists(file))
            {
                directory = "";
                if (Directory.Exists("C:/Program Files/TripleA/") && new DirectoryInfo("C:/Program Files/TripleA/").GetDirectories().Length > 0)
                    directory = "C:/Program Files/TripleA/";
                else
                {
                    FolderBrowserDialog od = new FolderBrowserDialog();
                    od.Description = "Please locate the TripleA Program's folder. (Where you installed TripleA)";
                    od.ShowNewFolderButton = true;
                    if (Directory.Exists(@"C:\Program Files\"))
                        od.SelectedPath = @"C:\Program Files\";
                    if (od.ShowDialog() != DialogResult.Cancel)
                        directory = od.SelectedPath;
                    else return;
                }
                bool isBaseDirectory = false;
                foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                {
                    if (cur.Name.ToLower() == "bin")
                    {
                        isBaseDirectory = true;
                    }
                }
                if (isBaseDirectory == false)
                {
                    DirectoryInfo newestTripleAVersion = new DirectoryInfo(directory).GetDirectories()[0];
                    foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                    {
                        try
                        {
                            if (Convert.ToInt32(cur.Name.Substring(cur.Name.IndexOf("_")).Replace("_", "")) > Convert.ToInt32(newestTripleAVersion.Name.Substring(newestTripleAVersion.Name.IndexOf("_")).Replace("_", "")) && File.Exists(cur.FullName + "/bin/triplea.jar"))
                                newestTripleAVersion = cur;
                        }
                        catch { }
                    }
                    directory = newestTripleAVersion.FullName;
                }
                file = directory + "/bin/triplea.jar";
                if (!File.Exists(file))
                {
                    OpenFileDialog d2 = new OpenFileDialog();
                    d2.Multiselect = false;
                    d2.InitialDirectory = directory;
                    d2.Title = "Please locate the 'triplea.jar' file. (It should be in the TripleA Home folder, under a folder named 'bin'.)";
                    if (d2.ShowDialog() == DialogResult.OK)
                        file = d2.FileName;
                }
            }
            if (File.Exists(file))
                System.Diagnostics.Process.Start("java", "-Xmx" + Settings.JavaHeapSize + "m" + " -classpath \"" + file + "\" util/image/TileImageBreaker");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (!File.Exists(file))
            {
                directory = "";
                if (Directory.Exists("C:/Program Files/TripleA/") && new DirectoryInfo("C:/Program Files/TripleA/").GetDirectories().Length > 0)
                    directory = "C:/Program Files/TripleA/";
                else
                {
                    FolderBrowserDialog od = new FolderBrowserDialog();
                    od.Description = "Please locate the TripleA Program's folder. (Where you installed TripleA)";
                    od.ShowNewFolderButton = true;
                    if (Directory.Exists(@"C:\Program Files\"))
                        od.SelectedPath = @"C:\Program Files\";
                    if (od.ShowDialog() != DialogResult.Cancel)
                        directory = od.SelectedPath;
                    else return;
                }
                bool isBaseDirectory = false;
                foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                {
                    if (cur.Name.ToLower() == "bin")
                    {
                        isBaseDirectory = true;
                    }
                }
                if (isBaseDirectory == false)
                {
                    DirectoryInfo newestTripleAVersion = new DirectoryInfo(directory).GetDirectories()[0];
                    foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                    {
                        try
                        {
                            if (Convert.ToInt32(cur.Name.Substring(cur.Name.IndexOf("_")).Replace("_", "")) > Convert.ToInt32(newestTripleAVersion.Name.Substring(newestTripleAVersion.Name.IndexOf("_")).Replace("_", "")) && File.Exists(cur.FullName + "/bin/triplea.jar"))
                                newestTripleAVersion = cur;
                        }
                        catch { }
                    }
                    directory = newestTripleAVersion.FullName;
                }
                file = directory + "/bin/triplea.jar";
                if (!File.Exists(file))
                {
                    OpenFileDialog d2 = new OpenFileDialog();
                    d2.Multiselect = false;
                    d2.InitialDirectory = directory;
                    d2.Title = "Please locate the 'triplea.jar' file. (It should be in the TripleA Home folder, under a folder named 'bin'.)";
                    if (d2.ShowDialog() == DialogResult.OK)
                        file = d2.FileName;
                }
            }
            cutPath = Directory.GetParent(file.Substring(0, file.LastIndexOf("/"))).FullName + "/maps/temp/";
            Directory.CreateDirectory(cutPath + "/reliefTiles/");
            foreach (string cur in Directory.GetFiles(textBox3.Text))
            {
                File.Copy(cur, cutPath + new FileInfo(cur).Name, true);
            }
            if (File.Exists(file))
                System.Diagnostics.Process.Start("java", "-Xmx" + Settings.JavaHeapSize + "m" + " -classpath \"" + file + "\" util/image/ReliefImageBreaker");
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (!File.Exists(file))
            {
                directory = "";
                if (Directory.Exists("C:/Program Files/TripleA/") && new DirectoryInfo("C:/Program Files/TripleA/").GetDirectories().Length > 0)
                    directory = "C:/Program Files/TripleA/";
                else
                {
                    FolderBrowserDialog od = new FolderBrowserDialog();
                    od.Description = "Please locate the TripleA Program's folder. (Where you installed TripleA)";
                    od.ShowNewFolderButton = true;
                    if(Directory.Exists(@"C:\Program Files\"))
                    od.SelectedPath = @"C:\Program Files\";
                    if (od.ShowDialog() != DialogResult.Cancel)
                        directory = od.SelectedPath;
                    else return;
                }
                bool isBaseDirectory = false;
                foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                {
                    if (cur.Name.ToLower() == "bin")
                    {
                        isBaseDirectory = true;
                    }
                }
                if (isBaseDirectory == false)
                {
                    DirectoryInfo newestTripleAVersion = new DirectoryInfo(directory).GetDirectories()[0];
                    foreach (DirectoryInfo cur in new DirectoryInfo(directory).GetDirectories())
                    {
                        try
                        {
                            if (Convert.ToInt32(cur.Name.Substring(cur.Name.IndexOf("_")).Replace("_", "")) > Convert.ToInt32(newestTripleAVersion.Name.Substring(newestTripleAVersion.Name.IndexOf("_")).Replace("_", "")) && File.Exists(cur.FullName + "/bin/triplea.jar"))
                                newestTripleAVersion = cur;
                        }
                        catch { }
                    }
                    directory = newestTripleAVersion.FullName;
                }
                file = directory + "/bin/triplea.jar";
                if (!File.Exists(file))
                {
                    OpenFileDialog d2 = new OpenFileDialog();
                    d2.Multiselect = false;
                    d2.InitialDirectory = directory;
                    d2.Title = "Please locate the 'triplea.jar' file. (It should be in the TripleA Home folder, under a folder named 'bin'.)";
                    if (d2.ShowDialog() == DialogResult.OK)
                        file = d2.FileName;
                }
            }
            if (File.Exists(file))
                System.Diagnostics.Process.Start("java", "-Xmx" + Settings.JavaHeapSize + "m" + " -classpath \"" + file + "\" util/image/ImageShrinker");
        }
        public static double getUnitScale(DirectoryInfo mapFolder)
        {
            double result = 0;
            if (File.Exists(mapFolder.FullName + @"\map.properties"))
            {
                string[] lines = File.ReadAllLines(mapFolder.FullName + @"\map.properties");
                foreach (string cur in lines)
                {
                    if (cur.ToLower().Contains("units.scale="))
                    {
                        result = Convert.ToDouble(cur.ToLower().Substring(cur.ToLower().IndexOf(".scale=") + 7));
                    }
                }
            }
            else
            {
                OpenFileDialog open = new OpenFileDialog();
                open.CheckFileExists = true;
                open.DefaultExt = ".properties";
                open.Filter = "Map Properties Files|*.properties|All files (*.*)|*.*";
                open.InitialDirectory = mapFolder.Parent.FullName;
                open.Multiselect = false;
                open.Title = "Please select the map.properties file for the map.";
                if (open.ShowDialog() != DialogResult.Cancel)
                {
                    string[] lines = File.ReadAllLines(open.FileName);
                    foreach (string cur in lines)
                    {
                        if (cur.ToLower().Contains("units.scale="))
                        {
                            result = Convert.ToDouble(cur.ToLower().Substring(cur.ToLower().IndexOf(".scale=") + 7));
                        }
                    }
                }
            }
            return result;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            ColorDialog chooser = new ColorDialog();
            chooser.AllowFullOpen = true;
            chooser.AnyColor = true;
            chooser.FullOpen = true;
            chooser.SolidColorOnly = true;
            try
            {
                chooser.Color = ColorTranslator.FromHtml(textBox2.Text);
            }
            catch { }
            if (chooser.ShowDialog() != DialogResult.Cancel)
            {
                textBox2.Text = ColorTranslator.ToHtml(chooser.Color).ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ((TextBox)sender).BackColor = ColorTranslator.FromHtml(((TextBox)sender).Text);
            }
            catch { ((TextBox)sender).BackColor = Color.FromKnownColor(KnownColor.Control); }
        }
        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog chooser = new ColorDialog();
            chooser.AllowFullOpen = true;
            chooser.AnyColor = true;
            chooser.FullOpen = true;
            chooser.SolidColorOnly = true;
            try
            {
                chooser.Color = ColorTranslator.FromHtml(textBox2.Text);
            }
            catch { }
            if (chooser.ShowDialog() != DialogResult.Cancel)
            {
                textBox2.Text = ColorTranslator.ToHtml(chooser.Color).ToString();
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (File.Exists(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + @"\TripleA Map Creator Part 2.exe"))
                System.Diagnostics.Process.Start((new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + @"\TripleA Map Creator Part 2.exe"));
            else
                MessageBox.Show("Unable to find Part 2 of the Map Creator. The program will now close.");
            Environment.Exit(0);
        }
    }
}
