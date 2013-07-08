#!/bin/sh -x
mkdir -p packages
cd packages
for i in .nuget WampSharp.Api WampSharp.Auxiliary WampSharp.Core WampSharp.Fleck WampSharp.PubSub WampSharp.Rpc WampSharp.Tests WampSharp.WebSocket4Net
  do mono --runtime=v4.0 ../.nuget/NuGet.exe install ../$i/packages.config
done
cd ..
