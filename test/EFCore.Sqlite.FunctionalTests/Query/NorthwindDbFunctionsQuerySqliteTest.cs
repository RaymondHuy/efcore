// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class NorthwindDbFunctionsQuerySqliteTest : NorthwindDbFunctionsQueryRelationalTestBase<
        NorthwindQuerySqliteFixture<NoopModelCustomizer>>
    {
        public NorthwindDbFunctionsQuerySqliteTest(
            NorthwindQuerySqliteFixture<NoopModelCustomizer> fixture,
            ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            Fixture.TestSqlLoggerFactory.Clear();
        }

        protected override string CaseInsensitiveCollation
            => "NOCASE";

        protected override string CaseSensitiveCollation
            => "BINARY";

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async Task Random_return_less_than_1(bool async)
        {
            await AssertCount(
                async,
                ss => ss.Set<Order>(),
                ss => ss.Set<Order>(),
                ss => EF.Functions.Random() < 1,
                c => true);

            AssertSql(
                @"SELECT COUNT(*)
FROM ""Orders"" AS ""o""
WHERE ef_compare(abs(random() / '9223372036854780000.0'), '1.0') < 0");
        }

        [ConditionalTheory]
        [MemberData(nameof(IsAsyncData))]
        public async Task Random_return_greater_than_0(bool async)
        {
            await AssertCount(
                async,
                ss => ss.Set<Order>(),
                ss => ss.Set<Order>(),
                ss => EF.Functions.Random() >= 0,
                c => true);

            AssertSql(
                @"SELECT COUNT(*)
FROM ""Orders"" AS ""o""
WHERE ef_compare(abs(random() / '9223372036854780000.0'), '0.0') >= 0");
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);
    }
}
