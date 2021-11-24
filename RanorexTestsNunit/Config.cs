using NUnit.Framework;
using Ranorex;
using System.Runtime.CompilerServices;

namespace RanorexTestsReworked
{
    [SetUpFixture]
    public class Config
    {
        [SetUp]
        public void SetUp()
        {
            InitResolver();
            RanorexInit();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitResolver()
        {
            Ranorex.Core.Resolver.AssemblyLoader.Initialize();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void RanorexInit()
        {
            TestingBootstrapper.SetupCore();
        }

    }
}
