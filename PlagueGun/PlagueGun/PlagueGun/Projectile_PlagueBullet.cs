using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PlagueGun;
using RimWorld;
using Verse;

namespace PlagueGun
{
    public class Projectile_PlagueBullet : Bullet
    {
        public ModExtension_PlagueBullet Props => def.GetModExtension<ModExtension_PlagueBullet>();

        protected override void Impact(Thing hitThing, bool blockedByShield = false) 
        {
            base.Impact(hitThing, blockedByShield);

            if (Props != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                float rand = Rand.Value;
                if (rand <= Props.addHediffChance)
                {
                    Messages.Message("TST_PlagueBullet_SuccessMessage".Translate(
                            this.launcher.Label, hitPawn.Label
                        ), MessageTypeDefOf.NeutralEvent);
                    Hediff plagueOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Props.hediffToAdd);
                    float randomSeverity = Rand.Range(0.15f, 0.30f);

                    if (plagueOnPawn != null)
                    {
                        plagueOnPawn.Severity += randomSeverity;
                        return;
                    }

                    Hediff hediff = HediffMaker.MakeHediff(Props.hediffToAdd, hitPawn);
                    hediff.Severity = randomSeverity;
                    hitPawn.health.AddHediff(hediff);

                    return;
                }

                MoteMaker.ThrowText(
                    hitThing.PositionHeld.ToVector3(),
                    hitThing.MapHeld,
                    "TST_PlagueBullet_FailureMote".Translate(Props.addHediffChance),
                    12f
                );
            }
        }
    }
}
