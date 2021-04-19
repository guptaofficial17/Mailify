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
            Menu.mainmenu();
        }
    }
}


