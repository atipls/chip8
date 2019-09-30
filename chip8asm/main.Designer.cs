namespace chip8asm {
    partial class main {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(main));
            this.mstrip = new System.Windows.Forms.MenuStrip();
            this.file = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_open = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_save = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_save_as = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.itm_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.build = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_compile = new System.Windows.Forms.ToolStripMenuItem();
            this.tb_main = new FastColoredTextBoxNS.FastColoredTextBox();
            this.mstrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_main)).BeginInit();
            this.SuspendLayout();
            // 
            // mstrip
            // 
            this.mstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.file,
            this.build});
            this.mstrip.Location = new System.Drawing.Point(0, 0);
            this.mstrip.Name = "mstrip";
            this.mstrip.Size = new System.Drawing.Size(800, 24);
            this.mstrip.TabIndex = 0;
            this.mstrip.Text = "menuStrip1";
            // 
            // file
            // 
            this.file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itm_open,
            this.itm_save,
            this.itm_save_as,
            this.toolStripSeparator1,
            this.itm_exit});
            this.file.Name = "file";
            this.file.Size = new System.Drawing.Size(35, 20);
            this.file.Text = "file";
            // 
            // itm_open
            // 
            this.itm_open.Name = "itm_open";
            this.itm_open.Size = new System.Drawing.Size(111, 22);
            this.itm_open.Text = "open";
            // 
            // itm_save
            // 
            this.itm_save.Name = "itm_save";
            this.itm_save.Size = new System.Drawing.Size(111, 22);
            this.itm_save.Text = "save";
            // 
            // itm_save_as
            // 
            this.itm_save_as.Name = "itm_save_as";
            this.itm_save_as.Size = new System.Drawing.Size(111, 22);
            this.itm_save_as.Text = "save as";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(108, 6);
            // 
            // itm_exit
            // 
            this.itm_exit.Name = "itm_exit";
            this.itm_exit.Size = new System.Drawing.Size(111, 22);
            this.itm_exit.Text = "exit";
            // 
            // build
            // 
            this.build.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itm_compile});
            this.build.Name = "build";
            this.build.Size = new System.Drawing.Size(46, 20);
            this.build.Text = "build";
            // 
            // itm_compile
            // 
            this.itm_compile.Name = "itm_compile";
            this.itm_compile.Size = new System.Drawing.Size(117, 22);
            this.itm_compile.Text = "compile";
            // 
            // tb_main
            // 
            this.tb_main.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.tb_main.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*" +
    "(?<range>:)\\s*(?<range>[^;]+);";
            this.tb_main.AutoScrollMinSize = new System.Drawing.Size(291, 42);
            this.tb_main.BackBrush = null;
            this.tb_main.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.tb_main.CharHeight = 14;
            this.tb_main.CharWidth = 8;
            this.tb_main.CurrentPenSize = 3;
            this.tb_main.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_main.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.tb_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_main.DocumentPath = null;
            this.tb_main.ForeColor = System.Drawing.SystemColors.Control;
            this.tb_main.IsReplaceMode = false;
            this.tb_main.LineNumberColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(145)))), ((int)(((byte)(175)))));
            this.tb_main.Location = new System.Drawing.Point(0, 24);
            this.tb_main.Name = "tb_main";
            this.tb_main.Paddings = new System.Windows.Forms.Padding(0);
            this.tb_main.SelectionChangedDelayedEnabled = false;
            this.tb_main.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.tb_main.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tb_main.ServiceColors")));
            this.tb_main.ServiceLinesColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.tb_main.Size = new System.Drawing.Size(800, 426);
            this.tb_main.TabIndex = 1;
            this.tb_main.Text = "# chip-8 assembler\r\n# basic directives are supported.\r\n";
            this.tb_main.Zoom = 100;
            this.tb_main.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.editor_changed);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tb_main);
            this.Controls.Add(this.mstrip);
            this.MainMenuStrip = this.mstrip;
            this.Name = "main";
            this.Text = "chip-8 assembler";
            this.mstrip.ResumeLayout(false);
            this.mstrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tb_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mstrip;
        private System.Windows.Forms.ToolStripMenuItem file;
        private System.Windows.Forms.ToolStripMenuItem itm_open;
        private System.Windows.Forms.ToolStripMenuItem itm_save;
        private System.Windows.Forms.ToolStripMenuItem itm_save_as;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem itm_exit;
        private System.Windows.Forms.ToolStripMenuItem build;
        private System.Windows.Forms.ToolStripMenuItem itm_compile;
        private FastColoredTextBoxNS.FastColoredTextBox tb_main;
    }
}

