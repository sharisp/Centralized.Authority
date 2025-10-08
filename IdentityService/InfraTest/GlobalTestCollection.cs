using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace InfraTest
{
    [CollectionDefinition("Global Test Collection")]
    public class GlobalTestCollection : ICollectionFixture<IdGeneratorFixture>, ICollectionFixture<DBContextFixture>
    {
       
    }
}
