using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSniper
{
    public class Hyperlink
    {
        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public string Text { get; set; }

        public Hyperlink(int sindex, int eindex, string stext)
        {
            StartIndex = sindex;
            EndIndex = eindex;
            Text = stext;
        }
        public override string ToString()
        {
            return $"{StartIndex},{EndIndex}={Text}";
        }
    }
}
