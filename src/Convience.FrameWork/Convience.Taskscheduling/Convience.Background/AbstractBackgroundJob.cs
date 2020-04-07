using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Convience.Background
{
    public abstract class AbstractBackgroundJob
    {
        public int Days { get; set; } = 0;
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 0;
        public int Seconds { get; set; } = 1;

        public abstract Task DoWork();
    }
}
