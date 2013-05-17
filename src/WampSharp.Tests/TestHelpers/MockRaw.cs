namespace WampSharp.Tests.TestHelpers
{
    public class MockRaw
    {
        private readonly object mValue;

        public MockRaw(object value)
        {
            MockRaw raw = value as MockRaw;
            
            if (raw != null)
            {
                mValue = raw.Value;
            }
            else
            {
                mValue = value;                
            }
        }

        public object Value
        {
            get
            {
                return mValue;
            }
        }
    }
}