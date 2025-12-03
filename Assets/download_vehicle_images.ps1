param(
    [string]$TargetFolder = ".\Assets\Vehicles"
)

Write-Host "=== Installation des outils nécessaires (via winget) ==="
try {
    winget --version *> $null
} catch {
    Write-Warning "winget n'est pas disponible sur ce système. Assurez-vous d'être sous Windows 10/11 récent."
}

# S'assurer que curl est disponible
try {
    curl --version *> $null
} catch {
    Write-Host "Installation de curl via winget..."
    winget install --id curl.curl -e --source winget
}

Write-Host "=== Téléchargement d'images libres de véhicules (Unsplash) ==="

$urls = @(
    "https://images.unsplash.com/photo-1502877338535-766e1452684a?auto=format&fit=crop&w=1200&q=80",
    "https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=1200&q=80",
    "https://images.unsplash.com/photo-1511919884226-fd3cad34687c?auto=format&fit=crop&w=1200&q=80",
    "https://images.unsplash.com/photo-1549921296-3c5282e87aeb?auto=format&fit=crop&w=1200&q=80"
)

if (-not (Test-Path $TargetFolder)) {
    New-Item -ItemType Directory -Path $TargetFolder | Out-Null
}

$i = 1
foreach ($url in $urls) {
    $file = Join-Path $TargetFolder ("vehicule_" + $i + ".jpg")
    Write-Host "Téléchargement de $url -> $file"
    curl -L $url -o $file
    $i++
}

Write-Host ""
Write-Host "Téléchargement terminé."
Write-Host "Dossier : $TargetFolder"
Write-Host "Vous pouvez maintenant utiliser ces images dans l'application (ex: liste des types de véhicules)."


