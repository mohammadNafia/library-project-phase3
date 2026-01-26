# Script to kill process using a specific port
param(
    [Parameter(Mandatory=$true)]
    [int]$Port
)

Write-Host "Finding process using port $Port..." -ForegroundColor Yellow

$processes = netstat -ano | findstr ":$Port"
if ($processes) {
    $pids = $processes | ForEach-Object {
        $_.Split(' ', [StringSplitOptions]::RemoveEmptyEntries)[-1]
    } | Select-Object -Unique

    foreach ($pid in $pids) {
        try {
            $process = Get-Process -Id $pid -ErrorAction SilentlyContinue
            if ($process) {
                Write-Host "Killing process $pid ($($process.ProcessName))..." -ForegroundColor Red
                Stop-Process -Id $pid -Force
                Write-Host "Process $pid killed successfully!" -ForegroundColor Green
            }
        }
        catch {
            Write-Host "Could not kill process $pid : $_" -ForegroundColor Red
        }
    }
    
    Start-Sleep -Seconds 1
    Write-Host "Checking if port $Port is now free..." -ForegroundColor Yellow
    $check = netstat -ano | findstr ":$Port"
    if (-not $check) {
        Write-Host "Port $Port is now free!" -ForegroundColor Green
    } else {
        Write-Host "Port $Port is still in use." -ForegroundColor Red
    }
} else {
    Write-Host "No process found using port $Port" -ForegroundColor Green
}
