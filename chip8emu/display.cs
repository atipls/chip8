using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chip8emu {
    public partial class display : Form {
        chip8 chip8 = new chip8();
        disasm disasm;
        Dictionary<Keys, int> keymap = new Dictionary<Keys, int>() {
            { Keys.D1, 0x1 },
            { Keys.D2, 0x2 },
            { Keys.D3, 0x3 },
            { Keys.D4, 0xC },
            { Keys.Q, 0x4 },
            { Keys.W, 0x5 },
            { Keys.E, 0x6 },
            { Keys.R, 0xD },
            { Keys.A, 0x7 },
            { Keys.S, 0x8 },
            { Keys.D, 0x9 },
            { Keys.F, 0xE },
            { Keys.Y, 0xA },
            { Keys.X, 0x0 },
            { Keys.C, 0xB },
            { Keys.V, 0xF },
        };
        public display() {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            chip8.load(main.path ?? "opcode_test2.bin");
            disasm = new disasm(chip8);
            disasm.Show();
        }
        protected override void OnFormClosing(FormClosingEventArgs e) {
            disasm.Close();
            disasm.Dispose();
            Process.GetCurrentProcess().Kill(); //yeah not the most elegant way to end a program is it
            base.OnFormClosing(e);
        }
        protected override async void OnPaint(PaintEventArgs e) {
            var then = DateTime.Now;
            while (!chip8.cpu.video.draw) {
                Application.DoEvents();
                chip8.update();
                disasm.update();
            }
            for (int x = 0; x < constants.x_size; x++) {
                for (int y = 0; y < constants.y_size; y++) {
                    if (chip8.cpu.video.raw[x, y])
                        e.Graphics.FillRectangle(Brushes.White, x * 5, y * 5, 5, 5);
                    else e.Graphics.FillRectangle(Brushes.DarkCyan, x * 5, y * 5, 5, 5);
                }
            }
            chip8.cpu.video.draw = false;
            await Task.Delay(Math.Max(1000 / 60 - (int)(DateTime.Now - then).TotalMilliseconds, 0)); //60 fps
            Invalidate();
        }

        private void on_key_down(object sender, KeyEventArgs e) {
            if (keymap.ContainsKey(e.KeyCode))
                chip8.cpu.keys[keymap[e.KeyCode]] = true;
        }
        private void on_key_up(object sender, KeyEventArgs e) {
            if (keymap.ContainsKey(e.KeyCode))
                chip8.cpu.keys[keymap[e.KeyCode]] = false;
        }
    }
}
