using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject
{
    class Restful
    {
        public static bool Start(string theUrl)
        {
            try
            {
                Console.WriteLine($"WebApi开始启动，获取的url是{theUrl}。");
                if (string.IsNullOrWhiteSpace(theUrl))
                {
                    Console.WriteLine("WebApi启动时，获取url为空");
                    return false;
                }
                var startOpts = new StartOptions(theUrl);
                // //以当前的Options和Startup启动Server
                WebApp.Start<Startup>(startOpts);
                Console.WriteLine($"-------------WebApi启动成功----------------");
                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"WebApi启动失败。owin地址{theUrl}。{ex}");
                return false;
            }

        }
    }
}
