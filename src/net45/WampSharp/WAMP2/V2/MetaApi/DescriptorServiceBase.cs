using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using WampSharp.V2.Core.Contracts;

namespace WampSharp.V2.MetaApi
{
    public class DescriptorServiceBase<TDetails> where TDetails : class, IGroupDetailsExtended
    {
        private readonly IDescriptorSubscriber<TDetails> mSubscriber;

        private ImmutableDictionary<string, ImmutableList<TDetails>> mUriToGroups =
            ImmutableDictionary<string, ImmutableList<TDetails>>.Empty;

        private ImmutableDictionary<long, TDetails> mGroupIdToDetails =
            ImmutableDictionary<long, TDetails>.Empty;

        private readonly object mLock = new object();

        protected DescriptorServiceBase(IDescriptorSubscriber<TDetails> subscriber)
        {
            mSubscriber = subscriber;
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

        protected void RemoveGroup(string uri, long groupId, long sessionId)
        {
            TDetails details;

            if (mGroupIdToDetails.TryGetValue(groupId, out details))
            {
                details.RemovePeer(sessionId);

                mSubscriber.OnDelete(sessionId, groupId);

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
            ImmutableList<TDetails> groups;

            if (mUriToGroups.TryGetValue(uri, out groups))
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

        protected long? LookupGroupId(string uri, string match)
        {
            match = match ?? WampMatchPattern.Default;

            ImmutableList<TDetails> groups;

            TDetails result = null;

            if (mUriToGroups.TryGetValue(uri, out groups))
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
            ImmutableList<TDetails> groups;

            if (mUriToGroups.TryGetValue(uri, out groups))
            {
                long[] result = groups.Select(x => x.GroupId).ToArray();

                return result;
            }

            return null;
        }

        protected TDetails GetGroupDetails(long groupId)
        {
            TDetails details;

            if (mGroupIdToDetails.TryGetValue(groupId, out details))
            {
                return details;
            }

            return null;
        }

        protected interface IDescriptorSubscriber<TDetails>
        {
            void OnCreate(long sessionId, TDetails details);

            void OnJoin(long sessionId, long groupId);

            void OnLeave(long sessionId, long groupId);

            void OnDelete(long sessionId, long groupId);
        }
    }
}