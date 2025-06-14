using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{
   public class WhiteUrl
    {
        public WhiteUrl()
        {
            
        }
        public long Id { get; set; }

        public  string Url { get; set; }
        public bool IsDel { get; set; }
        public string? Descriptions { get; set; }

        public DateTime? CreateDateTime { get; set; }

        public DateTime? DeleteDateTime { get; set; }
    }
}
