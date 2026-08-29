<#
.SYNOPSIS
    Queries GitHub REST API for baerenbude-org/hexchat using token from .env.
.DESCRIPTION
    Loads .env file from repository root and provides easy commands to inspect:
    - Actions runs and logs
    - Pull Requests
    - Issues
    - Security alerts (Dependabot, CodeQL, Secret scanning)
.EXAMPLE
    .\tools\Get-GitHubInfo.ps1 -Action runs
    .\tools\Get-GitHubInfo.ps1 -Action run-logs -RunId 123456789
    .\tools\Get-GitHubInfo.ps1 -Action prs
    .\tools\Get-GitHubInfo.ps1 -Action security
#>

[CmdletBinding()]
param(
    [Parameter(Position = 0)]
    [ValidateSet('runs', 'run-logs', 'prs', 'security', 'issues', 'status')]
    [string]$Action = 'status',

    [Parameter()]
    [string]$RunId = '',

    [Parameter()]
    [string]$EnvPath = ''
)

if (-not $EnvPath) {
    $EnvPath = Join-Path $PSScriptRoot "..\.env"
}

# Load .env file if present
function Load-DotEnv([string]$path) {
    if (Test-Path $path) {
        Get-Content $path | ForEach-Object {
            $line = $_.Trim()
            if ($line -and -not $line.StartsWith('#') -and $line.Contains('=')) {
                $parts = $line.Split('=', 2)
                $key = $parts[0].Trim()
                $val = $parts[1].Trim().Trim('"').Trim("'")
                [System.Environment]::SetEnvironmentVariable($key, $val, [System.EnvironmentVariableTarget]::Process)
            }
        }
    }
}

Load-DotEnv -path $EnvPath

$token = [System.Environment]::GetEnvironmentVariable('GITHUB_TOKEN')
$repo = [System.Environment]::GetEnvironmentVariable('GITHUB_REPOSITORY')
if (-not $repo) { $repo = 'baerenbude-org/hexchat' }

if (-not $token -or $token -eq 'ghp_your_token_here') {
    Write-Warning "Kein gueltiger GITHUB_TOKEN in '$EnvPath' gefunden."
    Write-Host "Bitte erstelle eine .env Datei (Vorlage: .env.example) mit deinem GitHub Personal Access Token." -ForegroundColor Yellow
    exit 1
}

$headers = @{
    'Authorization' = "Bearer $token"
    'Accept'        = 'application/vnd.github+json'
    'User-Agent'    = 'HexChat-Dev-Assistant'
}

$baseUrl = "https://api.github.com/repos/$repo"

