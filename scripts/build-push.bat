@echo off
setlocal

REM ===== Configuração =====
set IMAGE_NAME=marioguilhermedev/cidadeinteligentemvc
set TAG=%1
if "%TAG%"=="" set TAG=latest

echo.
echo === Building %IMAGE_NAME%:%TAG% ===
docker build -t %IMAGE_NAME%:%TAG% -f ./../CidadeInteligente.Mvc/Dockerfile ./../.
if errorlevel 1 (
    echo [ERRO] Build failed!
    exit /b 1
)

echo.
echo === Pushing to the registry ===
docker push %IMAGE_NAME%:%TAG%
if errorlevel 1 (
    echo [ERRO] Push failed! Did you run `docker login`?
    exit /b 1
)

echo.
echo [OK] Image %IMAGE_NAME%:%TAG% published!
