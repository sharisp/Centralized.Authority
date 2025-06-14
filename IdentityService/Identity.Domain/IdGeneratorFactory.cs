using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using IdGen;

namespace Identity.Domain
{
    public static class IdGeneratorFactory
    {
        private static readonly IdGenerator _generator = new IdGenerator(0); // 0 是 generatorId（建议每台机器不一样）

        public static long NewId() => _generator.CreateId();
    }
}
