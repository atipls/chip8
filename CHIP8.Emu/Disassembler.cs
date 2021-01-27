using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHIP8.Emu {
    public partial class Disassembler : Form {
        readonly CHIP8 Chip;

        static class Printer {
            public static string Run(ushort Instruction) {
                var x = (Instruction & 0x0F00) >> 8;
                var y = (Instruction & 0x00F0) >> 4;
                var nn = Instruction & 0x00FF;
                var nnn = Instruction & 0x0FFF;

                switch ((Instruction & 0xF000) >> 12) {
                    case 0:
                        switch (Instruction & 0x000F) {
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
                        switch (Instruction & 0x000F) {
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
                    case 0xD: return $"DRW V{x:X}, V{y:X}, ${Instruction & 0x000F:X2}"; //draw at vx, vy, height n
                    case 0xE:
                        switch (Instruction & 0x000F) {
                            case 0x0001: return $"SKN V{x:X}"; //skip next instruction if key vx is not pressed
                            case 0x000E: return $"SKK V{x:X}"; //skip next instruction if key vx is pressed
                        }
                        goto default;
                    case 0xF:
                        switch (Instruction & 0x00FF) {
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
                        return $"SYS ${Instruction & 0x0FFF:X2}";
                    default: return $"RAW ${Instruction:X2}";
                }
            }
        }

        public Disassembler(CHIP8 chip) {
            InitializeComponent();
            Chip = chip;
            for (int i = 0; i < Constants.RAMSize / 2; i += 2) {
                var instruction = chip.CPU.Memory.Get16(i);
                InstructionList.Items.Add(new ListViewItem(new string[] { $"{i:X4} [{instruction:X4}]", Printer.Run(instruction) }));
            }
        }

        public void DisasmUpdate() {
            Label[] RegLabels = { v0, v1, v2, v3, v4, v5, v6, v7, v8, v9, va, vb, vc, vd, ve, vf };

            Text = $"chip8 disassembler";
            PC.Text = $"PC: {Chip.CPU.PC:X}";
            SP.Text = $"SP: {Chip.CPU.SP:X}";
            IR.Text = $"IR: {Chip.CPU.IR:X}";

            DT.Text = $"DelayTimer: {Chip.CPU.DelayTimer}";
            ST.Text = $"SoundTimer: {Chip.CPU.SoundTimer}";

            State.Text = $"Executing: \n{(Chip.CPU.Halting ? "Nothing" : Printer.Run(Chip.CPU.Memory.Get16(Chip.CPU.PC)))}";

            for (int i = 0; i < 16; i++)
                RegLabels[i].Text = $"V{i:X}: {Chip.CPU.V[i]}";

            Stack.Text = "Stack: \n";
            if (Chip.CPU.SP != 0) {
                for (int i = 0; i < Chip.CPU.SP; i++)
                    Stack.Text += $"{Chip.CPU.Memory.Stack[i]:X2}\n";
            } else Stack.Text += "Empty.";
            if (!MemoryHex.IsDisposed)
                MemoryHex.ByteProvider = new Be.Windows.Forms.DynamicByteProvider(Chip.CPU.Memory.Raw);
            // MemoryText.Text = BitConverter.ToString(chip.cpu.memory.raw).Replace('-', ' ');
            if (!Chip.CPU.Halting) {
                OperationsPerSec.Text = $"Operations Per Second: {Chip.OperationsPerSec}";
            } else OperationsPerSec.Text = $"Halted!";
        }

    }
}
