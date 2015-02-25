WampSharp v1.2.1.0-beta release notes
=================================

###Api changes

####IAsyncDisposable

A new type called IAsyncDisposable is introduced in the library. It's similar to IDisposable, but returns a Task.

```csharp
public interface IAsyncDisposable
{
    Task DisposeAsync();
}
```

Task&lt;IAsyncDisposable&gt; is returned from some methods:

* IWampTopicProxy Subscribe method now returns an Task&lt;IAsyncDisposable&gt; instead of Task&lt;IDisposable&gt;, you can call DisposeAsync in order to unsubscribe from the topic.
* IWampRpcOperationCatalogProxy Register method now returns Task&lt;IAsyncDisposable&gt; instead of Task. Call DisposeAsync to Unregister the procedure from router.
* IWampRpcOperationCatalogProxy Unregister method has been removed. Use the IAsyncDisposable returned from Register instead.
* IWampRealmServiceProvider Register method now returns a Task&lt;IAsyncDisposable&gt;, call DisposeAsync in order to unregister the callee.

The IAsyncDisposable mentioned above's Task completes when the router sends a UNSUBSCRIBED/UNREGISTERED message corresponding to the sent request.

### New features

#### Progressive calls

From this version, progressive calls are supported. 
In order to use progressive calls as a Caller, declare in your callee service a [WampProcedure] method having a [WampProgressiveCall] attribute and a IProgress&lt;T&gt; as the last parameter.
> Note that the method return type should be Task&lt;T&gt; where this is the same T as in the IProgress&lt;T&gt; of the last parameter.
 
Example:

```csharp
public interface ILongOpService
{
    [WampProcedure("com.myapp.longop")]
    [WampProgressiveResultProcedure]
    Task<int> LongOp(int n, IProgress<int> progress);
}
```

Then obtain the proxy and call it:
```csharp
public async Task Run()
{
    DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

    IWampChannel channel = factory.CreateJsonChannel("ws://localhost:8080/ws", "realm1");

    await channel.Open();
    
    ILongOpService proxy = channel.RealmProxy.Services.GetCalleeProxy<ILongOpService>();

    Progress<int> progress = 
        new Progress<int>(i => Console.WriteLine("Got progress " + i));

    int result = await proxy.LongOp(10, progress);

    Console.WriteLine("Got result " + result);
}
```

In order to use progressive calls as a Callee, create a service with that implements an interface with the same kind of signature. In order to report progress, call the progress Report method. Example:

```csharp
public class LongOpService : ILongOpService
{
    public async Task<int> LongOp(int n, IProgress<int> progress)
    {
        for (int i = 0; i < n; i++)
        {
            progress.Report(i);
            await Task.Delay(100);
        }

        return n;
    }
}
```

> Note: you can put the attributes on the method itself instead of implementing an interface, i.e:
> 
>```csharp
> public class LongOpService
>{
>    [WampProcedure("com.myapp.longop")]
>    [WampProgressiveResultProcedure]
>    public async Task<int> LongOp(int n, IProgress<int> progress)
>    {
>	    // ...
>    }
> }
>```

Then register it to the realm regularly:
```csharp
public async Task Run()
{
    DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

    IWampChannel channel = factory.CreateJsonChannel("ws://localhost:8080/ws", "realm1");

    await channel.Open();

    ILongOpService service = new LongOpService();

    IAsyncDisposable disposable = 
        await channel.RealmProxy.Services.RegisterCallee(service);

    Console.WriteLine("Registered LongOpService");
}
```

