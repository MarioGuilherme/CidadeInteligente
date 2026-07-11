@echo off
setlocal

set SECRET_NAME=cidade-inteligente-mvc-secret
set ENV_FILE=./../.env

if not exist %ENV_FILE% (
    echo [ERRO] File %ENV_FILE% not found!
    echo Copy the .env.example file and fill in the values..
    exit /b 1
)

echo.
echo === Creating/updating secret %SECRET_NAME% ===
kubectl create secret generic %SECRET_NAME% --from-env-file=%ENV_FILE% --dry-run=client -o yaml | kubectl apply -f -
if errorlevel 1 (
    echo [ERRO] Failed to create the secret. Is the cluster running?
    exit /b 1
)

echo.
echo [OK] Secret applied!!
