using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ChangeTheChannel
{
    public class CompTVChannels : ThingComp
    {
        private int currentEventTicks = 0;
        private int targetEventTicks;
        public List<Pawn> pawnsWatching = [];
        public List<Pawn> pawnsWatchingForReading => pawnsWatching;
        public bool isBeingWatched => !pawnsWatching.NullOrEmpty();

        CompProperties_TVChannels Props => (CompProperties_TVChannels)props;
        CompPowerTrader PowerTrader => parent.TryGetComp<CompPowerTrader>();
        public TVChannel currentChannel;
        public TVChannel wantedChannel = null;
        public CompTVChannels() { }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            currentChannel ??= Props.channels.First();
            if (targetEventTicks == 0)
            {
                targetEventTicks = CTCHelper.GetTargetTicks();
            }
        }
        public override void CompTick()
        {
            base.CompTick();
            if (parent.IsHashIntervalTick(40))
            {
                List<Pawn> list = [.. pawnsWatchingForReading];
                foreach (Pawn p in list)
                {
                    if (p.jobs.curJob.def != DefOfs.WatchTelevision)
                    {
                        pawnsWatching.Remove(p);
                    }
                }
            }

            if (isBeingWatched && !currentChannel.events.NullOrEmpty())
            {
                if (currentEventTicks >= targetEventTicks)
                {
                    CTCHelper.RunEvent(currentChannel.GetRandomEvent(), pawnsWatching);
                    currentEventTicks = 0;
                    targetEventTicks = CTCHelper.GetTargetTicks();
                }
                currentEventTicks++;

            }
        }

        public override string CompInspectStringExtra()
        {
            if (!PowerTrader.PowerOn)
            {
                return null;
            }
            if (currentChannel == null)
            {
                return null;
            }
            return "CurrentChannel".Translate(currentChannel.title, currentChannel.skill?.LabelCap ?? "Recreation".Translate());
        }
        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {
            foreach (FloatMenuOption floatMenuOption in base.CompFloatMenuOptions(selPawn))
            {
                yield return floatMenuOption;
            }
            if (!PowerTrader.PowerOn)
            {
                yield return new FloatMenuOption("CannotChangeChannel".Translate() + ": " + "NoPower".Translate().CapitalizeFirst(), null);
                yield break;
            }
            if (!selPawn.CanReach(parent, PathEndMode.InteractionCell, Danger.Deadly))
            {
                yield return new FloatMenuOption("CannotChangeChannel".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null);
                yield break;
            }
            foreach (TVChannel channel in Props.channels)
            {
                yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("ChangeChannel".Translate(channel.title, channel.skill?.LabelCap ?? "Recreation".Translate()), delegate
                {
                    //Log.Message(parent.Label);
                    wantedChannel = channel;
                    Job job = JobMaker.MakeJob(DefOfs.ChangeChannel, parent);
                    selPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
                }), selPawn, parent);
            }

        }
        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref currentEventTicks, "currentEventTicks", 0);
            Scribe_Values.Look(ref targetEventTicks, "targetEventTicks", 0);
            Scribe_Collections.Look(ref pawnsWatching, "pawnsWatching", LookMode.Reference);
            Scribe_Deep.Look(ref currentChannel, "currentChannel");
            if (wantedChannel != null)
            {
                Scribe_Deep.Look(ref wantedChannel, "wantedChannel");

            }
        }
    }
}
