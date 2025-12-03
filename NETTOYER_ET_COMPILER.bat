@echo off
echo ========================================
echo NETTOYAGE ET COMPILATION DU PROJET
echo ========================================
echo.

echo Etape 1: Suppression des dossiers bin et obj...
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"
echo Dossiers supprimes avec succes!
echo.

echo Etape 2: Compilation du projet...
msbuild LocationVoitures.BackOffice.csproj /p:Configuration=Debug /t:Rebuild

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ========================================
    echo COMPILATION REUSSIE!
    echo ========================================
    echo Vous pouvez maintenant executer l'application.
) else (
    echo.
    echo ========================================
    echo ERREUR DE COMPILATION
    echo ========================================
    echo Regardez les messages d'erreur ci-dessus.
)

pause

