using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Tiny.Models
{
    public class Click
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int Clicks { get; set; }
        public int LinkId { get; set; }

        [ForeignKey("LinkId")]
        public virtual Link Link { get; set; }
    }
}