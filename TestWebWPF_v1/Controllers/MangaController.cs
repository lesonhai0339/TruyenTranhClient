using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using TestWebWPF_v1.Models;

namespace TestWebWPF_v1.Controllers
{
    [Route("Admin")]
    public class MangaController : Controller
    {
        private static HttpClient hc = new HttpClient();
        private static string strUrl = @"https://localhost:7132/Truyen-tranh";
        public List<BoTruyen> dstruyen = new List<BoTruyen>();
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            try
            {
                var conn = hc.GetAsync(strUrl + @"/GetAllManga");
                conn.Wait();
                if (conn.Result.IsSuccessStatusCode == false)
                    return ViewBag.request("Không thể kết nối tới máy chủ");
                var kq = conn.Result.Content.ReadAsAsync<List<BoTruyen>>();
                kq.Wait();
                dstruyen = kq.Result.ToList();
                return View(kq.Result.ToList());
            }
            catch (Exception)
            {
                return ViewBag.request("Không thể tải Danh sách truyện.");
            }
        }
        [Route("")]
        [Route("Create")]
        public IActionResult formThemManga()
        {
            return View();
        }
        [Route("")]
        [Route("CreateManga")]
        public IActionResult CreateManga(string MangaName,string MangaDetails,IFormFile MangaImage,string MangaAlternateName
            ,string MangaAuthor,string MangaArtist,string MangaGenre)
        {
            try
            {
                List<string> ds = dstruyen.Select(x => x.MangaId).ToList();// phải lấy tất cả bộ truyện sau đó mới dùng được
                string Id = ranDomId(ds);

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(Id), "MangaId");
                formData.Add(new StringContent(MangaName), "MangaName");
                formData.Add(new StringContent(MangaDetails), "MangaDetails");
                formData.Add(new StreamContent(MangaImage.OpenReadStream()), "MangaImage", MangaImage.FileName);
                formData.Add(new StringContent(MangaAlternateName), "MangaAlternateName");
                formData.Add(new StringContent(MangaAuthor), "MangaAuthor");
                formData.Add(new StringContent(MangaArtist), "MangaArtist");
                formData.Add(new StringContent(MangaGenre), "MangaGenre");
                string url = strUrl + @"/Create";
                var response =hc.PostAsync(url, formData);
                response.Wait();
                if(response.Result.IsSuccessStatusCode)
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return ViewBag.request("Không thể thêm truyện");
            }
            return ViewBag.request("Không thể thêm truyện");
        }
        [Route("")]
        [Route("{Id}/Create")]
        public IActionResult formcreateChapter(string Id)
        {
            ViewBag.MangaId = Id;
            return View();
        }
        [Route("")]
        [Route("CreateChapter")]
        public IActionResult CreateChapter(
            string MangaId,string TieuDe,string tenChuong, List<IFormFile> file,List<string > dsurl)
        {
            try { 
                List<string> ds = getDsChater(); //phải tạo hàm để lấy tất cả chương truyện sao đó mới dùng được
                string Id = "c"+ranDomId(ds);

                var formdata =new MultipartFormDataContent() ;
                if (file != null)
                {
                    foreach (var xfile in file)
                    {
                        formdata.Add(new StreamContent(xfile.OpenReadStream()), "MangaImage",xfile.FileName);
                    }
                }
                if (dsurl.Any())
                {
                    foreach (var xurl in dsurl)
                    {
                        formdata.Add(new StringContent(xurl), "MangaUrl");
                    }
                }
                formdata.Add(new StringContent(Id), "ChapterId");
                formdata.Add(new StringContent(tenChuong), "tenChuong");
                formdata.Add(new StringContent(TieuDe), "TieuDe");
                formdata.Add(new StringContent(MangaId), "MangaId");
                string httpurl = strUrl + @"/{MangaId}/Chapter";
                var response = hc.PostAsync(httpurl, formdata);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                return RedirectToAction("Index", new { success = true });
            }
            catch (Exception)
            {
                return ViewBag.request("Không thể thêm truyện");
            }
            return ViewBag.request("Không thể thêm truyện"); ;
        }

        public string ranDomId(List<string> dstruyen)
        {
            Random data = new Random();
            int number = data.Next(1, 1000000);
            string randomNumber = number.ToString("d6");
            if (dstruyen == null)
            {
                return randomNumber;
            }
            else
            {
                if (dstruyen.Contains(randomNumber))
                {
                    ranDomId(dstruyen);
                }
                return randomNumber;
            }
        }
        private List<string> getDsChater()
        {
            List<string> dsChapter = new List<string>();
            var conn = hc.GetAsync(strUrl + @"/GetdsChapter");
            conn.Wait();
            if (conn.Result.IsSuccessStatusCode == false)
                return ViewBag.request("Không thể kết nối tới máy chủ");
            var kq = conn.Result.Content.ReadAsAsync<List<string>>();
            kq.Wait();
            dsChapter = kq.Result.ToList();

            return dsChapter;
        }
    }
}
