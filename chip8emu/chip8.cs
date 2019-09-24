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
        public byte[] registers = new byte[16]; //V0-VF
        public ushort ir = 0; //index register
        public ushort pc = 0; //program counter
        public byte sp = 0; //stack pointer
        public byte delay_timer = 0;
        public byte sound_timer = 0;
        ushort instr = 0;
        public memory memory;
        public video video;

        Random random;
        Dictionary<int, Action> handlers;
        Dictionary<int, Action> handlers_8;
        Dictionary<int, Action> handlers_f;
        public cpu() {
            memory = new memory();
            video = new video();
            random = new Random();
            handlers = new Dictionary<int, Action>() {
                { 0  , table_0 },
                { 1  , op_1nnn },
                { 2  , op_2nnn },
                { 3  , op_3xkk },
                { 4  , op_4xkk },
                { 5  , op_5xy0 },
                { 6  , op_6xkk },
                { 7  , op_7xkk },
                { 8  , table_8 },
                { 9  , op_9xy0 },
                { 0xA, op_annn },
                { 0xB, op_bnnn },
                { 0xC, op_cxnn },
                { 0xD, op_dxyn },
                { 0xE, table_e },
                { 0xF, table_f },
            };
            handlers_8 = new Dictionary<int, Action>() {
                { 0  , op_8xy0 },
                { 1  , op_8xy1 },
                { 2  , op_8xy2 },
                { 3  , op_8xy3 },
                { 4  , op_8xy4 },
                { 5  , op_8xy5 },
                { 6  , op_8xy6 },
                { 7  , op_8xy7 },
                { 0xE, op_8xye },
            };
            handlers_f = new Dictionary<int, Action>() {
                { 7   , op_fx07 },
                { 0xA , op_fx0a },
                { 0x15, op_fx15 },
                { 0x18, op_fx18 },
                { 0x1E, op_fx1e },
                { 0x29, op_fx29 },
                { 0x33, op_fx33 },
                { 0x55, op_fx55 },
                { 0x65, op_fx65 },
            };
        }
        public void update() {
            instr = memory.get16(pc);
            pc += 2;
            handlers[(byte)((instr & 0xF000) >> 12)]();
            if (delay_timer > 0)
                delay_timer--;
            if (sound_timer > 0)
                sound_timer--;
        }
        void table_0() {
            switch (instr & 0x000F) {
                case 0x0000: op_00e0(); return;
                case 0x000E: op_00ee(); return;
                default: Debug.WriteLine("Unknown instruction!"); return;
            }
        }
        void table_e() {
            switch (instr & 0x000F) {
                case 0x0001: op_exa1(); return;
                case 0x000E: op_ex9e(); return;
                default: Debug.WriteLine("Unknown instruction!"); return;
            }
        }
        void table_8() { handlers_8[instr & 0x000F](); }
        void table_f() { if (handlers_f.ContainsKey(instr & 0x00FF)) handlers_f[instr & 0x00FF](); else Debug.WriteLine("SYS not implemented!"); }
        void op_1nnn() { pc = (ushort)(instr & 0x0FFF); } //jump
        void op_2nnn() { memory.stack[sp++] = pc; pc = (ushort)(instr & 0xFFF); } //call
        void op_3xkk() { if (registers[(instr & 0x0F00) >> 8] == (byte)(instr & 0x00FF)) pc += 2; } //skip next instruction if equal to byte
        void op_4xkk() { if (registers[(instr & 0x0F00) >> 8] != (byte)(instr & 0x00FF)) pc += 2; } //skip next instruction if not equal to byte
        void op_5xy0() { if (registers[(instr & 0x0F00) >> 8] == registers[(instr & 0x00F0) >> 4]) pc += 2; } //skip next instruction if equal to register
        void op_6xkk() { registers[(instr & 0x0F00) >> 8] = (byte)(instr & 0x00FF); } //set register to value
        void op_7xkk() { registers[(instr & 0x0F00) >> 8] += (byte)(instr & 0x00FF); } //add value to register
        void op_9xy0() { if (registers[(instr & 0x0F00) >> 8] != registers[(instr & 0x00F0) >> 4]) pc += 2; } //skip next instruction if not equal to register
        void op_annn() { ir = (ushort)(instr & 0x0FFF); }
        void op_bnnn() { pc = (ushort)(registers[0] + (ushort)(instr & 0x0FFF)); } //jump to v0 + value
        void op_cxnn() { registers[(instr & 0x0F00) >> 8] = (byte)(random.Next(0, 255) & (byte)(instr & 0x00FF)); }
        void op_00e0() { Array.Clear(video.raw, 0, video.raw.Length); }
        void op_00ee() { pc = memory.stack[--sp]; } //return
        void op_exa1() { if (!keys[registers[(instr & 0x0F00) >> 8]]) pc += 2; } //skip next instruction if key Vx is not pressed
        void op_ex9e() { if (keys[registers[(instr & 0x0F00) >> 8]]) pc += 2; } //skip next instruction if key Vx is pressed
        void op_8xy0() { registers[(instr & 0x0F00) >> 8] = registers[(instr & 0x00F0) >> 4]; } //set Vx to Vy
        void op_8xy1() { registers[(instr & 0x0F00) >> 8] |= registers[(instr & 0x00F0) >> 4]; } //or Vx with Vy
        void op_8xy2() { registers[(instr & 0x0F00) >> 8] &= registers[(instr & 0x00F0) >> 4]; } //and Vx with Vy
        void op_8xy3() { registers[(instr & 0x0F00) >> 8] ^= registers[(instr & 0x00F0) >> 4]; } //xor Vx with Vy
        void op_8xy4() { //add with carry
            ushort val = (ushort)(registers[(instr & 0x0F00) >> 8] + registers[(instr & 0x00F0) >> 4]);
            if (val > 255)
                registers[0xF] = 1;
            else registers[0xF] = 0;
            registers[(instr & 0x0F00) >> 8] = (byte)(val & 0xFF);
        }
        void op_8xy5() { //subtract with borrow
            if (registers[(instr & 0x0F00) >> 8] > registers[(instr & 0x00F0) >> 4])
                registers[0xF] = 1;
            else registers[0xF] = 0;
            registers[(instr & 0x0F00) >> 8] -= registers[(instr & 0x00F0) >> 4];
        }
        void op_8xy6() { registers[0xF] = (byte)(registers[(instr & 0x0F00) >> 8] & 1); registers[(instr & 0x0F00) >> 8] >>= 1; } //shift and set Vx to lsb
        void op_8xy7() { //subtract with not borrow
            if (registers[(instr & 0x00F0) >> 4] > registers[(instr & 0x0F00) >> 8])
                registers[0xF] = 1;
            else registers[0xF] = 0;
            registers[(instr & 0x0F00) >> 8] = (byte)(registers[(instr & 0x00F0) >> 4] - registers[(instr & 0x0F00) >> 8]);
        }
        void op_8xye() { registers[0xF] = (byte)((registers[(instr & 0x0F00) >> 8] & 0x80) >> 7); registers[(instr & 0x0F00) >> 8] <<= 1; } //shift and set Vx to msb
        void op_fx07() { registers[(instr & 0x0F00) >> 8] = delay_timer; } //load delay timer to Vx
        void op_fx0a() { //wait for key, then set the key idx to Vx
            for (int i = 0; i < 16; i++) {
                if (keys[i]) {
                    registers[(instr & 0x0F00) >> 8] = (byte)i;
                    return;
                }
            }
            pc -= 2;
        }
        void op_fx15() { delay_timer = registers[(instr & 0x0F00) >> 8]; } //set delay timer to Vx 
        void op_fx18() { sound_timer = registers[(instr & 0x0F00) >> 8]; } //set sound timer to Vx
        void op_fx1e() { ir += registers[(instr & 0x0F00) >> 8]; } //add Vx to ir
        void op_fx29() { ir = (ushort)(5 * registers[(instr & 0x0F00) >> 8]); }
        void op_fx33() { //set bcd to ir,ir+1,ir+2
            byte val = registers[(instr & 0x0F00) >> 8];
            memory.raw[ir + 2] = (byte)(val % 10); val /= 10;
            memory.raw[ir + 1] = (byte)(val % 10); val /= 10;
            memory.raw[ir] = (byte)(val % 10);
        }
        void op_fx55() { for (int i = 0; i <= (instr & 0x0F00) >> 8; i++) memory.raw[ir + i] = registers[i]; } //set V0-Vx to I
        void op_fx65() { for (int i = 0; i <= (instr & 0x0F00) >> 8; i++) registers[i] = memory.raw[ir + i]; } //set V0-Vx from I
        void op_dxyn() { //draw
            byte x = registers[(instr & 0x0F00) >> 8];
            byte y = registers[(instr & 0x00F0) >> 4];
            registers[0xF] = 0;
            for (int row = 0; row < (instr & 0x000F); row++) {
                byte sprite = memory.raw[ir + row];
                for (int col = 0; col < 8; col++) {
                    byte pixel = (byte)(sprite & (0x80 >> col));
                    byte dx = (byte)((x + col) % constants.x_size);
                    byte dy = (byte)((y + row) % constants.y_size);
                    if (pixel != 0) {
                        if (video.raw[dx, dy])
                            registers[0xF] = 1;
                        video.raw[dx, dy] ^= true;
                        video.draw = true;
                    }
                }
            }
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
