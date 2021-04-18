using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static Mailify.ConsoleUtilities;

namespace Mailify {
    internal class Import {
        public static string[] Load(string type) {
            while (true) {
                Print("Press any key to load " + type + ".\n", PrintType.Input);
                Console.ReadKey(true);

                using (OpenFileDialog ofd = new OpenFileDialog()) {
                    try
                    {
                        ofd.Multiselect = false;
                        ofd.CheckFileExists = true;
                        ofd.RestoreDirectory = true;
                        ofd.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
                        ofd.Filter = @"Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                        ofd.Title = $"Load " + type;

                        if (ofd.ShowDialog() == DialogResult.OK)
                        {
                            Print("Loading into memory...\n", PrintType.Output);
                          
                            switch (Convert.ToBoolean(Config.Remove_Dupes))
                            {
                                case true:
                                    if (type == "combos"){
                                        Variables.BASE_NAME = Path.GetFileNameWithoutExtension(ofd.FileName);
                                    }
                                    var array = File.ReadAllLines(ofd.FileName).Distinct().ToArray();
                                    if (array.Length != 0) return array;
                                    Print("Couldn't load " + type + ", ...\n", PrintType.Error);
                                    Thread.Sleep(-1);
                                    break;
                                case false:
                                    if (type == "combos"){
                                        Variables.BASE_NAME = Path.GetFileNameWithoutExtension(ofd.FileName);
                                    }
                                    var array1 = File.ReadAllLines(ofd.FileName).ToArray();
                                    if (array1.Length != 0) return array1;
                                    Print("Couldn't load " + type + ", ...\n", PrintType.Error);
                                    Thread.Sleep(-1);
                                    break;
                                default:
                                    MessageBox.Show("Could not parse, weather to remove dupes or not, so default removed dupes...", "Mailify");
                                    var array2 = File.ReadAllLines(ofd.FileName).Distinct().ToArray();
                                    if (array2.Length != 0) return array2;
                                    Print("Couldn't load " + type + ", ...\n", PrintType.Error);
                                    Thread.Sleep(-1);
                                    break;
                            }
         
                        }

                     }
                       catch (Exception ex) {
                        Print("An unexpected error occurred!\n", PrintType.Error);
                        MessageBox.Show(ex.ToString(), $"Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Thread.Sleep(-1);
                    }
                }
            }
        }
    }
}