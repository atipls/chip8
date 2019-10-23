namespace chip8emu {
    partial class display {
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
            this.main = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.scaleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.color1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.color2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.main.SuspendLayout();
            this.SuspendLayout();
            // 
            // main
            // 
            this.main.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scaleToolStripMenuItem,
            this.color1ToolStripMenuItem,
            this.color2ToolStripMenuItem});
            this.main.Name = "main";
            this.main.Size = new System.Drawing.Size(111, 70);
            // 
            // scaleToolStripMenuItem
            // 
            this.scaleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xToolStripMenuItem,
            this.xToolStripMenuItem1,
            this.xToolStripMenuItem2,
            this.xToolStripMenuItem3,
            this.xToolStripMenuItem4});
            this.scaleToolStripMenuItem.Name = "scaleToolStripMenuItem";
            this.scaleToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.scaleToolStripMenuItem.Text = "scale";
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem.Text = "1x";
            // 
            // xToolStripMenuItem1
            // 
            this.xToolStripMenuItem1.Name = "xToolStripMenuItem1";
            this.xToolStripMenuItem1.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem1.Text = "2x";
            // 
            // xToolStripMenuItem2
            // 
            this.xToolStripMenuItem2.Name = "xToolStripMenuItem2";
            this.xToolStripMenuItem2.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem2.Text = "3x";
            // 
            // xToolStripMenuItem3
            // 
            this.xToolStripMenuItem3.Name = "xToolStripMenuItem3";
            this.xToolStripMenuItem3.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem3.Text = "4x";
            // 
            // xToolStripMenuItem4
            // 
            this.xToolStripMenuItem4.Name = "xToolStripMenuItem4";
            this.xToolStripMenuItem4.Size = new System.Drawing.Size(86, 22);
            this.xToolStripMenuItem4.Text = "8x";
            // 
            // color1ToolStripMenuItem
            // 
            this.color1ToolStripMenuItem.Name = "color1ToolStripMenuItem";
            this.color1ToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.color1ToolStripMenuItem.Text = "color 1";
            this.color1ToolStripMenuItem.Click += new System.EventHandler(this.color1_click);
            // 
            // color2ToolStripMenuItem
            // 
            this.color2ToolStripMenuItem.Name = "color2ToolStripMenuItem";
            this.color2ToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.color2ToolStripMenuItem.Text = "color 2";
            this.color2ToolStripMenuItem.Click += new System.EventHandler(this.color2_click);
            // 
            // display
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 160);
            this.ContextMenuStrip = this.main;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "display";
            this.ShowIcon = false;
            this.Text = "chip-8";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.on_key_down);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.on_key_up);
            this.main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip main;
        private System.Windows.Forms.ToolStripMenuItem scaleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem color1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem color2ToolStripMenuItem;
    }
}

