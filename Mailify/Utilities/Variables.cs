using Leaf.xNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using static Mailify.ConsoleUtilities;
using System.Drawing;
using Console = Colorful.Console;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Mailify
{
    public class Variables
    {
        public static string version = "v3.0";
        public static int Valid;
        public static int Custom;
        public static int Errors;
        public static int cps;
        public static int loadedCombos;
        public static int Invalid;
        public static int proxyIndex;
        public static int Checked;
        public static string Module_Name = "";
        public static string Symbol = "";
        public static string ProxyType { get; set; }
        public static List<Thread> threads = new List<Thread>();
        public static ConcurrentQueue<string> combos = new ConcurrentQueue<string>();
        public static List<string> cloneCombo = new List<string>();
        public static string[] proxies = new string[0];
        public static string mainfolder = Directory.GetCurrentDirectory();
        public static string results;
        public static string subcaps;
        public static bool lines_in_use;
        public static readonly object WriteLock = new object();
        public static string[] OGNAMES;
        public static int Custom_Retries = 1;

       // public static string[] accounts;

        //ADDONS
        public static int Rares;
        public static int Skinned;
        public static int No_Skins;
        public static int FN_Hits;
        public static int XBOX_Hits;

        //
        public static string rares;
        public static string skins;
        public static string vbucks;

        // MC
        public static int three_hundred_over;
        public static int two_hundred_to_three_hundred;
        public static int one_hundred_to_two_hundred;
        public static int fifty_to_one_hundred;
        public static int twenty_five_to_fifty;
        public static int ten_to_twentyfive;
        public static int one_to_10;

        public static int MFA;
        public static int NFA;
        public static int SFA;
        public static int Optifine;
        public static int Minecon;
        public static int Hypixel_Lvl;
        public static int Hypixel_Ranked;
        public static int veltpvp;
        public static int Mineplex;
        public static int Skyblock;
        public static int Hive;
        public static int GettingCaptures;
        public static string BASE_NAME = "";

        //folder strings
        public static string Hypixel_Leveled_Folder;
        public static string Hypixel_Ranked_folder;
        public static string VeltPvp_Folder;
        public static string Hive_folder;
        public static string Capes_Folder;
        public static string Skyblock_Coins_Folder;


        public static void remove(string Combo)
        {
            if (lines_in_use == true)
            {
                cloneCombo.Remove(Combo);
            }
        }
        public static void Mail_Mode_Title()
        {
            while (true)
            {
               switch (Config.kekr_UI)
                {
                    case "CUI":
                        Console.Clear();
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Valids: {Valid} | Customs: {Custom} | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        WriteTitle();
                        Console.Write(" Mailify is Running - [" + $"{BASE_NAME}.txt" + "]");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Checked, Color.NavajoWhite);
                        Console.Write("/" + loadedCombos + "] Checked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Valid, Color.Green);
                        Console.Write("] Valids");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Custom, Color.OrangeRed);
                        Console.Write("] Customs");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Invalid, Color.Red);
                        Console.Write("] Invalid");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Errors, Color.DarkGray);
                        Console.Write("] Retries");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(cps * 60, Color.DeepSkyBlue);
                        Console.Write("] CPM");
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                    case "LOG":
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Valids: {Valid} | Customs: {Custom} | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                }
            }
        

        }
        public static void VM_title()
        {
            while (true)
            {
                switch (Config.kekr_UI)
                {
                    case "CUI":
                        Console.Clear();
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Registered: {Valid} | Not Registered: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        WriteTitle();
                        Console.Write(" Mailify is Running - [" + $"{BASE_NAME}.txt" + "]");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Checked, Color.NavajoWhite);
                        Console.Write("/" + loadedCombos + "] Checked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Valid, Color.Green);
                        Console.Write("] Registered");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Invalid, Color.Red);
                        Console.Write("] Not Registered");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Errors, Color.DarkGray);
                        Console.Write("] Retries");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(cps * 60, Color.DeepSkyBlue);
                        Console.Write("] CPM");
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                    case "LOG":
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Registered: {Valid} | Not Registered: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                           
                }
               
            }
        }

        public static void Inbox_title()
        {
            while (true)
            {
                switch (Config.kekr_UI)
                {
                    case "CUI":
                        Console.Clear();
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Matched: {Valid} | Empty/Customs: {Custom} | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        WriteTitle();
                        Console.Write(" Mailify is Running - [" + $"{BASE_NAME}.txt" + "]");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Checked, Color.NavajoWhite);
                        Console.Write("/" + loadedCombos + "] Checked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Valid, Color.Green);
                        Console.Write("] Matched");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Custom, Color.OrangeRed);
                        Console.Write("] Empty/Customs");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Invalid, Color.Red);
                        Console.Write("] Invalid");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Errors, Color.DarkGray);
                        Console.Write("] Retries");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(cps * 60, Color.DeepSkyBlue);
                        Console.Write("] CPM");
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                    case "LOG":
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Matched: {Valid} | Empty/Customs: {Custom} | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                } 
            }
        }

        public static void XboxFN_FULL_CAP_title()
        {
            while (true)
            {
                switch (Config.kekr_UI)
                {
                    case "CUI":
                        Console.Clear();
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Valids: {Valid} (FN Hits: {FN_Hits} | Skinned: {Skinned} | No-Skins: {No_Skins} | Rares: {Rares} - Xbox Hits: {XBOX_Hits}) | Customs: {Custom} | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        WriteTitle();
                        Console.Write(" Mailify is Running - [" + $"{BASE_NAME}.txt" + "]");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Checked, Color.NavajoWhite);
                        Console.Write("/" + loadedCombos + "] Checked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Valid, Color.Green);
                        Console.Write("] Valids");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(one_to_10, Color.BlueViolet);
                        Console.Write("] 1-10");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(ten_to_twentyfive, Color.AliceBlue);
                        Console.Write("] 10-25");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(twenty_five_to_fifty, Color.Cyan);
                        Console.Write("] 25-50");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(fifty_to_one_hundred, Color.OrangeRed);
                        Console.Write("] 50-100");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(one_hundred_to_two_hundred, Color.DarkMagenta);
                        Console.Write("] 100-200");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(two_hundred_to_three_hundred, Color.Crimson);
                        Console.Write("] 200-300");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(three_hundred_over, Color.Cornsilk);
                        Console.Write("] 300+");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Custom, Color.OrangeRed);
                        Console.Write("] Customs");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Skinned, Color.Aqua);
                        Console.Write("] Skinned");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Rares, Color.Aquamarine);
                        Console.Write("] Rares");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Invalid, Color.Red);
                        Console.Write("] Invalid");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Errors, Color.DarkGray);
                        Console.Write("] Retries");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(cps * 60, Color.DeepSkyBlue);
                        Console.Write("] CPM");
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                     
                    case "LOG":
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Valids: {Valid} (FN Hits: {FN_Hits} | Skinned: {Skinned} | No-Skins: {No_Skins} | Rares: {Rares} - Xbox Hits: {XBOX_Hits}) | Customs: {Custom} | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                }
                
            }
               
        }

        public static void microsoftMC()
        {
            while (true)
            {
                switch (Config.kekr_UI)
                {
                    case "CUI":
                        Console.Clear();
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Valids: {Valid} (Hypixel Leveled: {Hypixel_Lvl} | Server Ranked: {Hypixel_Ranked + veltpvp + Hive} | Skyblock: {Skyblock} | Optifine: {Optifine} | Minecon: {Minecon} - Getting Captures: {GettingCaptures}) | Customs: {Custom} | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        WriteTitle();
                        Console.Write(" Mailify is Running - [" + $"{BASE_NAME}.txt" + "]");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Checked, Color.NavajoWhite);
                        Console.Write("/" + loadedCombos + "] Checked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Valid, Color.Green);
                        Console.Write("] Valids");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Hypixel_Lvl, Color.OrangeRed);
                        Console.Write("] Hypixel Leveled");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Hypixel_Ranked, Color.Aqua);
                        Console.Write("] Hypixel Ranked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(veltpvp, Color.CadetBlue);
                        Console.Write("] Veltpvp Ranked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Hive, Color.Crimson);
                        Console.Write("] Hive Ranked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Skyblock, Color.Chartreuse);
                        Console.Write("] Skyblock");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Optifine, Color.OrangeRed);
                        Console.Write("] Optifine");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Minecon, Color.Crimson);
                        Console.Write("] Minecon");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Custom, Color.OrangeRed);
                        Console.Write("] Customs");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Invalid, Color.Red);
                        Console.Write("] Invalid");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Errors, Color.DarkGray);
                        Console.Write("] Retries");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(cps * 60, Color.DeepSkyBlue);
                        Console.Write("] CPM");
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                    case "LOG":
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Valids: {Valid} (Hypixel Leveled: {Hypixel_Lvl} | Server Ranked: {Hypixel_Ranked + veltpvp + Hive} | Skyblock: {Skyblock} | Optifine: {Optifine} | Minecon: {Minecon} - Getting Captures: {GettingCaptures}) | Customs: {Custom} | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

        public static void McMode_title()
        {
            while (true)
            {
                switch (Config.kekr_UI)
                {
                    case "CUI":
                        Console.Clear();
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Valids: {Valid} (Nfa: {NFA} | Sfa: {SFA} | Mfa: {MFA} | Hypixel Leveled: {Hypixel_Lvl} | Server Ranked: {Hypixel_Ranked + veltpvp + Hive} | Skyblock: {Skyblock} | Optifine: {Optifine} | Minecon: {Minecon} - Getting Captures: {GettingCaptures}) | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        WriteTitle();
                        Console.Write(" Mailify is Running - [" + $"{BASE_NAME}.txt" + "]");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Checked, Color.NavajoWhite);
                        Console.Write("/" + loadedCombos + "] Checked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Valid, Color.Green);
                        Console.Write("] Valids");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(NFA, Color.OrangeRed);
                        Console.Write("] NFA");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(SFA, Color.AliceBlue);
                        Console.Write("] SFA");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(MFA, Color.BlueViolet);
                        Console.Write("] MFA");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Hypixel_Lvl, Color.OrangeRed);
                        Console.Write("] Hypixel Leveled");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Hypixel_Ranked, Color.Aqua);
                        Console.Write("] Hypixel Ranked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(veltpvp, Color.CadetBlue);
                        Console.Write("] Veltpvp Ranked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Hive, Color.Crimson);
                        Console.Write("] Hive Ranked");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Skyblock, Color.Chartreuse);
                        Console.Write("] Skyblock");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Optifine, Color.OrangeRed);
                        Console.Write("] Optifine");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Minecon, Color.Crimson);
                        Console.Write("] Minecon");
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Invalid, Color.Red);
                        Console.Write("] Invalid");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(Errors, Color.DarkGray);
                        Console.Write("] Retries");
                        Console.WriteLine();
                        Console.Write("                     [");
                        Console.Write(cps * 60, Color.DeepSkyBlue);
                        Console.Write("] CPM");
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                    case "LOG":
                        Console.Title = $"Mailify {version} - ({Module_Name}) | Valids: {Valid} (Nfa: {NFA} | Sfa: {SFA} | Mfa: {MFA} | Hypixel Leveled: {Hypixel_Lvl} | Server Ranked: {Hypixel_Ranked + veltpvp + Hive} | Skyblock: {Skyblock} | Optifine: {Optifine} | Minecon: {Minecon} - Getting Captures: {GettingCaptures}) | Invalid: {Invalid} | Retries: {Errors} | CPM: {cps * 60} | Checked: {(Checked)}/{loadedCombos}";
                        cps = 0;
                        Thread.Sleep(1000);
                        break;
                }
            }
        }

    }
}