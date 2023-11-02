using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using Verse;

namespace PlagueGun
{
    public class Projectile_PlagueBullet : Bullet
    {
        #region Properties
        public ThingDef_PlagueBullet Def
        {
            get
            {
                return this.def as ThingDef_PlagueBullet;
            }
        }
        #endregion Properties

        #region Overrides
        protected override void Impact(Thing hitThing, bool blockedByShield = false) 
        {
            base.Impact(hitThing);

            if (Def != null && hitThing != null && hitThing is Pawn hitPawn)
            {
                string translatedMessageSuccess = TranslatorFormattedStringExtensions.Translate(
                    "PlagueBullet_SuccessMessage",
                    launcher.Label,
                    hitPawn.Label,
                    Def.HediffToAdd.label
                );

                string translatedMessageFailure = TranslatorFormattedStringExtensions.Translate(
                    "PlagueBullet_FailureMote",
                    Def.AddHediffChance.ToString("P")
                );

                var rand = Rand.Value;
                if (rand <= Def.AddHediffChance)
                {
                    Messages.Message(
                        translatedMessageSuccess, 
                        MessageTypeDefOf.NegativeHealthEvent
                    );
                    Hediff plagueOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);
                    float randomSeverity = Rand.Range(0.15f, 0.30f);

                    if (plagueOnPawn != null)
                    {
                        plagueOnPawn.Severity += randomSeverity;
                        return;
                    }

                    Hediff hediff = HediffMaker.MakeHediff(Def.HediffToAdd, hitPawn);
                    hediff.Severity = randomSeverity;
                    hitPawn.health.AddHediff(hediff);
                }

                MoteMaker.ThrowText(
                    hitThing.PositionHeld.ToVector3(), 
                    hitThing.MapHeld, 
                    translatedMessageFailure, 
                    2f
                );
            }
        }
        #endregion Overrides
    }
}
