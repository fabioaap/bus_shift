/**
 * AIOS Elicitation Module — Canonical Consolidated Index
 *
 * Consolidates agent, task, and workflow elicitation workflows into a single
 * canonical export. Previously these lived in three separate files:
 *   - agent-elicitation.js   → agentElicitationSteps
 *   - task-elicitation.js    → taskElicitationSteps
 *   - workflow-elicitation.js → workflowElicitationSteps
 *
 * Fixes #61: Consolidate 3 elicitation files
 *
 * @module elicitation
 */

// ---------------------------------------------------------------------------
// Agent Creation Elicitation Steps
// Progressive disclosure for creating new agents
// ---------------------------------------------------------------------------

const agentElicitationSteps = [
  {
    title: 'Basic Agent Information',
    description: 'Let\'s start with the fundamental details about your agent',
    help: 'An agent is a specialized AI assistant with a specific role and set of capabilities. Think of it as a team member with expertise in a particular area.',
    questions: [
      {
        type: 'input',
        name: 'agentName',
        message: 'What is the agent\'s name?',
        examples: ['data-analyst', 'code-reviewer', 'project-manager'],
        validate: (input) => {
          if (!input) return 'Agent name is required';
          if (!/^[a-z][a-z0-9-]*$/.test(input)) {
            return 'Name must be lowercase with hyphens only (e.g., data-analyst)';
          }
          return true;
        },
      },
      {
        type: 'input',
        name: 'agentTitle',
        message: 'What is the agent\'s professional title?',
        examples: ['Senior Data Analyst', 'Code Review Specialist', 'Project Manager'],
        smartDefault: {
          type: 'fromAnswer',
          source: 'agentName',
          transform: (name) => name.split('-').map(w =>
            w.charAt(0).toUpperCase() + w.slice(1),
          ).join(' '),
        },
      },
      {
        type: 'input',
        name: 'agentIcon',
        message: 'Choose an emoji icon for this agent',
        examples: ['📊', '🔍', '📋', '🚀', '🛡️'],
        default: '🤖',
      },
      {
        type: 'input',
        name: 'whenToUse',
        message: 'When should users activate this agent? (one line)',
        examples: [
          'Use for data analysis and visualization tasks',
          'Use for code review and quality assurance',
          'Use for project planning and tracking',
        ],
      },
    ],
    required: ['agentName', 'agentTitle', 'whenToUse'],
  },

  {
    title: 'Agent Persona & Style',
    description: 'Define how your agent communicates and behaves',
    help: 'The persona defines the agent\'s personality, communication style, and approach to tasks.',
    questions: [
      {
        type: 'input',
        name: 'personaRole',
        message: 'What is the agent\'s professional role?',
        examples: [
          'Expert Data Scientist & Analytics Specialist',
          'Senior Software Engineer & Code Quality Expert',
          'Agile Project Manager & Scrum Master',
        ],
      },
      {
        type: 'input',
        name: 'personaStyle',
        message: 'Describe their communication style',
        examples: [
          'analytical, precise, data-driven',
          'thorough, constructive, detail-oriented',
          'organized, proactive, collaborative',
        ],
        default: 'professional, helpful, focused',
      },
      {
        type: 'input',
        name: 'personaIdentity',
        message: 'What is their core identity? (one sentence)',
        examples: [
          'A data expert who transforms raw data into actionable insights',
          'A code quality guardian who ensures best practices',
          'A project orchestrator who keeps teams aligned and productive',
        ],
      },
      {
        type: 'list',
        name: 'personaFocus',
        message: 'What is their primary focus area?',
        choices: [
          'Technical implementation',
          'Analysis and insights',
          'Process and workflow',
          'Quality and standards',
          'Communication and documentation',
          'Other (specify)',
        ],
      },
      {
        type: 'input',
        name: 'personaFocusCustom',
        message: 'Specify the focus area:',
        when: (answers) => answers.personaFocus === 'Other (specify)',
      },
    ],
    required: ['personaRole', 'personaStyle', 'personaIdentity'],
  },

  {
    title: 'Agent Commands',
    description: 'Define what commands this agent will respond to',
    help: 'Commands are actions users can request from the agent. Each command should have a clear purpose.',
    questions: [
      {
        type: 'checkbox',
        name: 'standardCommands',
        message: 'Select standard commands to include:',
        choices: [
          { name: 'analyze - Perform analysis on data/code', value: 'analyze' },
          { name: 'create - Generate new content/files', value: 'create' },
          { name: 'review - Review existing work', value: 'review' },
          { name: 'suggest - Provide recommendations', value: 'suggest' },
          { name: 'explain - Explain concepts or code', value: 'explain' },
          { name: 'validate - Check for errors/issues', value: 'validate' },
          { name: 'report - Generate reports', value: 'report' },
        ],
        default: ['analyze', 'create', 'suggest'],
      },
      {
        type: 'confirm',
        name: 'addCustomCommands',
        message: 'Would you like to add custom commands?',
        default: false,
      },
      {
        type: 'input',
        name: 'customCommands',
        message: 'Enter custom commands (comma-separated, format: "name:description"):',
        when: (answers) => answers.addCustomCommands,
        examples: ['optimize:Optimize performance', 'debug:Debug issues'],
        filter: (input) => input.split(',').map(cmd => cmd.trim()),
      },
    ],
  },

  {
    title: 'Dependencies & Resources',
    description: 'Specify what resources this agent needs',
    help: 'Dependencies are tasks, templates, or data files the agent needs to function properly.',
    questions: [
      {
        type: 'checkbox',
        name: 'dependencyTypes',
        message: 'What types of dependencies does this agent need?',
        choices: [
          { name: 'Tasks - Reusable task workflows', value: 'tasks' },
          { name: 'Templates - Document/code templates', value: 'templates' },
          { name: 'Checklists - Quality checklists', value: 'checklists' },
          { name: 'Data - Reference data files', value: 'data' },
        ],
      },
      {
        type: 'input',
        name: 'taskDependencies',
        message: 'Enter task dependencies (comma-separated):',
        when: (answers) => answers.dependencyTypes.includes('tasks'),
        examples: ['analyze-data.md', 'generate-report.md'],
        filter: (input) => input ? input.split(',').map(t => t.trim()) : [],
      },
      {
        type: 'input',
        name: 'templateDependencies',
        message: 'Enter template dependencies (comma-separated):',
        when: (answers) => answers.dependencyTypes.includes('templates'),
        examples: ['report-template.md', 'analysis-template.yaml'],
        filter: (input) => input ? input.split(',').map(t => t.trim()) : [],
      },
    ],
  },

  {
    title: 'Security & Access Control',
    description: 'Configure security settings for this agent',
    help: 'Security settings control what the agent can access and who can use it.',
    condition: { field: 'agentName', operator: 'exists' },
    questions: [
      {
        type: 'list',
        name: 'securityLevel',
        message: 'What security level should this agent have?',
        choices: [
          { name: 'Standard - Default permissions', value: 'standard' },
          { name: 'Elevated - Additional capabilities', value: 'elevated' },
          { name: 'Restricted - Limited access', value: 'restricted' },
          { name: 'Custom - Define specific permissions', value: 'custom' },
        ],
        default: 'standard',
      },
      {
        type: 'confirm',
        name: 'requireAuthorization',
        message: 'Should this agent require special authorization to activate?',
        default: false,
        when: (answers) => answers.securityLevel !== 'standard',
      },
      {
        type: 'confirm',
        name: 'enableAuditLogging',
        message: 'Enable audit logging for this agent\'s operations?',
        default: true,
        when: (answers) => answers.securityLevel !== 'standard',
      },
      {
        type: 'checkbox',
        name: 'allowedOperations',
        message: 'Select allowed operations:',
        when: (answers) => answers.securityLevel === 'custom',
        choices: [
          'file_read',
          'file_write',
          'file_delete',
          'execute_commands',
          'network_access',
          'memory_access',
          'manifest_update',
        ],
      },
    ],
  },

  {
    title: 'Advanced Options',
    description: 'Configure advanced agent features',
    condition: { field: 'securityLevel', operator: 'notEquals', value: 'standard' },
    questions: [
      {
        type: 'confirm',
        name: 'enableMemoryLayer',
        message: 'Enable memory layer integration?',
        default: true,
      },
      {
        type: 'input',
        name: 'corePrinciples',
        message: 'Enter core principles for this agent (comma-separated):',
        examples: [
          'Always validate data before processing',
          'Follow security best practices',
          'Provide clear explanations',
        ],
        filter: (input) => input ? input.split(',').map(p => p.trim()) : [],
      },
      {
        type: 'input',
        name: 'customActivationInstructions',
        message: 'Any special activation instructions? (optional)',
        examples: ['Load specific context on activation', 'Initialize connections'],
      },
    ],
  },
];

