using System.Collections.Generic;
using System.Linq;
using WampSharp.Core.Message;
using WampSharp.Tests.Rpc.Helpers;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Rpc.Client;

namespace WampSharp.Tests.Rpc
{
    public static class RpcCalls
    {
        public static IEnumerable<object[]> SomeCalls
        {
            get
            {
                return Messages.CallMessages
                               .Select(x => new[] {GetRpcCall(x.Arguments)});
            }
        }

        public static IEnumerable<object[]> SimpleCalls
        {
            get
            {
                return AutobahnTestSuiteRpcCalls
                    .Requests.Calls
                    .Select(x => new[] {GetRpcCall(x.Arguments)});
            }
        }

        public static IEnumerable<object[]> CallsWithResults
        {
            get
            {
                IEnumerable<WampMessage<MockRaw>> requests = 
                    AutobahnTestSuiteRpcCalls.Requests.Calls;
                
                IEnumerable<WampMessage<MockRaw>> responses = 
                    AutobahnTestSuiteRpcCalls.Responses.Calls;

                var requestsToResponses =
                    from request in requests
                    from response in responses
                    where response.MessageType == WampMessageType.v1CallResult &&
                          Equals(request.Arguments[0].Value, response.Arguments[0].Value)
                    select new {request, response};

                IEnumerable<object[]> result =
                    from requestToResponse in requestsToResponses 
                    let rpcCall = GetRpcCall(requestToResponse.request.Arguments) 
                    let rpcResult = requestToResponse.response.Arguments[1].Value 
                    select new object[] {rpcCall, rpcResult};

                return result;
            }
        }

        public static IEnumerable<object[]> CallsWithErrors
        {
            get
            {
                IEnumerable<WampMessage<MockRaw>> requests =
                    AutobahnTestSuiteRpcCalls.Requests.Calls;

                IEnumerable<WampMessage<MockRaw>> responses =
                    AutobahnTestSuiteRpcCalls.Responses.Calls;

                var requestsToResponses =
                    from request in requests
                    from response in responses
                    where response.MessageType == WampMessageType.v1CallError &&
                          Equals(request.Arguments[0].Value, response.Arguments[0].Value)
                    select new { request, response };

                IEnumerable<object[]> result =
                    from requestToResponse in requestsToResponses
                    let rpcCall = GetRpcCall(requestToResponse.request.Arguments)
                    let errorDetails = GetErrorDetails(requestToResponse.response.Arguments)
                    select new object[] { rpcCall, errorDetails };

                return result;
            }
        }

        private static CallErrorDetails GetErrorDetails(MockRaw[] arguments)
        {
            object errorDetails = null;

            if (arguments.Length >= 4)
            {
                errorDetails = arguments[3].Value;
            }

            return new CallErrorDetails((string)arguments[1].Value, (string)arguments[2].Value, errorDetails);
        }


        private static WampRpcCall GetRpcCall(MockRaw[] arguments)
        {
            string callId = (string) arguments[0].Value;

            string procUri = (string) arguments[1].Value;

            object[] methodArguments =
                arguments.Skip(2).Select(x => x.Value).ToArray();

            return new WampRpcCall()
                       {
                           Arguments = methodArguments,
                           ProcUri = procUri,
                           ReturnType = typeof (object) // Not sure how we'll do it.
                       };
        }

        private static class AutobahnTestSuiteRpcCalls
        {
            public static class Requests
            {
                private static readonly WampMessage<MockRaw> mRpccall0;
                private static readonly WampMessage<MockRaw> mRpccall1;
                private static readonly WampMessage<MockRaw> mRpccall2;
                private static readonly WampMessage<MockRaw> mRpccall3;
                private static readonly WampMessage<MockRaw> mRpccall4;
                private static readonly WampMessage<MockRaw> mRpccall5;
                private static readonly WampMessage<MockRaw> mRpccall6;
                private static readonly WampMessage<MockRaw> mRpccall7;
                private static readonly WampMessage<MockRaw> mRpccall8;
                private static readonly WampMessage<MockRaw> mRpccall9;

