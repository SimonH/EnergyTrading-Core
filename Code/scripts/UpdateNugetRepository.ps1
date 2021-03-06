[CmdletBinding()]
Param(
  [Parameter(Mandatory=$True)]
  [string]$nugetFeedPath,
  [string[]]$removePackages,
  [string[]]$solutionPaths
)
if (![System.IO.Directory]::Exists($nugetFeedPath))
{
  Write-Host "$nugetFeedPath does not exist"
}
Write-Host solutionPaths
echo $solutionPaths

Write-Host removePackages
echo $removePackages
if ($removePackages.Length -gt 0)
{
  ForEach($package in $removePackages)
  {
    echo "package = $package"
    $packageDir = [System.IO.Path]::Combine($nugetFeedPath, $package)  
    echo $packageDir
    if ([System.IO.Directory]::Exists($packageDir))
    {
      remove-item $packageDir -recurse -force
    }
  }
}

if ($solutionPaths.Length -gt 0)
{
  $scriptDir = split-path $MyInvocation.MyCommand.Path
  echo $scriptDir
  $scriptPath = [System.IO.Path]::Combine($scriptDir, 'NugetPackAndPublish.ps1');
  echo $scriptPath
  ForEach($solution in $solutionPaths)
  {
    $args = "-solutionPath $solution -nugetFeedPath $nugetFeedPath -Pack -Publish"
    echo $args
    Invoke-Expression "$scriptPath $args"
  }
}


