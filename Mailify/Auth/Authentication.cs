using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Leaf.xNet;
using System.Management;
using System.Reflection;
using static Mailify.ConsoleUtilities;
using System.Threading;
using System.Windows;
using System.Drawing;
using AuthGG;
using System.IO.Compression;

namespace Mailify
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Utilities.GetColors();
            Utilities.GetColor();
            Console.Title = $"Mailify [{Variables.version}] - Made by YoBoi";
            auto();
        }
      
        public static void auto()
        {
            OnProgramStart.Initialize("Mailify", "204352", "21kuKrfs7B3OvtypJLVWmwKoiuJe52rUH9C", "30.0");
            if (File.Exists("Files//userinfo.mailify"))
            {
                string[] first = File.ReadAllLines("Files//userinfo.mailify");
                for (int i = 0; i < (int)first.Length; i++)
                {
                    string str = first[i];
                    string[] array = str.Split(new char[]
                    {
                        ':'
                    });
                    if (API.Login(array[0], array[1]))
                    {
                        killwitch();
                    }
                    else
                    {
                        login();
                    }
                }
            }
            else
            {
                login();
            }
        }
        public static void login()
        {
            WriteTitle();
            Colorful.Console.Write("[", Color.Lavender);
            Colorful.Console.Write("1", Menu.Theme);
            Colorful.Console.Write("]", Color.Lavender);
            Colorful.Console.Write(" Login\n", Color.White);
            Colorful.Console.Write("[", Color.Lavender);
            Colorful.Console.Write("2", Menu.Theme);
            Colorful.Console.Write("]", Color.Lavender);
            Colorful.Console.Write(" Register\n", Color.White);
            Console.WriteLine();
            Console.Write("> ");
            switch (Console.ReadKey(true).KeyChar)
            {
                case '1':
                    WriteTitle();
                    Print("Please enter your username: ", PrintType.Input);
                    string username = Console.ReadLine();
                    Print("Please enter your password: ", PrintType.Input);
                    string password = Console.ReadLine();
                    Print("Connecting...\n", PrintType.Output);
                    Thread.Sleep(500);
                    try
                    {
                        if (API.Login(username, password))
                        {
                            using (StreamWriter sw = new StreamWriter("Files//userinfo.mailify", true))
                            {
                                sw.WriteLine(username + ":" + password);
                            }
                            Print("Welcome Back to Mailify " + User.Username, PrintType.Output);
                            Thread.Sleep(400);
                            killwitch();
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                    catch
                    {
                        login();
                    }
                    break;
                case '2':
                    WriteTitle();
                    Print("Please choose a username: ", PrintType.Input);
                    string username1 = Console.ReadLine();
                    Print("Please choose a password: ", PrintType.Input);
                    string password1 = Console.ReadLine();
                    Print("What is your Discord Username: ", PrintType.Input);
                    string email = Console.ReadLine();
                    Print("Enter the license you have gotten: ", PrintType.Input);
                    string lice = Console.ReadLine();
                    Print($"Connecting...\n", PrintType.Output);
                    Thread.Sleep(200);
                    try
                    {
                        if (API.Register(username1, password1, email, lice))
                        {
                            Print("SuccessFully Registered " + username1 + "\n", PrintType.Output);
                            Print("Press Any Key To Continue...\n", PrintType.Output);
                            Console.ReadKey();
                            login();
                        }
                        else
                        {
                            Environment.Exit(0);
                        }
                    }
                    catch
                    {
                        Print("Incorrect License...\n", PrintType.Output);
                        Print("Press Any Key To Continue...\n", PrintType.Output);
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    break;
                default:
                    login();
                    break;
            }
        }
        public static void killwitch()
        {
            try
            {
                using (HttpRequest req = new HttpRequest())
                {
                    req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                    new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
                    string resp = req.Get("https://mailify.cheap/SWITCH.txt").ToString();
                    if (resp == "200")
                    {
                        Menu.mainmenu();
                    }
                    else
                    {
                        MessageBox.Show("External Api Might be Down!", "Mailify");
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("External Api Might be Down!", "Mailify");
                Environment.Exit(0);
            }
        }
    }
}




