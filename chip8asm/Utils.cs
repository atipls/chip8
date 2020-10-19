using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHIP8.Asm {
    public static class Utils {
        public static T ParseEnum<T>(this string data) where T : Enum =>
            (T)Enum.Parse(typeof(T), data, true);
    }
}
