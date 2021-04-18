using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;
using static Mailify.ConsoleUtilities;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AuthGG;
using System.Threading;
using System.Net;

namespace Mailify
{
    public class Menu
    {
        public static Color Theme = Color.Red;

        public static void mainmenu()
        {
            prefunc();
            WriteTitle();
            Console.Write("[", Color.Lavender);
            Console.Write("1", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mail Modules\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("2", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Valid Mail Modules\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("3", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Inbox Scanners\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("4", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mail Spammer\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("5", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Addons\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("6", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Domain Utils\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("7", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Theme System\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("8", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Settings\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("9", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Exit\n", Color.White);
            Console.WriteLine();
            Console.Write("> ");
            switch (Console.ReadKey(true).KeyChar)
            {
                case '1':
                    Mail_Modules();
                    break;

                case '2':
                    VM_Modules();
                    break;

                case '3':
                    Inbox_Modules();
                    break;
                case '4':
                    Spammer.instalize();
                    break;
                case '5':
                    Addons();
                    break;
                case '6':
                    DomainUtils();
                    break;
                case '7':
                    Console.WriteLine();
                    Utilities.SetCol();
                    mainmenu();
                    break;
                case '8':
                    Config.cfg_menu();
                    break;
                case '9':
                    Environment.Exit(0);
                    break;
                default:
                    mainmenu();
                    break;
            }
        }

        public static void Mail_Modules()
        {
            WriteTitle();
            Console.Write("[", Color.Lavender);
            Console.Write("1", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Elastic Email\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("2", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Fast Mail\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("3", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Gmx (Api#1)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("4", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Gmx (Api#2)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("5", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Gmx (Germany)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("6", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Hotmail,Outlook,Live\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("7", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Hotmail,Outlook,Live (Api#2)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("8", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mail.ru\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("9", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mailaccess\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("10", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Orange.fr\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("11", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Rediff Mail\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("12", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Web.de\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("13", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Yahoo (Api#1)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("14", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Yahoo (Api#2)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("15", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Yahoo (Api#3)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("16", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Yandex\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("17", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Abv.bg (Api#1)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("18", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Abv.bg (Api#2)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("19", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Kolab Now\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("20", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Hey Mail\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("21", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Finmail\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("22", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mail.com\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("23", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mail.bg\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("24", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mailfence\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("25", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Protonmail\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("26", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" TalkTalk\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("27", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Back\n", Color.White);
            Console.WriteLine();
            Console.Write("> ");
            switch (Console.ReadLine())
            {
                case "1":
                    Variables.Module_Name = "Elastic Email";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "2":
                    Variables.Module_Name = "Fast Mail";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "3":
                    Variables.Module_Name = "Gmx (Api#1)";
                    Variables.Symbol = "@gmx";
                    Configurataion.Start();
                    break;
                case "4":
                    Variables.Module_Name = "Gmx (Api#2)";
                    Variables.Symbol = "@gmx";
                    Configurataion.Start();
                    break;
                case "5":
                    Variables.Module_Name = "Gmx (Germany)";
                    Variables.Symbol = "@gmx";
                    Configurataion.Start();
                    break;
                case "6":
                    Variables.Module_Name = "Hotmail,Outlook,Live (Api#1)";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "7":
                    Variables.Module_Name = "Hotmail,Outlook,Live (Api#2)";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "8":
                    Variables.Module_Name = "Mail.ru";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "9":
                    Variables.Module_Name = "Mailaccess";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "10":
                    Variables.Module_Name = "Orange.fr";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "11":
                    Variables.Module_Name = "Rediff Mail";
                    Variables.Symbol = "@rediff";
                    Configurataion.Start();
                    break;
                case "12":
                    Variables.Module_Name = "Web.de";
                    Variables.Symbol = "@web.de";
                    Configurataion.Start();
                    break;
                case "13":
                    Variables.Module_Name = "Yahoo (Api#1)";
                    Variables.Symbol = "@yahoo";
                    Configurataion.Start();
                    break;
                case "14":
                    Variables.Module_Name = "Yahoo (Api#2)";
                    Variables.Symbol = "@yahoo";
                    Configurataion.Start();
                    break;
                case "15":
                    Variables.Module_Name = "Yahoo (Api#3)";
                    Variables.Symbol = "@yahoo";
                    Configurataion.Start();
                    break;
                case "16":
                    Variables.Module_Name = "Yandex";
                    Variables.Symbol = "@yandex";
                    Configurataion.Start();
                    break;
                case "17":
                    Variables.Module_Name = "Abv.bg (Api#1)";
                    Variables.Symbol = "@abv.bg";
                    Configurataion.Start();
                    break;
                case "18":
                    Variables.Module_Name = "Abv.bg (Api#2)";
                    Variables.Symbol = "@abv.bg";
                    Configurataion.Start();
                    break;
                case "19":
                    Variables.Module_Name = "Kolab Now";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "20":
                    Variables.Module_Name = "Hey Mail";
                    Variables.Symbol = "@hey.com";
                    Configurataion.Start();
                    break;
                case "21":
                    Variables.Module_Name = "Finmail";
                    Variables.Symbol = "@finmail.com";
                    Configurataion.Start();
                    break;
                case "22":
                    Variables.Module_Name = "Mail.com";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "23":
                    Variables.Module_Name = "Mail.bg";
                    Variables.Symbol = "@mail.bg";
                    Configurataion.Start();
                    break;
                case "24":
                    Variables.Module_Name = "Mailfence";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "25":
                    Variables.Module_Name = "Protonmail";
                    Variables.Symbol = "@protonmail";
                    Configurataion.Start();
                    break;
                case "26":
                    Variables.Module_Name = "TalkTalk";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "27":
                    mainmenu();
                    break;
                default:
                    Mail_Modules();
                    break;
            }
           
        }
        public static void VM_Modules()
        {
            WriteTitle();
            Console.Write("[", Color.Lavender);
            Console.Write("1", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Discord\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("2", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Nintendo\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("3", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Epicgames\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("4", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Paypal\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("5", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Amazon\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("6", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Afterpay\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("7", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Popeyes\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("8", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Grammarly\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("9", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Etsy\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("10", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Walmart\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("11", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Deliveroo\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("12", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Burger King\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("13", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Office Depot\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("14", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Adidas\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("15", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Uber\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("16", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Ebay\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("17", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Venmo\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("18", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Xbox\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("19", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Stock-X\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("20", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Poshmark\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("21", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Twitter\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("22", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Soundcloud\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("23", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Github\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("24", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Facebook\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("25", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Snapchat\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("26", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Chick-fil-A\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("27", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" RobinHood\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("28", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Pornhub\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("29", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Back\n", Color.White);
            Console.WriteLine();
            Console.Write("> ");
            switch (Console.ReadLine())
            {
                case "1":
                    Variables.Module_Name = "Discord VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "2":
                    Variables.Module_Name = "Nintendo VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "3":
                    Variables.Module_Name = "Epicgames VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "4":
                    Variables.Module_Name = "Paypal VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "5":
                    Variables.Module_Name = "Amazon VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "6":
                    Variables.Module_Name = "Afterpay VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "7":
                    Variables.Module_Name = "Popeyes VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "8":
                    Variables.Module_Name = "Grammarly VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "9":
                    Variables.Module_Name = "Etsy VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "10":
                    Variables.Module_Name = "Walmart VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "11":
                    Variables.Module_Name = "Deliveroo VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "12":
                    Variables.Module_Name = "Burger King VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "13":
                    Variables.Module_Name = "Office Depot VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "14":
                    Variables.Module_Name = "Adidas VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "15":
                    Variables.Module_Name = "Uber VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "16":
                    Variables.Module_Name = "Ebay VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "17":
                    Variables.Module_Name = "Venmo VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "18":
                    Variables.Module_Name = "Xbox VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "19":
                    Variables.Module_Name = "Stock-X VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "20":
                    Variables.Module_Name = "Poshmark VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "21":
                    Variables.Module_Name = "Twitter VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "22":
                    Variables.Module_Name = "Soundcloud VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "23":
                    Variables.Module_Name = "Github VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "24":
                    Variables.Module_Name = "Facebook VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "25":
                    Variables.Module_Name = "Snapchat VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "26":
                    Variables.Module_Name = "Chick-fil-A VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "27":
                    Variables.Module_Name = "RobinHood VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "28":
                    Variables.Module_Name = "Pornhub VM";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "29":
                    mainmenu();
                    break;
                default:
                    VM_Modules();
                    break;

            }

        }
        public static void Inbox_Modules()
        {
            WriteTitle();
            Console.Write("[", Color.Lavender);
            Console.Write("1", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Gmx Scan\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("2", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Gmx Germany Scan\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("3", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Web.de Scan\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("4", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mail.bg Scan\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("5", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Abv.bg Scan\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("6", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Yahoo Scan (Api#1)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("7", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Yahoo Scan (Api#2)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("8", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Yahoo Scan (Api#3)\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("9", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Hotmail,Outlook,Live Scan\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("10", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Protonmail Scan\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("11", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Mailaccess Scan\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("12", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Back\n", Color.White);
            Console.WriteLine();
            Console.Write("> ");
            switch (Console.ReadLine())
            {
                case "1":
                    Variables.Module_Name = "Gmx Scan";
                    Variables.Symbol = "@gmx";
                    Configurataion.Start();
                    break;
                case "2":
                    Variables.Module_Name = "Gmx Germany Scan";
                    Variables.Symbol = "@gmx";
                    Configurataion.Start();
                    break;
                case "3":
                    Variables.Module_Name = "Web.de Scan";
                    Variables.Symbol = "@web.de";
                    Configurataion.Start();
                    break;
                case "4":
                    Variables.Module_Name = "Mail.bg Scan";//
                    Variables.Symbol = "@mail.bg";
                    Configurataion.Start();
                    break;
                case "5":
                    Variables.Module_Name = "Abv.bg Scan";
                    Variables.Symbol = "@abv.bg";
                    Configurataion.Start();
                    break;
                case "6":
                    Variables.Module_Name = "Yahoo Scan (Api#1)";
                    Variables.Symbol = "@yahoo";
                    Configurataion.Start();
                    break;
                case "7":
                    Variables.Module_Name = "Yahoo Scan (Api#2)";
                    Variables.Symbol = "@yahoo";
                    Configurataion.Start();
                    break;
                case "8":
                    Variables.Module_Name = "Yahoo Scan (Api#3)";
                    Variables.Symbol = "@yahoo";
                    Configurataion.Start();
                    break;
                case "9":
                    Variables.Module_Name = "Hotmail,Outlook,Live Scan";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "10":
                    Variables.Module_Name = "Protonmail Scan";
                    Variables.Symbol = "@protonmail";
                    Configurataion.Start();
                    break;
                case "11":
                    Variables.Module_Name = "Mailaccess Scan";
                    Variables.Symbol = "@";
                    Configurataion.Start();
                    break;
                case "12":
                    mainmenu();
                    break;
                default:
                    Inbox_Modules();
                    break;
            }
        }
        public static void Addons()
        {
            if (Convert.ToInt32(User.Rank) == 5)
            {
            ight:
                WriteTitle();
                Console.Write("[", Color.Lavender);
                Console.Write("1", Theme);
                Console.Write("]", Color.Lavender);
                Console.Write(" Mc Mode\n", Color.White);
                Console.Write("[", Color.Lavender);
                Console.Write("2", Theme);
                Console.Write("]", Color.Lavender);
                Console.Write(" Microsoft Mc\n", Color.White);
                Console.Write("[", Color.Lavender);
                Console.Write("3", Theme);
                Console.Write("]", Color.Lavender);
                Console.Write(" Xbox Fn\n", Color.White);
                Console.Write("[", Color.Lavender);
                Console.Write("4", Theme);
                Console.Write("]", Color.Lavender);
                Console.Write(" Back\n", Color.White);
                switch (Console.ReadKey(true).KeyChar)
                {
                    case '1':
                        Console.WriteLine();
                        Console.Write("Antisilent ban? [y/n]: ");
                        string k = Console.ReadLine();
                        if (k == "y"){
                            Variables.Module_Name = "Mc Mode (Antisilent Ban)";
                        }
                        else{
                            Variables.Module_Name = "Mc Mode";
                        }
                        Variables.Symbol = ":";
                        Configurataion.Start();
                        break;
                    case '2':
                        Variables.Module_Name = "Microsoft Mc";
                        Variables.Symbol = "@";
                        Configurataion.Start();
                        break;
                    case '3':
                        Variables.Module_Name = "Xbox Fn";
                        Variables.Symbol = "@";
                        Configurataion.Start();
                        break;
                    case '4':
                        mainmenu();
                        break;
                    default:
                        goto ight;
                        break;
                }
            }
            else
            {
                Console.WriteLine("Current Addons: " + @"
- Xbox Fn
- Microsoft Mc
- Mc Mode (Minecraft Checker) + Antisilent ban");
                Console.WriteLine();
                Console.WriteLine("To Purchase Addon-pass - https://mailify.cheap");
                Console.WriteLine("Press any key to go back...");
                Console.ReadKey();
                mainmenu();
            }
            
        }
        public static void DomainUtils()
        {
            WriteTitle();
            Console.Write("[", Color.Lavender);
            Console.Write("1", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Domain Extractor\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("2", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Domain Sorter\n", Color.White);
            Console.Write("[", Color.Lavender);
            Console.Write("3", Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Back\n", Color.White);
            switch (Console.ReadKey(true).KeyChar)
            {
                case '1':
                    domainextractor.extractor();
                    break;
                case '2':
                    domainsorter.sorter();
                    break;
                case '3':
                    mainmenu();
                    break;
                default:
                    DomainUtils();
                    break;

            }
        }
        public static void prefunc()
        {
            Console.Title = $"Mailify [{Variables.version}] - Made by YoBoi";
            Console.ForegroundColor = Color.White;
            Mailify.Config.parseconfig();
            Variables.OGNAMES = File.ReadAllLines("Files//MC_OG Names.txt");
            if (App.GrabVariable("uPYDv5wmX5E9t3yH0SlCYXNisSACt") == "yes")
            {
                System.Diagnostics.Process.Start(App.GrabVariable("xjdCO5DIfIoC6pfQVWiPpfVqNWMgH"));
            }
            if (Config.Enable_Bot == "True")
            {
                new Thread(Stats).Start();
            }
            if (!Directory.Exists("Files//proton"))
            {
                Directory.CreateDirectory("Files//Proton");
                Directory.CreateDirectory("Files//Proton//x64");
                Directory.CreateDirectory("Files//Proton//x86");
                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri("https://mailify.cheap/Updater/ProtonPlugin/x64/GoSrp.dll"), "Files//Proton//x64//GoSrp.dll");
                webClient.DownloadFile(new Uri("https://mailify.cheap/Updater/ProtonPlugin/x86/GoSrp.dll"), "Files//Proton//x86//GoSrp.dll");
            }
        }
        public static void Stats()
        {
            try
            {
                Discord_Stats prog = new Discord_Stats();
                prog.MainAsync().GetAwaiter().GetResult();
            }
            catch 
            {
                MessageBox.Show("Hi there, it appears, there is something wrong with the bot, Report in Bugs Channel!", "Mailify");
            }
        }
    }
}
