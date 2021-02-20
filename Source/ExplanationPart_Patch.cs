using HarmonyLib;
using System.Text;
using RimWorld;
using Verse;
using System.Linq;

namespace TemperatureByRace
{
    [HarmonyPatch(typeof(StatPart_WorkTableTemperature), "ExplanationPart")]
    public class ExplanationPart_Patch
    {
        public static void Postfix(StatRequest req, ref string __result)
        {
            if (__result != null && req.HasThing && req.Thing != null)
            {
                if (req.Thing is Building_WorkTable table)
                {
                    Pawn pawn = table.BillInteractionCell.GetThingList(table.Map).Where(x => x is Pawn).Cast<Pawn>().Where(x => x.CurJob?.bill != null && table.billStack.Bills.Contains(x.CurJob.bill)).FirstOrDefault();
                    if (pawn != null)
                    {
                        __result = "BadTemperature".Translate().CapitalizeFirst() + $" (for {pawn.kindDef.label}): x{0.7f.ToStringPercent()} ";
                    }
                }
            }
        }
    }
}
