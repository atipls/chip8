using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip8asm {
    class token {
        public enum tt { //token type
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
        public tt type { get; private set; }
        public string value { get; private set; }

        public token(tt type, string value) {
            this.type = type;
            this.value = value;
        }
        public override string ToString() {
            return $"{type:F}: {value}";
        }
    }
}
