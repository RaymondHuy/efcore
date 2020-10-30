// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.TestModels.Northwind;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;
using Xunit.Abstractions;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class NorthwindDbFunctionsQueryCosmosTest : QueryTestBase<NorthwindQueryCosmosFixture<NoopModelCustomizer>>
    {
        public NorthwindDbFunctionsQueryCosmosTest(
            NorthwindQueryCosmosFixture<NoopModelCustomizer> fixture,
            ITestOutputHelper testOutputHelper)
            : base(fixture)
        {
            ClearLog();
        }

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
                @"SELECT COUNT(1) AS c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (RAND() < 1.0))");
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
                @"SELECT COUNT(1) AS c
FROM root c
WHERE ((c[""Discriminator""] = ""Order"") AND (RAND() >= 0.0))");
        }

        private void AssertSql(params string[] expected)
            => Fixture.TestSqlLoggerFactory.AssertBaseline(expected);

        protected void ClearLog()
            => Fixture.TestSqlLoggerFactory.Clear();
    }
}
