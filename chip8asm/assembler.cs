using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using static chip8asm.token.tt;
namespace chip8asm {
    class assembler {
        public byte[] output => buffer.ToArray();
        public List<token> tokens { get; private set; } = new List<token>();
        public bool successful => !error;
        public string error_str { get; private set; } = "";

        private List<byte> buffer = new List<byte>();
        private int pos;
        private bool end => pos >= tokens.Count;
        private ushort cur_address => (ushort)(0x200 /*chip8 constant*/ + buffer.Count);
        private bool error = false;

        private Dictionary<string, int> unresolved_labels = new Dictionary<string, int>();
        private Dictionary<string, ushort> resolved_labels = new Dictionary<string, ushort>();

        private void emit(ushort raw) => emit((byte)((raw & 0xFF00) >> 8), (byte)(raw & 0x00FF));
        private void emit(params byte[] raw) => buffer.AddRange(raw);
        private void emit_x(ushort opcode) {
            ushort instr = opcode;
            instr |= (ushort)(register() << 8);
            emit(instr);
        }
        private void emit_xy(ushort opcode) {
            ushort instr = opcode;
            instr |= (ushort)(register() << 8);
            expect(CHR, ",");
            instr |= (ushort)(register() << 4);
            emit(instr);
        }
        private token cur => tokens[pos];
        private token expect(token.tt type, string value = null) {
            if (!end && cur.type == type) {
                if (value == null)
                    return tokens[pos++];
                if (cur.value == value) {
                    return tokens[pos++];
                } else {
                    error_str = $"expected '{value}', got '{cur.value}' instead.";
                    error = true;
                    return null;
                }
            }
            error_str = $"expected type '{type}', got '{cur.type}' instead.";
            error = true;
            return null;
        }
        public assembler() => this.pos = 0;

