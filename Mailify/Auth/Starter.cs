using Leaf.xNet;
using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static Mailify.ConsoleUtilities;
using Console = Colorful.Console;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace Mailify
{
    public class Configurataion
    {

        public static void Start()
        {
            folders();
            try
            {
                Console.Clear();
                WriteTitle();
            ThreadsInput:
                Print("Threads: ", PrintType.Input);

                var threads = 0;

                try
                {
                    threads = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Print("Error parsing integer!\n", PrintType.Error);
                    goto ThreadsInput;
                }

            ProxyTypeInput:
                Print("Proxy type? ", PrintType.Input);
                Print("HTTP/s", PrintType.Custom, null, "1");
                Print("Socks-4", PrintType.Custom, " ", "2");
                Print("Socks-5\n", PrintType.Custom, " ", "3");

                var proxyType = default(ProxyType);

                switch (Console.ReadKey(true).KeyChar)
                {
                    case '1':
                        proxyType = ProxyType.HTTP;
                        Print("Using HTTP/s...\n", PrintType.Output);
                        break;

                    case '2':
                        proxyType = ProxyType.Socks4;
                        Print("Using Socks-4...\n", PrintType.Output);
                        break;

                    case '3':
                        proxyType = ProxyType.Socks5;
                        Print("Using Socks-5...\n", PrintType.Output);
                        break;

                    default:
                        Print("Invalid option!\n", PrintType.Error);
                        goto ProxyTypeInput;
                }


                var proxyTimeout = 0;

                try
                {
                    proxyTimeout = int.Parse(Config.proxytimeout);
                }
                catch
                {
                    MessageBox.Show("Could not Parse ProxyTimeout, Please check Config.json","Mailify");
                    Environment.Exit(0);
                }


                var combos = Import.Load("combos");

                foreach (var combo in combos)
                {
                    if (combo.Contains(Variables.Symbol))
                        Variables.combos.Enqueue(combo);
                }

                Variables.loadedCombos = Variables.combos.Count;
                Variables.cloneCombo = Variables.combos.ToList();

                Print("Loaded " + Variables.combos.Count + " combos!\n", PrintType.Output);

                Variables.proxies = Import.Load("proxies");

                Print("Loaded " + Variables.proxies.Length + " proxies!\n", PrintType.Output);

                WriteTitle();
                Variables.lines_in_use = true;
                KeyBinds.save();
                switch (Variables.Module_Name)
                {
                    case "Elastic Email":
                        ElasticEmail.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Fast Mail":
                        Fastmail.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Gmx (Api#1)":
                        Gmx_1.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Gmx (Api#2)":
                        Gmx_2.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Gmx (Germany)":
                        Gmx_Germany.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Hotmail,Outlook,Live (Api#1)":
                        Hotmail.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Hotmail,Outlook,Live (Api#2)":
                        Hotmail_2.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mail.com":
                        Mail_com.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mail.ru":
                        Mail_ru.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mailaccess":
                        MailAccess.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Orange.fr":
                        Orange_FR.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Rediff Mail":
                        Rediff.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Web.de":
                        Web_De.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Yahoo (Api#1)":
                        Yahoo_1.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Yahoo (Api#2)":
                        Yahoo_2.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Yahoo (Api#3)":
                        Yahoo_3.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Yandex":
                        Yandex.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Discord VM":
                        Discord_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Nintendo VM":
                        Nintendo_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Epicgames VM":
                        EpicGames_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Paypal VM":
                        Paypal_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Amazon VM":
                        Amazon_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Afterpay VM":
                        Afterpay_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Gmx Scan":
                        Gmx_Scan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mailaccess Scan":
                        Mailaccess_Scan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Web.de Scan":
                        Web_de_Scan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mail.bg Scan":
                        MailBG_Scan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Grammarly VM":
                        Grammarly_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mc Mode (Antisilent Ban)":
                        Mc_Mode_AntisilentBan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Xbox Fn":
                        Xbox_FN_Full_Cap.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Abv.bg (Api#1)":
                        Abv_bg.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Abv.bg (Api#2)":
                        Abv_bg_2.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Popeyes VM":
                        Popeyes_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Etsy VM":
                        Etsy_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Walmart VM":
                        Walmart_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Deliveroo VM":
                        Deliveroo_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Burger King VM":
                        BurgerKing_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Office Depot VM":
                       OfficeDepot_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Adidas VM":
                        Adidas_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Uber VM":
                        Uber_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Ebay VM":
                       Ebay_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Venmo VM":
                        Venmo_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Abv.bg Scan":
                        Abv_Bg_Scan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Yahoo Scan (Api#1)":
                        Yahoo_1_Scanner.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Yahoo Scan (Api#2)":
                        Yahoo_2_Scanner.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Yahoo Scan (Api#3)":
                        Yahoo_3_Scanner.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Xbox VM":
                        Xbox_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mailfence":
                        Mailfence.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Kolab Now":
                        KolabNow.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Hey Mail":
                        Hey_Mail.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Stock-X VM":
                         StockX_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Poshmark VM":
                        Poshmark_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Finmail":
                        Finmail.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Twitter VM":
                        Twitter_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Hotmail,Outlook,Live Scan":
                        Hotmail_Scan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mail.bg":
                        Mail_bg.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Protonmail":
                        Protonmail.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Protonmail Scan":
                        ProtonMail_Scan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Soundcloud VM":
                        Soundcloud_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Github VM":
                        Github_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Facebook VM":
                        Facebook_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Snapchat VM":
                        Snapchat_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Gmx Germany Scan":
                        Gmx_Germany_Scan.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "TalkTalk":
                        TalkTalk.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Mc Mode":
                        McMode_Normal.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Microsoft Mc":
                        MicrosoftMC.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Chick-fil-A VM":
                        ChickFilA_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "RobinHood VM":
                        Robinhood_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    case "Pornhub VM":
                        Pornhub_VM.Initialize(threads, proxyTimeout, proxyType);
                        break;
                    default:
                        Print("Error...", PrintType.Error);
                        Thread.Sleep(-1);
                        break;
                }
            }

            catch (Exception ex)
            {
                Print("An unexpected error occurred!\n", PrintType.Error);
                MessageBox.Show(ex.Message, $"Mailify", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Thread.Sleep(-1);
            }
        }
        public static void folders()
        {
            switch (Variables.Module_Name)
            {
                case "Xbox Fn":
                    Variables.mainfolder = Directory.GetCurrentDirectory();
                    Variables.results = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\";
                    Variables.vbucks = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "Vbucks Sorted" + "\\";
                    Variables.skins = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "Skins Sorted" + "\\";
                    Variables.rares = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "Rares Sorted" + "\\";
                    if (!Directory.Exists("Results"))
                    {
                        Directory.CreateDirectory("Results");
                    }
                    if (!Directory.Exists(Variables.results))
                    {
                        Directory.CreateDirectory(Variables.results);
                    }
                    if (!Directory.Exists(Variables.vbucks))
                    {
                        Directory.CreateDirectory(Variables.vbucks);
                    }
                    if (!Directory.Exists(Variables.skins))
                    {
                        Directory.CreateDirectory(Variables.skins);
                    }
                    if (!Directory.Exists(Variables.rares))
                    {
                        Directory.CreateDirectory(Variables.rares);
                    }
                    break;
                case "Yahoo (Api#1)":
                case "Yahoo (Api#2)":
                case "Yahoo (Api#3)":
                    Variables.mainfolder = Directory.GetCurrentDirectory();
                    Variables.results = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\";
                    if (Convert.ToBoolean(Config.Capture_Subscriptions) == true)  {
                        Variables.subcaps = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "Subs Captures" + "\\";
                    }
                   
                    if (!Directory.Exists("Results"))
                    {
                        Directory.CreateDirectory("Results");
                    }
                    if (!Directory.Exists(Variables.results))
                    {
                        Directory.CreateDirectory(Variables.results);
                    }
                    if (Convert.ToBoolean(Config.Capture_Subscriptions) == true)
                    {
                        if (!Directory.Exists(Variables.subcaps))
                        {
                            Directory.CreateDirectory(Variables.subcaps);
                        }
                    }
                    break;
                case "Mc Mode":
                case "Mc Mode (Antisilent Ban)":
                case "Microsoft Mc":
                    Variables.mainfolder = Directory.GetCurrentDirectory();
                    Variables.results = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\";
                    Variables.Hypixel_Leveled_Folder = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "Hypixel Leveled" + "\\";
                    Variables.Hypixel_Ranked_folder = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "Hypixel Ranked" + "\\";
                    Variables.VeltPvp_Folder = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "VeltPvp Ranked" + "\\";
                    Variables.Hive_folder = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "HiveMc Ranked" + "\\";
                    Variables.Capes_Folder = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "Capes" + "\\";
                    Variables.Skyblock_Coins_Folder = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + "\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\" + "Skyblock Coins" + "\\";
                    if (!Directory.Exists("Results"))
                    {
                        Directory.CreateDirectory("Results");
                    }
                    if (!Directory.Exists(Variables.results))
                    {
                        Directory.CreateDirectory(Variables.results);
                    }
                    if (!Directory.Exists(Variables.Hypixel_Leveled_Folder))
                    {
                        Directory.CreateDirectory(Variables.Hypixel_Leveled_Folder);
                    }
                    if (!Directory.Exists(Variables.Hypixel_Ranked_folder))
                    {
                        Directory.CreateDirectory(Variables.Hypixel_Ranked_folder);
                    }
                    if (!Directory.Exists(Variables.VeltPvp_Folder))
                    {
                        Directory.CreateDirectory(Variables.VeltPvp_Folder);
                    }
                    if (!Directory.Exists(Variables.Hive_folder))
                    {
                        Directory.CreateDirectory(Variables.Hive_folder);
                    }
                    if (!Directory.Exists(Variables.Capes_Folder))
                    {
                        Directory.CreateDirectory(Variables.Capes_Folder);
                    }
                    if (!Directory.Exists(Variables.Skyblock_Coins_Folder))
                    {
                        Directory.CreateDirectory(Variables.Skyblock_Coins_Folder);
                    }
                    break;
                default:
                    Variables.mainfolder = Directory.GetCurrentDirectory();
                    Variables.results = Variables.mainfolder + "\\Results\\" + $"\\{Variables.Module_Name}\\" + DateTime.Now.ToString("dd-MM-yyyy  HH-mm") + "\\";
                    if (!Directory.Exists("Results"))
                    {
                        Directory.CreateDirectory("Results");
                    }
                    if (!Directory.Exists(Variables.results))
                    {
                        Directory.CreateDirectory(Variables.results);
                    }
                    break;


            }
        }
    }
}
