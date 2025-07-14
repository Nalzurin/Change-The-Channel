using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ChangeTheChannel
{
    public class ChangeTheChannel_Settings : ModSettings
    {
        public static int minTargetTicks;
        private string minTargetTicksBuffer;
        public static int maxTargetTicks;
        private string maxTargetTicksBuffer;

        public void DoWindowContents(Rect inRect)
        {
            Listing_Standard ls = new Listing_Standard();
            ls.Begin(inRect);
            ls.Label("MinimumTargetTicksForEvent".Translate() + minTargetTicks.ToStringTicksToPeriod());
            ls.TextFieldNumeric(ref minTargetTicks, ref minTargetTicksBuffer, 100);
            ls.Label("MaximumTargetTicksForEvent".Translate() + maxTargetTicks.ToStringTicksToPeriod());
            ls.TextFieldNumeric(ref maxTargetTicks, ref maxTargetTicksBuffer, 100);
            ls.End();

        }
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref minTargetTicks, "minTargetTicks", 30000);
            Scribe_Values.Look(ref maxTargetTicks, "maxTargetTicks", 60000);
        }
    }
}
