using System;
using System.Security.Permissions;
using System.Security;
using BepInEx;
using R2API.Utils;
using R2API;
using BepInEx.Configuration;
using Facepunch.Steamworks;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace MoreBulletsponge
{
    [BepInDependency(R2API.RecalculateStatsAPI.PluginGUID)]
    [BepInDependency(R2API.R2API.PluginGUID)]
    [NetworkCompatibility(CompatibilityLevel.NoNeedForSync, VersionStrictness.DifferentModVersionsAreOk)]
    [BepInPlugin("com.Moffein.MoreBulletsponge", "MoreBulletsponge", "1.0.0")]
    public class MoreBulletspongePlugin : BaseUnityPlugin
    {
        public static ConfigEntry<float> levelArmor;
        public static ConfigEntry<float> championBonusLevelArmor;

        private void Awake()
        {
            levelArmor = base.Config.Bind<float>("Stats", "Armor Per Level", 2f);
            championBonusLevelArmor = base.Config.Bind<float>("Stats", "Boss Armor Per Level", 3f);
            RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        public static void RecalculateStatsAPI_GetStatCoefficients(RoR2.CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if(sender.teamComponent && sender.teamComponent.teamIndex != RoR2.TeamIndex.Player)
            {
                if (sender.isChampion)
                {
                    args.levelArmorAdd += championBonusLevelArmor.Value;
                }
                else
                {
                    args.levelArmorAdd += levelArmor.Value;
                }
            }
        }
    }
}
