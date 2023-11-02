using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace PlagueGun
{
    public class ThingDef_PlagueBullet : ThingDef
    {
        public float AddHediffChance = 0.05f;
        public HediffDef HediffToAdd;
    }
}
