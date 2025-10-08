using Domain.SharedKernel.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainTest
{
    public class IdGeneratorFixture
    {
        public IdGeneratorFixture()
        {
          
            IdGeneratorFactory.Initialize(1);
        }
    }

}
