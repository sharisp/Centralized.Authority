using Identity.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.Entity
{
    public class Sys : BaseAuditableEntity, IAggregateRoot
    {
        //insert manually
        private Sys()
        {

        }
        public string SystemName { get; private set; }
        public string SystemCode { get; private set; }
    }
}