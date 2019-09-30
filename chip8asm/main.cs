using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            tb_main.Text += $@"JMP $0262 
RAW $EAAC
SET IR, $0AEA
RND VD, $00AA
JSR $0020
DRW V9, VB, 4";
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
            new assembler(tb_main.Text);
        }
    }
}
