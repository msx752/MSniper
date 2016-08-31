using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSniper
{
    public class FConsole : RichTextBox
    {
        public FConsole()
        {
            InitializeFConsole();
        }

        public void InitializeFConsole()
        {
            Name = "Console";
            Text = Name;
            Title = Name;
            Arguments = new string[0];
            BackColor = Color.Black;
            ForeColor = Color.SlateGray;
            Dock = DockStyle.Fill;
            BorderStyle = BorderStyle.None;
            ReadOnly = true;
            Font = new Font("consolas", 11);
            MinimumSize = new Size(450, 200);
            ScrollBars = RichTextBoxScrollBars.Vertical;
            Pause = false;
            InputEnable = false;
            if (Parent != null)
            {
                Parent.MinimumSize = MinimumSize;
                (Parent as Form).WindowState = FormWindowState.Normal;
                Parent.BackColor = BackColor;
            }
            KeyDown += FConsole_KeyDown;
            LinkClicked += FConsole_LinkClicked;
            TextChanged += FConsole_TextChanged;
            UseHyperlinkStyle = true;
            HyperlinkColor = Color.Red;
            HyperLinks = new List<Hyperlink>();
            HideSelection = true;
        }

        private void FConsole_KeyDown(object sender, KeyEventArgs e)
        {
            if (Status == ConsoleStatus.Closing)
                return;
            Select(TextLength, 0);
            if ((int)e.KeyCode == (int)Keys.Enter && InputEnable == true && Status == ConsoleStatus.ReadLine)
            {
                WriteLine("");
                ReadOnly = !ReadOnly;//true
                CurrentLine = Lines[GetLineFromCharIndex(CurrentPoint)];
                InputEnable = !InputEnable;//false
            }
            else if (InputEnable == true && Status == ConsoleStatus.ReadKey)
            {
                ReadOnly = !ReadOnly;//true
                CurrentKey = (char)e.KeyCode;
                InputEnable = !InputEnable;//false
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
            if (UseHyperlinkStyle)
            {
                string re1 = "((?:http|https)(?::\\/{2}[\\w]+)(?:[\\/|\\.]?)(?:[^\\s\"]*))";
                Regex hyperlink = new Regex(re1, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                bool update = false;
                foreach (Match m in hyperlink.Matches(Text))
                {
                    Hyperlink hpl = new Hyperlink(m.Index, m.Length, m.Value);
                    if (HyperLinks.Where(p => p.ToString() == hpl.ToString()).Count() == 0)
                    {
                        Select(m.Index, m.Length);
                        SelectionColor = HyperlinkColor;
                        Font f = SelectionFont;
                        Font f2 = new Font(f.FontFamily, f.Size - 1.2f, FontStyle.Underline | FontStyle.Bold | FontStyle.Italic);
                        SelectionFont = f2;
                        HyperLinks.Add(hpl);
                        update = true;
                    }
                }
                if (update)
                    SelectLastLine();
            }
        }

        public List<Hyperlink> HyperLinks { get; set; }

        public Color HyperlinkColor { get; set; }

        private bool hplink;
        bool UseHyperlinkStyle { get { return hplink; } set { DetectUrls = false; hplink = value; } }

        bool Pause { get; set; }

        bool InputEnable { get; set; }

        public string Title { get { if (Parent != null) return Parent.Text; else return ""; } set { if (Parent != null) Parent.Text = value; } }

        public string[] Arguments { get; set; }

        int CurrentPoint { get; set; }

        string CurrentLine { get; set; }

        public ConsoleStatus Status { get; set; }

        char CurrentKey { get; set; }

        public void Write(string message, Color? color = null)
        {
            while (Pause) Thread.Sleep(1);

            Select(TextLength, 0);
            if (!color.HasValue)
                color = ForeColor;
            SelectionColor = color.Value;
            SelectedText = message;
            DeselectAll();
        }

        public void WriteLine(string message, Color? color = null)
        {
            Write(message + "\r\n", color);
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
            SelectionColor = color.Value;
            SelectedText = message;
            DeselectAll();
        }

        public string ReadLine()
        {
            CurrentLine = "";
            CurrentPoint = TextLength;
            InputEnable = !InputEnable;//true
            ReadOnly = !ReadOnly;//false
            Status = ConsoleStatus.ReadLine;
            while (InputEnable) Thread.Sleep(1);

            return CurrentLine;
        }

        public char ReadKey()
        {
            CurrentKey = ' ';
            CurrentPoint = Text.Length;
            InputEnable = !InputEnable;//true
            ReadOnly = !ReadOnly;//false
            Status = ConsoleStatus.ReadKey;
            while (InputEnable) Thread.Sleep(1);

            return CurrentKey;
        }

    }
}
