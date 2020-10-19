using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CHIP8.Asm {
    using static Token.TokenType;
    class Lexer {
        static readonly string[] Keywords = new string[] {
            "jmp", "jsr", "seq", "sne",
            "jne", "jre", "rnd", "cls",
            "ret", "skn", "skk", "set",
            "or", "xor", "sub", "shr",
            "snb", "shl", "ldt", "wky",
            "sdt", "sst", "add", "lsp",
            "bcd", "sto", "drw", "raw"
        };
        static readonly string[] Registers = new string[] {
            "v0", "v1", "v2", "v3",
            "v4", "v5", "v6", "v7",
            "v8", "v9", "va", "vb",
            "vc", "vd", "ve", "vf",
            "ir"
        };

        public static List<Token> Tokens = new List<Token>();

        private static bool AtEnd => CurPos >= CurSource.Length;

        private static int CurPos;
        private static string CurSource;

        private static bool Matches(string token) {
            if (CurPos + token.Length > CurSource.Length)
                return false;
            if (CurSource.Substring(CurPos, token.Length) == token) {
                CurPos += token.Length;
                return true;
            }
            return false;
        }

        private static void SkipWhitespace() {
            while (!AtEnd && (char.IsWhiteSpace(CurSource[CurPos]) || CurSource[CurPos] == '\n' || CurSource[CurPos] == '\t'))
                CurPos++;
        }

        private static void SkipComments() {
            while (true) {
                if (!AtEnd && CurSource[CurPos] == '#') {
                    while (!AtEnd && CurSource[CurPos] != '\n')
                        CurPos++;
                    if (!AtEnd && CurSource[CurPos] == '\n')
                        CurPos++;
                } else
                    return;
            }
        }

        private static void LexNumber() {
            string num = "";
            while (!AtEnd && (char.IsNumber(CurSource[CurPos]) || (CurSource[CurPos] >= 'a' && CurSource[CurPos] <= 'f')))
                num += CurSource[CurPos++];
            if (!string.IsNullOrEmpty(num))
                Tokens.Add(new Token(NUM, num));
        }

        private static void LexLabel() {
            string label = "";
            while (!AtEnd && char.IsLetterOrDigit(CurSource[CurPos]))
                label += CurSource[CurPos++];
            Tokens.Add(new Token(LBL, label));
        }

        private static void LexInstructions() {
            if (AtEnd) return;
            foreach (var keyword in Keywords) {
                if (Matches(keyword))
                    Tokens.Add(new Token(keyword.ParseEnum<Token.TokenType>(), keyword));
            }
            foreach (var register in Registers) {
                if (Matches(register))
                    Tokens.Add(new Token(REG, register));
            }
        }

        public static void Run(string source) {
            CurSource = source.ToLower();
            while (!AtEnd) {
                SkipComments();
                SkipWhitespace();
                LexInstructions();
                if (AtEnd)
                    return;

                char chr = CurSource[CurPos++];
                switch (chr) {
                    case '-':
                    case ':':
                    case ',':
                        Tokens.Add(new Token(CHR, $"{chr}"));
                        break;
                    case '$':
                        LexNumber();
                        break;
                    case '<':
                        LexLabel();
                        break;
                    case ' ':
                    case '\n':
                    case '\t':
                    default:
                        break;
                        //throw new Exception($"Unhandled character '{chr}'");
                }
            }
        }
    }
}
