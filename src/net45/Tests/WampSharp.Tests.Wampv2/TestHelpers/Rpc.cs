using System;
using System.Collections.Generic;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.TestHelpers
{
    public class Rpc
    {
        public class Arguments
        {
            public class CalleeToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                private static readonly WampMessage<MockRaw> mRequest5;
                private static readonly WampMessage<MockRaw> mRequest6;
                private static readonly WampMessage<MockRaw> mRequest7;
                private static readonly WampMessage<MockRaw> mRequest8;
                private static readonly WampMessage<MockRaw> mRequest9;
                private static readonly WampMessage<MockRaw> mRequest10;
                private static readonly WampMessage<MockRaw> mRequest11;
                private static readonly WampMessage<MockRaw> mRequest12;
                private static readonly WampMessage<MockRaw> mRequest13;
                private static readonly WampMessage<MockRaw> mRequest14;
                private static readonly WampMessage<MockRaw> mRequest15;
                private static readonly WampMessage<MockRaw> mRequest16;
                private static readonly WampMessage<MockRaw> mRequest17;
                static CalleeToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(6811926707306496);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.ping");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5042478608547840);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.add2");
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(6308400455483392);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.stars");
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(8035274248421376);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.orders");
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3179382642311168);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.arglen");
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2147782813617642);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
null,
});
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5548533415751543);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
5,
});
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4736136853294707);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"somebody starred 0x",
});
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4623401476744760);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"Homer starred 0x",
});
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(106285795952377);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"somebody starred 5x",
});
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2431905053280648);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"Homer starred 5x",
});
                        mRequest11.Arguments = arguments;
                    }
                    mRequest12 = new WampMessage<MockRaw>();
                    {
                        mRequest12.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5624183520390514);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
0,
1,
2,
3,
4,
},
});
                        mRequest12.Arguments = arguments;
                    }
                    mRequest13 = new WampMessage<MockRaw>();
                    {
                        mRequest13.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5618202403158810);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
0,
1,
2,
3,
4,
5,
6,
7,
8,
9,
},
});
                        mRequest13.Arguments = arguments;
                    }
                    mRequest14 = new WampMessage<MockRaw>();
                    {
                        mRequest14.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3263204727653488);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
0,
0,
},
});
                        mRequest14.Arguments = arguments;
                    }
                    mRequest15 = new WampMessage<MockRaw>();
                    {
                        mRequest15.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2597754391621670);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
3,
0,
},
});
                        mRequest15.Arguments = arguments;
                    }
                    mRequest16 = new WampMessage<MockRaw>();
                    {
                        mRequest16.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(6783179721480343);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
0,
3,
},
});
                        mRequest16.Arguments = arguments;
                    }
                    mRequest17 = new WampMessage<MockRaw>();
                    {
                        mRequest17.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(400135746467369);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
3,
3,
},
});
                        mRequest17.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                        yield return mRequest5;
                        yield return mRequest6;
                        yield return mRequest7;
                        yield return mRequest8;
                        yield return mRequest9;
                        yield return mRequest10;
                        yield return mRequest11;
                        yield return mRequest12;
                        yield return mRequest13;
                        yield return mRequest14;
                        yield return mRequest15;
                        yield return mRequest16;
                        yield return mRequest17;
                    }
                }
            }

            public class DealerToCallee
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                private static readonly WampMessage<MockRaw> mRequest5;
                private static readonly WampMessage<MockRaw> mRequest6;
                private static readonly WampMessage<MockRaw> mRequest7;
                private static readonly WampMessage<MockRaw> mRequest8;
                private static readonly WampMessage<MockRaw> mRequest9;
                private static readonly WampMessage<MockRaw> mRequest10;
                private static readonly WampMessage<MockRaw> mRequest11;
                private static readonly WampMessage<MockRaw> mRequest12;
                private static readonly WampMessage<MockRaw> mRequest13;
                private static readonly WampMessage<MockRaw> mRequest14;
                private static readonly WampMessage<MockRaw> mRequest15;
                private static readonly WampMessage<MockRaw> mRequest16;
                private static readonly WampMessage<MockRaw> mRequest17;
                static DealerToCallee()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(3747178033391796);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(6811926707306496);
                        arguments[1] = new MockRaw(6260583705484117);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(5042478608547840);
                        arguments[1] = new MockRaw(5520574658558943);
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(6308400455483392);
                        arguments[1] = new MockRaw(8518149122274282);
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8035274248421376);
                        arguments[1] = new MockRaw(8563643197559978);
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(3179382642311168);
                        arguments[1] = new MockRaw(7806950026192770);
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2147782813617642);
                        arguments[1] = new MockRaw(6260583705484117);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5548533415751543);
                        arguments[1] = new MockRaw(5520574658558943);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