                static Requests()
                {
                    mRpccall0 = new WampMessage<MockRaw>();
                    {
                        mRpccall0.MessageType = WampMessageType.v1Prefix;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("calc");
                        arguments[1] = new MockRaw("http://example.com/simple/calc#");
                        mRpccall0.Arguments = arguments;
                    }
                    mRpccall1 = new WampMessage<MockRaw>();
                    {
                        mRpccall1.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.gmf103gm25efjemi");
                        arguments[1] = new MockRaw("calc:square");
                        arguments[2] = new MockRaw(23);
                        mRpccall1.Arguments = arguments;
                    }
                    mRpccall2 = new WampMessage<MockRaw>();
                    {
                        mRpccall2.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw("0.2qpscjivpf58w7b9");
                        arguments[1] = new MockRaw("calc:add");
                        arguments[2] = new MockRaw(23);
                        arguments[3] = new MockRaw(7);
                        mRpccall2.Arguments = arguments;
                    }
                    mRpccall3 = new WampMessage<MockRaw>();
                    {
                        mRpccall3.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.407ldfwznk10dx6r");
                        arguments[1] = new MockRaw("calc:sum");
                        arguments[2] = new MockRaw(new []
                                                   {
                                                       1,
                                                       2,
                                                       3,
                                                       4,
                                                       5,
                                                   });
                        mRpccall3.Arguments = arguments;
                    }
                    mRpccall4 = new WampMessage<MockRaw>();
                    {
                        mRpccall4.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.d9bcva2fszjpds4i");
                        arguments[1] = new MockRaw("calc:square");
                        arguments[2] = new MockRaw(23);
                        mRpccall4.Arguments = arguments;
                    }
                    mRpccall5 = new WampMessage<MockRaw>();
                    {
                        mRpccall5.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.h1r0hxik62a3v7vi");
                        arguments[1] = new MockRaw("calc:sqrt");
                        arguments[2] = new MockRaw(-1);
                        mRpccall5.Arguments = arguments;
                    }
                    mRpccall6 = new WampMessage<MockRaw>();
                    {
                        mRpccall6.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.bbmk6lzxl6vibe29");
                        arguments[1] = new MockRaw("calc:square");
                        arguments[2] = new MockRaw(1001);
                        mRpccall6.Arguments = arguments;
                    }
                    mRpccall7 = new WampMessage<MockRaw>();
                    {
                        mRpccall7.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.8ddkhnsgjwtgldi");
                        arguments[1] = new MockRaw("calc:asum");
                        arguments[2] = new MockRaw(new []
                                                   {
                                                       1,
                                                       2,
                                                       3,
                                                   });
                        mRpccall7.Arguments = arguments;
                    }
                    mRpccall8 = new WampMessage<MockRaw>();
                    {
                        mRpccall8.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.nzdto9xdn6vl5wmi");
                        arguments[1] = new MockRaw("calc:sum");
                        arguments[2] = new MockRaw(new []
                                                   {
                                                       4,
                                                       5,
                                                       6,
                                                   });
                        mRpccall8.Arguments = arguments;
                    }
                    mRpccall9 = new WampMessage<MockRaw>();
                    {
                        mRpccall9.MessageType = WampMessageType.v1Call;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.tuoo16bh5pix80k9");
                        arguments[1] = new MockRaw("calc:pickySum");
                        arguments[2] = new MockRaw(new []
                                                   {
                                                       0,
                                                       1,
                                                       2,
                                                       3,
                                                       4,
                                                       5,
                                                   });
                        mRpccall9.Arguments = arguments;
                    }
                }

                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRpccall1;
                        yield return mRpccall2;
                        yield return mRpccall3;
                        yield return mRpccall4;
                        yield return mRpccall5;
                        yield return mRpccall6;
                        yield return mRpccall7;
                        yield return mRpccall8;
                        yield return mRpccall9;
                    }
                }
            }

            public static class Responses
            {
                private static readonly WampMessage<MockRaw> mRpccall0;
                private static readonly WampMessage<MockRaw> mRpccall1;
                private static readonly WampMessage<MockRaw> mRpccall2;
                private static readonly WampMessage<MockRaw> mRpccall3;
                private static readonly WampMessage<MockRaw> mRpccall4;
                private static readonly WampMessage<MockRaw> mRpccall5;
                private static readonly WampMessage<MockRaw> mRpccall6;
                private static readonly WampMessage<MockRaw> mRpccall7;
                private static readonly WampMessage<MockRaw> mRpccall8;
                private static readonly WampMessage<MockRaw> mRpccall9;

