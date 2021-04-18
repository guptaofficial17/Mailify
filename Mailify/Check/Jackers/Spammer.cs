using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using static Mailify.ConsoleUtilities;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows;
using System.Drawing;
using Console = Colorful.Console;

namespace Mailify
{
    public class Spammer
    {
        public static string Host;
        public static string Port;
        public static string MyCreds;
        public static string Timeout;
        public static string AmountOfEmailsToSpam;
        public static string Subject;
        public static string EmailToSpam;
        public static string Body;
        public static string json;
        public static JObject res;


        public static int sent;
        public static int errors;

        public static void instalize()
        {
            if(!File.Exists("Files//Spammer.json"))
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri("https://mailify.cheap/Updater/Spammer.json"), "Files//Spammer.json");
            }
            parseconfig();
            WriteTitle();
            Console.Write("[", Color.Lavender);
            Console.Write("1", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Host (Smtp) - " + Host, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("2", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Port (ex. 583) - " + Port, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("3", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Target (Email) - " + EmailToSpam, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("4", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Creds? (Email:Pass) - " + MyCreds, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("5", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Amount of Emails to Send - " + AmountOfEmailsToSpam, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("6", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Subject - " + Subject, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("7", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Body - " + Body, Color.White);
            Console.WriteLine();
            Console.Write("[", Color.Lavender);
            Console.Write("8", Menu.Theme);
            Console.Write("]", Color.Lavender);
            Console.Write(" Back", Color.White);
            Console.WriteLine();
            Console.WriteLine();
            Print("Press (S) To Start, do note, u need to allow: Less secure app access!", PrintType.Input);
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("> ");

            switch (Console.ReadKey(true).KeyChar)
            {
                case '1':
                    Console.WriteLine();
                    Print("Host (Smtp): ", PrintType.Input);
                    string con = Console.ReadLine();
                    res["Host (Smtp)"] = con;
                    handleconfig();
                    break;
                case '2':
                    Console.WriteLine();
                    Print("Port (ex. 583): ", PrintType.Input);
                    string con1 = Console.ReadLine();
                    res["Port (ex. 583)"] = con1;
                    handleconfig();
                    break;
                case '3':
                    Console.WriteLine();
                    Print("Target (Email): ", PrintType.Input);
                    string con7 = Console.ReadLine();
                    res["Target (Email)"] = con7;
                    handleconfig();
                    break;
                case '4':
                    Console.WriteLine();
                    Print("Creds? (Email:Pass): ", PrintType.Input);
                    string con3 = Console.ReadLine();
                    res["Creds? (Email:Pass)"] = con3;
                    handleconfig();
                    break;
                case 'S':
                    Variables.Module_Name = "Mail Spammer";
                    int threads = Convert.ToInt32(AmountOfEmailsToSpam);
                    Thread thread = new Thread((ThreadStart)delegate
                    {
                        while (true)
                        {
                            Console.Title = $"Mailify {Variables.version} - ({Variables.Module_Name}) | Sent: {sent} | Errors: {errors}";
                        }
                    });
                    thread.Start();
                    for (int i = 0; i < threads; i++)
                    {
                        new Thread(new ThreadStart(Worker)).Start();
                    }
                    thread.Abort();
                    Console.WriteLine();
                    Print("Finished Sending, Press Any Key to go back!", PrintType.Input);
                    Console.ReadKey();
                    Menu.mainmenu();
                    break;
                case '5':
                    Console.WriteLine();
                    Print("Email Count (To Send): ", PrintType.Input);
                    string con4 = Console.ReadLine();
                    res["Email Count (To Send)"] = con4;
                    handleconfig();
                    break;
                case '6':
                    Console.WriteLine();
                    Print("Subject: ", PrintType.Input);
                    string con5 = Console.ReadLine();
                    res["Subject"] = con5;
                    handleconfig();
                    break;
                case '7':
                    Console.WriteLine();
                    Print("Body: ", PrintType.Input);
                    string con6 = Console.ReadLine();
                    res["Body"] = con6;
                    handleconfig();
                    break;
                case '8':
                    Menu.mainmenu();
                    break;
                default:
                    instalize();
                    break;


            }
        }
        static void Worker()
        {
            try
            {
                int PORT = Convert.ToInt32(Port);
                string[] array = MyCreds.Split(':');
                SmtpClient client = new SmtpClient(Host, PORT);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(array[0], array[1]);
                MailMessage msg = new MailMessage();
                msg.To.Add(EmailToSpam);
                msg.From = new MailAddress(array[0]);
                msg.Subject = Subject;
                msg.Body = Body;
                client.Send(msg);
                sent++;

            }
            catch 
            {
                errors++;
            }
        }
        public static void handleconfig()
        {
            SetConfig();
            instalize();
        }
        static void parseconfig()
        {
            try
            {
                json = File.ReadAllText("Files//Spammer.json");
            }
            catch
            {
                MessageBox.Show("Spammer.json, Not Found", "Mailify");
                Environment.Exit(0);
            }
            res = JObject.Parse(json);
            Host = res["Host (Smtp)"].ToString();
            Port = res["Port (ex. 587)"].ToString();
            Timeout = res["Timeout (ex. 10000)"].ToString();
            MyCreds = res["Creds? (Email:Pass)"].ToString();
            AmountOfEmailsToSpam = res["Email Count (To Send)"].ToString();
            Subject = res["Subject"].ToString();
            Body = res["Body"].ToString();
            EmailToSpam = res["Target (Email)"].ToString();
        }
        static void SetConfig()
        {
            File.WriteAllText("Files//Spammer.json", res.ToString());
            parseconfig();
        }
    }
}
