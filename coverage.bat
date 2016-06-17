mkdir TestResults

.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -returntargetcode -register:user -filter:"+[*]* -[Tests*]*" "-target:C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" "-targetargs:.\src\Tests\Tests.Model\bin\Debug\Tests.Model.dll .\src\Tests\Tests.SpotifyMuter.Logic\bin\Debug\Tests.SpotifyMuter.Logic.dll" "-output:.\TestResults\coverage.xml"

.\packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe "-reports:.\TestResults\coverage.xml" "-targetdir:.\TestResults\Coverage"

pause