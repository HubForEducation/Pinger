﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Pinger.Intefaces;

namespace Pinger.Realizations
{
    public class TcpPinger : Loger, ITcpPinger
    {
        public async Task<Dictionary<string, string>> Ping(Dictionary<string, string> rowhosts)
        {
            var answer = new Dictionary<string, string>();
            foreach (var rowhost in rowhosts.Keys)
            {
                Console.WriteLine(DateTime.Now);
                try
                {
                    var ip = Dns.GetHostAddresses(rowhost);
                    EndPoint endPoint = new IPEndPoint(ip[0], 80);
                    var times = new List<double>();
                    var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    sock.Blocking = true;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    sock.Connect(endPoint);
                    stopwatch.Stop();
                    var t = stopwatch.Elapsed.TotalMilliseconds;
                    times.Add(t);
                    sock.Close();
                    ShowStatusConsole(ref answer, rowhost, true);
                }
                catch (SocketException)
                {
                    ShowStatusConsole(ref answer, rowhost, false);
                }

                ConsoleLogging(rowhost);
            }

            return answer;
        }
    }
}