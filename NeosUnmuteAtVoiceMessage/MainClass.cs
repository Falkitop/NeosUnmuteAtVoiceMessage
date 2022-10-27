using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Reflection;
using System.Net;
using NeosModLoader;
using CloudX;
using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;

namespace NeosUnmuteAtVoiceMessage
{
    public class MainClass : NeosMod
    {
        public override string Name => "NeosUnmuteAtVoiceMessage";
        public override string Author => "NVPN";
        public override string Version => "1.0.0";
        public override string Link => "";

        [AutoRegisterConfigKey]
        public static readonly ModConfigurationKey<BaseX.color> KEY_COLOR = new ModConfigurationKey<BaseX.color>("Color", "TextColor");
        public static ModConfiguration config;
        public override void OnEngineInit()
        {
            config = GetConfiguration();
            config.Set(KEY_COLOR, BaseX.color.Red);
            Harmony harmony = new Harmony("com.nordvpn.patch");
            harmony.PatchAll();

            
        }
        public static T CastTo<T>(object i) { return (T)i; }
    }

    [Harmony]
    public class VoiceModeSwitchPatch
    {
        private static bool WasMuted;

        [HarmonyPatch(typeof(FriendsDialog), "OnAttach")]
        [HarmonyPostfix]
        static void ButtonFix(ref FriendsDialog __instance)
        {
            MainClass.CastTo<SyncRef<Button>>(__instance.GetSyncMember(12)).Target.LocalPressed += (idk, et) => ButtonPress();
            MainClass.CastTo<SyncRef<Button>>(__instance.GetSyncMember(12)).Target.LocalReleased += (idk, et) => ButtonRelease();
        }
        
        public static void ButtonPress()
        {
            WasMuted = FrooxEngine.Engine.Current.InputInterface.IsMuted;
            FrooxEngine.Engine.Current.InputInterface.IsMuted = false;
        }

        public static void ButtonRelease()
        {
            if (WasMuted)
                FrooxEngine.Engine.Current.InputInterface.IsMuted = true;

        }
    }

}