2,
3,
});
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4736136853294707);
                        arguments[1] = new MockRaw(8518149122274282);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(4623401476744760);
                        arguments[1] = new MockRaw(8518149122274282);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
});
                        arguments[4] = new MockRaw(new
                        {
                            nick = "Homer",
                        });
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(106285795952377);
                        arguments[1] = new MockRaw(8518149122274282);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
});
                        arguments[4] = new MockRaw(new
                        {
                            stars = 5,
                        });
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(2431905053280648);
                        arguments[1] = new MockRaw(8518149122274282);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
});
                        arguments[4] = new MockRaw(new
                        {
                            nick = "Homer",
                            stars = 5,
                        });
                        mRequest11.Arguments = arguments;
                    }
                    mRequest12 = new WampMessage<MockRaw>();
                    {
                        mRequest12.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5624183520390514);
                        arguments[1] = new MockRaw(8563643197559978);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
"coffee",
});
                        mRequest12.Arguments = arguments;
                    }
                    mRequest13 = new WampMessage<MockRaw>();
                    {
                        mRequest13.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(5618202403158810);
                        arguments[1] = new MockRaw(8563643197559978);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
"coffee",
});
                        arguments[4] = new MockRaw(new
                        {
                            limit = 10,
                        });
                        mRequest13.Arguments = arguments;
                    }
                    mRequest14 = new WampMessage<MockRaw>();
                    {
                        mRequest14.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3263204727653488);
                        arguments[1] = new MockRaw(7806950026192770);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        mRequest14.Arguments = arguments;
                    }
                    mRequest15 = new WampMessage<MockRaw>();
                    {
                        mRequest15.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2597754391621670);
                        arguments[1] = new MockRaw(7806950026192770);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
