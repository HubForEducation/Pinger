﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Pinger.Intefaces;

namespace Pinger.Realizations
{
    public class HttpPinger : Loger, IHttpPinger
    {
        private IConfigurationRoot Configuration = Startup.Builder.Build();

        private string Okanswer { get; } = "OK";
        private string Failedanswer { get; } = "FAILED";
        private readonly List<string> _rowhosts;

        public HttpPinger(List<string> rowhosts)
        {
            _rowhosts = rowhosts;
        }

        public async Task<Dictionary<string, string>> Ping()
        {
            var consoleloger = new ConsoleLoger();
            Dictionary<string, string> answer = new Dictionary<string, string>();
            foreach (var rowhost in _rowhosts)
            {
                Console.WriteLine(DateTime.Now);
                try
                {
                    HttpWebRequest webRequest = (HttpWebRequest) WebRequest
                        .Create(rowhost);
                    webRequest.AllowAutoRedirect = false;
<<<<<<< HEAD
                    var response = (HttpWebResponse) webRequest.GetResponse();
                    if ((int) response.StatusCode == int.Parse(_configuration["Httpvalidcode"]))
                        ShowStatusConsole(ref answer, rowhost, true);
=======
                    HttpWebResponse response = (HttpWebResponse) webRequest.GetResponse();
                    if ((int) response.StatusCode == Int32.Parse(Configuration["Httpvalidcode"]))
                    {
                        answer.Add(rowhost, Okanswer);
                        Console.WriteLine(Okanswer);
                    }
>>>>>>> parent of 5d5f9cd... CLEANUP!
                    else
                        ShowStatusConsole(ref answer, rowhost, false);
                }
                catch (Exception ex)
                {
                    if (ex is PingException ||
                        ex is ArgumentException ||
                        ex is WebException ||
                        ex is UriFormatException ||
                        ex is FormatException ||
                        ex is SocketException)
                        ShowStatusConsole(ref answer, rowhost, false);
                }

                consoleloger.Show(rowhost);
            }

            return answer;
        }
    }
}