# Linux certificate handling
sudo mozroots --import --machine --sync
sudo certmgr -ssl -m https://go.microsoft.com
sudo certmgr -ssl -m https://nugetgallery.blob.core.windows.net
sudo certmgr -ssl -m https://nuget.org

#!/bin/sh -x
mono .nuget/NuGet.exe restore WampSharpMono.sln
