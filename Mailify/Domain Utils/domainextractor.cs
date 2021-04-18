using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using static Mailify.ConsoleUtilities;

namespace Mailify
{
    class domainextractor
    {
        public static void extractor()
        {
            WriteTitle();
            Directory.CreateDirectory("./Domain Extractor");
            string fileName;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            do
            {
                Print("Press any key to Load " + "Combos" + "!\n", PrintType.Input);
                Console.ReadKey();
                openFileDialog.Title = "Select Combo List";
                openFileDialog.DefaultExt = "txt";
                openFileDialog.Filter = "Text files|*.txt";
                openFileDialog.RestoreDirectory = true;
                openFileDialog.ShowDialog();
                fileName = openFileDialog.FileName;
            }
            while (!File.Exists(fileName));

            string[] hello = File.ReadAllLines(fileName);
            Print("Enter " + "Domain to Sort (example @yahoo)" + "!\n", PrintType.Input);


            string read = Console.ReadLine();
            string line;
            using (StreamWriter rite = new StreamWriter("./Domain Extractor/" + read + ".txt"))
            using (StreamReader file = new StreamReader(fileName))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains(read))
                    {
                        rite.WriteLine(line);
                    }

                }
                file.Close();
                rite.Close();
                int count = File.ReadAllLines("./Domain Extractor/" + read + ".txt").Length;
                if (count == 0)
                {
                    Print("No Domains " + "Found in your list" + "!\n", PrintType.Output);
                    Print("Press " + "Any Key to go Back" + "!\n", PrintType.Input);
                    Console.ReadLine();
                    goto helloo;
                }
                else
                {
                    Print("Successfully Extracted\n", PrintType.Output);
                    Print("Saved in " + "/Domain Extractor/" + read + "!\n", PrintType.Output);
                }
            helloo:
                Print("Press Any Key " + " To go back!\n", PrintType.Output);
                Console.ReadKey();
                Menu.DomainUtils();


            }
        }
    }
}
