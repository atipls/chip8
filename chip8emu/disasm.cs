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
                switch ((instr & 0xF000) >> 12) {
                    case 0:
                        switch (instr & 0x000F) {
                            case 0x0000: return "CLS"; //clear screen
                            case 0x000E: return "RET"; //return from subroutine
                        }
                        goto default;
                    case 1: return $"JMP ${instr & 0x0FFF:X4}"; //jump
                    case 2: return $"JSR ${instr & 0x0FFF:X4}"; //call
                    case 3: return $"SEQ V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; ; //skip next instruction if equal to byte
                    case 4: return $"SNE V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; ; //skip next instruction if not equal to byte
                    case 5: return $"SEQ V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; //skip next instruction if equal to register
                    case 6: return $"SET V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; //set register to value
                    case 7: return $"ADD V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; //add value to register
                    case 8:
                        switch (instr & 0x000F) {
                            case 0: return $"SET V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; //set vx to vy
                            case 1: return $"OR  V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; //or vx with vy
                            case 2: return $"AND V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; //and vx with vy
                            case 3: return $"XOR V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; //xor vx with vy
                            case 4: return $"ADD V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; //add vx to vy
                            case 5: return $"SUB V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}"; //subtract vx from vy
                            case 6: return $"SHR V{(instr & 0x0F00) >> 8}"; //shift and set vx to lsb
                            case 7: return $"SNB V{(instr & 0x00F0) >> 4}, V{(instr & 0x0F00) >> 8}"; //set vx to vy-vx
                            case 0xE: return $"SHL V{(instr & 0x0F00) >> 8}"; //shift and set vx to msb
                        }
                        goto default;
                    case 9: return $"JNE V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}";  //skip next instruction if not equal to register
                    case 0xA: return $"SET IR, ${instr & 0x0FFF:X4}"; //set ir to value
                    case 0xB: return $"JRE ${instr & 0x0FFF:X4}"; //jump to v0 + value
                    case 0xC: return $"RND V{(instr & 0x0F00) >> 8}, ${instr & 0x00FF:X4}"; //set register to RAND&NN
                    case 0xD: return $"DRW V{(instr & 0x0F00) >> 8}, V{(instr & 0x00F0) >> 4}, {instr & 0x000F}"; //draw at vx, vy, height n
                    case 0xE:
                        switch (instr & 0x000F) {
                            case 0x0001: return $"SKN V{(instr & 0x0F00) >> 8}"; //skip next instruction if key vx is not pressed
                            case 0x000E: return $"SKK V{(instr & 0x0F00) >> 8}"; //skip next instruction if key vx is pressed
                        }
                        goto default;
                    case 0xF:
                        switch (instr & 0x00FF) {
                            case 7: return $"LDT V{(instr & 0x0F00) >> 8}"; //set vx to delay timer
                            case 0xA: return $"WKY V{(instr & 0x0F00) >> 8}"; //wait for key and place it to vx (blocking)
                            case 0x15: return $"SDT V{(instr & 0x0F00) >> 8}"; //set delay timer to vx
                            case 0x18: return $"SST V{(instr & 0x0F00) >> 8}"; //set sound timer to vx
                            case 0x1E: return $"ADD IR, V{(instr & 0x0F00) >> 8}"; //add vx to ir
                            case 0x29: return $"LSP V{(instr & 0x0F00) >> 8}"; //load sprite to i from vx
                            case 0x33: return $"BCD V{(instr & 0x0F00) >> 8}"; //store bcd of vx in i
                            case 0x55: return $"STO V0-V{(instr & 0x0F00) >> 8}, I"; //set V0-vx to I
                            case 0x65: return $"STO I, V0-V{(instr & 0x0F00) >> 8}"; //set V0-vx from I
                        }
                        return $"SYS ${instr & 0x0FFF:X4}";
                    default: return $"RAW ${instr:X4}";
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

            Text = $"chip8 disasm.";
            pc.Text = $"PC: {chip.cpu.pc:X}";
            sp.Text = $"SP: {chip.cpu.sp:X}";
            ir.Text = $"IR: {chip.cpu.ir:X}";

            dt.Text = $"DelayTimer: {chip.cpu.delay_timer}";
            st.Text = $"SoundTimer: {chip.cpu.sound_timer}";

            lb_exec.Text = $"Executing: \n{printer.run(chip.cpu.memory.get16(chip.cpu.pc))}";

            for (int i = 0; i < 15; i++)
                lbregs[i].Text = $"V{i:X}: {chip.cpu.v[i]}";

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
