using System;

namespace Sample.RepositoryTests.Misc
{
    public class TestDbConnection
    {
        public class LocalDb
        {
            public static string Master =>
                    @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True";

            public const string Database =
                    @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog={0};Integrated Security=True;MultipleActiveResultSets=True";
        }

        public class Container
        {
            public const string Master =
                    @"Data Source={0};Initial Catalog=master;Persist Security Info=True;User ID=sa;Password=1q2w3e4r5t_;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;TrustServerCertificate=True";

            public const string Database =
                    @"Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID=sa;Password=1q2w3e4r5t_;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;TrustServerCertificate=True";
        }
    }
}