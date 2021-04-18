using System.Drawing;
using Console = Colorful.Console;

namespace Mailify {
    internal class ConsoleUtilities {
        public static void WriteTitle() {
            Console.Clear();
            Console.WriteLine();
            string[] ascii = new string[]
            {
                " ███▄ ▄███▓ ▄▄▄       ██▓ ██▓     ██▓  █████▒▓██   ██▓",
                "▓██▒▀█▀ ██▒▒████▄    ▓██▒▓██▒    ▓██▒▓██   ▒  ▒██  ██▒",
                "▓██    ▓██░▒██  ▀█▄  ▒██▒▒██░    ▒██▒▒████ ░   ▒██ ██░",
                "▒██    ▒██ ░██▄▄▄▄██ ░██░▒██░    ░██░░▓█▒  ░   ░ ▐██▓░",
                "▒██▒   ░██▒ ▓█   ▓██▒░██░░██████▒░██░░▒█░      ░ ██▒▓░",
                "░ ▒░   ░  ░ ▒▒   ▓▒█░░▓  ░ ▒░▓  ░░▓   ▒ ░       ██▒▒▒ ",
                "░  ░      ░  ▒   ▒▒ ░ ▒ ░░ ░ ▒  ░ ▒ ░ ░       ▓██ ░▒░ ",
                "░      ░     ░   ▒    ▒ ░  ░ ░    ▒ ░ ░ ░     ▒ ▒ ░░  ",
                "       ░         ░  ░ ░      ░  ░ ░           ░ ░     "
            };

            for (int i = 0; i < ascii.Length; i++)
                Console.WriteLine("  " + ascii[i], Menu.Theme);
            Console.WriteLine();
        }

        public static void Print(string message, PrintType printType, string indent = "", string customMsg = "") {
            Console.Write(indent + "[", Color.Silver);

            switch (printType) {
                case PrintType.Input:
                    Console.Write(">", Mailify.Menu.Theme);
                    break;

                case PrintType.Output:
                    Console.Write(">", Mailify.Menu.Theme);
                    break;

                case PrintType.Warning:
                    Console.Write(">", Mailify.Menu.Theme);
                    break;

                case PrintType.Error:
                    Console.Write(">", Mailify.Menu.Theme);
                    break;

                case PrintType.Custom:
                    Console.Write(customMsg, Color.LimeGreen);
                    break;

                default:
                    break;
            }

            Console.Write("] ", Color.Silver);
            Console.Write(message, Color.White);
        }
        
        public enum PrintType {
            Input,
            Output,
            Warning,
            Error,
            Custom
        }
    }
}