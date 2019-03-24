using System.Web.Http;
using System.Web.Http.Cors;

namespace BlogProject
{
    /// <summary>
    /// owin接口 user
    /// </summary>
    [RoutePrefix("User")]
    //允许跨域
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public  class UserController:ApiController
    {
        [HttpGet]
        public string GetTest()
        {
            return "This is  only a test function!";
        }
        [HttpPost]
        public string GetUserInfo([FromBody] int gender)
        {
            string str = "";
            switch (gender)
            {
                case 1:
                    str = "Jack is man!";
                    break;
                case 0:
                    str = "Rose is woman!";
                    break;
            }
            return str;
        }
    }
}