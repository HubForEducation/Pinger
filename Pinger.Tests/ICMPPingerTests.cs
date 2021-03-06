﻿using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Pinger.Realizations;

namespace Pinger.Tests
{
    public class IcmpPingerTests
    {
        private readonly string logpath = "./LogsTest.txt";
        private Dictionary<string, string> rowhosts = new Dictionary<string, string>();

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void PingTest()
        {
            var rowhostskeys = new List<string>(File.ReadAllLines("./Hosts.txt"));
            foreach (var item in rowhostskeys)
            {
                rowhosts.Add(item, item);
            }
            var icmppinger = new IcmpPinger();
            var actual = new Dictionary<string, string>();
            var expected = icmppinger.Ping(rowhosts).Result;
            actual.Add("https://www.google.com/", "FAILED");
            actual.Add("https://www.google1234455435435.com/", "FAILED");
            actual.Add("216.58.207.78", "OK");
            actual.Add("34.22.1.23", "FAILED");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LoggingTest()
        {
            var icmppinger = new IcmpPinger();
            var answer = icmppinger.Ping(rowhosts);
            foreach (var item in answer.Result) icmppinger.Logging(item.Key, item.Value);

            File.Delete(logpath);
        }
    }
}