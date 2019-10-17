using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip8asm {
    class assembler {
        public byte[] output { get; private set; }
        private ushort instruction;
        private int pos;
        public List<token> tokens { get; private set; }

        public assembler(string source) {
            lexer.run(source);
            this.tokens = lexer.tokens;
            this.pos = 0;
        }
    }
}
