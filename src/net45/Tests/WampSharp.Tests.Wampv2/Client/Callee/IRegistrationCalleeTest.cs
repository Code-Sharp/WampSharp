namespace WampSharp.Tests.Wampv2.Client.Callee
{
    interface IRegistrationCalleeTest
    {
        void SetupRegister(object options, string procedure);
        void SetupRegistered(long registrationId);

        void SetupError(object details, string error);
        void SetupError(object details, string error, object[] arguments);
        void SetupError(object details, string error, object[] arguments, object argumentsKeywords);

        void Act();
        void Assert();
    }
}