WampSharp
=========


A C# implementation of [WAMP (The WebSocket Application Messaging Protocol)][WampLink]

This branch is a dev branch of a WAMPv2 implementation.

It is still under development and isn't very mature right now.

The implementation includes both Json and MsgPack support, and both Router (Broker and Dealer roles) and Client (Publisher/Subscriber and Callee/Caller) roles.

There is no documentation yet, but you can find examples in the Samples/WAMP2 directory.

If you want to test it, you can use Teamcity NuGet feed:
http://teamcity.codebetter.com/httpAuth/app/nuget/v1/FeedService.svc/

(You need a TeamCity account. Register here: http://teamcity.codebetter.com/login.html)

Master | Provider
------ | --------
[![Build Status][WinImgMaster]][WinLinkMaster] | Windows CI Provided By [CodeBetter][] and [JetBrains][] 
[![Build Status][AppVeyorImgMaster]][AppVeyorLinkMaster] | Windows CI Provided By [AppVeyor][]
[![Build Status][MonoImgMaster]][MonoLinkMaster] | Mono CI Provided by [travis-ci][] 

[WampLink]:http://wamp.ws

[WinImgMaster]:http://teamcity.codebetter.com/app/rest/builds/buildType:\(id:WampSharp_Wampv2_Build\)/statusIcon
[WinLinkMaster]:http://teamcity.codebetter.com/project.html?projectId=WampSharp_Wampv2&guest=1
[MonoImgMaster]:https://travis-ci.org/Code-Sharp/WampSharp.png?branch=wampv2
[MonoLinkMaster]:https://travis-ci.org/Code-Sharp/WampSharp
[AppVeyorLinkMaster]:https://ci.appveyor.com/project/darkl/wampsharp-759
[AppVeyorImgMaster]:https://ci.appveyor.com/api/projects/status/fgbqbgwqx4j8jain

[JetBrains]:http://www.jetbrains.com/
[CodeBetter]:http://codebetter.com/
[travis-ci]:https://travis-ci.org/
[AppVeyor]:http://www.appveyor.com/
