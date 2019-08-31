using System;
using System.Linq;
using NUnit.Framework;
using WampSharp.Tests.Rpc.Helpers;
using WampSharp.Tests.TestHelpers;
using WampSharp.V1.Rpc;
using WampSharp.V1.Rpc.Client;
using WampSharp.V1.Rpc.Server;

namespace WampSharp.Tests.Rpc
{
    [TestFixture]
    public class RpcServerTests
    {
        private static Type GetType(object value)
        {
            if (value == null)
            {
                return typeof (object);
            }

            return value.GetType();
        }

        [TestCaseSource(typeof (RpcCalls), nameof(RpcCalls.CallsWithResults))]
        public void SuccessfulTaskCallsCallResultWithResult(WampRpcCall call, object result)
        {            
            MockRawFormatter formatter = new MockRawFormatter();

            MockRpcCatalog catalog = new MockRpcCatalog();
            
            call.CallId = Guid.NewGuid().ToString();
         
            MockRpcMethod mockMethod = GetMockMethod(call);
            
            mockMethod.Result = result;
            
            catalog.MapMethod(mockMethod);

            WampRpcServer<MockRaw> server =
                new WampRpcServer<MockRaw>(formatter, catalog);

            MockClient client = new MockClient();

            server.Call(client, call.CallId, call.ProcUri,
                        SerializeArguments(call, formatter));

            Assert.That(client.GetCallErrorByCallId(call.CallId),
                        Is.Null);

            Assert.That(client.GetResultByCallId(call.CallId),
                        Is.EqualTo(result));
        }

        [TestCaseSource(typeof(RpcCalls), nameof(RpcCalls.CallsWithErrors))]
        public void ErrorTaskCallsCallErrorWithError(WampRpcCall call, CallErrorDetails details)
        {
            MockRawFormatter formatter = new MockRawFormatter();

            MockRpcCatalog catalog = new MockRpcCatalog();

            call.CallId = Guid.NewGuid().ToString();

            MockRpcMethod mockMethod = GetMockMethod(call);

            mockMethod.Error =
                new WampRpcCallException(details.ErrorUri,
                                         details.ErrorDesc,
                                         details.ErrorDetails);

            catalog.MapMethod(mockMethod);

            WampRpcServer<MockRaw> server =
                new WampRpcServer<MockRaw>(formatter, catalog);

            MockClient client = new MockClient();

            server.Call(client, call.CallId, call.ProcUri,
                        SerializeArguments(call, formatter));

            Assert.That(client.GetResultByCallId(call.CallId),
                        Is.Null);

            CallErrorDetails error = client.GetCallErrorByCallId(call.CallId);
            
            Assert.That(error.ErrorDesc,
                        Is.EqualTo(details.ErrorDesc));

            Assert.That(error.ErrorDetails,
                        Is.EqualTo(details.ErrorDetails));
            
            Assert.That(error.ErrorUri,
                        Is.EqualTo(details.ErrorUri));
        }


        private static MockRaw[] SerializeArguments(WampRpcCall call, MockRawFormatter formatter)
        {
            return call.Arguments.Select(x => formatter.Serialize(x)).ToArray();
        }

        private static MockRpcMethod GetMockMethod(WampRpcCall call)
        {
            return new MockRpcMethod()
                {
                    ProcUri = call.ProcUri,
                    Parameters = call.Arguments.Select
                        (x => GetType(x)).ToArray()
                };
        }
    }
}