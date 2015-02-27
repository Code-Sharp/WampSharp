# Linux certificate handling
sudo mozroots --import --machine --sync
sudo certmgr -ssl -m https://go.microsoft.com
sudo certmgr -ssl -m https://nugetgallery.blob.core.windows.net
sudo certmgr -ssl -m https://nuget.org

#!/bin/sh -x
mkdir -p packages
cd packages
for i in .nuget WampSharp WampSharp.Default Tests/WampSharp.Tests Tests/WampSharp.Tests.Wampv2 Samples/WAMP1/WampSharp.PubSubServerSample Samples/WAMP1/WampSharp.RpcClientSample Samples/WAMP1/WampSharp.RpcServerSample Samples/WAMP2/WampSharp.Samples.Callee Samples/WAMP2/WampSharp.Samples.EdgeJs Samples/WAMP2/WampSharp.Samples.Authentication Extensions/WampSharp.RawSocket Extensions/WampSharp.SignalR Extensions/WampSharp.Vtortola
  do mono --runtime=v4.0 ../.nuget/NuGet.exe install ../$i/packages.config
done
cd ..
