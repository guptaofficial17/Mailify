using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Console = Colorful.Console;
using Colorful;

namespace Mailify
{
    class Utilities
    {
        public static List<Color> colorss = new List<Color>();
        private static void Writeit(int n)
        {
            File.WriteAllText("Files//theme.mailify", n.ToString());
        }

        public static void GetColors()
        {
            foreach (var colorValue in Enum.GetValues(typeof(KnownColor)))
            {
                Color coloor = Color.FromKnownColor((KnownColor)colorValue);
                colorss.Add(coloor);
            }
        }
        public static void SetCol()
        {
            int num = 0;
            for (int i = 0; i < colorss.Count; i++)
            {
                Console.Write("[", Color.Lavender);
                Console.Write($"{num}", Mailify.Menu.Theme);
                Console.Write("]", Color.Lavender);
                string coll = colorss[i].ToString();
                coll = coll.Replace("Color ", "");
                coll = coll.Replace("[", "");
                coll = coll.Replace("]", "");
                Console.Write($" {coll}" + "\n");
                num++;
            }
            Console.WriteLine();
            Console.Write("> ");
            int numm = int.Parse(Console.ReadLine());
            Mailify.Menu.Theme = colorss[numm];
            WriteNum(numm.ToString());
        }

        private static void WriteNum(string num)
        {
            if (!File.Exists("Files//theme.mailify"))
            {
                File.Create("Files//theme.mailify");
                File.WriteAllText("Files//theme.mailify", "1");
            }
            else
            {
                File.WriteAllText("Files//theme.mailify", num);
            }
        }
        public static void GetColor()
        {
            if (!File.Exists("Files//theme.mailify"))
            {
                File.Create("Files//theme.mailify");
                File.WriteAllText("Files//theme.mailify", "1");
            }
            else
            {
                if (File.Exists("Files//theme.mailify"))
                {
                    if (File.ReadAllText("Files//theme.mailify") == null)
                    {
                        File.WriteAllText("Files//theme.mailify", "1");
                    }
                    string color1 = File.ReadAllText("Files//theme.mailify");
                    try
                    {
                        int num = Convert.ToInt32(color1);
                        Mailify.Menu.Theme = colorss[num];
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("Could Not Locate Theme File", "Mailify");
                    }
                }
            }
        }
    }
}
    
