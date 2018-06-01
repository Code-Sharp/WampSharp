using System;
using System.Collections.Generic;
using NUnit.Framework;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.Tests.Wampv2.Client.Callee
{
    public class RawInvocationCalleeTests
    {
        [TestCaseSource(nameof(TestCases))]
        public void TestRawCallerApi(InvocationCalleeeTest test)
        {
            test.Act();
            test.Assert();
        }

        public static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                foreach (InvocationCalleeeTest testCase in Tests)
                {
                    TestCaseData testCaseData = new TestCaseData(testCase);
                    testCaseData.SetName(testCase.TestName);
                    yield return testCaseData;
                }
            }
        }

       
 
        private static IEnumerable<InvocationCalleeeTest> Tests
        {
            get
            {
                InvocationCalleeeTest ping1 = new InvocationCalleeeTest(6260583705484117);
                ping1.TestName = "com.arguments.ping_1";
                ping1.SetupInvocation(2147782813617642, 6260583705484117, new InvocationDetails());
                ping1.SetupYield(2147782813617642, new YieldOptions(), new object[] { null });
                yield return ping1;

                InvocationCalleeeTest add21 = new InvocationCalleeeTest(5520574658558943);
                add21.TestName = "com.arguments.add2_1";
                add21.SetupInvocation(5548533415751543, 5520574658558943, new InvocationDetails(), new object[] { 2, 3 });
                add21.SetupYield(5548533415751543, new YieldOptions(), new object[] { 5 });
                yield return add21;

                InvocationCalleeeTest stars1 = new InvocationCalleeeTest(8518149122274282);
                stars1.TestName = "com.arguments.stars_1";
                stars1.SetupInvocation(4736136853294707, 8518149122274282, new InvocationDetails());
                stars1.SetupYield(4736136853294707, new YieldOptions(), new object[] { "somebody starred 0x" });
                yield return stars1;

                InvocationCalleeeTest stars2 = new InvocationCalleeeTest(8518149122274282);
                stars2.TestName = "com.arguments.stars_2";
                stars2.SetupInvocation(4623401476744760, 8518149122274282, new InvocationDetails(), new object[] { }, new Dictionary<string, object>(){{ "nick" , "Homer" }});
                stars2.SetupYield(4623401476744760, new YieldOptions(), new object[] { "Homer starred 0x" });
                yield return stars2;

                InvocationCalleeeTest stars3 = new InvocationCalleeeTest(8518149122274282);
                stars3.TestName = "com.arguments.stars_3";
                stars3.SetupInvocation(106285795952377, 8518149122274282, new InvocationDetails(), new object[] {},
                                       new Dictionary<string, object>() {{"stars", 5}});
                stars3.SetupYield(106285795952377, new YieldOptions(), new object[] { "somebody starred 5x" });
                yield return stars3;

                InvocationCalleeeTest stars4 = new InvocationCalleeeTest(8518149122274282);
                stars4.TestName = "com.arguments.stars_4";
                stars4.SetupInvocation(2431905053280648, 8518149122274282, new InvocationDetails(), new object[] {},
                                       new Dictionary<string, object>()
                                           {
                                               {"nick", "Homer"},
                                               {"stars", 5}
                                           });
                stars4.SetupYield(2431905053280648, new YieldOptions(), new object[] { "Homer starred 5x" });
                yield return stars4;

                InvocationCalleeeTest orders1 = new InvocationCalleeeTest(8563643197559978);
                orders1.TestName = "com.arguments.orders_1";
                orders1.SetupInvocation(5624183520390514, 8563643197559978, new InvocationDetails(), new object[] { "coffee" });
                orders1.SetupYield(5624183520390514, new YieldOptions(), new object[] { new object[] { 0, 1, 2, 3, 4 } });
                yield return orders1;

                InvocationCalleeeTest orders2 = new InvocationCalleeeTest(8563643197559978);
                orders2.TestName = "com.arguments.orders_2";
                orders2.SetupInvocation(5618202403158810, 8563643197559978, new InvocationDetails(), new object[] { "coffee" }, new Dictionary<string, object>() { { "limit", 10 } });
                orders2.SetupYield(5618202403158810, new YieldOptions(), new object[] { new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 } });
                yield return orders2;

                InvocationCalleeeTest arglen1 = new InvocationCalleeeTest(7806950026192770);
                arglen1.TestName = "com.arguments.arglen_1";
                arglen1.SetupInvocation(3263204727653488, 7806950026192770, new InvocationDetails());
                arglen1.SetupYield(3263204727653488, new YieldOptions(), new object[] { new object[] { 0, 0 } });
                yield return arglen1;

                InvocationCalleeeTest arglen2 = new InvocationCalleeeTest(7806950026192770);
                arglen2.TestName = "com.arguments.arglen_2";
                arglen2.SetupInvocation(2597754391621670, 7806950026192770, new InvocationDetails(), new object[] { 1, 2, 3 });
                arglen2.SetupYield(2597754391621670, new YieldOptions(), new object[] { new object[] { 3, 0 } });
                yield return arglen2;

                InvocationCalleeeTest arglen3 = new InvocationCalleeeTest(7806950026192770);
                arglen3.TestName = "com.arguments.arglen_3";
                arglen3.SetupInvocation(6783179721480343, 7806950026192770, new InvocationDetails(), new object[] { },
                    new Dictionary<string, object>() { { "a", 1 }, { "c", 3 }, { "b", 2 } });
                arglen3.SetupYield(6783179721480343, new YieldOptions(), new object[] { new object[] { 0, 3 } });
                yield return arglen3;

                InvocationCalleeeTest arglen4 = new InvocationCalleeeTest(7806950026192770);
                arglen4.TestName = "com.arguments.arglen_4";
                arglen4.SetupInvocation(400135746467369, 7806950026192770, new InvocationDetails(),
                                        new object[] {1, 2, 3},
                                        new Dictionary<string, object>() {{"a", 1}, {"c", 3}, {"b", 2}});

                arglen4.SetupYield(400135746467369, new YieldOptions(), new object[] { new object[] { 3, 3 } });
                yield return arglen4;

                InvocationCalleeeTest add_complex1 = new InvocationCalleeeTest(2465104766220729);
                add_complex1.TestName = "com.myapp.add_complex_1";
                add_complex1.SetupInvocation(6779620896146572, 2465104766220729, new InvocationDetails(), new object[] { 2, 3, 4, 5 });
                add_complex1.SetupYield(6779620896146572, new YieldOptions(), new object[] {},
                                        new Dictionary<string, object>() {{"c", 6}, {"ci", 8}});
                yield return add_complex1;

                InvocationCalleeeTest split_name1 = new InvocationCalleeeTest(1469492476356560);
                split_name1.TestName = "com.myapp.split_name_1";
                split_name1.SetupInvocation(4575738492841349, 1469492476356560, new InvocationDetails(), new object[] { "Homer Simpson" });
                split_name1.SetupYield(4575738492841349, new YieldOptions(), new object[] { "Homer", "Simpson" });
                yield return split_name1;

                InvocationCalleeeTest sqrt1 = new InvocationCalleeeTest(5546152155562918);
                sqrt1.TestName = "com.myapp.sqrt_1";
                sqrt1.SetupInvocation(3232475339417480, 5546152155562918, new InvocationDetails(), new object[] { 2 });
                sqrt1.SetupYield(3232475339417480, new YieldOptions(), new object[] { 1.4142135623731 });
                yield return sqrt1;

                InvocationCalleeeTest sqrt2 = new InvocationCalleeeTest(5546152155562918);
                sqrt2.TestName = "com.myapp.sqrt_2";
                sqrt2.SetupInvocation(7260234764466627, 5546152155562918, new InvocationDetails(), new object[] { 0 });
                sqrt2.SetupError(68, 7260234764466627, new { }, "wamp.error.runtime_error", new object[] { "don't ask folly questions;)" });
                yield return sqrt2;

                InvocationCalleeeTest sqrt3 = new InvocationCalleeeTest(5546152155562918);
                sqrt3.TestName = "com.myapp.sqrt_3";
                sqrt3.SetupInvocation(2198486081163059, 5546152155562918, new InvocationDetails(), new object[] { -2 });
                sqrt3.SetupError(68, 2198486081163059, new { }, "com.myapp.error", new object[] { "fuck" }, new { a = 23, b = 9 });
                yield return sqrt3;

                InvocationCalleeeTest slowsquare1 = new InvocationCalleeeTest(2977303793738678);
                slowsquare1.TestName = "com.math.slowsquare_1";
                slowsquare1.SetupInvocation(455899852197574, 2977303793738678, new InvocationDetails(), new object[] { 3 });
                slowsquare1.SetupYield(455899852197574, new YieldOptions(), new object[] { 9 });
                yield return slowsquare1;

                InvocationCalleeeTest square1 = new InvocationCalleeeTest(5383295424135943);
                square1.TestName = "com.math.square_1";
                square1.SetupInvocation(2842632479931560, 5383295424135943, new InvocationDetails(), new object[] { 3 });
                square1.SetupYield(2842632479931560, new YieldOptions(), new object[] { 9 });
                yield return square1;

                InvocationCalleeeTest now1 = new InvocationCalleeeTest(6358525630838455);
                now1.TestName = "com.timeservice.now_1";
                now1.SetupInvocation(3129619970764329, 6358525630838455, new InvocationDetails());
                now1.SetupYield(3129619970764329, new YieldOptions(), new object[] { new DateTime(2014, 4, 8, 19, 48, 36, 956) });
                yield return now1;

            }
        }
    }
}