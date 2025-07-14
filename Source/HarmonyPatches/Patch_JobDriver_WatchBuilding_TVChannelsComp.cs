using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.AI;

namespace ChangeTheChannel.HarmonyPatches
{
    [HarmonyPatch]
    public static class Patch_JobDriver_WatchBuilding_TVChannelsComp
    {
        public static float DoChannelComp(float joyAmount, Building b, Pawn p, int delta)
        {
            //Log.Message("Test");
            if (b == null || p == null)
            {
                return joyAmount;
            }
            if (!b.HasComp<CompTVChannels>())
            {
                return joyAmount;
            }
            CompTVChannels comp = b.GetComp<CompTVChannels>();
            //Log.Message(comp.currentChannel == null);
            if (comp.currentChannel.skill == null)
            {
                return joyAmount;

            }
            if (!comp.pawnsWatching.Contains(p))
            {
                comp.pawnsWatching.Add(p);
            }
            p.skills.Learn(comp.currentChannel.skill, CTCHelper.expPerTick * delta);
            joyAmount *= p.skills.GetSkill(comp.currentChannel.skill).LearnRateFactor();
            return joyAmount;

        }
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(JobDriver_WatchBuilding), "WatchTickAction");
        }
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instr in instructions)
            {
                if (instr.Calls(AccessTools.Method(typeof(JoyUtility), "JoyTickCheckEnd")))
                {
                    //Log.Message("Found");
                    yield return new(OpCodes.Ldarg_0);
                    yield return new(OpCodes.Ldfld, AccessTools.Field(typeof(JobDriver), "pawn"));
                    yield return new(OpCodes.Ldarg_1);
                    yield return new(OpCodes.Call, AccessTools.Method(typeof(Patch_JobDriver_WatchBuilding_TVChannelsComp), "DoChannelComp"));
                    yield return new(OpCodes.Ldarg_0);
                    yield return new(OpCodes.Call, AccessTools.Method(typeof(JobDriver), "get_TargetThingA"));
                    yield return new(OpCodes.Castclass, typeof(Building));


                }

                yield return instr;
            }
        }
    }
}
