using Domain.SharedKernel.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraTest
{
    public class IdGeneratorFixture
    {
        public IdGeneratorFixture()
        {

            IdGeneratorFactory.Initialize(1);

        }
    }

}
