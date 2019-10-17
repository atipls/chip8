using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
namespace chip8emu {
    public class constants {
        public const int size = 0xFFF;
        public const int stack_size = 12;
        public const int rom_start = 0x200;
        public const int font_start = 0x50;
        public const int x_size = 64;
        public const int y_size = 32;
    }
    public class memory {
        public byte[] raw = new byte[constants.size];
        public ushort[] stack = new ushort[constants.stack_size]; //24 bytes

        public void write(byte[] buf, int start) {
            if (start > constants.size || start < 0)
                return; //TODO: show error message
            for (int i = start; i < start + buf.Length; i++)
                raw[i] = buf[i - start];
        }
        //assumes in bounds
        public ushort get16(int addr) => (ushort)(raw[addr] << 8 | raw[addr + 1]);
        public uint get32(int addr) => BitConverter.ToUInt32(raw, addr);
    }
    public class video {
        public bool draw = false;
        public bool[,] raw = new bool[constants.x_size, constants.y_size];
    }
    public class cpu {
        public bool[] keys = new bool[16];
        public byte[] v = new byte[16]; //V0-VF
        public ushort ir = 0; //index register
        public ushort pc = 0; //program counter
        public byte sp = 0; //stack pointer
        public byte delay_timer = 0;
        public byte sound_timer = 0;
        ushort instr = 0;
        public memory memory;
        public video video;

