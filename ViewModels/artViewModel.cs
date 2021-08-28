
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtNews.ViewModels
{
    public class artViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int year { get; set; }
        public IFormFile img { get; set; }
        public string author { get; set; }
        public string size { get; set; }
        public string introduction { get; set; }
        public double rating { get; set; }

    }
}
