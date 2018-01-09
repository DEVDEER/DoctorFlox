# get local version out of nuspec
$nuspec = Get-Content Logic.Core.csproj
$matches = [regex]::Match($nuspec, "<PackageVersion>(.*?)<\/PackageVersion>")
$localVersion = $matches[0].Captures.Groups[1].Value
# get version on nuget.org
$url = "https://api.nuget.org/v3-flatcontainer/devdeer.DoctorFlox/index.json"
$versions = Invoke-WebRequest $url -UseBasicParsing | ConvertFrom-Json | Select -expand versions
$nugetVersion = $versions[$versions.Length - 1]
# compare versions
$result = !$localVersion.Equals($nugetVersion)
Write-Host ("##vso[task.setvariable variable=NuGet.PushRequired;]$result")