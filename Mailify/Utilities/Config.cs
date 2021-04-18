using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mailify.ConsoleUtilities;
using System.Threading;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;
using Console = Colorful.Console;

namespace Mailify
{
    public class Config
    {
        public static string Enable_Bot;
        public static string Discord_Id;
        public static string prefix;
        public static string json;
        public static JObject res;
        public static string Remove_Dupes;
        public static string proxytimeout;
        public static string Capture_Subscriptions;
        public static string kekr_UI;

        public static void cfg_menu()
        {
            WriteTitle();
            Console.Write("[", Color.Lavender);
            Console.Write("1", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Enable Bot - ", Color.White);
            BoolChecker(Enable_Bot);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("2", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Discord Id - " + Discord_Id, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("3", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Prefix (Discord bot) - " + prefix, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("4", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Capture Subs (Yahoo) - ", Color.White);
            BoolChecker(Capture_Subscriptions);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("5", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Auto Remove Dupes - ", Color.White);
            BoolChecker(Remove_Dupes);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("6", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Proxy Timeout - " + proxytimeout, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("7", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Checker Ui - " + kekr_UI, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("8", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Back", Color.White);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("> ");
            switch (Console.ReadKey(true).KeyChar)
            {
                case '1':
                    if (Convert.ToBoolean(Enable_Bot) == true)
                        res["Enable Bot"] = false;
                    else
                        res["Enable Bot"] = true;
                    handleconfig();
                    break;
                case '2':
                    Console.WriteLine();
                    Print("Discord Id: ", PrintType.Input);
                    string con = Console.ReadLine();
                    res["Discord Id"] = con;
                    handleconfig();
                    break;
                case '3':
                    Console.WriteLine();
                    Print("Prefix: ", PrintType.Input);
                    string con11 = Console.ReadLine();
                    res["Prefix"] = con11;
                    handleconfig();
                    break;
                case '4':
                    if (Convert.ToBoolean(Capture_Subscriptions) == true)
                        res["Capture Subs (Yahoo)"] = false;
                    else
                        res["Capture Subs (Yahoo)"] = true;
                    handleconfig();
                    break;
                case '5':
                    if (Convert.ToBoolean(Remove_Dupes) == true)
                        res["Auto Remove Dupes"] = false;
                    else
                        res["Auto Remove Dupes"] = true;
                    handleconfig();
                    break;
                case '6':
                    Console.WriteLine();
                    Print("ProxyTimeout: ", PrintType.Input);
                    string con12 = Console.ReadLine();
                    res["ProxyTimeout"] = con12;
                    handleconfig();
                    break;
                case '7':
                    if (kekr_UI == "CUI")
                        res["Checker Ui"] = "LOG";
                    else
                        res["Checker Ui"] = "CUI";
                    handleconfig();
                    break;
                case '8':
                    Menu.mainmenu();
                    break;
                default:
                    cfg_menu();
                    break;


            }
        }
        public static void handleconfig()
        {
            SetConfig();
            cfg_menu();
        }
        public static void parseconfig()
        {
            try
            {
                json = File.ReadAllText("Files//Config.json");
            }
            catch
            {
                MessageBox.Show("Config.json, Not Found", "Mailify");
                Environment.Exit(0);
            }
            res = JObject.Parse(json);
            Enable_Bot = res["Enable Bot"].ToString();
            Discord_Id = res["Discord Id"].ToString();
            Capture_Subscriptions = res["Capture Subs (Yahoo)"].ToString();
            prefix = res["Prefix"].ToString();
            Remove_Dupes = res["Auto Remove Dupes"].ToString();
            proxytimeout = res["ProxyTimeout"].ToString();
            kekr_UI = res["Checker Ui"].ToString();
        }
        public static void SetConfig()
        {
            File.WriteAllText("Files//Config.json", res.ToString());
            parseconfig();
        }
        public static void BoolChecker(string val)
        {
            try
            {
                bool does = Convert.ToBoolean(val);
                if (does == true)
                {
                    Console.Write("True", Color.Green);
                }
                else
                {
                    Console.Write("False", Color.Red);
                }
            }
            catch
            {
                MessageBox.Show("There is error in Config.json please check it");
                Environment.Exit(0);
            }
        }
    }
}
