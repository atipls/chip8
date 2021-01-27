using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace CHIP8.Asm {
    public partial class Window : Form {
        static string[] Opcodes = new string[] {
             "JMP", "JSR", "SEQ", "SNE",
             "JNE", "JRE", "RND", "CLS",
             "RET", "SKN", "SKK", "SET",
             "OR ", "XOR", "SUB", "SHR",
             "SNB", "SHL", "LDT", "WKY",
             "SDT", "SST", "ADD", "LSP",
             "BCD", "STO", "DRW", "RAW"
        };

        // TODO: Finish
        public static string[] OpcodeReference = new string[] {
            "Jump to address. EX: JMP LABEL / JMP $0200",
            "Jump to subroutine. EX: JSR LABEL / JSR $0200",
            "Skip next instruction if Vx == B OR Vx == Vy. EX: SEQ V0, $FF",
            "Skip next instruction if Vx != B. EX: SNE V0, $FF",
        };

        private static readonly Assembler Assembler = new Assembler();

        private readonly Style CommentStyle = new TextStyle(new SolidBrush(Color.FromArgb(0x57, 0xA6, 0x4A)), null, FontStyle.Italic);
        private readonly Style NumberStyle = new TextStyle(new SolidBrush(Color.FromArgb(0xB5, 0xCE, 0xA8)), null, FontStyle.Regular);
        private readonly Style OpcodeStyle = new TextStyle(new SolidBrush(Color.FromArgb(0x56, 0x9C, 0xD6)), null, FontStyle.Regular);
        private readonly Style RegisterStyle = new TextStyle(new SolidBrush(Color.FromArgb(0x4E, 0xC9, 0xB0)), null, FontStyle.Regular);

        private bool IsModified;
        public bool Modified {
            get => IsModified;
            private set {
                Text = $"CHIP-8 Assembler{(Modified ? "*" : "")}";
                IsModified = value;
            }
        }
        private string Filename = "";

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Window());
        }

        public Window() {
            InitializeComponent();
            MainText.Text += $"# supported opcodes: \n";
            foreach (var opcode in Opcodes)
                MainText.Text += $"#\t{opcode}\n";
            MainText.Text += $@"
       SET V2, $00
<START:
       LSP V2
       DRW V0, V1, $05
       ADD V2, $01
       ADD V0, $05
       SNE V0, $3C
       JMP <INCY
       JMP <START
<INCY:
       ADD V1, $06
       SET V0, $00
       JMP <START";
        }
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null)
                    components.Dispose();
                if (CommentStyle != null)
                    CommentStyle.Dispose();
                if (NumberStyle != null)
                    NumberStyle.Dispose();
                if (OpcodeStyle != null)
                    OpcodeStyle.Dispose();
                if (RegisterStyle != null)
                    RegisterStyle.Dispose();
            }
            base.Dispose(disposing);
        }

        private void EditorChanged(object sender, TextChangedEventArgs e) {
            e.ChangedRange.ClearStyle(CommentStyle, OpcodeStyle, RegisterStyle, NumberStyle);
            e.ChangedRange.SetStyle(CommentStyle, @"#.*");
            e.ChangedRange.SetStyle(OpcodeStyle, $@"\b({string.Join("|", Opcodes)})\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(RegisterStyle, $@"\bV[A-z]\b|\bV[0-9]\b|\bIR\b|\bBCD\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(NumberStyle, @"\d+|\$[0-9a-fA-F]+");
            Modified = true;
        }

        private void OnCompileClick(object sender, EventArgs e) {
            using var sfd = new SaveFileDialog() {
                Title = "Select a path to assemble the file to.",
                Filter = "CHIP-8 Binaries|*.bin",
            };
            if (!Assembler.Assemble(MainText.Text)) {
                MessageBox.Show($"Error: \n{Assembler.Error}", "Assembler");
                return;
            }
            if (sfd.ShowDialog() == DialogResult.OK) {
                File.WriteAllBytes(sfd.FileName, Assembler.Output);
                MessageBox.Show("Assembled without errors.");
            }
        }

        private void OnRunClick(object sender, EventArgs e) {
            if (!Assembler.Assemble(MainText.Text)) {
                MessageBox.Show($"Error: \n{Assembler.Error}", "Assembler");
                return;
            }
            if (!File.Exists("CHIP8.Emu.exe")) {
                MessageBox.Show($"CHIP8.Emu.exe not found!\nPlease place it to the directory the assembler is in.", "Assembler");
                return;
            }
            Process.Start("CHIP8.Emu.exe", $"--raw {Convert.ToBase64String(Assembler.Output)}");
        }

        private void OnOpenClick(object sender, EventArgs e) {
            if (Modified && MessageBox.Show("File modified, do you still want to continue?", "Assembler", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            using var ofd = new OpenFileDialog() {
                Title = "Select the file to open.",
                Filter = "CHIP-8 Assembly Source|*.asm",
            };
            Filename = ofd.FileName;
            MainText.Text = File.ReadAllText(Filename);
            Modified = false;
        }

        private void OnSaveClick(object sender, EventArgs e) {
            if (Filename != "") {
                File.WriteAllText(Filename, MainText.Text);
                Modified = false;
            } else OnSaveAsClick(sender, e);
        }

        private void OnSaveAsClick(object sender, EventArgs e) {
            using var sfd = new SaveFileDialog() {
                Title = "Select the file to save as.",
                Filter = "CHIP-8 Assembly Source|*.asm",
            };
            if (sfd.ShowDialog() == DialogResult.OK) {
                Filename = sfd.FileName;
                File.WriteAllText(Filename, MainText.Text);
                Modified = false;
            }
        }
        private void OnExitClick(object sender, EventArgs e) {
            if (Modified && MessageBox.Show("File modified, do you still want to exit?", "Assembler", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            Application.Exit();
        }
    }
}
