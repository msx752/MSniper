using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace MSniper
{
    public class FConsole : RichTextBox
    {
        public FConsole()
        {
            InitializeFConsole();
        }

        public string[] Arguments { get; set; }

        public Color HyperlinkColor { get; set; }

        public ConsoleState Status { get; set; }

        public string Title { get { if (Parent != null) return Parent.Text; else return ""; } set { if (Parent != null) Parent.Text = value; } }

        private char CurrentKey { get; set; }

        private string CurrentLine { get; set; }

        private int CurrentPoint { get; set; }

        private bool InputEnable { get; set; }

        private bool Pause { get; set; }

        public void InitializeFConsole()
        {
            Name = "Console";
            Text = Name;
            Title = Name;
            Arguments = new string[0];
            BackColor = Color.Black;
            ForeColor = Color.Silver;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.None;
            ReadOnly = true;
            Font = new Font("consolas", 10);
            MinimumSize = new Size(410, 200);
            ScrollBars = RichTextBoxScrollBars.Vertical;
            Pause = false;
            InputEnable = false;
            if (Parent != null)
            {
                Parent.MinimumSize = MinimumSize;
                (Parent as Form).WindowState = FormWindowState.Normal;
                Parent.BackColor = BackColor;
            }
            DetectUrls = true;

            KeyDown -= FConsole_KeyDown;
            KeyDown += FConsole_KeyDown;

            TextChanged -= FConsole_TextChanged;
            TextChanged += FConsole_TextChanged;

            LinkClicked -= FConsole_LinkClicked;
            LinkClicked += FConsole_LinkClicked;


            MouseDown -= FConsole_MouseDown;
            MouseDown += FConsole_MouseDown;
        }

        private void FConsole_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ReadOnly = true;//true
                CurrentLine = Clipboard.GetText();
                InputEnable = false;//false
            }
        }

        public char ReadKey()
        {
            CurrentKey = ' ';
            CurrentPoint = Text.Length;
            InputEnable = !InputEnable;//true
            ReadOnly = !ReadOnly;//false
            Status = ConsoleState.ReadKey;
            while (InputEnable) Thread.Sleep(1);

            return CurrentKey;
        }

        public string ReadLine()
        {
            CurrentLine = "";
            CurrentPoint = TextLength;
            InputEnable = !InputEnable;//true
            ReadOnly = !ReadOnly;//false
            Status = ConsoleState.ReadLine;
            while (InputEnable) Thread.Sleep(1);
            Cursor = Cursors.IBeam;
            return CurrentLine;
        }

        public void SelectLastLine()
        {
            if (Lines.Count() > 0)
            {
                int line = Lines.Count() - 1;
                int s1 = GetFirstCharIndexFromLine(line);
                int s2 = line < Lines.Count() - 1 ?
                          GetFirstCharIndexFromLine(line + 1) - 1 :
                          Text.Length;
                Select(s1, s2 - s1);
            }
        }

        public void UpdateLine(int line, string message, Color? color = null)
        {
            ReadOnly = true;
            if (!color.HasValue)
                color = ForeColor;
            SelectLastLine();
            SetText(message, color);
        }

        public void Write(string message, Color? color = null)
        {
            while (Pause) Thread.Sleep(1);
            Select(TextLength, 0);
            SetText(message, color);
            DeselectAll();
        }
        public void SetText(string message, Color? color = null)
        {
            if (!color.HasValue)
                color = ForeColor;
            SelectionColor = color.Value;
            SelectedText = message;
        }
        public void WriteLine(string message, Color? color = null)
        {
            Write(message + Environment.NewLine, color);
        }

        private void FConsole_KeyDown(object sender, KeyEventArgs e)
        {
            if (Status == ConsoleState.Closing)
                return;

            Select(TextLength, 0);
            if ((int)e.KeyCode == (int)Keys.Enter && InputEnable == true && Status == ConsoleState.ReadLine)
            {
                Cursor = Cursors.WaitCursor;
                ReadOnly = true;
                CurrentLine = Lines[Lines.Count() - 1];
                InputEnable = false;
            }
            else if (InputEnable == true && Status == ConsoleState.ReadKey)
            {
                ReadOnly = true;
                CurrentKey = (char)e.KeyCode;
                InputEnable = false;
            }
            else if ((int)e.KeyCode == (int)Keys.Escape && InputEnable == false)//esc exit
            {
                Pause = false;
            }
            else if ((int)e.KeyCode == (int)Keys.Space && InputEnable == false)//space pause
            {
                Pause = !Pause;
            }
            else if ((int)e.KeyCode == (int)Keys.Back && ReadOnly == false && InputEnable == true && CurrentPoint + 1 > TextLength)
            {
                e.Handled = true;
            }
        }

        private void FConsole_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void FConsole_TextChanged(object sender, EventArgs e)
        {
            //string re1 = "((?:http|https)(?::\\/{2}[\\w]+)(?:[\\/|\\.]?)(?:[^\\s\"]*))";
            //Regex hyperlink = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            //bool update = false;
            //foreach (Match m in hyperlink.Matches(Text))
            //{
            //    Hyperlink hpl = new Hyperlink(m.Index, m.Index + m.Length, m.Value);
            //    if (Hyperlinks.Where(p => p.ToString() == hpl.ToString()).Count() == 0)
            //    {
            //        Select(m.Index, m.Length);
            //        SelectionColor = HyperlinkColor;
            //        Font f = SelectionFont;
            //        Font f2 = new Font(f.FontFamily, f.Size - 1.5f, FontStyle.Underline | FontStyle.Bold | FontStyle.Italic);
            //        SelectionFont = f2;
            //        Hyperlinks.Add(hpl);
            //        update = true;
            //    }
            //}
            //if (update)
            //    SelectLastLine();
        }
    }
}