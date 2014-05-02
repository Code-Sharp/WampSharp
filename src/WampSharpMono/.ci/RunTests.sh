#!/bin/sh -x

mono --runtime=v4.0 .nuget/NuGet.exe install NUnit.Runners -Version 2.6.3 -o packages

runTest(){
    mono --runtime=v4.0 packages/NUnit.Runners.2.6.3/tools/nunit-console.exe -noxml -nodots -labels $@
   if [ $? -ne 0 ]
   then   
     exit 1
   fi
}

runTest Tests/WampSharp.Tests/bin/Debug/WampSharp.Tests.dll -exclude=Performance
runTest Tests/WampSharp.Tests.Wampv2/bin/Debug/WampSharp.Tests.Wampv2.dll -exclude=Performance

exit $?
