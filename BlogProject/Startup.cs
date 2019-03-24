using System;
using System.IO;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Web.Http.Cors;
using Swashbuckle.Application;
using BlogProject;

[assembly: OwinStartup(typeof(Startup))]

namespace BlogProject
{
    public class Startup
    {
        private const string LogName = "OwinStartUp";

        public void Configuration(IAppBuilder app)
        {
            // web api 接口
            HttpConfiguration config = InitWebApiConfig();

            #region 将Swagger配置到Owin中
            config.EnableSwagger(c => c.SingleApiVersion("v1", "接口测试")).EnableSwaggerUi();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("multipart/form-data")); 
            #endregion

            app.UseWebApi(config);
            app.Use((context, fun) =>
            {

                return RequestHandle(context, fun);
            });

        }

        private Task RequestHandle(IOwinContext context, Func<Task> next)
        {
            //获取物理文件路径
            var path = GetFilePath(context.Request.Path.Value);

            //验证路径是否存在
            if (File.Exists(path))
            {
                return SetResponse(context, path);
            }

            //不存在返回下一个请求
            return next();
        }

        private Task SetResponse(IOwinContext context, string path)
        {
            string[] head = new string[1];
            head[0] = "*";
            context.Response.Headers.Add("Access-Control-Allow-Origin", head);
            context.Response.ContentType = "text/plain";
            var perfix = Path.GetExtension(path);
            string str = path.ToLower();
            str = str.TrimStart('.');
            switch (perfix)
            {

                case ".js":
                    context.Response.ContentType = "text/javascript"; /*"application/x-javascript";*/
                    break;
                case ".css":
                    context.Response.ContentType = "text/css";
                    break;
                case ".html":
                    context.Response.ContentType = "text/html";
                    break;
                case ".htm":
                    context.Response.ContentType = "text/html";
                    break;
                case ".svg":
                    context.Response.ContentType = "text/xml";
                    break;
                case ".png":
                    context.Response.ContentType = "image/png";
                    break;
                default:
                    context.Response.ContentType = "text/plain";
                    break;
            }
            //StringBuilder sb = new StringBuilder();
            try
            {
                byte[] contentText = File.ReadAllBytes(str);
                return Write(context.Response, contentText);

            }
            catch (Exception ex)
            {
                return EmptyWrite(context.Response);
            }

        }
        private async Task EmptyWrite(IOwinResponse resp)
        {
            Task a = Task.Run(() =>
            {
                try
                {
                    resp.Write("");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"IOwinResponse_发布定制服务的owin异常。{ex.Message}");
                }
            });
            await a;
        }

        private Task Write(IOwinResponse resp, string sMsg)
        {
            try
            {
                return resp.WriteAsync(sMsg);
            }
            catch (Exception ex)
            {

            }
            return EmptyWrite(resp);
        }

        private Task Write(IOwinResponse resp, byte[] sMsg)
        {
            try
            {
                return resp.WriteAsync(sMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IOwinResponse_发布定制服务的owin异常。{ex.Message}");
            }
            return EmptyWrite(resp);
        }
        private string GetFilePath(string relPath)
        {
            return Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory
                , ""
                , relPath.TrimStart('/').Replace('/', '\\'));
        }

        private HttpConfiguration InitWebApiConfig()
        {
            // 创建 Web API 的配置
            HttpConfiguration config = new HttpConfiguration();
            try
            {
                //跨域配置
                config.EnableCors(new EnableCorsAttribute("*", "*", "*"));
                // 启用标记路由
                //config.MapHttpAttributeRoutes();
                // 默认的 Web API 路由
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{action}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );
                config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
                //默认返回 json
                config.Formatters.JsonFormatter.MediaTypeMappings.Add(
                    new QueryStringMapping("datatype", "json", "application/json"));
                //返回格式选择
                config.Formatters.XmlFormatter.MediaTypeMappings.Add(
                    new QueryStringMapping("datatype", "xml", "application/xml"));
                //json 序列化设置
                config.Formatters.JsonFormatter.SerializerSettings = new Newtonsoft.Json.JsonSerializerSettings()
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"IOwinResponse_发布定制服务的owin异常。{ex.Message}");
            }
            return config;
        }
    }

}
