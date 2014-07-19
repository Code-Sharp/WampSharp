namespace WampSharp.Tests.Wampv2.Client.Callee
{
    interface IInvocationCalleeeTest
    {
        void SetupInvocation(long requestId, long registrationId, object details);
        void SetupInvocation(long requestId, long registrationId, object details, object[] arguments);
        void SetupInvocation(long requestId, long registrationId, object details, object[] arguments, object argumentsKeywords);

        void SetupYield(long requestId, object options);
        void SetupYield(long requestId, object options, object[] arguments);
        void SetupYield(long requestId, object options, object[] arguments, object argumentsKeywords);

        void SetupError(long requestType, long requestId, object details, string error);
        void SetupError(long requestType, long requestId, object details, string error, object[] arguments);
        void SetupError(long requestType, long requestId, object details, string error, object[] arguments, object argumentsKeywords);

        void Act();
        void Assert();
    }
}