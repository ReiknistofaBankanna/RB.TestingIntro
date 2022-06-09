1) Búa til möppu og flytja sig í hana
2) Opna cmd og keyra
   dotnet new console --framework net6.0
3) Opna möppu í Visual Code
4) Opna Terminal og keyra 
   dotnet run
5) Bætum við pökkum
   dotnet add package Microsoft.NET.Test.Sdk
   dotnet add package xunit
   dotnet add package xunit.runner.visualstudio
   dotnet add package coverlet.collector
   dotnet add package Moq
6) Búum til próf
7) Keyrum prófin
   dotnet test
8) Gerum test report
   dotnet test --collect:"XPlat Code Coverage"
   dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura
9) Setjum upp visalizer
   dotnet tool install -g dotnet-reportgenerator-globaltool
10) Búum til visual skýrslu
    reportgenerator -reports:".\TestResults\1cb3631f-4b47-4562-9b6d-8b79430b62db\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html

HEIMILDIR
---------
https://dev.to/moe23/learn-unit-test-with-net-6-with-xunit-and-moq-k9i
https://dotnetcoretutorials.com/2021/06/19/mocks-vs-stubs-vs-fakes-in-unit-testing/
https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=windows
