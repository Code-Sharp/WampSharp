WampSharp v1.2.2.8-beta release notes
=================================

**Contents**

1. [Breaking changes](#breaking-changes)
	* [Library split up](#library-split-up)
	 * [WampSharp.Default split up](#wampsharpdefault-split-up)
	 * [WAMPv1 split up](#wampv1-split-up)
2. [New features](#new-features)
    * [Portable Class Library Support](#portable-class-library-support)
    * [Logging support](#logging-support)
    * [WampChannelReconnector](#wampchannelreconnector)
    * [WAMP-CRA authentication](#wamp-cra-authentication)
    * [Pattern based subscriptions](#pattern-based-subscriptions)
    * [Shared registrations](#shared-registrations)
    * [Pattern based registrations](#pattern-based-registrations)
3. [Internal changes](#internal-changes)
	* [Performance improvements](#performance-improvements)
	* [Bug fixes](#bug-fixes)

###Breaking changes

####Library split up

##### WampSharp.Default split up

WampSharp.Default.dll has been split up into a couple of libraries. This allows you to consume only the libraries you are interested in:

* WampSharp.Fleck - This NuGet package contains WampSharp implementation of a WebSocket transport using [Fleck](https://github.com/statianzo/Fleck).
* WampSharp.NewtonsoftJson - This NuGet package contains WampSharp support for Json serialization using [Newtonsoft Json](http://www.newtonsoft.com/json)
* WampSharp.NewtonsoftMsgpack - This NuGet package contains WampSharp support for MsgPack serialization using [Newtonsoft.Msgpack](https://github.com/Code-Sharp/Newtonsoft.Msgpack) (which internally uses [msgpack-cli](http://cli.msgpack.org/) and [Newtonsoft Json](http://www.newtonsoft.com/json))
* WampSharp.WebSocket4Net - This NuGet package contains WampSharp implementation of a WebSocket client using [WebSocket4Net](https://github.com/kerryjiang/WebSocket4Net).

In addition the following packages exists:

* WampSharp.Default.Client - adds DefaultWampChannelFactory which uses WampSharp.WebSocket4Net, WampSharp.NewtonsoftJson and WampSharp.NewtonsoftMsgpack
* WampSharp.Default.Router - adds DefaultWampHost which uses WampSharp.Fleck, WampSharp.NewtonsoftJson and WampSharp.NewtonsoftMsgpack.
* WampSharp.Default - references both WampSharp.Default.Client and WampSharp.Default.Router.

> Note: You don't have to consume the WampSharp.Default.Client/WampSharp.Default.Router libraries, you can always use directly [WampChannel](https://github.com/Code-Sharp/WampSharp/wiki/WampChannel) or [WampHost](https://github.com/Code-Sharp/WampSharp/wiki/WampHost) if you're interested only in some of the dependencies.

##### WAMPv1 split up

From this version, WAMPv1 support has been moved to a dedicated dll name WampSharp.WAMP1.dll. The types DefaultWampHost, DefaultWampCraHost and DefaultWampChannelFactory (and some extension methods of IWampChannelFactory) are located in WampSharp.WAMP1.Default.dll.

In order to update a WAMP1 application to this version, please uninstall WampSharp.Default and install WampSharp.WAMP1.Default instead.

Please also note that [WAMPv1 is deprecated](https://groups.google.com/forum/#!msg/autobahnws/k-Jo8NnFtjA/qxnmFp2qGkMJ), and you're encouraged to upgrade your application to WAMPv2.

### New features

####Portable Class Library Support
From this version, Portable Class Library is supported (to be precise, Windows Phone 8.1 and Windows 8.1 platforms).

In order to use WampSharp in these platforms, simply install WampSharp.Default.Client and use DefaultWampChannelFactory as usual. Note that currently only Json serialization is supported in these platforms.

The WampSharp implementation of a WebSocket client for these platforms is located in WampSharp.Windows and is called MessageWebSocketTextConnection (based on the [MessageWebSocket class](https://msdn.microsoft.com/library/windows/apps/br226842)).

####Logging support

From this version, some logs are written by the library. Logging is supported by the [LibLog](https://github.com/damianh/LibLog) project. In order to enable logs, just install your favorite logging library that [LibLog supports](https://github.com/damianh/LibLog/wiki), and configure it. Logs will be written to "WampSharp." prefixed loggers automatically.

This is an initial logs works. You are welcome to request some other logs from this library - please comment [here](https://github.com/Code-Sharp/WampSharp/issues/6) about logs that you are interested in.

####WampChannelReconnector

Available from WampSharp v1.2.1.7-beta, defines a mechanism for reconnecting to a remote router.

In order to use it, create an instance of WampChannelReconnector, and pass to it a delegate that will be triggered everytime a channel gets connected to the remote router.

Example:

```csharp
public static async Task Run()
{
    DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

    string address = "ws://localhost:8080/ws";

    MySubscriber mySubscriber = new MySubscriber();

    IWampChannel channel =
        factory.CreateJsonChannel(address, "realm1");

    Func<Task> connect = async () =>
    {
        await channel.Open();

        var subscriptionTask =
            channel.RealmProxy.Services.RegisterSubscriber(mySubscriber);

        var asyncDisposable = await subscriptionTask;
    };

    WampChannelReconnector reconnector =
        new WampChannelReconnector(channel, connect);

    reconnector.Start();
}

```

####WAMP-CRA Authentication

[WAMP-CRA](http://crossbar.io/docs/WAMP-CRA-Authentication/) client side authentication is now supported. In order to use it, instantiate a new instance of WampCraAuthenticator and pass it to the channel factory.
Example:

```csharp
public async Task Run()
{
    DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

    IWampClientAuthenticator authenticator;

    if (false)
    {
        authenticator = new WampCraClientAuthenticator(authenticationId: "joe", authenticationKey: "secret2");
    }
    else
    {
        authenticator = 
            new WampCraClientAuthenticator(authenticationId: "peter", secret: "secret1", salt: "salt123", iterations: 100, keyLen: 16);
    }

    IWampChannel channel =
        channelFactory.CreateJsonChannel("ws://127.0.0.1:8080/ws",
            "realm1",
            authenticator);

    channel.RealmProxy.Monitor.ConnectionEstablished +=
        (sender, args) =>
        {
            Console.WriteLine("connected session with ID " + args.SessionId);

            dynamic details = args.Details.Deserialize<dynamic>();

            Console.WriteLine("authenticated using method '{0}' and provider '{1}'", details.authmethod,
                              details.authprovider);
            
            Console.WriteLine("authenticated with authid '{0}' and authrole '{1}'", details.authid,
                              details.authrole);
        };

    channel.RealmProxy.Monitor.ConnectionBroken += (sender, args) =>
    {
        dynamic details = args.Details.Deserialize<dynamic>();
        Console.WriteLine("disconnected " + args.Reason + " " + details.reason + details);
    };

    IWampRealmProxy realmProxy = channel.RealmProxy;

    await channel.Open().ConfigureAwait(false);

    // call a procedure we are allowed to call (so this should succeed)
    //
    IAdd2Service proxy = realmProxy.Services.GetCalleeProxy<IAdd2Service>();

    try
    {
        var five = await proxy.Add2Async(2, 3)
            .ConfigureAwait(false);

        Console.WriteLine("call result {0}", five);
    }
    catch (Exception e)
    {
        Console.WriteLine("call error {0}", e);
    }

    // (try to) register a procedure where we are not allowed to (so this should fail)
    //
    Mul2Service service = new Mul2Service();

    try
    {
        await realmProxy.Services.RegisterCallee(service)
            .ConfigureAwait(false);

        Console.WriteLine("huh, function registered!");
    }
    catch (WampException ex)
    {
        Console.WriteLine("registration failed - this is expected: " + ex.ErrorUri);
    }

    // (try to) publish to some topics
    //
    string[] topics = {
        "com.example.topic1",
        "com.example.topic2",
        "com.foobar.topic1",
        "com.foobar.topic2"
    };


    foreach (string topic in topics)
    {
        IWampTopicProxy topicProxy = realmProxy.TopicContainer.GetTopicByUri(topic);

        try
        {
            await topicProxy.Publish(new PublishOptions() { Acknowledge = true })
                .ConfigureAwait(false);

            Console.WriteLine("event published to topic " + topic);
        }
        catch (WampException ex)
        {
            Console.WriteLine("publication to topic " + topic + " failed: " + ex.ErrorUri);
        }
    }
}


public interface IAdd2Service
{
    [WampProcedure("com.example.add2")]
    Task<int> Add2Async(int x, int y);
}

public class Mul2Service
{
    [WampProcedure("com.example.mul2")]
    public int Multiply2(int x, int y)
    {
        return x*y;
    }
}
```

>Note:  The sample is based on [this](https://github.com/crossbario/crossbarexamples/tree/master/authenticate/wampcra) AutobahnJS sample

#### Pattern based subscriptions

This version has support for [pattern based subscriptions](http://crossbar.io/docs/Pattern-Based-Subscriptions/), for both router side and client side.

In order to use it from router side, you need to do nothing - WampSharp router implementation supports pattern based subscriptions. If you want to publish events from the router side so that they will be sent for all matching subscriptions, use one of the Realm Services publication methods, or use TopicContainer.Publish method. (IWampTopic methods publish to a specific subscription and therefore ignore other possible matching subscriptions)

In order to use it from client side, pass a SubscribeOptions with Match = "exact"/"prefix"/"wildcard" depending on your criteria:

```csharp
public async Task Run()
{
    DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

    IWampChannel channel =
        channelFactory.CreateJsonChannel("ws://127.0.0.1:8080/ws",
            "realm1");

    await channel.Open().ConfigureAwait(false);

    await channel.RealmProxy.Services.RegisterSubscriber(new Subscriber1())
        .ConfigureAwait(false);

    await channel.RealmProxy.Services.RegisterSubscriber
        (new Subscriber2(),
         new SubscriberRegistrationInterceptor(new SubscribeOptions
         {
             Match = "prefix"
         }))
         .ConfigureAwait(false);

    await channel.RealmProxy.Services.RegisterSubscriber
        (new Subscriber3(),
         new SubscriberRegistrationInterceptor(new SubscribeOptions
         {
             Match = "wildcard"
         }))
         .ConfigureAwait(false);
}

public class Subscriber1
{
    [WampTopic("com.example.topic1")]
    public void Handler1(string message)
    {
        Console.WriteLine("handler1: msg = '{0}', topic = '{1}'", message,
                          WampEventContext.Current.EventDetails.Topic);
    }
}

public class Subscriber2
{
    [WampTopic("com.example")]
    public void Handler2(string message)
    {
        Console.WriteLine("handler2: msg = '{0}', topic = '{1}'", message,
                          WampEventContext.Current.EventDetails.Topic);
    }             
}

public class Subscriber3
{
    [WampTopic("com..topic1")]
    public void Handler3(string message)
    {
        Console.WriteLine("handler3: msg = '{0}', topic = '{1}'", message,
                          WampEventContext.Current.EventDetails.Topic);
    }             
}
```
> Note: this sample is based on [this](https://github.com/crossbario/crossbarexamples/tree/master/patternsubs) Autobahn sample

#### Shared registrations

From this version [shared registrations](http://crossbar.io/docs/Shared-Registrations/) are supported both on router and client sides.

In order to use shared registrations, pass to the Register methods, RegisterOptions with a desired Invoke (the policy to be used). The possible options are: single/first/last/random/roundrobin.

Example:
```csharp
public async Task Run()
{
    DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

    IWampChannel channel =
        channelFactory.CreateJsonChannel
            ("ws://127.0.0.1:8080/ws",
             "realm1");

    TaskCompletionSource<string> identTask = new TaskCompletionSource<string>();

    channel.RealmProxy.Monitor.ConnectionEstablished += (sender, args) =>
    {
        string ident =
            string.Format("MyComponent (PID {0}, Session {1})",
                          Process.GetCurrentProcess().Id,
                          args.SessionId);

        identTask.SetResult(ident);
    };

    await channel.Open().ConfigureAwait(false);

    string identValue = await identTask.Task;

    await channel.RealmProxy.Services.RegisterCallee(new MyComponent(identValue),
        new CalleeRegistrationInterceptor(new RegisterOptions()
        {
            Invoke = "roundrobin"
        }))
        .ConfigureAwait(false);
}

public class MyComponent
{
    private readonly string mIdent;

    public MyComponent(string ident)
    {
        mIdent = ident;
    }

    [WampProcedure("com.example.add2")]
    public object Add2(double x, double y)
    {
        Console.WriteLine("add2 called on {0}", mIdent);

        return new
        {
            result = x + y,
            ident = mIdent
        };
    }
}
```

> Note: this sample is based on [this](https://github.com/crossbario/crossbarexamples/tree/master/sharedregs) Autobahn sample


#### Pattern-based registrations

From this version [pattern-based registrations](http://crossbar.io/docs/Pattern-Based-Registrations/) are supported both on router and client sides.

In order to use shared registrations, pass to the Register methods, RegisterOptions with a desired Match (the policy to be used). The possible options are: exact/prefix/wildcard.

Example:
```csharp
public static async Task Run()
{
    DefaultWampChannelFactory channelFactory = new DefaultWampChannelFactory();

    IWampChannel channel =
        channelFactory.CreateJsonChannel("ws://127.0.0.1:8080/ws",
            "realm1");

    await channel.Open().ConfigureAwait(false);

    await channel.RealmProxy.Services.RegisterCallee(new Callee1())
        .ConfigureAwait(false);

    await channel.RealmProxy.Services.RegisterCallee
        (new Callee2(),
         new CalleeRegistrationInterceptor(new RegisterOptions
         {
             Match = "prefix"
         }))
         .ConfigureAwait(false);

    await channel.RealmProxy.Services.RegisterCallee
        (new Callee3(),
         new CalleeRegistrationInterceptor(new RegisterOptions
         {
             Match = "wildcard"
         }))
         .ConfigureAwait(false);
}

public class Callee1
{
    [WampProcedure("com.example.procedure1")]
    public void Endpoint1(string message)
    {
        Console.WriteLine("endpoint1: msg = '{0}', procedure = '{1}'", message,
                          WampInvocationContext.Current.InvocationDetails.Procedure);
    }
}

public class Callee2
{
    [WampProcedure("com.example")]
    public void Endpoint2(string message)
    {
        Console.WriteLine("endpoint2: msg = '{0}', procedure = '{1}'", message,
                          WampInvocationContext.Current.InvocationDetails.Procedure);
    }             
}

public class Callee3
{
    [WampProcedure("com..procedure1")]
    public void Endpoint3(string message)
    {
        Console.WriteLine("endpoint3: msg = '{0}', procedure = '{1}'", message,
                          WampInvocationContext.Current.InvocationDetails.Procedure);
    }             
}

```

> Note: this sample is based on [this](https://github.com/crossbario/crossbarexamples/tree/master/patternregs) Autobahn sample


### Internal Changes

####Performance improvements

This version introduces some optimizations:

* Reflection optimizations - avoiding calling MethodInfo.Invoke using expression delegate compilation and other techniques - mostly in Reflection based roles, such as Reflection based callee, Reflection based subscriber, callee proxy.
* Serialization optimizations - serializing entire WampMessages instead of serializing each element of the array separately.
* Pub/Sub subscriber black and whitelisting, publisher exclusion calculation optimizations using HashSets.

####Bug fixes

This version contains a couple of bug fixes, mainly [SUBSCRIBE fixes](https://github.com/Code-Sharp/WampSharp/issues/67).

> Written with [StackEdit](https://stackedit.io/).