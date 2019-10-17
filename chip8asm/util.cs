using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chip8asm {
    public static class util {
        public static t parse<t>(this string str) where t : Enum =>
            (t)Enum.Parse(typeof(token.tt), str, true);
    }
}