1,
2,
3,
});
                        mRequest15.Arguments = arguments;
                    }
                    mRequest16 = new WampMessage<MockRaw>();
                    {
                        mRequest16.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(6783179721480343);
                        arguments[1] = new MockRaw(7806950026192770);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
});
                        arguments[4] = new MockRaw(new
                        {
                            a = 1,
                            c = 3,
                            b = 2,
                        });
                        mRequest16.Arguments = arguments;
                    }
                    mRequest17 = new WampMessage<MockRaw>();
                    {
                        mRequest17.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(400135746467369);
                        arguments[1] = new MockRaw(7806950026192770);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
1,
2,
3,
});
                        arguments[4] = new MockRaw(new
                        {
                            a = 1,
                            c = 3,
                            b = 2,
                        });
                        mRequest17.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                        yield return mRequest5;
                        yield return mRequest6;
                        yield return mRequest7;
                        yield return mRequest8;
                        yield return mRequest9;
                        yield return mRequest10;
                        yield return mRequest11;
                        yield return mRequest12;
                        yield return mRequest13;
                        yield return mRequest14;
                        yield return mRequest15;
                        yield return mRequest16;
                        yield return mRequest17;
                    }
                }
            }

            public class CallerToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                private static readonly WampMessage<MockRaw> mRequest5;
                private static readonly WampMessage<MockRaw> mRequest6;
                private static readonly WampMessage<MockRaw> mRequest7;
                private static readonly WampMessage<MockRaw> mRequest8;
                private static readonly WampMessage<MockRaw> mRequest9;
                private static readonly WampMessage<MockRaw> mRequest10;
                private static readonly WampMessage<MockRaw> mRequest11;
                private static readonly WampMessage<MockRaw> mRequest12;
                private static readonly WampMessage<MockRaw> mRequest13;
                static CallerToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(1211770158972928);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.ping");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2185481657778176);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.add2");
                        arguments[3] = new MockRaw(new object[] {
2,
3,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5544759154180096);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.stars");
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(5929743559950336);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.stars");
                        arguments[3] = new MockRaw(new object[] {
});
                        arguments[4] = new MockRaw(new
                        {
                            nick = "Homer",
                        });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(6736788634730496);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.stars");
                        arguments[3] = new MockRaw(new object[] {
});
                        arguments[4] = new MockRaw(new
                        {
                            stars = 5,
                        });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(121752562696192);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.stars");
                        arguments[3] = new MockRaw(new object[] {
});
                        arguments[4] = new MockRaw(new
                        {
                            nick = "Homer",
                            stars = 5,
                        });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(8715888870031360);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.orders");
                        arguments[3] = new MockRaw(new object[] {
"coffee",
});
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(8081201220812800);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.orders");
                        arguments[3] = new MockRaw(new object[] {
"coffee",
});
                        arguments[4] = new MockRaw(new
                        {
                            limit = 10,
                        });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4996174345928704);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.arglen");
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(6989784142577664);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.arglen");
                        arguments[3] = new MockRaw(new object[] {
1,
2,
3,
});
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(2696092684648448);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.arglen");
                        arguments[3] = new MockRaw(new object[] {
});
                        arguments[4] = new MockRaw(new
                        {
                            a = 1,
                            b = 2,
                            c = 3,
                        });
                        mRequest11.Arguments = arguments;
                    }
                    mRequest12 = new WampMessage<MockRaw>();
                    {
                        mRequest12.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(5630114672934912);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.arguments.arglen");
                        arguments[3] = new MockRaw(new object[] {
1,
2,
3,
});
                        arguments[4] = new MockRaw(new
                        {
                            a = 1,
                            b = 2,
                            c = 3,
                        });
                        mRequest12.Arguments = arguments;
                    }
                    mRequest13 = new WampMessage<MockRaw>();
                    {
                        mRequest13.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.close.normal");
                        mRequest13.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                        yield return mRequest5;
                        yield return mRequest6;
                        yield return mRequest7;
                        yield return mRequest8;
                        yield return mRequest9;
                        yield return mRequest10;
                        yield return mRequest11;
                        yield return mRequest12;
                        yield return mRequest13;
                    }
                }
            }

            public class DealerToCaller
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                private static readonly WampMessage<MockRaw> mRequest5;
                private static readonly WampMessage<MockRaw> mRequest6;
                private static readonly WampMessage<MockRaw> mRequest7;
                private static readonly WampMessage<MockRaw> mRequest8;
                private static readonly WampMessage<MockRaw> mRequest9;
                private static readonly WampMessage<MockRaw> mRequest10;
                private static readonly WampMessage<MockRaw> mRequest11;
                private static readonly WampMessage<MockRaw> mRequest12;
                private static readonly WampMessage<MockRaw> mRequest13;
                static DealerToCaller()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(7944885279454537);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(1211770158972928);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
null,
});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2185481657778176);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
5,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5544759154180096);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"somebody starred 0x",
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5929743559950336);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"Homer starred 0x",
});
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(6736788634730496);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"somebody starred 5x",
});
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(121752562696192);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"Homer starred 5x",
});
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(8715888870031360);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
0,
1,
2,
3,
4,
},
});
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(8081201220812800);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
0,
1,
2,
3,
4,
5,
6,
7,
8,
9,
},
});
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4996174345928704);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
0,
0,
},
});
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(6989784142577664);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
3,
0,
},
});
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2696092684648448);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
0,
3,
},
});
                        mRequest11.Arguments = arguments;
                    }
                    mRequest12 = new WampMessage<MockRaw>();
                    {
                        mRequest12.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5630114672934912);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new object[] {
3,
3,
},
});
                        mRequest12.Arguments = arguments;
                    }
                    mRequest13 = new WampMessage<MockRaw>();
                    {
                        mRequest13.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest13.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                        yield return mRequest5;
                        yield return mRequest6;
                        yield return mRequest7;
                        yield return mRequest8;
                        yield return mRequest9;
                        yield return mRequest10;
                        yield return mRequest11;
                        yield return mRequest12;
                        yield return mRequest13;
                    }
                }
            }

        }

        public class Complex
        {
            public class CalleeToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static CalleeToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(187466201956352);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.add_complex");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(1108809846095872);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.split_name");
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(6779620896146572);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
});
                        arguments[3] = new MockRaw(new
                        {
                            c = 6,
                            ci = 8,
                        });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4575738492841349);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"Homer",
"Simpson",
});
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class DealerToCallee
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static DealerToCallee()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(1426765236471398);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(187466201956352);
                        arguments[1] = new MockRaw(2465104766220729);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(1108809846095872);
                        arguments[1] = new MockRaw(1469492476356560);
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(6779620896146572);
                        arguments[1] = new MockRaw(2465104766220729);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
