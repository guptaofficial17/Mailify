using System;


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


