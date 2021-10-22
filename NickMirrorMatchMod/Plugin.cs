using System;
using BepInEx;
using BepInEx.Configuration;
using Nick;
using System.Collections.Generic;
using HarmonyLib;
using System.Reflection;
using System.IO;
using UnityEngine;
using BepInEx.Logging;
using MenuCore;

namespace NickMirrorMatchMod
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        internal static Plugin Instance;
        static ConfigEntry<Color> tintColor1;
        static ConfigEntry<Color> tintColor2;
        static ConfigEntry<Color> tintColor3;
        static int currentColorBeingEdited;
        static CustomOptionMenu customMenu;

        private void Awake()
        {
            Logger.LogDebug($"Plugin {PluginInfo.PLUGIN_NAME} is loaded!");

            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            var config = this.Config;
            tintColor1 = config.Bind<Color>("Colors", "TintColor1", new Color(0.51f, 0.27f, 1, 0.66f));
            tintColor2 = config.Bind<Color>("Colors", "TintColor2", new Color(1, 0.93f, 0.23f, 0.66f));
            tintColor3 = config.Bind<Color>("Colors", "TintColor3", new Color(0.16f, 0.95f, 0.82f, 0.66f));

            config.SettingChanged += OnConfigSettingChanged;

            // Harmony patches
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

            currentColorBeingEdited = 0;
            CustomOptionMenuHandler.initiateMenu(createMenu);
        }

        static void OnConfigSettingChanged(object sender, EventArgs args)
        {
            LogDebug($"{PluginInfo.PLUGIN_NAME} OnConfigSettingChanged");
            Plugin.Instance?.Config?.Reload();
        }

        private static void createMenu()
        {
            customMenu = new CustomOptionMenu();
            customMenu.menuName = $"{PluginInfo.PLUGIN_NAME} {PluginInfo.PLUGIN_VERSION}";

            List<string> selections = new List<string>();
            selections.Add("1");
            selections.Add("2");
            selections.Add("3");
            customMenu.createSelector("selectorCurrentColorBeingEdited", "Duplicate Player", selections, 0, SelectorType.Center, selectorCurrentColorBeingEditedChanged);

            addColorSlidersToMenu();

            CustomOptionMenuHandler.createMenuTab(customMenu);
        }

        private static void addColorSlidersToMenu()
        {
            Color color;
            switch (currentColorBeingEdited)
            {
                default: color = tintColor1.Value; break;
                case 1:  color = tintColor2.Value; break;
                case 2:  color = tintColor3.Value; break;
            }

            customMenu.createSlider("sliderColorR", "R", 0f, 100f, 10f, color.r * 100, sliderColorRChanged);
            customMenu.createSlider("sliderColorG", "G", 0f, 100f, 10f, color.g * 100, sliderColorGChanged);
            customMenu.createSlider("sliderColorB", "B", 0f, 100f, 10f, color.b * 100, sliderColorBChanged);
            customMenu.createSlider("sliderColorA", "A", 0f, 100f, 10f, color.a * 100, sliderColorAChanged);
        }

        private static void selectorCurrentColorBeingEditedChanged(float newVal)
        {
            Plugin.LogInfo($"selectorCurrentColorBeingEditedChanged: {newVal}");

            currentColorBeingEdited = (int)newVal;

            customMenu.menuList.Remove(customMenu.menuList.Find(x => x.optionID == "sliderColorR"));
            customMenu.menuList.Remove(customMenu.menuList.Find(x => x.optionID == "sliderColorG"));
            customMenu.menuList.Remove(customMenu.menuList.Find(x => x.optionID == "sliderColorB"));
            customMenu.menuList.Remove(customMenu.menuList.Find(x => x.optionID == "sliderColorA"));
        }

        private static void sliderColorChanged(string optionID, float newVal)
        {
            LogInfo($"Slider \"{optionID}\" set to value: {newVal}");
            newVal = newVal / 100;

            Color color;

            switch (currentColorBeingEdited)
            {
                default: color = new Color(tintColor1.Value.r, tintColor1.Value.g, tintColor1.Value.b, tintColor1.Value.a); break;
                case 1:  color = new Color(tintColor2.Value.r, tintColor2.Value.g, tintColor2.Value.b, tintColor2.Value.a); break;
                case 2:  color = new Color(tintColor3.Value.r, tintColor3.Value.g, tintColor3.Value.b, tintColor3.Value.a); break;
            }

            if (optionID.Equals("sliderColorR"))
            {
                color.r = newVal;
            }
            else if (optionID.Equals("sliderColorG"))
            {
                color.g = newVal;
            }
            else if (optionID.Equals("sliderColorB"))
            {
                color.b = newVal;
            }
            else if (optionID.Equals("sliderColorA"))
            {
                color.a = newVal;
            }

            switch (currentColorBeingEdited)
            {
                default: tintColor1.Value = color; break;
                case 1:  tintColor2.Value = color; break;
                case 2:  tintColor3.Value = color; break;
            }
            
        }

        private static void sliderColorRChanged(float newVal)
        {
            sliderColorChanged("sliderColorR", newVal);
        }

        private static void sliderColorGChanged(float newVal)
        {
            sliderColorChanged("sliderColorG", newVal);
        }

        private static void sliderColorBChanged(float newVal)
        {
            sliderColorChanged("sliderColorB", newVal);
        }

        private static void sliderColorAChanged(float newVal)
        {
            sliderColorChanged("sliderColorA", newVal);
        }

        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
    }
}
