using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using Microsoft.VisualBasic;
using System.Media;

namespace To_Do_Lists
{
    public partial class Form1 : Form
    {



        public enum enPriority { UrgentImportant = 0, ImportantNotUrgent = 1, UrgentNotImportant = 2, Routine = 3 }

        public class ClsTaskItems
        {
            public string TaskName { get; set; }
            public enPriority Priority { get; set; }
        }

        Timer focusTimer = new Timer();
        Label lblBigTimer;
        int remainingSeconds = 0;
        Label lblMotivation;
      
       
        string[] deepQuotes = {
                                  "«·‰Ã«Õ ·« ÌÕ «Ã ≈·Ï √ﬁœ«„° »· ≈·Ï ≈ﬁœ«„.",
                                  "«·—«Õ… ›Œ° Ê«·„Ãœ Ìﬂ„‰ ›Ì «· ›«’Ì· «· Ì ÌÂ—» „‰Â« «·Ã„Ì⁄.",
                                  "√‰  «·¬‰  ﬂ »  «—ÌŒﬂ «·Œ«’ »·€… «·ﬂÊœ° ›«Ã⁄·Â  «—ÌŒ« ⁄ŸÌ„«.",
                                  " –ﬂ— ·„«–« »œ√ .. «·ﬁ„… „“œÕ„… »«·‰«ÃÕÌ‰° ·ﬂ‰Â«   ”⁄ ··√ﬁÊÏ.",
                                  "Œ·› ﬂ· ”ÿ— »—„Ã ﬂ›«Õ ·« Ì—«Â «·‰«”° ”ÌŒ—Ã ⁄„·ﬂ ··‰Ê— ﬁ—Ì»«.",
                                  "»Ì‰„« Ì‰«„ «·¬Œ—Ê‰° √‰   ’„„ „” ﬁ»·ﬂ. «·›—ﬁ ÂÊ '«·¬‰'.",
                                  "«·⁄Ÿ„«¡ ·« Ì‰”Õ»Ê‰° «·√Œÿ«¡ ÂÌ œ—Ê” „€·›… »«· ÕœÌ.",
                                  " ŒÌ· ·ÕŸ… «·‹ 'Run' «·‰«ÃÕ…..  ·ﬂ «··–…  ” Õﬁ ﬂ· –—…  ⁄».",
                                  "·ﬁœ ﬁÿ⁄  ‘Êÿ« ÿÊÌ·« ·  Êﬁ› «·¬‰. «” —Õ · ⁄Êœ √ﬁÊÏ."
        };

       
        string scheduledRestMessage = "Ì« ’œÌﬁÌ° «·—«Õ… «· Ì  √Œ–Â« «·¬‰ ·Ì”  Â—Ê»«° »· ÂÌ '≈⁄«œ…  ‘€Ì·' ·⁄ﬁ·ﬂ «·„»œ⁄. " +
                                     "«·⁄«·„ „‰ ÕÊ·ﬂ €«—ﬁ ›Ì «·÷ÃÌÃ° Ê√‰  Â‰«°  »‰Ì „‰ «·⁄œ„ Ê«ﬁ⁄« —ﬁ„Ì« „–Â·«. " +
                                     " –ﬂ— √‰ «·„Ãœ Ìı’‰⁄ ›Ì  ·ﬂ «·”«⁄«  «·’«„ …° Œ·› «·‘«‘« ° ÕÌÀ ·« Ì—«ﬂ √Õœ ”ÊÏ ≈’—«—ﬂ. " +
                                     "√‰  ·”  „Ã—œ „»—„Ã° √‰  „Â‰œ” ·ÕÌ«… √”Â· ·€Ì—ﬂ. " +
                                     "«” ‰‘ﬁ »⁄„ﬁ° «” ⁄œ ‘€›ﬂ° À„ ⁄œ · À»  ··Ã„Ì⁄ √‰ √Õ·«„ﬂ √ﬂ»— „‰ √Ì 'Bug' ﬁœ ÌÊ«ÃÂﬂ.";

        bool isBreakMode = false;

        Random rand = new Random();


        public List<TableLayoutPanel> list = new List<TableLayoutPanel>();

        public Form1()
        {
            EnableDoubleBuffering(this);

            InitializeComponent();
            //  ›⁄Ì· «· Œ“Ì‰ «·„ƒﬁ  «·„“œÊÃ
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint |
                          ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
            this.WindowState = FormWindowState.Maximized;
            this.DoubleBuffered = true;



          

        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            SortTasksByTime();


            lblprogres.ForeColor = Color.FromArgb(255, 140, 0);
            label3.ForeColor = Color.FromArgb(255, 140, 0);
            lblTasksHave.ForeColor = Color.FromArgb(255, 140, 0);
            lblTasksFinshed.ForeColor = Color.FromArgb(0, 190, 255);


            OudaiUltraTheme.ApplyFullTheme(this);
            maskStartDate.BackColor = Color.FromArgb(20, 20, 20);
            maskStartDate.ForeColor = Color.White;
            maskStartDate.BorderStyle = BorderStyle.FixedSingle;


            maskEndDate.BackColor = Color.FromArgb(20, 20, 20);
            maskEndDate.ForeColor = Color.White;
            maskEndDate.BorderStyle = BorderStyle.FixedSingle;


            button1.BackColor = Color.Transparent;
            lblprogres.Text = progressBar1.Value.ToString() + "%:";
            lblTableTime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");

            floShowTasks.AutoScroll = true;

            SetupBigTimer();
            panel4.BackColor = Color.Transparent;
            EnableDoubleBuffering(floShowTasks);
            EnableDoubleBuffering(flowShowUpdateTasks);
            UpdateTaskLabels();
            LoadTasks();
        }


