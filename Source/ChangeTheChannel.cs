using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace ChangeTheChannel
{
    public class ChangeTheChannel : Mod
    {
        public static ChangeTheChannel_Settings settings;
        public ChangeTheChannel(ModContentPack content) : base(content)
        {
            settings = GetSettings<ChangeTheChannel_Settings>();
        }
        public override string SettingsCategory()
        {
            return "Change The Channel!";
        }
        public override void DoSettingsWindowContents(Rect inRect)
        {
            settings.DoWindowContents(inRect);
        }
    }
}
