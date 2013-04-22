WampSharp
=========

A C# implementation of http://www.wamp.ws

Currently still in development, there are a lot of things to do before this is stable.

Design
=========

The design is still open for changes, but it is in the current fashion:

The framework is built with the following layers:
*Handlers - Converts between a method call to a WAMP request and vice versa (called WampMessage)
*RPC (TBD) - Implemented above the Handlers layer - converts an interface service-contract call request to a WAMP CALL request, dispatches it to the right method, and returns a result using WAMP CALLRESULT or WAMP CALLERROR messages
*PubSub (TBD) - Implemented above both layers - Allows to dispatch messages using ISubject<T> api of rx.