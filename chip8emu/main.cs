using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chip8emu {
    static class main {
        public static string path;
        [STAThread]
        static void Main(string[] args) {
            if (args.Length > 0)
                path = args.First();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new display());
        }
    }
}
