Param (
    $parameters = @{},
    $srcFolder,
    $projectName,
    $projectVersion
)

# get script variables

# update package version in nuspec file
Write-Output "Updating version in WampSharp nuspec file"
$nuspecPath = "WampSharp.nuspec"
[xml]$xml = Get-Content $nuspecPath
$xml.package.metadata.version = $projectVersion
$xml.Save($nuspecPath)

# update package version in nuspec file
Write-Output "Updating version in WampSharp.Default nuspec file"
$nuspecPath = "WampSharp.Default.nuspec"
[xml]$xml = Get-Content $nuspecPath
$xml.package.metadata.version = $projectVersion
$xml.Save($nuspecPath)

# build NuGet package
Write-Output "Building WampSharp NuGet package"
..\src\.nuget\NuGet.exe pack .\WampSharp.nuspec -OutputDirectory ".\"

# build NuGet package
Write-Output "Building WampSharp.Default NuGet package"
..\src\.nuget\NuGet.exe pack .\WampSharp.Default.nuspec -OutputDirectory ".\"
