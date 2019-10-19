using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static chip8asm.token.tt;
namespace chip8asm {
    class lexer {
        private static string[] keywords = new string[] {
            "jmp", "jsr", "seq", "sne",
            "jne", "jre", "rnd", "cls",
            "ret", "skn", "skk", "set",
            "or", "xor", "sub", "shr",
            "snb", "shl", "ldt", "wky",
            "sdt", "sst", "add", "lsp",
            "bcd", "sto", "drw", "raw"
        };
        private static string[] registers = new string[] { //can use linq magic to generate these, i dont really care
            "v0", "v1", "v2", "v3",
            "v4", "v5", "v6", "v7",
            "v8", "v9", "va", "vb",
            "vc", "vd", "ve", "vf",
            "ir"
        };

        public static List<token> tokens = new List<token>();
        private static bool end => pos >= source.Length;
        private static int pos;
        private static string source;
        private static bool check(string st) {
            if (pos + st.Length > source.Length)
                return false;
            if (source.Substring(pos, st.Length) == st) {
                pos += st.Length;
                Debug.WriteLine($"pos {pos}: {st}");
                return true;
            }
            //error reporting?
            return false;
        }
        private static void whitespace() {
            while (!end && char.IsWhiteSpace(source[pos]))
                pos++;
        }
        private static void comments() {
            while (true) {
                if (!end && source[pos] == '#') {
                    while (!end && source[pos] != '\n')
                        pos++;
                    if (!end && source[pos] == '\n')
                        pos++;
                } else return;
            }
        }
        private static void number() {
            string num = "";
            while (!end && (char.IsNumber(source[pos]) || (source[pos] >= 'a' && source[pos] <= 'f')))
                num += source[pos++];
            if (!string.IsNullOrEmpty(num)) tokens.Add(new token(NUM, num));
        }
        private static void label() {
            string lbl = "";
            while (!end && char.IsLetterOrDigit(source[pos]))
                lbl += source[pos++];
            tokens.Add(new token(LBL, lbl));
        }
        private static void instructions() {
            if (end) return;
            foreach (var keyword in keywords) {
                if (check(keyword))
                    tokens.Add(new token(keyword.parse<token.tt>(), keyword));
            }
            foreach (var register in registers) {
                if (check(register))
                    tokens.Add(new token(REG, register));
            }
        }
        public static void run(string src) {
            source = src.ToLower();
            while (!end) {
                comments();
                whitespace();
                instructions();
                if (end) return;

                char chr = source[pos++];
                switch (chr) {
                    case '-': case ':': case ',': tokens.Add(new token(CHR, $"{chr}")); break;
                    case '$': number(); break;
                    case '<': label(); break;
                    case ' ': break;
                    default:
                        Debug.WriteLine($"unhandled character '{chr}'");
                        break;
                }
            }
        }
    }
}