switch ($Action) {
    'status' {
        Write-Host "=== GitHub Repository Status: $repo ===" -ForegroundColor Cyan
        try {
            $repoInfo = Invoke-RestMethod -Uri $baseUrl -Headers $headers -Method Get
            [PSCustomObject]@{
                FullName       = $repoInfo.full_name
                Private        = $repoInfo.private
                DefaultBranch  = $repoInfo.default_branch
                OpenIssues     = $repoInfo.open_issues_count
                Forks          = $repoInfo.forks_count
                Stargazers     = $repoInfo.stargazers_count
            } | Format-List
        } catch {
            Write-Error "Fehler beim Abrufen des Repository-Status: $_"
        }
    }

    'runs' {
        Write-Host "=== Neueste GitHub Actions Workflow Runs ($repo) ===" -ForegroundColor Cyan
        try {
            $response = Invoke-RestMethod -Uri "$baseUrl/actions/runs?per_page=10" -Headers $headers -Method Get
            $response.workflow_runs | Select-Object id, name, head_branch, status, conclusion, created_at, html_url | Format-Table -AutoSize
        } catch {
            Write-Error "Fehler beim Abrufen der Workflow Runs: $_"
        }
    }

    'run-logs' {
        if (-not $RunId) {
            Write-Error "Bitte -RunId angeben (z. B. .\tools\Get-GitHubInfo.ps1 -Action run-logs -RunId 123456)"
            exit 1
        }
        Write-Host "=== Jobs und Logs fuer Run ID $RunId ===" -ForegroundColor Cyan
        try {
            $jobsResponse = Invoke-RestMethod -Uri "$baseUrl/actions/runs/$RunId/jobs" -Headers $headers -Method Get
            foreach ($job in $jobsResponse.jobs) {
                Write-Host "`nJob: $($job.name) [Status: $($job.status), Conclusion: $($job.conclusion)]" -ForegroundColor Green
                foreach ($step in $job.steps) {
                    $statusColor = if ($step.conclusion -eq 'success') { 'Green' } elseif ($step.conclusion -eq 'failure') { 'Red' } else { 'Yellow' }
                    Write-Host "  - Step: $($step.name) -> $($step.conclusion)" -ForegroundColor $statusColor
                }
            }
        } catch {
            Write-Error "Fehler beim Abrufen der Run-Details: $_"
        }
    }

    'prs' {
        Write-Host "=== Pull Requests ($repo) ===" -ForegroundColor Cyan
        try {
            $prs = Invoke-RestMethod -Uri "$baseUrl/pulls?state=all&per_page=10" -Headers $headers -Method Get
            if ($prs.Count -eq 0) {
                Write-Host "Keine Pull Requests vorhanden." -ForegroundColor Yellow
            } else {
                $prs | Select-Object number, title, state, @{Name='User';Expression={$_.user.login}}, head_branch, created_at, html_url | Format-Table -AutoSize
            }
        } catch {
            Write-Error "Fehler beim Abrufen der PRs: $_"
        }
    }

    'security' {
        Write-Host "=== Dependabot & Security Alerts ($repo) ===" -ForegroundColor Cyan
        try {
            Write-Host "`n[Dependabot Alerts]" -ForegroundColor Yellow
            $depAlerts = Invoke-RestMethod -Uri "$baseUrl/dependabot/alerts?state=open" -Headers $headers -Method Get -ErrorAction SilentlyContinue
            if ($depAlerts.Count -gt 0) {
                $depAlerts | Select-Object number, state, @{Name='Package';Expression={$_.security_vulnerability.package.name}}, @{Name='Severity';Expression={$_.security_advisory.severity}}, @{Name='Summary';Expression={$_.security_advisory.summary}} | Format-Table -AutoSize
            } else {
                Write-Host "Keine offenen Dependabot-Warnungen gefunden." -ForegroundColor Green
            }
        } catch {
            Write-Warning "Dependabot Alerts nicht verfuegbar oder unzureichende Berechtigungen."
        }

        try {
            Write-Host "`n[Code Scanning Alerts (CodeQL)]" -ForegroundColor Yellow
            $codeAlerts = Invoke-RestMethod -Uri "$baseUrl/code-scanning/alerts?state=open" -Headers $headers -Method Get -ErrorAction SilentlyContinue
            if ($codeAlerts.Count -gt 0) {
                $codeAlerts | Select-Object number, state, @{Name='Rule';Expression={$_.rule.description}}, @{Name='Severity';Expression={$_.rule.severity}} | Format-Table -AutoSize
            } else {
                Write-Host "Keine offenen Code-Scanning-Warnungen gefunden." -ForegroundColor Green
            }
        } catch {
            Write-Warning "Code Scanning Alerts nicht verfuegbar oder unzureichende Berechtigungen."
        }
    }

    'issues' {
        Write-Host "=== Offene Issues ($repo) ===" -ForegroundColor Cyan
        try {
            $issues = Invoke-RestMethod -Uri "$baseUrl/issues?state=open&per_page=15" -Headers $headers -Method Get
            if ($issues.Count -eq 0) {
                Write-Host "Keine offenen Issues gefunden." -ForegroundColor Green
            } else {
                $issues | Where-Object { -not $_.pull_request } | Select-Object number, title, @{Name='User';Expression={$_.user.login}}, comments, created_at, html_url | Format-Table -AutoSize
            }
        } catch {
            Write-Error "Fehler beim Abrufen der Issues: $_"
        }
    }
}
