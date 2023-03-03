using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using HarmonyLib;
using MyBhapticsTactsuit;
using UnityEngine;

[assembly: MelonInfo(typeof(PresentimentOfDeath_bhaptics.PresentimentOfDeath_bhaptics), "PresentimentOfDeath_bhaptics", "1.0.0", "Florian Fahrenberger")]
[assembly: MelonGame("Xocus", "PresentimentOfDeath")]

namespace PresentimentOfDeath_bhaptics
{
    public class PresentimentOfDeath_bhaptics : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        public static HurricaneVR.Framework.Core.Grabbers.HVRHandGrabber rightHand;
        public static bool rightHanded = true;

        public override void OnInitializeMelon()
        {
            tactsuitVr = new TactsuitVR();
            tactsuitVr.PlaybackHaptics("HeartBeat");
        }

        [HarmonyPatch(typeof(PlayerBow), "ShootArrow", new Type[] { typeof(Vector3) })]
        public class bhaptics_ShootBow
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerBow __instance)
            {
                if (rightHanded) tactsuitVr.PlaybackHaptics("BowRelease_R");
                else tactsuitVr.PlaybackHaptics("BowRelease_R");
            }
        }

        [HarmonyPatch(typeof(HurricaneVR.Framework.Weapons.Bow.HVRPhysicsBow), "OnStringGrabbed", new Type[] { typeof(HurricaneVR.Framework.Core.Grabbers.HVRHandGrabber), typeof(HurricaneVR.Framework.Core.HVRGrabbable) })]
        public class bhaptics_StringGrabbed
        {
            [HarmonyPostfix]
            public static void Postfix(HurricaneVR.Framework.Core.Grabbers.HVRHandGrabber hand)
            {
                rightHanded = (hand == rightHand);
            }
        }

        [HarmonyPatch(typeof(Player), "Start", new Type[] {  })]
        public class bhaptics_PlayerSpawn
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {
                rightHand = __instance.rightHand;
            }
        }

        [HarmonyPatch(typeof(Player), "ApplyDamage", new Type[] { typeof(float), typeof(IDamageable) })]
        public class bhaptics_PlayerDamage
        {
            [HarmonyPostfix]
            public static void Postfix(Player __instance)
            {
                tactsuitVr.PlaybackHaptics("HitInTheFace");
            }
        }

    }
}
