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

            chip8.load(chip8emu.main.binary ?? new byte[] { 0x00 });
            disasm = new disasm(chip8);
            disasm.Show();
        }
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null)
                    components.Dispose();
                if (disasm != null)
                    disasm.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            Process.GetCurrentProcess().Kill(); //yeah not the most elegant way to end a program is it
            base.OnFormClosing(e);
        }
        private Brush color1 = Brushes.White;
        private Brush color2 = Brushes.DarkCyan;

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
                        e.Graphics.FillRectangle(color1, x * 5, y * 5, 5, 5);
                    else e.Graphics.FillRectangle(color2, x * 5, y * 5, 5, 5);
                }
            }
            chip8.cpu.video.draw = false;
            await Task.Delay(Math.Max(1000 / 60 - (int)(DateTime.Now - then).TotalMilliseconds, 0)); //60 fps
            Invalidate();
        }

        private void on_key_down(object sender, KeyEventArgs e) {
            if (keymap.ContainsKey(e.KeyCode))
                chip8.cpu.keys[keymap[e.KeyCode]] = true;
            if (e.KeyCode == Keys.Escape)
                chip8.init();
        }
        private void on_key_up(object sender, KeyEventArgs e) {
            if (keymap.ContainsKey(e.KeyCode))
                chip8.cpu.keys[keymap[e.KeyCode]] = false;
        }

        private void color1_click(object sender, EventArgs e) {
            using var cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                color1 = new SolidBrush(cd.Color);
        }

        private void color2_click(object sender, EventArgs e) {
            using var cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                color2 = new SolidBrush(cd.Color);
        }
        private void _60hz_Tick(object sender, EventArgs e) => chip8.cpu.timer_update();
    }
}
