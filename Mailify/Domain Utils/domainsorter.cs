using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using static Mailify.ConsoleUtilities;
using System.Drawing;
using Console = Colorful.Console;

namespace Mailify
{
    class domainsorter
    {
        public static void sorter()
        {

            int gmailc = 0;
            int yahooc = 0;
            int hotmailc = 0;
            int aolc = 0;
            int homailukc = 0;
            int hotmailfrc = 0;
            int yahoofrc = 0;
            int wanadoofrc = 0;
            int orangefrc = 0;
            int comcastc = 0;
            int yahoocouk = 0;
            int yahoocombr = 0;
            int yahoocoin = 0;
            int livecom = 0;
            int icloud = 0;
            int freefr = 0;
            int gmxde = 0;
            int webde = 0;
            int yandexru = 0;
            int ymail = 0;
            int outlookc = 0;
            int mailruc = 0;
            int googlemail = 0;
            int livecouk = 0;
            int verizonnet = 0;
            int protonmailc = 0;
            int gmxnet = 0;
            WriteTitle();
            Directory.CreateDirectory("./Domain Sorter");
            string fileName;
            OpenFileDialog openFileDialog = new OpenFileDialog();

            do
            {
                string type = "Combos";
                Print("Press any key to load " + type + ".\n", PrintType.Input);
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

            Print("Sorting ComboList...\n", PrintType.Output);
            string[] array = File.ReadAllLines(fileName);
            foreach (string text in array)
            {


                if (text.Contains("@gmail.com"))
                {
                    gmailc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@yahoo.com"))
                {
                    yahooc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@hotmail.com"))
                {
                    hotmailc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@aol.com"))
                {
                    aolc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("hotmail.co.uk"))
                {
                    homailukc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@hotmail.fr"))
                {
                    hotmailfrc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@yahoo.fr"))
                {
                    yahoofrc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@wanadoo.fr"))
                {
                    wanadoofrc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@orange.fr"))
                {
                    orangefrc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@comcast.net"))
                {
                    comcastc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@yahoo.co.uk"))
                {
                    yahoocouk++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@yahoo.com.br"))
                {
                    yahoocombr++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@yahoo.co.in"))
                {
                    yahoocoin++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@live.com"))
                {
                    livecom++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@icloud.com"))
                {
                    icloud++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@free.fr"))
                {
                    freefr++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@gmx.de"))
                {
                    gmxde++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@web.de"))
                {
                    webde++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@yandex.ru"))
                {
                    yandexru++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@ymail.com"))
                {
                    ymail++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@outlook.com"))
                {
                    outlookc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@mail.ru"))
                {
                    mailruc++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@googlemail.com"))
                {
                    googlemail++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@live.co.uk"))
                {
                    livecouk++;
                }
            }
            foreach (string text in array)
            {


                if (text.Contains("@verizon.net"))
                {
                    verizonnet++;
                }

            }
            foreach (string text in array)
            {


                if (text.Contains("@protonmail.com"))
                {
                    protonmailc++;
                }

            }
            foreach (string text in array)
            {


                if (text.Contains("@gmx.net"))
                {
                    gmxnet++;
                }

            }

            int count1 = File.ReadAllLines(fileName).Length;
            int other1 = (gmailc + yahooc + hotmailc + gmxnet + aolc + homailukc + hotmailfrc + yahoofrc + orangefrc + wanadoofrc + orangefrc + comcastc + yahoocouk + yahoocombr + yahoocoin + livecom + icloud + freefr + gmxde + webde + yandexru + ymail + outlookc + mailruc + googlemail + livecouk + verizonnet + protonmailc);
            int other = count1 - other1;
            WriteTitle();
            Console.WriteLine("Total Number Of Lines = " + count1, Color.White);
            Console.WriteLine("----------------------------------------------------------");
            Console.WriteLine("Gmail.com = " + gmailc, Color.White);
            Console.WriteLine("Yahoo.com = " + yahooc, Color.White);
            Console.WriteLine("Hotmail.com = " + hotmailc, Color.White);
            Console.WriteLine("Aol.com = " + aolc, Color.White);
            Console.WriteLine("Homtmail.co.uk = " + homailukc, Color.White);
            Console.WriteLine("Hotmail.fr = " + hotmailfrc, Color.White);
            Console.WriteLine("Yahoo.fr = " + yahoofrc, Color.White);
            Console.WriteLine("Wandoo.fr = " + wanadoofrc, Color.White);
            Console.WriteLine("Orange.fr = " + orangefrc, Color.White);
            Console.WriteLine("Comcast.net = " + comcastc, Color.White);
            Console.WriteLine("Gmx.net = " + gmxnet, Color.White);
            Console.WriteLine("Yahoo.co.uk = " + yahoocouk, Color.White);
            Console.WriteLine("Yahoo.co.br = " + yahoocombr, Color.White);
            Console.WriteLine("Yahoo.co.in = " + yahoocoin, Color.White);
            Console.WriteLine("Live.com = " + livecom, Color.White);
            Console.WriteLine("Icloud.com = " + icloud, Color.White);
            Console.WriteLine("free.fr = " + freefr, Color.White);
            Console.WriteLine("gmx.de = " + gmxde, Color.White);
            Console.WriteLine("web.de = " + webde, Color.White);
            Console.WriteLine("yandex.ru = " + yandexru, Color.White);
            Console.WriteLine("ymail.com = " + ymail, Color.White);
            Console.WriteLine("outlook.com = " + outlookc, Color.White);
            Console.WriteLine("mail.ru = " + mailruc, Color.White);
            Console.WriteLine("googlemail.com = " + googlemail, Color.White);
            Console.WriteLine("live.co.uk = " + livecouk, Color.White);
            Console.WriteLine("verizon.net = " + verizonnet, Color.White);
            Console.WriteLine("protonmail.com = " + protonmailc, Color.White);
            Console.WriteLine("Others = " + other, Color.White);
            Console.WriteLine("----------------------------------------------------------");
            Print("Press Any Key To Continue...\n", PrintType.Output);
            Console.ReadKey();
            string yesno = "Y";
            if (yesno == "Y" || (yesno == "y"))
            {
                Print("Saving Results...\n", PrintType.Output);
                Directory.CreateDirectory("./Domain Sorter");
                string[] array1 = File.ReadAllLines(fileName);
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@gmail.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@gmail.com") || (text.Contains("@Gmail.com")))
                        {
                            gmail.WriteLine(text);
                        }

                    }

                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@yahoo.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@yahoo.com") || (text.Contains("@Yahoo.com")))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@hotmail.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@hotmail.com") || (text.Contains("@Hotmail.com")))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@aol.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@aol.com"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@hotmail.co.uk.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@hotmail.co.uk"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@gmx.net.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@gmx.net"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@hotmail.fr.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@hotmail.fr"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@yahoo.fr.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@yahoo.fr"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@wanadoo.fr.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@wanadoo.fr"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@orange.fr.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@orange.fr"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@comcast.net.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@comcast.net"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@yahoo.co.uk.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@yahoo.co.uk"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@yahoo.com.br.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@yahoo.com.br"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@yahoo.co.in.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@yahoo.co.in"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@live.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@live.com"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@icloud.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@icloud.com.com"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@free.fr.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@free.fr"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@gmx.de.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@gmx.de"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@web.de.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@web.de"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@yandex.ru.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@yandex.ru"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@ymail.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@ymail.com"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@outlook.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@outlook.com"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@mail.ru.txt"))
                {

                    foreach (string text in array)
                    {


                        if (text.Contains("@mail.ru"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@googlemail.com.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@googlemail.com"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@live.co.uk.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@live.co.uk"))
                        {
                            gmail.WriteLine(text);
                        }
                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/@verizon.net.txt"))
                {
                    foreach (string text in array)
                    {


                        if (text.Contains("@verizon.net"))
                        {
                            gmail.WriteLine(text);
                        }

                    }
                }
                using (StreamWriter gmail = new StreamWriter("./Domain Sorter/other domain.txt"))
                {
                    foreach (string text in array)
                    {
                        if (!text.Contains("@gmail.com"))
                        {
                            if (!text.Contains("@yahoo.com"))
                            {


                                if (!text.Contains("@hotmail.com"))
                                {
                                    if (!text.Contains("@gmx.net"))
                                    {
                                        if (!text.Contains("@aol.com"))
                                        {
                                            if (!text.Contains("@hotmail.co.uk"))
                                            {
                                                if (!text.Contains("@hotmail.fr"))
                                                {
                                                    if (!text.Contains("@yahoo.fr"))
                                                    {
                                                        if (!text.Contains("@wandoo.fr"))
                                                        {
                                                            if (!text.Contains("@orange.fr"))
                                                            {
                                                                if (!text.Contains("@comcast.net"))
                                                                {
                                                                    if (!text.Contains("@yahoo.co.uk"))
                                                                    {
                                                                        if (!text.Contains("@yahoo.co.br"))
                                                                        {
                                                                            if (!text.Contains("@yahoo.co.in"))
                                                                            {
                                                                                if (!text.Contains("@live.com"))
                                                                                {
                                                                                    if (!text.Contains("@icloud.com"))
                                                                                    {
                                                                                        if (!text.Contains("@free.fr"))
                                                                                        {
                                                                                            if (!text.Contains("@gmx.de"))
                                                                                            {
                                                                                                if (!text.Contains("@web.de"))
                                                                                                {

                                                                                                    if (!text.Contains("@yandex.ru"))
                                                                                                    {
                                                                                                        if (!text.Contains("@ymail.com"))
                                                                                                        {
                                                                                                            if (!text.Contains("@outlook.com"))
                                                                                                            {
                                                                                                                if (!text.Contains("@mail.ru"))
                                                                                                                {
                                                                                                                    if (!text.Contains("@googlemail.com"))
                                                                                                                    {
                                                                                                                        if (!text.Contains("@live.co.uk"))
                                                                                                                        {
                                                                                                                            if (!text.Contains("@verizon.net"))
                                                                                                                            {
                                                                                                                                if (!text.Contains("@protonmail.com"))
                                                                                                                                {
                                                                                                                                    gmail.WriteLine(text);
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }

                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }


                    }
                }
                Print("Press any key to go to back...\n", PrintType.Output);
                Console.ReadKey();
                Console.Clear();
                Menu.DomainUtils();

            }
        }
    }
}