@echo off
setlocal

REM Run from the project root
cd /d %~dp0..

set PROJECT=CidadeInteligente.Mvc
set ENV_FILE=./.env
set DB_SERVER=localhost,1433

if not exist %ENV_FILE% (
    echo [ERRO] %ENV_FILE% not found in the project root!
    exit /b 1
)

REM ===== Loads the .env file as script variables =====
REM eol=# ignores comments; tokens=1* preserves values ​​containing '='
for /f "usebackq eol=# tokens=1* delims==" %%a in ("%ENV_FILE%") do set "%%a=%%b"

echo.
echo === Initializing user-secrets in %PROJECT% ===
dotnet user-secrets init --project %PROJECT%

echo.
echo === Defining secrets ===
dotnet user-secrets set "ConnectionStrings:Database" "Server=%DB_SERVER%;Database=%DB_NAME%;User Id=%DB_USER%;Password=%DB_PASSWORD%;TrustServerCertificate=True" --project %PROJECT%

dotnet user-secrets set "ConnectionStrings:FileStorage" "%ConnectionStrings__FileStorage%" --project %PROJECT%

dotnet user-secrets set "SendGrid:ApiKey" "%SendGrid__ApiKey%" --project %PROJECT%

echo.
echo === Secrets configured: ===
dotnet user-secrets list --project %PROJECT%
