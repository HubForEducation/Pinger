﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Pinger.Intefaces;


namespace Pinger.Realizations
{
    public class TcpPinger : Loger, ITcpPinger
    {
        private IConfigurationRoot Configuration = Startup.builder.Build();

        private string Okanswer { get; } = "OK";
        private string Failedanswer { get; } = "FAILED";
        private List<string> _rowhosts;
        private string _logpath;
        public TcpPinger(List<string> rowhosts, string logpath)
        {
            _rowhosts = rowhosts;
            _logpath = logpath;
        }
        public async Task<Dictionary<string, string>> Ping()
        {
            Dictionary<string, string> answer = new Dictionary<string, string>();
            foreach (var rowhost in _rowhosts)
            {
                try
                {
                    IPAddress[] ip = Dns.GetHostAddresses(rowhost);
                    EndPoint endPoint = new IPEndPoint(ip[0], 80);
                    var times = new List<double>();
                    var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sock.Blocking = true;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    sock.Connect(endPoint);
                    stopwatch.Stop();
                    double t = stopwatch.Elapsed.TotalMilliseconds;
                    times.Add(t);
                    sock.Close();
                    answer.Add(rowhost, Okanswer);
                    Console.WriteLine(Okanswer);
                }
                catch (SocketException)
                {
                    answer.Add(rowhost, Failedanswer);
                    Console.WriteLine(Failedanswer);
                }
                Console.WriteLine("Host: " + rowhost);
                Console.WriteLine("Period: " + Configuration["Period"]);
                Console.WriteLine("Protocol: " + Configuration["Protocol"]);
                Console.WriteLine();
            }
            return answer;
        }
    }
}