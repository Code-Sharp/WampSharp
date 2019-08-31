using System;
using System.Collections.Generic;
using WampSharp.V2;
using WampSharp.V2.Authentication;
using WampSharp.V2.Realm;
using WampSharp.V2.Rpc;

namespace WampSharp.Samples.WampCra.Router
{
    public class Program
    {
        private static void Main(string[] args)
        {
            HostCode();
            Console.ReadLine();
        }

        private static void HostCode()
        {
            DefaultWampAuthenticationHost host =
                new DefaultWampAuthenticationHost("ws://127.0.0.1:8080/ws",
                                                  new WampCraUserDbAuthenticationFactory(new MyAuthenticationProvider(),
                                                                                         new MyUserDb()));

            IWampHostedRealm realm = host.RealmContainer.GetRealmByName("realm1");

            string[] topics = new[]
                              {
                                  "com.example.topic1",
                                  "com.foobar.topic1",
                                  "com.foobar.topic2"
                              };

            foreach (string topic in topics)
            {
                string currentTopic = topic;

                realm.Services.GetSubject<string>(topic).Subscribe
                    (x => Console.WriteLine("event received on {0}: {1}", currentTopic, x));
            }

            realm.Services.RegisterCallee(new Add2Service()).Wait();

            host.Open();
        }

        public class Add2Service : IAdd2Service
        {
            public int Add2(int x, int y)
            {
                return (x + y);
            }
        }

        public interface IAdd2Service
        {
            [WampProcedure("com.example.add2")]
            int Add2(int a, int b);
        }

        private class MyAuthenticationProvider : WampStaticAuthenticationProvider
        {
            public MyAuthenticationProvider() :
                base(new Dictionary<string, IDictionary<string, WampAuthenticationRole>>()
                     {
                         ["realm1"] =
                             new Dictionary<string, WampAuthenticationRole>()
                             {
                                 ["frontend"] =
                                     new WampAuthenticationRole
                                     {
                                         Authorizer =
                                             new WampStaticAuthorizer
                                                 (new List<WampUriPermissions>
                                                  {
                                                      new WampUriPermissions
                                                      {
                                                          Uri = "com.example.add2",
                                                          CanCall = true
                                                      },
                                                      new WampUriPermissions
                                                      {
                                                          Uri = "com.example.",
                                                          Prefixed = true,
                                                          CanPublish = true
                                                      },
                                                      new WampUriPermissions
                                                      {
                                                          Uri = "com.example.topic2",
                                                          CanPublish = false
                                                      },
                                                      new WampUriPermissions
                                                      {
                                                          Uri = "com.foobar.topic1",
                                                          CanPublish = true
                                                      },
                                                  })
                                     }
                             }
                     })
            {
            }
        }

        private class MyUserDb : WampCraStaticUserDb
        {
            public MyUserDb() : 
                base(new Dictionary<string, WampCraUser>
                                     {
                                         ["joe"] = new WampCraUser()
                                                   {
                                                       Secret = "secret2",
                                                       AuthenticationRole = "frontend"
                                                   },
                                         ["peter"] = new WampCraUser()
                                                     {
                                                         Secret = "prq7+YkJ1/KlW1X0YczMHw==",
                                                         AuthenticationRole = "frontend",
                                                         Salt = "salt123",
                                                         Iterations = 100,
                                                         KeyLength = 16
                                                     }
                                     })
            {
            }
        }
    }
}