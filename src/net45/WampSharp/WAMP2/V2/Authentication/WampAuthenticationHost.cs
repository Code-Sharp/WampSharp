using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WampSharp.Core.Listener;
using WampSharp.Core.Message;
using WampSharp.Core.Serialization;
using WampSharp.V2.Authentication;
using WampSharp.V2.Binding;
using WampSharp.V2.Binding.Transports;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Session;
using WampSharp.V2.Transports;

namespace WampSharp.V2
{
    public class WampAuthenticationHost : WampHost
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public WampAuthenticationHost(IWampSessionAuthenticatorFactory sessionAuthenticationFactory,
                                      IWampRealmContainer realmContainer = null)
            : base(realmContainer)
        {
            if (sessionAuthenticationFactory == null)
            {
                throw new ArgumentNullException("sessionAuthenticationFactory");
            }

            mSessionAuthenticationFactory = sessionAuthenticationFactory;
        }

        public override void RegisterTransport(IWampTransport transport, IEnumerable<IWampBinding> bindings)
        {
            IEnumerable<IWampBinding> authenticationBindings =
                bindings.Select(binding => CreateAuthenticationBinding((dynamic) binding))
                        .Cast<IWampBinding>()
                        .Where(x => x != null);

            base.RegisterTransport(transport, authenticationBindings);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampTextBinding<TMessage> binding)
        {
            return new WampAuthenticationTextBinding<TMessage>(binding, mSessionAuthenticationFactory);
        }

        private IWampBinding CreateAuthenticationBinding<TMessage>
            (IWampBinaryBinding<TMessage> binding)
        {
            return new WampAuthenticationBinaryBinding<TMessage>(binding, mSessionAuthenticationFactory);
        }