        Random random;
        public cpu() {
            memory = new memory();
            video = new video();
            random = new Random();
        }
        public void update() {
            instr = memory.get16(pc);
            pc += 2;

            ushort nnn = (ushort)(instr & 0x0FFF);
            ushort x = (ushort)((instr & 0x0F00) >> 8);
            ushort y = (ushort)((instr & 0x00F0) >> 4);
            byte nn = (byte)(instr & 0x00FF);

            switch ((instr & 0xF000) >> 12) {
                case 0x0: {
                    switch (instr & 0x000F) {
                        case 0x0000: Array.Clear(video.raw, 0, video.raw.Length); break; //clear screen
                        case 0x000E: pc = memory.stack[--sp]; break; //return from subroutine
                        default: goto unresolved;
                    }
                    break;
                }
                case 0x1: pc = nnn; break; //jump
                case 0x2: memory.stack[sp++] = pc; pc = nnn; break; //call
                case 0x3: if (v[x] == nn) pc += 2; break; //skip next instruction if equal to byte
                case 0x4: if (v[x] != nn) pc += 2; break; //skip next instruction if not equal to byte
                case 0x5: if (v[x] == v[y]) pc += 2; break; //skip next instruction if equal to register
                case 0x6: v[x] = nn; break; //set register to value
                case 0x7: v[x] += nn; break; //add value to register
                case 0x8: {
                    switch (instr & 0x000F) {
                        case 0: v[x] = v[y]; break; //set vx to vy      
                        case 1: v[x] |= v[y]; break; //or vx with vy
                        case 2: v[x] &= v[y]; break; //and vx with vy
                        case 3: v[x] ^= v[y]; break; //xor vx with vy
                        case 4: { //add vx to vy
                            v[0xF] = (byte)(v[x] + v[y] > 255 ? 1 : 0);
                            v[x] = (byte)(v[x] + v[y] & 0xFF);
                            break;
                        }
                        case 5: { //subtract vx from vy
                            v[0xF] = (byte)(v[x] > v[y] ? 1 : 0);
                            v[x] -= v[y];
                            break;
                        }
                        case 6: { //shift and set vx to lsb
                            v[0xF] = (byte)(v[x] & 1);
                            v[x] >>= 1;
                            break;
                        }
                        case 7: { //set vx to vy-vx
                            v[0xF] = (byte)(v[y] > v[x] ? 1 : 0);
                            v[x] = (byte)(v[y] - v[x]);
                            break;
                        }
                        case 0xE: { //shift and set vx to msb
                            v[0xF] = (byte)((v[x] & 0x80) >> 7);
                            v[x] <<= 1;
                            break;
                        }
                        default: goto unresolved;
                    }
                    break;
                }
                case 0x9: if (v[x] != v[y]) pc += 2; break; //skip next instruction if not equal to register
                case 0xA: ir = nnn; break; //set ir to value
                case 0xB: pc = (ushort)(v[0] + nnn); break; //jump to v0 + value
                case 0xC: v[x] = (byte)(random.Next(0, 255) & nn); break; //set register to RAND&NN
                case 0xD: { //draw at vx, vy, height n
                    v[0xF] = 0;
                    for (int row = 0; row < (instr & 0x000F); row++) {
                        byte sprite = memory.raw[ir + row];
                        for (int col = 0; col < 8; col++) {
                            byte pixel = (byte)(sprite & (0x80 >> col));
                            byte dx = (byte)((v[x] + col) % constants.x_size);
                            byte dy = (byte)((v[y] + row) % constants.y_size);
                            if (pixel != 0) {
                                if (video.raw[dx, dy])
                                    v[0xF] = 1;
                                video.raw[dx, dy] ^= true;
                                video.draw = true;
                            }
                        }
                    }
                    break;
                }
                case 0xE: {
                    switch (instr & 0x000F) {
                        case 0x0001: if (!keys[v[x]]) pc += 2; break; //skip next instruction if key vx is not pressed
                        case 0x000E: if (keys[v[x]]) pc += 2; break; //skip next instruction if key vx is pressed
                        default: goto unresolved;
                    }
                    break;
                }
                case 0xF: {
                    switch (instr & 0x00FF) {
                        case 7: v[x] = delay_timer; break; //set vx to delay timer
                        case 0xA: { //wait for key and place it to vx (blocking)
                            for (int i = 0; i < 16; i++) {
                                if (keys[i]) {
                                    v[x] = (byte)i;
                                    return;
                                }
                            }
                            pc -= 2;
                            break;
                        }
                        case 0x15: delay_timer = v[x]; break; //set delay timer to vx
                        case 0x18: sound_timer = v[x]; break; //set sound timer to vx
                        case 0x1E: ir += v[x]; break; //add vx to ir
                        case 0x29: ir = (ushort)(5 * v[x]); break; //load sprite to i from vx
                        case 0x33: { //store bcd of vx in i
                            byte val = v[x];
                            memory.raw[ir + 2] = (byte)(val % 10); val /= 10;
                            memory.raw[ir + 1] = (byte)(val % 10); val /= 10;
                            memory.raw[ir] = (byte)(val % 10);
                            break;
                        }
                        case 0x55: { //set V0-vx to I
                            for (int i = 0; i <= x; i++)
                                memory.raw[ir + i] = v[i];
                            break;
                        }
                        case 0x65: { //set V0-vx from I
                            for (int i = 0; i <= x; i++)
                                v[i] = memory.raw[ir + i];
                            break;
                        }
                    }
                    Debug.WriteLine($"SYSCALL {instr & 0x0FFF:X4} not implemented!");
                    break;
                }
                default:
unresolved:
                    Debug.WriteLine($"Unknown instruction: {instr:X4}"); break;
            }
            if (delay_timer > 0)
                delay_timer--;
            if (sound_timer > 0)
                sound_timer--;
        }
    }
    public class chip8 {
        public cpu cpu;
        public int ops; //operations per second
        private DateTime last;
        private int handled;

        public void init() {
            cpu = new cpu();
            cpu.pc = constants.rom_start;
            cpu.memory.write(font_buffer, 0);
            ops = 0;
            last = DateTime.Now;
        }
        public void load(string file) {
            if (!File.Exists(file))
                return; //TODO: show error message or crash
            init();
            cpu.memory.write(File.ReadAllBytes(file), constants.rom_start);
        }
        public void update() {
            //for the disassembler
            if ((DateTime.Now - last).TotalSeconds > 1) {
                ops = handled;
                handled = 0;
                last = DateTime.Now;
            } else handled++;

            cpu.update();
            //draw maybe
        }
        public byte[] font_buffer = new byte[] {
                0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
	            0x20, 0x60, 0x20, 0x20, 0x70, // 1
	            0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
	            0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
	            0x90, 0x90, 0xF0, 0x10, 0x10, // 4
	            0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
	            0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
	            0xF0, 0x10, 0x20, 0x40, 0x40, // 7
	            0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
	            0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
	            0xF0, 0x90, 0xF0, 0x90, 0x90, // A
	            0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
	            0xF0, 0x80, 0x80, 0x80, 0xF0, // C
	            0xE0, 0x90, 0x90, 0x90, 0xE0, // D
	            0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
	            0xF0, 0x80, 0xF0, 0x80, 0x80  // F
         };
    }
}