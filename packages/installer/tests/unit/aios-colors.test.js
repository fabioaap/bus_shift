/**
 * Unit Tests: AIOS Color Palette
 *
 * Tests for packages/installer/src/utils/aios-colors.js
 * Covers: color exports, status helpers, heading helpers, list helpers
 *
 * Issue #65 – Add missing test coverage for AIOS modules
 */

const { colors, status, headings, lists } = require('../../src/utils/aios-colors');

// Strip ANSI escape codes so we can assert on plain text
const stripAnsi = (str) => str.replace(/\x1B\[[0-9;]*m/g, '');

describe('aios-colors', () => {
  // =========================================================================
  // Module structure
  // =========================================================================
  describe('module exports', () => {
    it('exports colors object', () => {
      expect(colors).toBeDefined();
      expect(typeof colors).toBe('object');
    });

    it('exports status helpers', () => {
      expect(status).toBeDefined();
      expect(typeof status).toBe('object');
    });

    it('exports headings helpers', () => {
      expect(headings).toBeDefined();
      expect(typeof headings).toBe('object');
    });

    it('exports lists helpers', () => {
      expect(lists).toBeDefined();
      expect(typeof lists).toBe('object');
    });
  });

  // =========================================================================
  // Core brand colors — happy path
  // =========================================================================
  describe('colors — brand palette', () => {
    it('primary renders text unchanged (stripped ANSI)', () => {
      expect(typeof colors.primary).toBe('function');
      expect(stripAnsi(colors.primary('hello'))).toBe('hello');
    });

    it('secondary renders text unchanged', () => {
      expect(typeof colors.secondary).toBe('function');
      expect(stripAnsi(colors.secondary('world'))).toBe('world');
    });

    it('tertiary renders text unchanged', () => {
      expect(typeof colors.tertiary).toBe('function');
      expect(stripAnsi(colors.tertiary('test'))).toBe('test');
    });

    it('success renders text unchanged', () => {
      expect(stripAnsi(colors.success('ok'))).toBe('ok');
    });

    it('warning renders text unchanged', () => {
      expect(stripAnsi(colors.warning('caution'))).toBe('caution');
    });

    it('error renders text unchanged', () => {
      expect(stripAnsi(colors.error('fail'))).toBe('fail');
    });

    it('info renders text unchanged', () => {
      expect(stripAnsi(colors.info('info'))).toBe('info');
    });

    it('muted renders text unchanged', () => {
      expect(stripAnsi(colors.muted('quiet'))).toBe('quiet');
    });

    it('dim renders text unchanged', () => {
      expect(stripAnsi(colors.dim('dim text'))).toBe('dim text');
    });
  });

  // =========================================================================
  // Gradient sub-object
  // =========================================================================
  describe('colors.gradient', () => {
    it('exposes start, middle, end functions', () => {
      expect(typeof colors.gradient.start).toBe('function');
      expect(typeof colors.gradient.middle).toBe('function');
      expect(typeof colors.gradient.end).toBe('function');
    });

    it('gradient.start renders text', () => {
      expect(stripAnsi(colors.gradient.start('begin'))).toBe('begin');
    });

    it('gradient.end renders text', () => {
      expect(stripAnsi(colors.gradient.end('end'))).toBe('end');
    });
  });

  // =========================================================================
  // Semantic shortcuts
  // =========================================================================
  describe('colors — semantic shortcuts', () => {
    it('highlight renders text', () => {
      expect(typeof colors.highlight).toBe('function');
      expect(stripAnsi(colors.highlight('important'))).toBe('important');
    });

    it('brandPrimary renders text', () => {
      expect(typeof colors.brandPrimary).toBe('function');
      expect(stripAnsi(colors.brandPrimary('AIOS'))).toBe('AIOS');
    });

    it('brandSecondary renders text', () => {
      expect(typeof colors.brandSecondary).toBe('function');
      expect(stripAnsi(colors.brandSecondary('brand'))).toBe('brand');
    });
  });

  // =========================================================================
  // Status helpers — happy path
  // =========================================================================
  describe('status helpers', () => {
    it('success includes checkmark symbol and message', () => {
      const result = status.success('Done');
      expect(stripAnsi(result)).toContain('✓');
      expect(stripAnsi(result)).toContain('Done');
    });

    it('error includes × symbol and message', () => {
      const result = status.error('Failed');
      expect(stripAnsi(result)).toContain('✗');
      expect(stripAnsi(result)).toContain('Failed');
    });

    it('warning includes message', () => {
      const result = status.warning('Watch out');
      expect(stripAnsi(result)).toContain('Watch out');
    });

    it('info includes ℹ symbol and message', () => {
      const result = status.info('FYI');
      expect(stripAnsi(result)).toContain('ℹ');
      expect(stripAnsi(result)).toContain('FYI');
    });

    it('loading includes message', () => {
      expect(stripAnsi(status.loading('Please wait'))).toContain('Please wait');
    });

    it('skipped includes message', () => {
      expect(stripAnsi(status.skipped('Skipped'))).toContain('Skipped');
    });

    it('tip includes message', () => {
      expect(stripAnsi(status.tip('Pro tip'))).toContain('Pro tip');
    });

    it('celebrate includes message', () => {
      expect(stripAnsi(status.celebrate('🎉 Success!'))).toContain('Success!');
    });
  });

  // =========================================================================
  // Heading helpers
  // =========================================================================
  describe('headings helpers', () => {
    it('h1 renders text with surrounding newlines', () => {
      const result = headings.h1('Main Title');
      expect(stripAnsi(result)).toContain('Main Title');
      expect(result).toMatch(/\n/);
    });

    it('h2 renders text with newlines', () => {
      const result = headings.h2('Section');
      expect(stripAnsi(result)).toContain('Section');
      expect(result).toMatch(/\n/);
    });

    it('h3 renders text', () => {
      expect(stripAnsi(headings.h3('Subsection'))).toContain('Subsection');
    });

    it('divider returns a string of ─ characters', () => {
      const result = headings.divider();
      expect(typeof result).toBe('string');
      expect(stripAnsi(result)).toMatch(/─+/);
    });
  });

  // =========================================================================
  // List helpers
  // =========================================================================
  describe('lists helpers', () => {
    it('bullet includes • and text', () => {
      const result = lists.bullet('Item one');
      expect(stripAnsi(result)).toContain('•');
      expect(stripAnsi(result)).toContain('Item one');
    });

    it('numbered includes digit and text', () => {
      const result = lists.numbered(1, 'First item');
      expect(stripAnsi(result)).toContain('1.');
      expect(stripAnsi(result)).toContain('First item');
    });

    it('checkbox(false) shows ☐ unchecked box', () => {
      const result = lists.checkbox('Task', false);
      expect(stripAnsi(result)).toContain('☐');
      expect(stripAnsi(result)).toContain('Task');
    });

    it('checkbox(true) shows ☑ checked box', () => {
      const result = lists.checkbox('Task', true);
      expect(stripAnsi(result)).toContain('☑');
      expect(stripAnsi(result)).toContain('Task');
    });

    it('checkbox defaults to unchecked when no second arg', () => {
      expect(stripAnsi(lists.checkbox('Default'))).toContain('☐');
    });
  });

  // =========================================================================
  // Edge cases
  // =========================================================================
  describe('edge cases', () => {
    it('colors.primary handles empty string', () => {
      expect(typeof colors.primary('')).toBe('string');
    });

    it('status.success handles empty string', () => {
      const result = status.success('');
      expect(stripAnsi(result)).toContain('✓');
    });

    it('lists.numbered handles 0', () => {
      expect(stripAnsi(lists.numbered(0, 'Zero'))).toContain('0.');
    });

    it('lists.numbered handles large numbers', () => {
      expect(stripAnsi(lists.numbered(999, 'Big'))).toContain('999.');
    });

    it('headings.divider always returns consistent length', () => {
      const d1 = stripAnsi(headings.divider());
      const d2 = stripAnsi(headings.divider());
      expect(d1.length).toBe(d2.length);
    });
  });
});