// ---------------------------------------------------------------------------
// Task Creation Elicitation Steps
// Progressive disclosure for creating new tasks
// ---------------------------------------------------------------------------

const taskElicitationSteps = [
  {
    title: 'Basic Task Information',
    description: 'Define the fundamental details of your task',
    help: 'A task is a reusable workflow that an agent can execute. It should have a clear purpose and outcome.',
    questions: [
      {
        type: 'input',
        name: 'taskId',
        message: 'What is the task ID?',
        examples: ['analyze-data', 'generate-report', 'validate-code'],
        validate: (input) => {
          if (!input) return 'Task ID is required';
          if (!/^[a-z][a-z0-9-]*$/.test(input)) {
            return 'ID must be lowercase with hyphens only';
          }
          return true;
        },
      },
      {
        type: 'input',
        name: 'taskTitle',
        message: 'What is the task title?',
        examples: ['Analyze Data Set', 'Generate Status Report', 'Validate Code Quality'],
        smartDefault: {
          type: 'fromAnswer',
          source: 'taskId',
          transform: (id) => id.split('-').map(w =>
            w.charAt(0).toUpperCase() + w.slice(1),
          ).join(' '),
        },
      },
      {
        type: 'input',
        name: 'agentName',
        message: 'Which agent will own this task?',
        examples: ['data-analyst', 'report-generator', 'code-reviewer'],
      },
      {
        type: 'input',
        name: 'taskDescription',
        message: 'Describe the task purpose (2-3 sentences):',
        validate: (input) => input.length > 10 || 'Please provide a meaningful description',
      },
    ],
    required: ['taskId', 'taskTitle', 'agentName', 'taskDescription'],
  },

  {
    title: 'Task Context & Prerequisites',
    description: 'Define what\'s needed before the task can run',
    questions: [
      {
        type: 'confirm',
        name: 'requiresContext',
        message: 'Does this task require specific context or input?',
        default: true,
      },
      {
        type: 'input',
        name: 'contextDescription',
        message: 'What context/input is required?',
        when: (answers) => answers.requiresContext,
        examples: ['Data file path and format', 'Project configuration', 'User preferences'],
      },
      {
        type: 'checkbox',
        name: 'prerequisites',
        message: 'Select prerequisites for this task:',
        choices: [
          'Valid file path provided',
          'Required permissions granted',
          'Dependencies installed',
          'Configuration loaded',
          'Previous task completed',
          'User authentication',
          'Network connectivity',
        ],
      },
      {
        type: 'input',
        name: 'customPrerequisites',
        message: 'Any additional prerequisites? (comma-separated):',
        filter: (input) => input ? input.split(',').map(p => p.trim()) : [],
      },
    ],
  },

  {
    title: 'Task Workflow',
    description: 'Define how the task should be executed',
    questions: [
      {
        type: 'confirm',
        name: 'isInteractive',
        message: 'Is this an interactive task (requires user input)?',
        default: false,
      },
      {
        type: 'list',
        name: 'workflowType',
        message: 'What type of workflow is this?',
        choices: [
          { name: 'Sequential - Steps run in order', value: 'sequential' },
          { name: 'Conditional - Steps depend on conditions', value: 'conditional' },
          { name: 'Iterative - Steps may repeat', value: 'iterative' },
          { name: 'Parallel - Some steps run simultaneously', value: 'parallel' },
        ],
        default: 'sequential',
      },
      {
        type: 'input',
        name: 'stepCount',
        message: 'How many main steps does this task have?',
        default: '3',
        validate: (input) => {
          const num = parseInt(input);
          return (num > 0 && num <= 10) || 'Please enter a number between 1 and 10';
        },
        filter: (input) => parseInt(input),
      },
    ],
  },

  {
    title: 'Define Task Steps',
    description: 'Specify each step in the workflow',
    questions: [
      {
        type: 'input',
        name: 'steps',
        message: 'This will be handled dynamically based on stepCount',
        // Note: In implementation, this would generate dynamic questions
        // based on the stepCount from previous step
      },
    ],
    validators: [
      {
        type: 'custom',
        validate: (_answers) => {
          // This would be implemented to collect step details
          return true;
        },
      },
    ],
  },

  {
    title: 'Output & Success Criteria',
    description: 'Define what the task produces and how to measure success',
    questions: [
      {
        type: 'input',
        name: 'outputDescription',
        message: 'What does this task output/produce?',
        examples: ['Analysis report in markdown', 'Validated data file', 'Test results summary'],
      },
      {
        type: 'list',
        name: 'outputFormat',
        message: 'What format is the output?',
        choices: [
          'Text/Markdown',
          'JSON',
          'YAML',
          'CSV',
          'HTML',
          'File(s)',
          'Console output',
          'Other',
        ],
      },
      {
        type: 'input',
        name: 'outputFormatCustom',
        message: 'Specify the output format:',
        when: (answers) => answers.outputFormat === 'Other',
      },
      {
        type: 'checkbox',
        name: 'successCriteria',
        message: 'Select success criteria:',
        choices: [
          'All steps completed without errors',
          'Output file(s) created successfully',
          'Validation checks passed',
          'Performance within limits',
          'User confirmation received',
          'Tests passed',
        ],
      },
    ],
    required: ['outputDescription'],
  },

  {
    title: 'Error Handling',
    description: 'Configure how the task handles errors',
    questions: [
      {
        type: 'list',
        name: 'errorStrategy',
        message: 'How should the task handle errors?',
        choices: [
          { name: 'Fail fast - Stop on first error', value: 'fail-fast' },
          { name: 'Collect errors - Continue and report all', value: 'collect' },
          { name: 'Retry - Attempt recovery', value: 'retry' },
          { name: 'Fallback - Use alternative approach', value: 'fallback' },
        ],
        default: 'fail-fast',
      },
      {
        type: 'input',
        name: 'retryCount',
        message: 'How many retry attempts?',
        when: (answers) => answers.errorStrategy === 'retry',
        default: '3',
        validate: (input) => {
          const num = parseInt(input);
          return (num > 0 && num <= 5) || 'Please enter 1-5';
        },
      },
      {
        type: 'checkbox',
        name: 'commonErrors',
        message: 'Select common errors to handle:',
        choices: [
          'File not found',
          'Permission denied',
          'Invalid format',
          'Network timeout',
          'Resource busy',
          'Validation failed',
          'Dependency missing',
        ],
      },
    ],
  },

  {
    title: 'Security & Validation',
    description: 'Configure security checks and validation',
    condition: { field: 'taskId', operator: 'exists' },
    questions: [
      {
        type: 'confirm',
        name: 'enableSecurityChecks',
        message: 'Enable security validation for inputs?',
        default: true,
      },
      {
        type: 'checkbox',
        name: 'securityChecks',
        message: 'Select security checks to apply:',
        when: (answers) => answers.enableSecurityChecks,
        choices: [
          'Input sanitization',
          'Path traversal prevention',
          'Command injection prevention',
          'File type validation',
          'Size limits',
          'Rate limiting',
        ],
        default: ['Input sanitization', 'Path traversal prevention'],
      },
      {
        type: 'confirm',
        name: 'enableExamples',
        message: 'Would you like to add usage examples?',
        default: false,
      },
    ],
  },
];

