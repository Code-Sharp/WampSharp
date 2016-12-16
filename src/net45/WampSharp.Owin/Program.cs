using System;
using Microsoft.Owin.Hosting;

namespace WampSharp.Owin
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:8080/"))
            {
                Console.WriteLine("Ready, press any key to exit...");
                Console.ReadKey();
            }

        }
    }
}