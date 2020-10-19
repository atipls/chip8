using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP8.Asm {
    class Token {
        public enum TokenType {
            //opcodes
            JMP, JSR, SEQ, SNE,
            JNE, JRE, RND, CLS,
            RET, SKN, SKK, SET,
            OR, XOR, SUB, SHR,
            SNB, SHL, LDT, WKY,
            SDT, SST, ADD, LSP,
            BCD, STO, DRW, RAW,

            REG, //register from V0 to VF
            LBL, //label
            NUM, //number like an address or a literal
            CHR, //character like , or -
        }
        public TokenType Type { get; private set; }
        public string Value { get; private set; }

        public Token(TokenType type, string value) {
            Type = type;
            Value = value;
        }

        public override string ToString() {
            return $"{Type:F}: {Value}";
        }
    }
}
