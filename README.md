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

Documentation has been moved to [its own site](https://wampsharp.net)!

## WampSharp v1.2.6.41-beta

WampSharp v1.2.6.41-beta released, see version [release notes](http://wampsharp.net/release-notes/wampsharp-v1.2.6.41-beta-release-notes/).

## Get Started

See [Get started tutorial](http://wampsharp.net/wamp2/getting-started-with-wampv2/) and
* [Getting started with Callee](http://wampsharp.net/wamp2/roles/callee/getting-started-with-callee/)
* [Getting started with Caller](http://wampsharp.net/wamp2/roles/caller/getting-started-with-caller/)
* [Getting started with Publisher](http://wampsharp.net/wamp2/roles/publisher/getting-started-with-publisher/)
* [Getting started with Subscriber](http://wampsharp.net/wamp2/roles/subscriber/getting-started-with-subscriber/)

See [documentation](https://wampsharp.net) for more help.

## WAMPv1 support

WAMPv1 support is still available. You can read about it on the [Documentation site](https://wampsharp.net).

In order to use it, Install WampSharp.WAMP1.Default from NuGet.

If you're updating from a previous WampSharp version and you're not interested yet in updating your application to WAMPv2, please read the following [notes](http://wampsharp.net/wamp1/notes-for-wampv1-users/).

## Poloniex Api issues

Poloniex no longer uses WAMP for its WebSockets api. Please don't open issues specific to Poloniex api. These will be closed without any comment. If you are still interested in connecting to Poloniex WebSockets api, take a look at [PoloniexWebSocketsApi](https://github.com/Code-Sharp/PoloniexWebSocketsApi).

[WampLink]:http://wamp.ws

[NuGetImgMaster]:http://img.shields.io/nuget/v/WampSharp.Default.svg
[NuGetLinkMaster]:http://www.nuget.org/packages/WampSharp.Default/
[WinImgMaster]:https://img.shields.io/teamcity/codebetter/WampSharp_NetCore_Wampv2_Build.svg
[WinLinkMaster]:http://teamcity.codebetter.com/project.html?projectId=WampSharp_NetCore_Wampv2_Build&guest=1
[MonoImgMaster]:https://img.shields.io/travis/Code-Sharp/WampSharp/wampv2.svg
[MonoLinkMaster]:https://travis-ci.org/Code-Sharp/WampSharp
[AppVeyorLinkMaster]:https://ci.appveyor.com/project/darkl/wampsharp-759
[AppVeyorImgMaster]:https://ci.appveyor.com/api/projects/status/fgbqbgwqx4j8jain

[JetBrains]:http://www.jetbrains.com/
[CodeBetter]:http://codebetter.com/
[travis-ci]:https://travis-ci.org/
[AppVeyor]:http://www.appveyor.com/
