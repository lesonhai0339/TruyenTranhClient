
namespace TestWebWPF_v1.Models.View
{
    public class chapterView
    {
        public string Chapter_Id { get; set; } = null!;
        public string Chapter_Name { get; set; } = null!;
        public string? Chapter_Title { get; set; }
        public List<string> Imagechapter {get;set;}=null!;
        public string Chapter_Date { get; set; }=null!;
        public string Manga_Id { get; set; } = null!;
    }
}
