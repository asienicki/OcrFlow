param(
    [string]$PublishDir = "D:\Source\OcrFlow\publish",
    [string]$OutputFile = "D:\Source\OcrFlow\installer\OcrFlow.Installer\PublishFiles.wxs"
)

if (-not (Test-Path $PublishDir)) {
    Write-Error "PublishDir not found: $PublishDir"
    exit 1
}

function Get-SafeId {
    param([string]$text)
    return ($text -replace '[^a-zA-Z0-9]', '_').TrimStart('_')
}

function Build-DirectoryTree {
    param([string]$basePath, [string]$currentPath = "", [string]$indent = "      ")
    
    $fullPath = if ($currentPath) { Join-Path $basePath $currentPath } else { $basePath }
    $items = Get-ChildItem $fullPath
    
    $result = ""
    
    foreach ($item in $items) {
        if ($item.PSIsContainer) {
            $relativePath = if ($currentPath) { "$currentPath\$($item.Name)" } else { $item.Name }
            $dirId = "Dir_" + (Get-SafeId $relativePath)
            
            $result += "$indent<Directory Id=`"$dirId`" Name=`"$($item.Name)`">`n"
            $result += Build-DirectoryTree -basePath $basePath -currentPath $relativePath -indent "$indent  "
            $result += "$indent</Directory>`n"
        }
    }
    
    return $result
}

function Build-Components {
    param([string]$basePath)
    
    $allFiles = Get-ChildItem $basePath -File -Recurse
    $result = ""
    
    foreach ($file in $allFiles) {
        $relativePath = $file.FullName.Substring($basePath.Length + 1)
        $relativeDir = Split-Path $relativePath -Parent
        
        $dirId = if ($relativeDir) { "Dir_" + (Get-SafeId $relativeDir) } else { "INSTALLDIR" }
        $compId = "Comp_" + (Get-SafeId $relativePath)
        $fileId = "File_" + (Get-SafeId $relativePath)
        $guid = [guid]::NewGuid().ToString()
        $keyPath = if ($file.Name -eq "OcrFlow.exe") { ' KeyPath="yes"' } else { '' }
        
        $result += @"

      <Component Id="$compId" Guid="$guid" Directory="$dirId">
        <File Id="$fileId" Source="`$(var.PublishDir)\$relativePath"$keyPath />
      </Component>
"@
    }
    
    return $result
}

$directoryTree = Build-DirectoryTree -basePath $PublishDir
$components = Build-Components -basePath $PublishDir
$fileCount = (Get-ChildItem $PublishDir -File -Recurse).Count

$xml = @"
<?xml version="1.0" encoding="utf-8"?>
<Wix xmlns="http://wixtoolset.org/schemas/v4/wxs">
  <Fragment>
    <DirectoryRef Id="INSTALLDIR">
$directoryTree    </DirectoryRef>
  </Fragment>

  <Fragment>
    <ComponentGroup Id="PublishFiles">$components
    </ComponentGroup>
  </Fragment>
</Wix>
"@

# Upewnij się, że katalog wyjściowy istnieje
$outputDir = Split-Path $OutputFile -Parent
if ($outputDir -and -not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Force -Path $outputDir | Out-Null
}

$xml | Out-File -FilePath $OutputFile -Encoding UTF8 -Force
Write-Host "Generated $OutputFile with $fileCount files" -ForegroundColor Green