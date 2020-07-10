using System;
using System.Collections.Generic;
using System.Text;

namespace SwagLyricsGUI.Models
{
    public static class MathEx
    {
        public static double Lerp(double start, double end, double t)
        {
            return (1 - t) * start + t * end;
        }
    }
}