        private void ApplyStyle(CheckBox chk, TableLayoutPanel table)
        {
            var lblTask = table.Controls.Find("TasksName", true).FirstOrDefault() as Label;
            var lblTime = table.Controls.Find("TasksTime", true).FirstOrDefault() as Label;

            if (chk.Checked)
            {
                // ?? Œ·›Ì…  ÊÂÃ √“—ﬁ œ«ﬂ‰
                table.BackColor = Color.FromArgb(10, 25, 50);

                // ? ·Ê‰ ‰’ „ ÊÂÃ
                if (lblTask != null)
                {
                    lblTask.ForeColor = Color.DeepSkyBlue;
                    lblTask.Font = new Font(lblTask.Font, FontStyle.Strikeout);
                }

                if (lblTime != null)
                {
                    lblTime.ForeColor = Color.DeepSkyBlue;
                }
            }
            else
            {
                // —ÃÊ⁄ ··Ê÷⁄ «·ÿ»Ì⁄Ì
                table.BackColor = Color.FromArgb(20, 20, 20);

                if (lblTask != null)
                {
                    lblTask.ForeColor = Color.White;
                    lblTask.Font = new Font(lblTask.Font, FontStyle.Regular);
                }

                if (lblTime != null)
                {
                    lblTime.ForeColor = Color.White;
                }
            }
        }

        private void Setting(TableLayoutPanel Table, Label lblTaskName)
        {

            ContextMenuStrip TaskMenu = new ContextMenuStrip();
            ToolStripMenuItem DeleteItem = new ToolStripMenuItem("Õ–› «·„Â„…");
            ToolStripMenuItem EditItem = new ToolStripMenuItem(" ⁄œÌ· «·„Â„…");

            DeleteItem.ForeColor = Color.Red;

            TaskMenu.Items.Add(DeleteItem);
            TaskMenu.Items.Add(new ToolStripSeparator());
            TaskMenu.Items.Add(EditItem);


            Button btnOptions = new Button();
            btnOptions.Name = "btnOptions";
            btnOptions.Font = new Font("Segoe UI Symbol", 16F, FontStyle.Bold);
            btnOptions.Text = "\u22EE";
            btnOptions.ForeColor = Color.White;
            btnOptions.BackColor = Color.Transparent;
            btnOptions.Dock = DockStyle.Fill;
            btnOptions.FlatStyle = FlatStyle.Flat;
            btnOptions.FlatAppearance.BorderSize = 0;
            btnOptions.Size = new Size(30, 30);
            btnOptions.Cursor = Cursors.Hand;
            btnOptions.Dock = DockStyle.Fill;


            btnOptions.Click += (S, e) =>
            {
                TaskMenu.Show(btnOptions, new Point(0, btnOptions.Height));
            };


            DeleteItem.Click += (s, e) =>
            {
                var result = MessageBox.Show("Â·  —Ìœ Õ–› Â–Â «·„Â„…ø", " ‰»ÌÂ", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    floShowTasks.Controls.Remove(Table);
                    Table.Dispose();
                    UpdateTaskLabels();
                    SaveTasks();
                }
            };



            EditItem.Click += (s, e) =>
            {
                // «” œ⁄«¡ ’‰œÊﬁ «·≈œŒ«· ›Ì ”ÿ— Ê«Õœ
                string input = Interaction.InputBox("√œŒ· «·«”„ «·ÃœÌœ ··„Â„…:", " ⁄œÌ·", lblTaskName.Text);

                // ≈–« ·„ Ì÷€ÿ «·„” Œœ„ ≈·€«¡ (Cancel) Ê·„ Ì —ﬂ «·‰’ ›«—€«
                if (!string.IsNullOrEmpty(input))
                {
                    lblTaskName.Text = input;
                    SaveTasks();
                }
            };

            Table.Controls.Add(btnOptions, 0, 0);

        }