2,
3,
4,
5,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4575738492841349);
                        arguments[1] = new MockRaw(1469492476356560);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
"Homer Simpson",
});
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class CallerToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                static CallerToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(577392449945600);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.add_complex");
                        arguments[3] = new MockRaw(new object[] {
2,
3,
4,
5,
});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4165958868402176);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.split_name");
                        arguments[3] = new MockRaw(new object[] {
"Homer Simpson",
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.close.normal");
                        mRequest3.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                    }
                }
            }

            public class DealerToCaller
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                static DealerToCaller()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(1401414657361369);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(577392449945600);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
});
                        arguments[3] = new MockRaw(new
                        {
                            c = 6,
                            ci = 8,
                        });
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4165958868402176);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
"Homer",
"Simpson",
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest3.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                    }
                }
            }

        }

        public class Errors
        {
            public class CalleeToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static CalleeToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(7956011021238272);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.sqrt");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3232475339417480);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
1.4142135623731,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Error;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(68);
                        arguments[1] = new MockRaw(7260234764466627);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw("wamp.error.runtime_error");
                        arguments[4] = new MockRaw(new object[] {
"don't ask folly questions;)",
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Error;
                        MockRaw[] arguments = new MockRaw[6];
                        arguments[0] = new MockRaw(68);
                        arguments[1] = new MockRaw(2198486081163059);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw("com.myapp.error");
                        arguments[4] = new MockRaw(new object[] {
"fuck",
});
                        arguments[5] = new MockRaw(new
                        {
                            a = 23,
                            b = 9,
                        });
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class DealerToCallee
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static DealerToCallee()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(4436155033947731);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(7956011021238272);
                        arguments[1] = new MockRaw(5546152155562918);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3232475339417480);
                        arguments[1] = new MockRaw(5546152155562918);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
2,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7260234764466627);
                        arguments[1] = new MockRaw(5546152155562918);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
0,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2198486081163059);
                        arguments[1] = new MockRaw(5546152155562918);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
-2,
});
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class CallerToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static CallerToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(8647933712924672);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.sqrt");
                        arguments[3] = new MockRaw(new object[] {
2,
});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(8012652739559424);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.sqrt");
                        arguments[3] = new MockRaw(new object[] {
0,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3778261177860096);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.sqrt");
                        arguments[3] = new MockRaw(new object[] {
-2,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.close.normal");
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class DealerToCaller
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static DealerToCaller()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(3585595364226389);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(8647933712924672);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
1.4142135623731,
});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Error;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(48);
                        arguments[1] = new MockRaw(8012652739559424);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw("wamp.error.runtime_error");
                        arguments[4] = new MockRaw(new object[] {
"don't ask folly questions;)",
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Error;
                        MockRaw[] arguments = new MockRaw[6];
                        arguments[0] = new MockRaw(48);
                        arguments[1] = new MockRaw(3778261177860096);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw("com.myapp.error");
                        arguments[4] = new MockRaw(new object[] {
"fuck",
});
                        arguments[5] = new MockRaw(new
                        {
                            a = 23,
                            b = 9,
                        });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

        }

        public class Options
        {
            public class CalleeToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                private static readonly WampMessage<MockRaw> mRequest5;
                private static readonly WampMessage<MockRaw> mRequest6;
                static CalleeToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3474270548131840);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.square");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(1902491289518080);
                        arguments[1] = new MockRaw(new
                        {
                            exclude = new object[] {
7360133092761726,
},
                        });
                        arguments[2] = new MockRaw("com.myapp.square_on_nonpositive");
                        arguments[3] = new MockRaw(new object[] {
0,
});
                        arguments[4] = new MockRaw(new
                        {
                        });
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5290154826661888);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.square_on_nonpositive");
                        arguments[3] = new MockRaw(new object[] {
-2,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3918772563835809);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
4,
});
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5661071145887550);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
0,
});
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(1604780475002212);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
4,
});
                        mRequest6.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                        yield return mRequest5;
                        yield return mRequest6;
                    }
                }
            }

            public class DealerToCallee
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static DealerToCallee()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8011573850396222);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(3474270548131840);
                        arguments[1] = new MockRaw(6690874359198988);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3918772563835809);
                        arguments[1] = new MockRaw(6690874359198988);
                        arguments[2] = new MockRaw(new
                        {
                            caller = 7360133092761726,
                        });
                        arguments[3] = new MockRaw(new object[] {
2,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5661071145887550);
                        arguments[1] = new MockRaw(6690874359198988);
                        arguments[2] = new MockRaw(new
                        {
                            caller = 7360133092761726,
                        });
                        arguments[3] = new MockRaw(new object[] {
0,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1604780475002212);
                        arguments[1] = new MockRaw(6690874359198988);
                        arguments[2] = new MockRaw(new
                        {
                            caller = 7360133092761726,
                        });
                        arguments[3] = new MockRaw(new object[] {
-2,
});
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class CallerToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static CallerToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Subscribe;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5232241456185344);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.square_on_nonpositive");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Call;
                        //MockRaw[] arguments = new MockRaw[5];
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7234508589891584);
                        arguments[1] = new MockRaw(new
                        {
                            disclose_me = true,
                        });
                        arguments[2] = new MockRaw("com.myapp.square");
                        arguments[3] = new MockRaw(new object[] {
2,
});
                        //arguments[4] = new MockRaw(new
                        //{
                        //});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Call;
                        //MockRaw[] arguments = new MockRaw[5];
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5522270709612544);
                        arguments[1] = new MockRaw(new
                        {
                            disclose_me = true,
                        });
                        arguments[2] = new MockRaw("com.myapp.square");
                        arguments[3] = new MockRaw(new object[] {
0,
});
                        //arguments[4] = new MockRaw(new
                        //{
                        //});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Call;
                        //MockRaw[] arguments = new MockRaw[5];
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(788260695572480);
                        arguments[1] = new MockRaw(new
                        {
                            disclose_me = true,
                        });
                        arguments[2] = new MockRaw("com.myapp.square");
                        arguments[3] = new MockRaw(new object[] {
-2,
});
                        //arguments[4] = new MockRaw(new
                        //{
                        //});
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class DealerToCaller
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                private static readonly WampMessage<MockRaw> mRequest5;
                static DealerToCaller()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(7360133092761726);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Subscribed;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(5232241456185344);
                        arguments[1] = new MockRaw(1155621269636344);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1155621269636344);
                        arguments[1] = new MockRaw(4226914243725591);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
