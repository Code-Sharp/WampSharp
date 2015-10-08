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
[![Build Status][AppVeyorImgMaster]][AppVeyorLinkMaster] | Windows CI Provided By [AppVeyor][]
[![Build Status][MonoImgMaster]][MonoLinkMaster] | Mono CI Provided by [travis-ci][]

## Documentation

Documentation has moved to [its own repository](https://github.com/Code-Sharp/WampSharp-docs)!

## WampSharp v1.2.3.12-beta

WampSharp v1.2.3.12-beta released, see version [release notes](https://github.com/Code-Sharp/WampSharp-docs/blob/master/Release-notes/WampSharp-v1.2.3.12-beta-release-notes.md).

## Get Started

See [Get started tutorial](https://github.com/Code-Sharp/WampSharp-docs/blob/master/WAMP2/Getting-Started-with-WAMPv2.md) and
* [Getting started with Callee](https://github.com/Code-Sharp/WampSharp-docs/blob/master/WAMP2/Roles/Callee/Getting-Started-with-Callee.md)
* [Getting started with Caller](https://github.com/Code-Sharp/WampSharp-docs/blob/master/WAMP2/Roles/Caller/Getting-Started-with-Caller.md)
* [Getting started with Publisher](https://github.com/Code-Sharp/WampSharp-docs/blob/master/WAMP2/Roles/Publisher/Getting-Started-with-Publisher.md)
* [Getting started with Subscriber](https://github.com/Code-Sharp/WampSharp-docs/blob/master/WAMP2/Roles/Publisher/Getting-Started-with-Subscriber.md)

See [documentation](https://github.com/Code-Sharp/WampSharp-docs) for more help.

## WAMPv1 support

WAMPv1 support is still available. You can read about it at the [Documentation site](https://github.com/Code-Sharp/WampSharp-docs).

In order to use it, Install WampSharp.WAMP1.Default from NuGet.

If you're updating from a previous WampSharp version and you're not interested yet in updating your application to WAMPv2, please read the following [notes](https://github.com/Code-Sharp/WampSharp-docs/blob/master/WAMP1/Notes-for-WAMPv1-users.md).

## Donations

If you found WampSharp helpful and want to donate, you are welcome to do so via [PayPal](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=UHRAS9KZPNPX4).

Your donations help keep this project development alive.

[WampLink]:http://wamp.ws

[NuGetImgMaster]:http://img.shields.io/nuget/v/WampSharp.Default.svg
[NuGetLinkMaster]:http://www.nuget.org/packages/WampSharp.Default/
[WinImgMaster]:https://img.shields.io/teamcity/codebetter/WampSharp_Wampv2_Build.svg
[WinLinkMaster]:http://teamcity.codebetter.com/project.html?projectId=WampSharp_Wampv2&guest=1
[MonoImgMaster]:https://img.shields.io/travis/Code-Sharp/WampSharp/wampv2.svg
[MonoLinkMaster]:https://travis-ci.org/Code-Sharp/WampSharp
[AppVeyorLinkMaster]:https://ci.appveyor.com/project/darkl/wampsharp-759
[AppVeyorImgMaster]:https://ci.appveyor.com/api/projects/status/fgbqbgwqx4j8jain

[JetBrains]:http://www.jetbrains.com/
[CodeBetter]:http://codebetter.com/
[travis-ci]:https://travis-ci.org/
[AppVeyor]:http://www.appveyor.com/
