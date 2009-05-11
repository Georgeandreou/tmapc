using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace TripleAGameCreator
{
    public partial class Automatic_Connection_Finder : Form
    {
        public Automatic_Connection_Finder()
        {
            InitializeComponent();
            t = new Thread(new ThreadStart(do1));
        }
        public Form1 main = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (d)
            {
                button1.Text = "Cancel";
                d = false;
                Automatic_Connection_Finder.CheckForIllegalCrossThreadCalls = false;
                n = Convert.ToInt32(textBox1.Text);
                t.Start();
            }
            else
            {
                t.Abort();
                t = new Thread(new ThreadStart(do1));
                d = true;
                button1.Text = "Start";
                toolStripProgressBar1.Value = 0;
            }
        }
        Thread t = null;
        public class Territory
        {
            public List<Point> points = new List<Point>();
            public string name = "";
        }
        public List<Territory> territories = new List<Territory>();
        int n = 0;
        bool d = true;
        public void do1()
        {
            try
            {
                territories.Clear();
                connections.Clear();
                string[] full;
                Step1Info.LoadedFile.Replace("/",@"\");
                Step1Info.MapImageLocation.Replace("/",@"\");
                if (Step1Info.MapImageLocation.Contains(@"\"))
                {
                    if (File.Exists(Step1Info.MapImageLocation.Substring(0, Step1Info.MapImageLocation.LastIndexOf(@"\")) + "/polygons.txt"))
                        full = File.ReadAllLines(Step1Info.MapImageLocation.Substring(0, Step1Info.MapImageLocation.LastIndexOf(@"\")) + "/polygons.txt");
                    else
                    {
                        if (File.Exists(new FileInfo(Step1Info.MapImageLocation.Substring(0, Step1Info.MapImageLocation.LastIndexOf(@"\"))).Directory.Parent + "/polygons.txt"))
                        {
                            full = File.ReadAllLines(new FileInfo(Step1Info.MapImageLocation.Substring(0, Step1Info.MapImageLocation.LastIndexOf(@"\"))).Directory.Parent + "/polygons.txt");
                        }
                        else
                        {
                            OpenFileDialog open = new OpenFileDialog();
                            open.Title = "Unable to locate the needed polygons file. Please select the 'polygons.txt' file for the map.";
                            open.Filter = "Text Files|*.txt|All files (*.*)|*.*";
                            if (open.ShowDialog() != DialogResult.Cancel)
                                full = File.ReadAllLines(open.FileName);
                            else
                            {
                                MessageBox.Show(this, "You need to specify a polygons file for the program to be able to find the connectons.", "Unable To Find Connections");
                                this.Close();
                                return;
                            }
                        }
                    }
                }
                else if (Step1Info.LoadedFile.Contains(@"\"))
                {
                    if (File.Exists(Step1Info.LoadedFile.Substring(0, Step1Info.LoadedFile.LastIndexOf(@"\")) + "/polygons.txt"))
                        full = File.ReadAllLines(Step1Info.LoadedFile.Substring(0, Step1Info.LoadedFile.LastIndexOf(@"\")) + "/polygons.txt");
                    else
                    {
                        if (File.Exists(new FileInfo(Step1Info.LoadedFile.Substring(0, Step1Info.LoadedFile.LastIndexOf(@"\"))).Directory.Parent + "/polygons.txt"))
                        {
                            full = File.ReadAllLines(new FileInfo(Step1Info.LoadedFile.Substring(0, Step1Info.LoadedFile.LastIndexOf(@"\"))).Directory.Parent + "/polygons.txt");
                        }
                        else
                        {
                            OpenFileDialog open = new OpenFileDialog();
                            open.Title = "Unable to locate the needed polygons file. Please select the 'polygons.txt' file for the map.";
                            open.Filter = "Text Files|*.txt|All files (*.*)|*.*";
                            if (open.ShowDialog() != DialogResult.Cancel)
                                full = File.ReadAllLines(open.FileName);
                            else
                            {
                                MessageBox.Show(this,"You need to specify a polygons file for the program to be able to find the connectons.", "Unable To Find Connections");
                                this.Close();
                                return;
                            }
                        }
                    }
                }
                else
                {
                    OpenFileDialog open = new OpenFileDialog();
                    open.Title = "Unable to locate the needed polygons file. Please select the 'polygons.txt' file for the map.";
                    open.Filter = "Text Files|*.txt|All files (*.*)|*.*";
                    if (open.ShowDialog() != DialogResult.Cancel)
                        full = File.ReadAllLines(open.FileName);
                    else
                    {
                        MessageBox.Show(this, "You need to specify a polygons file for the program to be able to find the connectons.", "Unable To Find Connections");
                        this.Close();
                        return;
                    }
                }
                foreach (string cur in full)
                {
                    //MessageBox.Show(cur);
                    int curPointIndex = 0;
                    Territory t = new Territory();
                    while (true)
                    {
                        try
                        {
                            curPointIndex = cur.Substring(curPointIndex).IndexOf("(") + curPointIndex;
                            if (curPointIndex > -1)
                            {
                                string curPointSubstring = cur.Substring(curPointIndex, cur.Substring(curPointIndex).IndexOf(")"));
                                Point curPoint = new Point(Convert.ToInt32(curPointSubstring.Substring(1, curPointSubstring.IndexOf(",") - 1)), Convert.ToInt32(curPointSubstring.Substring(curPointSubstring.IndexOf(",") + 1, curPointSubstring.Length - (curPointSubstring.IndexOf(",") + 1))));
                                t.points.Add(curPoint);
                                t.name = cur.Substring(0, cur.IndexOf("<")).Trim();
                                curPointIndex += curPointSubstring.Length;
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch { break; }
                    }
                    territories.Add(t);
                }
                List<string> lines = new List<string>();
                int num = 0;
                foreach (Territory cur in territories)
                {
                    num++;
                }
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = num;
                bool br = false;
                foreach (Territory cur in territories)
                {
                    toolStripProgressBar1.Value++;
                    foreach (Territory cur2 in territories)
                    {
                        if (cur2.name != cur.name && !done.Contains(cur2))
                        {
                            foreach (Point p in cur.points)
                            {
                                foreach (Point p2 in cur2.points)
                                {
                                    int xDiff = p.X - p2.X;
                                    int yDiff = p.Y - p2.Y;
                                    //if((xDiff > -33 && xDiff < 33 && yDiff > -33 && yDiff < 33))
                                    //MessageBox.Show("Points: " + p.X + "," + p.Y + "|" + p2.X + "," + p2.Y + ". Diff: " + xDiff + "," + yDiff);
                                    if (xDiff > -n && xDiff < n && yDiff > -n && yDiff < n)
                                    {
                                        connections.Add(new Connection() { t1 = cur, t2 = cur2 });
                                        br = true;
                                        break;
                                    }
                                }
                                if (br)
                                {
                                    br = false;
                                    break;
                                }
                            }
                        }
                    }
                    done.Add(cur);
                }
                toolStripProgressBar1.Value = 0;
                d = true;
                button1.Text = "Start";
                this.Close();
                //foreach (Connection cur in connections)
                //{
                //    lines.Add("                <connection t1=\"" + cur.t1.name + "\" t2=\"" + cur.t2.name + "\"/>");
                //}
                //File.WriteAllLines(Step1Info.MapImageLocation.Substring(0, Step1Info.MapImageLocation.LastIndexOf(@"\")) + "/cons.txt", lines.ToArray());
            }
            catch{ if(connections.Count == 0) MessageBox.Show("An error occured trying to find the connections. Make sure you selected a valid polygons file and try again.", "Error Occured"); }
        }
        List<Territory> done = new List<Territory>();
        public List<Connection> connections = new List<Connection>();
        public class Connection
        {
            public Territory t1 = new Territory();
            public Territory t2 = new Territory();
        }

        private void Automatic_Connection_Finder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!d)
            {
                e.Cancel = true;
                MessageBox.Show("Please wait for the Automatic Connection Finder to finish running.", "Still Running");
            }
            else
            {
                t.Abort();
                t = new Thread(new ThreadStart(do1));
            }
        }
    }
}
