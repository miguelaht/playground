// See https://aka.ms/new-console-template for more information
using System.Net;
using PrimeTime.Server;

var s = new Server(IPAddress.Any, 10000);
s.Start();
