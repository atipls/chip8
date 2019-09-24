using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chip8emu {
    public partial class disasm : Form {
        readonly chip8 chip;

        static class printer {
            static Dictionary<int, Func<ushort, string>> handlers;
            static Dictionary<int, Func<ushort, string>> handlers_8;
            static Dictionary<int, Func<ushort, string>> handlers_f;
            static bool ready = false;
            static void setup() {
                handlers = new Dictionary<int, Func<ushort, string>>() {
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
                handlers_8 = new Dictionary<int, Func<ushort, string>>() {
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
                handlers_f = new Dictionary<int, Func<ushort, string>>() {
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
                ready = true;
            }
            static string table_0(ushort instr) {
                switch (instr & 0x000F) {
                    case 0x0000: return op_00e0(instr);
                    case 0x000E: return op_00ee(instr);
                }
                return $"RAW ${instr:X4}";
            }
            static string table_e(ushort instr) {
                switch (instr & 0x000F) {
                    case 0x0001: return op_exa1(instr);
                    case 0x000E: return op_ex9e(instr);
                }
                return $"RAW ${instr:X4}";
            }
            static string table_8(ushort instr) { return handlers_8.ContainsKey(instr & 0x000F) ? handlers_8[instr & 0x000F](instr) : $"RAW ${instr:X4}"; }
            static string table_f(ushort instr) { return handlers_f.ContainsKey(instr & 0x00FF) ? handlers_f[instr & 0x00FF](instr) : $"SYS ${instr & 0x0FFF:X4}"; }
            static string op_1nnn(ushort instr) { return $"JMP ${instr & 0x0FFF:X4}"; } //jump
            static string op_2nnn(ushort instr) { return $"JSR ${instr & 0x0FFF:X4}"; } //call
            static string op_3xkk(ushort instr) { return $"SEQ V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; } //skip next instruction if equal to byte
            static string op_4xkk(ushort instr) { return $"SNE V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; } //skip next instruction if not equal to byte
            static string op_5xy0(ushort instr) { return $"SEQ V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; } //skip next instruction if equal to register
            static string op_6xkk(ushort instr) { return $"SET V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; } //set register to value
            static string op_7xkk(ushort instr) { return $"ADD V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; } //add value to register
            static string op_9xy0(ushort instr) { return $"JNE V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; } //skip next instruction if not equal to register
            static string op_annn(ushort instr) { return $"SET IR, ${instr & 0x0FFF:X4}"; } //set ir to value
            static string op_bnnn(ushort instr) { return $"JRE ${instr & 0x0FFF:X4}"; } //jump to v0 + value
            static string op_cxnn(ushort instr) { return $"RND V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; }
            static string op_00e0(ushort _) { return $"CLS"; } //clear screen
            static string op_00ee(ushort _) { return $"RET"; } //return
            static string op_exa1(ushort instr) { return $"SKN V{(instr & 0x0F00) >> 8}"; } //skip next instruction if key Vx is not pressed
            static string op_ex9e(ushort instr) { return $"SKK V{(instr & 0x0F00) >> 8}"; } //skip next instruction if key Vx is pressed
            static string op_8xy0(ushort instr) { return $"SET V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; } //set Vx to Vy
            static string op_8xy1(ushort instr) { return $"OR  V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; } //or Vx with Vy
            static string op_8xy2(ushort instr) { return $"ADD V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; } //and Vx with Vy
            static string op_8xy3(ushort instr) { return $"XOR V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; } //xor Vx with Vy
            static string op_8xy4(ushort instr) { return $"ADD V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; }
            static string op_8xy5(ushort instr) { return $"SUB V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; }
            static string op_8xy6(ushort instr) { return $"SHR V{(instr & 0x0F00) >> 8}"; } //shift and set Vx to lsb
            static string op_8xy7(ushort instr) { return $"SNB V{(instr & 0x00F0) >> 4}, V{(instr & 0x0F00) >> 8}"; }
            static string op_8xye(ushort instr) { return $"SHL V{(instr & 0x0F00) >> 8}"; } //shift and set Vx to msb
            static string op_fx07(ushort instr) { return $"LDT V{(instr & 0x0F00) >> 8}"; } //load delay timer to Vx
            static string op_fx0a(ushort instr) { return $"WKY V{(instr & 0x0F00) >> 8}"; }
            static string op_fx15(ushort instr) { return $"SDT V{(instr & 0x0F00) >> 8}"; } //set delay timer to Vx 
            static string op_fx18(ushort instr) { return $"SST V{(instr & 0x0F00) >> 8}"; } //set sound timer to Vx
            static string op_fx1e(ushort instr) { return $"ADD IR, V{(instr & 0x0F00) >> 8}"; } //add Vx to ir
            static string op_fx29(ushort instr) { return $"LSP V{(instr & 0x0F00) >> 8}"; }
            static string op_fx33(ushort instr) { return $"BCD V{(instr & 0x0F00) >> 8}"; }
            static string op_fx55(ushort instr) { return $"STO V0-V{(instr & 0x0F00) >> 8}, I"; } //set V0-Vx to I
            static string op_fx65(ushort instr) { return $"STO I, V0-V{(instr & 0x0F00) >> 8}"; } //set V0-Vx from I
            static string op_dxyn(ushort instr) { return $"DRW V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}, {instr & 0x000F}"; }
            public static string run(ushort instr) {
                if (!ready)
                    setup();
                return handlers.ContainsKey((instr & 0xF000) >> 12) ?
                    handlers[(instr & 0xF000) >> 12](instr) :
                    $"RAW ${instr:X4}";
            }
        }

        public disasm(chip8 chip) {
            InitializeComponent();
            this.chip = chip;
            for (int i = 0; i < constants.size / 2; i += 2) {
                ushort instr = chip.cpu.memory.get16(i);
                lv_instructions.Items.Add(new ListViewItem(new string[] { $"{i.ToString("X")} [{instr.ToString("X")}]", printer.run(instr) }));
            }
        }
        public void update() {
            Label[] lbregs = { v0, v1, v2, v3, v4, v5, v6, v7, v8, v9, va, vb, vc, vd, ve, vf };

            Text = $"chip8 disasm.";
            pc.Text = $"PC: {chip.cpu.pc:X}";
            sp.Text = $"SP: {chip.cpu.sp:X}";
            ir.Text = $"IR: {chip.cpu.ir:X}";

            dt.Text = $"DelayTimer: {chip.cpu.delay_timer}";
            st.Text = $"SoundTimer: {chip.cpu.sound_timer}";

            lb_exec.Text = $"Executing: \n{printer.run(chip.cpu.memory.get16(chip.cpu.pc))}";

            for (int i = 0; i < 15; i++)
                lbregs[i].Text = $"V{i:X}: {chip.cpu.registers[i]}";

            stack.Text = "Stack: \n";
            if (chip.cpu.sp != 0) {
                for (int i = 0; i < chip.cpu.sp; i++)
                    stack.Text += $"{chip.cpu.memory.stack[i]:X4}\n";
            } else stack.Text += "Empty.";

            tb_hex.Text = BitConverter.ToString(chip.cpu.memory.raw).Replace('-', ' ');
            ops.Text = $"Operations Per Second: {chip.ops}";
        }
    }
}
