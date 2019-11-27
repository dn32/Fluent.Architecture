$dirBase = "G:\Projetos\dn32\Fluent.Architecture\"

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
dotnet clean "$dirBase\Fluent.Architecture.sln"

echo "Build 2.2"
$result = dotnet build "$dirBase\Fluent.Architecture.sln" --configuration Release /p:CopyOutputSymbolsToPublishDirectory=false --framework netcoreapp2.2
CheckResult $result "Build 2.2 Sucess!"

echo "Build 3.0"
$result = dotnet build "$dirBase\Fluent.Architecture.sln" --configuration Release /p:CopyOutputSymbolsToPublishDirectory=false --framework netcoreapp3.0
CheckResult $result "Build 3.0 Sucess!"

echo "Cript"

$dirsPack = 
'Fluent.Architecture',
'Fluent.Architecture.Core.Doc', 
'Fluent.Architecture.EntityFramework',
'Fluent.Architecture.EntityFramework.MemoryDatabase',
'Fluent.Architecture.EntityFramework.MySQL',
'Fluent.Architecture.EntityFramework.Oracle',
'Fluent.Architecture.EntityFramework.PostgreSQL',
'Fluent.Architecture.EntityFramework.SqLite',
'Fluent.Architecture.EntityFramework.SqlServer',
'Fluent.Architecture.Redis',
'Fluent.Architecture.Test'

$dirs = 
'Fluent.Architecture\bin\Release\netcoreapp2.2\','Fluent.Architecture\bin\Release\netcoreapp3.0\', 
'Fluent.Architecture.Core.Doc\bin\Release\netcoreapp2.2\','Fluent.Architecture.Core.Doc\bin\Release\netcoreapp3.0\'

$files = 
'Fluent.Architecture.Core.dll','Fluent.Architecture.Core.dll',
'Fluent.Architecture.Core.Doc.dll', 'Fluent.Architecture.Core.Doc.dll'

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