        private void SpecifyTaskTime()
        {

            if (floShowTasks.Controls.Count == 0)
            {
                MessageBox.Show("·«  ÊÃœ „Â«„ · ⁄œÌ· Êﬁ Â«° Ì—ÃÏ ≈÷«›… „Â«„ √Ê·«.", " ‰»ÌÂ");
                return;
            }

            Form f = new Form()
            {
                Text = " Œ’Ì’ Êﬁ  «·„Â„…",
                Size = new Size(400, 250),
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(20, 20, 20),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

          

            Label lblInfo = new Label
            {
                Text = ": √·„Â„… «·ÃœÌœ…",
                ForeColor = Color.White,
                Top = 30,
                Left = 260, 
                Width = 100,
                TextAlign = ContentAlignment.MiddleRight,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold)

            };


            Label lblTimeInfo = new Label
            {
                Text = ": √·Êﬁ  «·ÃœÌœ",
                ForeColor = Color.White,
                Top = 80,
                Left = 260, 
                Width = 100,
                TextAlign = ContentAlignment.MiddleRight,
               Font = new Font("Segoe UI", 10F, FontStyle.Bold)

            };

          
            ComboBox comboTasks = new ComboBox
            {
                Top = 30,
                Left = 40,
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
            };

            MaskedTextBox txtMaskedTime = new MaskedTextBox();
            txtMaskedTime.Top = 80;
            txtMaskedTime.Left = 40;
            txtMaskedTime.Width = 200;
            txtMaskedTime.Mask = "00:00  -  00:00";
            txtMaskedTime.BackColor = Color.FromArgb(45, 45, 45);
            txtMaskedTime.ForeColor = Color.Cyan;
            txtMaskedTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtMaskedTime.TextAlign = HorizontalAlignment.Center; 

            Button btnSave = new Button
            {
                Text = " ÕœÌÀ «·Êﬁ ",
                Top = 150,
                Left = 40,
                Width = 300,
                Height = 40,
                BackColor = Color.FromArgb(0, 150, 136),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)

            };

            btnSave.FlatAppearance.BorderSize = 0;

           
            foreach (Control ctrl in floShowTasks.Controls)
            {
                if (ctrl is TableLayoutPanel table)
                {
                    Control[] found = table.Controls.Find("TasksName", true);
                    if (found.Length > 0) comboTasks.Items.Add(found[0].Text);
                }
            }
            if (comboTasks.Items.Count > 0) comboTasks.SelectedIndex = 0;

            btnSave.Click += (s, ev) => {
                if (!txtMaskedTime.MaskCompleted)
                {
                    MessageBox.Show("Ì—ÃÏ ≈œŒ«· «·Êﬁ  ﬂ«„·« (√—ﬁ«„ ›ﬁÿ).", "Œÿ√ ›Ì «·≈œŒ«·");
                    return;
                }


                foreach (Control ctrl in floShowTasks.Controls)
                {
                    if (ctrl is TableLayoutPanel table)
                    {
                        Control[] nameFound = table.Controls.Find("TasksName", true);
                        if (nameFound.Length > 0 && nameFound[0].Text == comboTasks.Text)
                        {
                            Control[] timeFound = table.Controls.Find("TasksTime", true);
                            if (timeFound.Length > 0)
                            {
                                timeFound[0].Text = txtMaskedTime.Text;
                                SaveTasks();
                                f.Close();
                                return;
                            }
                        }
                    }
                }
            };

            f.Controls.AddRange(new Control[] { lblInfo, comboTasks, lblTimeInfo, txtMaskedTime, btnSave });
            f.ShowDialog();
        }

        private void SetupBigTimer()
        {
            // ﬂÊœ «·„ƒﬁ  «·ﬂ»Ì— (‰›”Â «·”«»ﬁ)
            lblBigTimer = new Label
            {
                Text = "00:00",
                ForeColor = Color.Cyan,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 60F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                Visible = false
            };

            // ≈÷«›… Label «·—”«∆· «· Õ›Ì“Ì…
            lblMotivation = new Label
            {
                Text = "«»œ√ «·„Â„… «·¬‰!",
                ForeColor = Color.Gray,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 14F, FontStyle.Italic),
                TextAlign = ContentAlignment.TopCenter, 
                Dock = DockStyle.Bottom, 
                Height = 150,
                Padding = new Padding(0, 0, 0, 50),
                Visible = false
            };

