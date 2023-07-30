using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using TestWebWPF_v1.Models;

namespace TestWebWPF_v1.Controllers
{
    public class MangaController : Controller
    {
        private static HttpClient hc = new HttpClient();
        private static string strUrl = @"https://localhost:7132/Truyen-tranh";
        private List<BoTruyen> _dsBotruyen;
        private List<string> _dsChuongtruyen;
        public MangaController()
        {
            _dsBotruyen = getDSBoTruyen();
            _dsChuongtruyen = getDsChapter();
        }
        // Lấy danh sách truyện
        public IActionResult Index()
        {
            return View(_dsBotruyen);
        }

        /*--------------------------------------------------------Begin Action Manga-------------------------------------------*/

        //Form tạo bộ truyện
        [Route("CreateManga")]
        public IActionResult formThemManga()
        {
            return View();
        }
        //Action tạo bộ truyện
        [Route("ActionCreateManga")]
        public IActionResult CreateManga(string MangaName,string MangaDetails,IFormFile? MangaImage,string MangaAlternateName
            ,string MangaAuthor,string MangaArtist,string MangaGenre)
        {
            try
            {
                List<string> ds = _dsBotruyen.Select(x => x.MangaId).ToList();// phải lấy tất cả bộ truyện sau đó mới dùng được
                string Id = ranDomId(ds);

                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(Id), "MangaId");
                formData.Add(new StringContent(MangaName), "MangaName");
                formData.Add(new StringContent(MangaDetails), "MangaDetails");
                if(MangaImage!=null) formData.Add(new StreamContent(MangaImage.OpenReadStream()), "MangaImage", MangaImage.FileName);
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

        //Form sửa thông tin bộ truyện
        [Route("EditManga-{Id}")]
        public IActionResult formSuaTruyen(string Id)
        {
            BoTruyen? a = _dsBotruyen.FirstOrDefault(x => x.MangaId == Id);
            return View(a);
        }
        //Hàm sửa thông tin của bộ truyện
        [Route("ActionEditManga")]
        public IActionResult SuaTruyen(string MangaId, string? MangaName, string? MangaDetails, IFormFile? MangaImage, string? MangaAlternateName
            , string? MangaAuthor, string? MangaArtist, string? MangaGenre)
        {
            try
            {
                var formData = new MultipartFormDataContent();
                formData.Add(new StringContent(MangaId), "MangaId");
                if (MangaName != null) formData.Add(new StringContent(MangaName), "MangaName");
                if (MangaDetails != null) formData.Add(new StringContent(MangaDetails), "MangaDetails");
                if (MangaImage != null) formData.Add(new StreamContent(MangaImage.OpenReadStream()), "MangaImage", MangaImage.FileName);
                if (MangaAlternateName != null) formData.Add(new StringContent(MangaAlternateName), "MangaAlternateName");
                if (MangaAuthor != null) formData.Add(new StringContent(MangaAuthor), "MangaAuthor");
                if (MangaArtist != null) formData.Add(new StringContent(MangaArtist), "MangaArtist");
                if (MangaGenre != null) formData.Add(new StringContent(MangaGenre), "MangaGenre");
                string url = strUrl + @"/EditManga/" + MangaId;
                var response = hc.PutAsync(url, formData);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return ViewBag.request("Không thể thêm truyện");
            }
            return ViewBag.request("Không thể thêm truyện");
        }

        [HttpPost]
        [Route("DeleteManga-{MangaId}")]
        public IActionResult xoaTruyen(string MangaId)
        {
            try
            {
                string url = strUrl + @"/Delete/" + MangaId;
                var response = hc.DeleteAsync(url);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                    return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return ViewBag.request("Không thể thêm truyện");
            }
            return ViewBag.request("Không thể thêm truyện");
        }
        /*-------------------------------------------------------End Action Manga------------------------------------------------*/
        /*--------------------------------------------------------Begin Action Chapter-------------------------------------------*/

        //Form tạo chương truyện
        [Route("{Id}/CreateChapter")]
        public IActionResult formcreateChapter(string Id)
        {
            ViewBag.MangaId = Id;
            return View();
        }

        //Action tạo chương truyện
        [Route("ActionCreateChapter")]
        public IActionResult CreateChapter(
            string MangaId,string TieuDe,string tenChuong, List<IFormFile> file,List<string > dsurl)
        {
            try { 
                List<string> ds = _dsChuongtruyen; //phải tạo hàm để lấy tất cả chương truyện sao đó mới dùng được
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

        //Form lấy danh sách chương truyện của bộ truyện
        [Route("{Id}/Chapter")]
        public IActionResult listChapter(string Id)
        {
            try
            {
                var conn = hc.GetAsync(strUrl + @"/" + Id);
                conn.Wait();
                if (conn.Result.IsSuccessStatusCode == false)
                    return ViewBag.request("Không thể kết nối tới máy chủ");
                var kq = conn.Result.Content.ReadAsAsync<List<ChuongTruyen>>();
                kq.Wait();
                return View(kq.Result.ToList());
            }
            catch (Exception)
            {
                return ViewBag.request("Không thể tải Danh sách truyện.");
            }
        }

        //Form lấy danh sách ảnh của chương truyện
        [Route("{idManga}/{idChapter}")]
        public IActionResult layAnhChuong(string idChapter, string idManga)
        {
            try
            {
                var conn = hc.GetAsync(strUrl + @"/" + idManga+@"/"+idChapter+ @"/getDsImage");
                conn.Wait();
                if (conn.Result.IsSuccessStatusCode == false)
                    return ViewBag.request("Không thể kết nối tới máy chủ");
                var kq = conn.Result.Content.ReadAsAsync<List<string>>();
                kq.Wait();
                var data = kq.Result.ToList();
                data.Sort();
                ViewBag.DsImage=data;
                return View();
            }
            catch (Exception)
            {
                return ViewBag.request("Không thể tải Danh sách truyện.");
            }
        }

        /*--------------------------------------------------------End Action Chapter-------------------------------------------*/
        /*--------------------------------------------------------Hàm-------------------------------------------*/

        //Hàm random mã số để tạo mangaId và chapterId
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

        //Hàm lấy danh sách chương truyện
        private List<string> getDsChapter()
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

        //Hàm lấy danh sách bộ truyện
        public List<BoTruyen> getDSBoTruyen()
        {
            try
            {
                var conn = hc.GetAsync(strUrl + @"/GetAllManga");
                conn.Wait();
                if (conn.Result.IsSuccessStatusCode == false)
                    return ViewBag.request("Không thể kết nối tới máy chủ");
                var kq = conn.Result.Content.ReadAsAsync<List<BoTruyen>>();
                kq.Wait();
                var dstruyen = kq.Result.ToList();
                return dstruyen;
            }
            catch (Exception)
            {
                return ViewBag.request("Không thể tải Danh sách truyện.");
            }  
        }
    }
}
