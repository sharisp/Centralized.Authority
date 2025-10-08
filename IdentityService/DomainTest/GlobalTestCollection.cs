using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainTest
{
    [CollectionDefinition("Global Test Collection")]
    public class GlobalTestCollection : ICollectionFixture<IdGeneratorFixture>
    {
        // 空即可
    }
}
