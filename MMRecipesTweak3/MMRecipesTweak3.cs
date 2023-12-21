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
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System;
using System.CodeDom.Compiler;
using Jotunn;

namespace MMRecipesTweak3
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    //[NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
    internal class MMRecipesTweak3 : BaseUnityPlugin
    {
        public const string PluginGUID = "MofoMojo.MMRecipesTweak3";
        public const string PluginName = "MMRecipesTweak3";
        public const string PluginVersion = "4.0.0";
        public static MMRecipesTweak3 Instance;

        Harmony _Harmony;

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html

        // https://valheim-modding.github.io/Jotunn/data/objects/recipe-list.html
        // https://valheim-modding.github.io/Jotunn/data/objects/item-list.html

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

            AddAndUpdateRecipes();

            if (Settings.AllowPortalOverrides.Value) PrefabManager.OnVanillaPrefabsAvailable += InitPortalOverrides;
            if (Settings.MasonryChangesEnabled.Value) PrefabManager.OnVanillaPrefabsAvailable += DoStoneWork;

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

        private void AddAndUpdateRecipes()
        {
            if (Settings.FishingRodRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerFishingRod());
            if (Settings.FishingBaitRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerFishingBait());
            if (Settings.ChainsRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerChainsRecipe());
            if (Settings.LeatherRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerLeatherRecipe());
            if (Settings.LeatherScrapsRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerLeatherScrapsRecipe());
            if (Settings.FineWoodRecipeEnabled.Value)
            {
                ItemManager.Instance.AddRecipe(registerFineWoodRecipe());
                ItemManager.Instance.AddRecipe(registerFineWoodx3Recipe());
                ItemManager.Instance.AddRecipe(registerWoodFromFineWoodRecipe());
                ItemManager.Instance.AddRecipe(registerWoodx3FromFineWoodRecipe());
                ItemManager.Instance.AddRecipe(registerWoodFromCoreRecipe());
                ItemManager.Instance.AddRecipe(registerWoodx3FromCoreRecipe());
            }
            if (Settings.BlackCoreRecipeEnabled.Value) ItemManager.Instance.AddRecipe(registerBlackCoreRecipe());

            ItemManager.Instance.AddRecipe(registerMedStaminaPotionRecipe());
            ItemManager.Instance.AddRecipe(registerMedHealthPotionRecipe());
            ItemManager.Instance.AddRecipe(registerMajorHealthPotionRecipe());
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

        private CustomRecipe registerFineWoodRecipe()
        {
            CustomRecipe fineWood = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMFineWood",

                // Name of the prefab for the crafted item
                Item = "FineWood",

                Amount = 1,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                MinStationLevel = 3,

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "Wood",

                        // Amount required
                        Amount = 3
                    }
                }
            });

            return fineWood;



        }
        private CustomRecipe registerFineWoodx3Recipe()
        {
            CustomRecipe fineWood = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMFineWoodx3",

                // Name of the prefab for the crafted item
                Item = "FineWood",

                Amount = 3,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                MinStationLevel = 3,

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "Wood",

                        // Amount required
                        Amount = 9
                    }
                }
            });

            return fineWood;



        }

        private CustomRecipe registerWoodFromFineWoodRecipe()
        {
            CustomRecipe wood = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMWood",

                // Name of the prefab for the crafted item
                Item = "Wood",

                Amount = 3,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                MinStationLevel = 1,

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "FineWood",

                        // Amount required
                        Amount = 1
                    }
                }
            });

            return wood;



        }

        private CustomRecipe registerWoodx3FromFineWoodRecipe()
        {
            CustomRecipe wood = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMWoodx3",

                // Name of the prefab for the crafted item
                Item = "Wood",

                Amount = 9,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                MinStationLevel = 1,

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "FineWood",

                        // Amount required
                        Amount = 3
                    }
                }
            });

            return wood;



        }
 
        private CustomRecipe registerWoodFromCoreRecipe()
        {
            CustomRecipe wood = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMWoodFromCore",

                // Name of the prefab for the crafted item
                Item = "Wood",

                Amount = 5,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                MinStationLevel = 1,

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "RoundLog",

                        // Amount required
                        Amount = 1
                    }
                }
            });

            return wood;



        }

        private CustomRecipe registerWoodx3FromCoreRecipe()
        {
            CustomRecipe wood = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMWoodx3FromCore",

                // Name of the prefab for the crafted item
                Item = "Wood",

                Amount = 15,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_workbench",

                MinStationLevel = 1,

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                            {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "RoundLog",

                        // Amount required
                        Amount = 3
                    }
                }
            });

            return wood;



        }

        private CustomRecipe registerBlackCoreRecipe()
        {
            CustomRecipe blackCore = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMEitr",

                // Name of the prefab for the crafted item
                Item = "Eitr",

                Amount = 5,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_magetable",

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "BlackCore",
                        // Amount required
                        Amount = 1
                    }                    
    }
            });
            return blackCore;
        }

        private CustomRecipe registerMedStaminaPotionRecipe()
        {
            CustomRecipe recipe = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMMedStamPotion",

                // Name of the prefab for the crafted item
                Item = "MeadStaminaMedium",

                Amount = 1,

                MinStationLevel = 2,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_cauldron",

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "MeadStaminaMinor",
                        // Amount required
                        Amount = 3
                    }
    }
            });
            return recipe;
        }

        private CustomRecipe registerMedHealthPotionRecipe()
        {
            CustomRecipe recipe = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMMedHealthPotion",

                // Name of the prefab for the crafted item
                Item = "MeadHealthMedium",

                Amount = 1,

                MinStationLevel = 2,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_cauldron",

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "MeadHealthMinor",
                        // Amount required
                        Amount = 3
                    }
    }
            });
            return recipe;
        }

        private CustomRecipe registerMajorHealthPotionRecipe()
        {
            CustomRecipe recipe = new CustomRecipe(new RecipeConfig()
            {
                // Name of the recipe (defaults to "Recipe_YourItem")
                Name = "Recipe_MMMajorHealthPotion",

                // Name of the prefab for the crafted item
                Item = "MeadHealthMajor",

                Amount = 1,

                // Name of the prefab for the crafting station we wish to use
                // Can set this to null or leave out if you want your recipe to be craftable in your inventory
                CraftingStation = "piece_cauldron",

                MinStationLevel = 3,

                // List of requirements to craft your item
                Requirements = new RequirementConfig[]
                {
                    new RequirementConfig()
                    {
                        // Prefab name of requirement
                        Item = "MeadHealthMedium",
                        // Amount required
                        Amount = 3
                    }
    }
            });
            return recipe;
        }


        public static void DoStoneWork()
        {
            PrefabManager prefabManager = PrefabManager.Instance;

            foreach (string key in Settings.stoneWork.Keys)
            {
                LogVerbose($"Attempting to fixup {key}");
                //CustomPiece piece = pieceManager.GetPiece(key);
                Piece piece = prefabManager.GetPrefab(key).GetComponent<Piece>();

                if (null != piece)
                {
                    piece.m_resources[0].m_amount = Settings.stoneWork[key];
                    LogVerbose($"Found {key}, new amount {piece.m_resources[0].m_amount}");
                }
            }

        }

        private void InitPortalOverrides()
        {
            if (Settings.AllowIronOreTeleportationEnabled.Value)
            {
                MMRecipesTweak3.LogVerbose("Setting IronOre Portal(able) ");
                ItemDrop item = PrefabManager.Cache.GetPrefab<ItemDrop>("IronOre");
                item.m_itemData.m_shared.m_teleportable = true;
            }

            if (Settings.AllowIronScrapTeleportationEnabled.Value)
            {
                MMRecipesTweak3.LogVerbose("Setting IronScrap Portal(able) ");
                ItemDrop item = PrefabManager.Cache.GetPrefab<ItemDrop>("IronScrap");
                item.m_itemData.m_shared.m_teleportable = true;
            }

            if (Settings.AllowTinOreTeleportationEnabled.Value)
            {
                MMRecipesTweak3.LogVerbose("Setting TinOre Portal(able) ");
                ItemDrop item = PrefabManager.Cache.GetPrefab<ItemDrop>("TinOre");
                item.m_itemData.m_shared.m_teleportable = true;
            }
            if (Settings.AllowBlackMetalScrapTeleportationEnabled.Value)
            {
                MMRecipesTweak3.LogVerbose("Setting BlackMetalScrap Portal(able) ");
                ItemDrop item = PrefabManager.Cache.GetPrefab<ItemDrop>("BlackMetalScrap");
                item.m_itemData.m_shared.m_teleportable = true;
            }
            if (Settings.AllowSilverOreTeleportationEnabled.Value)
            {
                MMRecipesTweak3.LogVerbose("Setting SilverOre Portal(able) ");
                ItemDrop item = PrefabManager.Cache.GetPrefab<ItemDrop>("SilverOre");
                item.m_itemData.m_shared.m_teleportable = true;
            }
            if (Settings.AllowFlametalOreTeleportationEnabled.Value)
            {
                MMRecipesTweak3.LogVerbose("Setting FlametalOre Portal(able) ");
                ItemDrop item = PrefabManager.Cache.GetPrefab<ItemDrop>("FlametalOre");
                item.m_itemData.m_shared.m_teleportable = true;
            }
            if (Settings.AllowCopperOreTeleportationEnabled.Value)
            {
                MMRecipesTweak3.LogVerbose("Setting CopperOre Portal(able) ");
                ItemDrop item = PrefabManager.Cache.GetPrefab<ItemDrop>("CopperOre");
                item.m_itemData.m_shared.m_teleportable = true;
            }

        }

        #region BronzeTweak
        // Modify the Awake method of ObjectDB
        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        private static class MMRecipeTweaks
        {
            // check to see if it's enabled and if not, it won't patch for this mod
            [HarmonyPrepare]
            static bool IsRecipeTweaksEnabled()
            {
                bool enabled = Settings.RecipeTweaksEnabled.Value;
                MMRecipesTweak3.Log($"RecipeTweaksEnabled: {enabled}");

                return enabled;
            }

            // https://valheim-modding.github.io/Jotunn/data/objects/recipe-list.html
            // https://valheim-modding.github.io/Jotunn/data/objects/item-list.html
            // postfix attach to Awake method of ObjectDB
            [HarmonyPostfix]
            public static void ObjectDB_MMRecipeTweaks(ref ObjectDB __instance)
            {

                // https://valheim-modding.github.io/Jotunn/data/objects/recipe-list.html
                foreach (Recipe recipe in __instance.m_recipes)
                {
                    //Plugin.Log($"Looking at {recipe.name}");
                    switch (recipe.name.ToLower())
                    {
                        case "recipe_bronze":
                            if (Settings.BronzeTweakEnabled.Value) recipe.m_amount = 3;
                            MMRecipesTweak3.Log($"recipe_bronze: {3}");
                            break;
                        case "recipe_bronze5":
                            if (Settings.BronzeTweakEnabled.Value) recipe.m_amount = 15;
                            MMRecipesTweak3.Log($"recipe_bronze5: {15}");
                            break;
                        case "recipe_carrotsoup":
                            recipe.m_amount = Settings.CarrotSoupAmount.Value;
                            MMRecipesTweak3.Log($"recipe_carrotsoup: {Settings.CarrotSoupAmount.Value}");
                            break;
                        case "recipe_serpentstew":
                            recipe.m_amount = Settings.SerpentStewAmount.Value;
                            MMRecipesTweak3.Log($"recipe_serpentstew: {Settings.SerpentStewAmount.Value}");
                            break;
                        case "recipe_deerstew":
                            recipe.m_amount = Settings.DeerStewAmount.Value;
                            MMRecipesTweak3.Log($"recipe_deerstew: {Settings.DeerStewAmount.Value}");
                            break;
                        case "recipe_bloodpudding":
                            recipe.m_amount = Settings.BloodPuddingAmount.Value;
                            MMRecipesTweak3.Log($"recipe_bloodpudding: {Settings.BloodPuddingAmount.Value}");
                            break;
                        case "recipe_fishwraps":
                            recipe.m_amount = Settings.FishWrapsAmount.Value;
                            MMRecipesTweak3.Log($"recipe_fishwraps: {Settings.FishWrapsAmount.Value}");
                            break;
                        case "recipe_mincemeatsauce":
                            recipe.m_amount = Settings.MinceMeatSauceAmount.Value;
                            MMRecipesTweak3.Log($"recipe_mincemeatsauce: {Settings.MinceMeatSauceAmount.Value}");
                            break;
                        case "recipe_meadbasefrostresist":
                            recipe.m_amount = Settings.MeadBaseFrostResistAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasefrostresist: {Settings.MeadBaseFrostResistAmount.Value}");
                            break;
                        case "recipe_meadbasehealthmedium":
                            recipe.m_amount = Settings.MeadBaseHealthMediumAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasehealthmedium: {Settings.MeadBaseHealthMediumAmount.Value}");
                            break;
                        case "recipe_meadbasehealthminor":
                            recipe.m_amount = Settings.MeadBaseHealthMinorAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasehealthminor: {Settings.MeadBaseHealthMinorAmount.Value}");
                            break;
                        case "recipe_meadbasehealthmajor":
                            recipe.m_amount = Settings.MeadBaseHealthMajorAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasehealthmajor: {Settings.MeadBaseHealthMajorAmount.Value}");
                            break;
                        case "recipe_meadbasepoisonresist":
                            recipe.m_amount = Settings.MeadBasePoisonResistAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasepoisonresist: {Settings.MeadBasePoisonResistAmount.Value}");
                            break;
                        case "recipe_meadbasestaminamedium":
                            recipe.m_amount = Settings.MeadBaseStaminaMediumAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasestaminamedium: {Settings.MeadBaseStaminaMediumAmount.Value}");
                            break;
                        case "recipe_meadbasestaminaminor":
                            recipe.m_amount = Settings.MeadBaseStaminaMinorAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasestaminaminor: {Settings.MeadBaseStaminaMinorAmount.Value}");
                            break;
                        case "recipe_meadbasetasty":
                            recipe.m_amount = Settings.MeadBaseTastyAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasetasty: {Settings.MeadBaseTastyAmount.Value}");
                            break;
                        case "recipe_turnipstew":
                            recipe.m_amount = Settings.TurnipStewAmount.Value;
                            MMRecipesTweak3.Log($"recipe_turnipstew: {Settings.TurnipStewAmount.Value}");
                            break;
                        case "recipe_shocklatesmoothie":
                            recipe.m_amount = Settings.MuckShakeAmount.Value;
                            MMRecipesTweak3.Log($"recipe_shocklatesmoothie: {Settings.MuckShakeAmount.Value}");
                            break;
                        case "recipe_onionsoup":
                            recipe.m_amount = Settings.OnionSoupAmount.Value;
                            MMRecipesTweak3.Log($"recipe_onionsoup: {Settings.OnionSoupAmount.Value}");
                            break;
                        case "recipe_meatplatter":
                            recipe.m_amount = Settings.MeatPlatterAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meatplatter: {Settings.MeatPlatterAmount.Value}");
                            break;
                        case "recipe_fishandbread":
                            recipe.m_amount = Settings.FishAndBreadAmount.Value;
                            MMRecipesTweak3.Log($"recipe_fishandbread: {Settings.FishAndBreadAmount.Value}");
                            break;
                        case "recipe_blacksoup":
                            recipe.m_amount = Settings.BlackSoupAmount.Value;
                            MMRecipesTweak3.Log($"recipe_blacksoup: {Settings.BlackSoupAmount.Value}");
                            break;
                        case "recipe_bread":
                            recipe.m_amount = Settings.BreadDoughAmount.Value;
                            MMRecipesTweak3.Log($"recipe_bread: {Settings.BreadDoughAmount.Value}");
                            break;
                        case "recipe_loxpie":
                            recipe.m_amount = Settings.LoxPieAmount.Value;
                            MMRecipesTweak3.Log($"recipe_loxpie: {Settings.LoxPieAmount.Value}");
                            break;
                        case "recipe_eyescream":
                            recipe.m_amount = Settings.EyescreamAmount.Value;
                            MMRecipesTweak3.Log($"recipe_eyescream: {Settings.EyescreamAmount.Value}");
                            break;
                        case "recipe_mistharesupreme":
                            recipe.m_amount = Settings.MistHareSupremeAmount.Value;
                            MMRecipesTweak3.Log($"recipe_mistharesupreme: {Settings.MistHareSupremeAmount.Value}");
                            break;
                        case "recipe_mushroomomelette":
                            recipe.m_amount = Settings.MushroomOmeletteAmount.Value;
                            MMRecipesTweak3.Log($"recipe_mushroomomelette: {Settings.MushroomOmeletteAmount.Value}");
                            break;
                        case "recipe_salad":
                            recipe.m_amount = Settings.SaladAmount.Value;
                            MMRecipesTweak3.Log($"recipe_salad: {Settings.SaladAmount.Value}");
                            break;
                        case "recipe_seekeraspic":
                            recipe.m_amount = Settings.SeekerAspicAmount.Value;
                            MMRecipesTweak3.Log($"recipe_seekeraspic: {Settings.SeekerAspicAmount.Value}");
                            break;
                        case "recipe_wolfjerky":
                            recipe.m_amount = Settings.WolfJerkyAmount.Value;
                            MMRecipesTweak3.Log($"recipe_wolfjerky: {Settings.WolfJerkyAmount.Value}");
                            break;
                        case "recipe_wolfskewer":
                            recipe.m_amount = Settings.WolfSkewerAmount.Value;
                            MMRecipesTweak3.Log($"recipe_wolfskewer: {Settings.WolfSkewerAmount.Value}");
                            break;
                        case "recipe_yggdrasilporridge":
                            recipe.m_amount = Settings.YggdrasilPorridgeAmount.Value;
                            MMRecipesTweak3.Log($"recipe_yggdrasilporridge: {Settings.YggdrasilPorridgeAmount.Value}");
                            break;
                        case "recipe_honeyglazedchicken":
                            recipe.m_amount = Settings.HoneyGlazedChickenAmount.Value;
                            MMRecipesTweak3.Log($"recipe_honeyglazedchicken: {Settings.HoneyGlazedChickenAmount.Value}");
                            break;
                        case "recipe_magicallystuffedshroom":
                            recipe.m_amount = Settings.MagicallyStuffedShroomAmount.Value;
                            MMRecipesTweak3.Log($"recipe_magicallystuffedshroom: {Settings.MagicallyStuffedShroomAmount.Value}");
                            break;
                        case "recipe_meadbaseeitrminor":
                            recipe.m_amount = Settings.MeadBaseEitrMinorAmount.Value;
                            MMRecipesTweak3.Log($"Recipe_MeadBaseEitrMinor: {Settings.MeadBaseEitrMinorAmount.Value}");
                            break;
                        case "recipe_barleywinebase":
                            recipe.m_amount = Settings.BarleyWineBaseAmount.Value;
                            MMRecipesTweak3.Log($"recipe_barleywinebase: {Settings.BarleyWineBaseAmount.Value}");
                            break;
                        case "recipe_meadbasestaminalingering":
                            recipe.m_amount = Settings.MeadBaseStaminaLingeringAmount.Value;
                            MMRecipesTweak3.Log($"recipe_meadbasestaminalingering: {Settings.MeadBaseStaminaLingeringAmount.Value}");
                            break;

                    }

                }
            }
        }
        #endregion
        [HarmonyPatch(typeof(Fish), "Pickup")]
        public class HarmonyPatch_OceanFishing
        {

            // Only enable if EnableFishingInOceanMultiplierEnabled is set
            [HarmonyPrepare]
            static bool IsEnableFishingInOceanMultiplierEnabled()
            {
                bool enabled = Settings.FishingInOceanMultiplierEnabled.Value;
                MMRecipesTweak3.Log($"EnableFishingInOceanMultiplier {enabled}");

                return enabled;

            }

            [HarmonyPrefix]
            private static void PickupPrefix(Fish __instance)
            {

                var new_m_pickupItemStackSize = __instance.m_pickupItemStackSize;

                MMRecipesTweak3.Log($"Player Fishing PickupPrefix {new_m_pickupItemStackSize}");

                bool inOcean = false;

                List<Player> players = new List<Player>();

                Player.GetPlayersInRange(__instance.gameObject.transform.position, 50, players);

                /*
                foreach (Player player in players)
                {
                    // find the first player in the ocean near the fish
                    if (player.GetCurrentBiome() == Heightmap.Biome.Ocean)
                    {
                        inOcean = true;
                        break;
                    }
                }
                */

                // get the local player catching the fish
                if (Player.m_localPlayer.GetCurrentBiome() == Heightmap.Biome.Ocean)
                {
                    inOcean = true;
                }

                // if the player is in the ocean when fishing, increase fish caught count
                if (inOcean)
                {
                    new_m_pickupItemStackSize = UnityEngine.Random.Range(1, Settings.FeatherMultiplier.Value) * __instance.m_pickupItemStackSize;
                    __instance.m_pickupItemStackSize = new_m_pickupItemStackSize;
                    MMRecipesTweak3.Log($"Player Fishing PickupPrefix modified: {new_m_pickupItemStackSize}");
                }
                else
                {
                    MMRecipesTweak3.Log($"Player not found in the ocean");
                }



            }
        }

        #region EnableFeatherMultiplier
        /*
         * There are two GEtDropList methods in DropTable. One takes a parameter and one does not, patching normally causes an ambiguousmatchexception
         * https://outward.fandom.com/wiki/Advanced_Modding_Guide/Hooks had a good guide on it
         * ambiguousmatchexception
         * public List<GameObject> GetDropList()
         * private List<GameObject> GetDropList(int amount)
         * 
         * Interestingly... this is a client side patch even on a dedicated server
        */

        [HarmonyPatch(typeof(DropTable), "GetDropList", new Type[] { typeof(int) })]
        public class HarmonyPatch_FeatherMultiplier
        {
            // Only enable if EnableFeatherMultiplier is set
            [HarmonyPrepare]
            static bool IsEnableFeatherMultiplierEnabled()
            {
                bool enabled = Settings.FeatherMultiplierEnabled.Value;
                MMRecipesTweak3.Log($"EnableFeatherMultiplier {enabled}");

                return enabled;
            }

            // This is the method that will be called AFTER GetDropList is called so that we can modify the drop list
            /*
             * Crow and Seagal [sp] utilizes DropOnDestroyed
             * DropOnDestroyed has an OnDestroyed method
             * OnDestroyed calls m_dropWhenDestroyed's GetDropList() method which returns a List of GameObjects
             * we loop through the dropList and create/instantiate objects
             * Other creature types don't do this, they utilize CharacterDrop's GenerateDropList
             * GetDropList() calls GetDropList(amount) and returns it
             */
            [HarmonyPostfix]
            private static void MM_AdjustDropList(ref List<GameObject> __result, DropTable __instance)
            {
                //Clone the old list
                List<GameObject> list = new List<GameObject>(__result);

                // now loop through the old list 
                foreach (GameObject obj in __result)
                {
                    MMRecipesTweak3.Log($"MM_AdjustDropList: GameObject: {obj.name}");

                    //add 1-3 additional feathers) to the NEW list
                    if (obj.name.ToLower() == "feathers")
                    {
                        // Return a random int within [minInclusive..maxExclusive) (Read Only).
                        // Notes on UnityEngine.Random.Range(min int, max int) 
                        // Float numbers work differently where min/max are both inclusive
                        // these are ints below
                        int amount = UnityEngine.Random.Range(1, Settings.FeatherMultiplier.Value);
                        for (int i = 0; i < amount; i++)
                        {
                            list.Add(obj);
                        }

                        // added the feathers, now don't add any more...
                        // break here so that if there are additional feather entries in the result list we don't find and continue to add more
                        break;
                    }
                }

                // replace the initial List<GameObject> result with the new list
                __result = list;
            }

            /*
             * this is not used....
             * but preserving in case I want to make adjustments elsewhere
             * not applicable to birds as I had hoped but this hooks into the drop table successfully
             * Also not application to FISH... lol. 
             * 
            // This hooks into GenerateDropList on CharacterDrop
            [HarmonyPatch(typeof(CharacterDrop), "GenerateDropList")]
            public class HarmonyPatch_RememberServer
            {
                // Only enable if EnableFeatherMultiplier is set
                [HarmonyPrepare]
                static bool IsEnableFeatherMultiplierEnabled()
                {
                    bool enabled = Settings.EnableFeatherMultiplier.Value;
                    Plugin.Log($"EnableFeatherMultiplier {enabled}");
                    return enabled;
                }
                // This is the method that will be called AFTER GenerateDropList is called so that we can modify the drop list
                [HarmonyPostfix]
                private static void MM_AdjustLoot( ref List<KeyValuePair<GameObject, int>> __result, CharacterDrop __instance)
                {
                    Plugin.Log($"Hooked post GenerateDropList");
                    //Clone the old list
                    List<KeyValuePair<GameObject, int>> list = new List<KeyValuePair<GameObject, int>>(__result);
                    foreach (KeyValuePair<GameObject, int> obj in __result)
                    {
                        Plugin.Log($"KeyValuePair: {obj.Key.name}, Amount {obj.Value}");
                        //add 3-4 additional feathers)
                        if(obj.Key.name.ToLower() == "feathers")
                        {
                            int amount = UnityEngine.Random.Range(1, 4);
                            list.Add(new KeyValuePair<GameObject, int>(obj.Key, amount));
                        }
                    }
                    // return the new list
                    __result = list;
                }
            }
            */


        }

        #endregion

    }


    internal static class Settings
    {
        // https://valheim-modding.github.io/Jotunn/data/objects/recipe-list.html
        public static ConfigEntry<bool> FishingRodRecipeEnabled;

        public static ConfigEntry<bool> FeatherMultiplierEnabled;
        public static ConfigEntry<int> FeatherMultiplier;
        public static ConfigEntry<bool> FishingInOceanMultiplierEnabled;
        public static ConfigEntry<int> FishingInOceanMultiplier;

        public static ConfigEntry<bool> LoxMeatSurpriseRecipeEnabled;
        public static ConfigEntry<bool> FishingBaitRecipeEnabled;
        public static ConfigEntry<bool> ChainsRecipeEnabled;
        public static ConfigEntry<bool> RecipeTweaksEnabled;
        public static ConfigEntry<bool> LeatherScrapsRecipeEnabled;
        public static ConfigEntry<bool> LeatherRecipeEnabled;
        public static ConfigEntry<bool> FineWoodRecipeEnabled;
        public static ConfigEntry<bool> BronzeTweakEnabled;
        public static ConfigEntry<bool> BlackCoreRecipeEnabled;
        public static ConfigEntry<bool> AllowPotionCombining;

        public static ConfigEntry<bool> AllowPortalOverrides;
        public static ConfigEntry<bool> AllowCopperOreTeleportationEnabled;
        public static ConfigEntry<bool> AllowIronOreTeleportationEnabled;
        public static ConfigEntry<bool> AllowIronScrapTeleportationEnabled;
        public static ConfigEntry<bool> AllowSilverOreTeleportationEnabled;
        public static ConfigEntry<bool> AllowTinOreTeleportationEnabled;
        public static ConfigEntry<bool> AllowBlackMetalScrapTeleportationEnabled;
        public static ConfigEntry<bool> AllowFlametalOreTeleportationEnabled;

        public static ConfigEntry<int> CarrotSoupAmount;
        public static ConfigEntry<int> SerpentStewAmount;
        public static ConfigEntry<int> DeerStewAmount;
        public static ConfigEntry<int> BloodPuddingAmount;
        public static ConfigEntry<int> FishWrapsAmount;
        public static ConfigEntry<int> MinceMeatSauceAmount;
        public static ConfigEntry<int> TurnipStewAmount;
        public static ConfigEntry<int> MuckShakeAmount;
        public static ConfigEntry<int> OnionSoupAmount;
         public static ConfigEntry<int> MeatPlatterAmount;
        public static ConfigEntry<int> FishAndBreadAmount;
        public static ConfigEntry<int> BlackSoupAmount;
        public static ConfigEntry<int> BreadDoughAmount;
        public static ConfigEntry<int> LoxPieAmount;
        public static ConfigEntry<int> EyescreamAmount;
        public static ConfigEntry<int> MistHareSupremeAmount;
        public static ConfigEntry<int> MushroomOmeletteAmount;
        public static ConfigEntry<int> SaladAmount;
        public static ConfigEntry<int> SeekerAspicAmount;
        public static ConfigEntry<int> WolfJerkyAmount;
        public static ConfigEntry<int> WolfSkewerAmount;
        public static ConfigEntry<int> YggdrasilPorridgeAmount;
        public static ConfigEntry<int> HoneyGlazedChickenAmount;
        public static ConfigEntry<int> MagicallyStuffedShroomAmount;

        public static ConfigEntry<int> BarleyWineBaseAmount;
        public static ConfigEntry<int> MeadBaseHealthMajorAmount;
        public static ConfigEntry<int> MeadBaseFrostResistAmount;
        public static ConfigEntry<int> MeadBaseHealthMediumAmount;
        public static ConfigEntry<int> MeadBaseHealthMinorAmount;
        public static ConfigEntry<int> MeadBasePoisonResistAmount;
        public static ConfigEntry<int> MeadBaseStaminaMediumAmount;
        public static ConfigEntry<int> MeadBaseStaminaMinorAmount;
        public static ConfigEntry<int> MeadBaseTastyAmount;
        public static ConfigEntry<int> MeadBaseEitrMinorAmount;
        public static ConfigEntry<int> MeadBaseStaminaLingeringAmount;

        public static ConfigEntry<bool> MasonryChangesEnabled;
        public static ConfigEntry<int> stone_wall_4x2;
        public static ConfigEntry<int> stone_wall_2x1;
        public static ConfigEntry<int> stone_wall_1x1;
        public static ConfigEntry<int> stone_floor_2x2;
        public static ConfigEntry<int> stone_arch;
        public static ConfigEntry<int> stone_pillar;
        public static ConfigEntry<int> stone_stair;

        public static System.Collections.Generic.Dictionary<string, int> stoneWork = new System.Collections.Generic.Dictionary<string, int>();

        public static ConfigEntry<MMRecipesTweak3.LoggingLevel> PluginLoggingLevel;


        // These are the settings that will be saved in the ..\plugins\mofomojo.cfg file
        public static void Init()
        {
            PluginLoggingLevel = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<MMRecipesTweak3.LoggingLevel>("LoggingLevel", "PluginLoggingLevel", MMRecipesTweak3.LoggingLevel.None, "Supported values are None, Normal, Verbose");
            FishingRodRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "FishingRodRecipeEnabled", true, "Enables  a recipe for Fishing Rods");
            LeatherScrapsRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "LeatherScrapsRecipeEnabled", true, "Enables  a recipe for converting Leather to LeatherScraps");
            LeatherRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "LeatherRecipeEnabled", true, "Enables  a recipe for converting LeatherScraps to Leather");
            FineWoodRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "FineWoodRecipeEnabled", true, "Enables  crafting FineWood from 5 Wood with a min Workbench level of 3");
            BlackCoreRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "BlackCoreRecipeEnabled", true, "Enables  crafting BlackCores");
            AllowPotionCombining = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "AllowPotionCombining", true, "Allows for minor and medium potions to be combined for medium and major potions (if at proper level)");

            FeatherMultiplierEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Tweaks", "FeatherMultiplierEnabled", true, "Birds will drop additional feathers");
            FeatherMultiplier = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Tweaks", "FeatherMultiplier", 3, "Max feather multiplier for feathers (mutliplier will be a random range between 1 and this number)");

            if (FeatherMultiplier.Value < 1) FeatherMultiplier.Value = 1;

            FishingInOceanMultiplierEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Tweaks", "FishingInOceanMultiplierEnabled", true, "When fishing in the ocean, you get a multiplier on fish caught");
            FishingInOceanMultiplier = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Tweaks", "FishingInOceanMultiplier", 4, "Mutliplier will be a random range between 1 and this number");
            
            if (FishingInOceanMultiplier.Value < 1) FishingInOceanMultiplier.Value = 1;

            FishingBaitRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "FishingBaitRecipeEnabled", true, "Enables  a recipe for bait made from Necktails");
            LoxMeatSurpriseRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "LoxMeatSurpriseRecipeEnabled", true, "Enables a recipe and item for Lox Meat Surprise");
            ChainsRecipeEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Recipes", "ChainsRecipeEnabled", true, "Enables a recipe for making chains (4 Iron = 1 chain)");
            RecipeTweaksEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("RecipeTweaks", "RecipeTweaksEnabled", true, "Enabled Various Recipe Tweaks below");
            BronzeTweakEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("RecipeTweaks", "BronzeTweakEnabled", true, "Changes Bronze Recipe from 2 copper+1 tin = 1 bronze to 2+1=3 (and the x5 recipe too)");
            CarrotSoupAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "CarrotSoupAmount", 3, "The amount of carrot soup a single recipe makes");
            SerpentStewAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "SerpentStewAmount", 2, "The amount of Serpent Stew a single recipe makes");
            DeerStewAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "DeerStewAmount", 2, "The amount of Deer Stew a single recipe makes");
            BloodPuddingAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "BloodPuddingAmount", 2, "The amount of Blood Pudding a single recipe makes");
            FishWrapsAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "FishWrapsAmount", 2, "The amount of Fish Wraps a single recipe makes");
            MinceMeatSauceAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MinceMeatSauceAmount", 2, "The amount of mince meat sauce a single recipe makes");
            TurnipStewAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "TurnipStewAmount", 3, "The amount of Turnip Stew a single recipe makes");
            MuckShakeAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MuckShakeAmount", 2, "The amount of Muckshake a single recipe makes");
            OnionSoupAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "OnionSoupAmount", 2, "The amount of onion soup a single recipe makes");
            MeatPlatterAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeatPlatterAmount", 3, "The amount of Meat Platter a single recipe makes");
            FishAndBreadAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "FishAndBreadAmount", 2, "The amount of Fish and Bread a single recipe makes");
            BlackSoupAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "BlackSoupAmount", 3, "The amount of Black Soup a single recipe makes");
            BreadDoughAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "BreadDoughAmount", 4, "The amount of Bread dough a single recipe makes");
            LoxPieAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "LoxPieAmount", 4, "The amount of Unbaked Lox Pie a single recipe makes");
            EyescreamAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "EyescreamAmount", 3, "The amount of Eyescream a single recipe makes");
            MistHareSupremeAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MistHareSupremeAmount", 4, "The amount a single recipe makes");
            MushroomOmeletteAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MushroomOmeletteAmount", 3, "The amount a single recipe makes");
            SaladAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "SaladAmount", 4, "The amount a single recipe makes");
            SeekerAspicAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "SeekerAspicAmount", 3, "The amount a single recipe makes");
            WolfJerkyAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "WolfJerkyAmount", 3, "The amount a single recipe makes");
            WolfSkewerAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "WolfSkewerAmount", 3, "The amount a single recipe makes");
            YggdrasilPorridgeAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "YggdrasilPorridgeAmount", 3, "The amount a single recipe makes");
            HoneyGlazedChickenAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "HoneyGlazedChickenAmount", 3, "The amount a single recipe makes");
            MagicallyStuffedShroomAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MagicallyStuffedShroomAmount", 3, "The amount a single recipe makes");

            BarleyWineBaseAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "BarleyWineBaseAmount", 2, "The amount of Barley Wine base a single recipe makes");
            MeadBaseFrostResistAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseFrostResistAmount", 2, "The amount of mead a single recipe makes");
            MeadBaseHealthMajorAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseHealthMajorAmount", 2, "The amount of mead a single recipe makes");
            MeadBaseHealthMediumAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseHealthMediumAmount", 2, "The amount of mead a single recipe makes");
            MeadBaseHealthMinorAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseHealthMinorAmount", 2, "The amount of mead a single recipe makes");
            MeadBasePoisonResistAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBasePoisonResistAmount", 2, "The amount of mead a single recipe makes");
            MeadBaseStaminaMediumAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseStaminaMediumAmount", 2, "The amount of mead a single recipe makes");
            MeadBaseStaminaMinorAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseStaminaMinorAmount", 2, "The amount of mead a single recipe makes");
            MeadBaseTastyAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseTastyAmount", 2, "The amount of mead a single recipe makes");
            MeadBaseEitrMinorAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseEitrMinorAmount", 2, "The amount of mead a single recipe makes");
            MeadBaseStaminaLingeringAmount = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("RecipeTweaks", "MeadBaseStaminaLingeringAmount", 2, "The amount of mead a single recipe makes");

            MasonryChangesEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("Masonry", "MasonryChangesEnabled", true, "Enables  a stone recipe changes");
            stone_wall_4x2 = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Masonry", "stone_wall_4x2", 4, "The amount of stone to make this item");
            stone_wall_2x1 = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Masonry", "stone_wall_2x1", 2, "The amount of stone to make this item");
            stone_wall_1x1 = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Masonry", "stone_wall_1x1", 1, "The amount of stone to make this item");
            stone_floor_2x2 = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Masonry", "stone_floor_2x2", 2, "The amount of stone to make this items");
            stone_arch = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Masonry", "stone_arch", 3, "The amount of stone to make this item");
            stone_pillar = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Masonry", "stone_pillar", 3, "The amount of stone to make this item");
            stone_stair = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<int>("Masonry", "stone_stair", 4, "The amount of stone to make this item");

            AllowPortalOverrides = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("PortalOverrides", "AllowPortalOverrides", true, "Enable this to support portal overrides below");
            AllowCopperOreTeleportationEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("PortalOverrides", "AllowCopperOreTeleportationEnabled", true, "Enable this metal to go through portals");
            AllowIronOreTeleportationEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("PortalOverrides", "AllowIronOreTeleportationEnabled", true, "Enable this metal to go through portals");
            AllowIronScrapTeleportationEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("PortalOverrides", "AllowIronScrapTeleportationEnabled", true, "Enable this metal to go through portals");
            AllowSilverOreTeleportationEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("PortalOverrides", "AllowSilverOreTeleportationEnabled", true, "Enable this metal to go through portals");
            AllowTinOreTeleportationEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("PortalOverrides", "AllowTinOreTeleportationEnabled", true, "Enable this metal to go through portals");
            AllowBlackMetalScrapTeleportationEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("PortalOverrides", "AllowBlackMetalScrapTeleportationEnabled", true, "Enable this metal to go through portals");
            AllowFlametalOreTeleportationEnabled = ((BaseUnityPlugin)MMRecipesTweak3.Instance).Config.Bind<bool>("PortalOverrides", "AllowFlametalOreTeleportationEnabled", true, "Enable this metal to go through portals");

            stoneWork.Add("stone_wall_4x2", stone_wall_4x2.Value);
            stoneWork.Add("stone_wall_2x1", stone_wall_2x1.Value);
            stoneWork.Add("stone_wall_1x1", stone_wall_1x1.Value);
            stoneWork.Add("stone_floor_2x2", stone_floor_2x2.Value);
            stoneWork.Add("stone_arch", stone_arch.Value);
            stoneWork.Add("stone_pillar", stone_pillar.Value);
            stoneWork.Add("stone_stair", stone_stair.Value);

        }
    }

}


