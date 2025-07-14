using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ChangeTheChannel
{
    public class CompProperties_TVChannels : CompProperties
    {
        public List<TVChannel> channels = [];
        public CompProperties_TVChannels()
        {
            compClass = typeof(CompTVChannels);
        }
    }
}