                static Responses()
                {
                    mRpccall0 = new WampMessage<MockRaw>();
                    {
                        mRpccall0.MessageType = WampMessageType.v1Welcome;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("hT8vUg9h_9qh9rcd");
                        arguments[1] = new MockRaw(1);
                        arguments[2] = new MockRaw("Autobahn/0.5.14");
                        mRpccall0.Arguments = arguments;
                    }
                    mRpccall1 = new WampMessage<MockRaw>();
                    {
                        mRpccall1.MessageType = WampMessageType.v1CallResult;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("0.gmf103gm25efjemi");
                        arguments[1] = new MockRaw(529);
                        mRpccall1.Arguments = arguments;
                    }
                    mRpccall2 = new WampMessage<MockRaw>();
                    {
                        mRpccall2.MessageType = WampMessageType.v1CallResult;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("0.2qpscjivpf58w7b9");
                        arguments[1] = new MockRaw(30);
                        mRpccall2.Arguments = arguments;
                    }
                    mRpccall3 = new WampMessage<MockRaw>();
                    {
                        mRpccall3.MessageType = WampMessageType.v1CallResult;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("0.407ldfwznk10dx6r");
                        arguments[1] = new MockRaw(15);
                        mRpccall3.Arguments = arguments;
                    }
                    mRpccall4 = new WampMessage<MockRaw>();
                    {
                        mRpccall4.MessageType = WampMessageType.v1CallResult;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("0.d9bcva2fszjpds4i");
                        arguments[1] = new MockRaw(529);
                        mRpccall4.Arguments = arguments;
                    }
                    mRpccall5 = new WampMessage<MockRaw>();
                    {
                        mRpccall5.MessageType = WampMessageType.v1CallError;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw("0.h1r0hxik62a3v7vi");
                        arguments[1] = new MockRaw("http://api.wamp.ws/error#generic");
                        arguments[2] = new MockRaw("math domain error");
                        arguments[3] = new MockRaw(new []
                                                       {
                                                           @"Traceback (most recent call last):",
                                                           @"  File ""C:\Python27\lib\site-packages\autobahn-0.5.14-py2.7.egg\autobahn\websocket.py"", line 1674, in processDataHybi"
                                                           ,
                                                           @"    fr = self.onFrameEnd()",
                                                           @"  File ""C:\Python27\lib\site-packages\autobahn-0.5.14-py2.7.egg\autobahn\websocket.py"", line 1751, in onFrameEnd"
                                                           ,
                                                           @"    self.onMessageEnd()",
                                                           @"  File ""C:\Python27\lib\site-packages\autobahn-0.5.14-py2.7.egg\autobahn\websocket.py"", line 649, in onMessageEnd"
                                                           ,
                                                           @"    self.onMessage(payload, self.message_opcode == WebSocketProtocol.MESSAGE_TYPE_BINARY)"
                                                           ,
                                                           @"  File ""C:\Python27\lib\site-packages\autobahn-0.5.14-py2.7.egg\autobahn\wamp.py"", line 877, in onMessage"
                                                           ,
                                                           @"    d = maybeDeferred(self._callProcedure, call)",
                                                           @"--- <exception caught here> ---",
                                                           @"  File ""C:\Python27\lib\site-packages\twisted\internet\defer.py"", line 137, in maybeDeferred"
                                                           ,
                                                           @"    result = f(*args, **kw)",
                                                           @"  File ""C:\Python27\lib\site-packages\autobahn-0.5.14-py2.7.egg\autobahn\wamp.py"", line 633, in _callProcedure"
                                                           ,
                                                           @"    return m[1](m[0], *cargs)",
                                                           @"  File ""C:\Python27\lib\site-packages\autobahntestsuite\wamptestserver.py"", line 76, in sqrt"
                                                           ,
                                                           @"    return math.sqrt(x)",
                                                           @"exceptions.ValueError: math domain error",
                                                       });
                        mRpccall5.Arguments = arguments;
                    }
                    mRpccall6 = new WampMessage<MockRaw>();
                    {
                        mRpccall6.MessageType = WampMessageType.v1CallError;
                        MockRaw[] arguments = new MockRaw[3];
                        arguments[0] = new MockRaw("0.bbmk6lzxl6vibe29");
                        arguments[1] = new MockRaw("http://example.com/error#number_too_big");
                        arguments[2] = new MockRaw("number 1001 too big to square");
                        mRpccall6.Arguments = arguments;
                    }
                    mRpccall7 = new WampMessage<MockRaw>();
                    {
                        mRpccall7.MessageType = WampMessageType.v1CallResult;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("0.nzdto9xdn6vl5wmi");
                        arguments[1] = new MockRaw(15);
                        mRpccall7.Arguments = arguments;
                    }
                    mRpccall8 = new WampMessage<MockRaw>();
                    {
                        mRpccall8.MessageType = WampMessageType.v1CallError;
                        MockRaw[] arguments = new MockRaw[4];
                        arguments[0] = new MockRaw("0.tuoo16bh5pix80k9");
                        arguments[1] = new MockRaw("http://example.com/error#invalid_numbers");
                        arguments[2] = new MockRaw("one or more numbers are multiples of 3");
                        arguments[3] = new MockRaw(new []
                                                       {
                                                           0,
                                                           3,
                                                       });
                        mRpccall8.Arguments = arguments;
                    }
                    mRpccall9 = new WampMessage<MockRaw>();
                    {
                        mRpccall9.MessageType = WampMessageType.v1CallResult;
                        MockRaw[] arguments = new MockRaw[2];
                        arguments[0] = new MockRaw("0.8ddkhnsgjwtgldi");
                        arguments[1] = new MockRaw(6);
                        mRpccall9.Arguments = arguments;
                    }
                }

                public static IEnumerable<WampMessage<MockRaw>> Calls
                {
                    get
                    {
                        yield return mRpccall1;
                        yield return mRpccall2;
                        yield return mRpccall3;
                        yield return mRpccall4;
                        yield return mRpccall5;
                        yield return mRpccall6;
                        yield return mRpccall7;
                        yield return mRpccall8;
                        yield return mRpccall9;
                    }
                }            
            }
        }
    }
}