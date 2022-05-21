using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Lab
{
    public class Average : List<long>
    {
        public long GetTotalThicks()
            => this.Sum();

        public long GetTotalMilliseconds()
            => GetTotalThicks() / TimeSpan.TicksPerMillisecond;
        public long GetAverageTicks()
            => GetTotalThicks() / Count;        

        public long GetAverageMilliseconds()
            => GetAverageTicks() / TimeSpan.TicksPerMillisecond;
        
    }
}