-2,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(7234508589891584);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
4,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5522270709612544);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
0,
});
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(788260695572480);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
4,
});
                        mRequest5.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                        yield return mRequest5;
                    }
                }
            }

        }

        public class Progress
        {
            public class CalleeToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                private static readonly WampMessage<MockRaw> mRequest5;
                static CalleeToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2388581083512832);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.myapp.longop");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3529884459137455);
                        arguments[1] = new MockRaw(new
                        {
                            progress = true,
                        });
                        arguments[2] = new MockRaw(new object[] {
0,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3529884459137455);
                        arguments[1] = new MockRaw(new
                        {
                            progress = true,
                        });
                        arguments[2] = new MockRaw(new object[] {
1,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3529884459137455);
                        arguments[1] = new MockRaw(new
                        {
                            progress = true,
                        });
                        arguments[2] = new MockRaw(new object[] {
2,
});
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3529884459137455);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
3,
});
                        mRequest5.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                        yield return mRequest5;
                    }
                }
            }

            public class DealerToCallee
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                static DealerToCallee()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8141851371689246);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(2388581083512832);
                        arguments[1] = new MockRaw(5451361325813290);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3529884459137455);
                        arguments[1] = new MockRaw(5451361325813290);
                        arguments[2] = new MockRaw(new
                        {
                            receive_progress = true,
                        });
                        arguments[3] = new MockRaw(new object[] {
3,
});
                        mRequest2.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                    }
                }
            }

            public class CallerToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                static CallerToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Call;
                        //MockRaw[] arguments = new MockRaw[5];
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7814261684305920);
                        arguments[1] = new MockRaw(new
                        {
                            receive_progress = true,
                        });
                        arguments[2] = new MockRaw("com.myapp.longop");
                        arguments[3] = new MockRaw(new object[] {
3,
});
                        //arguments[4] = new MockRaw(new
                        //{
                        //});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.close.normal");
                        mRequest2.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                    }
                }
            }

            public class DealerToCaller
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                private static readonly WampMessage<MockRaw> mRequest5;
                static DealerToCaller()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8105835005721961);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(7814261684305920);
                        arguments[1] = new MockRaw(new
                        {
                            progress = true,
                        });
                        arguments[2] = new MockRaw(new object[] {
0,
});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(7814261684305920);
                        arguments[1] = new MockRaw(new
                        {
                            progress = true,
                        });
                        arguments[2] = new MockRaw(new object[] {
1,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(7814261684305920);
                        arguments[1] = new MockRaw(new
                        {
                            progress = true,
                        });
                        arguments[2] = new MockRaw(new object[] {
2,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(7814261684305920);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
3,
});
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest5.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                        yield return mRequest5;
                    }
                }
            }

        }

        public class Slowsquare
        {
            public class CalleeToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static CalleeToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(8622636745621504);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.math.square");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(6713344727711744);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.math.slowsquare");
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2842632479931560);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
9,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(455899852197574);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
9,
});
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class DealerToCallee
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                private static readonly WampMessage<MockRaw> mRequest4;
                static DealerToCallee()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(4430958192586627);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8622636745621504);
                        arguments[1] = new MockRaw(5383295424135943);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(6713344727711744);
                        arguments[1] = new MockRaw(2977303793738678);
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(455899852197574);
                        arguments[1] = new MockRaw(2977303793738678);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
