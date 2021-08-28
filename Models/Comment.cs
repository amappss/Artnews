using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ArtNews.Models
{
    public class Comment
    {
        public int id { get; set; }
        public string writerName { get; set; }
        public string text { get; set; }
        [DefaultValue(0)]
        public int rating { get; set; }
        public ArtPiece artPiece { get; set; }
        [DefaultValue(0)]
        public int approved { get; set; }
    }
}
