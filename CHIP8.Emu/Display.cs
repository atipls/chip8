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

namespace CHIP8.Emu {
    public partial class Display : Form {
        readonly CHIP8 CHIP8 = new CHIP8();
        readonly Disassembler Disassembler;
        readonly Dictionary<Keys, int> ChipKeymap = new Dictionary<Keys, int>() {
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

        public Display() {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            CHIP8.LoadROM(Program.Binary ?? new byte[] { 0x00 });
            Disassembler = new Disassembler(CHIP8);
            Disassembler.Show();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (components != null)
                    components.Dispose();
                if (Disassembler != null)
                    Disassembler.Dispose();
            }
            base.Dispose(disposing);
        }

        protected override void OnFormClosing(FormClosingEventArgs e) {
            Process.GetCurrentProcess().Kill();
            base.OnFormClosing(e);
        }

        private Brush ChipFore = Brushes.White;
        private Brush ChipBack = Brushes.DarkCyan;

        protected override async void OnPaint(PaintEventArgs e) {
            var then = DateTime.Now;
            while (!CHIP8.CPU.Video.DoDraw) {
                Application.DoEvents();
                CHIP8.Update();
                Disassembler.DisasmUpdate();
            }
            for (int x = 0; x < Constants.ResolutionX; x++) {
                for (int y = 0; y < Constants.ResolutionY; y++) {
                    if (CHIP8.CPU.Video.Raw[x, y])
                        e.Graphics.FillRectangle(ChipFore, x * 5, y * 5, 5, 5);
                    else e.Graphics.FillRectangle(ChipBack, x * 5, y * 5, 5, 5);
                }
            }
            CHIP8.CPU.Video.DoDraw = false;
            await Task.Delay(Math.Max(1000 / 60 - (int)(DateTime.Now - then).TotalMilliseconds, 0)); // 60 fps
            Invalidate();
        }

        private void OnKeyDown(object sender, KeyEventArgs e) {
            if (ChipKeymap.ContainsKey(e.KeyCode))
                CHIP8.CPU.Keys[ChipKeymap[e.KeyCode]] = true;
            if (e.KeyCode == Keys.Escape)
                CHIP8.Initialize();
        }
        private void OnKeyUp(object sender, KeyEventArgs e) {
            if (ChipKeymap.ContainsKey(e.KeyCode))
                CHIP8.CPU.Keys[ChipKeymap[e.KeyCode]] = false;
        }

        private void ChipForeClick(object sender, EventArgs e) {
            using var cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                ChipFore = new SolidBrush(cd.Color);
        }

        private void ChipBackClick(object sender, EventArgs e) {
            using var cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
                ChipBack = new SolidBrush(cd.Color);
        }
        private void TimerTick(object sender, EventArgs e) => CHIP8.CPU.TimerUpdate();
    }
}
