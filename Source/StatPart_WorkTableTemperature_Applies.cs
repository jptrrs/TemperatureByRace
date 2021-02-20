using HarmonyLib;
using RimWorld;
using Verse;
using System;
using System.Linq;

namespace TemperatureByRace
{
    [HarmonyPatch(typeof(StatPart_WorkTableTemperature), "Applies", new Type[] { typeof(Thing) })]
    public class StatPart_WorkTableTemperature_Applies
    {
        public static bool Prefix(Thing t, ref bool __result)
        {
            if (t is Building_WorkTable table)
            {
                Pawn pawn = table.BillInteractionCell.GetThingList(table.Map).Where(x => x is Pawn).Cast<Pawn>().Where(x => x.CurJob?.bill != null && table.billStack.Bills.Contains(x.CurJob.bill)).FirstOrDefault();
                if (pawn != null)
                {
                    __result = Applies(t.def, t.Map, t.Position, pawn);
                }
                return false;
            }
            return true;
        }

        public static bool Applies(ThingDef tDef, Map map, IntVec3 c, Pawn pawn)
        {
            if (map == null)
            {
                return false;
            }
            if (tDef.building == null)
            {
                return false;
            }
            float temperatureForCell = GenTemperature.GetTemperatureForCell(c, map);
            float min = pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null);
            float max = pawn.def.GetStatValueAbstract(StatDefOf.ComfyTemperatureMax, null);
            bool result = temperatureForCell < min || temperatureForCell > max;
            if (result) Log.Message($"Bad temperature for {pawn}");
            return result;
        }

    }
}
