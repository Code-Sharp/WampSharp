using System.Collections.Generic;
using WampSharp.Core.Message;
using WampSharp.Tests.TestHelpers;

namespace WampSharp.Tests.Wampv2.TestHelpers
{
    public class PubSub
    {
        public class Basic
        {
            public class PublisherToBroker
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
                static PublisherToBroker()
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
                        mRequest1.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5495107736305664);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    0,
                                                                });
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1380191131664384);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    1,
                                                                });
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1074076785311744);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    2,
                                                                });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(6956355384508416);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    3,
                                                                });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3816441759399936);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    4,
                                                                });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5368978585157632);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    5,
                                                                });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(838749214736384);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    6,
                                                                });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4948166818398208);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    7,
                                                                });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2182477120536576);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    8,
                                                                });
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5378436392550400);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    9,
                                                                });
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2534381939851264);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    10,
                                                                });
                        mRequest11.Arguments = arguments;
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
                    }
                }
            }

            public class BrokerToPublisher
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                static BrokerToPublisher()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(7871999452503864);
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
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                    }
                }
            }

            public class SubscriberToBroker
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                static SubscriberToBroker()
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
                        arguments[0] = new MockRaw(6310625196113920);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v1Unsubscribe;
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

            public class BrokerToSubscriber
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
                static BrokerToSubscriber()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(5408330594634204);
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
                        arguments[0] = new MockRaw(6310625196113920);
                        arguments[1] = new MockRaw(4662002734395671);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4662002734395671);
                        arguments[1] = new MockRaw(3342868652389940);
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
                        mRequest3.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4662002734395671);
                        arguments[1] = new MockRaw(3277030165335260);
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
                        mRequest4.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4662002734395671);
                        arguments[1] = new MockRaw(6598239233696265);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    4,
                                                                });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4662002734395671);
                        arguments[1] = new MockRaw(5244299925002926);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    5,
                                                                });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4662002734395671);
                        arguments[1] = new MockRaw(6997323342306209);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    6,
                                                                });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4662002734395671);
                        arguments[1] = new MockRaw(4602699089168075);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    7,
                                                                });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v1Unsubscribe;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                                                       {
                                                       });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest8.Arguments = arguments;
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
                    }
                }
            }

        }

        public class Complex
        {
            public class PublisherToBroker
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
                private static readonly WampMessage<MockRaw> mRequest18;
                private static readonly WampMessage<MockRaw> mRequest19;
                private static readonly WampMessage<MockRaw> mRequest20;
                private static readonly WampMessage<MockRaw> mRequest21;
                private static readonly WampMessage<MockRaw> mRequest22;
                static PublisherToBroker()
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
                        mRequest1.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3744908405899264);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(5365198372208640);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    37,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 0,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2455972557619200);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(1865857797980160);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    47,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 1,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(1483640248729600);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(7972137669230592);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    68,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 2,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(801806502330368);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(5662615472701440);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    63,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 3,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(6461305816875008);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(4761959916371968);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    24,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 4,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4301520434626560);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest11.Arguments = arguments;
                    }
                    mRequest12 = new WampMessage<MockRaw>();
                    {
                        mRequest12.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(1665074043289600);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    50,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 5,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest12.Arguments = arguments;
                    }
                    mRequest13 = new WampMessage<MockRaw>();
                    {
                        mRequest13.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(807888295559168);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest13.Arguments = arguments;
                    }
                    mRequest14 = new WampMessage<MockRaw>();
                    {
                        mRequest14.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(6748568706613248);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    98,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 6,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest14.Arguments = arguments;
                    }
                    mRequest15 = new WampMessage<MockRaw>();
                    {
                        mRequest15.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(734365669654528);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest15.Arguments = arguments;
                    }
                    mRequest16 = new WampMessage<MockRaw>();
                    {
                        mRequest16.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(8463396217290752);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    97,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 7,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest16.Arguments = arguments;
                    }
                    mRequest17 = new WampMessage<MockRaw>();
                    {
                        mRequest17.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(8673928069251072);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest17.Arguments = arguments;
                    }
                    mRequest18 = new WampMessage<MockRaw>();
                    {
                        mRequest18.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(7844644022910976);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    11,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 8,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest18.Arguments = arguments;
                    }
                    mRequest19 = new WampMessage<MockRaw>();
                    {
                        mRequest19.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(4150944967163904);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest19.Arguments = arguments;
                    }
                    mRequest20 = new WampMessage<MockRaw>();
                    {
                        mRequest20.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(5428445504864256);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    28,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 9,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest20.Arguments = arguments;
                    }
                    mRequest21 = new WampMessage<MockRaw>();
                    {
                        mRequest21.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(7061622132572160);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest21.Arguments = arguments;
                    }
                    mRequest22 = new WampMessage<MockRaw>();
                    {
                        mRequest22.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(850790434471936);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    13,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 10,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest22.Arguments = arguments;
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
                        yield return mRequest18;
                        yield return mRequest19;
                        yield return mRequest20;
                        yield return mRequest21;
                        yield return mRequest22;
                    }
                }
            }

            public class BrokerToPublisher
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                static BrokerToPublisher()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(7547253137785148);
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
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                    }
                }
            }

            public class SubscriberToBroker
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                static SubscriberToBroker()
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
                        arguments[0] = new MockRaw(6980044016582656);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.heartbeat");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Subscribe;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5337143425630208);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v1Unsubscribe;
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

            public class BrokerToSubscriber
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
                static BrokerToSubscriber()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(4905336726089839);
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
                        arguments[0] = new MockRaw(6980044016582656);
                        arguments[1] = new MockRaw(5206921752018583);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Subscribed;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(5337143425630208);
                        arguments[1] = new MockRaw(276647517742280);
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5206921752018583);
                        arguments[1] = new MockRaw(8845511056903388);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(276647517742280);
                        arguments[1] = new MockRaw(1664453009009963);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    68,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 2,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5206921752018583);
                        arguments[1] = new MockRaw(3490081616219912);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(276647517742280);
                        arguments[1] = new MockRaw(4414488209024943);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    63,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 3,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5206921752018583);
                        arguments[1] = new MockRaw(8397761090732100);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(276647517742280);
                        arguments[1] = new MockRaw(6589909731328829);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    24,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 4,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5206921752018583);
                        arguments[1] = new MockRaw(6494739457367770);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(276647517742280);
                        arguments[1] = new MockRaw(5142830358133543);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    50,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 5,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(5206921752018583);
                        arguments[1] = new MockRaw(3918997057153033);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        mRequest11.Arguments = arguments;
                    }
                    mRequest12 = new WampMessage<MockRaw>();
                    {
                        mRequest12.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[5];
                        arguments[0] = new MockRaw(276647517742280);
                        arguments[1] = new MockRaw(1964856029831184);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    98,
                                                                    23,
                                                                });
                        arguments[4] = new MockRaw(new
                                                       {
                                                           c = "Hello",
                                                           d = new
                                                                   {
                                                                       counter = 6,
                                                                       foo = new object[] {
                                                                                              1,
                                                                                              2,
                                                                                              3,
                                                                                          },
                                                                   },
                                                       });
                        mRequest12.Arguments = arguments;
                    }
                    mRequest13 = new WampMessage<MockRaw>();
                    {
                        mRequest13.MessageType = WampMessageType.v1Unsubscribe;
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

        public class Decorators
        {
            public class PublisherToBroker
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
                private static readonly WampMessage<MockRaw> mRequest18;
                private static readonly WampMessage<MockRaw> mRequest19;
                private static readonly WampMessage<MockRaw> mRequest20;
                private static readonly WampMessage<MockRaw> mRequest21;
                private static readonly WampMessage<MockRaw> mRequest22;
                static PublisherToBroker()
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
                        mRequest1.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4589737583050752);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    0,
                                                                });
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2018126950563840);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1015132526215168);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    1,
                                                                });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4102304928104448);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3294644834140160);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    2,
                                                                });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7327930061422592);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1381112085479424);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    3,
                                                                });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7646400932216832);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1161504336379904);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    4,
                                                                });
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5197886610472960);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7883472420995072);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    5,
                                                                });
                        mRequest11.Arguments = arguments;
                    }
                    mRequest12 = new WampMessage<MockRaw>();
                    {
                        mRequest12.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1965808546742272);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest12.Arguments = arguments;
                    }
                    mRequest13 = new WampMessage<MockRaw>();
                    {
                        mRequest13.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2356347534311424);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    6,
                                                                });
                        mRequest13.Arguments = arguments;
                    }
                    mRequest14 = new WampMessage<MockRaw>();
                    {
                        mRequest14.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1954499579084800);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest14.Arguments = arguments;
                    }
                    mRequest15 = new WampMessage<MockRaw>();
                    {
                        mRequest15.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(8539953361321984);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    7,
                                                                });
                        mRequest15.Arguments = arguments;
                    }
                    mRequest16 = new WampMessage<MockRaw>();
                    {
                        mRequest16.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(534952917598208);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest16.Arguments = arguments;
                    }
                    mRequest17 = new WampMessage<MockRaw>();
                    {
                        mRequest17.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4024480938590208);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    8,
                                                                });
                        mRequest17.Arguments = arguments;
                    }
                    mRequest18 = new WampMessage<MockRaw>();
                    {
                        mRequest18.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2321973082324992);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest18.Arguments = arguments;
                    }
                    mRequest19 = new WampMessage<MockRaw>();
                    {
                        mRequest19.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5176854497460224);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    9,
                                                                });
                        mRequest19.Arguments = arguments;
                    }
                    mRequest20 = new WampMessage<MockRaw>();
                    {
                        mRequest20.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2261801620209664);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest20.Arguments = arguments;
                    }
                    mRequest21 = new WampMessage<MockRaw>();
                    {
                        mRequest21.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4373585382604800);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    10,
                                                                });
                        mRequest21.Arguments = arguments;
                    }
                    mRequest22 = new WampMessage<MockRaw>();
                    {
                        mRequest22.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7229672999878656);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest22.Arguments = arguments;
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
                        yield return mRequest18;
                        yield return mRequest19;
                        yield return mRequest20;
                        yield return mRequest21;
                        yield return mRequest22;
                    }
                }
            }

            public class BrokerToPublisher
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                static BrokerToPublisher()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(6183979860635428);
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
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                    }
                }
            }

            public class SubscriberToBroker
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                static SubscriberToBroker()
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
                        arguments[0] = new MockRaw(5196507003224064);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Subscribe;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(2162370572976128);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic2");
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v1Unsubscribe;
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

            public class BrokerToSubscriber
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
                static BrokerToSubscriber()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8960945267708004);
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
                        arguments[0] = new MockRaw(5196507003224064);
                        arguments[1] = new MockRaw(7781334913978808);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Subscribed;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(2162370572976128);
                        arguments[1] = new MockRaw(5813816269741132);
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7781334913978808);
                        arguments[1] = new MockRaw(1279513002012629);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    2,
                                                                });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5813816269741132);
                        arguments[1] = new MockRaw(5072735158170068);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7781334913978808);
                        arguments[1] = new MockRaw(4322563949735716);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    3,
                                                                });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5813816269741132);
                        arguments[1] = new MockRaw(8426845269007930);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7781334913978808);
                        arguments[1] = new MockRaw(4096003092366550);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    4,
                                                                });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5813816269741132);
                        arguments[1] = new MockRaw(6684632982296405);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7781334913978808);
                        arguments[1] = new MockRaw(5006720978161751);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    5,
                                                                });
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5813816269741132);
                        arguments[1] = new MockRaw(2153654865728960);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7781334913978808);
                        arguments[1] = new MockRaw(318634588319766);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    6,
                                                                });
                        mRequest11.Arguments = arguments;
                    }
                    mRequest12 = new WampMessage<MockRaw>();
                    {
                        mRequest12.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5813816269741132);
                        arguments[1] = new MockRaw(2755831448242524);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest12.Arguments = arguments;
                    }
                    mRequest13 = new WampMessage<MockRaw>();
                    {
                        mRequest13.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7781334913978808);
                        arguments[1] = new MockRaw(4935718200176274);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    7,
                                                                });
                        mRequest13.Arguments = arguments;
                    }
                    mRequest14 = new WampMessage<MockRaw>();
                    {
                        mRequest14.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5813816269741132);
                        arguments[1] = new MockRaw(5149747433484418);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    "Hello world.",
                                                                });
                        mRequest14.Arguments = arguments;
                    }
                    mRequest15 = new WampMessage<MockRaw>();
                    {
                        mRequest15.MessageType = WampMessageType.v1Unsubscribe;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                                                       {
                                                       });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest15.Arguments = arguments;
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
                    }
                }
            }

        }

        public class Options
        {
            public class PublisherToBroker
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
                static PublisherToBroker()
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
                        mRequest1.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1321942957162496);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    0,
                                                                });
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(8591554579005440);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    1,
                                                                });
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5393136366911488);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    2,
                                                                });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2827662267514880);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    3,
                                                                });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3693598377771008);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    4,
                                                                });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1799077494784000);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    5,
                                                                });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(8273822264328192);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    6,
                                                                });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(8762065514659840);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    7,
                                                                });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(7515247063597056);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    8,
                                                                });
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3238000288858112);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    9,
                                                                });
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(394435936387072);
                        arguments[1] = new MockRaw(new
                                                       {
                                                           acknowledge = true,
                                                           disclose_me = true,
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    10,
                                                                });
                        mRequest11.Arguments = arguments;
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
                    }
                }
            }

            public class BrokerToPublisher
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
                static BrokerToPublisher()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(7615967134716561);
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
                        mRequest1.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(1321942957162496);
                        arguments[1] = new MockRaw(4786317357826879);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8591554579005440);
                        arguments[1] = new MockRaw(877372598374460);
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(5393136366911488);
                        arguments[1] = new MockRaw(6583300269135315);
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(2827662267514880);
                        arguments[1] = new MockRaw(8658853291763465);
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(3693598377771008);
                        arguments[1] = new MockRaw(703979703735183);
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(1799077494784000);
                        arguments[1] = new MockRaw(8799808257290414);
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8273822264328192);
                        arguments[1] = new MockRaw(2147720969305254);
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8762065514659840);
                        arguments[1] = new MockRaw(1544250353053263);
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(7515247063597056);
                        arguments[1] = new MockRaw(5700029190941942);
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(3238000288858112);
                        arguments[1] = new MockRaw(4068234012715059);
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Published;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(394435936387072);
                        arguments[1] = new MockRaw(8582891517728979);
                        mRequest11.Arguments = arguments;
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
                    }
                }
            }

            public class SubscriberToBroker
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                static SubscriberToBroker()
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
                        arguments[0] = new MockRaw(5999309346570240);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v1Unsubscribe;
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

            public class BrokerToSubscriber
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
                static BrokerToSubscriber()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(4547231625743031);
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
                        arguments[0] = new MockRaw(5999309346570240);
                        arguments[1] = new MockRaw(5297031543255876);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5297031543255876);
                        arguments[1] = new MockRaw(8658853291763465);
                        arguments[2] = new MockRaw(new
                                                       {
                                                           publisher = 7615967134716561,
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    3,
                                                                });
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5297031543255876);
                        arguments[1] = new MockRaw(703979703735183);
                        arguments[2] = new MockRaw(new
                                                       {
                                                           publisher = 7615967134716561,
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    4,
                                                                });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5297031543255876);
                        arguments[1] = new MockRaw(8799808257290414);
                        arguments[2] = new MockRaw(new
                                                       {
                                                           publisher = 7615967134716561,
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    5,
                                                                });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5297031543255876);
                        arguments[1] = new MockRaw(2147720969305254);
                        arguments[2] = new MockRaw(new
                                                       {
                                                           publisher = 7615967134716561,
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    6,
                                                                });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5297031543255876);
                        arguments[1] = new MockRaw(1544250353053263);
                        arguments[2] = new MockRaw(new
                                                       {
                                                           publisher = 7615967134716561,
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    7,
                                                                });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5297031543255876);
                        arguments[1] = new MockRaw(5700029190941942);
                        arguments[2] = new MockRaw(new
                                                       {
                                                           publisher = 7615967134716561,
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    8,
                                                                });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v1Unsubscribe;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(new
                                                       {
                                                       });
                        arguments[1] = new MockRaw("wamp.goodbye.normal");
                        mRequest8.Arguments = arguments;
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
                    }
                }
            }

        }

        public class Unsubscribe
        {
            public class PublisherToBroker
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
                static PublisherToBroker()
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
                        mRequest1.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2256934948306944);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    0,
                                                                });
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(6556882409881600);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    1,
                                                                });
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(8281244544532480);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    2,
                                                                });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1590289152081920);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    3,
                                                                });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(2236761799393280);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    4,
                                                                });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5661944635719680);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    5,
                                                                });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4473633636352000);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    6,
                                                                });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(5520047214690304);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    7,
                                                                });
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(4871776318259200);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    8,
                                                                });
                        mRequest9.Arguments = arguments;
                    }
                    mRequest10 = new WampMessage<MockRaw>();
                    {
                        mRequest10.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3252943958573056);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    9,
                                                                });
                        mRequest10.Arguments = arguments;
                    }
                    mRequest11 = new WampMessage<MockRaw>();
                    {
                        mRequest11.MessageType = WampMessageType.v2Publish;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(3186272468205568);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        arguments[3] = new MockRaw(new object[] {
                                                                    10,
                                                                });
                        mRequest11.Arguments = arguments;
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
                    }
                }
            }

            public class BrokerToPublisher
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                static BrokerToPublisher()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(447972536250554);
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
                }
                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRequest0;
                    }
                }
            }

            public class SubscriberToBroker
            {
                private static readonly WampMessage<MockRaw> mRequest0;
                private static readonly WampMessage<MockRaw> mRequest1;
                private static readonly WampMessage<MockRaw> mRequest2;
                private static readonly WampMessage<MockRaw> mRequest3;
                static SubscriberToBroker()
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
                        arguments[0] = new MockRaw(8607861795979264);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Unsubscribe;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(8349699834642432);
                        arguments[1] = new MockRaw(1708375999279093);
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Subscribe;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw(3361011688013824);
                        arguments[1] = new MockRaw(new
                                                       {
                                                       });
                        arguments[2] = new MockRaw("com.myapp.topic1");
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

            public class BrokerToSubscriber
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
                static BrokerToSubscriber()
                {
                    mRequest0 = new WampMessage<MockRaw>();
                    {
                        mRequest0.MessageType = WampMessageType.v2Welcome;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(6507830921970835);
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
                        arguments[0] = new MockRaw(8607861795979264);
                        arguments[1] = new MockRaw(1708375999279093);
                        mRequest1.Arguments = arguments;
                    }
                    mRequest2 = new WampMessage<MockRaw>();
                    {
                        mRequest2.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1708375999279093);
                        arguments[1] = new MockRaw(2503468839296406);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    3,
                                                                });
                        mRequest2.Arguments = arguments;
                    }
                    mRequest3 = new WampMessage<MockRaw>();
                    {
                        mRequest3.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1708375999279093);
                        arguments[1] = new MockRaw(6266147246221940);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    4,
                                                                });
                        mRequest3.Arguments = arguments;
                    }
                    mRequest4 = new WampMessage<MockRaw>();
                    {
                        mRequest4.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1708375999279093);
                        arguments[1] = new MockRaw(5498370740908599);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    5,
                                                                });
                        mRequest4.Arguments = arguments;
                    }
                    mRequest5 = new WampMessage<MockRaw>();
                    {
                        mRequest5.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1708375999279093);
                        arguments[1] = new MockRaw(4813907568278066);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    6,
                                                                });
                        mRequest5.Arguments = arguments;
                    }
                    mRequest6 = new WampMessage<MockRaw>();
                    {
                        mRequest6.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1708375999279093);
                        arguments[1] = new MockRaw(2055118043605150);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    7,
                                                                });
                        mRequest6.Arguments = arguments;
                    }
                    mRequest7 = new WampMessage<MockRaw>();
                    {
                        mRequest7.MessageType = WampMessageType.v2Event;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw(1708375999279093);
                        arguments[1] = new MockRaw(999998943518496);
                        arguments[2] = new MockRaw(new
                                                       {
                                                       });
                        arguments[3] = new MockRaw(new object[] {
                                                                    8,
                                                                });
                        mRequest7.Arguments = arguments;
                    }
                    mRequest8 = new WampMessage<MockRaw>();
                    {
                        mRequest8.MessageType = WampMessageType.v2Unsubscribed;
                        MockRaw[] arguments = new MockRaw[1];
                        arguments[0] = new MockRaw(8349699834642432);
                        mRequest8.Arguments = arguments;
                    }
                    mRequest9 = new WampMessage<MockRaw>();
                    {
                        mRequest9.MessageType = WampMessageType.v2Subscribed;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw(3361011688013824);
                        arguments[1] = new MockRaw(2026572862290656);
                        mRequest9.Arguments = arguments;
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
                    }
                }
            }
        }
    }
}