        private int register() {
            var reg = expect(REG);
            if (reg != null) {
                if (reg.value != "ir")
                    return byte.Parse(reg.value.Substring(1, 1), NumberStyles.HexNumber);
                return -1;
            }
            error_str = $"expected type '{REG}', got '{cur.type}' instead.";
            error = true;
            return 0;
        }
        private void try_resolve_labels() {
            foreach (var label in resolved_labels) {
                if (unresolved_labels.ContainsKey(label.Key)) {
                    var patchpos = unresolved_labels[label.Key];
                    var address = resolved_labels[label.Key];
                    ushort instr = (ushort)((buffer[patchpos] << 8) | buffer[patchpos + 1]);
                    instr |= address;
                    buffer[patchpos] = (byte)((instr & 0xFF00) >> 8);
                    buffer[patchpos + 1] = (byte)(instr & 0x00FF);
                    unresolved_labels.Remove(label.Key);
                }
            }
        }
        private ushort address() {
            switch (cur.type) {
                case NUM:
                    int val;
                    if (int.TryParse(cur.value, NumberStyles.HexNumber, null, out val) && val < 0xFFF) {
                        pos++;
                        return (ushort)val;
                    }
                    if (val > 0xFFF) {
                        pos++;
                        error_str = $"too big literal '{val:X}' for a valid address.";
                        error = true;
                    }
                    break;
                case LBL:
                    if (resolved_labels.ContainsKey(cur.value)) {
                        var addr = resolved_labels[cur.value];
                        pos++;
                        return addr;
                    } else {
                        unresolved_labels.Add(cur.value, buffer.Count);
                        pos++;
                        return 0; //placeholder
                    }
                default:
                    error_str = $"unknown token {cur.type}";
                    error = true;
                    break;
            }
            return 0;
        }
        private int literal(int max) {
            var num = expect(NUM);
            if (num != null) {
                if (int.TryParse(num.value, NumberStyles.HexNumber, null, out int val) && val < max)
                    return val;
                else {
                    error_str = $"'{val:X}' too big literal, expected max '{max:X}'";
                    error = true;
                    return 0;
                }
            }
            return 0; //if num is null we have already set the error in 'expect'
        }
        public void assemble(string source) {
            lexer.run(source);
            tokens = lexer.tokens;
            // address = 0x200;
            while (!end && !error) {
                var token = tokens[pos++];
                Debug.WriteLine($"{pos - 1}: {token}");
                switch (token.type) {
                    case JMP: emit((ushort)(0x1000 | address())); break; //1NNN
                    case JSR: emit((ushort)(0x2000 | address())); break; //2NNN
                    case SEQ: { //3XNN or 5XY0, depending if its a literal or a register
                        ushort instr = 0x0000;
                        instr |= (ushort)(register() << 8);
                        expect(CHR, ",");
                        if (cur.type == NUM)
                            instr |= (ushort)(0x3000 | literal(0xFF));
                        else instr |= (ushort)(0x5000 | (register() << 4));
                        emit(instr);
                        break;
                    }
                    case SNE: { //4XNN
                        ushort instr = 0x4000;
                        instr |= (ushort)(register() << 8);
                        expect(CHR, ",");
                        instr |= (ushort)literal(0xFF);
                        emit(instr);
                        break;
                    }
                    case JNE: emit_xy(0x9000); break; //9XY0
                    case JRE: emit((ushort)(0xB000 | address())); break; //BNNN
                    case RND: { //CXNN
                        ushort instr = 0xC000;
                        instr |= (ushort)(register() << 8);
                        expect(CHR, ",");
                        instr |= (ushort)literal(0xFF);
                        emit(instr);
                        break;
                    }
                    case CLS: emit(0x00E0); break; //00E0
                    case RET: emit(0x00EE); break; //00EE
                    case SKN: emit_xy(0xE0A1); break; //EXA1
                    case SKK: emit_xy(0xE09E); break; //EX9E
                    case SET: {
                        ushort instr = 0x0000; //ANNN / 6XNN / 8XY0 depending on instructions
                        var reg1 = register();
                        expect(CHR, ",");
                        if (reg1 == -1)
                            instr |= (ushort)(0xA000 | address()); //ANNN
                        else {
                            if (cur.type == NUM) //6XNN
                                instr |= (ushort)(0x6000 | (reg1 << 8) | literal(0xFF));
                            else instr |= (ushort)(0x8000 | (reg1 << 8) | (register() << 4)); //8XY0
                        }
                        emit(instr);
                        break;
                    }
                    case OR: emit_xy(0x8001); break; //8XY1
                    case XOR: emit_xy(0x8003); break; //8XY3
                    case SUB: emit_xy(0x8005); break; //8XY5
                    case SHR: emit_x(0x8006); break; //8XY6
                    case SNB: emit_xy(0x8007); break; //8XY7
                    case SHL: emit_x(0x800E); break; //8XYE
                    case LDT: emit_x(0xF007); break; //FX06
                    case WKY: emit_x(0xF00A); break; //FX0A
                    case SDT: emit_x(0xF015); break; //FX15
                    case SST: emit_x(0xF018); break; //FX18
                    case ADD: {
                        ushort instr = 0x0000; //FX1E / 7XNN / 8XY4 depending on instructions
                        var reg1 = register();
                        expect(CHR, ",");
                        if (reg1 == -1)
                            instr |= (ushort)(0xF01E | (register() << 8)); //FX1E
                        else {
                            if (cur.type == NUM) //7XNN
                                instr |= (ushort)(0x7000 | (reg1 << 8) | literal(0xFF));
                            else instr |= (ushort)(0x8004 | (reg1 << 8) | (register() << 4)); //8XY4
                        }
                        emit(instr);
                        break;
                    }
                    case LSP: emit_x(0xF029); break; //FX29
                    case BCD: emit_x(0xF033); break; //FX33
                    case STO: {
                        ushort instr = 0xF000;
                        var reg1 = register();
                        if (reg1 == -1) {
                            instr |= 0x65;
                            instr |= (ushort)(register() << 8);
                        } else {
                            instr |= 0x55;
                            instr |= (ushort)(reg1 << 8);
                        }
                        emit(instr);
                        break;
                    }
                    case DRW: { //DXYN
                        ushort instr = 0xD000;
                        instr |= (ushort)(register() << 8);
                        expect(CHR, ",");
                        instr |= (ushort)(register() << 4);
                        expect(CHR, ",");
                        instr |= (ushort)literal(0xF);
                        emit(instr);
                        break;
                    }
                    case RAW: { //CUSTOM
                        var num = expect(NUM);
                        if (num != null) {
                            for (int i = 0; i < num.value.Length; i += 2)
                                emit(byte.Parse(num.value.Substring(i, 2), NumberStyles.HexNumber));
                            if (num.value.Length % 2 == 1)
                                emit(byte.Parse(num.value.Substring(num.value.Length - 1, 1), NumberStyles.HexNumber));
                        } else {
                            error_str = "expected raw data after 'RAW' pseudo-opcode";
                            error = true;
                            break;
                        }
                        break;
                    }
                    case LBL: {
                        if (resolved_labels.ContainsKey(token.value)) {
                            error_str = $"label '{token.value}' already defined.";
                            error = true;
                            break;
                        }
                        expect(CHR, ":");
                        resolved_labels.Add(token.value, cur_address);
                        try_resolve_labels();
                        break;
                    }
                    default: break; //how
                }
            }
            if (error) buffer.Clear();
        }
    }
}