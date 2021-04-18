using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mailify
{
    class KeyBinds
    {
        public static void save()
        {
            Task.Factory.StartNew(delegate
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        Task.Factory.StartNew(delegate
                        {
                            var key = Console.ReadKey(true);
                            if (key.Key == ConsoleKey.S) 
                            {
                                Variables.lines_in_use = false;
                                File.AppendAllLines($"{Variables.results}//Remaining.txt", Variables.cloneCombo);
                                Process.Start(new ProcessStartInfo("cmd.exe", "/c " + "START CMD /k \"TITLE Mailify && ECHO Saved Remaining Lines...\"") { CreateNoWindow = false, UseShellExecute = false, RedirectStandardOutput = true });
                                Process.GetCurrentProcess().Kill();
                            }
                        });
                    }
                }
            });
        }
    }
}
