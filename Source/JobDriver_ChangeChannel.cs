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
    public class JobDriver_ChangeChannel : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }
        protected override IEnumerable<Toil> MakeNewToils()
        {
            CompTVChannels comp = TargetA.Thing.TryGetComp<CompTVChannels>();
            Log.Message("test");
            this.FailOnDespawnedOrNull(TargetIndex.A);
            this.FailOnForbidden(TargetIndex.A);
            this.FailOn(() =>
            {
                return comp == null;
            });
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
            yield return Toils_General.Wait(15).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            Toil finalize = ToilMaker.MakeToil("MakeNewToils");
            finalize.initAction = delegate
            {
                comp.currentChannel = comp.wantedChannel;
            };
            finalize.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return finalize;
        }

    }
}
