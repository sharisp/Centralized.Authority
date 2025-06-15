using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Domain.ValueObject
{

    //用record有问题
    public record PhoneNumber
    {
        private PhoneNumber()
        {

        }
        public string CountryCode { get; private set; }
        public string Number { get; private set; }
        public PhoneNumber(string countryCode, string number)
        {
           
            CountryCode = countryCode;
            Number = number;
        }
    }
}
