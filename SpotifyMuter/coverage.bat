.\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe -register:user -filter:"+[*]* -[Tests*]*" "-target:C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\MSTest.exe" "-targetargs:/testcontainer:.\src\Tests\Tests.Model\bin\Debug\Tests.Model.dll"

.\packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe "-reports:results.xml" "-targetdir:.\coverage"