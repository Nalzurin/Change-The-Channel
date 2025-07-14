using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ChangeTheChannel
{

    public static class CTCHelper
    {
        public const float expPerTick = 0.004f;
        public static int GetTargetTicks()
        {
            return Rand.RangeInclusive(ChangeTheChannel_Settings.minTargetTicks, ChangeTheChannel_Settings.maxTargetTicks);
        }
        public static void RunEvent(TVChannelEvent tvEvent, List<Pawn> pawns)
        {

            if (tvEvent == null)
            {
                return;
            }
            if (pawns.NullOrEmpty())
            {
                return;
            }
            List<Pawn> affectedPawns = [];
            foreach (Pawn p in pawns)
            {
                foreach (EventTraitThought tt in tvEvent.eventTraitThoughts)
                {
                    if (p.story.traits.HasTrait(tt.traitDef, tt.degree))
                    {
                        p.needs.mood.thoughts.memories.TryGainMemory(tt.thoughtDef);
                        affectedPawns.Add(p);
                        break;
                    }

                }
            }
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(tvEvent.text);
            if (!affectedPawns.Empty())
            {
                stringBuilder.AppendLine("AffectedPawns".Translate());
                foreach (Pawn pawn in affectedPawns)
                {
                    stringBuilder.AppendLine("- " + pawn.LabelCap);
                }
            }
            else
            {
                stringBuilder.AppendLine("TVEventNoOneAffected".Translate());
            }

            DiaNode diaNode = new DiaNode(stringBuilder.ToString());
            DiaOption item = new DiaOption("OK".Translate())
            {
                resolveTree = true
            };
            diaNode.options.Add(item);
            Find.WindowStack.Add(new Dialog_NodeTree(diaNode, delayInteractivity: true, radioMode: true, "ChannelEvent".Translate()));
        }
    }
}
