@echo off
SET ROOT=C:\Users\Difficult\Enterprise-Workflow-Platform

echo Добавляем .gitkeep во все подпапки в src...

for /R "%ROOT%" %%F in (.) do (
    if exist "%%F" (
        type nul > "%%F\.gitkeep"
    )
)

echo Готово! .gitkeep добавлен во все подпапки.
pause