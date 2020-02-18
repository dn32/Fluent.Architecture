$dirBase = "D:\Projetos\dn32\dn32.infra\"

function CheckResult {
    param( $result, $sucess )
	if($LASTEXITCODE -eq 0)
	{
	   echo $sucess
	}
	else
	{
		$ErrorString = $result -join [System.Environment]::NewLine	
		Write-Error  $ErrorString
		pause
		exit	
	}
}

echo "Clean"
dotnet clean "$dirBase\dn32.infra.sln"

echo "Build 2.2"
$result = dotnet build "$dirBase\dn32.infra.sln" --configuration Release /p:CopyOutputSymbolsToPublishDirectory=false --framework netcoreapp2.2
CheckResult $result "Build 2.2 Sucess!"

echo "Build 3.0"
$result = dotnet build "$dirBase\dn32.infra.sln" --configuration Release /p:CopyOutputSymbolsToPublishDirectory=false --framework netcoreapp3.1
CheckResult $result "Build 3.1 Sucess!"

echo "Cript"

$dirsPack = 
'dn32.infra',
'dn32.infra.Base',
'dn32.infra.Nucleo.Doc', 
'dn32.infra.EntityFramework',
'dn32.infra.EntityFramework.MemoryDatabase',
'dn32.infra.EntityFramework.MySQL',
'dn32.infra.EntityFramework.Oracle',
'dn32.infra.EntityFramework.PostgreSQL',
'dn32.infra.EntityFramework.SqLite',
'dn32.infra.EntityFramework.SqlServer',
'dn32.infra.Redis',
'dn32.infra.Test'

$dirs = 
'dn32.infra\bin\Release\netcoreapp2.2\','dn32.infra\bin\Release\netcoreapp3.1\', 
'dn32.infra.Nucleo.Doc\bin\Release\netcoreapp2.2\','dn32.infra.Nucleo.Doc\bin\Release\netcoreapp3.1\'

$files = 
'dn32.infra.Nucleo.dll','dn32.infra.Nucleo.dll',
'dn32.infra.Nucleo.Doc.dll', 'dn32.infra.Nucleo.Doc.dll'

For ($i=0; $i -lt $files.Length; $i++) 
{
   $dir = $dirs[$i]
   $file = $files[$i]

   $in = "$dirBase$dir$file"
   $out = "$dirBase$dir"   
 
   $result = dotfuscatorCLI -in:"$in" -out:"$out"
   CheckResult $result "Cript $file Sucess!"
}

echo "Pack"

For ($i=0; $i -lt $dirsPack.Length; $i++) 
{
   $dir = $dirsPack[$i]
   $result = dotnet pack -c release "$dirBase$dir"
   CheckResult $result "Pack $dir Sucess!"
   
   get-childitem "$dir\bin\release\*.nupkg" | foreach-object {move-item $_ -destination "D:\Drive\FOut" -Force}
}

pause

