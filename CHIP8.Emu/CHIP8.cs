using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace CHIP8.Emu {

    public class Constants {
        public const int RAMSize = 0x1000;
        public const int StackSize = 12;
        public const int RomStart = 0x200;
        public const int FontStart = 0x50;
        public const int ResolutionX = 64;
        public const int ResolutionY = 32;
    }

    public class Memory {
        public byte[] Raw = new byte[Constants.RAMSize];
        public ushort[] Stack = new ushort[Constants.StackSize];

        public void Write(byte[] buffer, int start) {
            if (start > Constants.RAMSize || start < 0) {
                Debug.WriteLine($"Invalid write! Address: {start:X}");
                return;
            }
            for (int i = start; i < start + buffer.Length; i++)
                Raw[i] = buffer[i - start];
        }
        //assumes in bounds
        public ushort Get16(int addr) => (ushort)(Raw[addr] << 8 | Raw[addr + 1]);
        public uint Get32(int addr) => BitConverter.ToUInt32(Raw, addr);
    }

    public class Video {
        public bool DoDraw = false;
        public bool[,] Raw = new bool[Constants.ResolutionX, Constants.ResolutionY];
    }

    public class CPU {
        public bool[] Keys = new bool[16];
        public byte[] V = new byte[16]; // V0-VF
        public ushort IR = 0;           // Index register
        public ushort PC = 0;           // Program counter
        public byte SP = 0;             // Stack pointer

        public byte DelayTimer = 0;
        public byte SoundTimer = 0;

        ushort CurrentInstruction = 0;
        public Memory Memory;
        public Video Video;
        public bool Halting = false;

        readonly Random Random;
        public CPU() {
            Memory = new Memory();
            Video = new Video();
            Random = new Random();
        }

        public void TimerUpdate() {
            if (DelayTimer > 0)
                DelayTimer--;
            if (SoundTimer > 0)
                SoundTimer--;
        }

        public void Update() {
            if (Halting || PC > 0xFFD) {
                Halting = true;
                return;
            }

            CurrentInstruction = Memory.Get16(PC);
            PC += 2;

            ushort nnn = (ushort)(CurrentInstruction & 0x0FFF);
            ushort x = (ushort)((CurrentInstruction & 0x0F00) >> 8);
            ushort y = (ushort)((CurrentInstruction & 0x00F0) >> 4);
            byte nn = (byte)(CurrentInstruction & 0x00FF);

            switch ((CurrentInstruction & 0xF000) >> 12) {
            case 0x0: {
                switch (CurrentInstruction & 0x000F) {
                case 0x0000: Array.Clear(Video.Raw, 0, Video.Raw.Length); break; // Clear screen
                case 0x000E: PC = Memory.Stack[--SP]; break;                     // Return from subroutine
                default: goto unresolved;
                }
                break;
            }
            case 0x1: PC = nnn; break;                          // Jump
            case 0x2: Memory.Stack[SP++] = PC; PC = nnn; break; // Call
            case 0x3: if (V[x] == nn) PC += 2; break;           // Skip next instruction if equal to byte
            case 0x4: if (V[x] != nn) PC += 2; break;           // Skip next instruction if not equal to byte
            case 0x5: if (V[x] == V[y]) PC += 2; break;         // Skip next instruction if equal to register
            case 0x6: V[x] = nn; break;                         // Set register to value
            case 0x7: V[x] += nn; break;                        // Add value to register
            case 0x8: {
                switch (CurrentInstruction & 0x000F) {
                case 0: V[x] = V[y]; break;  // Set vx to vy      
                case 1: V[x] |= V[y]; break; // Or vx with vy
                case 2: V[x] &= V[y]; break; // And vx with vy
                case 3: V[x] ^= V[y]; break; // Xor vx with vy
                case 4: {                    // Add vx to vy
                    V[0xF] = (byte)(V[x] + V[y] > 255 ? 1 : 0);
                    V[x] = (byte)(V[x] + V[y] & 0xFF);
                    break;
                }
                case 5: { //subtract vx from vy
                    V[0xF] = (byte)(V[x] > V[y] ? 1 : 0);
                    V[x] -= V[y];
                    break;
                }
                case 6: { //shift and set vx to lsb
                    V[0xF] = (byte)(V[x] & 1);
                    V[x] >>= 1;
                    break;
                }
                case 7: { //set vx to vy-vx
                    V[0xF] = (byte)(V[y] > V[x] ? 1 : 0);
                    V[x] = (byte)(V[y] - V[x]);
                    break;
                }
                case 0xE: { //shift and set vx to msb
                    V[0xF] = (byte)((V[x] & 0x80) >> 7);
                    V[x] <<= 1;
                    break;
                }
                default: goto unresolved;
                }
                break;
            }
            case 0x9: if (V[x] != V[y]) PC += 2; break;               // Skip next instruction if not equal to register
            case 0xA: IR = nnn; break;                                // Set ir to value
            case 0xB: PC = (ushort)(V[0] + nnn); break;               // Jump to v0 + value
            case 0xC: V[x] = (byte)(Random.Next(0, 255) & nn); break; // Set register to RAND&NN
            case 0xD: {                                               // Draw at vx, vy, height n
                V[0xF] = 0;
                for (int row = 0; row < (CurrentInstruction & 0x000F); row++) {
                    byte sprite = Memory.Raw[IR + row];
                    for (int col = 0; col < 8; col++) {
                        byte pixel = (byte)(sprite & (0x80 >> col));
                        byte dx = (byte)((V[x] + col) % Constants.ResolutionX);
                        byte dy = (byte)((V[y] + row) % Constants.ResolutionY);
                        if (pixel != 0) {
                            if (Video.Raw[dx, dy])
                                V[0xF] = 1;
                            Video.Raw[dx, dy] ^= true;
                            Video.DoDraw = true;
                        }
                    }
                }
                break;
            }
            case 0xE: {
                switch (CurrentInstruction & 0x000F) {
                case 0x0001: if (!Keys[V[x]]) PC += 2; break; // Skip next instruction if key vx is not pressed
                case 0x000E: if (Keys[V[x]]) PC += 2; break;  // Skip next instruction if key vx is pressed
                default: goto unresolved;
                }
                break;
            }
            case 0xF: {
                switch (CurrentInstruction & 0x00FF) {
                case 7: V[x] = DelayTimer; break; //set vx to delay timer
                case 0xA: { //wait for key and place it to vx (blocking)
                    for (int i = 0; i < 16; i++) {
                        if (Keys[i]) {
                            V[x] = (byte)i;
                            return;
                        }
                    }
                    PC -= 2;
                    break;
                }
                case 0x15: DelayTimer = V[x]; break; //set delay timer to vx
                case 0x18: SoundTimer = V[x]; break; //set sound timer to vx
                case 0x1E: IR += V[x]; break; //add vx to ir
                case 0x29: IR = (ushort)(5 * V[x]); break; //load sprite to i from vx
                case 0x33: { //store bcd of vx in i
                    byte val = V[x];
                    Memory.Raw[IR + 2] = (byte)(val % 10); val /= 10;
                    Memory.Raw[IR + 1] = (byte)(val % 10); val /= 10;
                    Memory.Raw[IR] = (byte)(val % 10);
                    break;
                }
                case 0x55: { //set V0-vx to I
                    for (int i = 0; i <= x; i++)
                        Memory.Raw[IR + i] = V[i];
                    break;
                }
                case 0x65: { //set V0-vx from I
                    for (int i = 0; i <= x; i++)
                        V[i] = Memory.Raw[IR + i];
                    break;
                }
                }
                Debug.WriteLine($"SYSCALL {CurrentInstruction & 0x0FFF:X4} not implemented!");
                break;
            }
            default:
            unresolved:
                Debug.WriteLine($"Unknown instruction: {CurrentInstruction:X4}"); break;
            }
        }
    }

    public class CHIP8 {
        public CPU CPU;
        public int OperationsPerSec;
        private DateTime LastUpdate;
        private int HandledInstructions;

        public void Initialize() {
            CPU = new CPU {
                PC = Constants.RomStart
            };
            CPU.Memory.Write(FontBuffer, 0);
            OperationsPerSec = 0;
            LastUpdate = DateTime.Now;
        }

        public void LoadROM(byte[] raw) {
            Initialize();
            CPU.Memory.Write(raw, Constants.RomStart);
        }

        public void Update() {
            // For the disassembler
            if ((DateTime.Now - LastUpdate).TotalSeconds > 1) {
                OperationsPerSec = HandledInstructions;
                HandledInstructions = 0;
                LastUpdate = DateTime.Now;
            } else HandledInstructions++;

            CPU.Update();
        }

        public byte[] FontBuffer = new byte[] {
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