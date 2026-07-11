@echo off
setlocal

set K8S_DIR=./../k8s/

echo.
echo === Applying manifests from %K8S_DIR%/ ===
kubectl apply -f %K8S_DIR%/
if errorlevel 1 (
    echo [ERRO] Failed to apply the manifests!
    exit /b 1
)

echo.
echo === Status dos pods ===
kubectl get pods

echo.
echo [OK] Deployment complete!
