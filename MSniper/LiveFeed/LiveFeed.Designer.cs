namespace MSniper.LiveFeed
{
    partial class LiveFeed
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
            this.cmbPokemons = new System.Windows.Forms.ComboBox();
            this.lvlPokemonFilter = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.listPokemons = new System.Windows.Forms.ListView();
            this.clPokemonName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPokemonIV = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmbIv = new System.Windows.Forms.ComboBox();
            this.lblIvFilter = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbPokemons
            // 
            this.cmbPokemons.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbPokemons.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPokemons.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbPokemons.FormattingEnabled = true;
            this.cmbPokemons.Location = new System.Drawing.Point(4, 20);
            this.cmbPokemons.Name = "cmbPokemons";
            this.cmbPokemons.Size = new System.Drawing.Size(121, 21);
            this.cmbPokemons.TabIndex = 0;
            // 
            // lvlPokemonFilter
            // 
            this.lvlPokemonFilter.AutoSize = true;
            this.lvlPokemonFilter.Location = new System.Drawing.Point(21, 2);
            this.lvlPokemonFilter.Name = "lvlPokemonFilter";
            this.lvlPokemonFilter.Size = new System.Drawing.Size(77, 13);
            this.lvlPokemonFilter.TabIndex = 1;
            this.lvlPokemonFilter.Text = "Pokemon Filter";
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Location = new System.Drawing.Point(202, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(95, 37);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // listPokemons
            // 
            this.listPokemons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clPokemonName,
            this.clPokemonIV});
            this.listPokemons.Cursor = System.Windows.Forms.Cursors.Hand;
            this.listPokemons.FullRowSelect = true;
            this.listPokemons.GridLines = true;
            this.listPokemons.HoverSelection = true;
            this.listPokemons.Location = new System.Drawing.Point(1, 45);
            this.listPokemons.MultiSelect = false;
            this.listPokemons.Name = "listPokemons";
            this.listPokemons.Size = new System.Drawing.Size(199, 368);
            this.listPokemons.TabIndex = 3;
            this.listPokemons.UseCompatibleStateImageBehavior = false;
            this.listPokemons.View = System.Windows.Forms.View.Details;
            // 
            // clPokemonName
            // 
            this.clPokemonName.Text = "Pokemon Name";
            this.clPokemonName.Width = 95;
            // 
            // clPokemonIV
            // 
            this.clPokemonIV.Text = "IV";
            this.clPokemonIV.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clPokemonIV.Width = 23;
            // 
            // cmbIv
            // 
            this.cmbIv.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cmbIv.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbIv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbIv.FormattingEnabled = true;
            this.cmbIv.Location = new System.Drawing.Point(140, 19);
            this.cmbIv.Name = "cmbIv";
            this.cmbIv.Size = new System.Drawing.Size(56, 21);
            this.cmbIv.TabIndex = 0;
            // 
            // lblIvFilter
            // 
            this.lblIvFilter.AutoSize = true;
            this.lblIvFilter.Location = new System.Drawing.Point(141, 1);
            this.lblIvFilter.Name = "lblIvFilter";
            this.lblIvFilter.Size = new System.Drawing.Size(50, 13);
            this.lblIvFilter.TabIndex = 1;
            this.lblIvFilter.Text = "IV% Filter";
            // 
            // btnClear
            // 
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Location = new System.Drawing.Point(202, 47);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(95, 37);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // LiveFeed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 415);
            this.Controls.Add(this.listPokemons);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblIvFilter);
            this.Controls.Add(this.lvlPokemonFilter);
            this.Controls.Add(this.cmbIv);
            this.Controls.Add(this.cmbPokemons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "LiveFeed";
            this.Text = "LiveFeed";
            this.Load += new System.EventHandler(this.LiveFeed_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPokemons;
        private System.Windows.Forms.Label lvlPokemonFilter;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView listPokemons;
        private System.Windows.Forms.ColumnHeader clPokemonName;
        private System.Windows.Forms.ColumnHeader clPokemonIV;
        private System.Windows.Forms.ComboBox cmbIv;
        private System.Windows.Forms.Label lblIvFilter;
        private System.Windows.Forms.Button btnClear;
    }
}