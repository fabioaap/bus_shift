# QA Task: Web Security Scan (Webscan)

**Task ID:** `qa-webscan`
**Agent:** @qa
**Command:** `*webscan`
**Version:** 1.0.0
**Status:** Active

---

## Purpose

Executes a targeted web-security scan against a running application or deployed URL.
Covers OWASP Top-10 surface areas, exposed headers, cookie security flags, open redirect
vectors, and TLS/HTTPS configuration.

**Strategy:** Automated scanning with structured finding report. No manual interaction
required unless a finding needs triage confirmation.

---

## Execution Modes

**Choose your execution mode:**

### 1. YOLO Mode - Fast, Autonomous (0-1 prompts)
- Autonomous decision making with logging
- Minimal user interaction
- **Best for:** CI pipelines, pre-deploy gates, routine scans

### 2. Interactive Mode - Balanced, Educational (5-10 prompts) **[DEFAULT]**
- Explicit decision checkpoints
- Educational explanations of each finding
- **Best for:** Learning, first-time scans on a new service

### 3. Pre-Flight Planning - Comprehensive Upfront Planning
- Scope definition before scanning
- Zero ambiguity on targets/exclusions
- **Best for:** Scans on production or regulated environments

---

## Inputs Required

| Input | Source | Required |
|-------|--------|----------|
| Target URL / base URL | User prompt or story context | ✅ |
| Auth headers / session cookies | `.env` or story context | Optional |
| Exclusion paths | Story context | Optional |

---

## Steps

### Step 1 — Resolve Target

1. Extract target URL from prompt / story context.
2. If not provided, check `PROJECT_URL` in `.env.example` or project config.
3. Confirm scheme is `https://` (flag plain `http://` as a finding).

### Step 2 — Header Analysis

Check HTTP response headers for security posture:

- `Strict-Transport-Security` (HSTS) present and `max-age` ≥ 31536000
- `Content-Security-Policy` (CSP) present and non-trivial
- `X-Frame-Options` or CSP `frame-ancestors` directive
- `X-Content-Type-Options: nosniff`
- `Referrer-Policy` set
- `Permissions-Policy` set
- Server/version header disclosure (should be absent or masked)

### Step 3 — Cookie Security Flags

For any `Set-Cookie` headers:
- `Secure` flag present
- `HttpOnly` flag present
- `SameSite` set to `Strict` or `Lax`

### Step 4 — TLS / Certificate Check

- TLS 1.2+ enforced, TLS 1.0/1.1 disabled
- Certificate valid and not expiring within 30 days
- No mixed-content warnings

### Step 5 — Common Vulnerability Surface Checks

| Check | Method |
|-------|--------|
| Open redirect | Append `?redirect=https://evil.example` to common endpoints |
| Clickjacking | Verify `X-Frame-Options` or CSP `frame-ancestors` |
| CORS misconfiguration | Check `Access-Control-Allow-Origin: *` on authenticated routes |
| Exposed `.env` / `.git` | Request `/.env`, `/.git/config`, `/config.json` |
| Directory listing | Check common paths for index listing |
| Error disclosure | Trigger 4xx/5xx and check for stack traces |

### Step 6 — Generate Findings Report

Produce a structured markdown report:

```markdown
## Webscan Report — {TARGET_URL}

**Date:** {ISO date}
**Severity Summary:** Critical: N | High: N | Medium: N | Low: N | Info: N

### Findings

| # | Severity | Category | Description | Recommendation |
|---|----------|----------|-------------|----------------|
| 1 | HIGH | Missing Header | HSTS not set | Add `Strict-Transport-Security` header |
...

### Passed Checks
- ✅ X-Content-Type-Options: nosniff present
- ✅ Cookies have Secure + HttpOnly flags
...
```

### Step 7 — Gate Decision

| Condition | Decision |
|-----------|----------|
| Any **Critical** finding | **FAIL** |
| Any **High** finding unmitigated | **NEEDS_WORK** |
| Only Medium/Low/Info findings | **APPROVED** (with notes) |
| No findings | **APPROVED** |

---

## Output

- Inline findings report in the chat / story QA section
- Gate decision: **APPROVED** / **NEEDS_WORK** / **FAIL**

---

## References

- OWASP Testing Guide: https://owasp.org/www-project-web-security-testing-guide/
- Mozilla Observatory: https://observatory.mozilla.org
- Related task: `security-scan.md` (SAST / static analysis)
