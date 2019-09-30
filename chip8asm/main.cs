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
             "JMP", "JSR", "SEQ", "SNE", "SEQ",
             "SET", "ADD", "JNE", "SET", "JRE",
             "RND", "CLS", "RET", "SKN", "SKK",
             "SET", "OR ", "ADD", "XOR", "ADD",
             "SUB", "SHR", "SNB", "SHL", "LDT",
             "WKY", "SDT", "SST", "ADD", "LSP",
             "BCD", "STO", "STO", "DRW",
        };

        Style comment_style = new TextStyle(Brushes.Green, null, FontStyle.Italic);
        Style number_style = new TextStyle(Brushes.DarkGreen, null, FontStyle.Regular);
        Style opcode_style = new TextStyle(Brushes.DarkBlue, null, FontStyle.Bold);
        Style register_style = new TextStyle(Brushes.DarkMagenta, null, FontStyle.Bold);

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
            e.ChangedRange.ClearStyle(comment_style, number_style, opcode_style);
            e.ChangedRange.SetStyle(comment_style, @"#.*", RegexOptions.Singleline);
            e.ChangedRange.SetStyle(opcode_style, $@"\b({string.Join("|", opcodes)})\b");
            e.ChangedRange.SetStyle(register_style, $@"\b[Vv][A-z]\b|\b[Vv][0-9]\b");
            e.ChangedRange.SetStyle(number_style, @"\d+|\$[0-9a-fA-F]+");
            new assembler(tb_main.Text);
            tb_main.Refresh();
        }
    }
}
