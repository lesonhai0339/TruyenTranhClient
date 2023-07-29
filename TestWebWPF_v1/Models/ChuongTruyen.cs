using System;
using System.Collections.Generic;

namespace TestWebWPF_v1.Models
{
    public partial class ChuongTruyen
    {
        public ChuongTruyen()
        {
            ChapterImages = new HashSet<ChapterImage>();
        }

        public string ChapterId { get; set; } = null!;
        public string ChapterName { get; set; } = null!;
        public string? ChapterTitle { get; set; }
        public DateTime ChapterDate { get; set; }
        public string MangaId { get; set; } = null!;

        public virtual BoTruyen Manga { get; set; } = null!;
        public virtual ICollection<ChapterImage> ChapterImages { get; set; }
    }
}
