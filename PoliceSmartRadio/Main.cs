using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPD_First_Response.Mod.API;
using Rage;
using System.Reflection;
using System.IO;

namespace PoliceSmartRadio
{
    internal class Main : Plugin
    {
        public Main()
        {
            
        }

        public override void Finally()
        {

        }

        public override void Initialize()
        {
            Game.Console.Print("PoliceSmartRadio-LITE " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ", by FAC851, original code by Albo1125, loaded successfully!");
            Game.Console.Print("Special thanks to FinKone for the inspiration and OfficerSquare for the default UI.");
            Game.Console.Print("Please go on duty to start Police SmartRadio.");

            Functions.OnOnDutyStateChanged += Functions_OnOnDutyStateChanged;
        }

        internal static string path = "Plugins/LSPDFR/PoliceSmartRadio/";
        internal static string[] FilesToCheckFor = new string[] { "Audio/ButtonScroll.wav", "Audio/ButtonSelect.wav", "Audio/PlateCheck/TargetPlate1.wav", "Audio/PanicButton.wav",
            "Config/GeneralConfig.ini", "Config/ControllerConfig.ini", "Config/KeyboardConfig.ini", "Config/DisplayConfig.ini", "Config/PanicButton.ini" };
        
        public static void Functions_OnOnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            {
                bool CheckPassedSuccessfully = true;
                string title = "PoliceSmartRadio-LITE warning: ";
                string text;
                
                foreach (string s in FilesToCheckFor)
                {
                    if (!File.Exists(path+s))
                    {
                        Game.LogTrivial("Couldn't find the required file at " + path + s);
                        CheckPassedSuccessfully = false;
                    }
                }
                if (!CheckPassedSuccessfully)
                {
                    text = "PoliceSmartRadio-LITE requires additional files: install all files from the original PoliceSmartRadio except for PoliceSmartRadio.dll and Albo1125.Common.dll.";
                    Game.LogTrivial(title + text);
                    Albo1125.Common.CommonLibrary.ExtensionMethods.DisplayPopupTextBoxWithConfirmation(title, text, true);
                }

                if (File.Exists("Plugins/LSPDFR/Traffic Policer.dll"))
                {
                    text = "Delete 'Traffic Policer.dll' (located in: /Plugins/LSPDFR/) before using PoliceSmartRadio-LITE or reinstall the original full version of PoliceSmartRadio. ";
                    Game.LogTrivial(title + text);
                    Albo1125.Common.CommonLibrary.ExtensionMethods.DisplayPopupTextBoxWithConfirmation(title, text, true);
                    CheckPassedSuccessfully = false;
                }
                if (!CheckPassedSuccessfully) return;

                GameFiber.StartNew(delegate
                {
                    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssemblyEventHandler);
                    PoliceSmartRadio.Initialise();

                });
            }
        }

        public static Assembly ResolveAssemblyEventHandler(object sender, ResolveEventArgs args)
        {
            foreach (Assembly assembly in LSPD_First_Response.Mod.API.Functions.GetAllUserPlugins())
            {
                if (args.Name.ToLower().Contains(assembly.GetName().Name.ToLower()))
                {
                    return assembly;
                }
            }
            return null;
        }
    }
}
