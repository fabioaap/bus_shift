/**
 * Unit Tests: IDE Configuration Metadata
 *
 * Tests for packages/installer/src/config/ide-configs.js
 * Covers: IDE_CONFIGS structure, getIDEKeys, getIDEConfig, isValidIDE
 *
 * Note: getIDEChoices() requires an i18n dependency and is not tested here
 * (integration test responsibility).
 *
 * Issue #65 – Add missing test coverage for AIOS modules
 */

const { IDE_CONFIGS, getIDEKeys, getIDEConfig, isValidIDE } = require('../../src/config/ide-configs');

describe('ide-configs', () => {
  // =========================================================================
  // IDE_CONFIGS structure
  // =========================================================================
  describe('IDE_CONFIGS object', () => {
    it('exports a non-null object', () => {
      expect(IDE_CONFIGS).toBeDefined();
      expect(typeof IDE_CONFIGS).toBe('object');
      expect(IDE_CONFIGS).not.toBeNull();
    });

    it('contains at least 5 IDE entries', () => {
      expect(Object.keys(IDE_CONFIGS).length).toBeGreaterThanOrEqual(5);
    });

    it('includes claude-code entry', () => {
      expect(IDE_CONFIGS['claude-code']).toBeDefined();
    });

    it('includes cursor entry', () => {
      expect(IDE_CONFIGS['cursor']).toBeDefined();
    });

    it('includes windsurf entry', () => {
      expect(IDE_CONFIGS['windsurf']).toBeDefined();
    });

    it('includes github-copilot entry', () => {
      expect(IDE_CONFIGS['github-copilot']).toBeDefined();
    });

    it('includes antigravity entry', () => {
      expect(IDE_CONFIGS['antigravity']).toBeDefined();
    });
  });

  // =========================================================================
  // Each IDE config has required fields
  // =========================================================================
  describe('each IDE config has required fields', () => {
    const expectedKeys = ['name', 'configFile', 'template', 'requiresDirectory', 'format'];

    Object.keys(IDE_CONFIGS).forEach((ideKey) => {
      describe(`IDE: ${ideKey}`, () => {
        expectedKeys.forEach((field) => {
          it(`has '${field}' field`, () => {
            expect(IDE_CONFIGS[ideKey]).toHaveProperty(field);
          });
        });

        it('name is a non-empty string', () => {
          expect(typeof IDE_CONFIGS[ideKey].name).toBe('string');
          expect(IDE_CONFIGS[ideKey].name.length).toBeGreaterThan(0);
        });

        it('configFile is a non-empty string', () => {
          expect(typeof IDE_CONFIGS[ideKey].configFile).toBe('string');
          expect(IDE_CONFIGS[ideKey].configFile.length).toBeGreaterThan(0);
        });

        it('template is a non-empty string', () => {
          expect(typeof IDE_CONFIGS[ideKey].template).toBe('string');
          expect(IDE_CONFIGS[ideKey].template.length).toBeGreaterThan(0);
        });

        it('requiresDirectory is a boolean', () => {
          expect(typeof IDE_CONFIGS[ideKey].requiresDirectory).toBe('boolean');
        });

        it("format is 'text', 'json', or 'yaml'", () => {
          expect(['text', 'json', 'yaml']).toContain(IDE_CONFIGS[ideKey].format);
        });
      });
    });
  });

  // =========================================================================
  // claude-code specific checks
  // =========================================================================
  describe('claude-code config specifics', () => {
    const cc = IDE_CONFIGS['claude-code'];

    it('is marked as recommended', () => {
      expect(cc.recommended).toBe(true);
    });

    it('requiresDirectory is true', () => {
      expect(cc.requiresDirectory).toBe(true);
    });

    it('configFile includes .claude path segment', () => {
      expect(cc.configFile).toContain('.claude');
    });

    it('agentFolder is defined', () => {
      expect(cc.agentFolder).toBeDefined();
    });
  });

  // =========================================================================
  // windsurf specific checks
  // =========================================================================
  describe('windsurf config specifics', () => {
    const ws = IDE_CONFIGS['windsurf'];

    it('requiresDirectory is false', () => {
      expect(ws.requiresDirectory).toBe(false);
    });

    it('configFile is .windsurfrules', () => {
      expect(ws.configFile).toBe('.windsurfrules');
    });
  });

  // =========================================================================
  // antigravity special config
  // =========================================================================
  describe('antigravity specialConfig', () => {
    const ag = IDE_CONFIGS['antigravity'];

    it('has specialConfig object', () => {
      expect(ag.specialConfig).toBeDefined();
      expect(typeof ag.specialConfig).toBe('object');
    });

    it('specialConfig.type is "antigravity"', () => {
      expect(ag.specialConfig.type).toBe('antigravity');
    });

    it('specialConfig.configJsonPath is defined', () => {
      expect(ag.specialConfig.configJsonPath).toBeDefined();
    });

    it('specialConfig.workflowsFolder is defined', () => {
      expect(ag.specialConfig.workflowsFolder).toBeDefined();
    });
  });

  // =========================================================================
  // getIDEKeys
  // =========================================================================
  describe('getIDEKeys()', () => {
    it('returns an array', () => {
      expect(Array.isArray(getIDEKeys())).toBe(true);
    });

    it('returns the same keys as Object.keys(IDE_CONFIGS)', () => {
      expect(getIDEKeys()).toEqual(Object.keys(IDE_CONFIGS));
    });

    it('includes all 5 known IDEs', () => {
      const keys = getIDEKeys();
      expect(keys).toContain('claude-code');
      expect(keys).toContain('cursor');
      expect(keys).toContain('windsurf');
      expect(keys).toContain('github-copilot');
      expect(keys).toContain('antigravity');
    });

    it('returns at least 5 entries', () => {
      expect(getIDEKeys().length).toBeGreaterThanOrEqual(5);
    });
  });

  // =========================================================================
  // getIDEConfig
  // =========================================================================
  describe('getIDEConfig()', () => {
    it('returns config object for a valid key', () => {
      const config = getIDEConfig('claude-code');
      expect(config).toBeDefined();
      expect(config.name).toBe('Claude Code');
    });

    it('returns config for cursor', () => {
      const config = getIDEConfig('cursor');
      expect(config).toBeDefined();
      expect(config.name).toBe('Cursor');
    });

    it('returns null for an unknown IDE key', () => {
      expect(getIDEConfig('unknown-ide')).toBeNull();
    });

    it('returns null for empty string', () => {
      expect(getIDEConfig('')).toBeNull();
    });

    it('returns null for null input', () => {
      expect(getIDEConfig(null)).toBeNull();
    });

    it('returns null for undefined input', () => {
      expect(getIDEConfig(undefined)).toBeNull();
    });

    it('returns the same object reference as IDE_CONFIGS', () => {
      expect(getIDEConfig('windsurf')).toBe(IDE_CONFIGS['windsurf']);
    });
  });

  // =========================================================================
  // isValidIDE
  // =========================================================================
  describe('isValidIDE()', () => {
    it('returns true for claude-code', () => {
      expect(isValidIDE('claude-code')).toBe(true);
    });

    it('returns true for cursor', () => {
      expect(isValidIDE('cursor')).toBe(true);
    });

    it('returns true for windsurf', () => {
      expect(isValidIDE('windsurf')).toBe(true);
    });

    it('returns true for github-copilot', () => {
      expect(isValidIDE('github-copilot')).toBe(true);
    });

    it('returns true for antigravity', () => {
      expect(isValidIDE('antigravity')).toBe(true);
    });

    it('returns false for unknown IDE', () => {
      expect(isValidIDE('vscode')).toBe(false);
    });

    it('returns false for empty string', () => {
      expect(isValidIDE('')).toBe(false);
    });

    it('returns false for null', () => {
      expect(isValidIDE(null)).toBe(false);
    });

    it('is case-sensitive (uppercase fails)', () => {
      expect(isValidIDE('CURSOR')).toBe(false);
      expect(isValidIDE('Claude-Code')).toBe(false);
    });
  });

  // =========================================================================
  // Cross-platform path handling
  // =========================================================================
  describe('cross-platform path handling', () => {
    it('configFile paths do not start with a separator', () => {
      // Relative paths should not start with / or \\
      getIDEKeys().forEach((key) => {
        const { configFile } = IDE_CONFIGS[key];
        expect(configFile).not.toMatch(/^[/\\]/);
      });
    });

    it('template paths use forward-slash prefix style (ide-rules/)', () => {
      getIDEKeys().forEach((key) => {
        const { template } = IDE_CONFIGS[key];
        expect(template).toContain('ide-rules/');
      });
    });
  });
});
