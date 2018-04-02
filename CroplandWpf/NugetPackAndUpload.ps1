$ApiKey = [Microsoft.VisualBasic.Interaction]::InputBox("Nuget ApiKey", "ApiKey", "", 0, 0);
$dte.Solution.SolutionBuild.Clean($true)
$dte.Solution.SolutionBuild.Build($true)
$project = Get-Project | select -First 1
$dte.Solution.SolutionBuild.BuildProject("Release", $project.FullName, $true)
nuget pack -OutputDirectory bin/Release -Prop Configuration=Release -Version (gci bin/Release/CroplandWpf.dll | resolve-path | gcm | %{$_.FileVersionInfo} | Select -ExpandProperty "FileVersion")
nuget push ((Gci bin/Release/*.nupkg | Sort-Object { [regex]::Replace($_.Name, '\d+', { $args[0].Value.PadLeft(20) }) })[-1]).fullname -Source https://api.nuget.org/v3/index.json -ApiKey $ApiKey