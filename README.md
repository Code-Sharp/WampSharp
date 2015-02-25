WampSharp
=========


A C# implementation of [WAMP (The Web Application Messaging Protocol)][WampLink]

The implementation supports WAMPv2 and includes both Json and MsgPack support, and both Router (Broker and Dealer roles) and Client (Publisher/Subscriber and Callee/Caller) roles.

The implementation also supports WAMPv1, both client and server roles.

## Builds

Master | Provider
------ | --------
[![Build Status][WinImgMaster]][WinLinkMaster] | Windows CI Provided By [CodeBetter][] and [JetBrains][] 
[![Build Status][MonoImgMaster]][MonoLinkMaster] | Mono CI Provided by [travis-ci][] 

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

[WampLink]:http://wamp.ws

[WinImgMaster]:http://teamcity.codebetter.com/app/rest/builds/buildType:\(id:WampSharp_Dev_Build\)/statusIcon
[WinLinkMaster]:http://teamcity.codebetter.com/project.html?projectId=WampSharp_Dev&guest=1
[MonoImgMaster]:https://travis-ci.org/Code-Sharp/WampSharp.png?branch=dev
[MonoLinkMaster]:https://travis-ci.org/Code-Sharp/WampSharp

[JetBrains]:http://www.jetbrains.com/
[CodeBetter]:http://codebetter.com/
[travis-ci]:https://travis-ci.org/
[AppVeyor]:http://www.appveyor.com/
