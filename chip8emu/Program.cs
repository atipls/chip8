using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chip8emu {
    static class Program {
        public static byte[] binary;
        [STAThread]
        static void Main(string[] args) {
            if (args.Length > 0) {
                if (args[0] == "--raw") {
                    if (args.Length < 2) {
                        MessageBox.Show("invalid / non existent base64 encoded data!", "");
                        return;
                    }
                    try {
                        binary = Convert.FromBase64String(args[1]);
                    } catch (Exception ex) { MessageBox.Show(ex.Message.ToLower(), "Exception!"); return; }
                } else {
                    var path = args.First();
                    if (!File.Exists(path)) {
                        MessageBox.Show("select a valid file!", "");
                        return;
                    }
                    binary = File.ReadAllBytes(path);
                }
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new display());
        }
    }
}
