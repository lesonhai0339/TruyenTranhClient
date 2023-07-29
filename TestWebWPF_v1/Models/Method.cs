using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestWebWPF_v1.Controllers;
using TestWebWPF_v1.Models;

namespace TestWebWPF_v1.Models
{
    public class Method
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Method(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        //Hàm random không dùng ở đây mà dùng ở client do client gửi nguyên một đối tượng đến chứ không phải từng tham số
        public string ranDomId(List<string> dstruyen)
        {
            Random data = new Random();
            int number = data.Next(1, 1000000);
            string randomNumber = number.ToString("d6");
            return randomNumber;
        }
    }
}
