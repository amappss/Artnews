using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ArtNews.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ArtNewsUser class
    public class ArtNewsUser : IdentityUser
    {
        public string name { get; set; }
        public string signalId { get; set; }

    }
}
