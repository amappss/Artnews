using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ArtNews.Models
{
    public class ArtPiece
    {
        public int id { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public byte[] img { get; set; }
        public string author { get; set; }
        public string size { get; set; }
        public string introduction { get; set; }
        public ICollection<Comment> comments { get; set; }
        public double rating { get; set; }
    }
}