3,
});
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2842632479931560);
                        arguments[1] = new MockRaw(5383295424135943);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        arguments[3] = new MockRaw(new object[] {
3,
});
                        mRequest4.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                        yield return mRequest4;
                    }
                }
            }

            public class CallerToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                static CallerToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3401341244276736);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.math.slowsquare");
                        arguments[3] = new MockRaw(new object[] {
3,
});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5294499177693184);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.math.square");
                        arguments[3] = new MockRaw(new object[] {
3,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.close.normal");
                        mRequest3.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                    }
                }
            }

            public class DealerToCaller
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                static DealerToCaller()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(7938040662437610);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5294499177693184);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
9,
});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3401341244276736);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
9,
});
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest3.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                        yield return mRequest3;
                    }
                }
            }

        }

        public class Timeservice
        {
            public class CalleeToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                static CalleeToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Register;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5487003168669696);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.timeservice.now");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Yield;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3129619970764329);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new DateTime(2014, 4, 8, 19, 48, 36, 956),
});
                        mRequest2.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                    }
                }
            }

            public class DealerToCallee
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                static DealerToCallee()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(2478062886150247);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Registered;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(5487003168669696);
                        arguments[1] = new MockRaw(6358525630838455);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Invocation;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3129619970764329);
                        arguments[1] = new MockRaw(6358525630838455);
                        arguments[2] = new MockRaw(new
                        {
                        });
                        mRequest2.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                    }
                }
            }

            public class CallerToDealer
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                static CallerToDealer()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Hello;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("realm1");
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                caller = new
                                {
                                    features = new
                                    {
                                        caller_identification = true,
                                        progressive_call_results = true,
                                    },
                                },
                                callee = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                    },
                                },
                                publisher = new
                                {
                                    features = new
                                    {
                                        subscriber_blackwhite_listing = true,
                                        publisher_exclusion = true,
                                        publisher_identification = true,
                                    },
                                },
                                subscriber = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3945114078543872);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw("com.timeservice.now");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.close.normal");
                        mRequest2.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                    }
                }
            }

            public class DealerToCaller
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                static DealerToCaller()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8513808765576784);
                        arguments[1] = new MockRaw(new
                        {
                            roles = new
                            {
                                broker = new
                                {
                                    features = new
                                    {
                                        publisher_identification = true,
                                        publisher_exclusion = true,
                                        subscriber_blackwhite_listing = true,
                                    },
                                },
                                dealer = new
                                {
                                    features = new
                                    {
                                        progressive_call_results = true,
                                        caller_identification = true,
                                    },
                                },
                            },
                        });
                        mRequest0.Arguments = arguments;
                    }
                    mRequest1 = new WampMessage<MockRaw>();
                    {
                        mRequest1.MessageType = WampMessageType.v2Result;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3945114078543872);
                        arguments[1] = new MockRaw(new
                        {
                        });
                        arguments[2] = new MockRaw(new object[] {
new DateTime(2014, 4, 8, 19, 48, 36, 956),
});
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Goodbye;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                        {
                        });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest2.Arguments = arguments;
                    }
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                        yield return mRequest1;
                        yield return mRequest2;
                    }
                }
            }

        }

    }
}