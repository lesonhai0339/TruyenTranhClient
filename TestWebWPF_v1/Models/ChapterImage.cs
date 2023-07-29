using System;
using System.Collections.Generic;

namespace TestWebWPF_v1.Models
{
    public partial class ChapterImage
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; } = null!;
        public string? ImageUl { get; set; }
        public string ChapterId { get; set; } = null!;

        public virtual ChuongTruyen Chapter { get; set; } = null!;
    }
}
