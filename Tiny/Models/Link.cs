using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiny.Models
{
    public class Link
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime Created { get; set; }
        public bool Public { get; set; }
        public string UserName { get; set; }

        public string OwnerId { get; set; } //Guid

        [ForeignKey("OwnerId")]
        public virtual ApplicationUser Owner { get; set; }
    }
}