# -*- coding: utf-8 -*-
"""
Add all issues to GitHub Project Board
"""

import sys
import io
sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8', errors='replace')

import requests
import json
import time
import argparse

API_BASE = "https://api.github.com"

def get_issues(owner: str, repo: str, token: str) -> list:
    """Get all issues from repository"""
    headers = {
        "Authorization": f"token {token}",
        "Accept": "application/vnd.github.v3+json"
    }
    
    issues = []
    page = 1
    
    while True:
        r = requests.get(
            f"{API_BASE}/repos/{owner}/{repo}/issues?state=all&per_page=100&page={page}",
            headers=headers
        )
        
        if r.status_code != 200:
            print(f"Erro ao buscar issues: {r.status_code}")
            break
            
        data = r.json()
        if not data:
            break
            
        issues.extend(data)
        page += 1
    
    return issues

def add_to_project(owner: str, repo: str, project_number: int, token: str, issue_numbers: list):
    """Add issues to GitHub Project"""
    headers = {
        "Authorization": f"token {token}",
        "Accept": "application/vnd.github.v3+json"
    }
    
    print(f"Adicionando {len(issue_numbers)} issues ao Project #{project_number}...")
    
    success = 0
    for issue_num in issue_numbers:
        try:
            # GraphQL para adicionar issue ao project
            query = f"""
            mutation {{
              addProjectV2ItemById(input: {{
                projectId: "PVT_kwEOBpTEks4Az6zA"
                contentId: "I_{issue_num}"
              }}) {{
                item {{
                  id
                }}
              }}
            }}
            """
            
            r = requests.post(
                f"{API_BASE}/graphql",
                headers={**headers, "Content-Type": "application/json"},
                json={"query": query}
            )
            
            if r.status_code == 200:
                data = r.json()
                if 'errors' in data:
                    print(f"  Erro na issue #{issue_num}: {data['errors']}")
                else:
                    print(f"  OK Issue #{issue_num}")
                    success += 1
            else:
                print(f"  Erro HTTP {r.status_code} na issue #{issue_num}")
                
        except Exception as e:
            print(f"  Excecao na issue #{issue_num}: {e}")
        
        time.sleep(0.5)
    
    print(f"\nCompleto: {success}/{len(issue_numbers)} issues adicionadas ao projeto")

def main():
    parser = argparse.ArgumentParser(description='Adiciona issues ao GitHub Project')
    parser.add_argument('--token', required=True, help='GitHub Personal Access Token')
    parser.add_argument('--owner', required=True, help='Repository owner')
    parser.add_argument('--repo', required=True, help='Repository name')
    parser.add_argument('--project', type=int, required=True, help='Project number')
    
    args = parser.parse_args()
    
    print(f"\nBuscando issues do repositorio {args.owner}/{args.repo}...")
    issues = get_issues(args.owner, args.repo, args.token)
    
    # Filter issues criadas por bus_shift (pulsar issue #1 que eh de outro contexto)
    issue_numbers = [i['number'] for i in issues if i['number'] > 1]
    
    print(f"Encontradas {len(issue_numbers)} issues\n")
    
    add_to_project(args.owner, args.repo, args.project, args.token, issue_numbers)

if __name__ == "__main__":
    main()
