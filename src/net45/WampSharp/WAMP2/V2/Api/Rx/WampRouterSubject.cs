using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;

namespace WampSharp.V2
{
    internal class WampRouterSubject : WampSubject
    {
        private readonly IWampTopic mTopic;

        public WampRouterSubject(IWampTopic topic)
        {
            mTopic = topic;
        }

        public override IDisposable Subscribe(IObserver<IWampSerializedEvent> observer)
        {
            return mTopic.Subscribe(new RawRouterSubscriber(observer));
        }

        protected override void Publish(IDictionary<string, object> options)
        {
            mTopic.Publish(WampObjectFormatter.Value,
                           DeserializeOptions(options));
        }

        protected override void Publish(IDictionary<string, object> options, object[] arguments)
        {
            mTopic.Publish(WampObjectFormatter.Value,
                           DeserializeOptions(options),
                           arguments);
        }

        protected override void Publish(IDictionary<string, object> options, object[] arguments, IDictionary<string, object> argumentsKeywords)
        {
            mTopic.Publish(WampObjectFormatter.Value,
                           DeserializeOptions(options),
                           arguments,
                           argumentsKeywords);
        }

        private static PublishOptions DeserializeOptions(IDictionary<string, object> options)
        {
            PublishOptions result = new PublishOptions();

            foreach (PropertyInfo property in typeof (PublishOptions).GetProperties())
            {
                if (property.IsDefined(typeof (PropertyNameAttribute), true))
                {
                    string name = property.GetCustomAttribute<PropertyNameAttribute>()
                                          .PropertyName;

                    object value;
                    
                    if (options.TryGetValue(name, out value) && 
                        property.PropertyType.IsInstanceOfType(value))
                    {
                        property.SetValue(result, value, null);
                    }
                }
            }

            return result;
        }
    }
}