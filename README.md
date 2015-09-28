WampSharp
=========
[![NuGet Version][NuGetImgMaster]][NuGetLinkMaster]

A C# implementation of [WAMP (The Web Application Messaging Protocol)][WampLink]

The implementation supports WAMPv2 and includes both Json and MsgPack support, and both Router (Broker and Dealer roles) and Client (Publisher/Subscriber and Callee/Caller) roles.

The implementation also supports WAMPv1, both client and server roles.

## Builds

Master | Provider
------ | --------
[![Build Status][WinImgMaster]][WinLinkMaster] | Windows CI Provided By [CodeBetter][] and [JetBrains][] 
[![Build Status][MonoImgMaster]][MonoLinkMaster] | Mono CI Provided by [travis-ci][] 


## Advanced profile supported features:

The following [Advanced profile features](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced.md) are supported

* [Progressive call results](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/progressive-call-results.md): [[caller tutorial|Caller role#progressive-calls]] | [[callee tutorial|Callee role#progressive-callee]]
* [Caller identification](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/caller-identification.md): [[caller tutorial|Caller-role#caller-identification]] | [[callee tutorial|Callee-role#caller-identification]]
* [Session meta api](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/session-meta-api.md), [Registration meta api](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/registration-meta-api.md), [Subscription meta api](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/subscription-meta-api.md)
* [Shared registrations](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/shared-registration.md), see also [here](http://crossbar.io/docs/Shared-Registrations/)
* [Subscriber black and whitelisting](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/subscriber-blackwhite-listing.md)
* [Publisher exclusion](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/publisher-exclusion.md)
* [Publisher identification](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/publisher-identification.md)
* [Pattern-based subscriptions](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/pattern-based-subscription.md) - see also [here](http://crossbar.io/docs/Pattern-Based-Subscriptions/)
* [Pattern-based registrations](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/pattern-based-registration.md) - see also [here](http://crossbar.io/docs/Pattern-Based-Registrations/)
* [RawSocket transport](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/rawsocket-transport.md)
* [Authentication](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/authentication.md)
* [WAMP-CRA](https://github.com/wamp-proto/wamp-proto/blob/master/spec/advanced/challenge-response-authentication.md)

## WampSharp v1.2.1.6-beta

WampSharp v1.2.1.6-beta released, see version [release notes](https://github.com/Code-Sharp/WampSharp/wiki/WampSharp-v1.2.1.6-beta-release-notes).

## Get Started

See [Get started tutorial](https://github.com/Code-Sharp/WampSharp/wiki/Getting-started-with-WAMPv2) and
* [Getting started with Callee](https://github.com/Code-Sharp/WampSharp/wiki/Getting-Started-with-Callee)
* [Getting started with Caller](https://github.com/Code-Sharp/WampSharp/wiki/Getting-Started-with-Caller)
* [Getting started with Subscriber](https://github.com/Code-Sharp/WampSharp/wiki/Getting-Started-with-Subscriber)
* [Getting started with Publisher](https://github.com/Code-Sharp/WampSharp/wiki/Getting-Started-with-Publisher)

See [Wiki documentation](https://github.com/Code-Sharp/WampSharp/wiki) for more help.


## WAMPv1 support

WAMPv1 support is still available. You can read about it at the [Wiki](https://github.com/Code-Sharp/WampSharp/wiki).

If you're updating from a previous WampSharp version and you're not interested yet in updating your application to WAMPv2, please read the following [notes](https://github.com/Code-Sharp/WampSharp/wiki/Notes-for-WAMPv1-users).

## Donations

If you found WampSharp helpful and want to donate, you are welcome to do so via [PayPal](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=UHRAS9KZPNPX4).

Your donations help keep this project development alive.

[WampLink]:http://wamp.ws

[WinImgMaster]:http://teamcity.codebetter.com/app/rest/builds/buildType:\(id:WampSharp_Dev_Build\)/statusIcon
[WinLinkMaster]:http://teamcity.codebetter.com/project.html?projectId=WampSharp_Dev&guest=1
[MonoImgMaster]:https://travis-ci.org/Code-Sharp/WampSharp.png?branch=develop
[MonoLinkMaster]:https://travis-ci.org/Code-Sharp/WampSharp

[JetBrains]:http://www.jetbrains.com/
[CodeBetter]:http://codebetter.com/
[travis-ci]:https://travis-ci.org/
[AppVeyor]:http://www.appveyor.com/
