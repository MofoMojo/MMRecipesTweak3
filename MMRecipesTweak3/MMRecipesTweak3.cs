// MMRecipesTweak3
// a Valheim mod skeleton using Jötunn
// 
// File:    MMRecipesTweak3.cs
// Project: MMRecipesTweak3

using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.Managers;
using System.Reflection;


namespace MMRecipesTweak3
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class MMRecipesTweak3 : BaseUnityPlugin
    {
        public const string PluginGUID = "MofoMojo.MMRecipesTweak3";
        public const string PluginName = "MMRecipesTweak3";
        public const string PluginVersion = "3.0.0";
        public static MMRecipesTweak3 Instance;

        Harmony _Harmony;

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        public static CustomLocalization Localization = LocalizationManager.Instance.GetLocalization();

        public static LoggingLevel PluginLoggingLevel = LoggingLevel.None;
        public enum LoggingLevel
        {
            None,
            Normal,
            Verbose
        }

        private void Awake()
        {
            Instance = this;
            Settings.Init();
            PluginLoggingLevel = Settings.PluginLoggingLevel.Value;

            // Jotunn comes with its own Logger class to provide a consistent Log style for all mods using it
            Jotunn.Logger.LogInfo("MMRecipesTweak3 has landed");

            // To learn more about Jotunn's features, go to
            // https://valheim-modding.github.io/Jotunn/tutorials/overview.html

            AddRecipes();

            _Harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
        }

        public static void Log(string message)
        {
            message = $"{PluginName}: {message}";
            if (PluginLoggingLevel > LoggingLevel.None) Jotunn.Logger.LogInfo(message);
        }

        public static void LogWarning(string message)
        {
            message = $"{PluginName}: {message}";
            if (PluginLoggingLevel > LoggingLevel.None) Jotunn.Logger.LogWarning(message);
        }

        public static void LogError(string message)
        {
            message = $"{PluginName}: {message}";
            if (PluginLoggingLevel > LoggingLevel.None) Jotunn.Logger.LogError(message);
        }

        public static void LogVerbose(string message)
        {
            message = $"{PluginName}: {message}";
            if (PluginLoggingLevel == LoggingLevel.Verbose) Jotunn.Logger.LogError(message);
        }




        private void OnDestroy()
        {
            if (_Harmony != null) _Harmony.UnpatchSelf();
        }

        private void AddRecipes()
        {
            if (Settings.FishingRodRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerFishingRod());
            if (Settings.FishingBaitRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerFishingBait());
            if (Settings.ChainsRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerChainsRecipe());
            if (Settings.LeatherRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerLeatherRecipe());
            if (Settings.LeatherScrapsRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerLeatherScrapsRecipe());
        }

        #region FishingRodRecipe
        private CustomRecipe registerFishingRod()
        {

            // Create a custom recipe with a RecipeConfig
            CustomRecipe fishingRod = new CustomRecipe(new RecipeConfig()
            {
                Name = "Recipe_MMFishingRod",
                Item = "FishingRod",                    // Name of the item prefab to be crafted
                MinStationLevel = 2,
                Amount = 1,
                CraftingStation = "piece_workbench",
                Requirements = new RequirementConfig[]  // Resources and amount needed for it to be crafted
                {
                    new RequirementConfig { Item = "Wood", Amount = 2 },
                    new RequirementConfig { Item = "LinenThread", Amount = 2 }
                }
            });

            return fishingRod;
        }
        #endregion
        private CustomRecipe registerFishingBait()
        {
            CustomRecipe fishingBait = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMFishingBait",

                // Name of the prefab for the crafted item
                Item = "FishingBait",

                Amount = 5,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "NeckTail",

                        // Amount required
                        Amount = 1
                    }
                }
            });

            return fishingBait;
        }

        private CustomRecipe registerChainsRecipe()
        {
            CustomRecipe chains = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMChain",

                // Name of the prefab for the crafted item
                Item = "Chain",

                Amount = 1,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "forge",

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "Iron",

                        // Amount required
                        Amount = 4
                    }
                }
            });

            return chains;
        }

        private CustomRecipe registerLeatherRecipe()
        {
            CustomRecipe leather = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMDeerHide",

                // Name of the prefab for the crafted item
                Item = "DeerHide",

                Amount = 1,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "LeatherScraps",

                        // Amount required
                        Amount = 3
                    }
    }
            });
            return leather;
        }

        private CustomRecipe registerLeatherScrapsRecipe()
        {
            CustomRecipe leatherscraps = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_LeatherScraps",

                // Name of the prefab for the crafted item
                Item = "LeatherScraps",

                Amount = 3,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "DeerHide",

                        // Amount required
                        Amount = 1
                    }
                }
            });

            return leatherscraps;



        }

        #region BronzeTweak
        // Modify the Awake method of ObjectDB
        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        private static class MMEnableBronzeTweak
        {
            // check to see if it's enabled and if not, it won't patch for this mod
            [HarmonyPrepare]
            static bool IsBronzeTweakEnabled()
            {
                bool enabled = Settings.BronzeTweakEnabled.Value;
                MMRecipesTweak3.Log($"EnableBronzeTweak: {enabled}");

                return enabled;
            }

            // postfix attach to Awake method of ObjectDB
            [HarmonyPostfix]
            public static void ObjectDB_BronzeTweak(ref ObjectDB __instance)
            {
                //Plugin.Log($"Looking for Bronze Recipes");
                foreach (Recipe recipe in __instance.m_recipes)
                {
                    //Plugin.Log($"Looking at {recipe.name}");

                    // Plugin.Log($"Checking {recipe.name}");
                    if (recipe.name == "Recipe_Bronze")
                    {
                        // Plugin.Log($"Patching {recipe.name}");
                        recipe.m_amount = 3;
                    }
                    else if (recipe.name == "Recipe_Bronze5")
                    {
                        //Plugin.Log($"Patching {recipe.name}");
                        recipe.m_amount = 15;
                    }
                }
            }
        }
        #endregion



    }

    internal static class Settings
    {

        public static ConfigEntry<bool> FishingRodRecipeEnabled;
        public static ConfigEntry<bool> LoxMeatSurpriseRecipeEnabled;
        public static ConfigEntry<bool> FishingBaitRecipeEnabled;
        public static ConfigEntry<bool> ChainsRecipeEnabled;
        public static ConfigEntry<bool> BronzeTweakEnabled;
        public static ConfigEntry<bool> LeatherScrapsRecipeEnabled;
        public static ConfigEntry<bool> LeatherRecipeEnabled;
        public static ConfigEntry<MMRecipesTweak3.LoggingLevel> PluginLoggingLevel;
        // These are the settings that will be saved in the ..\plugins\mofomojo.cfg file
        public static void Init()
        {
            PluginLoggingLevel = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<MMRecipesTweak3.LoggingLevel>("LoggingLevel", "PluginLoggingLevel", MMRecipesTweak3.LoggingLevel.None, "Supported values are None, Normal, Verbose");
            FishingRodRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "FishingRodRecipeEnabled", true, "Enables  a recipe for Fishing Rods");
            LeatherScrapsRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "LeatherScrapsRecipeEnabled", true, "Enables  a recipe for converting Leather to LeatherScraps");
            LeatherRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "LeatherRecipeEnabled", true, "Enables  a recipe for converting LeatherScraps to Leather");

            FishingBaitRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "FishingBaitRecipeEnabled", true, "Enables  a recipe for bait made from Necktails");
            LoxMeatSurpriseRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "LoxMeatSurpriseRecipeEnabled", true, "Enables a recipe and item for Lox Meat Surprise");
            ChainsRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "ChainsRecipeEnabled", true, "Enables a recipe for making chains (4 Iron = 1 chain)");
            BronzeTweakEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "BronzeTweakEnabled", true, "Changes Bronze Recipe from 2 copper+1 tin = 1 bronze to 2+1=3 (and the x5 recipe too)");
        }

    }
}

