#!/bin/sh -x
mkdir -p packages
cd packages
for i in .nuget WampSharp WampSharp.Default WampSharp.Tests Samples/WampSharp.PubSubServerSample Samples/WampSharp.RpcClientSample Samples/WampSharp.RpcServerSample
  do mono --runtime=v4.0 ../.nuget/NuGet.exe install ../$i/packages.config
done
cd ..