>Note:  The samples are based on [this](https://github.com/tavendo/AutobahnPython/tree/master/examples/twisted/wamp/basic/rpc/progress) AutobahnJS sample

#### Caller identification

From this version, it is possible to get/supply caller identification details. According to WAMP2 specification, a Callee can request to get caller identification details (by specifying disclose_caller = true on registration), and a Caller can request to disclose its identification (by specifying disclose_me = true on call request).

Specifying these is now possible on callee registration and when obtaining callee proxy:

Callee registration example:

```csharp
public async Task Run()
{
    DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

    IWampChannel channel = 
        factory.CreateJsonChannel("ws://localhost:8080/ws", "realm1");

    await channel.Open();

    SquareService service = new SquareService();

    var registerOptions =
        new RegisterOptions
        {
            DiscloseCaller = true
        };

    IAsyncDisposable disposable =
        await channel.RealmProxy.Services.RegisterCallee(service,
            new CalleeRegistrationInterceptor(registerOptions));
}
```

Callee proxy sample:
```csharp
public async Task Run()
{
    DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

    IWampChannel channel =
        factory.CreateJsonChannel("ws://localhost:8080/ws", "realm1");

    await channel.Open();

    var callOptions = new CallOptions()
    {
        DiscloseMe = true
    };

    ISquareService proxy =
        channel.RealmProxy.Services.GetCalleeProxy<ISquareService>
        (new CachedCalleeProxyInterceptor(new CalleeProxyInterceptor(callOptions)));

    await proxy.Square(-2);
    await proxy.Square(0);
    await proxy.Square(2);
}

```

In order to obtain these details as a Callee (using reflection api), access WampInvocationContext.Current.

Sample:

```csharp
public class SquareService
{
    [WampProcedure("com.myapp.square")]
    public int Square(int n)
    {
        InvocationDetails details = 
            WampInvocationContext.Current.InvocationDetails;

        Console.WriteLine("Someone is calling me: " + details.Caller);

        return n*n;
    }
}

```

> Note: The samples are based on [this](https://github.com/tavendo/AutobahnPython/tree/master/examples/twisted/wamp/basic/rpc/options) AutobahnJS sample


#### WampInvocationContext

WampInvocationContext allows you to get the invocation details provided with the current invocation. It currently contains the caller identification (if present) and whether the caller requested a progressive call. 
Example:

```csharp
public class LongOpService : ILongOpService
{
    public async Task<int> LongOp(int n, IProgress<int> progress)
    {
        InvocationDetails details = 
            WampInvocationContext.Current.InvocationDetails;

        for (int i = 0; i < n; i++)
        {
            if (details.ReceiveProgress == true)
            {
                progress.Report(i);                    
            }

            await Task.Delay(100);
        }

        return n;
    }
}
```

#### Attribute based pub/sub

Allows to use WAMPv2 pub/sub features in a similar fashion as reflection rpc.

In order to use it from a publisher, create a class containing an event decorated with a [WampTopic] attribute. Then register an instance of the class using the RegisterPublisher method of IWampRealmServiceProvider. The arguments published to the event will be treated as the arguments keywords of the publication.

Example: Publisher class:
```csharp
public class MyClass
{
    [JsonProperty("counter")]
    public int Counter { get; set; }

    [JsonProperty("foo")]
    public int[] Foo { get; set; }
}

public delegate void MyPublicationDelegate(int number1, int number2, string c, MyClass d);

public interface IMyPublisher
{
    [WampTopic("com.myapp.heartbeat")]
    event Action Heartbeat; 

    [WampTopic("com.myapp.topic2")]
    event MyPublicationDelegate MyEvent;
}

public class MyPublisher : IMyPublisher
{
    private readonly Random mRandom = new Random();
    private IDisposable mSubscription;

    public MyPublisher()
    {
        mSubscription = Observable.Timer(TimeSpan.FromSeconds(0),
            TimeSpan.FromSeconds(1)).Select((x, i) => i)
            .Subscribe(x => OnTimer(x));
    }

    private void OnTimer(int value)
    {
        RaiseHeartbeat();

        RaiseMyEvent(mRandom.Next(0, 100),
            23,
            "Hello",
            new MyClass()
            {
                Counter = value,
                Foo = new int[] {1, 2, 3}
            });
    }

    private void RaiseHeartbeat()
    {
        Action handler = Heartbeat;
        
        if (handler != null)
        {
            handler();
        }
    }

    private void RaiseMyEvent(int number1, int number2, string c, MyClass d)
    {
        MyPublicationDelegate handler = MyEvent;
        
        if (handler != null)
        {
            handler(number1, number2, c, d);
        }
    }

    public event Action Heartbeat;

    public event MyPublicationDelegate MyEvent;
}
```

Publisher registration:
```csharp
public static async Task Run()
{
    DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

    IWampChannel channel =
        factory.CreateJsonChannel("ws://localhost:8080/ws", "realm1");

    await channel.Open();

    IDisposable publisherDisposable = 
        channel.RealmProxy.Services.RegisterPublisher(new MyPublisher());

    // call publisherDisposable.Dispose(); to unsubscribe from the event.
}
```

>Note: if the delegate used is of type Action&lt;&gt;, the publication will send the parameters as the positional arguments of the publication, otherwise it will use the parameters as the keyword arguments of the publication (with the parameters names as the keys).

In order to use the feature from a subscriber, create a class with a method having a [WampTopic] attribute, Then call RegisterSubscriber of IWampRealmServiceProvider.

Example:

```csharp
public class MyClass
{
    [JsonProperty("counter")]
    public int Counter { get; set; }

    [JsonProperty("foo")]
    public int[] Foo { get; set; }

    public override string ToString()
    {
        return string.Format("counter: {0}, foo: [{1}]",
            Counter,
            string.Join(", ", Foo));
    }
}

public interface IMySubscriber
{
    [WampTopic("com.myapp.heartbeat")]
    void OnHeartbeat();

    [WampTopic("com.myapp.topic2")]
    void OnTopic2(int number1, int number2, string c, MyClass d);
}

public class MySubscriber : IMySubscriber
{
    public void OnHeartbeat()
    {
        long publicationId = WampEventContext.Current.PublicationId;
        Console.WriteLine("Got heartbeat (publication ID " + publicationId + ")");
    }

    public void OnTopic2(int number1, int number2, string c, MyClass d)
    {
        Console.WriteLine("Got event: number1:{0}, number2:{1}, c:{2}, d:{3}",
            number1, number2, c, d);
    }
}
```

Subscriber registration:
```csharp
public static async Task Run()
{
    DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

    IWampChannel channel =
        factory.CreateJsonChannel("ws://localhost:8080/ws", "realm1");

    await channel.Open();

    Task<IAsyncDisposable> subscriptionTask = 
        channel.RealmProxy.Services.RegisterSubscriber(new MySubscriber());

    IAsyncDisposable asyncDisposable = await subscriptionTask;

    // call await asyncDisposable.DisposeAsync(); to unsubscribe from the topic.
}

```

>Note:  The samples are based on [this](https://github.com/tavendo/AutobahnPython/tree/master/examples/twisted/wamp/basic/pubsub/complex) AutobahnJS sample, but are a bit different (WampSharp doesn't support publishing both positional arguments and keyword arguments)

#### WampEventContext

As illustrated in last sample, you can use in pub/sub based subscribers WampEventContext.Current in order to get details about the current received event:

```csharp
public class MySubscriber
{
    [WampTopic("com.myapp.topic1")]
    public void OnTopic1(int counter)
    {
        WampEventContext context = WampEventContext.Current;

        Console.WriteLine("Got event, publication ID {0}, publisher {1}: {2}",
            context.PublicationId,
            context.EventDetails.Publisher,
            counter);
    }
}
```

>Note:  The sample is based on [this](https://github.com/tavendo/AutobahnPython/tree/master/examples/twisted/wamp/basic/pubsub/options) AutobahnJS sample.

#### Registration customization

The RegisterCallee, GetCalleeProxy, RegisterSubscriber and RegisterPublisher methods of IWampRealmServiceProvider now all have overloads that receive an "interceptor" instance. The "interceptors" allow customizing the request being performed.

For instance, assume you want to call procedures of a contract that its procedures uris are known only on runtime.  This is possible implementing a ICalleeProxyInterceptor:

```csharp
public class MyCalleeProxyInterceptor : CalleeProxyInterceptor
{
    private readonly int mCalleeIndex;

    public MyCalleeProxyInterceptor(int calleeIndex) : 
        base(new CallOptions())
    {
        mCalleeIndex = calleeIndex;
    }

    public override string GetProcedureUri(MethodInfo method)
    {
        string format = base.GetProcedureUri(method);
        string result = string.Format(format, mCalleeIndex);
        return result;
    }
}

```

This interceptor modifies the procedure uri of the procedure to call. For example, we can declare an interface with a method with this signature:

```csharp
public interface ISquareService
{
    [WampProcedure("com.myapp.square.{0}")]
    Task<int> Square(int number);
}
```

And then specify the index in runtime:
```csharp
public static async Task Run()
{
    DefaultWampChannelFactory factory = new DefaultWampChannelFactory();

    IWampChannel channel =
        factory.CreateJsonChannel("ws://localhost:8080/ws", "realm1");

    await channel.Open();

    int index = GetRuntimeIndex();

    ISquareService proxy = 
        channel.RealmProxy.Services.GetCalleeProxy<ISquareService>
        (new CachedCalleeProxyInterceptor(
            new MyCalleeProxyInterceptor(index)));

    int nine = await proxy.Square(3); // Calls ("com.myapp.square." + index)
}

```

> Note: we wrap our interceptor with the CachedCalleeProxyInterceptor in order to cache the results of our interceptor, in order to avoid calculating them each call.

Other interceptors work similarly. 
In addition, the interceptors allow modifying the options sent to each request.

> Note: these interceptors are still "static", i.e: they don't allow returning a value that depends on the publication/call parameters.

####Authentication

> Written with [StackEdit](https://stackedit.io/).
