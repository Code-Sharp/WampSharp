using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.MetaApi
{
    internal class DescriptorServiceBase<TDetails> where TDetails : class, IGroupDetailsExtended
    {
        private readonly IDescriptorSubscriber<TDetails> mSubscriber;

        private ImmutableDictionary<string, ImmutableList<TDetails>> mUriToGroups =
            ImmutableDictionary<string, ImmutableList<TDetails>>.Empty;

        private ImmutableDictionary<long, TDetails> mGroupIdToDetails =
            ImmutableDictionary<long, TDetails>.Empty;

        private readonly object mLock = new object();
        private readonly string mMissingErrorUri;

        protected DescriptorServiceBase(IDescriptorSubscriber<TDetails> subscriber, string missingErrorUri)
        {
            mSubscriber = subscriber;
            mMissingErrorUri = missingErrorUri;
        }

        protected void AddPeer(long sessionId, long groupId, Func<TDetails> detailsCreator)
        {
            lock (mLock)
            {
                TDetails detailsExtended =
                    ImmutableInterlocked.GetOrAdd
                        (ref mGroupIdToDetails,
                         groupId,
                         x => detailsCreator());

                detailsExtended.AddPeer(sessionId);
            }

            mSubscriber.OnJoin(sessionId, groupId);
        }

        protected void AddGroup(string uri, long sessionId, TDetails groupDetails)
        {
            mSubscriber.OnCreate(sessionId, groupDetails);

            var groups =
                ImmutableInterlocked.GetOrAdd(ref mUriToGroups,
                                              uri,
                                              x => ImmutableList<TDetails>.Empty);

            mUriToGroups =
                mUriToGroups.SetItem(uri, groups.Add(groupDetails));
        }

        protected void RemovePeerFromGroup(string uri, long sessionId, long groupId)
        {

            if (mGroupIdToDetails.TryGetValue(groupId, out TDetails details))
            {
                details.RemovePeer(sessionId);

                mSubscriber.OnLeave(sessionId, groupId);

                if (details.Peers.Count == 0)
                {
                    lock (mLock)
                    {
                        if (details.Peers.Count == 0)
                        {
                            DeleteGroup(uri, sessionId, groupId, details);
                        }
                    }
                }
            }
        }

        private void DeleteGroup(string uri,
                                 long sessionId,
                                 long groupId,
                                 TDetails details)
        {
            mGroupIdToDetails =
                mGroupIdToDetails.Remove(groupId);

            mSubscriber.OnDelete(sessionId, groupId);

            DeleteUriToGroup(uri, details);
        }

        private void DeleteUriToGroup(string uri, TDetails details)
        {

            if (mUriToGroups.TryGetValue(uri, out ImmutableList<TDetails> groups))
            {
                groups = groups.Remove(details);

                if (groups.Count != 0)
                {
                    mUriToGroups =
                        mUriToGroups.SetItem(uri, groups);
                }
                else
                {
                    mUriToGroups =
                        mUriToGroups.Remove(uri);
                }
            }
        }

        protected Dictionary<string, long[]> GetMatchToGroupId()
        {
            Dictionary<string, long[]> matchToGroupId =
                mGroupIdToDetails.Values.GroupBy(x => x.Match, x => x.GroupId)
                                 .ToDictionary(x => x.Key, x => x.ToArray());

            return matchToGroupId;
        }

        protected AvailableGroups GetAllGroupIds()
        {
            Dictionary<string, long[]> matchToGroupId = GetMatchToGroupId();

            AvailableGroups result = new AvailableGroups();


            // Yuck!
            if (matchToGroupId.TryGetValue(WampMatchPattern.Exact, out long[] groups))
            {
                result.Exact = groups;
            }
            if (matchToGroupId.TryGetValue(WampMatchPattern.Prefix, out groups))
            {
                result.Prefix = groups;
            }
            if (matchToGroupId.TryGetValue(WampMatchPattern.Wildcard, out groups))
            {
                result.Wildcard = groups;
            }

            return result;
        }

        protected long? LookupGroupId(string uri, string match)
        {
            match = match ?? WampMatchPattern.Default;


            TDetails result = null;

            if (mUriToGroups.TryGetValue(uri, out ImmutableList<TDetails> groups))
            {
                result = groups.FirstOrDefault(x => x.Match == match);
            }

            if (result != null)
            {
                return result.GroupId;
            }

            return null;
        }

        protected long[] GetMatchingGroupIds(string uri)
        {
            return GetMatchingGroups(uri)
                .Select(x => x.GroupId).ToArray();
        }

        protected IEnumerable<TDetails> GetMatchingGroups(string uri)
        {

            if (mUriToGroups.TryGetValue(uri, out ImmutableList<TDetails> groups))
            {
                return groups;
            }

            throw new WampException(mMissingErrorUri);
        }

        protected TDetails GetGroupDetails(long groupId)
        {

            if (mGroupIdToDetails.TryGetValue(groupId, out TDetails details))
            {
                return details;
            }

            throw new WampException(mMissingErrorUri);
        }

        protected long[] GetPeersIds(long groupId)
        {
            TDetails details = GetGroupDetails(groupId);

            if (details != null)
            {
                return details.Peers.ToArray();
            }

            throw new WampException(mMissingErrorUri);
        }

        public long CountPeers(long groupId)
        {
            TDetails details = GetGroupDetails(groupId);

            if (details != null)
            {
                return details.Peers.Count;
            }

            throw new WampException(mMissingErrorUri);
        }

        protected interface IDescriptorSubscriber<TSubscriberDetails>
        {
            void OnCreate(long sessionId, TSubscriberDetails details);

            void OnJoin(long sessionId, long groupId);

            void OnLeave(long sessionId, long groupId);

            void OnDelete(long sessionId, long groupId);
        }
    }
}