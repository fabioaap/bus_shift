/**
 * GitHub Copilot Transformer - Custom Agent profile format (.agent.md)
 * @story GH-COPILOT-1 - GitHub Copilot IDE Support
 *
 * Format: Frontmatter YAML + system prompt (agent profile)
 * Target: .github/agents/*.agent.md
 *
 * Works with:
 *   - GitHub Copilot CLI  (/agent command or --agent flag)
 *   - VS Code Copilot Chat (agents dropdown)
 *   - JetBrains, Eclipse, Xcode
 *   - GitHub.com Copilot coding agent
 *
 * Ref: https://docs.github.com/en/copilot/how-tos/use-copilot-agents/coding-agent/create-custom-agents
 */

const { getVisibleCommands, normalizeCommands } = require('../agent-parser');

/**
 * Map AIOS agent id to relevant VS Code Copilot tools
 * @param {string} agentId - AIOS agent id
 * @returns {string[]} - Array of tool names
 */
function getToolsForAgent(agentId) {
  const toolMap = {
    dev:              ['codebase', 'editFiles', 'runCommands', 'search', 'problems', 'findTestFiles', 'usages', 'terminalLastCommand'],
    qa:               ['codebase', 'problems', 'runCommands', 'search', 'findTestFiles', 'usages', 'terminalLastCommand'],
    architect:        ['codebase', 'search', 'problems', 'usages'],
    devops:           ['runCommands', 'terminalLastCommand', 'codebase', 'problems', 'scm'],
    'data-engineer':  ['codebase', 'editFiles', 'runCommands', 'search', 'problems'],
    pm:               ['codebase', 'search'],
    po:               ['codebase', 'search', 'editFiles'],
    sm:               ['codebase', 'search', 'editFiles'],
    analyst:          ['codebase', 'search'],
    'ux-design-expert': ['codebase', 'search', 'editFiles', 'problems'],
    'aios-master':    ['codebase', 'editFiles', 'runCommands', 'search', 'problems', 'findTestFiles', 'usages', 'terminalLastCommand', 'scm'],
    'squad-creator':  ['codebase', 'search', 'editFiles'],
  };

  return toolMap[agentId] || ['codebase', 'search', 'editFiles'];
}

/**
 * Transform agent data to GitHub Copilot chatmode format
 * @param {object} agentData - Parsed agent data from agent-parser
 * @returns {string} - Transformed content (.chatmode.md)
 */
function transform(agentData) {
  const agent = agentData.agent || {};
  const persona = agentData.persona_profile || {};

  const name = agent.name || agentData.id;
  const title = agent.title || 'AIOS Agent';
  const icon = agent.icon || '🤖';
  const whenToUse = agent.whenToUse || `Use this agent for ${title.toLowerCase()} tasks`;
  const archetype = persona.archetype || '';

  const tools = getToolsForAgent(agentData.id);
  const toolsYaml = `['${tools.join("', '")}']`;

  // Description shown in VS Code mode selector (keep under 100 chars)
  const description = `${icon} AIOS ${name}${archetype ? ` (${archetype})` : ''} - ${title}`;
  const truncatedDesc = description.length > 100 ? description.slice(0, 97) + '...' : description;

  // Build frontmatter
  let content = `---
description: ${truncatedDesc}
tools: ${toolsYaml}
---

`;

  // Agent identity header
  content += `You are **${name}**, the AIOS ${title}.

> ${whenToUse}

`;

  // Core instructions — pull from raw sections if available
  if (agentData.sections.guide) {
    content += `## Core Instructions\n\n${agentData.sections.guide}\n\n`;
  }

  // Commands as quick reference
  const allCommands = normalizeCommands(agentData.commands || []);
  const quickCommands = getVisibleCommands(allCommands, 'quick');

  if (quickCommands.length > 0) {
    content += `## Quick Commands\n\n`;
    for (const cmd of quickCommands) {
      content += `- \`*${cmd.name}\` — ${cmd.description || 'No description'}\n`;
    }
    content += '\n';
  }

  // Collaboration
  if (agentData.sections.collaboration) {
    content += `## Collaboration\n\n${agentData.sections.collaboration}\n\n`;
  }

  content += `---
*AIOS Agent - Synced from .aios-core/development/agents/${agentData.filename}*
*Works with: GH Copilot CLI (/agent), VS Code, JetBrains, Eclipse, Xcode, GitHub.com*
`;

  return content;
}

/**
 * Get the target filename for this agent
 * GitHub Copilot custom agents use .agent.md extension
 * @param {object} agentData - Parsed agent data
 * @returns {string} - Target filename (e.g., dev.agent.md)
 */
function getFilename(agentData) {
  const base = path.basename(agentData.filename, '.md');
  return `${base}.agent.md`;
}

// path required for getFilename
const path = require('path');

module.exports = {
  transform,
  getFilename,
  format: 'chatmode-md',
};
