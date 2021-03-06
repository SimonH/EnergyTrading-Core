[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True)]
  [string]$solutionPath,
  [string]$nugetFeedPath,
  [switch]$Pack,
  [switch]$Publish
  
)

if (![System.IO.Directory]::Exists($solutionPath))
{
  Write-Host "$solutionPath does Not Exist"
  return
}

$packedPath = [System.IO.Path]::Combine($solutionPath, 'packed')
$wildcardPath = [System.IO.Path]::Combine($packedPath, '*')
if ($Pack)
{
if (![System.IO.Directory]::Exists($packedPath))
{
  md $packedPath
}
else
{
   Remove-Item $wildcardPath
}

ForEach ($folder in gci $solutionPath | where {$_.Attributes -eq 'Directory'})
{
## change to use folder wildcard here and find nuspec files so we don't have to match name (e.g. SimonH.xx.nuspec)
  $nuspecFile = [System.IO.Path]::Combine($folder.FullName, $folder.Name + ".nuspec")
  if ([System.IO.File]::Exists($nuspecFile))
  {
    $command = @'
cmd /C nuget.exe pack $nuspecFile -BasePath $solutionPath -OutputDirectory $packedPath
'@
    Invoke-Expression -Command:$command
  }
}
}
if ($Publish -and ![System.String]::IsNullOrEmpty($nugetFeedPath) -and [System.IO.Directory]::Exists($nugetFeedPath) -and [System.IO.Directory]::Exists($packedPath))
{
  ForEach ($package in gci $wildcardPath -Include *nupkg)
  {     
     $packagePath = [System.IO.Path]::Combine($packedPath, $package.Name);
    $command = @'
cmd /C nuget.exe add $packagePath -Source $nugetFeedPath
'@
    Invoke-Expression -Command:$command
  }
}

