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
            public static string run(ushort instr) {
                var x = (instr & 0x0F00) >> 8;
                var y = (instr & 0x00F0) >> 4;
                var nn = instr & 0x00FF;
                var nnn = instr & 0x0FFF;

                switch ((instr & 0xF000) >> 12) {
                    case 0:
                        switch (instr & 0x000F) {
                            case 0x0000: return "CLS"; //clear screen
                            case 0x000E: return "RET"; //return from subroutine
                        }
                        goto default;
                    case 1: return $"JMP ${nnn:X4}"; //jump
                    case 2: return $"JSR ${nnn:X4}"; //call
                    case 3: return $"SEQ V{x:X}, ${nn:X2}"; ; //skip next instruction if equal to byte
                    case 4: return $"SNE V{x:X}, ${nn:X2}"; ; //skip next instruction if not equal to byte
                    case 5: return $"SEQ V{x:X}, V{y:X}"; //skip next instruction if equal to register
                    case 6: return $"SET V{x:X}, ${nn:X2}"; //set register to value
                    case 7: return $"ADD V{x:X}, ${nn:X2}"; //add value to register
                    case 8:
                        switch (instr & 0x000F) {
                            case 0: return $"SET V{x:X}, V{y:X}"; //set vx to vy
                            case 1: return $"OR  V{x:X}, V{y:X}"; //or vx with vy
                            case 2: return $"AND V{x:X}, V{y:X}"; //and vx with vy
                            case 3: return $"XOR V{x:X}, V{y:X}"; //xor vx with vy
                            case 4: return $"ADD V{x:X}, V{y:X}"; //add vx to vy
                            case 5: return $"SUB V{x:X}, V{y:X}"; //subtract vx from vy
                            case 6: return $"SHR V{x:X}"; //shift and set vx to lsb
                            case 7: return $"SNB V{y:X}, V{x:X}"; //set vx to vy-vx
                            case 0xE: return $"SHL V{x:X}"; //shift and set vx to msb
                        }
                        goto default;
                    case 9: return $"JNE V{x:X}, V{y:X}";  //skip next instruction if not equal to register
                    case 0xA: return $"SET IR, ${nnn:X4}"; //set ir to value
                    case 0xB: return $"JRE ${nnn:X4}"; //jump to v0 + value
                    case 0xC: return $"RND V{x:X}, ${nn:X2}"; //set register to RAND&NN
                    case 0xD: return $"DRW V{x:X}, V{y:X}, ${instr & 0x000F:X2}"; //draw at vx, vy, height n
                    case 0xE:
                        switch (instr & 0x000F) {
                            case 0x0001: return $"SKN V{x:X}"; //skip next instruction if key vx is not pressed
                            case 0x000E: return $"SKK V{x:X}"; //skip next instruction if key vx is pressed
                        }
                        goto default;
                    case 0xF:
                        switch (instr & 0x00FF) {
                            case 7: return $"LDT V{x:X}"; //set vx to delay timer
                            case 0xA: return $"WKY V{x:X}"; //wait for key and place it to vx (blocking)
                            case 0x15: return $"SDT V{x:X}"; //set delay timer to vx
                            case 0x18: return $"SST V{x:X}"; //set sound timer to vx
                            case 0x1E: return $"ADD IR, V{x:X}"; //add vx to ir
                            case 0x29: return $"LSP V{x:X}"; //load sprite to i from vx
                            case 0x33: return $"BCD V{x:X}"; //store bcd of vx in ir
                            case 0x55: return $"STO V{x:X}, IR"; //set V0-vx to ir
                            case 0x65: return $"STO IR, V{x:X}"; //set V0-vx from ir
                        }
                        return $"SYS ${instr & 0x0FFF:X2}";
                    default: return $"RAW ${instr:X2}";
                }
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

            Text = $"chip8 disassembler";
            pc.Text = $"PC: {chip.cpu.pc:X}";
            sp.Text = $"SP: {chip.cpu.sp:X}";
            ir.Text = $"IR: {chip.cpu.ir:X}";

            dt.Text = $"DelayTimer: {chip.cpu.delay_timer}";
            st.Text = $"SoundTimer: {chip.cpu.sound_timer}";

            lb_exec.Text = $"Executing: \n{(chip.cpu.halt ? "Nothing" : printer.run(chip.cpu.memory.get16(chip.cpu.pc)))}";

            for (int i = 0; i < 16; i++)
                lbregs[i].Text = $"V{i:X}: {chip.cpu.v[i]}";

            stack.Text = "Stack: \n";
            if (chip.cpu.sp != 0) {
                for (int i = 0; i < chip.cpu.sp; i++)
                    stack.Text += $"{chip.cpu.memory.stack[i]:X2}\n";
            } else stack.Text += "Empty.";
            if (!hb_mem.IsDisposed)
                hb_mem.ByteProvider = new Be.Windows.Forms.DynamicByteProvider(chip.cpu.memory.raw);
            //tb_hex.Text = BitConverter.ToString(chip.cpu.memory.raw).Replace('-', ' ');
            if (!chip.cpu.halt) {
                ops.Text = $"Operations Per Second: {chip.ops}";
            } else ops.Text = $"Halted!";
        }
    }
}
