﻿namespace CHIP8.Asm {
    partial class Window {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window));
            this.mstrip = new System.Windows.Forms.MenuStrip();
            this.file = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_open = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_save = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_save_as = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.itm_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.build = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_compile = new System.Windows.Forms.ToolStripMenuItem();
            this.itm_run = new System.Windows.Forms.ToolStripMenuItem();
            this.MainText = new FastColoredTextBoxNS.FastColoredTextBox();
            this.mstrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainText)).BeginInit();
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
            this.itm_open.ShortcutKeyDisplayString = "ctrl+o";
            this.itm_open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.itm_open.Size = new System.Drawing.Size(180, 22);
            this.itm_open.Text = "open";
            this.itm_open.Click += new System.EventHandler(this.OnOpenClick);
            // 
            // itm_save
            // 
            this.itm_save.Name = "itm_save";
            this.itm_save.ShortcutKeyDisplayString = "ctrl+s";
            this.itm_save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.itm_save.Size = new System.Drawing.Size(180, 22);
            this.itm_save.Text = "save";
            this.itm_save.Click += new System.EventHandler(this.OnSaveClick);
            // 
            // itm_save_as
            // 
            this.itm_save_as.Name = "itm_save_as";
            this.itm_save_as.Size = new System.Drawing.Size(180, 22);
            this.itm_save_as.Text = "save as";
            this.itm_save_as.Click += new System.EventHandler(this.OnSaveAsClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // itm_exit
            // 
            this.itm_exit.Name = "itm_exit";
            this.itm_exit.Size = new System.Drawing.Size(180, 22);
            this.itm_exit.Text = "exit";
            this.itm_exit.Click += new System.EventHandler(this.OnExitClick);
            // 
            // build
            // 
            this.build.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.itm_compile,
            this.itm_run});
            this.build.Name = "build";
            this.build.Size = new System.Drawing.Size(46, 20);
            this.build.Text = "build";
            // 
            // itm_compile
            // 
            this.itm_compile.Name = "itm_compile";
            this.itm_compile.Size = new System.Drawing.Size(176, 22);
            this.itm_compile.Text = "compile";
            this.itm_compile.Click += new System.EventHandler(this.OnCompileClick);
            // 
            // itm_run
            // 
            this.itm_run.Name = "itm_run";
            this.itm_run.Size = new System.Drawing.Size(176, 22);
            this.itm_run.Text = "run in the emulator";
            this.itm_run.Click += new System.EventHandler(this.OnRunClick);
            // 
            // tb_main
            // 
            this.MainText.AutoCompleteBracketsList = new char[] {
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
            this.MainText.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*" +
    "(?<range>:)\\s*(?<range>[^;]+);";
            this.MainText.AutoScrollMinSize = new System.Drawing.Size(291, 42);
            this.MainText.BackBrush = null;
            this.MainText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.MainText.CharHeight = 14;
            this.MainText.CharWidth = 8;
            this.MainText.CurrentPenSize = 3;
            this.MainText.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.MainText.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.MainText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainText.DocumentPath = null;
            this.MainText.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.MainText.ForeColor = System.Drawing.SystemColors.Control;
            this.MainText.IsReplaceMode = false;
            this.MainText.LineNumberColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(145)))), ((int)(((byte)(175)))));
            this.MainText.Location = new System.Drawing.Point(0, 24);
            this.MainText.Name = "tb_main";
            this.MainText.Paddings = new System.Windows.Forms.Padding(0);
            this.MainText.SelectionChangedDelayedEnabled = false;
            this.MainText.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.MainText.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("tb_main.ServiceColors")));
            this.MainText.ServiceLinesColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.MainText.Size = new System.Drawing.Size(800, 426);
            this.MainText.TabIndex = 1;
            this.MainText.Text = "# chip-8 assembler\r\n# basic directives are supported.\r\n";
            this.MainText.Zoom = 100;
            this.MainText.TextChanged += new System.EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(this.EditorChanged);
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MainText);
            this.Controls.Add(this.mstrip);
            this.MainMenuStrip = this.mstrip;
            this.Name = "main";
            this.Text = "chip-8 assembler";
            this.mstrip.ResumeLayout(false);
            this.mstrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainText)).EndInit();
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
        private FastColoredTextBoxNS.FastColoredTextBox MainText;
        private System.Windows.Forms.ToolStripMenuItem itm_run;
    }
}