            this.Controls.Add(lblBigTimer);
            this.Controls.Add(lblMotivation);
            lblBigTimer.BringToFront();
            lblMotivation.BringToFront();
        }

        private void ToggleUI(bool enable)
        {
            // ≈ŸÂ«— «·„ƒﬁ  «·ﬂ»Ì— ›Ì «·„‰ ’› ⁄‰œ «·ﬁ›·
            if (lblBigTimer != null)
            {
                lblBigTimer.Visible = !enable;
                lblMotivation.Visible = !enable;
                lblBigTimer.BringToFront();
            }

            // ﬁ›· Ê› Õ √“—«— «· Õﬂ„
            butAddTasks.Enabled = enable;
            button1.Enabled = enable;
            btnSpecifyTime.Enabled = enable;
            TxtTasksName.Enabled = enable;

            // ≈Œ›«¡ ﬁ«∆„… «·„Â«„ ·“Ì«œ… «· —ﬂÌ“ («Œ Ì«—Ì)
            floShowTasks.Visible = enable;

            if (!enable)
                this.Text = "? Ê÷⁄ «· —ﬂÌ“ Ì⁄„·... ·«   ‘  !";
            else
                this.Text = "To Do List";
        }

        private void StopFocusMode()
        {
            // ≈⁄«œ… ≈ŸÂ«— ⁄‰«’— «· Õﬂ„ «·√’·Ì…
            ToggleUI(true);

            // ≈Œ›«¡ ⁄‰«’— Ê÷⁄ «· —ﬂÌ“
            lblBigTimer.Visible = false;
            lblMotivation.Visible = false;

            //  ’›Ì— «·„ƒﬁ 
            remainingSeconds = 0;
        }

        private void FocusTimer_Tick(object sender, EventArgs e)
        {
            if (remainingSeconds > 0)
            {
                remainingSeconds--;
                TimeSpan t = TimeSpan.FromSeconds(remainingSeconds);
                lblBigTimer.Text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

                if (remainingSeconds % 5 == 0) //  €ÌÌ— «·«ﬁ »«” ﬂ· 5 ÀÊ«‰Ú
                {
                    lblMotivation.Text = deepQuotes[rand.Next(deepQuotes.Length)];
                }

                //  √ÀÌ— «·‰»÷ (Pulse)
                if (remainingSeconds % 2 == 0)
                {
                    lblBigTimer.ForeColor = isBreakMode ? Color.SpringGreen : Color.Cyan;
                }
                else
                {
                    lblBigTimer.ForeColor = isBreakMode ? Color.ForestGreen : Color.FromArgb(0, 150, 200);
                }
            }
            else
            {
                focusTimer.Stop();
                SystemSounds.Exclamation.Play();
                StopFocusMode();

                // ≈ŸÂ«— „·Œ’ «·≈‰Ã«“ »⁄œ «‰ Â«¡ «·⁄„· √Ê «·«” —«Õ…
                ShowDailySummary();
                isBreakMode = false;
            }
        }

        private void ShowDailySummary()
        {
            int total = floShowTasks.Controls.Count;
            int completed = 0;

            foreach (Control ctrl in floShowTasks.Controls)
            {
                if (ctrl is TableLayoutPanel table)
                {
                    var chk = table.Controls.OfType<CheckBox>().FirstOrDefault();
                    if (chk != null && chk.Checked) completed++;
                }
            }

            int successRate = (total > 0 ? (completed * 100 / total) : 0);

            // —„Ê“ «·—”„ «·’‰œÊﬁÌ (Double Line) · ⁄ÿÌ ›Œ«„… ··≈ÿ«—
            string tl = "\u2554"; // ?
            string tr = "\u2557"; // ?
            string bl = "\u255A"; // ?
            string br = "\u255D"; // ?
           // string hor = "\u2550"; // ?
            string ver = "\u2551"; // ?
            string line = new string('\u2550', 25); // Œÿ √›ﬁÌ ÿÊÌ·

            // «·—„Ê“ «· ⁄»Ì—Ì… (Emojis) »«” Œœ«„ «·‹ Surrogates ·÷„«‰ «·√·Ê«‰
            string iconChart = "\uD83D\uDCCA"; // ??
          //  string iconCheck = "\u2705"; // ?
            string iconTrend = "\uD83D\uDCC8"; // ??
            string iconStar = "\uD83C\uDF1F"; // ??
            string iconFire = "\uD83D\uDD25"; // ??
            string iconTarget = "\uD83C\uDFAF"; // ??

            // —”«∆·  Õ›Ì“Ì… „ €Ì—… »‰«¡ ⁄·Ï «·√œ«¡
            string motivation = (completed == total && total > 0)
                ? $"{iconStar} √œ«¡ «” À‰«∆Ì! ·ﬁœ «ﬂ ”Õ  „Â«„ «·ÌÊ„ »«·ﬂ«„·° √‰  ÊÕ‘ »—„Ã…! {iconStar}"
                : $"{iconFire} ⁄„· —«∆⁄! «” „— ›Ì «· ﬁœ„° ›«·„Ãœ Ìı»‰Ï ŒÿÊ… »ŒÿÊ… {iconFire}";

            // »‰«¡ ‰’ «·—”«·… » ‰”Ìﬁ „— » Ãœ«
            string summary =
                $"{tl}{line}{tr}\n" +
                $"{ver}    {iconChart}  „‹·‹Œ‹’ «·≈‰Ã‹«“ «·‹Ì‹Ê„‹Ì  {iconChart}    {ver}\n" +
                $"{bl}{line}{br}\n\n" +
                $"{iconTarget}   Õ‹«·‹… «·‹„‹Â‹«„  :   {completed} / {total}\n" +
                $"{iconTrend}   ‰‹”‹»‹… «·‹‰‹Ã‹«Õ :   %{successRate}\n\n" +
                "??????????????????????????\n" +
                $"{motivation}";

            // ·⁄—÷ «·—”«·… »ÕÃ„ Œÿ √ﬂ»— Ê‘ﬂ· „‰”ﬁ (RTL)
            MessageBox.Show(summary,
                            "‰Ÿ«„ ≈œ«—… «·„Â«„ - «·„—«Ã⁄… «·ÌÊ„Ì…",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.None, // ≈“«·… «·√ÌﬁÊ‰… «·«› —«÷Ì… · Ê”Ì⁄ „”«Õ… «·‰’
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
        }

        private void AddNewTasks(string TasksName, string tasksTime, bool IsChecked = false)
        {




            TableLayoutPanel Table = new TableLayoutPanel();
            Table.ColumnCount = 4;
            Table.RowCount = 1;
            Table.Size = new Size(680, 70);
            Table.Dock = DockStyle.Top;
            Table.BackColor = Color.FromArgb(20, 20, 20);
            Table.Padding = new Padding(5);
            Table.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;


            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 40F));  // Settings Button
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));   // Time
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));   // TasksName
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));   // Checkked 



            Label lblTime = new Label();

            lblTime.Name = "TasksTime";
            lblTime.Text = tasksTime;
            lblTime.ForeColor = Color.White;
            lblTime.Size = new Size(210, 50);
            lblTime.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTime.TextAlign = ContentAlignment.MiddleCenter;
            lblTime.Dock = DockStyle.Fill;


            Label lblTaskName = new Label();
            lblTaskName.Name = "TasksName";
            lblTaskName.Text = TasksName;
            lblTaskName.ForeColor = Color.White;
            lblTaskName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTaskName.TextAlign = ContentAlignment.MiddleRight;
            lblTaskName.Dock = DockStyle.Fill;


            CheckBox chkDone = new CheckBox();
            chkDone.Checked = IsChecked;
            chkDone.Dock = DockStyle.Fill;
            chkDone.CheckAlign = ContentAlignment.MiddleCenter;
            chkDone.Cursor = Cursors.Hand;

            chkDone.CheckedChanged += (s, e) =>
            {
                CheckBox chk = (CheckBox)s;

                ApplyStyle(chk, Table);
                UpdateTaskLabels();
                SaveTasks();
            };



            //  ApplyStyle(chkDone, Table);

            Setting(Table, lblTaskName);

            Table.Controls.Add(lblTime, 1, 0);
            Table.Controls.Add(lblTaskName, 2, 0);
            Table.Controls.Add(chkDone, 3, 0);




            ApplyStyle(chkDone, Table);

            if (floShowTasks.Controls.Count >= 10)
            {
                MessageBox.Show("·ﬁœ  Ã«Ê“  «·Õœ «·«ﬁ’∆ ·«÷«›«  „Â«„ ··ÌÊ„");
                return;
            }


            floShowTasks.Controls.Add(Table);
            UpdateTaskLabels();

            // floShowTasks.Controls.SetChildIndex(Table, 0);

        }


        private void AddTasksWithComboBox(string TasksName, string tasksTime)
        {


            TableLayoutPanel Table = new TableLayoutPanel();
            Table.ColumnCount = 3;
            Table.RowCount = 1;
            Table.Size = new Size(800, 55);
            Table.Dock = DockStyle.Top;
            Table.BackColor = Color.FromArgb(20, 20, 20);
            Table.Padding = new Padding(5);
            Table.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;



            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            Table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));


            Label lblTime = new Label();
            lblTime.Name = "TasksTime";
            lblTime.Text = tasksTime;
            lblTime.ForeColor = Color.White;
            lblTime.Size = new Size(210, 50);
            lblTime.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTime.TextAlign = ContentAlignment.MiddleCenter;
            lblTime.Dock = DockStyle.Fill;



            Label lblTaskName = new Label();
            lblTaskName.Name = "TasksName";
            lblTaskName.Text = TasksName;
            lblTaskName.ForeColor = Color.White;
            lblTaskName.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTaskName.TextAlign = ContentAlignment.MiddleRight;
            lblTaskName.Dock = DockStyle.Fill;


            ComboBox combPriority = new ComboBox();
            combPriority.DropDownStyle = ComboBoxStyle.DropDownList;
            combPriority.Size = new Size(200, 30);
            combPriority.BackColor = Color.FromArgb(30, 30, 30);
            combPriority.ForeColor = Color.White;
            combPriority.FlatStyle = FlatStyle.Flat;
            combPriority.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            combPriority.Dock = DockStyle.Fill;
            combPriority.Font = new Font("Segoe UI Emoji", 10);


            combPriority.Items.AddRange(new string[]
            {
                    " „Â„ + ⁄«Ã·",
                    " „Â„ + €Ì— ⁄«Ã·",
                    " €Ì— „Â„ + ⁄«Ã·",
                    " €Ì— „Â„ + €Ì— ⁄«Ã·"
            });
            combPriority.SelectedIndex = 1;


            combPriority.SelectedIndexChanged += (s, e) =>
            {

                switch (combPriority.SelectedIndex)
                {

                    case 0:
                        Table.BackColor = Color.FromArgb(60, 20, 20);
                        combPriority.ForeColor = Color.Red;
                        break;


                    case 1:
                        Table.BackColor = Color.FromArgb(60, 60, 20);
                        combPriority.ForeColor = Color.Gold;
                        break;


                    case 2:
                        Table.BackColor = Color.FromArgb(20, 40, 60);
                        combPriority.ForeColor = Color.Cyan;
                        break;


                    case 3:
                        Table.BackColor = Color.FromArgb(30, 30, 30);
                        combPriority.ForeColor = Color.White;
                        break;

                }

                SortTasksByPriority();
            };


            Table.Controls.Add(lblTime, 1, 0);
            Table.Controls.Add(lblTaskName, 2, 0);
            Table.Controls.Add(combPriority, 0, 0);


            //if (floShowTasks.Controls.Count >= 10)
            //{
            //    MessageBox.Show("·ﬁœ  Ã«Ê“  «·Õœ «·«ﬁ’∆ ·«÷«›«  „Â«„ ··ÌÊ„");
            //    return;
            //}


            flowShowUpdateTasks.Controls.Add(Table);
            //flowShowUpdateTasks.Controls.SetChildIndex(Table, 0);


        }


        private void SortTasksByPriority()
        {
            // 1. «” Œ—«Ã ﬂ· «·„Â«„ „‰ ·ÊÕ… «· ÕœÌÀ Ê ÕÊÌ·Â« ·ﬁ«∆„… (List)
            var allTasks = flowShowUpdateTasks.Controls.OfType<TableLayoutPanel>().ToList();

            // 2.  — Ì» «·„Â«„ »‰«¡ ⁄·Ï «Œ Ì«— «·ﬂÊ„»Ê »Êﬂ” (0° À„ 1° À„ 2° À„ 3)
            var sortedTasks = allTasks.OrderBy(t =>
            {
                var combo = t.Controls.OfType<ComboBox>().FirstOrDefault();
                // ≈–« ·„ ÌÃœ ﬂÊ„»Ê »Êﬂ” Ì÷⁄Â« ›Ì «·√ŒÌ— (—ﬁ„ 99)
                return combo != null ? combo.SelectedIndex : 99;
            }).ToList();

            // 3. ≈Ìﬁ«› «·—”„ „ƒﬁ « · ”—Ì⁄ «·⁄„·Ì… Ê„‰⁄ «·Ê„Ì÷ (Flicker)
            flowShowUpdateTasks.SuspendLayout();

            // 4. „”Õ «· — Ì» «·ﬁœÌ„ „‰ «·Ê«ÃÂ…
            flowShowUpdateTasks.Controls.Clear();

            // 5. ≈⁄«œ… ≈÷«›… «·„Â«„ »«· — Ì» «·ÃœÌœ
            foreach (var task in sortedTasks)
            {
                // ≈÷«›… «·„Â„… ›Ì ‰Â«Ì… «·ﬁ«∆„… (”  — »  ·ﬁ«∆Ì« Œ·› »⁄÷Â«)
                flowShowUpdateTasks.Controls.Add(task);
            }

            // 6. «” ∆‰«› «·—”„
            flowShowUpdateTasks.ResumeLayout();

            // 7. Õ›Ÿ «· — Ì» «·ÃœÌœ ›Ì „·› JSON ›Ê—«

        }


        private void SortTasksByTime()
        {
            // 1. Ã·» Ã„Ì⁄ «·„Â«„ Ê ÕÊÌ·Â« ≈·Ï ﬁ«∆„…
            var tasks = floShowTasks.Controls.OfType<TableLayoutPanel>().ToList();

            // 2. «· — Ì» »‰«¡ ⁄·Ï Êﬁ  «·»œ«Ì…
            var sortedTasks = tasks.OrderBy(t =>
            {
                var lblTime = t.Controls.Find("TasksTime", true).FirstOrDefault() as Label;
                if (lblTime == null) return "99:99"; // ≈–« ·„ ÌÃœ Êﬁ « Ì÷⁄Â ›Ì «·√ŒÌ—

                // ‰› —÷ √‰ «·Êﬁ  »’Ì€… "03:00-05:00"° ‰√Œ– «·Ã“¡ «·√Ê· ›ﬁÿ "03:00"
                string startTime = lblTime.Text.Split('-')[0].Trim();
                return startTime;
            }).ToList();

            // 3.  ÕœÌÀ «·Ê«ÃÂ…
            floShowTasks.SuspendLayout(); // ≈Ìﬁ«› «·—”„ „ƒﬁ « ·„‰⁄ «·Ê„Ì÷
            floShowTasks.Controls.Clear();

            foreach (var task in sortedTasks)
            {
                floShowTasks.Controls.Add(task);
            }

            floShowTasks.ResumeLayout();
        }


        private void UpdateTaskLabels()
        {

            int FinshedTasks = 0;
            int HavenTasks = 0;

            foreach (Control Tasks in floShowTasks.Controls)
            {

                if (Tasks is TableLayoutPanel T)
                {
                    var Chk = T.Controls.OfType<CheckBox>().FirstOrDefault();

                    if (Chk != null)
                    {

                        if (Chk.Checked)
                        {
                            FinshedTasks++;

                        }
                        else
                        {
                            HavenTasks++;
                        }
                    }
                }

                lblTasksHave.Text = HavenTasks.ToString();
                lblTasksFinshed.Text = FinshedTasks.ToString();

            }




            int total = FinshedTasks + HavenTasks;
            if (total > 0)
            {
                progressBar1.Value = (int)((double)FinshedTasks / total * 100);
                lblprogres.Text = progressBar1.Value.ToString() + "%:";
            }
            else
            {
                progressBar1.Value = 0;
                lblprogres.Text = "0%:";
            }

        }

        private void SaveTasks()
        {

            List<TasksItems> TasksList = new List<TasksItems>();


            var activePanel = pnlUpdateTasks.Visible ? flowShowUpdateTasks : floShowTasks;

            foreach (Control Con in floShowTasks.Controls)
            {

                if (Con is TableLayoutPanel T)
                {

                    var TasksName = T.Controls.Find("TasksName", true).FirstOrDefault();
                    var TasksTime = T.Controls.Find("TasksTime", true).FirstOrDefault();


                    int priorityValue = 1;
                    var Combo = T.Controls.OfType<ComboBox>().FirstOrDefault();
                    if (Combo != null) priorityValue = Combo.SelectedIndex;

                    var Chk = T.Controls.OfType<CheckBox>().FirstOrDefault();


                    if (TasksName != null && TasksTime != null)

                    {


                        TasksList.Add(new TasksItems
                        {
                            TaskName = TasksName.Text,
                            TaskTime = TasksTime.Text,
                            IsCompleted = Chk != null ? Chk.Checked : false,
                            Priority = priorityValue

                        });


                    }
                }
            }

            string Tasks = JsonConvert.SerializeObject(TasksList, Formatting.Indented);

            File.WriteAllText("TasksList.json", Tasks);


        }


        private void LoadTasks()
        {

            if (File.Exists("TasksList.json"))
            {

                string Tasks = File.ReadAllText("TasksList.json");

                var TasksList = JsonConvert.DeserializeObject<List<TasksItems>>(Tasks);

                floShowTasks.Controls.Clear();

                foreach (var Task in TasksList)
                {

                    AddNewTasks(Task.TaskName, Task.TaskTime, Task.IsCompleted);

                }
            }
        }


        private void butAddTasks_Click_1(object sender, EventArgs e)
        {


            SortTasksByTime();
            string TasksDate = maskStartDate.Text + " - " + maskEndDate.Text;


            if (floShowTasks.Controls.Count >= 10)
            {
                MessageBox.Show("·ﬁœ  Ã«Ê“  «·Õœ «·√ﬁ’Ï ·≈÷«›«  „Â«„ «·ÌÊ„");
                return;
            }


            if (string.IsNullOrWhiteSpace(TxtTasksName.Text) || string.IsNullOrWhiteSpace(TasksDate))
            {
                MessageBox.Show("Ì—ÃÏ „·¡ Ã„Ì⁄ «·»Ì«‰«  ﬁ»· ≈÷«›… «·„Â„….");
                return;
            }

            string time = string.IsNullOrWhiteSpace(TasksDate) ?
                  DateTime.Now.ToString("hh:mm tt") : TasksDate;


            AddNewTasks(TxtTasksName.Text, TasksDate);

            SaveTasks();

            TxtTasksName.Clear();
            maskStartDate.Clear();
            maskEndDate.Clear();


        }


        private void button1_Click(object sender, EventArgs e)
        {
            pnlUpdateTasks.Visible = true;
            pnlUpdateTasks.BringToFront();

            flowShowUpdateTasks.SuspendLayout();
            flowShowUpdateTasks.Controls.Clear();

            foreach (Control table in floShowTasks.Controls)
            {

                if (table is TableLayoutPanel OldTable)
                {
                    var TasksName = OldTable.Controls.Find("TasksName", true).FirstOrDefault();
                    var TasksTime = OldTable.Controls.Find("TasksTime", true).FirstOrDefault();


                    if (TasksName != null && TasksTime != null)
                    {

                        AddTasksWithComboBox(TasksName.Text, TasksTime.Text);

                    }

                }

            }

            flowShowUpdateTasks.ResumeLayout();
        }

        private void btnSaveUpdateTasks_Click_1(object sender, EventArgs e)
        {

            flowShowUpdateTasks.SuspendLayout();
            floShowTasks.Controls.Clear();

            foreach (Control Table in flowShowUpdateTasks.Controls)
            {

                if (Table is TableLayoutPanel OldTable)
                {
                    var TasksName = OldTable.Controls.Find("TasksName", true).FirstOrDefault();
                    var TasksTime = OldTable.Controls.Find("TasksTime", true).FirstOrDefault();


                    if (TasksName != null && TasksTime != null)
                    {
                        AddNewTasks(TasksName.Text, TasksTime.Text);
                    }
                }
            }

            UpdateTaskLabels();
            SaveTasks();
            // SortTasksByTime();

            flowShowUpdateTasks.ResumeLayout();

            // pnlUpdateTasks.Visible = false;


            MessageBox.Show(" „  ÕœÌÀ «·√Ê·ÊÌ«  ÊÕ›Ÿ «·„Â«„ »‰Ã«Õ!");

        }

        private void btnLClosepanel_Click(object sender, EventArgs e)
        {

            pnlUpdateTasks.Visible = false;
        }

        private void btnSpecifyTime_Click(object sender, EventArgs e)
        {
            SpecifyTaskTime();
        }

        private void btnAntiDistraction_Click_1(object sender, EventArgs e)
        {
            //  √ﬂœ „‰ «” œ⁄«¡ SetupBigTimer() ›Ì Load ≈–« ·„  ﬂ‰ ﬁœ ›⁄· 
            if (lblBigTimer == null) SetupBigTimer();

            string input = Interaction.InputBox("ﬂ„ œﬁÌﬁ…  —Ìœ «· —ﬂÌ“ø", "„ƒﬁ  «· —ﬂÌ“", "25");

            if (int.TryParse(input, out int seconds))
            {
                remainingSeconds = seconds/* minutes * 60*/;
                ToggleUI(false);

                focusTimer.Interval = 1000;
                focusTimer.Tick -= FocusTimer_Tick; // · Ã‰»  ﬂ—«— «·—»ÿ
                focusTimer.Tick += FocusTimer_Tick;
                focusTimer.Start();
            }
        }

        private void EnableDoubleBuffering(Control control)
        {
            var property = typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            property.SetValue(control, true, null);

            foreach (Control child in control.Controls)
            {
                EnableDoubleBuffering(child);
            }
        }

        public static class OudaiMessageBox
        {
            public static void Show(string message, string title)
            {
                Form msgForm = new Form();
                msgForm.Size = new Size(700, 450); // ÕÃ„ «·‰«›–… ﬂ»Ì—
                msgForm.BackColor = Color.FromArgb(15, 15, 15); // Œ·›Ì… Noir
                msgForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                msgForm.StartPosition = FormStartPosition.CenterParent;
                msgForm.Text = title;
                msgForm.RightToLeft = RightToLeft.Yes;
                msgForm.RightToLeftLayout = true;

                // Label ·‰’ «·—”«·…
                Label lblMessage = new Label();
                lblMessage.Text = message;
                lblMessage.ForeColor = Color.White;
                lblMessage.Font = new Font("Segoe UI", 16F, FontStyle.Regular); // Œÿ ﬂ»Ì— ÊÊ«÷Õ
                lblMessage.Dock = DockStyle.Fill;
                lblMessage.TextAlign = ContentAlignment.MiddleCenter;
                lblMessage.Padding = new Padding(20);

                // “— «·≈€·«ﬁ » ‰”Ìﬁ Neon
                Button btnOk = new Button();
                btnOk.Text = "«” ⁄œ  ‘€›Ì° ·‰»œ√!";
                btnOk.Size = new Size(250, 50);
                btnOk.Dock = DockStyle.Bottom;
                btnOk.FlatStyle = FlatStyle.Flat;
                btnOk.FlatAppearance.BorderSize = 1;
                btnOk.FlatAppearance.BorderColor = Color.SpringGreen; //  ÊÂÃ √Œ÷—
                btnOk.ForeColor = Color.SpringGreen;
                btnOk.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
                btnOk.Cursor = Cursors.Hand;
                btnOk.Click += (s, e) => { msgForm.Close(); };

                msgForm.Controls.Add(lblMessage);
                msgForm.Controls.Add(btnOk);

                msgForm.ShowDialog();
            }
        }


        private void btnScheduledBreak_Click(object sender, EventArgs e)
        {
            isBreakMode = true;
            remainingSeconds = 300; // 5 œﬁ«∆ﬁ

             ToggleUI(false);
            lblBigTimer.Text = "05:00";
            lblBigTimer.ForeColor = Color.SpringGreen;

            
            focusTimer.Stop(); 
            focusTimer.Tick -= FocusTimer_Tick; 
            focusTimer.Tick += FocusTimer_Tick; 
            focusTimer.Interval = 1000;
            focusTimer.Start();

             OudaiMessageBox.Show(scheduledRestMessage, "??? ·ÕŸ… ≈œ—«ﬂ - Ì« ⁄œÌ");
        }


        private void UpdateProgress()
        {

            UpdateTaskLabels();
           
            int totalTasks = 3; 
            int completedTasks = 1;  

            int progress = (completedTasks * 100) / totalTasks;
            progressBar1.Value = progress;
            lblprogres.Text = $"{progress}%";
        }


        private void btnDailyReview_Click(object sender, EventArgs e)
        {
            ShowDailySummary();
        }



        private void btnPriority_Click(object sender, EventArgs e)
        {

            if (!File.Exists("TasksList.json")) return;

            string jsonData = File.ReadAllText("TasksList.json");

            var tasksList = JsonConvert.DeserializeObject<List<TasksItems>>(jsonData);

            if (tasksList == null || tasksList.Count == 0) return;


            var sortedTasks = tasksList.OrderBy(t => t.Priority).ToList();


            floShowTasks.SuspendLayout();
            floShowTasks.Controls.Clear();

            foreach (var task in sortedTasks)
            {
              
                AddNewTasks(task.TaskName, task.TaskTime, task.IsCompleted);
            }

            floShowTasks.ResumeLayout();

            SaveTasks();
        }

        private void btnDeleteTask_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Â· √‰  „ √ﬂœ „‰ Õ–› Ã„Ì⁄ «·„Â«„ «·Ÿ«Â—…ø",
                                            " √ﬂÌœ «·Õ–›",
                                            MessageBoxButtons.YesNo,
                                            MessageBoxIcon.Warning);


            if (result == DialogResult.Yes)
            {
                

                flowShowUpdateTasks.Controls.Clear();

                SaveTasks();
                UpdateTaskLabels();

                MessageBox.Show(" „ Õ–› Ã„Ì⁄ «·„Â«„ »‰Ã«Õ.", " „ «·⁄„·");
            }      
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
               
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }
    }
}

