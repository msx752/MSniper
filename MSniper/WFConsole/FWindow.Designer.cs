namespace MSniper
{
    partial class FWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FWindow));
            this.Console = new MSniper.FConsole();
            this.SuspendLayout();
            // 
            // Console
            // 
            this.Console.Arguments = new string[0];
            this.Console.BackColor = System.Drawing.Color.Black;
            this.Console.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Console.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Console.Font = new System.Drawing.Font("Consolas", 10F);
            this.Console.ForeColor = System.Drawing.Color.Gray;
            this.Console.Location = new System.Drawing.Point(0, 0);
            this.Console.MinimumSize = new System.Drawing.Size(100, 200);
            this.Console.Name = "Console";
            this.Console.ReadOnly = true;
            this.Console.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.Console.Size = new System.Drawing.Size(634, 311);
            this.Console.Status = MSniper.ConsoleState.ReadLine;
            this.Console.TabIndex = 0;
            this.Console.Text = "";
            this.Console.Title = "MSniper";
            // 
            // FWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(634, 311);
            this.Controls.Add(this.Console);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FWindow";
            this.Text = "MSniper";
            this.Load += new System.EventHandler(this.FWindow_Load);
            this.ResumeLayout(false);

        }


        #endregion

        public FConsole Console;
    }
}