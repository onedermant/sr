using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SR2
{
    public partial class Form1 : Form
    {
        int labelPosY = 30;
        int labelPosX = 6;
        int BadlabelPosY = 30;
        int BadlabelPosX = 6;
        List<CharacterClass> goodList = new List<CharacterClass>();
        List<CharacterClass> badList = new List<CharacterClass>();
        List<CharacterClass> InitiativeOrder = new List<CharacterClass>();
        int turn = 1;
        Button goButton = new Button();
        CharacterClass target = null;
        List<Button> targetButtons = new List<Button>();
        bool bScroll = false;
        public Form1()
        {
            InitializeComponent();
            EventClass.eventLog2 += new EventClass.EventLogHandler(EventLog);
            Populate();
        }

        bool isGood(CharacterClass c)
        {
            foreach(CharacterClass goodChar in goodList)
            {
                if (c.Name == goodChar.Name) return true;
            }
            return false;
        }

        void addChar(string line,bool good)
        {
            string[] values = line.Split(',');
            if (values.Count() != 14)
            {
                EventClass.LogEvent("Error reading line : " + line);
                return;
            }
            CharacterClass c = new CharacterClass();
            c.Name = values[0];
            c.Agility = int.Parse(values[1]);
            c.Body = int.Parse(values[2]);            
            c.Strength = int.Parse(values[3]);
            c.Intelligence = int.Parse(values[4]);
            c.Intuition = int.Parse(values[5]);
            c.Willpower = int.Parse(values[6]);
            c.Charisma = int.Parse(values[7]);
            c.Logic = int.Parse(values[8]);
            c.Edge = int.Parse(values[9]);
            c.Armor = int.Parse(values[10]);
            c.Magic = float.Parse(values[11]);
            c.Initiative = int.Parse(values[12]);
            c.Reaction = int.Parse(values[13]);
            c.curInitiative = 0;
            ComboBox combo = new ComboBox();
            combo.DisplayMember = "Weapons";
            combo.SelectedIndexChanged += new EventHandler(changeModeEvent);
            combo.Tag = c;
            ComboBox modes = new ComboBox();
            modes.DisplayMember = "Modes";
           
            if (good)
            {
                Label newLabel = new Label();
                newLabel.Location = new Point(40, labelPosY);
                newLabel.Name = c.Name;
                newLabel.Text = c.getLabel();
                newLabel.AutoSize = true;
                groupBox1.Controls.Add(newLabel);
                combo.Location = new Point(40, labelPosY + 15);
                modes.Location = new Point(140, labelPosY + 15);
                groupBox1.Controls.Add(combo);
                groupBox1.Controls.Add(modes);
                labelPosY += 40;
                c.label = newLabel;
                c.weaponList = combo;
                c.modeList = modes;
                goodList.Add(c);
                EventClass.LogEvent(c.Name + " added to good guys.");          
            }
            else
            {
                Label newLabel = new Label();
                newLabel.Location = new Point(40, BadlabelPosY);
                newLabel.Name = c.Name;
                newLabel.Text = c.getLabel();
                newLabel.AutoSize = true;
                groupBox2.Controls.Add(newLabel);
                combo.Location = new Point(40, BadlabelPosY + 15);
                modes.Location = new Point(140, BadlabelPosY + 15);
                groupBox2.Controls.Add(combo);
                groupBox2.Controls.Add(modes);
                BadlabelPosY += 40;
                c.label = newLabel;
                c.weaponList = combo;
                c.modeList = modes;
                badList.Add(c);
                EventClass.LogEvent(c.Name + " added to bad guys.");
            }
        }

        void addWeaponToChar(string line)
        {
            string[] parse = line.Replace("{", "").Replace("}", "").Split(',');
            bool found = false;
            DamageClass d = new DamageClass();
            d.baseDamage = int.Parse(parse[1]);
            d.isPhysical = bool.Parse(parse[2]);
            d.armorPen = int.Parse(parse[3]);
            d.fullAuto = bool.Parse(parse[4]);
            d.semi = bool.Parse(parse[5]);
            d.single = bool.Parse(parse[6]);
            d.melee = bool.Parse(parse[7]);
            d.spell = bool.Parse(parse[8]);
            d.modifier = int.Parse(parse[9]);
            d.Name = parse[10];
            d.attrib = parse[11];
            foreach (CharacterClass c in goodList)
            {
                if (c.Name == parse[0])
                {
                    c.weaponList.Items.Add(d.Name);
                    c.damageList.Add(d);
                    EventClass.LogEvent(d.Name + " weapon added to " + c.Name);
                    c.damageList.Add(d);                    
                    found = true;
                    return;
                }
            }

            foreach (CharacterClass c in badList)
            {
                if (c.Name == parse[0])
                {
                    c.weaponList.Items.Add(d.Name);
                    c.damageList.Add(d);
                    EventClass.LogEvent(d.Name + " weapon added to " + c.Name);
                    found = true;
                    return;
                }
            }
        }

        void addSkillToChar(string line)
        {
            string[] parse = line.Replace("[", "").Replace("]", "").Split(',');
            bool found = false;
            SkillClass d = new SkillClass();
            d.Name = parse[1];
            d.Value = int.Parse(parse[2]);
            d.Specialization = parse[3];
            foreach (CharacterClass c in goodList)
            {
                if (c.Name == parse[0])
                {                                       
                    c.skillList.Add(d);
                    EventClass.LogEvent(d.Name + " skill added to " + c.Name);
                    found = true;
                    return;
                }
            }

            foreach (CharacterClass c in badList)
            {
                if (c.Name == parse[0])
                {
                    c.skillList.Add(d);
                    EventClass.LogEvent(d.Name + " skill added to " + c.Name);
                    found = true;
                    return;
                }
            }
            EventClass.LogEvent("ERROR: cannot find " + parse[0] + " when adding skill " + d.Name);
        }

        void ImportGood()
        {
            try
            {
                string[] lines = File.ReadAllLines("good.csv");
                foreach (string line in lines)
                {
                    if (line[0] == '/') continue;
                    if (line[0] == '{')
                    {
                        addWeaponToChar(line);
                        continue;
                    }
                    if (line[0] == '[')
                    {
                        addSkillToChar(line);
                        continue;
                    }
                    addChar(line, true);
                }
            }
            catch (Exception e)
            {

            }
        }

        void ImportBad()
        {
            try
            {
                string[] lines = File.ReadAllLines("bad.csv");
                foreach (string line in lines)
                {
                    if (line[0] == '/') continue;
                    if (line[0] == '{')
                    {
                        addWeaponToChar(line);
                        continue;
                    }
                    if (line[0] == '[')
                    {
                        addSkillToChar(line);
                        continue;
                    }
                    addChar(line, false);
                }
            }
            catch (Exception e)
            {

            }
        }
        void ParseImportAllFile()
        {
            ImportGood();
            ImportBad();
        }

        void Populate()
        {
            ParseImportAllFile();          
        }
        private void button1_Click(object sender, EventArgs e)
        {
            foreach(CharacterClass c in goodList)
            {
                c.weaponList.SelectedIndex = 0;
                c.modeList.SelectedIndex = 0;
            }
            foreach (CharacterClass c in badList)
            {
                c.weaponList.SelectedIndex = 0;
                c.modeList.SelectedIndex = 0;
            }
            rollInitiative();
        }

        void newInitiative()
        {
            List<CharacterClass> removeList = new List<CharacterClass>();
            foreach (CharacterClass c in goodList)
            {
                if (c.curInitiative - 10 <= 0)
                {
                    c.curInitiative = 0;
                    c.label.Text = c.getLabel() + " (" + c.curInitiative + ")";
                    removeList.Add(c);
                    continue;
                }
                c.curInitiative -= 10;
                c.label.Text = c.getLabel() + " (" + c.curInitiative + ")";
                InitiativeOrder.Add(c);
            }
            foreach (CharacterClass c in badList)
            {
                if (c.curInitiative - 10 <= 0)
                {
                    c.curInitiative = 0;
                    c.label.Text = c.getLabel() + " (" + c.curInitiative + ")";
                    removeList.Add(c);
                    continue;
                }
                c.curInitiative -= 10;
                c.label.Text = c.getLabel() + " (" + c.curInitiative + ")";
                InitiativeOrder.Add(c);
            }
            foreach (CharacterClass c in removeList)
            {
                c.weaponList.Enabled = false;
                c.modeList.Enabled = false;
                InitiativeOrder.Remove(c);
            }
            if (InitiativeOrder.Count == 0)
            {
                return;
            }
            InitiativeOrder = InitiativeOrder.OrderByDescending(x => x.curInitiative).ToList();
        }

        void rollInitiative()
        {
            lblTarget.Text = "";
            target = null;
            RollerClass r = new RollerClass();
            foreach (CharacterClass c in goodList)
            {
                // + " " + r.displayInitiative(c.Initiative, c.Reaction);                
                c.weaponList.Enabled = true;
                c.modeList.Enabled = true;
                EventClass.LogEvent(new EventLogArgs(EventLogArgs.LogEntryTypeEnum.INITIATIVE, c.Name + " initiative is " + c.curInitiative + " " + r.displayInitiative(c.Initiative, c.Reaction)));
                c.curInitiative = r.Initative;
                c.label.Text = c.getLabel() + " (" + r.Initative + ")";
            }
            foreach (CharacterClass c in badList)
            {
                // + " " + r.displayInitiative(c.Initiative, c.Reaction);
                c.weaponList.Enabled = true;
                c.modeList.Enabled = true;
                EventClass.LogEvent(new EventLogArgs(EventLogArgs.LogEntryTypeEnum.INITIATIVE, c.Name + " initiative is " + c.curInitiative + " " + r.displayInitiative(c.Initiative, c.Reaction)));
                c.curInitiative = r.Initative;
                c.label.Text = c.getLabel() + " (" + r.Initative + ")";
            }
            List<CharacterClass> allList = new List<CharacterClass>();
            allList.AddRange(goodList);
            allList.AddRange(badList);
            InitiativeOrder = new List<CharacterClass>();
            InitiativeOrder = allList.OrderByDescending(x => x.curInitiative).ToList();
            CreateGoButton();
        }

        void changeModeEvent(object sender, EventArgs e)
        {
            int i = 0;
            ComboBox combo = (ComboBox)sender;
            CharacterClass targetChar = (CharacterClass)combo.Tag;
            List<CharacterClass> cList = isGood(targetChar) ? goodList : badList;
            foreach(CharacterClass c in cList)
            {
                if (c.weaponList == sender)
                {
                    c.modeList.Items.Clear();
                    foreach(DamageClass d in c.damageList)
                    {
                        if (d.Name != (string)combo.SelectedItem) continue;
                        if (d.fullAuto) c.modeList.Items.Add("Full Auto");
                        if (d.semi) c.modeList.Items.Add("Semi-Automatic");
                        if (d.single) c.modeList.Items.Add("Single");
                        if (d.melee) c.modeList.Items.Add("Melee");
                        if (d.spell) c.modeList.Items.Add("Spell");
                        break;
                    }
                    break;
                }
            }
            //
        }

        void CreateTargetButtons(CharacterClass c)
        {
            if (isGood(c))
            {
                for(int i = 0;i < badList.Count;i++)
                {
                    Button b = new Button();
                    b.Tag = badList[i];
                    b.Text = "T";
                    b.Location = new Point(6, badList[i].label.Location.Y);
                    b.Width = 30;
                    b.Click += goTargetEvent;
                    groupBox2.Controls.Add(b);
                    targetButtons.Add(b);
                }
            } else
            {
                for (int i = 0; i < goodList.Count; i++)
                {
                    Button b = new Button();
                    b.Tag = goodList[i];
                    b.Text = "T";
                    b.Location = new Point(6, goodList[i].label.Location.Y);
                    b.Width = 30;
                    b.Click += goTargetEvent;
                    groupBox1.Controls.Add(b);
                    targetButtons.Add(b);
                }
            }
        }

        void CreateGoButton()
        {
            lblTarget.Text = "";
            foreach(Button b in targetButtons)
            {
                groupBox1.Controls.Remove(b);
                groupBox2.Controls.Remove(b);
            }
            targetButtons.Clear();
            goButton.Click -= goClickEvent;
            goButton = new Button();
            goButton.Tag = InitiativeOrder[0];
            Point p = InitiativeOrder[0].label.Location;
            p.X = p.X - 40;
            goButton.Location = p;
            goButton.Text = "GO";
            goButton.Width = 40;
            goButton.Click += new EventHandler(goClickEvent);
            if (isGood(InitiativeOrder[0]))
                groupBox1.Controls.Add(goButton);
            else
                groupBox2.Controls.Add(goButton);
            EventClass.LogEvent(InitiativeOrder[0].Name + "'s turn.");
            CreateTargetButtons(InitiativeOrder[0]);
        }
        void goClickEvent(object sender,EventArgs e)
        {
            Button b = (Button)sender;
            CharacterClass c = (CharacterClass)b.Tag;
            if (string.IsNullOrEmpty((string)c.weaponList.SelectedItem))
            {
                MessageBox.Show("Select a weapon for " + c.Name);
                c.weaponList.Focus();
                return;
            }
            if (string.IsNullOrEmpty((string)c.modeList.SelectedItem))
            {
                MessageBox.Show("Select a weapon mode for " + c.Name);
                c.modeList.Focus();
                return;
            }
            if (string.IsNullOrEmpty((string)lblTarget.Text))
            {
                MessageBox.Show("Select a target for " + c.Name);
                return;
            }
            resolveCombat(c, target);
            InitiativeOrder.Remove(c);
            b.Click -= goClickEvent;
            groupBox1.Controls.Remove(b);
            groupBox2.Controls.Remove(b);
            if (InitiativeOrder.Count == 0)
            {
                newInitiative();
            }
            if (InitiativeOrder.Count == 0)
            {
                EventClass.LogEvent("Initiative is now resolved, combat round is over.");
                return;
            }
            CreateGoButton();
               
        }

        void goTargetEvent(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            CharacterClass c = (CharacterClass)b.Tag;
            lblTarget.Text = c.Name;
            target = c;
        }

        void resolveCombat(CharacterClass c, CharacterClass t)
        {
            int i = 0;
            DamageClass weapon = new DamageClass();
            foreach(DamageClass dam in c.damageList)
            {
                if (dam.Name == c.weaponList.SelectedItem.ToString())
                {
                    weapon = dam;
                    break;
                }
            }
        }

        // ============================================================================================================================
        // EVENT HANDLERS
        //

        // ----------------------------------------------------------------------------------------------------------------------------
        // EVENTCLASS EVENT
        //
        void EventLog(DateTime dateTime, EventLogArgs eventLogArgs, string fileName = null)
        {
            try
            {
                string dateTimeString = string.Format("{0:MM/dd/yy hh:mm:ss.fff tt}", dateTime);
                //format:  Date, Job, Message 
                string jobName = "                ";

                string messageString;
                if (eventLogArgs.isException)
                {
                    messageString = String.Format("{0}\t\t{1}\t{2}\t\tEXCEPTION: {3}:{4} - [{5}] {6}", dateTimeString, eventLogArgs.propertyLogEntryType.ToString(), jobName, eventLogArgs.propertyClassName, eventLogArgs.propertyMethodName, eventLogArgs.propertyExceptionKey, eventLogArgs.propertyException.Message);
                    string errorMessageString = String.Format("{0}\t\t{1}\t\t{2} --> {3}", dateTimeString, eventLogArgs.propertyExceptionKey, eventLogArgs.propertyException.Message, eventLogArgs.propertyException.StackTrace);
                    LogWriteStack(dateTime, errorMessageString);
                }
                else
                {
                    messageString = String.Format("{0}\t\t{1}\t{2}\t\t{3}", dateTimeString, eventLogArgs.propertyLogEntryType.ToString(), jobName, eventLogArgs.propertyMessage);
                }


                LogWriteTextBox(dateTime, messageString);
                LogWriteFile(dateTime, messageString, fileName);
            }

            catch (Exception ex)
            {
                LogWriteTextBox(DateTime.Now, DateTime.Now.ToString("0:MM/dd/yy hh:mm:ss.fff tt") + "     EXCEPTION: " + ex.Message);
                LogWriteFile(DateTime.Now, DateTime.Now.ToString("0:MM/dd/yy hh:mm:ss.fff tt") + "     EXCEPTION: " + ex.Message);
            }

        }


        // ----------------------------------------------------------------------------------------------------------------------------
        // WRITE LINE TO LOG TEXTBOX - THREADSAFE
        //
        private void LogWriteTextBox(DateTime dateTime, string messageString, string fileName = null)
        {

            try
            {
                if (LogTextBox.InvokeRequired)
                {
                    EventClass.EventHandler d = new EventClass.EventHandler(LogWriteTextBox);
                    LogTextBox.Invoke(d, new object[] { dateTime, messageString, fileName });
                    return;
                }

                // CLEAR SOME LINES FROM THE TOP OF THE LOG
                if (LogTextBox.Text.Length > 30000) LogTextBox.Text = LogTextBox.Text.Substring(10000);

                // ADD LINE AND SCROLL TO END
                LogTextBox.AppendText(messageString + "\r\n");

                if (bScroll)
                {
                    //LogTextBox.CaretIndex = LogTextBox.Text.Length;
                    //LogTextBox.ScrollToEnd();
                }
            }

            catch (Exception ex)
            {
                //LogWriteTextBox(DateTime.Now, DateTime.Now.ToString("0:MM/dd/yy hh:mm:ss.fff tt") + "     EXCEPTION: " + ex.Message);
                LogWriteFile(DateTime.Now, DateTime.Now.ToString("0:MM/dd/yy hh:mm:ss.fff tt") + "     EXCEPTION: " + ex.Message);
            }

        }

        // ----------------------------------------------------------------------------------------------------------------------------
        // WRITE LINE TO LOG FILE
        //
        private void LogWriteFile(DateTime dateTime, string messageString, string fileName = null)
        {

            try
            {
                //check if logs dir exists
                String logsDir = ".\\logs";
                if (!Directory.Exists(logsDir)) Directory.CreateDirectory(logsDir);
                if (!string.IsNullOrEmpty(fileName))
                {
                    if (!Directory.Exists(Path.GetDirectoryName(fileName))) Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                }
                if (this.InvokeRequired)
                {
                    EventClass.EventHandler d = new EventClass.EventHandler(LogWriteFile);
                    this.Invoke(d, new object[] { dateTime, messageString, fileName });
                    return;
                }

                File.AppendAllText
                (
                    logsDir + "\\sr." + string.Format("{0:yyMMdd}", dateTime) + ".log",
                    messageString + "\r\n"
                );
                if (!string.IsNullOrEmpty(fileName))
                {
                    File.AppendAllText(fileName, messageString + "\r\n");
                }
            }

            catch (Exception ex)
            {
                LogWriteTextBox(DateTime.Now, DateTime.Now.ToString("0:MM/dd/yy hh:mm:ss.fff tt") + "     EXCEPTION: " + ex.Message);
                //LogWriteFile(DateTime.Now, DateTime.Now.ToString("0:MM/dd/yy hh:mm:ss.fff tt") + "     EXCEPTION: " + ex.Message);
            }

        }

        // ----------------------------------------------------------------------------------------------------------------------------
        // WRITE LINE TO LOG FILE
        //
        private void LogWriteStack(DateTime dateTime, string messageString, string fileName = null)
        {

            try
            {
                //check if logs dir exists
                String logsDir = ".\\logs\\exeptions";
                if (!Directory.Exists(logsDir)) Directory.CreateDirectory(logsDir);

                if (this.InvokeRequired)
                {
                    EventClass.EventHandler d = new EventClass.EventHandler(LogWriteStack);
                    this.Invoke(d, new object[] { dateTime, messageString, fileName });
                    return;
                }

                File.AppendAllText
                (
                    logsDir + "\\sr.trace." + string.Format("{0:yyMMdd}", dateTime) + ".log",
                    messageString + "\r\n"
                );
            }

            catch (Exception ex)
            {
                LogWriteTextBox(DateTime.Now, DateTime.Now.ToString("0:MM/dd/yy hh:mm:ss.fff tt") + "     EXCEPTION: " + ex.Message);
                //LogWriteFile(DateTime.Now, DateTime.Now.ToString("0:MM/dd/yy hh:mm:ss.fff tt") + "     EXCEPTION: " + ex.Message);
            }

        }

    }
}
