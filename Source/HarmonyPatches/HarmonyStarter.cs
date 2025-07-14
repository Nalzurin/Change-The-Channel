using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ChangeTheChannel.HarmonyPatches
{
    [StaticConstructorOnStartup]
    public static class HarmonyStarter
    {
        static HarmonyStarter()
        {
            Harmony harmony = new Harmony("Nalzurin.ChangeTheChannel");
            harmony.PatchAll();
        }
    }
}
