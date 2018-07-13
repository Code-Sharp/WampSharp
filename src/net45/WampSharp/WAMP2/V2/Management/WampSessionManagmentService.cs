using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.V2.Core;
using WampSharp.V2.Core.Contracts;
using WampSharp.V2.Realm;
using WampSharp.V2.Realm.Binded;

namespace WampSharp.V2.Management
{
    internal class WampSessionManagmentService : IWampSessionManagementService, IDisposable
    {
        private readonly IWampHostedRealm mRealm;
        private readonly IWampUriValidator mUriValidator;

        private readonly object mLock = new object();

        private ImmutableDictionary<long, SessionDetails> mSessionIdToDetails = 
            ImmutableDictionary<long, SessionDetails>.Empty;

        private ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>> mAuthIdToTerminator =
            ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>>.Empty;

        private ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>> mAuthRoleToTerminator =
            ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>>.Empty;

        public WampSessionManagmentService(IWampHostedRealm realm, IWampUriValidator uriValidator = null)
        {
            mRealm = realm;
            mUriValidator = uriValidator ?? new LooseUriValidator();
            mRealm.SessionCreated += OnSessionCreated;
            mRealm.SessionClosed += OnSessionClosed;
        }

        public void KillBySessionId(long session, string reason = WampErrors.CloseNormal, string message = null)
        {
            long? callerId = WampInvocationContext.Current.InvocationDetails.Caller;

            if (!mUriValidator.IsValid(reason))
            {
                throw new WampException(WampErrors.InvalidUri);
            }
            else if ((callerId == session) || !mSessionIdToDetails.TryGetValue(session, out var details))
            {
                throw new WampException(WampErrors.NoSuchSession);
            }
            else
            {
                details.Terminator.Disconnect(new GoodbyeDetails(){Message = message}, reason);
            }
        }

        public int KillByAuthId(string authId, string reason = WampErrors.CloseNormal, string message = null)
        {
            return KillByAuthData(mAuthIdToTerminator, authId, reason, message);
        }

        public int KillByAuthRole(string authRole, string reason = WampErrors.CloseNormal, string message = null)
        {
            return KillByAuthData(mAuthRoleToTerminator, authRole, reason, message);
        }

        private int KillByAuthData(ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>> authMap,
                                   string key, string reason, string message)
        {
            long callerId = (long) WampInvocationContext.Current.InvocationDetails.Caller;

            if (!mUriValidator.IsValid(reason))
            {
                throw new WampException(WampErrors.InvalidUri);
            }

            if (authMap.TryGetValue(key, out var list))
            {
                return KillSessionList(callerId, list, reason, message);
            }
            else
            {
                return 0;
            }
        }

        private static int KillSessionList(long callerId, IEnumerable<IWampSessionTerminator> list, string reason,
                                           string message)
        {
            WampSessionClientTerminator callerTerminator =
                new WampSessionClientTerminator(callerId);

            GoodbyeDetails goodbyeDetails =
                new GoodbyeDetails {Message = message};

            int count = 0;

            foreach (IWampSessionTerminator terminator in list)
            {
                if (!callerTerminator.Equals(terminator))
                {
                    terminator.Disconnect(goodbyeDetails, reason);
                    count++;
                }
            }

            return count;
        }

        public int KillAll(string reason = WampErrors.CloseNormal, string message = null)
        {
            long callerId = (long)WampInvocationContext.Current.InvocationDetails.Caller;

            if (!mUriValidator.IsValid(reason))
            {
                throw new WampException(WampErrors.InvalidUri);
            }

            IEnumerable<IWampSessionTerminator> terminators = 
                mSessionIdToDetails.Values.Select(x => x.Terminator);

            return KillSessionList(callerId, terminators, reason, message);
        }

        private void OnSessionCreated(object sender, WampSessionCreatedEventArgs e)
        {
            lock (mLock)
            {
                IWampSessionTerminator terminator = e.Terminator;
                WelcomeDetails welcomeDetails = e.WelcomeDetails;

                mSessionIdToDetails = mSessionIdToDetails.SetItem(e.SessionId, new SessionDetails(e.SessionId, e.WelcomeDetails, e.Terminator));
                mAuthIdToTerminator = RecalculateAuthenticationDetails(mAuthIdToTerminator, welcomeDetails?.AuthenticationId, terminator);
                mAuthRoleToTerminator = RecalculateAuthenticationDetails(mAuthRoleToTerminator, welcomeDetails?.AuthenticationRole, terminator);
            }
        }

        private ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>> RecalculateAuthenticationDetails(
            ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>> map, string key,
            IWampSessionTerminator terminator)
        {
            var result = map;

            if (key != null)
            {
                if (!map.TryGetValue(key, out var list))
                {
                    list = ImmutableList<IWampSessionTerminator>.Empty;
                }

                result = map.SetItem(key,
                                     list.Add(terminator));
            }

            return result;
        }

        private void OnSessionClosed(object sender, WampSessionCloseEventArgs e)
        {
            long sessionId = e.SessionId;

            lock (mLock)
            {
                if (mSessionIdToDetails.TryGetValue(sessionId, out var details))
                {

                    mSessionIdToDetails = mSessionIdToDetails.Remove(sessionId);

                    WelcomeDetails welcomeDetails = details.WelcomeDetails;

                    mAuthIdToTerminator =
                        TryRemoveAuthData(mAuthIdToTerminator,
                                          welcomeDetails?.AuthenticationId,
                                          details.Terminator);

                    mAuthRoleToTerminator =
                        TryRemoveAuthData(mAuthRoleToTerminator,
                                          welcomeDetails?.AuthenticationRole,
                                          details.Terminator);
                }
            }
        }

        private ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>> TryRemoveAuthData(
            ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>> map, string key, IWampSessionTerminator terminator)
        {
            var result = map;

            if (map.TryGetValue(key, out var list))
            {
                list = list.Remove(terminator);

                if (list.IsEmpty)
                {
                    result = map.Remove(key);
                }
                else
                {
                    result = map.SetItem(key, list);
                }
            }

            return result;
        }

        public void Dispose()
        {
            mRealm.SessionCreated -= OnSessionCreated;
            mRealm.SessionClosed -= OnSessionClosed;

            lock (mLock)
            {
                mSessionIdToDetails = ImmutableDictionary<long, SessionDetails>.Empty;
                mAuthIdToTerminator = ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>>.Empty;
                mAuthRoleToTerminator = ImmutableDictionary<string, ImmutableList<IWampSessionTerminator>>.Empty;
            }
        }

        private class SessionDetails
        {
            public SessionDetails(long session, WelcomeDetails welcomeDetails, IWampSessionTerminator terminator)
            {
                Terminator = terminator;
                Session = session;
                WelcomeDetails = welcomeDetails;
            }

            public IWampSessionTerminator Terminator { get; }
            public long Session { get; }
            public WelcomeDetails WelcomeDetails { get; }
        }
    }
}