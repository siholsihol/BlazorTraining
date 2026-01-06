Write-Host "Cleaning bin and obj folders..." -ForegroundColor Cyan

Get-ChildItem -Path . -Recurse -Directory -Include bin, obj |
Where-Object {
    $_.FullName -notmatch '\\node_modules\\'
} |
ForEach-Object {
    Write-Host "Deleting $($_.FullName)"
    Remove-Item $_.FullName -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host "Clean complete." -ForegroundColor Green
