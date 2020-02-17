$dirBase = "D:\Projetos\dn32\Fluente.Arquitetura\"

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
dotnet clean "$dirBase\Fluente.Arquitetura.sln"

echo "Build 2.2"
$result = dotnet build "$dirBase\Fluente.Arquitetura.sln" --configuration Release /p:CopyOutputSymbolsToPublishDirectory=false --framework netcoreapp2.2
CheckResult $result "Build 2.2 Sucess!"

echo "Build 3.0"
$result = dotnet build "$dirBase\Fluente.Arquitetura.sln" --configuration Release /p:CopyOutputSymbolsToPublishDirectory=false --framework netcoreapp3.1
CheckResult $result "Build 3.1 Sucess!"

echo "Cript"

$dirsPack = 
'Fluente.Arquitetura',
'Fluente.Arquitetura.Base',
'Fluente.Arquitetura.Nucleo.Doc', 
'Fluente.Arquitetura.EntityFramework',
'Fluente.Arquitetura.EntityFramework.MemoryDatabase',
'Fluente.Arquitetura.EntityFramework.MySQL',
'Fluente.Arquitetura.EntityFramework.Oracle',
'Fluente.Arquitetura.EntityFramework.PostgreSQL',
'Fluente.Arquitetura.EntityFramework.SqLite',
'Fluente.Arquitetura.EntityFramework.SqlServer',
'Fluente.Arquitetura.Redis',
'Fluente.Arquitetura.Test'

$dirs = 
'Fluente.Arquitetura\bin\Release\netcoreapp2.2\','Fluente.Arquitetura\bin\Release\netcoreapp3.1\', 
'Fluente.Arquitetura.Nucleo.Doc\bin\Release\netcoreapp2.2\','Fluente.Arquitetura.Nucleo.Doc\bin\Release\netcoreapp3.1\'

$files = 
'Fluente.Arquitetura.Nucleo.dll','Fluente.Arquitetura.Nucleo.dll',
'Fluente.Arquitetura.Nucleo.Doc.dll', 'Fluente.Arquitetura.Nucleo.Doc.dll'

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

