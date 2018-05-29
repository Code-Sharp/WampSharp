using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using WampSharp.V2.Authentication;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.V2.MetaApi
{
    internal class RegistrationDescriptorService : 
        DescriptorServiceBase<RegistrationDetailsExtended>,
        IWampRegistrationDescriptor
    {
        private readonly IDisposable mDisposable;
        private readonly IWampRpcOperationCatalog mOperationCatalog;

        public RegistrationDescriptorService(IWampRealm realm) : 
            base(new RegistrationMetadataSubscriber(realm.TopicContainer), WampErrors.NoSuchRegistration)
        {
            IWampRpcOperationCatalog rpcCatalog = realm.RpcCatalog;

            IObservable<IWampProcedureRegistration> removed = 
                GetRegistrationRemoved(rpcCatalog);

            var observable =
                from registration in GetRegistrationAdded(rpcCatalog)
                let registrationRemoved = removed.Where(x => x == registration)
                let calleeRegistered = GetCalleeRegistered(registration, registrationRemoved)
                let calleeUnregistered = GetCalleeUnregistered(registration, registrationRemoved)
                select new { registration, calleeRegistered, calleeUnregistered};

            var addObservable =
                from item in observable
                from operation in item.calleeRegistered
                select new { Registration = item.registration, Operation = operation };

            var removeObservable =
                from item in observable
                from operation in item.calleeUnregistered
                select new { Registration = item.registration, Operation = operation };

            IDisposable addDisposable = addObservable.Subscribe(x => OnRegistrationAdded(x.Registration, x.Operation));
            IDisposable removeDisposable = removeObservable.Subscribe(x => OnRegistrationRemoved(x.Registration, x.Operation));

            mDisposable = new CompositeDisposable(addDisposable, removeDisposable);

            mOperationCatalog = rpcCatalog;
        }

        private void OnRegistrationAdded(IWampProcedureRegistration registration, IRemoteWampCalleeOperation operation)
        {
            AddPeer(operation.SessionId, registration.RegistrationId,
                    () => GetRegistrationDetails(registration, operation.SessionId));
        }

        private RegistrationDetailsExtended GetRegistrationDetails(IWampProcedureRegistration registration, long sessionId)
        {
            string procedureUri = registration.Procedure;

            RegistrationDetailsExtended result = new RegistrationDetailsExtended()
            {
                Created = DateTime.Now,
                Uri = procedureUri,
                Match = registration.RegisterOptions.Match,
                RegistrationId = registration.RegistrationId,
                Invoke = registration.RegisterOptions.Invoke
            };

            // TODO: if we can ignore the session id, there is no need for the special
            // TODO: IRemoteWampCalleeOperation interface
            AddGroup(procedureUri, sessionId, result);

            return result;
        }

        private void OnRegistrationRemoved(IWampProcedureRegistration registration, IRemoteWampCalleeOperation operation)
        {
            RemovePeerFromGroup(registration.Procedure,
                                operation.SessionId,
                                registration.RegistrationId);
        }

        private IObservable<IRemoteWampCalleeOperation> GetCalleeRegistered
            (IWampProcedureRegistration registration,
             IObservable<IWampProcedureRegistration> registrationRemoved)
        {
            return Observable.FromEventPattern<WampCalleeAddEventArgs>
                (x => registration.CalleeRegistered += x,
                 x => registration.CalleeRegistered -= x)
                             .Select(x => x.EventArgs.Operation)
                             .OfType<IRemoteWampCalleeOperation>()
                             .TakeUntil(registrationRemoved);
        }

        private IObservable<IRemoteWampCalleeOperation> GetCalleeUnregistered
            (IWampProcedureRegistration registration,
             IObservable<IWampProcedureRegistration> registrationRemoved)
        {
            return Observable.FromEventPattern<WampCalleeRemoveEventArgs>
                (x => registration.CalleeUnregistered += x,
                 x => registration.CalleeUnregistered -= x)
                             .Select(x => x.EventArgs.Operation)
                             .OfType<IRemoteWampCalleeOperation>()
                             .TakeUntil(registrationRemoved);
        }

        private IObservable<IWampProcedureRegistration> GetRegistrationRemoved(IWampRpcOperationCatalog rpcCatalog)
        {
            return Observable.FromEventPattern<WampProcedureRegisterEventArgs>
                (x => rpcCatalog.RegistrationRemoved += x,
                 x => rpcCatalog.RegistrationRemoved -= x)
                             .Select(x => x.EventArgs.Registration)
                             .Where(x => !WampRestrictedUris.IsRestrictedUri(x.Procedure));
        }

        private static IObservable<IWampProcedureRegistration> GetRegistrationAdded(IWampRpcOperationCatalog rpcCatalog)
        {
            return Observable.FromEventPattern<WampProcedureRegisterEventArgs>
                (x => rpcCatalog.RegistrationAdded += x,
                 x => rpcCatalog.RegistrationAdded -= x)
                             .Select(x => x.EventArgs.Registration)
                             .Where(x => !WampRestrictedUris.IsRestrictedUri(x.Procedure));
        }

        public AvailableGroups GetAllRegistrations()
        {
            return GetAllGroupIds();
        }

        public long? LookupRegistrationId(string procedureUri, RegisterOptions options = null)
        {
            string match = null;

            if (options != null)
            {
                match = options.Match;
            }

            return base.LookupGroupId(procedureUri, match);
        }

        public long? GetBestMatchingRegistrationId(string procedureUri)
        {

            if (!(mOperationCatalog.GetMatchingOperation(procedureUri) is IWampProcedureRegistration registration))
            {
                return null;
            }

            return registration.RegistrationId;
        }

        public RegistrationDetails GetRegistrationDetails(long registrationId)
        {
            return base.GetGroupDetails(registrationId);
        }

        public long[] GetCalleesIds(long registrationId)
        {
            return base.GetPeersIds(registrationId);
        }

        public long CountCallees(long registrationId)
        {
            return base.CountPeers(registrationId);
        }

        public void Dispose()
        {
            mDisposable.Dispose();
        }

        private class RegistrationMetadataSubscriber : 
            ManualSubscriber<IWampRegistrationMetadataSubscriber>, 
            IWampRegistrationMetadataSubscriber,
            IDescriptorSubscriber<RegistrationDetailsExtended>
        {
            private readonly IWampTopicContainer mTopicContainer;
            private readonly PublishOptions mPublishOptions = new PublishOptions();
            private static readonly string mOnCreateTopicUri = GetTopicUri(subscriber => subscriber.OnCreate(default(long), default(RegistrationDetails)));
            private static readonly string mOnRegisterTopicUri = GetTopicUri(subscriber => subscriber.OnRegister(default(long), default(long)));
            private static readonly string mOnUnregisterTopicUri = GetTopicUri(subscriber => subscriber.OnUnregister(default(long), default(long)));
            private static readonly string mOnDeleteTopicUri = GetTopicUri(subscriber => subscriber.OnDelete(default(long), default(long)));

            public RegistrationMetadataSubscriber(IWampTopicContainer topicContainer)
            {
                mTopicContainer = topicContainer;
                mTopicContainer.CreateTopicByUri(mOnCreateTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnRegisterTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnUnregisterTopicUri, true);
                mTopicContainer.CreateTopicByUri(mOnDeleteTopicUri, true);
            }

            public void OnCreate(long sessionId, RegistrationDetails details)
            {
                mTopicContainer.Publish(mPublishOptions, mOnCreateTopicUri, sessionId, details);
            }

            public void OnRegister(long sessionId, long registrationId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnRegisterTopicUri, sessionId, registrationId);
            }

            public void OnUnregister(long sessionId, long registrationId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnUnregisterTopicUri, sessionId, registrationId);
            }

            public void OnDelete(long sessionId, long registrationId)
            {
                mTopicContainer.Publish(mPublishOptions, mOnDeleteTopicUri, sessionId, registrationId);
            }

            void IDescriptorSubscriber<RegistrationDetailsExtended>.OnCreate(long sessionId, RegistrationDetailsExtended details)
            {
                OnCreate(sessionId, details);
            }

            void IDescriptorSubscriber<RegistrationDetailsExtended>.OnJoin(long sessionId, long groupId)
            {
                OnRegister(sessionId, groupId);
            }

            void IDescriptorSubscriber<RegistrationDetailsExtended>.OnLeave(long sessionId, long groupId)
            {
                OnUnregister(sessionId, groupId);
            }

            void IDescriptorSubscriber<RegistrationDetailsExtended>.OnDelete(long sessionId, long groupId)
            {
                OnDelete(sessionId, groupId);
            }
        }
    }
}