// ---------------------------------------------------------------------------
// Workflow Creation Elicitation Steps
// Progressive disclosure for creating new workflows
// ---------------------------------------------------------------------------

const workflowElicitationSteps = [
  {
    title: 'Basic Workflow Information',
    description: 'Define the core details of your workflow',
    help: 'A workflow orchestrates multiple tasks and agents to achieve a complex goal.',
    questions: [
      {
        type: 'input',
        name: 'workflowId',
        message: 'What is the workflow ID?',
        examples: ['data-pipeline', 'release-process', 'quality-check'],
        validate: (input) => {
          if (!input) return 'Workflow ID is required';
          if (!/^[a-z][a-z0-9-]*$/.test(input)) {
            return 'ID must be lowercase with hyphens only';
          }
          return true;
        },
      },
      {
        type: 'input',
        name: 'workflowName',
        message: 'What is the workflow name?',
        examples: ['Data Processing Pipeline', 'Software Release Process', 'Quality Assurance Workflow'],
        smartDefault: {
          type: 'fromAnswer',
          source: 'workflowId',
          transform: (id) => id.split('-').map(w =>
            w.charAt(0).toUpperCase() + w.slice(1),
          ).join(' '),
        },
      },
      {
        type: 'input',
        name: 'workflowDescription',
        message: 'Describe the workflow purpose:',
        validate: (input) => input.length > 20 || 'Please provide a detailed description',
      },
      {
        type: 'list',
        name: 'workflowType',
        message: 'What type of workflow is this?',
        choices: [
          { name: 'Sequential - Steps run one after another', value: 'sequential' },
          { name: 'Parallel - Multiple steps can run simultaneously', value: 'parallel' },
          { name: 'Conditional - Steps depend on conditions', value: 'conditional' },
          { name: 'Hybrid - Mix of sequential and parallel', value: 'hybrid' },
        ],
        default: 'sequential',
      },
    ],
    required: ['workflowId', 'workflowName', 'workflowDescription', 'workflowType'],
  },

  {
    title: 'Workflow Triggers',
    description: 'Define what starts this workflow',
    questions: [
      {
        type: 'checkbox',
        name: 'triggerTypes',
        message: 'How can this workflow be triggered?',
        choices: [
          { name: 'Manual - User command', value: 'manual' },
          { name: 'Schedule - Time-based', value: 'schedule' },
          { name: 'Event - System event', value: 'event' },
          { name: 'Webhook - External trigger', value: 'webhook' },
          { name: 'File - File system change', value: 'file' },
          { name: 'Completion - After another workflow', value: 'completion' },
        ],
        default: ['manual'],
      },
      {
        type: 'input',
        name: 'schedulePattern',
        message: 'Enter schedule pattern (cron format):',
        when: (answers) => answers.triggerTypes.includes('schedule'),
        examples: ['0 9 * * *', '*/30 * * * *', '0 0 * * SUN'],
        default: '0 9 * * *',
      },
      {
        type: 'input',
        name: 'eventTriggers',
        message: 'Which events trigger this workflow? (comma-separated):',
        when: (answers) => answers.triggerTypes.includes('event'),
        examples: ['file.created', 'task.completed', 'error.detected'],
        filter: (input) => input ? input.split(',').map(e => e.trim()) : [],
      },
    ],
  },

  {
    title: 'Workflow Inputs',
    description: 'Define input parameters for the workflow',
    questions: [
      {
        type: 'confirm',
        name: 'hasInputs',
        message: 'Does this workflow require input parameters?',
        default: true,
      },
      {
        type: 'input',
        name: 'inputCount',
        message: 'How many input parameters?',
        when: (answers) => answers.hasInputs,
        default: '2',
        validate: (input) => {
          const num = parseInt(input);
          return (num > 0 && num <= 10) || 'Please enter 1-10';
        },
        filter: (input) => parseInt(input),
      },
      {
        type: 'confirm',
        name: 'validateInputs',
        message: 'Add input validation rules?',
        when: (answers) => answers.hasInputs,
        default: true,
      },
    ],
  },

  {
    title: 'Workflow Steps',
    description: 'Define the steps in your workflow',
    questions: [
      {
        type: 'input',
        name: 'stepCount',
        message: 'How many steps in this workflow?',
        default: '3',
        validate: (input) => {
          const num = parseInt(input);
          return (num > 0 && num <= 20) || 'Please enter 1-20';
        },
        filter: (input) => parseInt(input),
      },
      {
        type: 'list',
        name: 'stepDefinitionMethod',
        message: 'How would you like to define steps?',
        choices: [
          { name: 'Quick mode - Basic step info only', value: 'quick' },
          { name: 'Detailed mode - Full step configuration', value: 'detailed' },
          { name: 'Import - Use existing task definitions', value: 'import' },
        ],
        default: 'quick',
      },
    ],
  },

  {
    title: 'Step Dependencies & Flow',
    description: 'Configure how steps relate to each other',
    condition: { field: 'workflowType', operator: 'notEquals', value: 'sequential' },
    questions: [
      {
        type: 'confirm',
        name: 'hasStepDependencies',
        message: 'Do steps have dependencies on other steps?',
        default: true,
      },
      {
        type: 'confirm',
        name: 'allowParallel',
        message: 'Can some steps run in parallel?',
        when: (answers) => answers.workflowType !== 'sequential',
        default: true,
      },
      {
        type: 'input',
        name: 'maxParallel',
        message: 'Maximum parallel executions:',
        when: (answers) => answers.allowParallel,
        default: '3',
        validate: (input) => {
          const num = parseInt(input);
          return (num > 0 && num <= 10) || 'Please enter 1-10';
        },
      },
    ],
  },

  {
    title: 'Error Handling & Recovery',
    description: 'Configure workflow error behavior',
    questions: [
      {
        type: 'list',
        name: 'globalErrorStrategy',
        message: 'How should the workflow handle errors?',
        choices: [
          { name: 'Abort - Stop entire workflow on error', value: 'abort' },
          { name: 'Continue - Log error and continue', value: 'continue' },
          { name: 'Rollback - Undo completed steps', value: 'rollback' },
          { name: 'Compensate - Run compensation steps', value: 'compensate' },
        ],
        default: 'abort',
      },
      {
        type: 'confirm',
        name: 'enableNotifications',
        message: 'Send notifications on workflow events?',
        default: true,
      },
      {
        type: 'checkbox',
        name: 'notificationEvents',
        message: 'Which events should trigger notifications?',
        when: (answers) => answers.enableNotifications,
        choices: [
          'Workflow started',
          'Workflow completed',
          'Workflow failed',
          'Step failed',
          'Waiting for input',
          'Performance threshold exceeded',
        ],
        default: ['Workflow failed', 'Workflow completed'],
      },
    ],
  },

  {
    title: 'Outputs & Results',
    description: 'Define what the workflow produces',
    questions: [
      {
        type: 'input',
        name: 'outputDescription',
        message: 'What does this workflow produce?',
        examples: ['Processed data files', 'Deployment status report', 'Quality metrics'],
        validate: (input) => input.length > 10 || 'Please describe the output',
      },
      {
        type: 'checkbox',
        name: 'outputTypes',
        message: 'What types of output does it generate?',
        choices: [
          'Status report',
          'Data files',
          'Metrics/statistics',
          'Logs',
          'Notifications',
          'Database updates',
          'API responses',
        ],
      },
      {
        type: 'confirm',
        name: 'saveOutputs',
        message: 'Should outputs be saved for later reference?',
        default: true,
      },
    ],
    required: ['outputDescription'],
  },

  {
    title: 'Security & Permissions',
    description: 'Configure workflow security settings',
    questions: [
      {
        type: 'confirm',
        name: 'requireAuth',
        message: 'Require authorization to run this workflow?',
        default: false,
      },
      {
        type: 'checkbox',
        name: 'allowedRoles',
        message: 'Which roles can execute this workflow?',
        when: (answers) => answers.requireAuth,
        choices: [
          'admin',
          'developer',
          'analyst',
          'reviewer',
          'operator',
          'viewer',
        ],
        default: ['admin', 'developer'],
      },
      {
        type: 'confirm',
        name: 'enableAuditLog',
        message: 'Enable audit logging for this workflow?',
        default: true,
      },
      {
        type: 'checkbox',
        name: 'securityFeatures',
        message: 'Additional security features:',
        choices: [
          'Encrypt sensitive data',
          'Mask credentials in logs',
          'Validate all inputs',
          'Sandbox execution',
          'Rate limiting',
        ],
        when: (answers) => answers.requireAuth,
      },
    ],
  },
];

// ---------------------------------------------------------------------------
// Exports
// ---------------------------------------------------------------------------

module.exports = {
  agentElicitationSteps,
  taskElicitationSteps,
  workflowElicitationSteps,
};

// Named aliases for convenience
module.exports.agent = agentElicitationSteps;
module.exports.task = taskElicitationSteps;
module.exports.workflow = workflowElicitationSteps;
