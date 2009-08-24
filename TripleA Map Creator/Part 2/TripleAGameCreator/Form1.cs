using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Threading;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Net;

namespace TripleAGameCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            origSize = Size;
            tabControl1.Tag = tabControl1.Size;
            panel2.Tag = panel2.Location;
            panel17.Tag = panel17.Location;
            button26.Tag = button26.Location;
            button15.Tag = button15.Location;
            button16.Tag = button16.Location;
            panel4.Tag = panel4.Size;
            panel10.Tag = panel10.Size;
            panel11.Tag = panel11.Size;
            panel12.Tag = panel12.Size;
            panel13.Tag = panel13.Size;
            panel14.Tag = panel14.Size;
            tabControl2.Tag = tabControl2.Size;
            tabControl3.Tag = tabControl3.Size;
            mapNotesTextBox.Tag = mapNotesTextBox.Size;
            RefreshSettings();
            CheckForUpdates();
        }
        private Version usersVersion = new Version(1, 0, 0, 8);
        public void CheckForUpdates()
        {
            Thread t = new Thread(new ThreadStart(update));
            t.Priority = ThreadPriority.Lowest;
            t.Start();
        }
        private void update()
        {
            WebClient client = new WebClient(); //http://tmapc.googlecode.com/files/TripleA%20Map%20Creator%20v1.0.0.8.zip
            Version currentCheckingVersion = usersVersion;
            Version newestVersionAvailable = usersVersion;
            bool doBreak = false;

            for (int buildI = usersVersion.Build + 1; buildI < 10; buildI++)
            {
                try
                {
                    Version checkVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor, buildI, 0);
                    Stream s = client.OpenRead("http://tmapc.googlecode.com/files/TripleA%20Map%20Creator%20v" + checkVersion.ToString() + ".zip");
                    newestVersionAvailable = checkVersion;
                    currentCheckingVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor, buildI, 1);
                    s.Close();
                }
                catch { }
            }
            while (!doBreak)
            {
                try
                {
                    Stream s = client.OpenRead("http://tmapc.googlecode.com/files/TripleA%20Map%20Creator%20v" + currentCheckingVersion.ToString() + ".zip");
                    newestVersionAvailable = currentCheckingVersion;
                    currentCheckingVersion = new Version(currentCheckingVersion.Major, currentCheckingVersion.Minor, currentCheckingVersion.Build, currentCheckingVersion.Revision + 1);
                    s.Close();
                }
                catch
                {
                    break;
                }
            }
            if (Convert.ToInt32(usersVersion.ToString().Replace(".", "")) < Convert.ToInt32(newestVersionAvailable.ToString().Replace(".", "")))
            {
                MessageBox.Show("There is a newer version of the Map Creator available.\r\nYour version: " + usersVersion.ToString() + ".\r\nNewest Version: " + newestVersionAvailable.ToString() + ".\r\n\r\nTo download the latest version, please go to \"http://code.google.com/p/tmapc/downloads/list\" and click on the latest download.", "Checking For Updates");
            }
        }
        public void RefreshSettings()
        {
            try
            {
                if (File.Exists(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/Settings.inf"))
                {
                    string[] lines = File.ReadAllLines(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/Settings.inf");
                    foreach (string cur in lines)
                    {
                        if (cur.Contains("Display Current Units When Entering New Units=\""))
                        {
                            Settings.DisplayCurrentUnitsWhenEnteringNewUnits = Convert.ToBoolean(cur.Substring(cur.IndexOf("New Units=\"") + 11, cur.Substring(cur.IndexOf("New Units=\"") + 11).IndexOf("\"")));
                        }
                        if (cur.Contains("Stop Loading XML File When Error Is Found=\""))
                        {
                            Settings.StopLoadingXMLWhenErrorFound = Convert.ToBoolean(cur.Substring(cur.IndexOf("Is Found=\"") + 10, cur.Substring(cur.IndexOf("Is Found=\"") + 10).IndexOf("\"")));
                        }
                    }
                }
                else
                {
                    if (MessageBox.Show("The program is unable to locate the settings file. Do you want the program to re-create the settings file?", "Can't Locate Settings", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        try
                        {
                            List<string> lines = new List<string>();
                            lines.Add("Display Current Units When Entering New Units=\"true\"");
                            lines.Add("Stop Loading XML File When Error Is Found=\"false\"");
                            lines.Add("Java Heap Size=\"1000\"");
                            File.WriteAllLines(new FileInfo(Assembly.GetExecutingAssembly().Location).Directory + "/Settings.inf", lines.ToArray());
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occured when trying to load the settings file for the program. Please make sure the \"Settings.inf\" file contains no errors.", "Error loading settings file.");
            }
        }
        public static class Settings
        {
            public static bool DisplayCurrentUnitsWhenEnteringNewUnits = false;
            public static bool StopLoadingXMLWhenErrorFound = false;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Back();
            DisplayStepTabPage();
            //MessageBox.Show(Step2Info.territories.Count.ToString() + "/" + Step3Info.connections.Count.ToString());
            //MessageBox.Show(TerritoryDefinitionsImageDrawer.Controls.Count.ToString() + "/" + TerritoryConnectionsImageDrawer.Controls.Count.ToString());
        }
        int stepIndex = 1;
        int oldStepIndex = 1;
        bool surpress1 = false;
        bool surpress2 = false;
        private string defaultWindowText = "TripleA Map Creator - Part 2";
        public void UpdateWindowText()
        {
            switch (stepIndex - 1)
            {
                case 0:
                    {
                        this.Text = defaultWindowText;
                        break;
                    }
                case 1:
                    {
                        this.Text = defaultWindowText + " (Approximately 3% Done With Part 2)";
                        break;
                    }
                case 2:
                    {
                        this.Text = defaultWindowText + " (Approximately 10% Done With Part 2)";
                        break;
                    }
                case 3:
                    {
                        this.Text = defaultWindowText + " (Approximately 30% Done With Part 2)";
                        break;
                    }
                case 4:
                    {
                        this.Text = defaultWindowText + " (Approximately 32% Done With Part 2)";
                        break;
                    }
                case 5:
                    {
                        this.Text = defaultWindowText + " (Approximately 35% Done With Part 2)";
                        break;
                    }
                case 6:
                    {
                        this.Text = defaultWindowText + " (Approximately 38% Done With Part 2)";
                        break;
                    }
                case 7:
                    {
                        this.Text = defaultWindowText + " (Approximately 44% Done With Part 2)";
                        break;
                    }
                case 8:
                    {
                        this.Text = defaultWindowText + " (Approximately 46% Done With Part 2)";
                        break;
                    }
                case 9:
                    {
                        this.Text = defaultWindowText + " (Approximately 50% Done With Part 2)";
                        break;
                    }
                case 10:
                    {
                        this.Text = defaultWindowText + " (Approximately 53% Done With Part 2)";
                        break;
                    }
                case 11:
                    {
                        this.Text = defaultWindowText + " (Approximately 70% Done With Part 2)";
                        break;
                    }
                case 12:
                    {
                        this.Text = defaultWindowText + " (Approximately 72% Done With Part 2)";
                        break;
                    }
                case 13:
                    {
                        this.Text = defaultWindowText + " (Approximately 80% Done With Part 2)";
                        break;
                    }
                case 14:
                    {
                        this.Text = defaultWindowText + " (Approximately 97% Done With Part 2)";
                        break;
                    }
                case 15:
                    {
                        this.Text = defaultWindowText + " (Completely Done With Part 2)";
                        break;
                    }
            }
        }
        private void Back()
        {
            Control oldControl = getControl("label" + stepIndex);
            oldStepIndex = stepIndex;
            if (stepIndex > 1)
                stepIndex--;
            Control newControl = getControl("label" + stepIndex);
            oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
            newControl.Font = new Font(newControl.Font, FontStyle.Bold);
            UpdateWindowText();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Next();
                DisplayStepTabPage();
            }
            catch { }
            //MessageBox.Show(Step2Info.territories.Count.ToString() + "/" + Step3Info.connections.Count.ToString());
            //MessageBox.Show(TerritoryDefinitionsImageDrawer.Controls.Count.ToString() + "/" + TerritoryConnectionsImageDrawer.Controls.Count.ToString());
        }
        private void Next()
        {
            if (stepIndex == 1)
                t.Abort();
            foreach (Control cur in tabControl1.TabPages[stepIndex - 1].Controls)
            {
                if ((cur is TextBox  || cur is ComboBox) && !(cur.Text.Trim().Length > 0) && cur.Name != "textBox7" && cur.Name != "textBox8")
                {
                    MessageBox.Show("Fill in all the fields before going to the next step.", "Complete Current Step");
                    return;
                }
                else if (cur is TabControl)
                {
                    foreach (TabPage cur2 in ((TabControl)cur).TabPages)
                    {
                        foreach (Control cur3 in cur2.Controls)
                        {
                            if ((cur3 is TextBox || cur3 is ComboBox) && !(cur3.Text.Trim().Length > 0) && cur3.Name != "textBox7" && cur3.Name != "textBox8")
                            {
                                MessageBox.Show("Fill in all the fields before going to the next step.", "Complete Current Step");
                                return;
                            }
                        }
                    }
                }
            }
            Control oldControl = getControl("label" + stepIndex);
            oldStepIndex = stepIndex;
            if (stepIndex < 16)
                stepIndex++;
            Control newControl = getControl("label" + stepIndex);
            oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
            newControl.Font = new Font(newControl.Font, FontStyle.Bold);
            UpdateWindowText();
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
            if (MessageBox.Show("Are you sure you want to exit? All unsaved progress will be lost.", "Confirmation", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                e.Cancel = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DisplayStepTabPage();
        }
        private void DisplayStepTabPage()
        {
            tabControl1.SelectedIndex = stepIndex - 1;
            if (tabControl1.SelectedIndex == 2 || tabControl1.SelectedIndex == 4 || tabControl1.SelectedIndex == 5 || tabControl1.SelectedIndex == 6 || tabControl1.SelectedIndex == 7 || tabControl1.SelectedIndex == 8 || tabControl1.SelectedIndex == 9 || tabControl1.SelectedIndex == 14)
            {
                button27.Enabled = true;
            }
            else
                button27.Enabled = false;
        }
        bool force = false;
        public int capitolsFound = 0;
        public bool territoryChange = false;
        bool opened1 = false;
        bool opened2 = false;
        bool opened3 = false;
        bool opened4 = false;
        bool opened5 = false;
        bool infochange1 = false;
        bool infochange2 = false;
        bool infochange3 = false;
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            try
            {
                if (!surpress1 && (stepIndex > oldStepIndex || force))
                {
                    //button27.Enabled = false;
                    if (oldStepIndex == 1) //These methods send out information from the results of the step
                    {
                        Step1Info.MapName = textBox1.Text;
                        Step1Info.MapVersion = textBox2.Text;
                        Step1Info.ResourceName = textBox3.Text;
                        Step1Info.MapImageLocation = textBox4.Text;
                        if (Step1Info.MapImageLocation != "blank")
                        {
                            Step1Info.MapImage = Image.FromFile(Step1Info.MapImageLocation);
                            Step1Info.MapImageWL = Image.FromFile(Step1Info.MapImageLocation);
                        }
                        Step1Info.CentersLocation = textBox7.Text;
                        Step1Info.WaterTerritoryFilter = textBox8.Text;
                    }
                    else if (oldStepIndex == 2)
                    {
                        territoryChange = true;
                        capitolsFound = 0;
                        foreach (Control cur in TerritoryDefinitionsImageDrawer.Controls)
                        {
                            cur.Text = cur.Text.Trim();
                            Territory t = new Territory();
                            t.Name = cur.Text.Trim();
                            try
                            {
                                if (((string)cur.Tag).Contains("Water"))
                                    t.IsWater = true;
                                if (((string)cur.Tag).Contains("Capitol"))
                                {
                                    t.IsCapitol = true;
                                    capitolsFound++;
                                }
                                if (((string)cur.Tag).Contains("Impassable"))
                                    t.IsImpassable = true;
                                if (((string)cur.Tag).Contains("VictoryCity"))
                                    t.IsVictoryCity = true;
                            }
                            catch { }
                            
                            try
                            {
                                t.IsWater = cur.BackColor == Color.DodgerBlue ? true : false;
                                try
                                {
                                    t.Production = Step2Info.territories[cur.Text.Trim()].Production;
                                    t.Owner = Step2Info.territories[cur.Text.Trim()].Owner;
                                    t.Units = Step2Info.territories[cur.Text.Trim()].Units;
                                    //t.IsWater = Step2Info.territories[cur.Text.Trim()].IsWater;
                                    //t.IsCapitol = Step2Info.territories[cur.Text.Trim()].IsCapitol;
                                    //t.IsImpassable = Step2Info.territories[cur.Text.Trim()].IsImpassable;
                                    //t.IsVictoryCity = Step2Info.territories[cur.Text.Trim()].IsVictoryCity;
                                    //if (t.IsCapitol)
                                    //    capitolsFound++;
                                }
                                catch{}
                                try
                                {
                                    Step2Info.territories.Remove(t.Name.Trim());
                                }
                                catch { }
                                Step2Info.territories.Add(t.Name, t);
                                t.Label = (Label)cur;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Found two territories with the same name, skipping second one!");
                            }
                        }
                        if (capitolsFound < Step4Info.players.Count)
                        {
                            if (MessageBox.Show("You have not set the capitol for all the players. Are you sure you want to continue?", "Confirmation", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                            {
                                Back();
                                e.Cancel = true;
                            }
                        }
                    }
                    else if (oldStepIndex == 4)
                    {
                        Step4Info.players.Clear();
                        String toAddName = "";
                        String alliance = "";
                        foreach (Control cur in tabPage4.Controls)
                        {
                            cur.Text = cur.Text.Trim();
                            if (cur is TextBox)
                            {
                                if (toAddName == "")
                                {
                                    toAddName = cur.Text;
                                }
                                else if (alliance == "")
                                {
                                    alliance = cur.Text;
                                }
                                else
                                {
                                    Step4Info.players.Add(toAddName, new Player() { Alliance = alliance, Name = toAddName, InitialResources = Convert.ToInt32(cur.Text)});
                                    toAddName = "";
                                    alliance = "";
                                }
                            }
                        }
                        if (capitolsFound < Step4Info.players.Count)
                        {
                            if (MessageBox.Show("You have not set the capitols for all the players. Do you want to go back to the 'Territory Definitions' step and select the players' capitols?", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                            {
                                Back();
                                Back();
                                Back();
                                refresh = true;
                            }
                        }
                    }
                    else if (oldStepIndex == 5)
                    {
                        Step5Info.units.Clear();
                        String toAddName = "";
                        String cost = "";
                        foreach (Control cur in tabPage5.Controls)
                        {
                            cur.Text = cur.Text.Trim();
                            if (cur is TextBox)
                            {
                                if (toAddName == "")
                                {
                                    toAddName = cur.Text;
                                }
                                else if (cost == "")
                                {
                                    cost = cur.Text;
                                }
                                else
                                {
                                    Step5Info.units.Add(toAddName.ToLower(), new Unit() { Name = toAddName, cost = new Cost { cost = Convert.ToInt32(cost), ResourceType = Step1Info.ResourceName, result = new Result() { BuyQuantity = Convert.ToInt32(cur.Text), ResourceOrUnitName = toAddName } } });
                                    toAddName = "";
                                    cost = "";
                                }
                            }
                        }
                    }
                    else if (oldStepIndex == 6)
                    {
                        Step6Info.gameplaySequences.Clear();
                        String toAddName = "";
                        String className = "";
                        foreach (Control cur in tabPage6.Controls)
                        {
                            try
                            {
                                cur.Text = cur.Text.Trim();
                                if (cur is TextBox)
                                {
                                    if (toAddName == "")
                                    {
                                        toAddName = cur.Text;
                                    }
                                    else if (className == "")
                                    {
                                        className = cur.Text;
                                    }
                                    else
                                    {
                                        //MessageBox.Show(toAddName + "," + className + "," + cur.Text);
                                        Step6Info.gameplaySequences.Add(toAddName, new GameplaySequence() { Name = toAddName, Class = className, Display = cur.Text });
                                        toAddName = "";
                                        className = "";
                                    }
                                }
                            }
                            catch { smallErrorOccured = true; }
                        }
                    }
                    else if (oldStepIndex == 7)
                    {
                            Step7Info.playerSequences.Clear();
                            String toAddName = "";
                            String gameplaySequence = "";
                            String _player = "";
                            foreach (Control cur in tabPage7.Controls)
                            {
                                try
                                {
                                    cur.Text = cur.Text.Trim();
                                    if (cur is TextBox || cur is ComboBox)
                                    {
                                        if (toAddName == "")
                                        {
                                            toAddName = cur.Text;
                                        }
                                        else if (gameplaySequence == "")
                                        {
                                            gameplaySequence = cur.Text;
                                        }
                                        else if (_player == "")
                                        {
                                            _player = cur.Text;
                                        }
                                        else
                                        {
                                            //MessageBox.Show(toAddName + "," + gameplaySequence + "," + _player + "," + cur.Text);
                                            Step7Info.playerSequences.Add(toAddName, new PlayerSequence() { Name = toAddName, Delegate = Step6Info.gameplaySequences[gameplaySequence], player = Step4Info.players[_player], MaxRunCount = Convert.ToInt32(cur.Text) });
                                            toAddName = "";
                                            gameplaySequence = "";
                                            _player = "";
                                        }
                                    }
                                }
                                catch{ smallErrorOccured = true; }
                            }
                    }
                    else if (oldStepIndex == 8)
                    {
                        Step8Info.technologies.Clear();
                        String toAddName = "";
                        String _player = "";
                        foreach (Control cur in tabPage8.Controls)
                        {
                            try
                            {
                                cur.Text = cur.Text.Trim();
                                if (cur is TextBox || cur is ComboBox)
                                {
                                    if (toAddName == "")
                                    {
                                        toAddName = cur.Text;
                                    }
                                    else if (_player == "")
                                    {
                                        _player = cur.Text;
                                    }
                                    else
                                    {
                                        Step8Info.technologies.Add(toAddName + "|" + _player, new Technology() { Name = toAddName, player = Step4Info.players[_player], AlreadyEnabled = Convert.ToBoolean(cur.Text) });
                                        toAddName = "";
                                        _player = "";
                                    }
                                }
                            }
                            catch { smallErrorOccured = true; }
                        }
                    }
                    else if (oldStepIndex == 9)
                    {
                        Step9Info.ProductionFrontiers.Clear();
                        String toAddName = "";
                        List<Unit> units = new List<Unit>();
                        foreach (TabPage cur in tabControl2.TabPages)
                        {
                            try
                            {
                                units.Clear();
                                toAddName = cur.Text;
                                foreach (Control cur2 in cur.Controls)
                                {
                                    if (cur2 is ComboBox)
                                    {
                                        units.Add(Step5Info.units[cur2.Text.ToLower()]);
                                    }
                                }
                                Step9Info.ProductionFrontiers.Add(toAddName, new ProductionFrontier() { Name = toAddName, UnitsInFrontier = units });
                            }
                            catch { smallErrorOccured = true; }
                        }
                    }
                    else if (oldStepIndex == 10)
                    {
                        UnitOption curOption = new UnitOption();
                        String toAddName = "";
                        foreach (TabPage cur in tabControl3.TabPages)
                        {
                            try
                            {
                                toAddName = cur.Text;
                                Step5Info.units[toAddName.ToLower()].attachment = new UnitAttachment();
                                foreach (Control cur2 in cur.Controls)
                                {
                                    if (cur2 is TextBox)
                                    {
                                        if (curOption.Name == "")
                                        {
                                            curOption.Name = cur2.Text;
                                        }
                                        else
                                        {
                                            //MessageBox.Show(toAddName + "," + curOption.Name + "," + cur2.Text);
                                            if (Step5Info.units[toAddName.ToLower()].attachment.options.Count == 0)
                                            {
                                                Step5Info.units[toAddName.ToLower()].attachment = new UnitAttachment() { options = new List<UnitOption>() { new UnitOption() { Name = curOption.Name, Value = cur2.Text } } };
                                            }
                                            else
                                            {
                                                Step5Info.units[toAddName.ToLower()].attachment.options.Add(new UnitOption() { Name = curOption.Name, Value = cur2.Text });
                                            }
                                            curOption.Name = "";
                                        }
                                    }
                                }
                            }
                            catch { smallErrorOccured = true; }
                        }
                    }
                    else if (oldStepIndex == 11)
                    {
                        foreach (Control cur in TerritoryProductionsImageDrawer.Controls)
                        {
                                cur.Text = cur.Text.Trim();
                                Step2Info.territories[(String)cur.Tag].Production = Convert.ToInt32(cur.Text);
                        }
                    }
                    else if (oldStepIndex == 13)
                    {
                        foreach (Control cur in TerritoryOwnershipImageDrawer.Controls)
                        {
                            try
                            {
                                cur.Text = cur.Text.Trim();
                                Step2Info.territories[(String)cur.Tag].Owner = Step4Info.players[cur.Text];
                            }
                            catch { Step2Info.territories[(String)cur.Tag].Owner = new Player() { Name = "" }; }
                        }
                    }
                    else if (oldStepIndex == 14)
                    {
                        foreach (Control cur in UnitPlacementsImageDrawer.Controls)
                        {
                            try
                            {
                                string tag = ((string)cur.Tag).Trim();
                                Dictionary<string,Unit>.Enumerator en = Step5Info.units.GetEnumerator();
                                en.MoveNext();
                                string tag1 = tag.Substring(0, ((string)cur.Tag).IndexOf("|")) + "," + en.Current.Value.Name + ":1";
                                string tag2 = tag.Substring(tag.IndexOf("|") + 1);
                                foreach (Unit cur2 in Step5Info.units.Values)
                                {
                                    try
                                    {
                                        int index = tag1.ToLower().IndexOf(cur2.Name.ToLower());
                                        if (tag1.Contains(cur2.Name))
                                        {
                                            int numIndex = index + cur2.Name.Length + 1;
                                            try
                                            {
                                                int numLength = tag1.Substring(numIndex).IndexOf(",");
                                                if (numIndex > -1)
                                                {
                                                    int addAmount = Convert.ToInt32(tag1.Substring(numIndex, numLength).Trim());
                                                    Step2Info.territories[tag2].Units.Add(new Unit() { Name = cur2.Name.ToString(), cost = new Cost() { cost = cur2.cost.cost, ResourceType = cur2.cost.ResourceType, result = new Result() { BuyQuantity = addAmount, ResourceOrUnitName = cur2.Name } }, attachment = cur2.attachment, unitOwner = new Player() { Name = cur.AccessibleName != null ? cur.AccessibleName : ""} });
                                                }
                                            }
                                            catch
                                            {
                                                try
                                                {
                                                    int numLength = tag.Substring(numIndex).Length;
                                                    if (numIndex > -1)
                                                    {
                                                        int addAmount = Convert.ToInt32(tag.Substring(numIndex, numLength).Trim());
                                                        for (int i = 0; i < addAmount; i++)
                                                        {
                                                            Step2Info.territories[tag2].Units.Add(new Unit() { Name = cur2.Name.ToString(), cost = new Cost() { cost = cur2.cost.cost, ResourceType = cur2.cost.ResourceType, result = new Result() { BuyQuantity = addAmount, ResourceOrUnitName = cur2.Name } }, attachment = cur2.attachment, unitOwner = new Player() { Name = cur.AccessibleName != null ? cur.AccessibleName : "" } });
                                                        }
                                                    }
                                                }
                                                catch { }
                                            }
                                        }
                                    }
                                    catch { }
                                }
                            }
                            catch { /*smallErrorOccured = true;*/ }
                        }
                    }
                    else if (oldStepIndex == 15)
                    {
                        Step15Info.Settings.Clear();
                        String toAddName = "";
                        String value = "";
                        Boolean? editable = null;
                        int minN = -1;
                        foreach (Control cur in tabPage15.Controls)
                        {
                            try
                            {
                                cur.Text = cur.Text.Trim();
                                if (cur is TextBox || cur is ComboBox)
                                {
                                    if (toAddName == "")
                                    {
                                        toAddName = cur.Text;
                                    }
                                    else if (value == "")
                                    {
                                        value = cur.Text;
                                    }
                                    else if (editable == null)
                                    {
                                        editable = Convert.ToBoolean(cur.Text);
                                    }
                                    else if (minN == -1)
                                    {
                                        minN = Convert.ToInt32(cur.Text);
                                    }
                                    else
                                    {
                                        Step15Info.Settings.Add(toAddName, new Setting() { Name = toAddName, Editable = (bool)editable, Value = value, IntMin = minN, IntMax = Convert.ToInt32(cur.Text) });
                                        toAddName = "";
                                        value = "";
                                        editable = null;
                                        minN = -1;
                                    }
                                }
                            }
                            catch { smallErrorOccured = true; }
                        }
                    }
                }
                if (!surpress2 && (stepIndex > oldStepIndex || force))
                {
                    if (tabControl1.SelectedIndex == 1) //These methods are like instantiation methods, they preset the tabs when they are opened
                    {
                        if (!(Step2Info.territories.Count > 0))
                        {
                            TerritoryDefinitionsImageDrawer.BackgroundImage = Step1Info.MapImage;
                            TerritoryDefinitionsImageDrawer.Size = TerritoryDefinitionsImageDrawer.BackgroundImage.Size;
                        }
                        if (Step1Info.CentersLocation.Trim().Length > 0)
                        {
                            try
                            {
                                List<Label> labels = new List<Label>();
                                String[] lines = File.ReadAllLines(Step1Info.CentersLocation);
                                foreach (String cur in lines)
                                {
                                    String name = cur.Substring(0, cur.IndexOf('('));
                                    String sPoint = cur.Substring(cur.IndexOf('(') + 1, cur.LastIndexOf(")") - (cur.IndexOf("(") + 1));
                                    Point point = new Point(Convert.ToInt32(sPoint.Substring(0, sPoint.IndexOf(","))), Convert.ToInt32(sPoint.Substring(sPoint.IndexOf(",") + 1, sPoint.Length - sPoint.IndexOf(",") - 1)));
                                    labels.Add(new Label() { Text = name.Trim(), BackColor = Step1Info.WaterTerritoryFilter.Trim().Length > 0 && name.Contains(Step1Info.WaterTerritoryFilter) ? Color.DodgerBlue : Color.LightGreen, Font = new Font(label1.Font, FontStyle.Bold), Location = new Point(point.X - (int)(Graphics.FromImage(new Bitmap(1, 1)).MeasureString(name, new Font(label1.Font, FontStyle.Bold)).Width / 2), point.Y) });
                                }
                                foreach (Label cur in labels)
                                {
                                    //MessageBox.Show(cur.Text + "/" + cur.Location.ToString());
                                    Label l = new Label() { Text = cur.Text, BackColor = cur.BackColor, Font = cur.Font, AutoSize = true, Location = cur.Location };
                                    l.MouseClick += new MouseEventHandler(l_MouseClick);
                                    Step2Info.territories.Add(l.Text, new Territory() { Label = l, Name = l.Text });
                                    TerritoryDefinitionsImageDrawer.Controls.Add(l);
                                }
                            }
                            catch { }
                        }
                    }
                    else if (tabControl1.SelectedIndex == 2)
                    {
                        //button27.Enabled = true;
                        if ((territoryChange || force || !opened1))
                        {
                            opened1 = true;
                            TerritoryConnectionsImageDrawer.Controls.Clear();
                            foreach (Territory cur in Step2Info.territories.Values)
                            {
                                Label l = new Label() { Text = cur.Label.Text, BackColor = cur.Label.BackColor, Font = cur.Label.Font, AutoSize = true, Location = cur.Label.Location };
                                l.MouseClick += new MouseEventHandler(l_MouseClick);
                                TerritoryConnectionsImageDrawer.Controls.Add(l);
                            }
                                TerritoryConnectionsImageDrawer.BackgroundImage = Step1Info.MapImageWL;
                            TerritoryConnectionsImageDrawer.Size = TerritoryDefinitionsImageDrawer.BackgroundImage.Size;
                            foreach (Connection cur in Step3Info.connections.Values)
                            {
                                Graphics.FromImage(TerritoryConnectionsImageDrawer.BackgroundImage).DrawLine(Pens.Red, cur.t1.Label.Location + new Size(cur.t1.Label.Size.Width / 2, 0), cur.t2.Label.Location + new Size(cur.t2.Label.Size.Width / 2, 0));
                            }
                        }
                    }
                    else if (tabControl1.SelectedIndex == 4)
                    {
                        //button27.Enabled = true;
                    }
                    else if (tabControl1.SelectedIndex == 5)
                    {
                        //button27.Enabled = true;
                    }
                    else if (tabControl1.SelectedIndex == 6 )
                    {
                        //button27.Enabled = true;
                        if (true)//infochange1)
                        {
                            infochange1 = false;
                            comboBox16.Items.Clear();
                            comboBox17.Items.Clear();
                            bool isFirst = false;
                            foreach (Control cur2 in tabControl1.TabPages[tabControl1.SelectedIndex].Controls)
                            {
                                if (cur2 is ComboBox)
                                {
                                    isFirst = !isFirst;
                                    if (isFirst)
                                    {
                                        ((ComboBox)cur2).Items.Clear();
                                    }
                                }
                            }
                            isFirst = false;
                            foreach (GameplaySequence cur in Step6Info.gameplaySequences.Values)
                            {
                                foreach (Control cur2 in tabControl1.TabPages[tabControl1.SelectedIndex].Controls)
                                {
                                    if (cur2 is ComboBox)
                                    {
                                        isFirst = !isFirst;
                                        if (isFirst)
                                        {
                                            ((ComboBox)cur2).Items.Add(cur.Name);
                                        }
                                    }
                                }
                            }
                            isFirst = true;
                            foreach (Control cur2 in tabControl1.TabPages[tabControl1.SelectedIndex].Controls)
                            {
                                if (cur2 is ComboBox)
                                {
                                    isFirst = !isFirst;
                                    if (isFirst)
                                    {
                                        ((ComboBox)cur2).Items.Clear();
                                    }
                                }
                            }
                            isFirst = true;
                            foreach (Player cur in Step4Info.players.Values)
                            {
                                foreach (Control cur2 in tabControl1.TabPages[tabControl1.SelectedIndex].Controls)
                                {
                                    if (cur2 is ComboBox)
                                    {
                                        isFirst = !isFirst;
                                        if (isFirst)
                                        {
                                            ((ComboBox)cur2).Items.Add(cur.Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (tabControl1.SelectedIndex == 7)
                    {
                        //button27.Enabled = true;
                        comboBox2.Items.Clear();
                        bool isFirst = false;
                        foreach (Player cur in Step4Info.players.Values)
                        {
                            foreach (Control cur2 in tabControl1.TabPages[tabControl1.SelectedIndex].Controls)
                            {
                                if (cur2 is ComboBox)
                                {
                                    isFirst = !isFirst;
                                    if (isFirst)
                                    {
                                        ((ComboBox)cur2).Items.Add(cur.Name);
                                    }
                                }
                            }
                        }
                    }
                    else if (tabControl1.SelectedIndex == 8)
                    {
                        //button27.Enabled = true;
                        if (true)//infochange2)
                        {
                            infochange2 = false;
                            List<string> listOfTabPagesJustAdded = new List<string>();
                            foreach (Player cur in Step4Info.players.Values)
                            {
                                bool foundOne = false;
                                foreach (TabPage cur2 in tabControl2.TabPages)
                                {
                                    if (cur2.Text.ToLower() == cur.Name.ToLower())
                                        foundOne = true;
                                }
                                if (!foundOne)
                                {
                                    tabControl2.TabPages.Add(new TabPage(cur.Name) { AutoScroll = true });
                                    listOfTabPagesJustAdded.Add(cur.Name);
                                }
                            }
                            for (int i = 0; i < tabControl2.TabPages.Count; i++)
                            {
                                TabPage cur = tabControl2.TabPages[i];
                                if (!Step4Info.players.ContainsKey(cur.Text))
                                    tabControl2.TabPages.RemoveAt(i);
                            }
                            foreach (TabPage cur in tabControl2.TabPages)
                            {
                                if (listOfTabPagesJustAdded.Contains(cur.Text))
                                {
                                    Label c1 = new Label() { Text = "Units in frontier", AutoSize = true, Location = new Point(185, 14), Font = new Font(label19.Font, FontStyle.Regular) };
                                    Label c2 = new Label() { Text = "Units", AutoSize = true, Location = new Point(82, 41), Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))) };
                                    ComboBox c3 = new ComboBox() { Location = new Point(39, 59), Size = new Size(143, 21) };
                                    foreach (Unit cur2 in Step5Info.units.Values)
                                    {
                                        c3.Items.Add(cur2.Name);
                                    }
                                    Button c4 = new Button() { Location = new Point(189, 57), Size = new Size(71, 23), Text = "Remove" };
                                    c4.Click += new EventHandler(c4_Click);
                                    Button c5 = new Button() { Location = new Point(39, 87), Size = new Size(82, 23), Text = "Add Attachment..." };
                                    c5.Click += new EventHandler(c5_Click);
                                    cur.Controls.AddRange(new Control[] { c1, c2, c3, c4, c5 });
                                    cur.Tag = 0;
                                }
                            }
                        }
                    }
                    else if (tabControl1.SelectedIndex == 9)
                    {
                        //button27.Enabled = true;
                        if (true)//infochange3)
                        {
                            infochange3 = false;
                            List<string> listOfTabPagesJustAdded = new List<string>();
                            foreach (Unit cur in Step5Info.units.Values)
                            {
                                bool foundOne = false;
                                foreach (TabPage cur2 in tabControl3.TabPages)
                                {
                                    if (cur2.Text.ToLower() == cur.Name.ToLower())
                                        foundOne = true;
                                }
                                if (!foundOne)
                                {
                                    tabControl3.TabPages.Add(new TabPage(cur.Name) { AutoScroll = true });
                                    listOfTabPagesJustAdded.Add(cur.Name);
                                }
                            }
                            for(int i = 0;i < tabControl3.TabPages.Count;i++)
                            {
                                TabPage cur = tabControl3.TabPages[i];
                                if (!Step5Info.units.ContainsKey(cur.Text.ToLower()))
                                    tabControl3.TabPages.RemoveAt(i);
                            }
                            foreach (TabPage cur in tabControl3.TabPages)
                            {
                                if (listOfTabPagesJustAdded.Contains(cur.Text))
                                {
                                    Label c1 = new Label() { Text = "Unit Attachments", AutoSize = true, Location = new Point(198, 14), Font = new Font(label19.Font, FontStyle.Regular) };
                                    Label c2 = new Label() { Text = "Attachment Name", AutoSize = true, Location = new Point(63, 43), Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))) };
                                    Label c3 = new Label() { Text = "Values", AutoSize = true, Location = new Point(223, 43), Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))) };

                                    TextBox c4 = new TextBox() { Location = new Point(39, 59), Size = new Size(143, 20) };
                                    TextBox c5 = new TextBox() { Location = new Point(196, 59), Size = new Size(92, 20) };

                                    Button c6 = new Button() { Location = new Point(294, 57), Size = new Size(71, 23), Text = "Remove" };
                                    c6.Click += new EventHandler(c4_Click2);
                                    Button c7 = new Button() { Location = new Point(39, 87), Size = new Size(116, 23), Text = "Add Attachment..." };
                                    c7.Click += new EventHandler(c5_Click2);
                                    cur.Controls.AddRange(new Control[] { c1, c2, c3, c4, c5, c6, c7 });
                                    cur.Tag = 0;
                                }
                            }
                        }
                    }
                    else if (tabControl1.SelectedIndex == 10 && (territoryChange || force || !opened2))
                    {
                        opened2 = true;
                        TerritoryProductionsImageDrawer.Controls.Clear();
                        foreach (Territory cur in Step2Info.territories.Values)
                        {
                            Label l = new Label() { Text = cur.Production.ToString(), Tag = cur.Name, BackColor = cur.Label.BackColor, Font = cur.Label.Font, AutoSize = true, Location = cur.Label.Location + new Size(((int)(cur.Label.Size.Width / 2)), 0) };
                            l.MouseClick += new MouseEventHandler(l_MouseClick);
                            TerritoryProductionsImageDrawer.Controls.Add(l);
                        }
                        TerritoryProductionsImageDrawer.BackgroundImage = Step1Info.MapImage;
                        TerritoryProductionsImageDrawer.Size = TerritoryProductionsImageDrawer.BackgroundImage.Size;
                    }
                    else if (tabControl1.SelectedIndex == 11 && (territoryChange || force || !opened3))
                    {
                        opened3 = true;
                        CanalsImageDrawer.Controls.Clear();
                        foreach (Territory cur in Step2Info.territories.Values)
                        {
                            Label l = new Label() { Name = cur.Name,Text = cur.Name, Tag = cur.Name, BackColor = cur.Label.BackColor, Font = cur.Label.Font, AutoSize = true, Location = cur.Label.Location };
                            l.MouseClick += new MouseEventHandler(l_MouseClick);
                            CanalsImageDrawer.Controls.Add(l);
                        }
                        foreach (Canal cur2 in Step12Info.Canals.Values)
                        {
                            try
                            {
                                foreach (Territory cur3 in cur2.CanalSeaNeighbors)
                                {
                                    CanalsImageDrawer.Controls[cur3.Name].BackColor = Color.Red;
                                }
                            }
                            catch { }
                        }
                        CanalsImageDrawer.BackgroundImage = Step1Info.MapImage;
                        CanalsImageDrawer.Size = CanalsImageDrawer.BackgroundImage.Size;
                    }
                    else if (tabControl1.SelectedIndex == 12 && (territoryChange || force || !opened4))
                    {
                        opened4 = true;
                        List<Control> toDelete = new List<Control>();
                        foreach (Control cur in TerritoryOwnershipImageDrawer.Controls)
                        {
                            if (!Step2Info.territories.ContainsKey(((string)(cur.Tag))))
                                toDelete.Add(cur);
                        }
                        foreach (Control cur in toDelete)
                        {
                            TerritoryOwnershipImageDrawer.Controls.RemoveByKey((string)(cur.Tag));
                        }
                        foreach (Territory cur in Step2Info.territories.Values)
                        {
                            if (!TerritoryOwnershipImageDrawer.Controls.ContainsKey(cur.Name))
                            {
                                Label l;
                                if (cur.IsWater || cur.Owner.Name.ToLower() == "neutral" || cur.Owner.Name.Trim().Length == 0)
                                    l = new Label() { Text = cur.IsWater ? "None" : "Neutral", Tag = cur.Name, BackColor = cur.Owner.Name.ToLower().Trim() == "neutral" || (!cur.IsWater && cur.Owner.Name.Trim().Length == 0) ? Color.White : cur.Label.BackColor, Font = cur.Label.Font, AutoSize = true, Location = cur.Label.Location };
                                else
                                {
                                    l = new Label() { Text = cur.Owner.Name, Tag = cur.Name, Font = cur.Label.Font, AutoSize = true, Location = cur.Label.Location };
                                    int index = 0;
                                    bool found = false;
                                    foreach (Player cur2 in Step4Info.players.Values)
                                    {
                                        if (cur2.Name == cur.Owner.Name)
                                        {
                                            found = true;
                                            break;
                                        }
                                        index++;
                                    }
                                    if (index == 0)
                                        l.BackColor = Color.Red;
                                    else if (index == 1)
                                        l.BackColor = Color.Orange;
                                    else if (index == 2)
                                        l.BackColor = Color.Yellow;
                                    else if (index == 3)
                                        l.BackColor = Color.Green;
                                    else if (index == 4)
                                        l.BackColor = Color.Blue;
                                    else if (index == 5)
                                        l.BackColor = Color.Indigo;
                                    else if (index == 6)
                                        l.BackColor = Color.Violet;

                                    if (!found)
                                        l.BackColor = Color.White;
                                }
                                l.Location = new Point((l.Location.X + ((int)(l.Size.Width / 2))) - (int)(Graphics.FromImage(new Bitmap(1, 1)).MeasureString(l.Text, l.Font).Width / 2), l.Location.Y);
                                l.Name = cur.Name;
                                l.MouseClick += new MouseEventHandler(l_MouseClick);
                                TerritoryOwnershipImageDrawer.Controls.Add(l);
                            }
                            TerritoryOwnershipImageDrawer.BackgroundImage = Step1Info.MapImage;
                            TerritoryOwnershipImageDrawer.Size = TerritoryOwnershipImageDrawer.BackgroundImage.Size;
                        }
                    }
                    else if (tabControl1.SelectedIndex == 13 && (territoryChange || force || !opened5))
                    {
                        opened5 = true;
                        territoryChange = false;
                        UnitPlacementsImageDrawer.Controls.Clear();
                        foreach (Territory cur in Step2Info.territories.Values)
                        {
                            Label l;
                            if (cur.Units.Count > 0)
                            {
                                l = new Label() { Text = cur.Name, Name = cur.Name, BackColor = Color.LightGray, Font = TerritoryOwnershipImageDrawer.Controls[cur.Name].Font, AutoSize = true, Location = cur.Label.Location };
                                Dictionary<string, int> unitsTA = new Dictionary<string, int>();
                                foreach (Unit cur2 in cur.Units)
                                {
                                    try
                                    {
                                        unitsTA[cur2.Name].ToString();
                                        unitsTA[cur2.Name]++;
                                    }
                                    catch
                                    {
                                        unitsTA.Add(cur2.Name, 1);
                                    }
                                    if (cur2.unitOwner.Name.Trim().Length > 0)
                                        l.AccessibleName = cur2.unitOwner.Name;
                                }
                                foreach (KeyValuePair<string, int> cur2 in unitsTA)
                                {
                                    l.Tag += cur2.Key + ":" + cur2.Value + ",";
                                }
                                l.Tag = ((string)l.Tag).Substring(0, ((string)l.Tag).Length - 1);
                                l.Tag += "|" + l.Text;
                            }
                            else
                                l = new Label() { Text = cur.Name, Tag = "", BackColor = cur.Label.BackColor, Font = TerritoryOwnershipImageDrawer.Controls[cur.Name].Font, AutoSize = true, Location = cur.Label.Location };
                            
                            if (cur.IsWater == false && cur.Owner.Name.Trim().Length > 0)
                                l.AccessibleName = cur.Owner.Name;

                            l.MouseClick += new MouseEventHandler(l_MouseClick);
                            UnitPlacementsImageDrawer.Controls.Add(l);
                        }
                        UnitPlacementsImageDrawer.BackgroundImage = Step1Info.MapImage;
                        UnitPlacementsImageDrawer.Size = UnitPlacementsImageDrawer.BackgroundImage.Size;
                    }
                    else if (tabControl1.SelectedIndex == 14)
                    {
                        //button27.Enabled = true;
                    }
                }
            }
            catch(Exception ex)
            {
                Back();
                e.Cancel = true;
                if (MessageBox.Show("An error occured while trying to process the data you entered. Please make sure you entered everything correctly and try again.\r\n\r\nDo you want to view the error message?", "Error Parsing Data", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    throw ex;
            }
            if (smallErrorOccured)
            {
                smallErrorOccured = false;
                if (MessageBox.Show("An error occured while trying to process some of the data you entered. Some of the information may not have been applied. Do you want to go back and fix it?", "Error Parsing Data", MessageBoxButtons.YesNoCancel) != DialogResult.No)
                {
                    Back();
                    e.Cancel = true;
                }
            }
            force = false;
        }
        bool smallErrorOccured = false;
        bool refresh = false;
        public int FindOccurences(string s, string toFind)
        {
            int amount = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s.Substring(i, toFind.Length) == toFind)
                    amount++;
            }
            return amount;
        }
        void c5_Click(object sender, EventArgs e)
        {
            tabControl2.TabPages[tabControl2.SelectedIndex].Tag = Convert.ToInt32(tabControl2.TabPages[tabControl2.SelectedIndex].Tag) + 25;
            int change6 = (int)tabControl2.TabPages[tabControl2.SelectedIndex].Tag;
            Button remove = new Button();
            Button add = new Button();
            int buttonindex = 0;
            ComboBox cBox = null;
            foreach (Control cur in tabControl2.TabPages[tabControl2.SelectedIndex].Controls)
            {
                if(cur is Button)
                {
                    if (buttonindex == 0)
                        remove = (Button)cur;
                    else
                        add = (Button)cur;
                    buttonindex++;
                }
                else if (cur is ComboBox)
                {
                    if(cBox == null)
                    cBox = (ComboBox)cur;
                }
            }
            remove.Location += new Size(0, 25);
            add.Location += new Size(0, 25);
            ComboBox cb1 = new ComboBox() { Size = cBox.Size, Location = new Point(cBox.Location.X, cBox.Location.Y + change6) };
            cb1.Items.AddRange(getUnits());
            tabControl2.TabPages[tabControl2.SelectedIndex].Controls.Add(cb1);
        }
        void c4_Click(object sender, EventArgs e)
        {
            int change6 = (int)tabControl2.TabPages[tabControl2.SelectedIndex].Tag;
            if (change6 > 0)
            {
                tabControl2.TabPages[tabControl2.SelectedIndex].Tag = Convert.ToInt32(tabControl2.TabPages[tabControl2.SelectedIndex].Tag) - 25;
                Button remove = new Button();
                Button add = new Button();
                int buttonindex = 0;
                ComboBox lastBox = new ComboBox();
                foreach (Control cur in tabControl2.TabPages[tabControl2.SelectedIndex].Controls)
                {
                    if (cur is Button)
                    {
                        if (buttonindex == 0)
                            remove = (Button)cur;
                        else
                            add = (Button)cur;
                        buttonindex++;
                    }
                    else if (cur is ComboBox)
                    {
                        lastBox = (ComboBox)cur;
                    }
                }
                remove.Location -= new Size(0, 25);
                add.Location -= new Size(0, 25);
                tabControl2.TabPages[tabControl2.SelectedIndex].Controls.Remove(lastBox);
            }
        }
        void c5_Click2(object sender, EventArgs e)
        {
            tabControl3.TabPages[tabControl3.SelectedIndex].Tag = Convert.ToInt32(tabControl3.TabPages[tabControl3.SelectedIndex].Tag) + 25;
            int change6 = (int)tabControl3.TabPages[tabControl3.SelectedIndex].Tag;
            Button remove = new Button();
            Button add = new Button();
            int buttonindex = 0;
            TextBox tBox = null;
            foreach (Control cur in tabControl3.TabPages[tabControl3.SelectedIndex].Controls)
            {
                if (cur is Button)
                {
                    if (buttonindex == 0)
                        remove = (Button)cur;
                    else
                        add = (Button)cur;
                    buttonindex++;
                }
                if (cur is TextBox)
                {
                    tBox = (TextBox)cur;
                }
            }
            remove.Location += new Size(0, 25);
            add.Location += new Size(0, 25);
            TextBox c1 = new TextBox() { Size = new Size(143, 20), Location = new Point(39, 59 + change6 - tabControl3.TabPages[tabControl3.SelectedIndex].VerticalScroll.Value) };
            TextBox c2 = new TextBox() { Size = tBox.Size, Location = new Point(tBox.Location.X, tBox.Location.Y + 25) };
             tabControl3.TabPages[tabControl3.SelectedIndex].Controls.AddRange(new Control[]{c1, c2});
        }
        void c4_Click2(object sender, EventArgs e)
        {
            int change6 = (int)tabControl3.TabPages[tabControl3.SelectedIndex].Tag;
            if (change6 > 0)
            {
                tabControl3.TabPages[tabControl3.SelectedIndex].Tag = Convert.ToInt32(tabControl3.TabPages[tabControl3.SelectedIndex].Tag) - 25;
                Button remove = new Button();
                Button add = new Button();
                int buttonindex = 0;
                TextBox lastBox = new TextBox();
                foreach (Control cur in tabControl3.TabPages[tabControl3.SelectedIndex].Controls)
                {
                    if (cur is Button)
                    {
                        if (buttonindex == 0)
                            remove = (Button)cur;
                        else
                            add = (Button)cur;
                        buttonindex++;
                    }
                    else if (cur is TextBox)
                    {
                        lastBox = (TextBox)cur;
                    }
                }
                tabControl3.TabPages[tabControl3.SelectedIndex].Controls.Remove(lastBox);
                foreach (Control cur in tabControl3.TabPages[tabControl3.SelectedIndex].Controls)
                {
                    if (cur is TextBox)
                    {
                        lastBox = (TextBox)cur;
                    }
                }
                remove.Location -= new Size(0, 25);
                add.Location -= new Size(0, 25);
                tabControl3.TabPages[tabControl3.SelectedIndex].Controls.Remove(lastBox);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Png Image Files|*.png|All files (*.*)|*.*";
            if (open.ShowDialog(this) == DialogResult.OK)
                textBox4.Text = open.FileName;
        }
        Point olocaiton = new Point();
        void l_MouseClick(object sender, MouseEventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ((Label)sender).Tag = "";
                    DialogResult result = MessageBox.Show("Is territory \"" + ((Label)sender).Text + "\" water?", "Is Territory Water?", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        ((Label)sender).BackColor = Color.DodgerBlue;
                        if (((Label)sender).Tag is string)
                            ((Label)sender).Tag = ((string)((Label)sender).Tag) + "Water";
                    }
                    else if (result == DialogResult.Cancel)
                        return;
                    else
                    {
                        ((Label)sender).BackColor = Color.LightGreen;
                        result = MessageBox.Show("Is territory \"" + ((Label)sender).Text + "\" a victory city?", "Is Territory Victory City?", MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Yes)
                        {
                            ((Label)sender).BackColor = Color.Red;
                            if (((Label)sender).Tag is string)
                                ((Label)sender).Tag = ((string)((Label)sender).Tag) + "VictoryCity";
                        }
                        else if (result == DialogResult.Cancel)
                            return;
                        result = MessageBox.Show("Is territory \"" + ((Label)sender).Text + "\" impassable?", "Is Territory Impassable?", MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Yes)
                        {
                            ((Label)sender).BackColor = Color.DarkGray;
                            if (((Label)sender).Tag is string)
                                ((Label)sender).Tag = ((string)((Label)sender).Tag) + "Impassable";
                        }
                        else if (result == DialogResult.Cancel)
                            return;
                        result = MessageBox.Show("Is territory \"" + ((Label)sender).Text + "\" a capitol?", "Is Territory Capitol?", MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Yes)
                        {
                            ((Label)sender).BackColor = Color.Violet;
                            if (((Label)sender).Tag is string)
                                ((Label)sender).Tag = ((string)((Label)sender).Tag) + "Capitol";
                        }
                        else if (result == DialogResult.Cancel)
                            return;
                    }
                }
                else
                {
                    if (MessageBox.Show("Are you sure you want to remove this territory?", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        TerritoryDefinitionsImageDrawer.Controls.Remove((Control)sender);
                        Step2Info.territories.Remove(((Control)sender).Text);
                        List<Connection> ToRemove = new List<Connection>();
                        foreach (String cur in Step3Info.connections.Keys)
                        {
                            if (cur.Contains(((Control)sender).Text))
                                ToRemove.Add(Step3Info.connections[cur]);
                        }
                        for (int i = 0; i < ToRemove.Count; i++)
                        {
                            Connection cur = ToRemove[i];
                            Step3Info.connections.Remove(cur.t1.Name + "|" + cur.t2.Name);
                        }
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                Label l = (Label)sender;
                if (e.Button == MouseButtons.Left)
                {
                    if (L1 == null)
                    {
                        L1 = l;
                        l.ForeColor = Color.Yellow;
                    }
                    else
                    {
                        L2 = l;
                        L1.ForeColor = Color.Black;
                        try
                        {
                            if (L1.Text != L2.Text)
                            {
                                Step3Info.connections.Add(L1.Text + "|" + L2.Text, new Connection() { t1 = Step2Info.territories[L1.Text], t2 = Step2Info.territories[L2.Text] });
                                Graphics.FromImage(TerritoryConnectionsImageDrawer.BackgroundImage).DrawLine(Pens.Red, L1.Location + new Size(L1.Size.Width / 2, 0), L2.Location + new Size(L2.Size.Width / 2, 0));
                                TerritoryConnectionsImageDrawer.Refresh();
                            }
                        }
                        catch { }
                        L1 = null;
                    }
                }
                else
                {
                    Step1Info.MapImageWL = new Bitmap(Step1Info.MapImage);
                    TerritoryConnectionsImageDrawer.BackgroundImage = Step1Info.MapImageWL;
                    TerritoryConnectionsImageDrawer.Size = TerritoryDefinitionsImageDrawer.BackgroundImage.Size;
                    List<Connection> ToRemove = new List<Connection>();
                    foreach (String cur in Step3Info.connections.Keys)
                    {
                        if (cur.ToLower().Contains(l.Text.ToLower()))
                            ToRemove.Add(Step3Info.connections[cur]);
                    }
                    for (int i = 0; i < ToRemove.Count; i++)
                    {
                        Connection cur = ToRemove[i];
                        Step3Info.connections.Remove(cur.t1.Name + "|" + cur.t2.Name);
                    }
                    foreach (Connection cur in Step3Info.connections.Values)
                    {
                        Graphics.FromImage(TerritoryConnectionsImageDrawer.BackgroundImage).DrawLine(Pens.Red, cur.t1.Label.Location + new Size(cur.t1.Label.Size.Width / 2, 0), cur.t2.Label.Location + new Size(cur.t2.Label.Size.Width / 2, 0));
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 10)
            {
                Label l = (Label)sender;
                if (e.Button == MouseButtons.Left)
                {
                    String s;
                    //if (Settings.DisplayCurrentUnitsWhenEnteringNewUnits == false)
                    s = RetrieveString("Enter territory \"" + l.Tag.ToString() + "\"'s production amount.", Convert.ToInt32(l.Text)).Trim();
                    //else
                    //    s = RetrieveString("Enter territory \"" + l.Tag.ToString() + "\"'s production amount.", Step2Info.territories[l.Tag.ToString()].Production.ToString());
                    if (s.Length > 0)
                    {
                        try
                        {
                            int i = Convert.ToInt32(s);
                            l.Text = s;
                        }
                        catch { }
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 11)
            {
                Label l = (Label)sender;
                if (e.Button == MouseButtons.Left)
                {
                    if (L1 == null)
                    {
                        l.ForeColor = Color.Yellow;
                        L1 = l;
                    }
                    else
                    {

                        L1.ForeColor = Color.Black;
                        if (MessageBox.Show("Do you want these territories to be a canal?", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                        {
                            L1.BackColor = Color.Red;
                            l.BackColor = Color.Red;
                            Canal c = new Canal() { Name = l.Text + "|" + L1.Text };
                            c.CanalSeaNeighbors.Add(Step2Info.territories[L1.Text]);
                            c.CanalSeaNeighbors.Add(Step2Info.territories[l.Text]);
                            List<Territory> L1N = getLandNeighbors(Step2Info.territories[L1.Text]);
                            foreach (Territory cur in getLandNeighbors(Step2Info.territories[l.Text]))
                            {
                                if (L1N.Contains(cur))
                                    c.LandTerritories.Add(cur);
                            }
                            try
                            {
                                Step12Info.Canals.Add(l.Text + "|" + L1.Text, c);
                            }
                            catch
                            {
                                Step12Info.Canals.Remove(l.Text + "|" + L1.Text);
                                Step12Info.Canals.Add(l.Text + "|" + L1.Text, c);
                            }
                        }
                        L1 = null;
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    if (MessageBox.Show("Are you sure you want to remove all canals?", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        Step12Info.Canals.Clear();
                        foreach (Control cur in CanalsImageDrawer.Controls)
                        {
                            if (cur.BackColor == Color.Red)
                            {
                                cur.BackColor = TerritoryDefinitionsImageDrawer.Controls[cur.Text].BackColor;
                            }
                        }
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 12)
            {
                Label l = (Label)sender;
                if (e.Button == MouseButtons.Left)
                {
                    String s;
                    //if (Settings.DisplayCurrentUnitsWhenEnteringNewUnits == false)
                    s = RetrieveString("Enter territory \"" + l.Tag + "\"'s new owner.", l.Text, getPlayersWithNeutral()).Trim();
                    //else
                    //    s = RetrieveString("Enter territory \"" + l.Tag + "\"'s new owner.", Step2Info.territories[(string)l.Tag].Owner.Name);
                    if (s.Length > 0)
                    {
                        l.Location = new Point((l.Location.X + ((int)(l.Size.Width / 2))) - (int)(Graphics.FromImage(new Bitmap(1, 1)).MeasureString(s, l.Font).Width / 2), l.Location.Y);
                        int index = 0;
                        bool found = false;
                        foreach (Player cur in Step4Info.players.Values)
                        {
                            if (cur.Name == s)
                            {
                                found = true;
                                break;
                            }
                            index++;
                        }
                        if (index == 0)
                            l.BackColor = Color.Red;
                        else if (index == 1)
                            l.BackColor = Color.Orange;
                        else if (index == 2)
                            l.BackColor = Color.Yellow;
                        else if (index == 3)
                            l.BackColor = Color.Green;
                        else if (index == 4)
                            l.BackColor = Color.Blue;
                        else if (index == 5)
                            l.BackColor = Color.Indigo;
                        else if (index == 6)
                            l.BackColor = Color.Violet;

                        if (!found)
                            l.BackColor = Color.White;
                        l.Text = s;
                    }
                }
            }
            else if (tabControl1.SelectedIndex == 13)
            {
                Label l = (Label)sender;
                if (e.Button == MouseButtons.Left)
                {
                    //try
                    //{
                    //    if (((string)l.Tag).Contains("|"))
                    //        sretriever.textBox1.Text = ((string)l.Tag).Substring(((string)l.Tag).IndexOf("|") + 1);
                    //}
                    //catch { }
                    String s;
                    if (Settings.DisplayCurrentUnitsWhenEnteringNewUnits == false)
                        s = RetrieveString("Enter territory \"" + l.Text + "\"'s units.").Trim();
                    else
                    {
                        try
                        {
                            s = RetrieveString("Enter territory \"" + l.Text + "\"'s units.", l.Tag.ToString().Substring(0, l.Tag.ToString().IndexOf("|"))).Trim();
                        }
                        catch { s = RetrieveString("Enter territory \"" + l.Text + "\"'s units.", "").Trim(); }
                    }
                    string s2 = "";
                    if (Step2Info.territories[l.Text].IsWater)
                    {
                        s2 = RetrieveString("Enter the owner of these units.", "Neutral", getPlayersWithNeutral());
                        if (s2.ToLower().Trim() == "neutral")
                            s2 = "";
                        l.AccessibleName = s2;
                    }
                    else
                    {
                        l.AccessibleName = Step2Info.territories[l.Text].Owner.Name;
                    }
                    if (s.Length > 0 || !s.ToLower().Trim().Equals((string)l.Tag))
                    {
                        l.BackColor = Color.LightGray;
                        l.Tag = s;
                        l.Tag += "|" + l.Text;
                    }
                }
            }
        }
        public List<Territory> getLandNeighbors(Territory t)
        {
            List<Territory> results = new List<Territory>();
            foreach (Territory cur in Step2Info.territories.Values)
            {
                if (cur.IsWater == false)
                {
                    foreach (Connection cur2 in Step3Info.connections.Values)
                    {
                        if ((cur2.t1.Name == t.Name && cur2.t2.Name == cur.Name) || (cur2.t2.Name == t.Name && cur2.t1.Name == cur.Name))
                            results.Add(cur);
                    }
                }
            }
            return results;
        }
        public List<Territory> getTResults(Matches match)
        {
            List<Territory> results = new List<Territory>();
            foreach (Territory cur in Step2Info.territories.Values)
            {
                if (match == Matches.IsLand)
                {
                    if (cur.IsWater == false)
                        results.Add(cur);
                }
            }
            return results;
        }
        public enum Matches
        {
            IsLand
        }
        public Label L1 = null;
        public Label L2 = new Label();
        public Point GetPosition()
        {
            return new Point(olocaiton.X - this.Location.X - this.tabControl1.Location.X - TerritoryDefinitionsImageDrawer.Location.X - 15, olocaiton.Y - this.Location.Y - this.tabControl1.Location.Y - TerritoryDefinitionsImageDrawer.Location.Y - 92);
        }
        StringRetriever sretriever = new StringRetriever();
        public String RetrieveString(string labelString)
        {
            sretriever.parent = this;
            return sretriever.RetrieveString(labelString);
        }
        public String RetrieveString(string labelString,string textBoxString)
        {
            sretriever.parent = this;
            return sretriever.RetrieveString(labelString,textBoxString);
        }
        public String RetrieveString(string labelString, string textBoxString,object[] comboBoxItems)
        {
            sretriever.parent = this;
            return sretriever.RetrieveString(labelString, textBoxString,comboBoxItems);
        }
        public String RetrieveString(string labelString, int numberToDisplay)
        {
            sretriever.parent = this;
            return sretriever.RetrieveString(labelString,numberToDisplay);
        }
        Size origSize;
        private void Form1_Resize(object sender, EventArgs e)
        {
            Size change = new Size(Size.Width - origSize.Width, Size.Height - origSize.Height);


            tabControl1.Size = ((Size)tabControl1.Tag); tabControl1.Size += change;
            panel2.Location = ((Point)panel2.Tag); panel2.Location += new Size(change.Width / 2,change.Height);
            panel17.Location = ((Point)panel17.Tag); panel17.Location += new Size(change.Width / 2, change.Height);
            button26.Location = ((Point)button26.Tag); button26.Location += new Size(change.Width / 2, change.Height);            
            button15.Location = ((Point)button15.Tag); button15.Location += new Size(0,change.Height);
            button16.Location = ((Point)button16.Tag); button16.Location += new Size(0,change.Height);
            panel4.Size = ((Size)panel4.Tag); panel4.Size += change;
            panel10.Size = ((Size)panel10.Tag); panel10.Size += change;
            panel11.Size = ((Size)panel11.Tag); panel11.Size += change;
            panel12.Size = ((Size)panel12.Tag); panel12.Size += change;
            panel13.Size = ((Size)panel13.Tag); panel13.Size += change;
            panel14.Size = ((Size)panel14.Tag); panel14.Size += change;
            mapNotesTextBox.Size = ((Size)mapNotesTextBox.Tag); mapNotesTextBox.Size += change;
            tabControl2.Size = ((Size)tabControl2.Tag); tabControl2.Size += change;
            tabControl3.Size = ((Size)tabControl3.Tag); tabControl3.Size += change;
        }
        int change = 0;
        private void button6_Click(object sender, EventArgs e)
        {
            infochange1 = true;
            change += 25;
            panel5.Size += new Size(0, 25);
            button6.Location += new Size(0, 25);
            button7.Location += new Size(0, 25);
            tabPage4.Controls.AddRange(new Control[] { new TextBox() { Size = textBox5.Size, Location = new Point(textBox5.Location.X, textBox5.Location.Y + change - tabPage4.VerticalScroll.Value) }, new TextBox() { Size = textBox6.Size, Location = new Point(textBox6.Location.X, textBox6.Location.Y + change - tabPage4.VerticalScroll.Value) }, new TextBox() { Size = textBox24.Size, Location = new Point(textBox24.Location.X, textBox24.Location.Y + change - tabPage4.VerticalScroll.Value) } });
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (change > 0)
            {
                infochange1 = true;
                tabPage4.Controls.RemoveAt(tabPage4.Controls.Count - 1);
                tabPage4.Controls.RemoveAt(tabPage4.Controls.Count - 1);
                tabPage4.Controls.RemoveAt(tabPage4.Controls.Count - 1);
                change -= 25;
                panel5.Size += new Size(0, -25);
                button6.Location += new Size(0, -25);
                button7.Location += new Size(0, -25);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text Files|*.txt|All files (*.*)|*.*";
            if (open.ShowDialog(this) == DialogResult.OK)
                textBox7.Text = open.FileName;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (stepIndex == 1)
            {
                MessageBox.Show("Map Name: The name of the map. Examples: Revised, Classic, Big World, and Great War\r\n\r\nMap Version: The map release version. Examples: 1.0.0.0, 2.5.0.0, and 1.7.0.1\r\n\r\nResource Name: The name of the resource to use in the map. Examples: IPCs, Gold, and Silver\r\n\r\nMap Image Location: The location of the map image. Example: C:/My Maps/Sleeping Giant/full_map.png\r\n\r\nCenters File: The location of a premade centers file produced by the 'Center Picker' program. It is used later to automatically add territories by using the centers file content. Example: C:/My Maps/Sleeping Giant/centers.txt\r\n\r\nWater Territory Filter: An optional setting that makes the program automatically apply the 'Is Water' property to every territory that contains the filter text. Examples: SZ, Sea Zone, Pacific, and Atlantic.", "Help On Current Step");
            }
            else if (stepIndex == 2)
            {
                MessageBox.Show("To add a new territory, click on the location of the territory and enter its name in the window that appears. If you want to edit the properties of a territory, left click on it and answer each question. The color of the territory label changes for each property that is applies. If you want to remove a territory label, right click on it and click yes when it asks for confirmation.", "Help On Current Step");
            }
            else if (stepIndex == 3)
            {
                MessageBox.Show("To add a connection between two territories, click on the first territory in the connection then the second. To remove all the connections from a certain territory, right click on it and click yes.\r\n\r\nNote: To have the program find the connections automatticaly, click the 'Auto-Fill' button between the Back and Next buttons.", "Help On Current Step");
            }
            else if (stepIndex == 4)
            {
                MessageBox.Show("Player Name: The name of the player. Examples: Russians, Germans, British, Americans, Chinese, and Italians.\r\n\r\nPlayer Alliance: The alliance the player belongs to. Examples: Allies, and Axis.\r\n\r\nInitial Resources: The amount of resources(IPCs) the player gets in the beginning.", "Help On Current Step");
            }
            else if (stepIndex == 5)
            {
                MessageBox.Show("Unit Name: The name of the unit. Examples: Infantry, Artillery, Tank, Fighter, Bomber, and Transport.\r\n\r\nBuy Cost: The cost of the unit.\r\n\r\nBuy Quantity: How many units to recieve with each buy.\r\n\r\nTo have the program automatically enter the default units, click the 'Auto-Fill' button between the Back and Next buttons.", "Help On Current Step");
            }
            else if (stepIndex == 6)
            {
                MessageBox.Show("Sequence Name: The name of the sequence. Examples(Typical): tech, techActivation, battle, move, place, purchase, endTurn, placeBid, bid.\r\n\r\nClass Name: The name of the java class. Examples(Typical): TechnologyDelegate, TechActivationDelegate, BattleDelegate, MoveDelegate, PlaceDelegate, PurchaseDelegate, EndTurnDelegate, BidPlaceDelegate, BidPurchaseDelegate.\r\n\r\nDisplay: The text displayed on the game. Examples(Typical): Research Technology, Activate Technology, Combat, Combat Move, Place Units, Purchase Units, Turn Complete, Bid Placement, and Bid Purchase\r\n\r\nTo have the program automatically enter the default Gameplay Sequences, click the 'Auto-Fill' button between the Back and Next buttons.", "Help On Current Step");
            }
            else if (stepIndex == 7)
            {
                MessageBox.Show("Sequence Name: The name of the sequence. Examples: russianBid, germanBidPlace, chineseTech, americanCombatMove, and germanPlace.\r\n\r\nGameplay Sequence: The name of the Gameplay Sequence to call. Examples: bid, tech, move, place, endTurn.\r\n\r\nPlayer: The name of the player the Player Delegate applies to. Examples: Russians, Germans, Americans, and Chinese.\r\n\r\nMax Run Count: The maximum number of times the Sequence can be called for the whole game (Always set to 100 or more unless on a 'bid' sequence).\r\n\r\nTo have the porgram automattically enter the default Player Sequences, click the 'Auto-Fill' button between the Back and Next buttons.", "Help On Current Step");
            }
            else if (stepIndex == 8)
            {
                MessageBox.Show("Technology Name: The name of the technology. Examples(Typical): heavyBomber, jetPower, industrialTechnology, superSub, rocket, and longRangeAir.\r\n\r\nPlayer: The player it applies to. Examples: Russians, Germans, Americans, and Chinese.\r\n\r\nAlready Enabled: Determines if the technology should be in use for the selected player when the game first starts.\r\n\r\nTo have the program automatically enter the default technologies, click the 'Auto-Fill' button between the Back and Next buttons.", "Help On Current Step");
            }
            else if (stepIndex == 9)
            {
                MessageBox.Show("Unit Name: The name of the unit in the production frontier. Examples: Infantry, Artillery, and Tank.\r\n\r\nTo have the program automatically add all the units available, click the 'Auto-Fill' button between the Back and Next buttons.", "Help On Current Step");
            }
            else if (stepIndex == 10)
            {
                MessageBox.Show("Attachment Name: The name of the attachment. Examples: movement, attack, defense, isAir, isSea, and isStrategicBomber.\r\n\r\nValue: The value of the attachment. Examples: True, False, 1, 2\r\n\r\nTo have the program automatically enter the default attachments for the unit being viewed, click the 'Auto-Fill' button between the Back and Next buttons.", "Help On Current Step");
            }
            else if (stepIndex == 11)
            {
                MessageBox.Show("To change the production of a territory, click on the territory and enter the territory's new production value in the window that appears.", "Help On Current Step");
            }
            else if (stepIndex == 12)
            {
                MessageBox.Show("To add a new canal, click on both of the water territories touching the canal and click yes for each of them when it asks for confirmation. To remove all the canals right click on a territory and click yes", "Help On Current Step");
            }
            else if (stepIndex == 13)
            {
                MessageBox.Show("To change a territory's owner, click on the territory and type in its new owner when it asks for it.", "Help On Current Step");
            }
            else if (stepIndex == 14)
            {
                MessageBox.Show("To change what units a territory has, click on the territory and type in the name of each unit you want to add, followed by ':', followed by the unit quantitiy: Example: 'Infantry: 1, Artillery: 3, Tank: 2, Fighter: 1'.", "Help On Current Step");
            }
            else if (stepIndex == 15)
            {
                MessageBox.Show("Setting Name: The name of the setting. Examples: Always on AA, Two hit battleship, and Japanese bid.\r\n\r\nValue: The value of the setting. Examples: true, false, 0, 5, 32.\r\n\r\nEditable: Whether the setting should be able to be changed by the player.\r\n\r\nMin. N. (Optional): The lowest number that the player can set the value to.\r\n\r\nMax. N.(Optional): The highest number that the player can set the value to.\r\n\r\nTo have the program automatically enter the defualt In-Game Settings, click the 'Auto-Fill' button between the Back and Next buttons.", "Help On Current Step");
            }
            else if (stepIndex == 16)
            {
                MessageBox.Show("To add the Map Notes, just type what you want the Map Notes to say in the text box. Then just click the 'Save Map To File' button, and select where you want to save the finished map xml file.", "Help On Current Step");
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (textBox7.Text.Length > 0)
                textBox8.Enabled = true;
            else
                textBox8.Enabled = false;
        }
        int change2 = 0;
        private void button10_Click(object sender, EventArgs e)
        {
            infochange2 = true;
            infochange3 = true;
            change2 += 25;
            panel6.Size += new Size(0, 25);
            button9.Location += new Size(0, 25);
            button10.Location += new Size(0, 25);
            tabPage5.Controls.AddRange(new Control[] { new TextBox() { Size = textBox9.Size, Location = new Point(textBox9.Location.X, textBox9.Location.Y + change2 - tabPage5.VerticalScroll.Value) }, new TextBox() { Size = textBox10.Size, Location = new Point(textBox10.Location.X, textBox10.Location.Y + change2 - tabPage5.VerticalScroll.Value) }, new TextBox() { Size = textBox11.Size, Location = new Point(textBox11.Location.X, textBox11.Location.Y + change2 - tabPage5.VerticalScroll.Value) } });
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (change2 > 0)
            {
                infochange2 = true;
                infochange3 = true;
                tabPage5.Controls.RemoveAt(tabPage5.Controls.Count - 1);
                tabPage5.Controls.RemoveAt(tabPage5.Controls.Count - 1);
                tabPage5.Controls.RemoveAt(tabPage5.Controls.Count - 1);
                change2 -= 25;
                panel6.Size += new Size(0, -25);
                button9.Location += new Size(0, -25);
                button10.Location += new Size(0, -25);
            }
        }
        int change3 = 0;
        private void button11_Click(object sender, EventArgs e)
        {
            if (change3 > 0)
            {
                tabPage6.Controls.RemoveAt(tabPage6.Controls.Count - 1);
                tabPage6.Controls.RemoveAt(tabPage6.Controls.Count - 1);
                tabPage6.Controls.RemoveAt(tabPage6.Controls.Count - 1);
                change3 -= 25;
                panel7.Size += new Size(0, -25);
                button11.Location += new Size(0, -25);
                button12.Location += new Size(0, -25);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            change3 += 25;
            panel7.Size += new Size(0, 25);
            button11.Location += new Size(0, 25);
            button12.Location += new Size(0, 25);
            tabPage6.Controls.AddRange(new Control[] { new TextBox() { Size = textBox12.Size, Location = new Point(textBox12.Location.X, textBox12.Location.Y + change3 - tabPage6.VerticalScroll.Value) }, new TextBox() { Size = textBox14.Size, Location = new Point(textBox14.Location.X, textBox14.Location.Y + change3 - tabPage6.VerticalScroll.Value) }, new TextBox() { Size = textBox13.Size, Location = new Point(textBox13.Location.X, textBox13.Location.Y + change3 - tabPage6.VerticalScroll.Value) } });
        }
        int change4 = 0;
        private void button13_Click(object sender, EventArgs e)
        {
            if (change4 > 0)
            {
                tabPage7.Controls.RemoveAt(tabPage7.Controls.Count - 1);
                tabPage7.Controls.RemoveAt(tabPage7.Controls.Count - 1);
                tabPage7.Controls.RemoveAt(tabPage7.Controls.Count - 1);
                tabPage7.Controls.RemoveAt(tabPage7.Controls.Count - 1);
                change4 -= 25;
                panel8.Size += new Size(0, -25);
                button13.Location += new Size(0, -25);
                button14.Location += new Size(0, -25);
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("BFChange4:" + change4.ToString() + ",Base Box Loc:" + comboBox16.Location + ", Combined Y:" + (comboBox16.Location.Y + change4).ToString());
            change4 += 25;
            panel8.Size += new Size(0, 25);
            button13.Location += new Size(0, 25);
            button14.Location += new Size(0, 25);
            ComboBox cb1 = new ComboBox() { Size = comboBox16.Size, Location = new Point(comboBox16.Location.X, comboBox16.Location.Y + change4 - tabPage7.VerticalScroll.Value) };
            ComboBox cb2 = new ComboBox() { Size = comboBox17.Size, Location = new Point(comboBox17.Location.X, comboBox17.Location.Y + change4 - tabPage7.VerticalScroll.Value) };
            cb1.Items.AddRange(getGameplaySequences());
            cb2.Items.AddRange(getPlayers());
            //MessageBox.Show("AFChange4:" + change4.ToString() + ",Base Box Loc:" + comboBox16.Location + ", Combined Y:" + (comboBox16.Location.Y + change4).ToString());
            tabPage7.Controls.AddRange(new Control[] { new TextBox() { Size = textBox15.Size, Location = new Point(textBox15.Location.X, textBox15.Location.Y + change4 - tabPage7.VerticalScroll.Value) }, cb1, cb2, new TextBox() { Size = textBox18.Size, Location = new Point(textBox18.Location.X, textBox18.Location.Y + change4 - tabPage7.VerticalScroll.Value) } });
        }
        public object[] getGameplaySequences()
        {
            List<object> result = new List<object>();
            foreach (GameplaySequence cur in Step6Info.gameplaySequences.Values)
            {
                result.Add(cur.Name);
            }
            return result.ToArray();
        }
        public Object[] getObjectArray(ComboBox.ObjectCollection collection)
        {
            List<Object> result = new List<object>();
            IEnumerator enumerator = collection.GetEnumerator();
            while(enumerator.MoveNext())
            {
                result.Add(enumerator.Current);
            }
            return result.ToArray();
        }
        public Control[] getControlArray(ComboBox.ControlCollection collection)
        {
            List<Control> result = new List<Control>();
            IEnumerator enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext())
            {
                result.Add((Control)enumerator.Current);
            }
            return result.ToArray();
        }
        public void Save()
        {
                SaveFileDialog d4 = new SaveFileDialog();
                d4.Title = "Select a location to save the xml file...";
                d4.Filter = "Xml files|*.xml|All files (*.*)|*.*";
                d4.DefaultExt = ".xml";
                d4.OverwritePrompt = true;
                if (d4.ShowDialog(this) == DialogResult.OK)
                {
                    Write(d4.FileName);
                }
        }
        System.Windows.Forms.Timer t2 = new System.Windows.Forms.Timer();
        Thread t = null;
        public void Load2()
        {
            t2.Interval = 500;
            t2.Tick += new EventHandler(t2_Tick);
            panel2.Enabled = false;
            button15.Enabled = false;
            button16.Enabled = false;
            tabControl1.Enabled = false;
            t2.Start();
            t = new Thread(new ThreadStart(LoadMap));
            Form1.CheckForIllegalCrossThreadCalls = false;
            t.Start();
            //do1();
            //Stop();
        }
        int n2 = 1;
        void t2_Tick(object sender, EventArgs e)
        {
            if(n2 == 1)
            {
                this.Text = "TripleA Map Creator - Part 2 (Loading.)";
                n2++;
            }
            else if (n2 == 2)
            {
                this.Text = "TripleA Map Creator - Part 2 (Loading..)";
                n2++;
            }
            else
            {
                this.Text = "TripleA Map Creator - Part 2 (Loading...)";
                n2 = 1;
            }
        }
        public void LoadMap()
        {
            bool errorOccured = false;
            Step1Info.LoadedFile = "";
            string textThatFailedParsing = "";
            int indexOfTextThatFailed = -1;
            int guessedLineNumberThatFailed = -1;
            Exception thrownException = new Exception();
            try
            {
                OpenFileDialog d4 = new OpenFileDialog();
                d4.Title = "Select the Xml file to load...";
                d4.Filter = "Xml files|*.xml|All files (*.*)|*.*";
                if (d4.ShowDialog(this) != DialogResult.Cancel)
                {
                    string imageSelectStartFolder = new FileInfo(d4.FileName).DirectoryName;
                    Control oldControl = getControl("label" + stepIndex);
                    Control newControl = getControl("label1");
                    oldControl.Font = new Font(oldControl.Font, FontStyle.Regular);
                    newControl.Font = new Font(newControl.Font, FontStyle.Bold);
                    stepIndex = 1;
                    oldStepIndex = 1;
                    surpress1 = true;
                    surpress2 = true;
                    tabControl1.SelectedIndex = 0;
                    surpress1 = false;
                    surpress2 = false;
                    Point loc = new Point();
                    string centerLocation = "";
                    string imageLoc = "";
                    string[] xmlLines = File.ReadAllLines(d4.FileName);
                    string text = "";
                    int lineIndex = 0;
                    foreach(string cur in xmlLines)
                    {
                        if (cur.Contains(">"))
                            text = String.Concat(text, cur);
                        else
                            lineIndex++;

                    }
                    if (File.Exists(new FileInfo(d4.FileName).DirectoryName + @"\centers.txt"))
                    {
                        centerLocation = new FileInfo(d4.FileName).DirectoryName + @"\centers.txt";
                        imageSelectStartFolder = new FileInfo(d4.FileName).DirectoryName;
                    }
                    else
                    {
                        bool found = false;
                        string mn = "";
                        if (text.Contains("name=\"mapName\""))
                        {
                            string mnss = text.Substring(text.IndexOf("name=\"mapName\""));
                            mn = mnss.Substring(mnss.IndexOf(" value=\"") + 8, mnss.Substring(mnss.IndexOf(" value=\"") + 8).IndexOf("\""));
                        }
                        DirectoryInfo mapsFolder = null;
                        foreach (DirectoryInfo cur in new FileInfo(d4.FileName).Directory.Parent.GetDirectories())
                        {
                            if (cur.Name.ToLower() == "maps")
                            {
                                mapsFolder = cur;
                            }
                        }
                        if (mapsFolder == null)
                        {
                            foreach (DirectoryInfo cur in new FileInfo(d4.FileName).Directory.Parent.Parent.GetDirectories())
                            {
                                if (cur.Name.ToLower() == "maps")
                                {
                                    mapsFolder = cur;
                                }
                            }
                        }
                        if (mapsFolder == null)
                        {
                            foreach (DirectoryInfo cur in new FileInfo(d4.FileName).Directory.Parent.Parent.Parent.GetDirectories())
                            {
                                if (cur.Name.ToLower() == "maps")
                                {
                                    mapsFolder = cur;
                                }
                            }
                        }
                        if (mapsFolder == null)
                        {
                            foreach (DirectoryInfo cur in new FileInfo(d4.FileName).Directory.Parent.Parent.Parent.Parent.GetDirectories())
                            {
                                if (cur.Name.ToLower() == "maps")
                                {
                                    mapsFolder = cur;
                                }
                            }
                        }

                        if (!found)
                        {
                            foreach (DirectoryInfo cur2 in mapsFolder.GetDirectories())
                            {
                                if (cur2.Name.ToLower() == mn.ToLower())
                                {
                                    found = true;
                                    centerLocation = cur2.FullName + "/centers.txt";
                                    imageSelectStartFolder = new DirectoryInfo(cur2.FullName).FullName;
                                }
                            }
                        }
                        if (!found)
                        {
                            foreach (DirectoryInfo cur2 in mapsFolder.GetDirectories())
                            {
                                if (cur2.Name.Replace(" ", "").Replace("_", "") == new FileInfo(d4.FileName).Name.Replace(" ", "").Replace("_", ""))
                                {
                                    found = true;
                                    centerLocation = cur2.FullName + "/centers.txt";
                                    imageSelectStartFolder = new DirectoryInfo(cur2.FullName).FullName;
                                }
                            }
                        }
                        if (!found)
                        {
                            string parentHomeFolder = @"C:\Program Files\TripleA\";
                            if (!new DirectoryInfo(parentHomeFolder).Exists)
                            {
                                FolderBrowserDialog od = new FolderBrowserDialog();
                                od.Description = "Please locate the TripleA Program's folder. (Where you installed TripleA)";
                                od.ShowNewFolderButton = false;
                                if (Directory.Exists(@"C:\Program Files\"))
                                    od.SelectedPath = @"C:\Program Files\";
                                if (od.ShowDialog() != DialogResult.Cancel)
                                {
                                    if (!new DirectoryInfo(od.SelectedPath).Exists)
                                        return;
                                    foreach (DirectoryInfo d in new DirectoryInfo(od.SelectedPath).GetDirectories())
                                    {
                                        if (d.Name.ToLower() == "bin" || d.Name.ToLower() == "maps")
                                        {
                                            od.SelectedPath = d.Parent.Parent.FullName;
                                            break;
                                        }
                                    }
                                    parentHomeFolder = od.SelectedPath;
                                }
                                else return;
                            }
                            if (!new DirectoryInfo(parentHomeFolder).Exists)
                                return;
                            DirectoryInfo newestTripleAVersion = new DirectoryInfo(parentHomeFolder).GetDirectories()[0];
                            foreach (DirectoryInfo cur in new DirectoryInfo(parentHomeFolder).GetDirectories())
                            {
                                try
                                {
                                    if (Convert.ToInt32(cur.Name.Substring(cur.Name.IndexOf("_")).Replace("_", "")) > Convert.ToInt32(newestTripleAVersion.Name.Substring(newestTripleAVersion.Name.IndexOf("_")).Replace("_", "")) && File.Exists(cur.FullName + "/bin/triplea.jar"))
                                        newestTripleAVersion = cur;
                                }
                                catch { }
                            }
                            string homeFolder = newestTripleAVersion.FullName;

                            if (File.Exists(homeFolder + @"\Maps\" + mn + @"\centers.txt"))
                            {
                                found = true;
                                centerLocation = homeFolder + @"\Maps\" + mn + @"\centers.txt";
                                imageSelectStartFolder = new DirectoryInfo(homeFolder + @"\Maps\" + mn).FullName;
                            }
                        }
                        if (!found)
                        {
                            OpenFileDialog open = new OpenFileDialog();
                            open.Title = "Unable to locate the centers file. Please select the 'centers.txt' file for the map.";
                            open.Filter = "Text Files|*.txt|All files (*.*)|*.*";
                            if (open.ShowDialog(this) == DialogResult.OK)
                            {
                                centerLocation = open.FileName;
                                imageSelectStartFolder = new FileInfo(open.FileName).DirectoryName;
                            }
                            else
                            {
                                Stop();
                                return;
                            }
                        }
                    }
                    ClearAllDataAndControls();
                    try
                    {
                        List<Label> labels = new List<Label>();
                        String[] lines = File.ReadAllLines(centerLocation);
                        TerritoryDefinitionsImageDrawer.Controls.Clear();
                        Step2Info.territories.Clear();
                        foreach (String cur2 in lines)
                        {
                            String name = cur2.Substring(0, cur2.IndexOf('('));
                            String sPoint = cur2.Substring(cur2.IndexOf('(') + 1, cur2.LastIndexOf(")") - (cur2.IndexOf("(") + 1));
                            Point point = new Point(Convert.ToInt32(sPoint.Substring(0, sPoint.IndexOf(","))), Convert.ToInt32(sPoint.Substring(sPoint.IndexOf(",") + 1, sPoint.Length - sPoint.IndexOf(",") - 1)));
                            labels.Add(new Label() { Text = name.Trim(), BackColor = textBox8.Text.Trim().Length > 0 && name.Contains(textBox8.Text) ? Color.DodgerBlue : Color.LightGreen, Font = new Font(label1.Font, FontStyle.Bold), Location = new Point(point.X - (int)(Graphics.FromImage(new Bitmap(1, 1)).MeasureString(name, new Font(label1.Font, FontStyle.Bold)).Width / 2), point.Y) });
                        }
                        foreach (Label cur2 in labels)
                        {
                            //MessageBox.Show(cur2.Text + "/" + cur2.Location.ToString());
                            Label l = new Label() { Text = cur2.Text, BackColor = cur2.BackColor, Font = cur2.Font, AutoSize = true, Location = cur2.Location };
                            l.MouseClick += new MouseEventHandler(l_MouseClick);
                            l.Name = l.Text;
                            Step2Info.territories.Add(l.Text, new Territory() { Label = l, Name = l.Text, IsWater = l.Text.Contains(textBox8.Text) });
                            TerritoryDefinitionsImageDrawer.Controls.Add(l);
                        }
                    }
                    catch { }
                    Size taS = new Size();
                    OpenFileDialog open2 = new OpenFileDialog();
                    open2.InitialDirectory = imageSelectStartFolder;
                    open2.Title = "Please select the map's image file. (Click cancel to use blank image)";
                    open2.Filter = "Png Files|*.png|All files (*.*)|*.*";
                    if (open2.ShowDialog(this) == DialogResult.OK)
                        imageLoc = open2.FileName;
                    else
                    {
                        imageLoc = "blank";
                        //Stop();
                        //return;
                    }
                    Image ita;
                    if (imageLoc == "blank")
                    {
                        foreach (Control cur2 in TerritoryDefinitionsImageDrawer.Controls)
                        {
                            if (cur2.Location.X + cur2.Size.Width > taS.Width)
                                taS.Width = cur2.Location.X + cur2.Size.Width + 10;
                            if (cur2.Location.Y + cur2.Size.Height > taS.Height)
                                taS.Height = cur2.Location.Y + cur2.Size.Height + 10;
                        }
                        ita = new Bitmap(taS.Width, taS.Height);
                        Step1Info.MapImage = ita;
                        Step1Info.MapImageWL = new Bitmap(taS.Width, taS.Height);
                        Graphics.FromImage(ita).FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), taS));
                        Graphics.FromImage(Step1Info.MapImageWL).FillRectangle(Brushes.White, new Rectangle(new Point(0, 0), taS));
                    }
                    else
                    {
                        ita = Image.FromFile(imageLoc);
                        taS = ita.Size;
                        Step1Info.MapImage = ita;
                        Step1Info.MapImageWL = Image.FromFile(imageLoc);
                    }
                    textBox4.Text = imageLoc.Replace(@"/", @"\");
                    textBox7.Text = centerLocation.Replace(@"/", @"\");
                    textBox6.Text = "";
                    TerritoryConnectionsImageDrawer.BackgroundImage = Step1Info.MapImageWL;
                    TerritoryDefinitionsImageDrawer.BackgroundImage = ita;
                    TerritoryDefinitionsImageDrawer.Size = TerritoryDefinitionsImageDrawer.BackgroundImage.Size;
                    int index = 0;
                    bool doBreak = false;
                    while(index < text.Length)
                    {
                        string cur = text.Substring(index,text.Substring(index).IndexOf(">") + 1);
                        if (cur.Length > 0)
                            index += cur.Length;
                        else
                            break;

                        if (!doBreak)
                        {
                            lineIndex++;

                            textThatFailedParsing = cur;
                            indexOfTextThatFailed = index;
                            guessedLineNumberThatFailed = lineIndex;
                        }
                        //MessageBox.Show(cur);
                        try
                        {
                            if (cur.Contains("<info ") && cur.Contains("name=\""))
                            {
                                textBox1.Text = cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\""));
                                textBox2.Text = cur.Substring(cur.IndexOf("version=\"") + 9, cur.Substring(cur.IndexOf("version=\"") + 9).IndexOf("\""));
                            }
                            else if (cur.Contains("<territory ") && cur.Contains("name=\""))
                            {
                                if (cur.Contains(" water=\"true\""))
                                {
                                    try
                                    {
                                        Step2Info.territories[cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\""))].IsWater = true;
                                        TerritoryDefinitionsImageDrawer.Controls[cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\""))].BackColor = Color.DodgerBlue;
                                        TerritoryDefinitionsImageDrawer.Controls[cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\""))].Tag = ((string)TerritoryDefinitionsImageDrawer.Controls[cur.Substring(cur.IndexOf("<territory name=\"") + 17, cur.Substring(cur.IndexOf("<territory name=\"") + 17).IndexOf("\""))].Tag) + "Water";
                                    }
                                    catch { }
                                }
                            }
                            else if (cur.Contains("<connection ") && cur.Contains("t1=\""))
                            {
                                try
                                {
                                    string L1name = cur.Substring(cur.IndexOf("t1=\"") + 4, cur.Substring(cur.IndexOf("t1=\"") + 4).IndexOf("\""));
                                    string L2name = cur.Substring(cur.IndexOf(" t2=\"") + 5, cur.Substring(cur.IndexOf(" t2=\"") + 5).IndexOf("\""));
                                    Label l1 = (Label)TerritoryDefinitionsImageDrawer.Controls[L1name];
                                    Label l2 = (Label)TerritoryDefinitionsImageDrawer.Controls[L2name];
                                    Step3Info.connections.Add(l1.Text + "|" + l2.Text, new Connection() { t1 = Step2Info.territories[l1.Text], t2 = Step2Info.territories[l2.Text] });
                                    Graphics.FromImage(TerritoryConnectionsImageDrawer.BackgroundImage).DrawLine(Pens.Red, l1.Location + new Size(l1.Size.Width / 2, 0), l2.Location + new Size(l2.Size.Width / 2, 0));
                                }
                                catch { }
                            }
                            else if (cur.Contains("<resource ") && cur.Contains("name=\""))
                            {
                                textBox3.Text = cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\""));
                            }
                            else if (cur.Contains("<player ") && cur.Contains("name=\""))
                            {
                                tplayers.Add(cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")), new Player() { Name = cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")) });
                            }
                            else if (cur.Contains("<alliance ") && cur.Contains("player=\""))
                            {
                                tplayers[cur.Substring(cur.IndexOf("player=\"") + 8, cur.Substring(cur.IndexOf("player=\"") + 8).IndexOf("\""))].Alliance = cur.Substring(cur.IndexOf("alliance=\"") + 10, cur.Substring(cur.IndexOf("alliance=\"") + 10).IndexOf("\""));
                            }
                            else if (cur.Contains("<unit ") && cur.Contains("name=\""))
                            {
                                tunits.Add(cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")).ToLower(), new Unit() { Name = cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")) });
                            }
                            else if (cur.Contains("<productionRule ") && cur.Contains("name=\""))
                            {
                                unitTName = cur.Substring(cur.IndexOf("name=\"") + 9, cur.Substring(cur.IndexOf("name=\"") + 9).IndexOf("\""));
                            }
                            else if (cur.Contains("<cost ") && cur.Contains("resource=\""))
                            {
                                try
                                {
                                    string numTP = cur.Substring(cur.IndexOf(" quantity=\"") + 11, cur.Substring(cur.IndexOf(" quantity=\"") + 11).IndexOf("\""));
                                    tunits[unitTName.ToLower()].cost.cost = Convert.ToInt32(numTP);
                                }
                                catch { }
                            }
                            else if (cur.Contains("<result ") && cur.Contains("resourceOrUnit=\""))
                            {
                                try
                                {
                                    tunits[unitTName.ToLower()].cost.result.ResourceOrUnitName = unitTName;
                                    tunits[unitTName.ToLower()].cost.result.BuyQuantity = Convert.ToInt32(cur.Substring(cur.IndexOf(" quantity=\"") + 11, cur.Substring(cur.IndexOf(" quantity=\"") + 11).IndexOf("\"")));
                                }
                                catch { }
                            }
                            else if (cur.Contains("<attatchment ") && cur.Contains("name=\"unitAttatchment\""))
                            {
                                unitTName2 = cur.Substring(cur.IndexOf("attatchTo=\"") + 11, cur.Substring(cur.IndexOf("attatchTo=\"") + 11).IndexOf("\"")).ToLower();
                            }
                            else if (cur.Contains("</attatchment>") && unitTName2.Length > 0)
                            {
                                AddUnit(tunits[unitTName2.ToLower()].Name, tunits[unitTName2.ToLower()].cost.cost.ToString(), tunits[unitTName2.ToLower()].cost.result.BuyQuantity.ToString());
                                AddUnitAttachment(tunits[unitTName2.ToLower()]);
                                unitTName2 = "";
                            }
                            else if (cur.Contains("<option ") && cur.Contains("name=\"") && unitTName2.Length > 0)
                            {
                                tunits[unitTName2.ToLower()].attachment.options.Add(new UnitOption() { Name = cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")), Value = cur.Substring(cur.IndexOf(" value=\"") + 8, cur.Substring(cur.IndexOf(" value=\"") + 8).IndexOf("\"")) });
                            }
                            else if (cur.Contains("<resourceGiven ") && cur.Contains("player=\""))
                            {
                                string num = cur.Substring(cur.IndexOf(" quantity=\"") + 11, cur.Substring(cur.IndexOf(" quantity=\"") + 11).IndexOf("\""));
                                tplayers[cur.Substring(cur.IndexOf("player=\"") + 8, cur.Substring(cur.IndexOf("player=\"") + 8).IndexOf("\""))].InitialResources = Convert.ToInt32(num);
                                AddPlayer(tplayers[cur.Substring(cur.IndexOf("player=\"") + 8, cur.Substring(cur.IndexOf("player=\"") + 8).IndexOf("\""))].Name, tplayers[cur.Substring(cur.IndexOf("player=\"") + 8, cur.Substring(cur.IndexOf("player=\"") + 8).IndexOf("\""))].Alliance, tplayers[cur.Substring(cur.IndexOf("player=\"") + 8, cur.Substring(cur.IndexOf("player=\"") + 8).IndexOf("\""))].InitialResources.ToString());
                            }
                            else if (cur.Contains("<delegate ") && cur.Contains("name=\""))
                            {
                                AddGameplayDelegate(cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")), cur.Substring(cur.IndexOf("javaClass=\"") + 11 + 32, cur.Substring(cur.IndexOf("javaClass=\"") + 11 + 32).IndexOf("\"")), cur.Substring(cur.IndexOf("display=\"") + 9, cur.Substring(cur.IndexOf("display=\"") + 9).IndexOf("\"")));
                            }
                            else if (cur.Contains("<step ") && cur.Contains("name=\""))
                            {
                                if (cur.Contains("player=\""))
                                {
                                    if (!cur.Contains(" maxRunCount=\""))
                                        AddPlayerDelegate(cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")), cur.Substring(cur.IndexOf(" delegate=\"") + 11, cur.Substring(cur.IndexOf(" delegate=\"") + 11).IndexOf("\"")), cur.Substring(cur.IndexOf(" player=\"") + 9, cur.Substring(cur.IndexOf(" player=\"") + 9).IndexOf("\"")), "0");
                                    else
                                        AddPlayerDelegate(cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")), cur.Substring(cur.IndexOf(" delegate=\"") + 11, cur.Substring(cur.IndexOf(" delegate=\"") + 11).IndexOf("\"")), cur.Substring(cur.IndexOf(" player=\"") + 9, cur.Substring(cur.IndexOf(" player=\"") + 9).IndexOf("\"")), cur.Substring(cur.IndexOf(" maxRunCount=\"") + 14, cur.Substring(cur.IndexOf(" maxRunCount=\"") + 14).IndexOf("\"")));
                                }
                                else
                                {
                                    //AddPlayerDelegate(cur.Substring(cur.IndexOf("<step name=\"") + 12, cur.Substring(cur.IndexOf("<step name=\"") + 12).IndexOf("\"")), cur.Substring(cur.IndexOf("\" delegate=\"") + 12, cur.Substring(cur.IndexOf("\" delegate=\"") + 12).IndexOf("\"")), "" , cur.Substring(cur.IndexOf("\" maxRunCount=\"") + 15, cur.Substring(cur.IndexOf("\" maxRunCount=\"") + 15).IndexOf("\"")));
                                }
                            }
                            else if (cur.Contains("<productionFrontier ") && cur.Contains("name=\""))
                            {
                                pfTName = cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\""));
                                pfts.Add(pfTName, new ProductionFrontier() { Name = pfTName });
                            }
                            else if (cur.Contains("<frontierRules ") && cur.Contains("name=\""))
                            {
                                pfts[pfTName].UnitsInFrontier.Add(new Unit() { Name = cur.Substring(cur.IndexOf("name=\"") + 9, cur.Substring(cur.IndexOf("name=\"") + 9).IndexOf("\"")) });
                            }
                            else if (cur.Contains("<playerProduction ") && cur.Contains("player=\""))
                            {
                                pfts[cur.Substring(cur.IndexOf(" frontier=\"") + 11, cur.Substring(cur.IndexOf(" frontier=\"") + 11).IndexOf("\""))].Name = cur.Substring(cur.IndexOf("player=\"") + 8, cur.Substring(cur.IndexOf("player=\"") + 8).IndexOf("\""));
                                AddProductionFrontier(pfts[cur.Substring(cur.IndexOf(" frontier=\"") + 11, cur.Substring(cur.IndexOf(" frontier=\"") + 11).IndexOf("\""))]);
                            }
                            else if (cur.Contains("<attatchment ") && cur.Contains("name=\"techAttatchment\""))
                            {
                                ttplayer = cur.Substring(cur.IndexOf(" attatchTo=\"") + 12, cur.Substring(cur.IndexOf(" attatchTo=\"") + 12).IndexOf("\""));
                            }
                            else if (cur.Contains("<option ") && cur.Contains("name=\"") && ttplayer.Length > 0)
                            {
                                techs.Add(new Technology() { Name = cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")), AlreadyEnabled = Convert.ToBoolean(cur.Substring(cur.IndexOf(" value=\"") + 8, cur.Substring(cur.IndexOf(" value=\"") + 8).IndexOf("\""))), player = new Player() { Name = ttplayer } });
                            }
                            else if (cur.Contains("</attatchment>") && ttplayer.Length > 0)
                            {
                                AddTechs(techs);
                                ttplayer = "";
                            }
                            else if (cur.Contains("<attatchment ") && cur.Contains("name=\"territoryAttatchment\""))
                            {
                                tTName = cur.Substring(cur.IndexOf(" attatchTo=\"") + 12, cur.Substring(cur.IndexOf(" attatchTo=\"") + 12).IndexOf("\""));
                            }
                            else if (cur.Contains("<option ") && cur.Contains("name=\"production\""))
                            {
                                Step2Info.territories[tTName].Production = Convert.ToInt32(cur.Substring(cur.IndexOf(" value=\"") + 8, cur.Substring(cur.IndexOf(" value=\"") + 8).IndexOf("\"")));
                            }
                            else if (cur.Contains("<option ") && cur.Contains("name=\"capital\""))
                            {
                                Step2Info.territories[tTName].IsCapitol = true; //                            TerritoryDefinitionsImageDrawer.Controls[cur.Substring(cur.IndexOf("<territory name=\"") + 17, cur.Substring(cur.IndexOf("<territory name=\"") + 17).IndexOf("\""))].BackColor = Color.DodgerBlue;
                                TerritoryDefinitionsImageDrawer.Controls[tTName].Tag = ((string)TerritoryDefinitionsImageDrawer.Controls[tTName].Tag) + "Capitol";
                                TerritoryDefinitionsImageDrawer.Controls[tTName].BackColor = Color.Violet;
                            }
                            else if (cur.Contains("<option ") && cur.Contains("name=\"isImpassible\"") && cur.Contains("value=\"true\""))
                            {
                                Step2Info.territories[tTName].IsImpassable = true;
                                TerritoryDefinitionsImageDrawer.Controls[tTName].Tag = ((string)TerritoryDefinitionsImageDrawer.Controls[tTName].Tag) + "Impassable";
                                if (TerritoryDefinitionsImageDrawer.Controls[tTName].BackColor != Color.Violet && TerritoryDefinitionsImageDrawer.Controls[tTName].BackColor != Color.Red)
                                    TerritoryDefinitionsImageDrawer.Controls[tTName].BackColor = Color.DarkGray;
                            }
                            else if (cur.Contains("<option ") && cur.Contains("name=\"victoryCity\"") && cur.Contains("value=\"true\""))
                            {
                                Step2Info.territories[tTName].IsVictoryCity = true;
                                TerritoryDefinitionsImageDrawer.Controls[tTName].Tag = ((string)TerritoryDefinitionsImageDrawer.Controls[tTName].Tag) + "VictoryCity";
                                if(TerritoryDefinitionsImageDrawer.Controls[tTName].BackColor != Color.Violet)
                                    TerritoryDefinitionsImageDrawer.Controls[tTName].BackColor = Color.Red;
                            }
                            else if (cur.Contains("</attatchment>") && tTName.Length > 0)
                            {
                                tTName = "";
                            }
                            else if (cur.Contains("<attatchment ") && cur.Contains("name=\"canalAttatchment"))
                            {
                                tTName2 = cur.Substring(cur.IndexOf(" attatchTo=\"") + 12, cur.Substring(cur.IndexOf(" attatchTo=\"") + 12).IndexOf("\""));
                            }
                            else if (cur.Contains("<option ") && cur.Contains("name=\"canalName\"") && cur.Contains("value=\""))
                            {
                                cn = cur.Substring(cur.IndexOf(" value=\"") + 8, cur.Substring(cur.IndexOf(" value=\"") + 8).IndexOf("\""));
                                try
                                {
                                    Step12Info.Canals[cn].CanalSeaNeighbors.Add(Step2Info.territories[tTName2]);
                                }
                                catch
                                {
                                    Canal c = new Canal() { Name = cn };
                                    c.CanalSeaNeighbors.Add(Step2Info.territories[tTName2]);
                                    Step12Info.Canals.Add(cn, c);
                                }
                            }
                            else if (cur.Contains("<option ") && cur.Contains("name=\"landTerritories\""))
                            {
                                int ni = 0;
                                try
                                {
                                    string tn = cur.Substring(cur.IndexOf(" value=\"") + 8, cur.Substring(cur.IndexOf(" value=\"") + 8).IndexOf(":"));
                                    Step12Info.Canals[cn].LandTerritories.Add(Step2Info.territories[tn]);
                                }
                                catch
                                {
                                    string tn = cur.Substring(cur.IndexOf(" value=\"") + 8, cur.Substring(cur.IndexOf(" value=\"") + 8).IndexOf("\""));
                                    Step12Info.Canals[cn].LandTerritories.Add(Step2Info.territories[tn]);
                                }
                                try
                                {
                                    if (cur.Substring(ni).Contains(":"))
                                    {
                                        Step12Info.Canals[cn].LandTerritories.Add(Step2Info.territories[cur.Substring(cur.IndexOf(":") + 1, cur.Substring(cur.IndexOf(":") + 1).IndexOf("\""))]);
                                        ni += cur.IndexOf(":") + 1;
                                    }
                                }
                                catch { }
                            }
                            else if (cur.Contains("</attatchment>") && tTName2.Trim().Length > 0)
                            {
                                tTName2 = "";
                            }
                            else if (cur.Contains("<territoryOwner ") && cur.Contains("territory=\""))
                            {
                                string tn = cur.Substring(cur.IndexOf("territory=\"") + 11, cur.Substring(cur.IndexOf("territory=\"") + 11).IndexOf("\""));
                                string on = cur.Substring(cur.IndexOf(" owner=\"") + 8, cur.Substring(cur.IndexOf(" owner=\"") + 8).IndexOf("\""));
                                Step2Info.territories[tn].Owner = new Player() { Name = on };
                            }
                            else if (cur.Contains("<unitPlacement ") && cur.Contains("unitType=\""))
                            {
                                string tn = cur.Substring(cur.IndexOf("territory=\"") + 11, cur.Substring(cur.IndexOf("territory=\"") + 11).IndexOf("\""));
                                string on = "";
                                if (cur.Contains(" owner=\""))
                                    on = cur.Substring(cur.IndexOf(" owner=\"") + 8, cur.Substring(cur.IndexOf(" owner=\"") + 8).IndexOf("\""));
                                string ut = cur.Substring(cur.IndexOf("unitType=\"") + 10, cur.Substring(cur.IndexOf("unitType=\"") + 10).IndexOf("\""));
                                string uq = cur.Substring(cur.IndexOf(" quantity=\"") + 11, cur.Substring(cur.IndexOf(" quantity=\"") + 11).IndexOf("\""));
                                int addAmount = Convert.ToInt32(uq);
                                for (int i = 0; i < addAmount; i++)
                                {
                                    if (on.Trim().Length > 0)
                                        Step2Info.territories[tn].Units.Add(new Unit() { Name = ut, unitOwner = new Player() { Name = on } });
                                    else
                                        Step2Info.territories[tn].Units.Add(new Unit() { Name = ut });
                                }
                            }
                            else if (cur.Contains("<property ") && !(cur.Contains("name=\"notes\" value=\"")/* || cur.Contains("name=\"mapName\" value=\"")*/))
                            {
                                sta = new Setting() { Name = cur.Substring(cur.IndexOf("name=\"") + 6, cur.Substring(cur.IndexOf("name=\"") + 6).IndexOf("\"")), Value = cur.Substring(cur.IndexOf(" value=\"") + 8, cur.Substring(cur.IndexOf(" value=\"") + 8).IndexOf("\"")) };
                                if (cur.Contains(" editable=\""))
                                    sta.Editable = Convert.ToBoolean(cur.Substring(cur.IndexOf(" editable=\"") + 11, cur.Substring(cur.IndexOf(" editable=\"") + 11).IndexOf("\"")));
                                if (cur.Contains("/>"))
                                    AddInGameSetting(sta);
                            }
                            else if (cur.Contains("<number min=\""))
                            {
                                sta.IntMin = Convert.ToInt32(cur.Substring(cur.IndexOf("min=\"") + 5, cur.Substring(cur.IndexOf("min=\"") + 5).IndexOf("\"")));
                                sta.IntMax = Convert.ToInt32(cur.Substring(cur.IndexOf("max=\"") + 5, cur.Substring(cur.IndexOf("max=\"") + 5).IndexOf("\"")));
                            }
                            else if (cur.Contains("</property>") && !(cur.Contains("name=\"notes\" value=\"")/* || cur.Contains("name=\"mapName\" value=\"")*/))
                            {
                                AddInGameSetting(sta);
                            }
                            else if (cur.Contains("name=\"notes\" value=\""))
                            {
                                mapNotesTextBox.Text = cur.Substring(cur.IndexOf(" value=\"") + 8, cur.Substring(cur.IndexOf(" value=\"") + 8).IndexOf("\""));
                            }
                        }
                        catch (Exception ex) { errorOccured = true; thrownException = ex; doBreak = true; }
                        if (doBreak)
                        {
                            if(Settings.StopLoadingXMLWhenErrorFound)
                                break;
                        }
                    }
                    Step1Info.LoadedFile = d4.FileName;
                }
            }
            catch(Exception ex)
            {
                if (MessageBox.Show(this, "An error occured when trying to setup the program for the Xml file. Make sure that all the map files, other than the xml file,(The map's centers.txt, polygons.txt, map.properties, etc.) have no errors in them.\r\n\r\nDo you want to view the error message?", "Error Reading Map Files", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    throw ex;
            }
            if (errorOccured)
            {
                if (MessageBox.Show(this, "An error occured when trying to load the Xml file. Make sure the xml file has no errors.\r\n\r\nText that failed to load: " + textThatFailedParsing + "\r\nLocation of failed text in file: " + indexOfTextThatFailed + "\r\nGuessed Line Number: " + guessedLineNumberThatFailed + "\r\n\r\nDo you want to view the error message?", "Error Loading Xml File", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    throw thrownException;
            }
            Stop();
        }
        private void ClearAllDataAndControls()
        {
            ClearAllControls();
            ClearAllData();
        }
        private void ClearAllData()
        {
            foreach (TabPage cur in tabControl1.TabPages)
            {
                cur.VerticalScroll.Value = 0;
            }

            Step1Info.CentersLocation = "";
            Step1Info.LoadedFile = "";
            Step1Info.MapImage = new Bitmap(1, 1);
            Step1Info.MapImageLocation = "";
            Step1Info.MapImageWL = new Bitmap(1, 1);
            Step1Info.MapName = "";
            Step1Info.MapVersion = "";
            Step1Info.ResourceName = "";
            Step1Info.WaterTerritoryFilter = "";
                Step2Info.territories.Clear();
            Step3Info.connections.Clear();
            Step4Info.players.Clear();
            Step5Info.units.Clear();
            Step6Info.gameplaySequences.Clear();
            Step7Info.playerSequences.Clear();
            Step8Info.technologies.Clear();
            Step9Info.ProductionFrontiers.Clear();
            Step12Info.Canals.Clear();
            Step15Info.Settings.Clear();
            Step16Info.Notes = "";
            tplayers.Clear();
            tunits.Clear();
            pfts.Clear();
            Step12Info.Canals.Clear();
            territoryChange = true;
            infochange1 = true;
            infochange2 = true;
            infochange3 = true;
            opened1 = false;
            opened2 = false;
            opened3 = false;
            opened4 = false;
            opened5 = false;
        }
        private void ClearAllControls()
        {
            ClearPlayers();
            ClearUnits();
            ClearGameplayDelegates();
            ClearPlayerDelegates();
            ClearProductionFrontiers();
            ClearTechs();
            ClearUnitAttachments();
            ClearInGameSettings();
        }
        public void Stop()
        {
            t2.Enabled = false;
            panel2.Enabled = true;
            button15.Enabled = true;
            button16.Enabled = true;
            tabControl1.Enabled = true;
            this.Text = "TripleA Map Creator - Part 2";
        }
        Setting sta;
        string cn = "";
        string tTName = "";
        string tTName2 = "";
        string pfTName = "";
        string ttplayer = "";
        string unitTName = "";
        string unitTName2 = "";
        List<Technology> techs = new List<Technology>();
        Dictionary<string, Player> tplayers = new Dictionary<string, Player>();
        Dictionary<string, Unit> tunits = new Dictionary<string, Unit>();
        Dictionary<string, ProductionFrontier> pfts = new Dictionary<string, ProductionFrontier>();
        public void ClearPlayers()
        {
            List<Control> toDelete = new List<Control>();
            int count = 0;
            change = -25;
            panel5.Size = new Size(5, 11);
            button6.Location = new Point(55, 78);
            button7.Location = new Point(458, 51);
            foreach (Control cur in tabPage4.Controls)
            {
                if (cur is TextBox)
                {
                    toDelete.Add(cur);
                }
             }
            for (int i = 0; i < toDelete.Count; i++)
            {
                tabPage4.Controls.Remove(toDelete[i]);
            }
        }
        public void ClearUnits()
        {
            List<Control> toDelete = new List<Control>();
            change2 = -25;
            panel6.Size = new Size(5, 11);
            button9.Location = new Point(456, 27);
            button10.Location = new Point(102, 57);
            foreach (Control cur in tabPage5.Controls)
            {
                if (cur is TextBox)
                {
                    toDelete.Add(cur);
                }
            }
            for (int i = 0; i < toDelete.Count; i++)
            {
                tabPage5.Controls.Remove(toDelete[i]);
            }
        }
        public void ClearGameplayDelegates()
        {
            List<Control> toDelete = new List<Control>();
            change3 = -25;
            panel7.Size = new Size(5, 11);
            button11.Location = new Point(488, 40);
            button12.Location = new Point(39, 68);
            foreach (Control cur in tabPage6.Controls)
            {
                if (cur is TextBox)
                {
                    toDelete.Add(cur);
                }
            }
            for (int i = 0; i < toDelete.Count; i++)
            {
                tabPage6.Controls.Remove(toDelete[i]);
            }
        }
        public void ClearPlayerDelegates()
        {
            List<Control> toDelete = new List<Control>();
            change4 = -25;
            panel8.Size = new Size(5, 11);
            button13.Location = new Point(485, 40);
            button14.Location = new Point(18, 68);
            foreach (Control cur in tabPage7.Controls)
            {
                if (cur is TextBox || cur is ComboBox)
                {
                    toDelete.Add(cur);
                }
            }
            for (int i = 0; i < toDelete.Count; i++)
            {
                tabPage7.Controls.Remove(toDelete[i]);
            }
        }
        public void ClearProductionFrontiers()
        {
            try
            {
                tabControl2.TabPages.Clear();
            }
            catch { }
        }
        public void ClearTechs()
        {
            List<Control> toDelete = new List<Control>();
            change5 = -25;
            panel9.Size = new Size(5, 11);
            button17.Location = new Point(423, 41);
            button18.Location = new Point(62, 68);
            foreach (Control cur in tabPage8.Controls)
            {
                if (cur is TextBox || cur is ComboBox)
                {
                    toDelete.Add(cur);
                }
            }
            for (int i = 0; i < toDelete.Count; i++)
            {
                tabPage8.Controls.Remove(toDelete[i]);
            }
        }
        public void ClearUnitAttachments()
        {
            try
            {
                tabControl3.TabPages.Clear();
            }
            catch { }
        }
        public void ClearInGameSettings()
        {
            List<Control> toDelete = new List<Control>();
            change6 = -25;
            panel15.Size = new Size(5, 11);
            panel16.Size = new Size(5, 11);
            button23.Location = new Point(494, 52 - 25);
            button24.Location = new Point(25, 80 - 25);
            foreach (Control cur in tabPage15.Controls)
            {
                if (cur is TextBox || cur is ComboBox)
                {
                    toDelete.Add(cur);
                }
            }
            for (int i = 0; i < toDelete.Count; i++)
            {
                tabPage15.Controls.Remove(toDelete[i]);
            }
        }
        TabPage baseTP = new TabPage();
        public void AddPlayer(string playerName,string playerAlliance,string playerResources)
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            infochange1 = true;
            change += 25;
            panel5.Size += new Size(0, 25);
            button6.Location += new Size(0, 25);
            button7.Location += new Size(0, 25);
            if (playerAlliance.Trim().Length == 0)
                playerAlliance = playerName;
            tabPage4.Controls.AddRange(new Control[] { new TextBox() { Text = playerName, Size = textBox5.Size, Location = new Point(textBox5.Location.X, textBox5.Location.Y + change) }, new TextBox() { Text = playerAlliance,Size = textBox6.Size, Location = new Point(textBox6.Location.X, textBox6.Location.Y + change) }, new TextBox() { Text = playerResources, Size = textBox24.Size, Location = new Point(textBox24.Location.X, textBox24.Location.Y + change) } });
        }
        public void AddUnit(string unitName, string unitCost, string unitBuyQuantity)
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            change2 += 25;
            panel6.Size += new Size(0, 25);
            button9.Location += new Size(0, 25);
            button10.Location += new Size(0, 25);
            tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = unitName, Size = textBox9.Size, Location = new Point(textBox9.Location.X, textBox9.Location.Y + change2) }, new TextBox() { Text = unitCost, Size = textBox10.Size, Location = new Point(textBox10.Location.X, textBox10.Location.Y + change2) }, new TextBox() { Text = unitBuyQuantity, Size = textBox11.Size, Location = new Point(textBox11.Location.X, textBox11.Location.Y + change2) } });
        }
        public void AddGameplayDelegate(string delName, string delClass, string delDisplay)
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            change3 += 25;
            panel7.Size += new Size(0, 25);
            button11.Location += new Size(0, 25);
            button12.Location += new Size(0, 25);
            tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = delName, Size = textBox12.Size, Location = new Point(textBox12.Location.X, textBox12.Location.Y + change3) }, new TextBox() { Text = delClass, Size = textBox14.Size, Location = new Point(textBox14.Location.X, textBox14.Location.Y + change3) }, new TextBox() { Text = delDisplay, Size = textBox13.Size, Location = new Point(textBox13.Location.X, textBox13.Location.Y + change3) } });
        }
        public void AddPlayerDelegate(string delName, string delDelegate,string player, string delMaxRun)
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            change4 += 25;
            panel8.Size += new Size(0, 25);
            button13.Location += new Size(0, 25);
            button14.Location += new Size(0, 25);
            ComboBox cb1 = new ComboBox() { Text = delDelegate, Size = comboBox16.Size, Location = new Point(comboBox16.Location.X, comboBox16.Location.Y + change4)};
            cb1.Items.AddRange(getGameplayDelegates());
            tabPage7.Controls.AddRange(new Control[] { new TextBox() { Text = delName, Size = textBox15.Size, Location = new Point(textBox15.Location.X, textBox15.Location.Y + change4) }, cb1, new ComboBox() { Text = player, Size = comboBox17.Size, Location = new Point(comboBox17.Location.X, comboBox17.Location.Y + change4) }, new TextBox() { Text = delMaxRun, Size = textBox18.Size, Location = new Point(textBox18.Location.X, textBox18.Location.Y + change4) } });
        }
        public object[] getGameplayDelegates()
        {
            List<object> result = new List<object>();
            foreach (GameplaySequence cur in Step6Info.gameplaySequences.Values)
            {
                result.Add(cur);
            }
            return result.ToArray();
        }
        public void AddProductionFrontier(ProductionFrontier frontierToAdd)
        {
            infochange2 = false;
            TabPage cur = new TabPage(frontierToAdd.Name);
            cur.Text = frontierToAdd.Name;
            cur.AutoScroll = true;
            cur.VerticalScroll.Value = 0;
            cur.HorizontalScroll.Value = 0;
            Button b1 = new Button() { Text = "Remove", Size = new Size(71, 23), Location = new Point(189, 57) };
            Button b2 = new Button() { Text = "Add Unit...", Size = new Size(82, 23), Location = new Point(39, 87) };
            b1.Click+=new EventHandler(c4_Click);
            b2.Click += new EventHandler(c5_Click);
            cur.Controls.AddRange(new Control[] { b1, b2, new Label() { Text = label59.Text, Location = label59.Location, Size = label59.Size, Font = label59.Font }, new Label() { Text = label27.Text, Location = label27.Location, Size = label27.Size, Font = label27.Font } });
            int ch = 0;
            foreach (Unit cur2 in frontierToAdd.UnitsInFrontier)
            {
                ComboBox cb1 = new ComboBox() { Text = cur2.Name, Location = comboBox3.Location + new Size(0, ch), Size = comboBox3.Size };
                foreach (Unit cur3 in frontierToAdd.UnitsInFrontier)
                {
                    cb1.Items.Add(cur3);
                }
                cur.Controls.Add(cb1);
                ch += 25;
            }
            cur.Tag = ch - 25;
            try
            {
                b1.Location = new Point(189, (57 + ch) - 25);
                b2.Location = new Point(39, (87 + ch) - 25);
            }
            catch { }
            tabControl2.TabPages.Add(cur);
        }
        public void AddTechs(List<Technology> techsTA)
        {
            tabPage8.VerticalScroll.Value = 0;
            tabPage8.HorizontalScroll.Value = 0;
            List<Control> toDelete = new List<Control>();
            foreach (Control cur in tabPage8.Controls)
            {
                if (cur is TextBox || cur is ComboBox)
                {
                    toDelete.Add(cur);
                }
            }
            for (int i = 0; i < toDelete.Count; i++)
            {
                tabPage8.Controls.Remove(toDelete[i]);
            }
            int ch = 0;
            foreach (Technology cur in techsTA)
            {
                ComboBox cb1 = new ComboBox() { Text = cur.player.Name, Location = comboBox2.Location + new Size(0, ch), Size = comboBox2.Size };
                foreach (Player cur2 in Step4Info.players.Values)
                    cb1.Items.Add(cur2);
                ComboBox cb2 = new ComboBox() { Text = cur.AlreadyEnabled.ToString(), Location = comboBox1.Location + new Size(0, ch), Size = comboBox1.Size };
                cb2.Items.AddRange(getTrueFalseItems());
                tabPage8.Controls.AddRange(new Control[] { new TextBox() { Text = cur.Name, Location = textBox16.Location + new Size(0, ch), Size = textBox16.Size }, cb1, cb2 });
                ch += 25;
            }
            change5 = ch - 25;
            button17.Location = new Point(423, (66 + ch) - 25);
            button18.Location = new Point(62, (93 + ch) - 25);
            panel9.Size = new Size(5, (36 + ch) - 25);
        }
        public object[] getTrueFalseItems()
        {
            return new List<string>() { "True", "False" }.ToArray();
        }
        public void AddUnitAttachment(Unit unitWA)
        {
            infochange3 = false;
            TabPage cur = new TabPage(unitWA.Name);
            cur.Text = unitWA.Name;
            cur.AutoScroll = true;
            cur.VerticalScroll.Value = 0;
            cur.HorizontalScroll.Value = 0;
            Button b1 = new Button() { Text = "Remove", Size = new Size(71, 23), Location = new Point(294, 57) };
            Button b2 = new Button() { Text = "Add Attachment...", Size = new Size(116, 23), Location = new Point(39, 87) };
            b1.Click += new EventHandler(c4_Click2);
            b2.Click += new EventHandler(c5_Click2);
            cur.Controls.AddRange(new Control[] { b1, b2, new Label() { Text = label110.Text, Location = label110.Location, Size = label110.Size, Font = label110.Font }, new Label() { Text = label36.Text, Location = label36.Location, Size = label36.Size, Font = label36.Font }, new Label() { Text = label65.Text, Location = label65.Location, Size = label65.Size, Font = label65.Font } });
            int ch = 0;
            foreach (UnitOption cur2 in unitWA.attachment.options)
            {
                cur.Controls.AddRange(new Control[] { new TextBox() { Text = cur2.Name, Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = cur2.Value.ToString(), Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                ch += 25;
            }
            cur.Tag = ch - 25;
            try
            {
                b1.Location = new Point(294, (57 + ch) - 25);
                b2.Location = new Point(39, (87 + ch) - 25);
            }
            catch { }
            tabControl3.TabPages.Add(cur);
        }
        public void AddInGameSetting(Setting sta)
        {
            Form1.CheckForIllegalCrossThreadCalls = false;
            change6 += 25;
            panel15.Size += new Size(0, 25);
            panel16.Size += new Size(0, 25);
            button23.Location += new Size(0, 25);
            button24.Location += new Size(0, 25);
            ComboBox cb1 = new ComboBox() { Text = sta.Editable.ToString(), Size = comboBox5.Size, Location = new Point(comboBox5.Location.X, comboBox5.Location.Y + change6) };
            cb1.Items.AddRange(getTrueFalseItems());
            tabPage15.Controls.AddRange(new Control[] { new TextBox() { Text = sta.Name, Size = textBox19.Size, Location = new Point(textBox19.Location.X, textBox19.Location.Y + change6) }, new TextBox() { Text = sta.Value.ToString(), Size = textBox21.Size, Location = new Point(textBox21.Location.X, textBox21.Location.Y + change6) }, cb1, new TextBox() { Text = sta.IntMin.ToString(), Size = textBox20.Size, Location = new Point(textBox20.Location.X, textBox20.Location.Y + change6) }, new TextBox() { Text = sta.IntMax.ToString(), Size = textBox22.Size, Location = new Point(textBox22.Location.X, textBox22.Location.Y + change6) } });
        }
        private void button16_Click(object sender, EventArgs e)
        {
            force = true;
            oldStepIndex = stepIndex;
            surpress2 = true;
            tabControl1_Selecting(new object(), new TabControlCancelEventArgs(new TabPage(), 0, false, TabControlAction.Selecting));
            surpress2 = false;
            Save();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            Load2();
        }
        [Serializable]
        public class SavePackage
        {
            public Dictionary<string, SConnection> connections = new Dictionary<string, SConnection>();
        }
        int change5 = 0;
        private void button17_Click(object sender, EventArgs e)
        {
            if (change5 > -1)
            {
                if (!(change5 > 0))
                {
                    List<Control> toDelete = new List<Control>();
                    foreach (Control cur in tabPage8.Controls)
                    {
                        if (cur is TextBox || cur is ComboBox)
                        {
                            toDelete.Add(cur);
                        }
                    }
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        tabPage8.Controls.Remove(toDelete[i]);
                    }
                }
                else
                {
                    tabPage8.Controls.RemoveAt(tabPage8.Controls.Count - 1);
                    tabPage8.Controls.RemoveAt(tabPage8.Controls.Count - 1);
                    tabPage8.Controls.RemoveAt(tabPage8.Controls.Count - 1);
                }
                change5 -= 25;
                panel9.Size += new Size(0, -25);
                button17.Location += new Size(0, -25);
                button18.Location += new Size(0, -25);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            change5 += 25;
            panel9.Size += new Size(0, 25);
            button17.Location += new Size(0, 25);
            button18.Location += new Size(0, 25);
            ComboBox cb1 = new ComboBox() { Size = comboBox2.Size, Location = new Point(comboBox2.Location.X, comboBox2.Location.Y + change5 - tabPage8.VerticalScroll.Value) };
            ComboBox cb2 = new ComboBox() { Size = comboBox1.Size, Location = new Point(comboBox1.Location.X, comboBox1.Location.Y + change5 - tabPage8.VerticalScroll.Value) };
            cb1.Items.AddRange(getPlayers());
            cb2.Items.AddRange(getTrueFalseItems());
            tabPage8.Controls.AddRange(new Control[] { new TextBox() { Size = textBox16.Size, Location = new Point(textBox16.Location.X, textBox16.Location.Y + change5 - tabPage8.VerticalScroll.Value) }, cb1, cb2 });
        }
        public object[] getPlayers()
        {
            List<object> result = new List<object>();
            foreach (Player cur in Step4Info.players.Values)
            {
                result.Add(cur.Name);
            }
            return result.ToArray();
        }
        public object[] getPlayersWithNeutral()
        {
            List<object> result = new List<object>();
            foreach (Player cur in Step4Info.players.Values)
            {
                result.Add(cur.Name);
            }
            result.Add("Neutral");
            return result.ToArray();
        }
        int change6 = 0;
        private void button23_Click(object sender, EventArgs e)
        {
            if (change6 > -1)
            {
                tabPage15.Controls.RemoveAt(tabPage15.Controls.Count - 1);
                tabPage15.Controls.RemoveAt(tabPage15.Controls.Count - 1);
                tabPage15.Controls.RemoveAt(tabPage15.Controls.Count - 1);
                tabPage15.Controls.RemoveAt(tabPage15.Controls.Count - 1);
                tabPage15.Controls.RemoveAt(tabPage15.Controls.Count - 1);
                change6 -= 25;
                panel16.Size += new Size(0, -25);
                panel15.Size += new Size(0, -25);
                button23.Location += new Size(0, -25);
                button24.Location += new Size(0, -25);
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            change6 += 25;
            panel16.Size += new Size(0, 25);
            panel15.Size += new Size(0, 25);
            button23.Location += new Size(0, 25);
            button24.Location += new Size(0, 25);
            ComboBox cb1 = new ComboBox() { Size = comboBox5.Size, Location = new Point(comboBox5.Location.X, comboBox5.Location.Y + change6) };
            cb1.Items.AddRange(getTrueFalseItems());
            tabPage15.Controls.AddRange(new Control[] { new TextBox() { Size = textBox19.Size, Location = new Point(textBox19.Location.X, textBox19.Location.Y + change6 - tabPage15.VerticalScroll.Value) }, new TextBox() { Size = textBox21.Size, Location = new Point(textBox21.Location.X, textBox21.Location.Y + change6 - tabPage15.VerticalScroll.Value) }, cb1, new TextBox() { Size = textBox20.Size, Location = new Point(textBox20.Location.X, textBox20.Location.Y + change6 - tabPage15.VerticalScroll.Value), Text = "0" }, new TextBox() { Size = textBox22.Size, Location = new Point(textBox22.Location.X, textBox22.Location.Y + change6 - tabPage15.VerticalScroll.Value) } });
        }

        private void button26_Click(object sender, EventArgs e)
        {
            force = true;
            surpress2 = true;
            tabControl1_Selecting(new object(), new TabControlCancelEventArgs(new TabPage(), 0, false, TabControlAction.Selecting));
            surpress2 = false;
            Save();
        }

        public void Write(string fileLoc)
        {
            try
            {
                List<String> lines = new List<String>();
                lines.Add("<?xml version=\"1.0\" ?>");
                lines.Add("<!DOCTYPE game SYSTEM \"game.dtd\">");
                lines.Add("<game>");
                lines.Add("        <info name=\"" + Step1Info.MapName + "\" version=\"" + Step1Info.MapVersion + "\"/>");
                lines.Add("        <loader javaClass=\"games.strategy.triplea.TripleA\"/>");
                lines.Add("        <map>");
                lines.Add("                <!-- Territory Definitions -->");
                foreach (Player player in Step4Info.players.Values)
                {
                    foreach (Territory cur in Step2Info.territories.Values)
                    {
                        if (cur.Owner.Name == player.Name)
                        {
                            lines.Add("                <territory name=\"" + cur.Name + "\""
                                + (cur.IsWater ? " water=\"true\"" : "") + "/>");
                        }
                    }
                }
                foreach (Territory cur in Step2Info.territories.Values)
                {
                    if (cur.Owner.Name.ToLower().Trim() == "neutral" || cur.Owner.Name.ToLower().Trim().Length == 0)
                    {
                        lines.Add("                <territory name=\"" + cur.Name + "\""
                            + (cur.IsWater ? " water=\"true\"" : "") + "/>");
                    }
                }
                lines.Add("");
                lines.Add("                <!-- Territory Connections -->");
                foreach (Player player in Step4Info.players.Values)
                {
                    foreach (Territory terr in Step2Info.territories.Values)
                    {
                        if (terr.Owner.Name == player.Name)
                        {
                            foreach (Connection cur in Step3Info.connections.Values)
                            {
                                if (cur.t1.Name == terr.Name)
                                {
                                    if (cur.t1.Name != cur.t2.Name)
                                        lines.Add("                <connection t1=\"" + cur.t1.Name + "\" t2=\"" + cur.t2.Name + "\"/>");
                                }
                            }
                        }
                    }
                }
                foreach (Territory terr in Step2Info.territories.Values)
                {
                    if (terr.Owner.Name.ToLower().Trim() == "neutral" || terr.Owner.Name.ToLower().Trim().Length == 0)
                    {
                        foreach (Connection cur in Step3Info.connections.Values)
                        {
                            if (cur.t1.Name == terr.Name)
                            {
                                if (cur.t1.Name != cur.t2.Name)
                                    lines.Add("                <connection t1=\"" + cur.t1.Name + "\" t2=\"" + cur.t2.Name + "\"/>");
                            }
                        }
                    }
                }
                lines.Add("        </map>");
                lines.Add("        <resourceList>");
                lines.Add("                <resource name=\"" + Step1Info.ResourceName + "\"/>");
                lines.Add("        </resourceList>");
                lines.Add("        <playerList>");
                lines.Add("                <!-- In turn order -->");
                foreach (Player cur in Step4Info.players.Values)
                {
                    lines.Add("                <player name=\"" + cur.Name.Replace(" ","") + "\" optional=\"" + cur.Optional.ToString().ToLower() + "\"/>");
                }
                lines.Add("");
                foreach (Player cur in Step4Info.players.Values)
                {
                    lines.Add("                <alliance player=\"" + cur.Name.Replace(" ","") + "\" alliance=\"" + cur.Alliance + "\"/>");
                }
                lines.Add("        </playerList>");
                lines.Add("        <unitList>");
                foreach (Unit cur in Step5Info.units.Values)
                {
                    lines.Add("                <unit name=\"" + cur.Name + "\"/>");
                }
                lines.Add("        </unitList>");
                lines.Add("        <gamePlay>");
                lines.Add("                <delegate name=\"initDelegate\" javaClass=\"games.strategy.triplea.delegate.InitializationDelegate\" display=\"Initializing Delegates\"/>");
                foreach (GameplaySequence cur in Step6Info.gameplaySequences.Values)
                {
                    if(!cur.Name.Contains("initDelegate") && !cur.Name.Contains("endRound"))
                    lines.Add("                <delegate name=\"" + cur.Name + "\" javaClass=\"games.strategy.triplea.delegate." + cur.Class + "\" display=\"" + cur.Display + "\"/>");
                }
                lines.Add("                <delegate name=\"endRound\" javaClass=\"games.strategy.triplea.delegate.EndRoundDelegate\" display=\"Round Complete\"/>");
                lines.Add("                <sequence>");
                lines.Add("                        <step name=\"gameInitDelegate\" delegate=\"initDelegate\" maxRunCount=\"1\"/>");
                foreach (PlayerSequence cur in Step7Info.playerSequences.Values)
                {
                    lines.Add("                        <step name=\"" + cur.Name + "\" delegate=\"" + cur.Delegate.Name + "\" player=\"" + cur.player.Name.Replace(" ", "") + (cur.MaxRunCount > 0 ? "\" maxRunCount=\"" + cur.MaxRunCount : "\" display=\"Non Combat Move") + "\"/>");
                }
                lines.Add("                        <step name=\"endRoundStep\" delegate=\"endRound\"/>");
                lines.Add("                </sequence>");
                lines.Add("        </gamePlay>");
                lines.Add("        <production>");
                lines.Add("                <!-- Unit Production Cost -->");
                foreach (Unit cur in Step5Info.units.Values)
                {
                    lines.Add("                <productionRule name=\"buy" + cur.Name + "\">");
                    lines.Add("                        <cost resource=\"" + Step1Info.ResourceName + "\" quantity=\"" + cur.cost.cost + "\"/>");
                    lines.Add("                        <result resourceOrUnit=\"" + cur.Name + "\" quantity=\"" + cur.cost.result.BuyQuantity + "\"/>");
                    lines.Add("                </productionRule>");
                }
                foreach (ProductionFrontier cur in Step9Info.ProductionFrontiers.Values)
                {
                    lines.Add("                <productionFrontier name=\"" + cur.Name + "Frontier\">");
                    foreach (Unit cur2 in cur.UnitsInFrontier)
                    {
                        lines.Add("                        <frontierRules name=\"buy" + cur2.Name + "\"/>");
                    }
                    lines.Add("                </productionFrontier>");
                }
                foreach (ProductionFrontier cur in Step9Info.ProductionFrontiers.Values)
                {
                    lines.Add("                <playerProduction player=\"" + cur.Name + "\" frontier=\"" + cur.Name + "Frontier\"/>");
                }
                lines.Add("        </production>");
                lines.Add("        <attatchmentList>");
                foreach (Technology cur in Step8Info.technologies.Values)
                {
                    if (!used.Contains(cur))
                    {
                        lines.Add("                    <attatchment name=\"techAttatchment\" attatchTo=\"" + cur.player.Name.Replace(" ","") + "\" javaClass=\"games.strategy.triplea.attatchments.TechAttachment\" type=\"player\">");
                        foreach (Technology cur2 in getResultsTM(Step8Info.technologies, cur.player.Name))
                        {
                            lines.Add("                           <option name=\"" + cur2.Name + "\" value=\"" + cur.AlreadyEnabled + "\"/>");
                        }
                        lines.Add("                    </attatchment>");
                    }
                }
                foreach (Unit cur in Step5Info.units.Values)
                {
                    lines.Add("                    <attatchment name=\"unitAttatchment\" attatchTo=\"" + cur.Name + "\" javaClass=\"games.strategy.triplea.attatchments.UnitAttachment\" type=\"unitType\">");
                    foreach (UnitOption cur2 in cur.attachment.options)
                    {
                        lines.Add("                         <option name=\"" + cur2.Name + "\" value=\"" + cur2.Value + "\"/>");
                    }
                    lines.Add("                    </attatchment>");
                }
                foreach (Player player in Step4Info.players.Values)
                {
                    foreach (Territory cur in Step2Info.territories.Values)
                    {
                        if (cur.Owner.Name == player.Name)
                        {
                            if ((cur.Production > 0 || cur.IsWater == false || cur.IsCapitol || cur.IsVictoryCity || cur.IsImpassable))
                            {
                                lines.Add("                    <attatchment name=\"territoryAttatchment\" attatchTo=\"" + cur.Name + "\" javaClass=\"games.strategy.triplea.attatchments.TerritoryAttachment\" type=\"territory\">");
                                if (cur.Production > 0 || cur.IsWater == false)
                                    lines.Add("                        <option name=\"production\" value=\"" + cur.Production + "\"/>");
                                if (cur.IsCapitol)
                                    lines.Add("                        <option name=\"capital\" value=\"" + cur.Owner.Name.Replace(" ", "") + "\"/>");
                                if (cur.IsVictoryCity)
                                    lines.Add("                        <option name=\"victoryCity\" value=\"true\"/>");
                                if (cur.IsImpassable)
                                    lines.Add("                        <option name=\"isImpassible\" value=\"true\"/>");
                                lines.Add("                   </attatchment>");
                            }
                        }
                    }
                }
                foreach (Territory cur in Step2Info.territories.Values)
                {
                    if (cur.Owner.Name.ToLower().Trim() == "neutral" || cur.Owner.Name.ToLower().Trim().Length == 0)
                    {
                        if ((cur.Production > 0 || cur.IsWater == false || cur.IsCapitol || cur.IsVictoryCity || cur.IsImpassable))
                        {
                            lines.Add("                    <attatchment name=\"territoryAttatchment\" attatchTo=\"" + cur.Name + "\" javaClass=\"games.strategy.triplea.attatchments.TerritoryAttachment\" type=\"territory\">");
                            if (cur.Production > 0 || cur.IsWater == false)
                                lines.Add("                        <option name=\"production\" value=\"" + cur.Production + "\"/>");
                            if (cur.IsCapitol)
                                lines.Add("                        <option name=\"capital\" value=\"" + cur.Owner.Name.Replace(" ", "") + "\"/>");
                            if (cur.IsVictoryCity)
                                lines.Add("                        <option name=\"victoryCity\" value=\"true\"/>");
                            if (cur.IsImpassable)
                                lines.Add("                        <option name=\"isImpassible\" value=\"true\"/>");
                            lines.Add("                   </attatchment>");
                        }
                    }
                }
                try
                {
                    int index = 0;
                    foreach (Canal cur in Step12Info.Canals.Values)
                    {
                        index++;
                        lines.Add("				    <attatchment name=\"canalAttatchment" + index + "\" attatchTo=\"" + cur.CanalSeaNeighbors[0].Name + "\" javaClass=\"games.strategy.triplea.attatchments.CanalAttachment\" type=\"territory\">");
                        lines.Add("				    	<option name=\"canalName\" value=\"canal" + index + "\"/>");
                        string lt = "";
                        lt = cur.LandTerritories[0].Name;
                        if (cur.LandTerritories.Count > 1)
                        {
                            foreach (Territory cur2 in cur.LandTerritories)
                            {
                                if (cur2 != cur.LandTerritories[0])
                                {
                                    lt += ":" + cur2.Name;
                                }
                            }
                        }
                        lines.Add("				    	<option name=\"landTerritories\" value=\"" + lt + "\"/>");
                        lines.Add("				    </attatchment>");

                        lines.Add("				    <attatchment name=\"canalAttatchment" + index + "\" attatchTo=\"" + cur.CanalSeaNeighbors[1].Name + "\" javaClass=\"games.strategy.triplea.attatchments.CanalAttachment\" type=\"territory\">");
                        lines.Add("				    	<option name=\"canalName\" value=\"canal" + index + "\"/>");
                        lines.Add("				    	<option name=\"landTerritories\" value=\"" + lt + "\"/>");
                        lines.Add("				    </attatchment>");
                    }
                }
                catch { MessageBox.Show("Error writing canals. Please go back and make sure all the canals have at least one land territory connected to it."); return; }
                lines.Add("        </attatchmentList>");
                lines.Add("        <initialize>");
                lines.Add("                <ownerInitialize>");
                foreach (Player player in Step4Info.players.Values)
                {
                    foreach (Territory cur in Step2Info.territories.Values)
                    {
                        if (cur.Owner.Name == player.Name)
                        {
                            if (cur.Owner.Name.Length > 0)
                                lines.Add("                        <territoryOwner territory=\"" + cur.Name + "\" owner=\"" + cur.Owner.Name.Replace(" ", "") + "\"/>");
                        }
                    }
                }
                foreach (Territory cur in Step2Info.territories.Values)
                {
                    if (cur.Owner.Name.ToLower().Trim() == "neutral" || cur.Owner.Name.ToLower().Trim().Length == 0)
                    {
                        if (cur.Owner.Name.Length > 0)
                            lines.Add("                        <territoryOwner territory=\"" + cur.Name + "\" owner=\"" + cur.Owner.Name.Replace(" ", "") + "\"/>");
                    }
                }
                lines.Add("                </ownerInitialize>");
                lines.Add("                <unitInitialize>");
                foreach (Player player in Step4Info.players.Values)
                {
                    foreach (Territory cur in Step2Info.territories.Values)
                    {
                        if (cur.Owner.Name == player.Name)
                        {
                            foreach (Unit cur2 in cur.Units)
                            {
                                if (cur2.unitOwner.Name.Trim().Length > 0)
                                    lines.Add("                        <unitPlacement unitType=\"" + cur2.Name + "\" territory=\"" + cur.Name + "\" quantity=\"" + cur2.cost.result.BuyQuantity + "\" owner=\"" + cur2.unitOwner.Name.Replace(" ", "") + "\"/>");
                                else if (cur.Owner.Name.Length > 0)
                                    lines.Add("                        <unitPlacement unitType=\"" + cur2.Name + "\" territory=\"" + cur.Name + "\" quantity=\"" + cur2.cost.result.BuyQuantity + "\" owner=\"" + cur.Owner.Name.Replace(" ", "") + "\"/>");
                                else
                                    lines.Add("                        <unitPlacement unitType=\"" + cur2.Name + "\" territory=\"" + cur.Name + "\" quantity=\"" + cur2.cost.result.BuyQuantity + "\"/>");
                            }
                        }
                    }
                }
                foreach (Territory cur in Step2Info.territories.Values)
                {
                    if (cur.Owner.Name.ToLower().Trim() == "neutral" || cur.Owner.Name.ToLower().Trim().Length == 0)
                    {
                        foreach (Unit cur2 in cur.Units)
                        {
                            if (cur2.unitOwner.Name.Trim().Length > 0)
                                lines.Add("                        <unitPlacement unitType=\"" + cur2.Name + "\" territory=\"" + cur.Name + "\" quantity=\"" + cur2.cost.result.BuyQuantity + "\" owner=\"" + cur2.unitOwner.Name.Replace(" ", "") + "\"/>");
                            else if (cur.Owner.Name.Length > 0)
                                lines.Add("                        <unitPlacement unitType=\"" + cur2.Name + "\" territory=\"" + cur.Name + "\" quantity=\"" + cur2.cost.result.BuyQuantity + "\" owner=\"" + cur.Owner.Name.Replace(" ", "") + "\"/>");
                            else
                                lines.Add("                        <unitPlacement unitType=\"" + cur2.Name + "\" territory=\"" + cur.Name + "\" quantity=\"" + cur2.cost.result.BuyQuantity + "\"/>");
                        }
                    }
                }
                lines.Add("                </unitInitialize>");
                lines.Add("                <resourceInitialize>");
                foreach (Player cur in Step4Info.players.Values)
                {
                    lines.Add("                        <resourceGiven player=\"" + cur.Name.Replace(" ", "") + "\" resource=\"" + Step1Info.ResourceName + "\" quantity=\"" + cur.InitialResources + "\"/>");
                }
                lines.Add("                </resourceInitialize>");
                lines.Add("        </initialize>");
                lines.Add("        <propertyList>");
                bool foundMapName = false;
                foreach (Setting cur in Step15Info.Settings.Values)
                {
                    if (cur.IntMin != 0 || cur.IntMax != 0)
                    {
                        lines.Add("                <property name=\"" + cur.Name + "\" value=\"" + cur.Value.ToString() + "\" editable=\"" + cur.Editable.ToString().ToLower() + "\">");
                        lines.Add("                        <number min=\"" + cur.IntMin + "\" max=\"" + cur.IntMax + "\"/>");
                        lines.Add("                </property>");
                    }
                    else if (cur.Value.ToString().ToLower() == "false" || cur.Value.ToString().ToLower() == "true")
                    {
                        lines.Add("                <property name=\"" + cur.Name + "\" value=\"" + cur.Value.ToString().ToLower() + "\" editable=\"" + cur.Editable.ToString().ToLower() + "\">");
                        lines.Add("                        <boolean/>");
                        lines.Add("                </property>");
                    }
                    else
                    {
                        lines.Add("                <property name=\"" + cur.Name + "\" value=\"" + cur.Value.ToString() + "\" editable=\"" + cur.Editable.ToString().ToLower() + "\"/>");
                    }
                    if (cur.Name == "mapName")
                        foundMapName = true;
                }
                if(foundMapName == false)
                lines.Add("                <property name=\"mapName\" value=\"" + Step1Info.MapName + "\" editable=\"false\"/>");
                if(!mapNotesTextBox.Text.Equals("(Fill this with the notes for the map.)"))
                    lines.Add("                <property name=\"notes\" value=\"" + mapNotesTextBox.Text + "\"/>");
                lines.Add("        </propertyList>");
                lines.Add("</game>");
                File.WriteAllLines(fileLoc, lines.ToArray());
                MessageBox.Show("Succesfully wrote to XML map file.", "Success");
            }
            catch
            {
                MessageBox.Show("An error occured writing to the file. Please go back and make sure you entered everything properly.", "Saving Error");
            }
        }
        SaveFileDialog d = new SaveFileDialog();
        private void button25_Click(object sender, EventArgs e)
        {
            d.Filter = "Xml Files|*.xml|All files (*.*)|*.*";
            d.DefaultExt = ".xml";
            if (d.ShowDialog(this) != DialogResult.Cancel)
            {
                mapNotesTextBox.Text = d.FileName;
            }
        }
        public List<Technology> used = new List<Technology>();
        public List<Technology> getResultsTM(Dictionary<string,Technology> d, string filter)
        {
            List<Technology> results = new List<Technology>();
            foreach (Technology cur in d.Values)
            {
                if (cur.player.Name == filter)
                {
                    used.Add(cur);
                    results.Add(cur);
                }
            }
            return results;
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (refresh)
            {
                refresh = false;
                DisplayStepTabPage();
            }
        }
        bool down = false;
        Point omLocation = new Point();
        Point oLocation = new Point();
        private void TerritoryDefinitionsImageDrawer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                olocaiton = Form1.MousePosition;
                String s = RetrieveString("Enter New Territory's Name").Trim();
                Label l = new Label() { Text = s, BackColor = Color.LightGreen, Font = new Font(label1.Font, FontStyle.Bold), AutoSize = true, Location = GetPosition() - new Size((int)(Graphics.FromImage(new Bitmap(1, 1)).MeasureString(s, new Font(label1.Font, FontStyle.Bold)).Width / 2), 5) };
                l.MouseClick += new MouseEventHandler(l_MouseClick);
                if (l.Text.Length > 0)
                    TerritoryDefinitionsImageDrawer.Controls.Add(l);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            AutoFill();
        }
        public object[] getUnits()
        {
            List<object> result = new List<object>();
            foreach (Unit cur in Step5Info.units.Values)
            {
                result.Add(cur.Name);
            }
            return result.ToArray();
        }
        Automatic_Connection_Finder finder = new Automatic_Connection_Finder();
        public void AutoFill()//4,5,6,7,8,9,14
        {
            if (MessageBox.Show("Are you sure you want to use 'Auto-Fill'. Doing so will remove any information you entered in this step, and will try to enter the information using information from earlier steps or what a typical game would contain.", "Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                if (tabControl1.SelectedIndex == 2)
                {
                    finder.main = this;
                    if (finder.ShowDialog(this) != DialogResult.None && finder.connections.Count > 0)
                    {
                        Step1Info.MapImageWL = new Bitmap(Step1Info.MapImage);
                        Step3Info.connections.Clear();
                        foreach (Automatic_Connection_Finder.Connection cur in finder.connections)
                        {
                            try
                            {
                                Step3Info.connections.Add(cur.t1.name + "|" + cur.t2.name, new Connection() { t1 = Step2Info.territories[cur.t1.name], t2 = Step2Info.territories[cur.t2.name] });
                            }
                            catch
                            {
                                try
                                {
                                    Step3Info.connections.Add(cur.t2.name + "|" + cur.t1.name, new Connection() { t1 = Step2Info.territories[cur.t1.name], t2 = Step2Info.territories[cur.t2.name] });
                                }
                                catch { }
                            }
                        }
                        force = true;
                        surpress1 = true;
                        tabControl1_Selecting(new object(), new TabControlCancelEventArgs(new TabPage(), 0, false, TabControlAction.Selecting));
                        surpress1 = false;
                        finder.connections.Clear();
                    }
                    TerritoryConnectionsImageDrawer.Refresh();
                }
                if (tabControl1.SelectedIndex == 4)
                {
                    infochange1 = true;
                    List<Control> toDelete = new List<Control>();
                    foreach (Control cur in tabPage5.Controls)
                    {
                        if(cur is TextBox)
                        {
                            toDelete.Add(cur);
                        }
                    }
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        tabPage5.Controls.Remove(toDelete[i]);
                    }
                    change2 = 0;
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Infantry", Location = textBox9.Location, Size = textBox9.Size }, new TextBox() { Text = "3", Location = textBox10.Location, Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location, Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Artillery", Location = textBox9.Location + new Size(0, 25), Size = textBox9.Size }, new TextBox() { Text = "4", Location = textBox10.Location + new Size(0, 25), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 25), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Armour", Location = textBox9.Location + new Size(0, 50), Size = textBox9.Size }, new TextBox() { Text = "5", Location = textBox10.Location + new Size(0, 50), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 50), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Fighter", Location = textBox9.Location + new Size(0, 75), Size = textBox9.Size }, new TextBox() { Text = "10", Location = textBox10.Location + new Size(0, 75), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 75), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Bomber", Location = textBox9.Location + new Size(0, 100), Size = textBox9.Size }, new TextBox() { Text = "15", Location = textBox10.Location + new Size(0, 100), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 100), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Transport", Location = textBox9.Location + new Size(0, 125), Size = textBox9.Size }, new TextBox() { Text = "8", Location = textBox10.Location + new Size(0, 125), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 125), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Carrier", Location = textBox9.Location + new Size(0, 150), Size = textBox9.Size }, new TextBox() { Text = "16", Location = textBox10.Location + new Size(0, 150), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 150), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Destroyer", Location = textBox9.Location + new Size(0, 175), Size = textBox9.Size }, new TextBox() { Text = "12", Location = textBox10.Location + new Size(0, 175), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 175), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Battleship", Location = textBox9.Location + new Size(0, 200), Size = textBox9.Size }, new TextBox() { Text = "24", Location = textBox10.Location + new Size(0, 200), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 200), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Submarine", Location = textBox9.Location + new Size(0, 225), Size = textBox9.Size }, new TextBox() { Text = "8", Location = textBox10.Location + new Size(0, 225), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 225), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "Factory", Location = textBox9.Location + new Size(0, 250), Size = textBox9.Size }, new TextBox() { Text = "15", Location = textBox10.Location + new Size(0, 250), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 250), Size = textBox11.Size } });
                    tabPage5.Controls.AddRange(new Control[] { new TextBox() { Text = "AAGun", Location = textBox9.Location + new Size(0, 275), Size = textBox9.Size }, new TextBox() { Text = "5", Location = textBox10.Location + new Size(0, 275), Size = textBox10.Size }, new TextBox() { Text = "1", Location = textBox11.Location + new Size(0, 275), Size = textBox11.Size } });
                    change2 += 275;
                    button9.Location = new Point(456, 52 + 275);
                    button10.Location = new Point(102, 82 + 275);
                    panel6.Size = new Size(5, 36 + 275);
                }
                else if (tabControl1.SelectedIndex == 5)
                {
                    infochange2 = true;
                    infochange3 = true;
                    List<Control> toDelete = new List<Control>();
                    foreach (Control cur in tabPage6.Controls)
                    {
                        if (cur is TextBox)
                        {
                            toDelete.Add(cur);
                        }
                    }
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        tabPage6.Controls.Remove(toDelete[i]);
                    }
                    change3 = 0;
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "bid", Location = textBox12.Location, Size = textBox12.Size }, new TextBox() { Text = "BidPurchaseDelegate", Location = textBox14.Location, Size = textBox14.Size }, new TextBox() { Text = "Bid Purchase", Location = textBox13.Location, Size = textBox13.Size } });
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "placeBid", Location = textBox12.Location + new Size(0, 25), Size = textBox12.Size }, new TextBox() { Text = "BidPlaceDelegate", Location = textBox14.Location + new Size(0, 25), Size = textBox14.Size }, new TextBox() { Text = "Bid Placement", Location = textBox13.Location + new Size(0, 25), Size = textBox13.Size } });
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "tech", Location = textBox12.Location + new Size(0, 50), Size = textBox12.Size }, new TextBox() { Text = "TechnologyDelegate", Location = textBox14.Location + new Size(0, 50), Size = textBox14.Size }, new TextBox() { Text = "Research Technology", Location = textBox13.Location + new Size(0, 50), Size = textBox13.Size } });
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "tech_Activation", Location = textBox12.Location + new Size(0, 75), Size = textBox12.Size }, new TextBox() { Text = "TechActivationDelegate", Location = textBox14.Location + new Size(0, 75), Size = textBox14.Size }, new TextBox() { Text = "Activate Technology", Location = textBox13.Location + new Size(0, 75), Size = textBox13.Size } });
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "purchase", Location = textBox12.Location + new Size(0, 100), Size = textBox12.Size }, new TextBox() { Text = "PurchaseDelegate", Location = textBox14.Location + new Size(0, 100), Size = textBox14.Size }, new TextBox() { Text = "Purchase Units", Location = textBox13.Location + new Size(0, 100), Size = textBox13.Size } });
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "move", Location = textBox12.Location + new Size(0, 125), Size = textBox12.Size }, new TextBox() { Text = "MoveDelegate", Location = textBox14.Location + new Size(0, 125), Size = textBox14.Size }, new TextBox() { Text = "Combat Move", Location = textBox13.Location + new Size(0, 125), Size = textBox13.Size } });
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "battle", Location = textBox12.Location + new Size(0, 150), Size = textBox12.Size }, new TextBox() { Text = "BattleDelegate", Location = textBox14.Location + new Size(0, 150), Size = textBox14.Size }, new TextBox() { Text = "Combat", Location = textBox13.Location + new Size(0, 150), Size = textBox13.Size } });
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "place", Location = textBox12.Location + new Size(0, 175), Size = textBox12.Size }, new TextBox() { Text = "PlaceDelegate", Location = textBox14.Location + new Size(0, 175), Size = textBox14.Size }, new TextBox() { Text = "Place Units", Location = textBox13.Location + new Size(0, 175), Size = textBox13.Size } });
                    tabPage6.Controls.AddRange(new Control[] { new TextBox() { Text = "endTurn", Location = textBox12.Location + new Size(0, 200), Size = textBox12.Size }, new TextBox() { Text = "EndTurnDelegate", Location = textBox14.Location + new Size(0, 200), Size = textBox14.Size }, new TextBox() { Text = "Turn Complete", Location = textBox13.Location + new Size(0, 200), Size = textBox13.Size } });
                    change3 += 200;
                    button11.Location = new Point(488, 65 + 200);
                    button12.Location = new Point(39, 93 + 200);
                    panel7.Size = new Size(5, 36 + 200);
                }
                else if (tabControl1.SelectedIndex == 6)
                {
                    tabPage7.VerticalScroll.Value = 0;
                    tabPage7.HorizontalScroll.Value = 0;
                    List<Control> toDelete = new List<Control>();
                    foreach (Control cur in tabPage7.Controls)
                    {
                        if (cur is TextBox || cur is ComboBox)
                        {
                            toDelete.Add(cur);
                        }
                    }
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        tabPage7.Controls.Remove(toDelete[i]);
                    }
                    int ch = 0;
                    bool sw = false;
                    foreach (Player cur in Step4Info.players.Values)
                    {
                        foreach (GameplaySequence cur2 in Step6Info.gameplaySequences.Values)
                        {
                            if (cur2.Name.ToLower().Contains("bid"))
                            {
                                ComboBox cb1 = new ComboBox() { Text = cur2.Name, Location = comboBox16.Location + new Size(0, ch), Size = comboBox16.Size };
                                foreach (GameplaySequence cur3 in Step6Info.gameplaySequences.Values)
                                {
                                    cb1.Items.Add(cur3.Name);
                                }
                                ComboBox cb2 = new ComboBox() { Text = cur.Name, Location = comboBox17.Location + new Size(0, ch), Size = comboBox17.Size };
                                foreach (Player cur3 in Step4Info.players.Values)
                                {
                                    cb2.Items.Add(cur3.Name);
                                }
                                tabPage7.Controls.AddRange(new Control[] { new TextBox() { Text = cur.Name.Replace(" ", "").ToLower() + cur2.Name.Substring(0, 1).ToUpper() + cur2.Name.Substring(1).Replace("_", ""), Location = textBox15.Location + new Size(0, ch), Size = textBox15.Size }, cb1, cb2, new TextBox() { Text = "1", Location = textBox18.Location + new Size(0, ch), Size = textBox18.Size } });
                                ch += 25;
                            }
                            else
                            {
                                if (!cur2.Name.ToLower().Contains("move") && !cur2.Name.ToLower().Contains("battle"))
                                {
                                    ComboBox cb1 = new ComboBox() { Text = cur2.Name, Location = comboBox16.Location + new Size(0, ch), Size = comboBox16.Size };
                                    foreach (GameplaySequence cur3 in Step6Info.gameplaySequences.Values)
                                    {
                                        cb1.Items.Add(cur3.Name);
                                    }
                                    ComboBox cb2 = new ComboBox() { Text = cur.Name, Location = comboBox17.Location + new Size(0, ch), Size = comboBox17.Size };
                                    foreach (Player cur3 in Step4Info.players.Values)
                                    {
                                        cb2.Items.Add(cur3.Name);
                                    }
                                    tabPage7.Controls.AddRange(new Control[] { new TextBox() { Text = cur.Name.Replace(" ", "").ToLower() + cur2.Name.Substring(0, 1).ToUpper() + cur2.Name.Substring(1).Replace("_", ""), Location = textBox15.Location + new Size(0, ch), Size = textBox15.Size }, cb1,cb2, new TextBox() { Text = "1000", Location = textBox18.Location + new Size(0, ch), Size = textBox18.Size } });
                                    ch += 25;
                                }
                                else if (cur2.Name.ToLower().Contains("move"))
                                {
                                    sw = !sw;
                                    ComboBox cb1 = new ComboBox() { Text = cur2.Name, Location = comboBox16.Location + new Size(0, ch), Size = comboBox16.Size };
                                    foreach (GameplaySequence cur3 in Step6Info.gameplaySequences.Values)
                                    {
                                        cb1.Items.Add(cur3.Name);
                                    }
                                    ComboBox cb2 = new ComboBox() { Text = cur.Name, Location = comboBox17.Location + new Size(0, ch), Size = comboBox17.Size };
                                    foreach (Player cur3 in Step4Info.players.Values)
                                    {
                                        cb2.Items.Add(cur3.Name);
                                    }
                                    tabPage7.Controls.AddRange(new Control[] { new TextBox() { Text = cur.Name.Replace(" ", "").ToLower() + "CombatMove", Location = textBox15.Location + new Size(0, ch), Size = textBox15.Size }, cb1, cb2, new TextBox() { Text = "1000", Location = textBox18.Location + new Size(0, ch), Size = textBox18.Size } });
                                    ch += 25;
                                }
                                else
                                {
                                    ComboBox cb1 = new ComboBox() { Text = cur2.Name, Location = comboBox16.Location + new Size(0, ch), Size = comboBox16.Size };
                                    foreach (GameplaySequence cur3 in Step6Info.gameplaySequences.Values)
                                    {
                                        cb1.Items.Add(cur3.Name);
                                    }
                                    ComboBox cb2 = new ComboBox() { Text = cur.Name, Location = comboBox17.Location + new Size(0, ch), Size = comboBox17.Size };
                                    foreach (Player cur3 in Step4Info.players.Values)
                                    {
                                        cb2.Items.Add(cur3.Name);
                                    }
                                    tabPage7.Controls.AddRange(new Control[] { new TextBox() { Text = cur.Name.Replace(" ", "").ToLower() + cur2.Name.Substring(0, 1).ToUpper() + cur2.Name.Substring(1).Replace("_", ""), Location = textBox15.Location + new Size(0, ch), Size = textBox15.Size }, cb1, cb2, new TextBox() { Text = "1000", Location = textBox18.Location + new Size(0, ch), Size = textBox18.Size } });
                                    ch += 25;
                                }
                            }
                        }
                    }
                    change4 = ch - 25;
                    button13.Location = new Point(485, (65 + ch) - 25);
                    button14.Location = new Point(18, (93 + ch) - 25);
                    panel8.Size = new Size(5, (36 + ch) - 25);
                }   
                else if (tabControl1.SelectedIndex == 7)
                {
                    tabPage8.VerticalScroll.Value = 0;
                    tabPage8.HorizontalScroll.Value = 0;
                    List<Control> toDelete = new List<Control>();
                    foreach (Control cur in tabPage8.Controls)
                    {
                        if (cur is TextBox || cur is ComboBox)
                        {
                            toDelete.Add(cur);
                        }
                    }
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        tabPage8.Controls.Remove(toDelete[i]);
                    }
                    int ch = 0;
                    object[] units = getUnits();
                    foreach (Player cur in Step4Info.players.Values)
                    {
                        ComboBox cb1 = new ComboBox() { Text = cur.Name, Location = comboBox2.Location + new Size(0, ch), Size = comboBox2.Size };
                        ComboBox cb2 = new ComboBox() { Text = "False", Location = comboBox1.Location + new Size(0, ch), Size = comboBox1.Size };
                        tabPage8.Controls.AddRange(new Control[] { new TextBox() { Text = "heavyBomber", Location = textBox16.Location + new Size(0, ch), Size = textBox16.Size }, cb1, cb2 });
                        ch += 25;
                        cb1 = new ComboBox() { Text = cur.Name, Location = comboBox2.Location + new Size(0, ch), Size = comboBox2.Size };
                        cb2 = new ComboBox() { Text = "False", Location = comboBox1.Location + new Size(0, ch), Size = comboBox1.Size };
                        cb1.Items.AddRange(units);
                        cb2.Items.AddRange(getTrueFalseItems());
                        tabPage8.Controls.AddRange(new Control[] { new TextBox() { Text = "jetPower", Location = textBox16.Location + new Size(0, ch), Size = textBox16.Size }, cb1, cb2 });
                        ch += 25;
                        cb1 = new ComboBox() { Text = cur.Name, Location = comboBox2.Location + new Size(0, ch), Size = comboBox2.Size };
                        cb2 = new ComboBox() { Text = "False", Location = comboBox1.Location + new Size(0, ch), Size = comboBox1.Size };
                        cb1.Items.AddRange(units);
                        cb2.Items.AddRange(getTrueFalseItems());
                        tabPage8.Controls.AddRange(new Control[] { new TextBox() { Text = "superSub", Location = textBox16.Location + new Size(0, ch), Size = textBox16.Size }, cb1, cb2 });
                        ch += 25;
                        cb1 = new ComboBox() { Text = cur.Name, Location = comboBox2.Location + new Size(0, ch), Size = comboBox2.Size };
                        cb2 = new ComboBox() { Text = "False", Location = comboBox1.Location + new Size(0, ch), Size = comboBox1.Size };
                        cb1.Items.AddRange(units);
                        cb2.Items.AddRange(getTrueFalseItems());
                        tabPage8.Controls.AddRange(new Control[] { new TextBox() { Text = "rocket", Location = textBox16.Location + new Size(0, ch), Size = textBox16.Size }, cb1, cb2 });
                        ch += 25;
                        cb1 = new ComboBox() { Text = cur.Name, Location = comboBox2.Location + new Size(0, ch), Size = comboBox2.Size };
                        cb2 = new ComboBox() { Text = "False", Location = comboBox1.Location + new Size(0, ch), Size = comboBox1.Size };
                        cb1.Items.AddRange(units);
                        cb2.Items.AddRange(getTrueFalseItems());
                        tabPage8.Controls.AddRange(new Control[] { new TextBox() { Text = "longRangeAir", Location = textBox16.Location + new Size(0, ch), Size = textBox16.Size }, cb1, cb2 });
                        ch += 25;
                    }
                    change5 = ch - 25;
                    button17.Location = new Point(423, (66 + ch) - 25);
                    button18.Location = new Point(62, (93 + ch) - 25);
                    panel9.Size = new Size(5, (36 + ch) - 25);
                }
                else if (tabControl1.SelectedIndex == 8) 
                {
                    TabPage cur = tabControl2.TabPages[tabControl2.SelectedIndex];

                    cur.VerticalScroll.Value = 0;
                    cur.HorizontalScroll.Value = 0;
                    List<Control> toDelete = new List<Control>();
                    Button b1 = null;
                    Button b2 = null;
                    foreach (Control cur2 in cur.Controls)
                    {
                        if (cur2 is TextBox || cur2 is ComboBox)
                        {
                            toDelete.Add(cur2);
                        }
                        else if (cur2 is Button)
                        {
                            if (b1 == null)
                                b1 = (Button)cur2;
                            else
                                b2 = (Button)cur2;
                        }
                    }
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        cur.Controls.Remove(toDelete[i]);
                    }
                    int ch = 0;
                    object[] units = getUnits();
                    foreach (Unit cur2 in Step5Info.units.Values)
                    {
                        ComboBox cb1 = new ComboBox() { Text = cur2.Name, Location = comboBox3.Location + new Size(0, ch), Size = comboBox3.Size };
                        cb1.Items.AddRange(units);
                        cur.Controls.Add(cb1);
                        ch += 25;
                    }
                    cur.Tag = ch - 25;
                    b1.Location = new Point(189, (57 + ch) - 25);
                    b2.Location = new Point(39, (87 + ch) - 25);
                }
                else if (tabControl1.SelectedIndex == 9)
                {
                    foreach (TabPage cur in tabControl3.TabPages)
                    {
                        cur.VerticalScroll.Value = 0;
                        cur.HorizontalScroll.Value = 0;
                        List<Control> toDelete = new List<Control>();
                        Button b1 = null;
                        Button b2 = null;
                        foreach (Control cur2 in cur.Controls)
                        {
                            if (cur2 is TextBox || cur2 is ComboBox)
                            {
                                toDelete.Add(cur2);
                            }
                            else if (cur2 is Button)
                            {
                                if (b1 == null)
                                    b1 = (Button)cur2;
                                else
                                    b2 = (Button)cur2;
                            }
                        }
                        for (int i = 0; i < toDelete.Count; i++)
                        {
                            cur.Controls.Remove(toDelete[i]);
                        }
                        int ch = 0;
                        if (cur.Text.ToLower().Contains("infantry"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "1", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "1", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "transportCost", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "artillerySupportable", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("artillery"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "1", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "transportCost", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "artillery", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("armour"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "transportCost", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "canBlitz", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("fighter"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "4", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "4", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "carrierCost", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "1", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isAir", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("bomber"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "6", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "4", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "1", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isStrategicBomber", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isAir", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("transport"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "0", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "1", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "transportCapacity", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "5", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isSea", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("battleship"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "4", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "4", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "canBombard", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isSea", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isTwoHit", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("destroyer"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isSea", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isDestroyer", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("carrier"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "1", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isSea", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "carrierCapacity", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("submarine"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "attack", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "defense", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "2", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isSea", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isSub", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("aagun"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isAA", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "transportCost", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "3", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "movement", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "1", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        else if (cur.Text.ToLower().Contains("factory"))
                        {
                            cur.Controls.AddRange(new Control[] { new TextBox() { Text = "isFactory", Location = comboBox4.Location + new Size(0, ch), Size = comboBox4.Size }, new TextBox() { Text = "true", Location = textBox17.Location + new Size(0, ch), Size = textBox17.Size } });
                            ch += 25;
                        }
                        cur.Tag = ch;
                        b1.Location = new Point(294, (57 + ch) - 25);
                        b2.Location = new Point(39, (87 + ch) - 25);
                    }
                }
                else if (tabControl1.SelectedIndex == 14)
                {
                    List<Control> toDelete = new List<Control>();
                    foreach (Control cur in tabPage15.Controls)
                    {
                        if (cur is TextBox || cur is ComboBox)
                        {
                            toDelete.Add(cur);
                        }
                    }
                    for (int i = 0; i < toDelete.Count; i++)
                    {
                        tabPage15.Controls.Remove(toDelete[i]);
                    } 
                    int ch = 0;
                    ComboBox cb1 = new ComboBox() { Text = "True", Location = comboBox5.Location + new Size(0, ch), Size = comboBox5.Size };
                    cb1.Items.AddRange(getTrueFalseItems());


                    tabPage15.Controls.AddRange(new Control[] { new TextBox() { Text = "Heavy Bomber Dice Rolls", Location = textBox19.Location + new Size(0, ch), Size = textBox19.Size }, new TextBox() { Text = "2", Location = textBox21.Location + new Size(0, ch), Size = textBox21.Size }, cb1, new TextBox() { Text = "0", Location = textBox20.Location + new Size(0, ch), Size = textBox20.Size }, new TextBox() { Text = "10", Location = textBox22.Location + new Size(0, ch), Size = textBox22.Size } });
                    ch += 25;
                    cb1 = new ComboBox() { Text = "True", Location = comboBox5.Location + new Size(0, ch), Size = comboBox5.Size };
                    cb1.Items.AddRange(getTrueFalseItems());
                    tabPage15.Controls.AddRange(new Control[] { new TextBox() { Text = "Always on AA", Location = textBox19.Location + new Size(0, ch), Size = textBox19.Size }, new TextBox() { Text = "true", Location = textBox21.Location + new Size(0, ch), Size = textBox21.Size }, cb1, new TextBox() { Text = "0", Location = textBox20.Location + new Size(0, ch), Size = textBox20.Size }, new TextBox() { Text = "0", Location = textBox22.Location + new Size(0, ch), Size = textBox22.Size } });
                    ch += 25;
                    cb1 = new ComboBox() { Text = "True", Location = comboBox5.Location + new Size(0, ch), Size = comboBox5.Size };
                    cb1.Items.AddRange(getTrueFalseItems());
                    tabPage15.Controls.AddRange(new Control[] { new TextBox() { Text = "Battleships repair at end of round", Location = textBox19.Location + new Size(0, ch), Size = textBox19.Size }, new TextBox() { Text = "true", Location = textBox21.Location + new Size(0, ch), Size = textBox21.Size }, cb1, new TextBox() { Text = "0", Location = textBox20.Location + new Size(0, ch), Size = textBox20.Size }, new TextBox() { Text = "0", Location = textBox22.Location + new Size(0, ch), Size = textBox22.Size } });
                    ch += 25;
                    cb1 = new ComboBox() { Text = "True", Location = comboBox5.Location + new Size(0, ch), Size = comboBox5.Size };
                    cb1.Items.AddRange(getTrueFalseItems());
                    tabPage15.Controls.AddRange(new Control[] { new TextBox() { Text = "Territory Turn Limit", Location = textBox19.Location + new Size(0, ch), Size = textBox19.Size }, new TextBox() { Text = "false", Location = textBox21.Location + new Size(0, ch), Size = textBox21.Size }, cb1, new TextBox() { Text = "0", Location = textBox20.Location + new Size(0, ch), Size = textBox20.Size }, new TextBox() { Text = "0", Location = textBox22.Location + new Size(0, ch), Size = textBox22.Size } });
                    ch += 25;
                    cb1 = new ComboBox() { Text = "True", Location = comboBox5.Location + new Size(0, ch), Size = comboBox5.Size };
                    cb1.Items.AddRange(getTrueFalseItems());
                    tabPage15.Controls.AddRange(new Control[] { new TextBox() { Text = "Low Luck", Location = textBox19.Location + new Size(0, ch), Size = textBox19.Size }, new TextBox() { Text = "false", Location = textBox21.Location + new Size(0, ch), Size = textBox21.Size }, cb1, new TextBox() { Text = "0", Location = textBox20.Location + new Size(0, ch), Size = textBox20.Size }, new TextBox() { Text = "0", Location = textBox22.Location + new Size(0, ch), Size = textBox22.Size } });
                    ch += 25;
                    cb1 = new ComboBox() { Text = "True", Location = comboBox5.Location + new Size(0, ch), Size = comboBox5.Size };
                    cb1.Items.AddRange(getTrueFalseItems());
                    foreach (Player cur in Step4Info.players.Values)
                    {
                        tabPage15.Controls.AddRange(new Control[] { new TextBox() { Text = cur.Name + " bid", Location = textBox19.Location + new Size(0, ch), Size = textBox19.Size }, new TextBox() { Text = "0", Location = textBox21.Location + new Size(0, ch), Size = textBox21.Size }, new ComboBox() { Text = "True", Location = comboBox5.Location + new Size(0, ch), Size = comboBox5.Size }, new TextBox() { Text = "0", Location = textBox20.Location + new Size(0, ch), Size = textBox20.Size }, new TextBox() { Text = "1000", Location = textBox22.Location + new Size(0, ch), Size = textBox22.Size } });
                        ch += 25;
                    }
                    change6 = ch;
                    button23.Location = new Point(494, (52 + ch) - 25);
                    button24.Location = new Point(25, (80 + ch) - 25);
                    panel15.Size = new Size(5, (36 + ch) - 25);
                    panel16.Size = new Size(5, (36 + ch) - 25);
                }
            }
        }
        public string Cap(string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }
    }
    public class GrabPanel : Panel
    {
        bool down = false;
        Point omLocation = new Point();
        Point oLocation = new Point();
        public GrabPanel()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
            this.BackgroundImageLayout = ImageLayout.None;
            MouseDown += new MouseEventHandler(mouseDown);
            MouseMove += new MouseEventHandler(mouseMove);
            MouseUp += new MouseEventHandler(mouseUp);
        }
        Rectangle lastVisibleBounds = new Rectangle();
        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.Clip = new Region(e.ClipRectangle);
            //base.OnPaint(e);
        }
        Rectangle visibleBounds = new Rectangle();
        //public new Image BackgroundImage = null;
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clip = new Region(e.ClipRectangle);
            visibleBounds = new Rectangle(new Point(-((Panel)this.Parent).AutoScrollPosition.X, -((Panel)this.Parent).AutoScrollPosition.Y), new Size(((Panel)this.Parent).Size.Width - (((Panel)this.Parent).VerticalScroll.Visible ? 21 : 4), ((Panel)this.Parent).Size.Height - (((Panel)this.Parent).HorizontalScroll.Visible ? 21 : 4)));
            //e.Graphics.DrawImage(BackgroundImage, visibleBounds.Location.X,visibleBounds.Location.Y,visibleBounds,GraphicsUnit.Pixel);
            e.Graphics.Clip.Intersect(visibleBounds);
            if (this.DesignMode)
                base.OnPaintBackground(e);
            else
            {
                if (!visibleBounds.Equals(lastVisibleBounds))
                {
                    Region r = new Region(visibleBounds);
                    r.Exclude(lastVisibleBounds);
                    e.Graphics.Clip = r;

                    base.OnPaintBackground(e);

                    lastVisibleBounds = new Rectangle(visibleBounds.X, visibleBounds.Y, visibleBounds.Width, visibleBounds.Height);
                }
                else
                {
                    base.OnPaintBackground(e);
                }
            }
        }
        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                omLocation = Form1.MousePosition;
                oLocation = new Point(((Panel)Parent).HorizontalScroll.Value, ((Panel)Parent).VerticalScroll.Value);
                down = true;
            }
        }

        private void mouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (down)
                {
                    ((Form1.FlickerFreeHolderPanel)Parent).AutoScrollPosition = new Point(oLocation.X + omLocation.X - Form1.MousePosition.X, oLocation.Y + omLocation.Y - Form1.MousePosition.Y);
                }
            }
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                down = false;
            }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams params1 = base.CreateParams;
                params1.ExStyle |= 0x20;
                return params1;
            }
        }
    }
    public class Step1Info
    {
        public static String MapName = "";
        public static String MapVersion = "";
        public static String ResourceName = "";
        public static String MapImageLocation = "";
        public static String CentersLocation = "";
        public static String WaterTerritoryFilter = "";
        public static Image MapImage = new Bitmap(1,1);
        public static Image MapImageWL = new Bitmap(1, 1);
        public static string LoadedFile = "";
        //Also see players 'InitialResources'
    }
    public class Step2Info
    {
        public static Dictionary<string, Territory> territories = new Dictionary<string, Territory>();
    }
    public class Step3Info
    {
        public static Dictionary<string, Connection> connections = new Dictionary<string, Connection>();
    }
    public class Step4Info
    {
        public static Dictionary<string, Player> players = new Dictionary<string, Player>();
    }
    public class Step5Info
    {
        public static Dictionary<string, Unit> units = new Dictionary<string, Unit>();
    }
    public class Step6Info
    {
        public static Dictionary<string, GameplaySequence> gameplaySequences = new Dictionary<string, GameplaySequence>();
    }
    public class Step7Info
    {
        public static Dictionary<string, PlayerSequence> playerSequences = new Dictionary<string, PlayerSequence>();
    }
    public class Step8Info
    {
        public static Dictionary<string, Technology> technologies = new Dictionary<string, Technology>();
    }
    public class Step9Info
    {
        public static Dictionary<string, ProductionFrontier> ProductionFrontiers = new Dictionary<string, ProductionFrontier>();
    }
    public class Step10Info
    {
        //Use players 'attachment'
    }
    public class Step11Info
    {
        //Use territories 'production'
    }
    public class Step12Info
    {
        public static Dictionary<string, Canal> Canals = new Dictionary<string, Canal>();
    }
    public class Step13Info
    {
        //Use territories 'owner'
    }
    public class Step14Info
    {
        //Use territories 'units'
    }
    public class Step15Info
    {
        public static Dictionary<string, Setting> Settings = new Dictionary<string, Setting>();
    }
    public class Step16Info
    {
        public static string Notes = "";
    }
    public class Territory
    {
        public string Name = "";
        public bool IsWater = false;
        public Player Owner = new Player();
        public int Production = 0;
        public bool IsCapitol = false;
        public bool IsVictoryCity = false;
        public bool IsImpassable = false;
        public List<Unit> Units = new List<Unit>();
        public Label Label = new Label();
    }
    public class Connection
    {
        public Territory t1 = new Territory();
        public Territory t2 = new Territory();
    }
    [Serializable]
    public class SConnection
    {
        public SConnection(Connection from)
        {
            t1 = from.t1.Name;
            t2 = from.t2.Name;
        }
        public string t1 = "";
        public string t2 = "";
    }
    public class Player
    {
        public string Name = "";
        public bool Optional = false;
        public string Alliance = "";
        public Dictionary<string, StartTechnology> AvailableTechnologies = new Dictionary<string, StartTechnology>();
        public int InitialResources = 0;
    }
    public class Unit
    {
        public string Name = "";
        public Cost cost = new Cost();
        public Player unitOwner = new Player();
        public UnitAttachment attachment = new UnitAttachment();
    }
    public class UnitAttachment
    {
        public List<UnitOption> options = new List<UnitOption>();
    }
    public class UnitOption
    {
        public string Name = "";
        public object Value = null;
    }
    public class GameplaySequence
    {
        public string Name = "";
        public string Class = "";
        public string Display = "";
    }
    public class PlayerSequence
    {
        public string Name = "";
        public GameplaySequence Delegate = new GameplaySequence();
        public Player player = new Player();
        public int MaxRunCount = 0;
    }
    public class Technology
    {
        public string Name = "";
        public Player player = new Player();
        public bool AlreadyEnabled = false;
    }
    public class Cost
    {
        public string ResourceType = "";
        public int cost = 0;
        public Result result = new Result();
    }
    public class Result
    {
        public string ResourceOrUnitName = "";
        public int BuyQuantity = 0;
    }
    public class ProductionFrontier
    {
        public string Name = "";
        public List<Unit> UnitsInFrontier = new List<Unit>();
    }
    public class StartTechnology
    {
        public string Name = "";
        public bool InitiallyEnabled = false;
    }
    public class Canal
    {
        public string Name = "";
        public List<Territory> CanalSeaNeighbors = new List<Territory>();
        public List<Territory> LandTerritories = new List<Territory>();
    }
    public class Setting
    {
        public string Name = "";
        public Object Value = null;
        public bool Editable = false;
        public int IntMin = 0;
        public int IntMax = 0;
    }
}
