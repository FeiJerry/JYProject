using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlogProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Restful.Start("http://192.168.0.103:9090");
            while (true)
            {
                Thread.Sleep(1000);
                var str = Console.ReadLine();
                str = str?.ToLower();
                switch (str)
                {
                    case "exit":                    
                        Environment.Exit(1);
                        break;
                }
            }
        }
        
    }
}