        /// <summary>
        /// Fallback in case that binding doesn't implement
        /// IWampBinding{TMessage}
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        private IWampBinding CreateAuthenticationBinding(IWampBinding binding)
        {
            return null;
        }
    }

    internal abstract class WampAuthenticationBinding<TMessage> : IWampRouterBinding<TMessage>
    {
        private readonly IWampBinding<TMessage> mBinding;
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public WampAuthenticationBinding(IWampBinding<TMessage> binding,
                                         IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            mBinding = binding;

            mSessionAuthenticationFactory = 
                new RestrictedSessionAuthenticationFactory(sessionAuthenticationFactory);
        }

        public IWampBindingHost CreateHost(IWampHostedRealmContainer realmContainer, IWampConnectionListener<TMessage> connectionListener)
        {
            IWampRouterBuilder wampRouterBuilder = new WampAuthenticationRouterBuilder(mSessionAuthenticationFactory);

            return new WampBindingHost<TMessage>(realmContainer,
                                                 wampRouterBuilder,
                                                 connectionListener,
                                                 mBinding);
        }

        public string Name
        {
            get
            {
                return mBinding.Name;
            }
        }

        public WampMessage<object> GetRawMessage(WampMessage<object> message)
        {
            return mBinding.GetRawMessage(message);
        }

        public IWampFormatter<TMessage> Formatter
        {
            get
            {
                return mBinding.Formatter;
            }
        }
    }

    internal class RestrictedSessionAuthenticationFactory : IWampSessionAuthenticatorFactory
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public RestrictedSessionAuthenticationFactory(IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            mSessionAuthenticationFactory = sessionAuthenticationFactory;
        }

        public IWampSessionAuthenticator GetSessionAuthenticator
            (PendingClientDetails details,
             IWampSessionAuthenticator transportAuthenticator)
        {
            IWampSessionAuthenticator result =
                mSessionAuthenticationFactory.GetSessionAuthenticator
                    (details,
                     transportAuthenticator);

            if (result == null)
            {
                return null;
            }
            
            return new RestrictedSessionAuthenticator(result);
        }
    }

    internal class RestrictedSessionAuthenticator : IWampSessionAuthenticator
    {
        private readonly IWampSessionAuthenticator mAuthenticator;
        private readonly RestrictedAuthorizer mAuthorizer;

        public RestrictedSessionAuthenticator(IWampSessionAuthenticator authenticator)
        {
            mAuthenticator = authenticator;
            mAuthorizer = new RestrictedAuthorizer(mAuthenticator);
        }

        public bool IsAuthenticated
        {
            get
            {
                return mAuthenticator.IsAuthenticated;
            }
        }

        public string AuthenticationId
        {
            get
            {
                return mAuthenticator.AuthenticationId;
            }
        }

        public string AuthenticationMethod
        {
            get
            {
                return mAuthenticator.AuthenticationMethod;
            }
        }

        public ChallengeDetails ChallengeDetails
        {
            get
            {
                return mAuthenticator.ChallengeDetails;
            }
        }

        public void Authenticate(string signature, AuthenticateExtraData extra)
        {
            mAuthenticator.Authenticate(signature, extra);
        }

        public IWampAuthorizer Authorizer
        {
            get
            {
                return mAuthorizer;
            }
        }

        public WelcomeDetails WelcomeDetails
        {
            get { return mAuthenticator.WelcomeDetails; }
        }
    }

    internal class RestrictedAuthorizer : IWampAuthorizer
    {
        private readonly IWampSessionAuthenticator mAuthenticator;

        public RestrictedAuthorizer(IWampSessionAuthenticator authenticator)
        {
            mAuthenticator = authenticator;
        }

        public bool CanRegister(RegisterOptions options, string procedure)
        {
            if (procedure.StartsWith("wamp.", StringComparison.Ordinal))
            {
                return false;
            }
            else
            {
                return Authorizer.CanRegister(options, procedure);                
            }
        }

        public bool CanCall(CallOptions options, string procedure)
        {
            return Authorizer.CanCall(options, procedure);
        }

        public bool CanPublish(PublishOptions options, string topicUri)
        {
            if (topicUri.StartsWith("wamp.", StringComparison.Ordinal))
            {
                return false;
            }
            else
            {
                return Authorizer.CanPublish(options, topicUri);
            }
        }

        public bool CanSubscribe(SubscribeOptions options, string topicUri)
        {
            return Authorizer.CanSubscribe(options, topicUri);
        }

        public IWampAuthorizer Authorizer
        {
            get
            {
                return mAuthenticator.Authorizer;
            }
        }
    }

    internal class WampAuthenticationBinaryBinding<TMessage> : WampAuthenticationBinding<TMessage>,
        IWampBinaryBinding<TMessage>
    {
        private readonly IWampBinaryBinding<TMessage> mBinding;

        public WampAuthenticationBinaryBinding(IWampBinaryBinding<TMessage> binding,
                                               IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : base(binding, sessionAuthenticationFactory)
        {
            mBinding = binding;
        }

        public WampMessage<TMessage> Parse(byte[] raw)
        {
            return mBinding.Parse(raw);
        }

        public byte[] Format(WampMessage<object> message)
        {
            return mBinding.Format(message);
        }

        public WampMessage<TMessage> Parse(Stream stream)
        {
            return mBinding.Parse(stream);
        }

        public void Format(WampMessage<object> message, Stream stream)
        {
            mBinding.Format(message, stream);
        }
    }

    internal class WampAuthenticationTextBinding<TMessage> : WampAuthenticationBinding<TMessage>,
        IWampTextBinding<TMessage>
    {
        private readonly IWampTextBinding<TMessage> mBinding;

        public WampAuthenticationTextBinding(IWampTextBinding<TMessage> binding,
                                             IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
            : base(binding, sessionAuthenticationFactory)
        {
            mBinding = binding;
        }

        public WampMessage<TMessage> Parse(string raw)
        {
            return mBinding.Parse(raw);
        }

        public string Format(WampMessage<object> message)
        {
            return mBinding.Format(message);
        }

        public WampMessage<TMessage> Parse(Stream stream)
        {
            return mBinding.Parse(stream);
        }

        public void Format(WampMessage<object> message, Stream stream)
        {
            mBinding.Format(message, stream);
        }
    }

    internal class WampAuthenticationRouterBuilder : WampRouterBuilder
    {
        private readonly IWampSessionAuthenticatorFactory mSessionAuthenticationFactory;

        public WampAuthenticationRouterBuilder(IWampSessionAuthenticatorFactory sessionAuthenticationFactory)
        {
            mSessionAuthenticationFactory = sessionAuthenticationFactory;
        }

        public override IWampSessionServer<TMessage> CreateSessionHandler<TMessage>
            (IWampHostedRealmContainer realmContainer,
             IWampBinding<TMessage> binding,
             IWampEventSerializer eventSerializer)
        {
            return new WampAuthenticationSessionServer<TMessage>
                (binding,
                 realmContainer,
                 this,
                 eventSerializer,
                 mSessionAuthenticationFactory);
        }

        public override IWampServer<TMessage> CreateServer<TMessage>(IWampSessionServer<TMessage> session, IWampDealer<TMessage> dealer, IWampBroker<TMessage> broker)
        {
            return new WampAuthenticationServer<TMessage>(session, dealer, broker);
        }
    }


    public abstract class WampSessionAuthenticator : IWampSessionAuthenticator
    {
        private static readonly ChallengeDetails mEmptyChallengeDetails = new ChallengeDetails();
        private ChallengeDetails mChallengeDetails;

        public WampSessionAuthenticator()
        {
            mChallengeDetails = mEmptyChallengeDetails;
        }

        public virtual bool IsAuthenticated { get; protected set; }
        
        public virtual IWampAuthorizer Authorizer { get; protected set; }

        public abstract void Authenticate(string signature, AuthenticateExtraData extra);

        public abstract string AuthenticationId { get; }
        
        public abstract string AuthenticationMethod { get; }

        public virtual ChallengeDetails ChallengeDetails
        {
            get
            {
                return mChallengeDetails;
            }
            protected set
            {
                mChallengeDetails = value;
            }
        }

        public virtual WelcomeDetails WelcomeDetails
        {
            get;
            protected set;
        }
    }

    public abstract class WampSessionAuthenticator<TExtra> : WampSessionAuthenticator
    {
        public override void Authenticate(string signature, AuthenticateExtraData extra)
        {
            TExtra deserialized = extra.OriginalValue.Deserialize<TExtra>();

            Authenticate(signature, deserialized);
        }

        protected abstract void Authenticate(string signature, TExtra extra);
    }
}