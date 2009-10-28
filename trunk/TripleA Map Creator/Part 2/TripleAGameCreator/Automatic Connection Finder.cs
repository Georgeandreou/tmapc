using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Drawing.Drawing2D;

namespace TripleAGameCreator
{
    public partial class Automatic_Connection_Finder : Form
    {
        public Automatic_Connection_Finder()
        {
            InitializeComponent();
            Automatic_Connection_Finder.CheckForIllegalCrossThreadCalls = false;
            SetUpForAnotherScan();
        }
        public Form1 main = null;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isScanningForConnections)
            {
                button1.Text = "Cancel";
                this.Text = "Automatic Connection Finder - Initializing...";
                isScanningForConnections = true;
                lineWidth = Convert.ToInt32(numericUpDown1.Text);
                hasCanceled = false;
                groupBox1.Enabled = false;
                scanningThread.Start();
            }
            else
            {
                hasCanceled = true;
                SetUpForAnotherScan();
            }
        }

        public void SetUpForAnotherScan()
        {
            if(scanningThread != null && scanningThread.IsAlive)
                scanningThread.Abort();
            scanningThread = new Thread(new ThreadStart(startFindingConnections));
            scanningThread.Priority = ThreadPriority.Lowest;
            isScanningForConnections = false;
            this.Text = "Automatic Connection Finder";
            button1.Text = "Start";
            groupBox1.Enabled = true;
            toolStripProgressBar1.Value = 0;
        }
        public Thread scanningThread = null;
        public class Territory
        {
            public List<Point> points = new List<Point>();
            public string name = "";
            public Rectangle polygonBounds = new Rectangle(-1,-1,1,1);
        }
        public List<Territory> territories = new List<Territory>();
        public int lineWidth = 0;
        public bool isScanningForConnections = false;
        public bool hasCanceled = false;
        public void startFindingConnections()
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
                int gcCollectCountdown = 25;
                foreach (string cur in full)
                {
                    string tName = cur.Substring(0, cur.IndexOf("<")).Trim();
                    if (Step2Info.territories.ContainsKey(tName))
                    {
                        //MessageBox.Show(cur);
                        int curPointIndex = 0;
                        Territory t = new Territory();
                        t.name = tName;
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
                                    curPointIndex += curPointSubstring.Length;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            catch { break; }
                        }
                        if (increaseAccuracy.Checked && addPointsBeforeRunning.Checked && (!onlyAddPointsToSeaZones.Checked || Step2Info.territories[t.name].IsWater))
                        {
                            t.points = FillInPointsBetweenListOfPoints(t.points);
                            gcCollectCountdown--;
                            if (gcCollectCountdown <= 0)
                            {
                                GC.Collect();
                                gcCollectCountdown = 25;
                            }
                        }
                        territories.Add(t);
                    }
                }
                List<string> lines = new List<string>();
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = territories.Count;
                bool br = false;
                gcCollectCountdown = 25;
                while (territories.Count > 0)
                {
                    Territory cur = territories[0];
                    toolStripProgressBar1.Value++;
                    this.Text = String.Concat("Automatic Connection Finder - Processing ",toolStripProgressBar1.Value, " Of ", toolStripProgressBar1.Maximum);
                    if (increaseAccuracy.Checked && !addPointsBeforeRunning.Checked && (!onlyAddPointsToSeaZones.Checked || Step2Info.territories[cur.name].IsWater))
                    {
                        cur.points = FillInPointsBetweenListOfPoints(cur.points);
                        gcCollectCountdown--;
                        if (gcCollectCountdown <= 0)
                        {
                            GC.Collect();
                            gcCollectCountdown = 25;
                        }
                    }
                    int index = 0;
                    while(index < territories.Count)
                    {
                        Territory cur2 = territories[index];
                        if (cur2.name != cur.name)
                        {
                            if (checkPolygonBounds.Checked)
                            {
                                if (cur.polygonBounds.X == -1 && cur.polygonBounds.Y == -1 && cur.polygonBounds.Width == 1 && cur.polygonBounds.Height == 1)
                                    cur.polygonBounds = Inflate(Rectangle.Round(new GraphicsPath(cur.points.ToArray(), getPolygonBytes(cur.points)).GetBounds()), (int)numericUpDown1.Value);
                                if (cur2.polygonBounds.X == -1 && cur2.polygonBounds.Y == -1 && cur2.polygonBounds.Width == 1 && cur2.polygonBounds.Height == 1)
                                    cur2.polygonBounds = Inflate(Rectangle.Round(new GraphicsPath(cur2.points.ToArray(), getPolygonBytes(cur2.points)).GetBounds()), (int)numericUpDown1.Value);
                            }
                            if(!checkPolygonBounds.Checked || cur.polygonBounds.IntersectsWith(cur2.polygonBounds))
                            {
                                foreach (Point p in cur.points)
                                {
                                    foreach (Point p2 in cur2.points)
                                    {
                                        int xDiff = p.X - p2.X;
                                        int yDiff = p.Y - p2.Y;
                                        //if((xDiff > -33 && xDiff < 33 && yDiff > -33 && yDiff < 33))
                                        //MessageBox.Show("Points: " + p.X + "," + p.Y + "|" + p2.X + "," + p2.Y + ". Diff: " + xDiff + "," + yDiff);
                                        if (xDiff > -lineWidth && xDiff < lineWidth && yDiff > -lineWidth && yDiff < lineWidth)
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
                        index++;
                    }
                    territories.Remove(cur);
                }
                toolStripProgressBar1.Value = 0;
                this.Text = "Automatic Connection Finder";
                isScanningForConnections = false;
                button1.Text = "Start";
                groupBox1.Enabled = true;
                this.Close();
            }
            catch (Exception ex) { if (connections.Count == 0 && !(ex is ThreadAbortException)) if (MessageBox.Show("An error occured trying to find the connections. Make sure the polygons file for the map has no errors in it. (Like misspelling a territory name.) Do you want to view the error message", "Error Occured", MessageBoxButtons.YesNoCancel) == DialogResult.Yes) { exceptionViewerWindow.ShowInformationAboutException(ex, true); } }
        }
        ExceptionViewer exceptionViewerWindow = new ExceptionViewer();
        private byte[] getPolygonBytes(List<Point> list)
        {
            byte[] result = new byte[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = 1;
            }
            return result;
        }
        private Rectangle Inflate(Rectangle rect, int inflateAmount)
        {
            return new Rectangle(rect.X - inflateAmount, rect.Y - inflateAmount, rect.Size.Width + inflateAmount * 2, rect.Size.Height + inflateAmount * 2);
        }
        private List<Point> FillInPointsBetweenListOfPoints(List<Point> origPoints)
        {
            List<Point> points = new List<Point>();
            foreach (Point curPoint in origPoints)
            {
                if (points.Count > 0)
                {
                    if((curPoint.X != points[points.Count - 1].X || curPoint.Y != points[points.Count - 1].Y))
                        points.AddRange(GetPointsToAddBetweenTwoPoints(curPoint, points[points.Count - 1]));
                }
                points.Add(curPoint);
            }
            return points;
        }

        private List<Point> GetPointsToAddBetweenTwoPoints(Point startPoint,Point destPoint)
        {
            List<Point> points = new List<Point>();
            Size difference = new Size(destPoint.X - startPoint.X, destPoint.Y - startPoint.Y);
            PointF position = new PointF(startPoint.X,startPoint.Y);
            Ratio difRatioSimple = increaseOrDecreaseRatioUntilSimplified(new Ratio(difference.Width, difference.Height));
            int timesLooped = 0;
            int pixelDifference = (int)(ToPositive((int)(destPoint.X - position.X)) + ToPositive((int)(destPoint.Y - position.Y)));
            int lastPixelDifference = pixelDifference;
            while (pixelDifference > 0)
            {
                SizeF jumpSize = new SizeF(difRatioSimple.xRatio,difRatioSimple.yRatio);
                position += jumpSize;
                Point pointTA = new Point((int)position.X,(int)position.Y);
                points.Add(pointTA);
                pixelDifference = (int)(ToPositive((int)(destPoint.X - position.X)) + ToPositive((int)(destPoint.Y - position.Y)));
                timesLooped++;
                if (pixelDifference > lastPixelDifference)
                    break;
                lastPixelDifference = pixelDifference;
            }
            return points;
        }
        private int ToPositive(int num)
        {
            if (num < 0)
                return -num;
            else
                return num;
        }
        private Ratio increaseOrDecreaseRatioUntilSimplified(Ratio ratio)
        {
            if (ratio.xRatio == 0)
            {
                if (ratio.yRatio > 0)
                    return new Ratio(0, 1);
                else if (ratio.yRatio < 0)
                    return new Ratio(0, -1);
            }
            else if (ratio.yRatio == 0)
            {
                if (ratio.xRatio > 0)
                    return new Ratio(1, 0);
                else if (ratio.xRatio < 0)
                    return new Ratio(-1, 0);
            }
            else
            {
                Ratio result = new Ratio(ratio.xRatio, ratio.yRatio);
                bool xNegative = false;
                bool yNegative = false;
                if (result.xRatio < 0)
                {
                    result.xRatio = -result.xRatio;
                    xNegative = true;
                }
                if (result.yRatio < 0)
                {
                    result.yRatio = -result.yRatio;
                    yNegative = true;
                }
                if (result.xRatio < result.yRatio)
                {
                    double downsizeRatio = 1 / result.xRatio;
                    result.xRatio = 1;
                    result.yRatio = (float)(result.yRatio * downsizeRatio);
                }
                else if (result.yRatio < result.xRatio)
                {
                    double downsizeRatio = 1 / result.yRatio;
                    result.yRatio = 1;
                    result.xRatio = (float)(result.xRatio * downsizeRatio);
                }
                if (xNegative)
                {
                    result.xRatio = -result.xRatio;
                }
                if (yNegative)
                {
                    result.yRatio = -result.yRatio;
                }
                return result;
            }
            return new Ratio(0, 0);
        }
        private class Ratio
        {
            public float xRatio = 0F;
            public float yRatio = 0F;
            public Ratio(float x, float y)
            {
                xRatio = x;
                yRatio = y;
            }
        }
        public List<Connection> connections = new List<Connection>();
        public class Connection
        {
            public Territory t1 = new Territory();
            public Territory t2 = new Territory();
        }

        private void Automatic_Connection_Finder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isScanningForConnections)
            {
                e.Cancel = true;
                MessageBox.Show("Please wait for the Automatic Connection Finder to finish running.", "Still Running");
            }
            else
            {
                scanningThread.Abort();
                scanningThread = new Thread(new ThreadStart(startFindingConnections));
                scanningThread.Priority = ThreadPriority.Lowest;
            }
        }

        private void increaseAccuracy_CheckedChanged(object sender, EventArgs e)
        {
            onlyAddPointsToSeaZones.Enabled = increaseAccuracy.Checked;
            addPointsBeforeRunning.Enabled = increaseAccuracy.Checked;
            //checkPolygonBounds.Enabled = increaseAccuracy.Checked;
        }
    }
}
