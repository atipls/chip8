using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip8asm {
    class assembler {
        public byte[] output { get; private set; }
        private bool end => position >= source.Length;
        private int position;
        private readonly string source;
        private void whitespace() {
            while (!end && char.IsWhiteSpace(source[position])) {
                position++;
            }
        }
        private void comments() {
            while (true) {
                if (!end && source[position] == '#') {
                    while (!end && source[position] != '\n') {
                        position++;
                    }
                    if (!end && source[position] == '\n')
                        position++;
                } else return;
            }
        }
        public assembler(string source) {
            this.source = source;
            position = 0;
            if (!string.IsNullOrEmpty(source))
                build_output();
        }
        private void build_output() {
            while (!end) {
                whitespace();
                comments();
                if (end) return;
                var chr = source[position++];
                Debug.Write(chr);
            }
            Debug.Write('\n');
        }
    }
}
