using System;
using System.Threading.Tasks;
using EdgeJs;
using WampSharp.V2;
using WampSharp.V2.PubSub;
using WampSharp.V2.Realm;

namespace WampSharp.Samples.EdgeJs
{
    class Program
    {
        public static async Task Start()
        {
            DefaultWampHost host = new DefaultWampHost("ws://127.0.0.1:8080/");

            IWampHostedRealm realm = host.RealmContainer.GetRealmByName("realm1");
            realm.SessionCreated += SessionCreated;
            realm.TopicContainer.TopicCreated += TopicCreated;

            host.Open();

            Func<object, Task<object>> createConnection =
                Edge.Func(@"
            var autobahn = require('autobahn');

            return function (options, cb) {

                var connection = new autobahn.Connection({url: options.url, realm: options.realm});
    
connection.onopen = function (session) {

   // 1) subscribe to a topic
   function onevent(args) {
      console.log(""Event:"", args[0]);
   }
   session.subscribe('com.myapp.hello', onevent);

   // 2) publish an event
   session.publish('com.myapp.hello', ['Hello, world!']);

   // 3) register a procedure for remoting
   function add2(args) {
      return args[0] + args[1];
   }
   session.register('com.myapp.add2', add2);

   // 4) call a remote procedure
   session.call('com.myapp.add2', [2, 3]).then(
      function (res) {
         console.log(""Result:"", res);
      }
   );
};

                connection.open();

                cb(null, connection);
            };
        ");

            dynamic result =
                await createConnection(new
                {
                    url = "ws://localhost:8080/",
                    realm = "realm1",
                });

            dynamic result2 =
                await createConnection(new
                {
                    url = "ws://localhost:8080/",
                    realm = "realm1",
                });
        }

        static void TopicCreated(object sender, WampTopicCreatedEventArgs e)
        {
        }

        static void SessionCreated(object sender, WampSessionEventArgs e)
        {
        }

        static void Main(string[] args)
        {
            Task task = Start();
            task.Wait();
            Console.ReadLine();
        }
    }
}
