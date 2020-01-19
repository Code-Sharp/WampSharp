WampSharp
=========
[![NuGet Version][NuGetImgMaster]][NuGetLinkMaster]

A C# implementation of [WAMP (The Web Application Messaging Protocol)][WampLink]

The implementation supports WAMPv2 and includes both Json and MsgPack support, and both Router (Broker and Dealer roles) and Client (Publisher/Subscriber and Callee/Caller) roles. See here for a list of [implemented advanced profile features](http://wampsharp.net/#advanced-profile-features).

The implementation also supports WAMPv1, both client and server roles.

## Donations

If you found WampSharp helpful and want to donate, you are welcome to do so via [PayPal](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=UHRAS9KZPNPX4).

Your donations help keep this project's development alive.

## Documentation

Documentation is available at [https://wampsharp.net](https://wampsharp.net)!

## WampSharp v20.1.1

WampSharp v20.1.1 is available, see version [release notes](https://wampsharp.net/release-notes/wampsharp-v20.1.1-release-notes/).

## Get Started

See [Get started tutorial](http://wampsharp.net/wamp2/getting-started-with-wampv2/) and
* [Getting started with Callee](http://wampsharp.net/wamp2/roles/callee/getting-started-with-callee/)
* [Getting started with Caller](http://wampsharp.net/wamp2/roles/caller/getting-started-with-caller/)
* [Getting started with Publisher](http://wampsharp.net/wamp2/roles/publisher/getting-started-with-publisher/)
* [Getting started with Subscriber](http://wampsharp.net/wamp2/roles/subscriber/getting-started-with-subscriber/)

See the [documentation](https://wampsharp.net) for more help.

## WAMPv1 support

WAMPv1 support is still available. You can read about it on the [documentation site](https://wampsharp.net/categories/wamp1/).

In order to use it, Install WampSharp.WAMP1.Default from NuGet.

If you're updating from a previous WampSharp version and you're not interested yet in updating your application to WAMPv2, please read the following [notes](http://wampsharp.net/wamp1/notes-for-wampv1-users/).

## Poloniex Api issues

Poloniex no longer uses WAMP for its WebSockets api. Please don't open issues specific to Poloniex api. These will be closed without any comment. If you are still interested in connecting to Poloniex WebSockets api, take a look at [PoloniexWebSocketsApi](https://github.com/Code-Sharp/PoloniexWebSocketsApi).

[WampLink]:http://wamp-proto.org

[NuGetImgMaster]:http://img.shields.io/nuget/v/WampSharp.Default.svg
[NuGetLinkMaster]:http://www.nuget.org/packages/WampSharp.Default/
