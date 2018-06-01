using System;
using System.Collections.Generic;
using NUnit.Framework;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.Client.Caller
{
    [TestFixture]
    public class CallerRawTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void TestRawCallerApi(CallerTest test)
        {
            test.Act();
            test.Assert();
        }

        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (CallerTest testCase in CallerTests)
                {
                    TestCaseData testCaseData = new TestCaseData(testCase);
                    testCaseData.SetName(testCase.TestName);
                    yield return testCaseData;
                }
            }
        }

        public static IEnumerable<CallerTest> CallerTests
        {
            get
            {
                CallerTest ping1 = new CallerTest();
                ping1.TestName = "com.arguments.ping_1";
                ping1.SetupCall(new CallOptions(), "com.arguments.ping");
                ping1.SetupResult(new ResultDetails(), new object[] { null });
                yield return ping1;

                CallerTest add21 = new CallerTest();
                add21.TestName = "com.arguments.add2_1";
                add21.SetupCall(new CallOptions(), "com.arguments.add2", new object[] { 2, 3 });
                add21.SetupResult(new ResultDetails(), new object[] { 5 });
                yield return add21;

                CallerTest stars1 = new CallerTest();
                stars1.TestName = "com.arguments.stars_1";
                stars1.SetupCall(new CallOptions(), "com.arguments.stars");
                stars1.SetupResult(new ResultDetails(), new object[] { "somebody starred 0x" });
                yield return stars1;

                CallerTest stars2 = new CallerTest();
                stars2.TestName = "com.arguments.stars_2";
                stars2.SetupCall(new CallOptions(), "com.arguments.stars", new object[] { }, new Dictionary<string, object> {{"nick" , "Homer"} });
                stars2.SetupResult(new ResultDetails(), new object[] { "Homer starred 0x" });
                yield return stars2;

                CallerTest stars3 = new CallerTest();
                stars3.TestName = "com.arguments.stars_3";
                stars3.SetupCall(new CallOptions(), "com.arguments.stars", new object[] { }, new Dictionary<string, object> {{"stars", 5 }});
                stars3.SetupResult(new ResultDetails(), new object[] { "somebody starred 5x" });
                yield return stars3;

                CallerTest stars4 = new CallerTest();
                stars4.TestName = "com.arguments.stars_4";
                stars4.SetupCall(new CallOptions(), "com.arguments.stars", new object[] {},
                                 new Dictionary<string, object> {{"nick", "Homer"}, {"stars", 5}});
                stars4.SetupResult(new ResultDetails(), new object[] { "Homer starred 5x" });
                yield return stars4;

                CallerTest orders1 = new CallerTest();
                orders1.TestName = "com.arguments.orders_1";
                orders1.SetupCall(new CallOptions(), "com.arguments.orders", new object[] { "coffee" });
                orders1.SetupResult(new ResultDetails(), new object[] { new object[] { 0, 1, 2, 3, 4 } });
                yield return orders1;

                CallerTest orders2 = new CallerTest();
                orders2.TestName = "com.arguments.orders_2";
                orders2.SetupCall(new CallOptions(), "com.arguments.orders", new object[] { "coffee" }, 
                    new Dictionary<string, object> {{ "limit", 10 }});
                orders2.SetupResult(new ResultDetails(), new object[] { new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 } });
                yield return orders2;

                CallerTest arglen1 = new CallerTest();
                arglen1.TestName = "com.arguments.arglen_1";
                arglen1.SetupCall(new CallOptions(), "com.arguments.arglen");
                arglen1.SetupResult(new ResultDetails(), new object[] { new object[] { 0, 0 } });
                yield return arglen1;

                CallerTest arglen2 = new CallerTest();
                arglen2.TestName = "com.arguments.arglen_2";
                arglen2.SetupCall(new CallOptions(), "com.arguments.arglen", new object[] { 1, 2, 3 });
                arglen2.SetupResult(new ResultDetails(), new object[] { new object[] { 3, 0 } });
                yield return arglen2;

                CallerTest arglen3 = new CallerTest();
                arglen3.TestName = "com.arguments.arglen_3";
                arglen3.SetupCall(new CallOptions(), "com.arguments.arglen", new object[] {},
                                  new Dictionary<string, object> {{"a", 1}, {"b", 2}, {"c", 3}});
                arglen3.SetupResult(new ResultDetails(), new object[] { new object[] { 0, 3 } });
                yield return arglen3;

                CallerTest arglen4 = new CallerTest();
                arglen4.TestName = "com.arguments.arglen_4";
                arglen4.SetupCall(new CallOptions(), "com.arguments.arglen", new object[] {1, 2, 3},
                                  new Dictionary<string, object>() {{"a", 1}, {"b", 2}, {"c", 3}});
                arglen4.SetupResult(new ResultDetails(), new object[] { new object[] { 3, 3 } });
                yield return arglen4;

                CallerTest add_complex1 = new CallerTest();
                add_complex1.TestName = "com.myapp.add_complex_1";
                add_complex1.SetupCall(new CallOptions(), "com.myapp.add_complex", new object[] { 2, 3, 4, 5 });
                add_complex1.SetupResult(new ResultDetails(), new object[] {},
                                         new Dictionary<string, object>() {{"c", 6}, {"ci", 8}});
                yield return add_complex1;

                CallerTest split_name1 = new CallerTest();
                split_name1.TestName = "com.myapp.split_name_1";
                split_name1.SetupCall(new CallOptions(), "com.myapp.split_name", new object[] { "Homer Simpson" });
                split_name1.SetupResult(new ResultDetails(), new object[] { "Homer", "Simpson" });
                yield return split_name1;

                CallerTest sqrt1 = new CallerTest();
                sqrt1.TestName = "com.myapp.sqrt_1";
                sqrt1.SetupCall(new CallOptions(), "com.myapp.sqrt", new object[] { 2 });
                sqrt1.SetupResult(new ResultDetails(), new object[] { 1.4142135623731 });
                yield return sqrt1;

                CallerTest sqrt2 = new CallerTest();
                sqrt2.TestName = "com.myapp.sqrt_2";
                sqrt2.SetupCall(new CallOptions(), "com.myapp.sqrt", new object[] { 0 });
                sqrt2.SetupError(new { }, "wamp.error.runtime_error", new object[] { "don't ask folly questions;)" });
                yield return sqrt2;

                CallerTest sqrt3 = new CallerTest();
                sqrt3.TestName = "com.myapp.sqrt_3";
                sqrt3.SetupCall(new CallOptions(), "com.myapp.sqrt", new object[] { -2 });
                sqrt3.SetupError(new { }, "com.myapp.error", new object[] { "fuck" }, new { a = 23, b = 9 });
                yield return sqrt3;

                CallerTest slowsquare1 = new CallerTest();
                slowsquare1.TestName = "com.math.slowsquare_1";
                slowsquare1.SetupCall(new CallOptions(), "com.math.slowsquare", new object[] { 3 });
                slowsquare1.SetupResult(new ResultDetails(), new object[] { 9 });
                yield return slowsquare1;

                CallerTest square1 = new CallerTest();
                square1.TestName = "com.math.square_1";
                square1.SetupCall(new CallOptions(), "com.math.square", new object[] { 3 });
                square1.SetupResult(new ResultDetails(), new object[] { 9 });
                yield return square1;

                CallerTest now1 = new CallerTest();
                now1.TestName = "com.timeservice.now_1";
                now1.SetupCall(new CallOptions(), "com.timeservice.now");
                now1.SetupResult(new ResultDetails(), new object[] { new DateTime(2014, 4, 8, 19, 48, 36, 956) });
                yield return now1;
            }
        }
    }
}