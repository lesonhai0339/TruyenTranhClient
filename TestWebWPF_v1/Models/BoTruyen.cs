using System;
using System.Collections.Generic;

namespace TestWebWPF_v1.Models
{
    public partial class BoTruyen
    {
        public BoTruyen()
        {
            ChuongTruyens = new HashSet<ChuongTruyen>();
        }

        public string MangaId { get; set; } = null!;
        public string MangaName { get; set; } = null!;
        public string? MangaDetails { get; set; }
        public string? MangaImage { get; set; }
        public string? MangaAlternateName { get; set; }
        public string MangaAuthor { get; set; } = null!;
        public string? MangaArtist { get; set; }
        public string MangaGenre { get; set; } = null!;

        public virtual ICollection<ChuongTruyen> ChuongTruyens { get; set; }
    }
}
