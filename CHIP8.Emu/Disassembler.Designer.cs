namespace CHIP8.Emu {
    partial class Disassembler {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.InstructionList = new System.Windows.Forms.ListView();
            this.ch_hex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_opcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gb_main = new System.Windows.Forms.GroupBox();
            this.State = new System.Windows.Forms.Label();
            this.OperationsPerSec = new System.Windows.Forms.Label();
            this.DT = new System.Windows.Forms.Label();
            this.ST = new System.Windows.Forms.Label();
            this.Stack = new System.Windows.Forms.Label();
            this.vf = new System.Windows.Forms.Label();
            this.ve = new System.Windows.Forms.Label();
            this.vd = new System.Windows.Forms.Label();
            this.vc = new System.Windows.Forms.Label();
            this.vb = new System.Windows.Forms.Label();
            this.va = new System.Windows.Forms.Label();
            this.v9 = new System.Windows.Forms.Label();
            this.v8 = new System.Windows.Forms.Label();
            this.v7 = new System.Windows.Forms.Label();
            this.v6 = new System.Windows.Forms.Label();
            this.v5 = new System.Windows.Forms.Label();
            this.v4 = new System.Windows.Forms.Label();
            this.v3 = new System.Windows.Forms.Label();
            this.v2 = new System.Windows.Forms.Label();
            this.v1 = new System.Windows.Forms.Label();
            this.v0 = new System.Windows.Forms.Label();
            this.IR = new System.Windows.Forms.Label();
            this.SP = new System.Windows.Forms.Label();
            this.PC = new System.Windows.Forms.Label();
            this.MemoryHex = new Be.Windows.Forms.HexBox();
            this.gb_main.SuspendLayout();
            this.SuspendLayout();
            // 
            // lv_instructions
            // 
            this.InstructionList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InstructionList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_hex,
            this.ch_opcode});
            this.InstructionList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.InstructionList.HideSelection = false;
            this.InstructionList.Location = new System.Drawing.Point(0, 0);
            this.InstructionList.Name = "lv_instructions";
            this.InstructionList.Size = new System.Drawing.Size(409, 224);
            this.InstructionList.TabIndex = 0;
            this.InstructionList.UseCompatibleStateImageBehavior = false;
            this.InstructionList.View = System.Windows.Forms.View.Details;
            // 
            // ch_hex
            // 
            this.ch_hex.Text = "Hex";
            this.ch_hex.Width = 90;
            // 
            // ch_opcode
            // 
            this.ch_opcode.Text = "opcode";
            this.ch_opcode.Width = 120;
            // 
            // gb_main
            // 
            this.gb_main.Controls.Add(this.State);
            this.gb_main.Controls.Add(this.OperationsPerSec);
            this.gb_main.Controls.Add(this.DT);
            this.gb_main.Controls.Add(this.ST);
            this.gb_main.Controls.Add(this.Stack);
            this.gb_main.Controls.Add(this.vf);
            this.gb_main.Controls.Add(this.ve);
            this.gb_main.Controls.Add(this.vd);
            this.gb_main.Controls.Add(this.vc);
            this.gb_main.Controls.Add(this.vb);
            this.gb_main.Controls.Add(this.va);
            this.gb_main.Controls.Add(this.v9);
            this.gb_main.Controls.Add(this.v8);
            this.gb_main.Controls.Add(this.v7);
            this.gb_main.Controls.Add(this.v6);
            this.gb_main.Controls.Add(this.v5);
            this.gb_main.Controls.Add(this.v4);
            this.gb_main.Controls.Add(this.v3);
            this.gb_main.Controls.Add(this.v2);
            this.gb_main.Controls.Add(this.v1);
            this.gb_main.Controls.Add(this.v0);
            this.gb_main.Controls.Add(this.IR);
            this.gb_main.Controls.Add(this.SP);
            this.gb_main.Controls.Add(this.PC);
            this.gb_main.Dock = System.Windows.Forms.DockStyle.Right;
            this.gb_main.Location = new System.Drawing.Point(409, 0);
            this.gb_main.Name = "gb_main";
            this.gb_main.Size = new System.Drawing.Size(181, 337);
            this.gb_main.TabIndex = 1;
            this.gb_main.TabStop = false;
            this.gb_main.Text = "cpu info";
            // 
            // lb_exec
            // 
            this.State.AutoSize = true;
            this.State.Location = new System.Drawing.Point(67, 168);
            this.State.Name = "lb_exec";
            this.State.Size = new System.Drawing.Size(57, 13);
            this.State.TabIndex = 23;
            this.State.Text = "Executing:";
            // 
            // ops
            // 
            this.OperationsPerSec.AutoSize = true;
            this.OperationsPerSec.Location = new System.Drawing.Point(7, 315);
            this.OperationsPerSec.Name = "ops";
            this.OperationsPerSec.Size = new System.Drawing.Size(120, 13);
            this.OperationsPerSec.TabIndex = 22;
            this.OperationsPerSec.Text = "Operations Per Second:";
            // 
            // dt
            // 
            this.DT.AutoSize = true;
            this.DT.Location = new System.Drawing.Point(7, 41);
            this.DT.Name = "dt";
            this.DT.Size = new System.Drawing.Size(63, 13);
            this.DT.TabIndex = 21;
            this.DT.Text = "DelayTimer:";
            // 
            // st
            // 
            this.ST.AutoSize = true;
            this.ST.Location = new System.Drawing.Point(89, 41);
            this.ST.Name = "st";
            this.ST.Size = new System.Drawing.Size(67, 13);
            this.ST.TabIndex = 20;
            this.ST.Text = "SoundTimer:";
            // 
            // stack
            // 
            this.Stack.AutoSize = true;
            this.Stack.Location = new System.Drawing.Point(7, 168);
            this.Stack.Name = "stack";
            this.Stack.Size = new System.Drawing.Size(41, 13);
            this.Stack.TabIndex = 19;
            this.Stack.Text = "Stack: ";
            // 
            // vf
            // 
            this.vf.AutoSize = true;
            this.vf.Location = new System.Drawing.Point(7, 132);
            this.vf.Name = "vf";
            this.vf.Size = new System.Drawing.Size(26, 13);
            this.vf.TabIndex = 18;
            this.vf.Text = "VF: ";
            // 
            // ve
            // 
            this.ve.AutoSize = true;
            this.ve.Location = new System.Drawing.Point(129, 119);
            this.ve.Name = "ve";
            this.ve.Size = new System.Drawing.Size(27, 13);
            this.ve.TabIndex = 17;
            this.ve.Text = "VE: ";
            // 
            // vd
            // 
            this.vd.AutoSize = true;
            this.vd.Location = new System.Drawing.Point(67, 119);
            this.vd.Name = "vd";
            this.vd.Size = new System.Drawing.Size(28, 13);
            this.vd.TabIndex = 16;
            this.vd.Text = "VD: ";
            // 
            // vc
            // 
            this.vc.AutoSize = true;
            this.vc.Location = new System.Drawing.Point(7, 119);
            this.vc.Name = "vc";
            this.vc.Size = new System.Drawing.Size(27, 13);
            this.vc.TabIndex = 15;
            this.vc.Text = "VC: ";
            // 
            // vb
            // 
            this.vb.AutoSize = true;
            this.vb.Location = new System.Drawing.Point(129, 106);
            this.vb.Name = "vb";
            this.vb.Size = new System.Drawing.Size(27, 13);
            this.vb.TabIndex = 14;
            this.vb.Text = "VB: ";
            // 
            // va
            // 
            this.va.AutoSize = true;
            this.va.Location = new System.Drawing.Point(67, 106);
            this.va.Name = "va";
            this.va.Size = new System.Drawing.Size(27, 13);
            this.va.TabIndex = 13;
            this.va.Text = "VA: ";
            // 
            // v9
            // 
            this.v9.AutoSize = true;
            this.v9.Location = new System.Drawing.Point(7, 106);
            this.v9.Name = "v9";
            this.v9.Size = new System.Drawing.Size(26, 13);
            this.v9.TabIndex = 12;
            this.v9.Text = "V9: ";
            // 
            // v8
            // 
            this.v8.AutoSize = true;
            this.v8.Location = new System.Drawing.Point(129, 93);
            this.v8.Name = "v8";
            this.v8.Size = new System.Drawing.Size(26, 13);
            this.v8.TabIndex = 11;
            this.v8.Text = "V8: ";
            // 
            // v7
            // 
            this.v7.AutoSize = true;
            this.v7.Location = new System.Drawing.Point(67, 93);
            this.v7.Name = "v7";
            this.v7.Size = new System.Drawing.Size(26, 13);
            this.v7.TabIndex = 10;
            this.v7.Text = "V7: ";
            // 
            // v6
            // 
            this.v6.AutoSize = true;
            this.v6.Location = new System.Drawing.Point(7, 93);
            this.v6.Name = "v6";
            this.v6.Size = new System.Drawing.Size(26, 13);
            this.v6.TabIndex = 9;
            this.v6.Text = "V6: ";
            // 
            // v5
            // 
            this.v5.AutoSize = true;
            this.v5.Location = new System.Drawing.Point(129, 80);
            this.v5.Name = "v5";
            this.v5.Size = new System.Drawing.Size(26, 13);
            this.v5.TabIndex = 8;
            this.v5.Text = "V5: ";
            // 
            // v4
            // 
            this.v4.AutoSize = true;
            this.v4.Location = new System.Drawing.Point(67, 80);
            this.v4.Name = "v4";
            this.v4.Size = new System.Drawing.Size(26, 13);
            this.v4.TabIndex = 7;
            this.v4.Text = "V4: ";
            // 
            // v3
            // 
            this.v3.AutoSize = true;
            this.v3.Location = new System.Drawing.Point(7, 80);
            this.v3.Name = "v3";
            this.v3.Size = new System.Drawing.Size(26, 13);
            this.v3.TabIndex = 6;
            this.v3.Text = "V3: ";
            // 
            // v2
            // 
            this.v2.AutoSize = true;
            this.v2.Location = new System.Drawing.Point(129, 67);
            this.v2.Name = "v2";
            this.v2.Size = new System.Drawing.Size(26, 13);
            this.v2.TabIndex = 5;
            this.v2.Text = "V2: ";
            // 
            // v1
            // 
            this.v1.AutoSize = true;
            this.v1.Location = new System.Drawing.Point(67, 67);
            this.v1.Name = "v1";
            this.v1.Size = new System.Drawing.Size(26, 13);
            this.v1.TabIndex = 4;
            this.v1.Text = "V1: ";
            // 
            // v0
            // 
            this.v0.AutoSize = true;
            this.v0.Location = new System.Drawing.Point(7, 67);
            this.v0.Name = "v0";
            this.v0.Size = new System.Drawing.Size(23, 13);
            this.v0.TabIndex = 3;
            this.v0.Text = "V0:";
            // 
            // ir
            // 
            this.IR.AutoSize = true;
            this.IR.Location = new System.Drawing.Point(129, 18);
            this.IR.Name = "ir";
            this.IR.Size = new System.Drawing.Size(24, 13);
            this.IR.TabIndex = 2;
            this.IR.Text = "IR: ";
            // 
            // sp
            // 
            this.SP.AutoSize = true;
            this.SP.Location = new System.Drawing.Point(67, 18);
            this.SP.Name = "sp";
            this.SP.Size = new System.Drawing.Size(27, 13);
            this.SP.TabIndex = 1;
            this.SP.Text = "SP: ";
            // 
            // pc
            // 
            this.PC.AutoSize = true;
            this.PC.Location = new System.Drawing.Point(7, 18);
            this.PC.Name = "pc";
            this.PC.Size = new System.Drawing.Size(27, 13);
            this.PC.TabIndex = 0;
            this.PC.Text = "PC: ";
            // 
            // hb_mem
            // 
            this.MemoryHex.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MemoryHex.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MemoryHex.Location = new System.Drawing.Point(0, 230);
            this.MemoryHex.Name = "hb_mem";
            this.MemoryHex.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.MemoryHex.Size = new System.Drawing.Size(409, 107);
            this.MemoryHex.TabIndex = 20;
            // 
            // disasm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 337);
            this.Controls.Add(this.MemoryHex);
            this.Controls.Add(this.gb_main);
            this.Controls.Add(this.InstructionList);
            this.Name = "disasm";
            this.Text = "chip-8 disassembler";
            this.gb_main.ResumeLayout(false);
            this.gb_main.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ColumnHeader ch_hex;
        private System.Windows.Forms.ColumnHeader ch_opcode;
        public System.Windows.Forms.ListView InstructionList;
        private System.Windows.Forms.GroupBox gb_main;
        private System.Windows.Forms.Label PC;
        private System.Windows.Forms.Label IR;
        private System.Windows.Forms.Label SP;
        private System.Windows.Forms.Label vb;
        private System.Windows.Forms.Label va;
        private System.Windows.Forms.Label v9;
        private System.Windows.Forms.Label v8;
        private System.Windows.Forms.Label v7;
        private System.Windows.Forms.Label v6;
        private System.Windows.Forms.Label v5;
        private System.Windows.Forms.Label v4;
        private System.Windows.Forms.Label v3;
        private System.Windows.Forms.Label v2;
        private System.Windows.Forms.Label v1;
        private System.Windows.Forms.Label v0;
        private System.Windows.Forms.Label ve;
        private System.Windows.Forms.Label vd;
        private System.Windows.Forms.Label vc;
        private System.Windows.Forms.Label vf;
        private System.Windows.Forms.Label Stack;
        private System.Windows.Forms.Label DT;
        private System.Windows.Forms.Label ST;
        private System.Windows.Forms.Label OperationsPerSec;
        private System.Windows.Forms.Label State;
        private Be.Windows.Forms.HexBox MemoryHex;
    }
}