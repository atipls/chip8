using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastColoredTextBoxNS;

namespace chip8asm {
    public partial class main : Form {
        static string[] opcodes = new string[] {
             "JMP", "JSR", "SEQ", "SNE",
             "JNE", "JRE", "RND", "CLS",
             "RET", "SKN", "SKK", "SET",
             "OR ", "XOR", "SUB", "SHR",
             "SNB", "SHL", "LDT", "WKY",
             "SDT", "SST", "ADD", "LSP",
             "BCD", "STO", "DRW", "RAW"
        };
        public static string[] opcode_reference = new string[] {
            "Jump to address. EX: JMP LABEL / JMP $0200",
            "Jump to subroutine. EX: JSR LABEL / JSR $0200",
            "Skip next instruction if Vx == B OR Vx == Vy. EX: SEQ V0, $FF",
            "Skip next instruction if Vx != B. EX: SNE V0, $FF",
        };
        Style comment_style = new TextStyle(new SolidBrush(Color.FromArgb(0x57, 0xA6, 0x4A)), null, FontStyle.Italic);
        Style number_style = new TextStyle(new SolidBrush(Color.FromArgb(0xB5, 0xCE, 0xA8)), null, FontStyle.Regular);
        Style opcode_style = new TextStyle(new SolidBrush(Color.FromArgb(0x56, 0x9C, 0xD6)), null, FontStyle.Regular);
        Style register_style = new TextStyle(new SolidBrush(Color.FromArgb(0x4E, 0xC9, 0xB0)), null, FontStyle.Regular);

        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new main());
        }
        public main() {
            InitializeComponent();
            tb_main.Text += $"# supported opcodes: \n";
            foreach (var opcode in opcodes)
                tb_main.Text += $"#\t{opcode}\n";
            tb_main.Text += $@"
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
                if (comment_style != null)
                    comment_style.Dispose();
                if (number_style != null)
                    number_style.Dispose();
                if (opcode_style != null)
                    opcode_style.Dispose();
                if (register_style != null)
                    register_style.Dispose();
            }
            base.Dispose(disposing);
        }

        private void editor_changed(object sender, TextChangedEventArgs e) {
            e.ChangedRange.ClearStyle(comment_style, opcode_style, register_style, number_style);
            e.ChangedRange.SetStyle(comment_style, @"#.*");
            e.ChangedRange.SetStyle(opcode_style, $@"\b({string.Join("|", opcodes)})\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(register_style, $@"\bV[A-z]\b|\bV[0-9]\b|\bIR\b|\bBCD\b", RegexOptions.IgnoreCase);
            e.ChangedRange.SetStyle(number_style, @"\d+|\$[0-9a-fA-F]+");
        }

        private void itm_compile_click(object sender, EventArgs e) {
            using var sfd = new SaveFileDialog() {
                Title = "Select a path to assemble the file to.",
                Filter = "CHIP-8 Binaries|*.bin",
            };
            var asm = new assembler();
            asm.assemble(tb_main.Text);
            if (!asm.successful) {
                MessageBox.Show($"error: \n{asm.error_str}", "assembler");
                return;
            }
            if (sfd.ShowDialog() == DialogResult.OK) {
                File.WriteAllBytes(sfd.FileName, asm.output);
                MessageBox.Show("assembled without errors.");
            }
        }
    }
}
