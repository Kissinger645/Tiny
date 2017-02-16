using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiny.Models
{
    public class Like
    {
        public int Id { get; set; }

        public string LikerId { get; set; } //ApplicationUserId

        [ForeignKey("LikerId")]
        public virtual ApplicationUser Liker { get; set; }

        public int LinkId { get; set; } //ApplicationUserId

        [ForeignKey("LinkId")]
        public virtual Link Link { get; set; }
    }
}