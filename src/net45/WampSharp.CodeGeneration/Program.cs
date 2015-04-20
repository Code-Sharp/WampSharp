namespace WampSharp.CodeGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            CalleeProxyCodeGenerator generator = new CalleeProxyCodeGenerator("MyNamespace");

            string generateCode = generator.GenerateCode(typeof (IArgumentsService));

        } 
    }
}