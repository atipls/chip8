using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace CHIP8.Asm {
    using static Token.TokenType;
    class Assembler {
        public byte[] Output => Buffer.ToArray();
        public List<Token> Tokens { get; private set; } = new List<Token>();

        public string Error { get; private set; } = "";

        private List<byte> Buffer = new List<byte>();
        private int CurPos;
        private bool AtEnd => CurPos >= Tokens.Count;
        private ushort Address => (ushort)(0x200 /*chip8 constant*/ + Buffer.Count);

        private Dictionary<string, int> UnresolvedLabels = new Dictionary<string, int>();
        private Dictionary<string, ushort> ResolvedLabels = new Dictionary<string, ushort>();

        private void Emit(ushort raw) => Emit((byte)((raw & 0xFF00) >> 8), (byte)(raw & 0x00FF));
        private void Emit(params byte[] raw) => Buffer.AddRange(raw);
        private void EmitX(ushort opcode) {
            ushort instr = opcode;
            instr |= (ushort)(GetRegisterNumber() << 8);
            Emit(instr);
        }

        private void EmitXY(ushort opcode) {
            ushort instr = opcode;
            instr |= (ushort)(GetRegisterNumber() << 8);
            Expect(CHR, ",");
            instr |= (ushort)(GetRegisterNumber() << 4);
            Emit(instr);
        }

        private Token CurToken => Tokens[CurPos];

        private Token Expect(Token.TokenType type, string value = null) {
            if (!AtEnd && CurToken.Type == type) {
                if (value == null)
                    return Tokens[CurPos++];
                if (CurToken.Value == value)
                    return Tokens[CurPos++];
                throw new Exception($"Expected '{value}', got '{CurToken.Value}' instead.");
            }
            throw new Exception($"Expected type '{type}', got '{CurToken.Type}' instead.");
        }

        public Assembler() => CurPos = 0;

        private int GetRegisterNumber() {
            var reg = Expect(REG);
            if (reg != null) {
                if (reg.Value != "ir")
                    return byte.Parse(reg.Value.Substring(1, 1), NumberStyles.HexNumber);
                return -1;
            }
            throw new Exception($"Expected register, got '{CurToken.Type}' instead.");
        }

        private void TryResolvingLabels() {
            foreach (var label in ResolvedLabels) {
                if (UnresolvedLabels.ContainsKey(label.Key)) {
                    var patchpos = UnresolvedLabels[label.Key];
                    var address = ResolvedLabels[label.Key];
                    ushort instr = (ushort)((Buffer[patchpos] << 8) | Buffer[patchpos + 1]);
                    instr |= address;
                    Buffer[patchpos] = (byte)((instr & 0xFF00) >> 8);
                    Buffer[patchpos + 1] = (byte)(instr & 0x00FF);
                    UnresolvedLabels.Remove(label.Key);
                }
            }
        }

        private ushort GetAddress() {
            switch (CurToken.Type) {
                case NUM:
                    int val;
                    if (int.TryParse(CurToken.Value, NumberStyles.HexNumber, null, out val) && val < 0xFFF) {
                        CurPos++;
                        return (ushort)val;
                    }
                    if (val >= 0xFFF)
                        throw new Exception($"Too big literal '{val:X}' for a valid address.");
                    break;
                case LBL:
                    if (ResolvedLabels.ContainsKey(CurToken.Value)) {
                        var addr = ResolvedLabels[CurToken.Value];
                        CurPos++;
                        return addr;
                    } else {
                        UnresolvedLabels.Add(CurToken.Value, Buffer.Count);
                        CurPos++;
                        return 0;
                    }
                default:
                    throw new Exception($"Unexpected {CurToken.Type}");
            }
            return 0;
        }

        private int GetLiteral(int max) {
            var num = Expect(NUM);
            if (num != null) {
                if (int.TryParse(num.Value, NumberStyles.HexNumber, null, out int val) && val < max)
                    return val;
                throw new Exception($"'{val:X}' Too big literal, max '{max:X}'");
            }
            throw new NotImplementedException();
        }

        public bool Assemble(string source) {
            Error = "";
            Buffer.Clear();
            UnresolvedLabels.Clear();
            ResolvedLabels.Clear();

            try {
                Lexer.Run(source);
                CurPos = 0;
                Tokens = Lexer.Tokens;

                while (!AtEnd) {
                    var token = Tokens[CurPos++];
                    Debug.WriteLine($"{CurPos - 1}: {token}");
                    switch (token.Type) {
                        case JMP:
                            Emit((ushort)(0x1000 | GetAddress()));
                            break; //1NNN
                        case JSR:
                            Emit((ushort)(0x2000 | GetAddress()));
                            break; //2NNN
                        case SEQ: { //3XNN or 5XY0, depending if its a literal or a register
                                ushort instr = 0x0000;
                                instr |= (ushort)(GetRegisterNumber() << 8);
                                Expect(CHR, ",");
                                if (CurToken.Type == NUM)
                                    instr |= (ushort)(0x3000 | GetLiteral(0xFF));
                                else
                                    instr |= (ushort)(0x5000 | (GetRegisterNumber() << 4));
                                Emit(instr);
                                break;
                            }
                        case SNE: { //4XNN
                                ushort instr = 0x4000;
                                instr |= (ushort)(GetRegisterNumber() << 8);
                                Expect(CHR, ",");
                                instr |= (ushort)GetLiteral(0xFF);
                                Emit(instr);
                                break;
                            }
                        case JNE:
                            EmitXY(0x9000);
                            break; //9XY0
                        case JRE:
                            Emit((ushort)(0xB000 | GetAddress()));
                            break; //BNNN
                        case RND: { //CXNN
                                ushort instr = 0xC000;
                                instr |= (ushort)(GetRegisterNumber() << 8);
                                Expect(CHR, ",");
                                instr |= (ushort)GetLiteral(0xFF);
                                Emit(instr);
                                break;
                            }
                        case CLS:
                            Emit(0x00E0);
                            break; //00E0
                        case RET:
                            Emit(0x00EE);
                            break; //00EE
                        case SKN:
                            EmitXY(0xE0A1);
                            break; //EXA1
                        case SKK:
                            EmitXY(0xE09E);
                            break; //EX9E
                        case SET: {
                                ushort instr = 0x0000; //ANNN / 6XNN / 8XY0 depending on instructions
                                var reg1 = GetRegisterNumber();
                                Expect(CHR, ",");
                                if (reg1 == -1)
                                    instr |= (ushort)(0xA000 | GetAddress()); //ANNN
                                else {
                                    if (CurToken.Type == NUM) //6XNN
                                        instr |= (ushort)(0x6000 | (reg1 << 8) | GetLiteral(0xFF));
                                    else
                                        instr |= (ushort)(0x8000 | (reg1 << 8) | (GetRegisterNumber() << 4)); //8XY0
                                }
                                Emit(instr);
                                break;
                            }
                        case OR:
                            EmitXY(0x8001);
                            break; //8XY1
                        case XOR:
                            EmitXY(0x8003);
                            break; //8XY3
                        case SUB:
                            EmitXY(0x8005);
                            break; //8XY5
                        case SHR:
                            EmitX(0x8006);
                            break; //8XY6
                        case SNB:
                            EmitXY(0x8007);
                            break; //8XY7
                        case SHL:
                            EmitX(0x800E);
                            break; //8XYE
                        case LDT:
                            EmitX(0xF007);
                            break; //FX06
                        case WKY:
                            EmitX(0xF00A);
                            break; //FX0A
                        case SDT:
                            EmitX(0xF015);
                            break; //FX15
                        case SST:
                            EmitX(0xF018);
                            break; //FX18
                        case ADD: {
                                ushort instr = 0x0000; //FX1E / 7XNN / 8XY4 depending on instructions
                                var reg1 = GetRegisterNumber();
                                Expect(CHR, ",");
                                if (reg1 == -1)
                                    instr |= (ushort)(0xF01E | (GetRegisterNumber() << 8)); //FX1E
                                else {
                                    if (CurToken.Type == NUM) //7XNN
                                        instr |= (ushort)(0x7000 | (reg1 << 8) | GetLiteral(0xFF));
                                    else
                                        instr |= (ushort)(0x8004 | (reg1 << 8) | (GetRegisterNumber() << 4)); //8XY4
                                }
                                Emit(instr);
                                break;
                            }
                        case LSP:
                            EmitX(0xF029);
                            break; //FX29
                        case BCD:
                            EmitX(0xF033);
                            break; //FX33
                        case STO: {
                                ushort instr = 0xF000;
                                var reg1 = GetRegisterNumber();
                                if (reg1 == -1) {
                                    instr |= 0x65;
                                    instr |= (ushort)(GetRegisterNumber() << 8);
                                } else {
                                    instr |= 0x55;
                                    instr |= (ushort)(reg1 << 8);
                                }
                                Emit(instr);
                                break;
                            }
                        case DRW: { //DXYN
                                ushort instr = 0xD000;
                                instr |= (ushort)(GetRegisterNumber() << 8);
                                Expect(CHR, ",");
                                instr |= (ushort)(GetRegisterNumber() << 4);
                                Expect(CHR, ",");
                                instr |= (ushort)GetLiteral(0xF);
                                Emit(instr);
                                break;
                            }
                        case RAW: { //CUSTOM
                                var num = Expect(NUM);
                                if (num != null) {
                                    for (int i = 0; i < num.Value.Length; i += 2)
                                        Emit(byte.Parse(num.Value.Substring(i, 2), NumberStyles.HexNumber));
                                    if (num.Value.Length % 2 == 1)
                                        Emit(byte.Parse(num.Value.Substring(num.Value.Length - 1, 1), NumberStyles.HexNumber));
                                } else
                                    throw new Exception("Expected raw data after 'RAW' pseudo-opcode");
                                break;
                            }
                        case LBL: {
                                if (ResolvedLabels.ContainsKey(token.Value))
                                    throw new Exception($"Label '{token.Value}' already defined.");
                                Expect(CHR, ":");
                                ResolvedLabels.Add(token.Value, Address);
                                TryResolvingLabels();
                                break;
                            }
                        default:
                            break; //how
                    }
                }
            } catch (Exception ex) {
                Buffer.Clear();
                Error = ex.Message;
                return false;
            }
            return true;
        }
    }
}