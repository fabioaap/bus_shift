/**
 * Unit Tests: File Hasher Utility
 *
 * Tests for packages/installer/src/installer/file-hasher.js
 * Covers: hashString, hashesMatch, isBinaryFile, normalizeLineEndings,
 *         removeBOM, hashFile, hashFileAsync, hashFilesParallel
 *
 * Issue #65 – Add missing test coverage for AIOS modules
 */

const path = require('path');
const os = require('os');
const fs = require('fs');

// We need fs-extra for the module under test. Jest will auto-mock when needed,
// but here we allow real FS operations via a temp directory.
const {
  hashString,
  hashesMatch,
  isBinaryFile,
  normalizeLineEndings,
  removeBOM,
  BINARY_EXTENSIONS,
  hashFile,
  hashFileAsync,
  hashFilesParallel,
  getFileMetadata,
} = require('../../../src/installer/file-hasher');

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

let tmpDir;

beforeAll(() => {
  tmpDir = fs.mkdtempSync(path.join(os.tmpdir(), 'aios-file-hasher-test-'));
});

afterAll(() => {
  // Clean up temp files
  try {
    fs.rmSync(tmpDir, { recursive: true, force: true });
  } catch {
    // ignore cleanup errors
  }
});

function writeTmp(filename, content, encoding = 'utf8') {
  const filePath = path.join(tmpDir, filename);
  fs.writeFileSync(filePath, content, encoding);
  return filePath;
}

describe('file-hasher', () => {
  // =========================================================================
  // hashString — pure function, no FS
  // =========================================================================
  describe('hashString', () => {
    it('returns a 64-character hex string (SHA-256)', () => {
      const hash = hashString('hello world');
      expect(typeof hash).toBe('string');
      expect(hash).toHaveLength(64);
      expect(hash).toMatch(/^[a-f0-9]+$/);
    });

    it('returns the same hash for the same input', () => {
      expect(hashString('deterministic')).toBe(hashString('deterministic'));
    });

    it('returns different hashes for different inputs', () => {
      expect(hashString('foo')).not.toBe(hashString('bar'));
    });

    it('handles empty string', () => {
      const hash = hashString('');
      expect(hash).toHaveLength(64);
    });

    it('handles unicode content', () => {
      const hash = hashString('héllo wörld 🚀');
      expect(hash).toHaveLength(64);
    });
  });

  // =========================================================================
  // hashesMatch — pure function
  // =========================================================================
  describe('hashesMatch', () => {
    it('returns true for identical hashes', () => {
      const h = hashString('same');
      expect(hashesMatch(h, h)).toBe(true);
    });

    it('returns true for case-insensitive match', () => {
      const h = 'abcdef1234567890';
      expect(hashesMatch(h.toUpperCase(), h.toLowerCase())).toBe(true);
    });

    it('returns false for different hashes', () => {
      expect(hashesMatch(hashString('foo'), hashString('bar'))).toBe(false);
    });

    it('returns false when first hash is null', () => {
      expect(hashesMatch(null, hashString('test'))).toBe(false);
    });

    it('returns false when second hash is null', () => {
      expect(hashesMatch(hashString('test'), null)).toBe(false);
    });

    it('returns false when both are null', () => {
      expect(hashesMatch(null, null)).toBe(false);
    });

    it('returns false when either is empty string', () => {
      expect(hashesMatch('', hashString('test'))).toBe(false);
    });
  });

  // =========================================================================
  // isBinaryFile — pure function
  // =========================================================================
  describe('isBinaryFile', () => {
    it.each([
      ['/path/to/image.png', true],
      ['/path/to/photo.jpg', true],
      ['/path/to/photo.jpeg', true],
      ['/path/to/icon.ico', true],
      ['/path/to/font.woff', true],
      ['/path/to/font.woff2', true],
      ['/path/to/archive.zip', true],
      ['/path/to/document.pdf', true],
      ['/path/to/executable.exe', true],
    ])('correctly identifies %s as binary', (filePath, expected) => {
      expect(isBinaryFile(filePath)).toBe(expected);
    });

    it.each([
      ['/path/to/script.js', false],
      ['/path/to/styles.css', false],
      ['/path/to/data.json', false],
      ['/path/to/config.yaml', false],
      ['/path/to/README.md', false],
      ['/path/to/file.txt', false],
      ['/path/to/code.ts', false],
    ])('correctly identifies %s as non-binary', (filePath, expected) => {
      expect(isBinaryFile(filePath)).toBe(expected);
    });

    it('is case-insensitive for extensions', () => {
      expect(isBinaryFile('/path/file.PNG')).toBe(true);
      expect(isBinaryFile('/path/file.JPG')).toBe(true);
    });

    it('BINARY_EXTENSIONS is an array of strings', () => {
      expect(Array.isArray(BINARY_EXTENSIONS)).toBe(true);
      BINARY_EXTENSIONS.forEach(ext => {
        expect(typeof ext).toBe('string');
        expect(ext).toMatch(/^\./);
      });
    });
  });

  // =========================================================================
  // normalizeLineEndings — pure function
  // =========================================================================
  describe('normalizeLineEndings', () => {
    it('converts CRLF to LF', () => {
      expect(normalizeLineEndings('line1\r\nline2')).toBe('line1\nline2');
    });

    it('converts lone CR to LF', () => {
      expect(normalizeLineEndings('line1\rline2')).toBe('line1\nline2');
    });

    it('leaves LF-only content unchanged', () => {
      expect(normalizeLineEndings('line1\nline2')).toBe('line1\nline2');
    });

    it('handles mixed line endings', () => {
      const mixed = 'a\r\nb\rc\nd';
      expect(normalizeLineEndings(mixed)).toBe('a\nb\nc\nd');
    });

    it('handles empty string', () => {
      expect(normalizeLineEndings('')).toBe('');
    });

    it('handles content without any line endings', () => {
      expect(normalizeLineEndings('no newlines here')).toBe('no newlines here');
    });
  });

  // =========================================================================
  // removeBOM — pure function
  // =========================================================================
  describe('removeBOM', () => {
    it('removes UTF-8 BOM (U+FEFF) from start', () => {
      const withBOM = '\uFEFFhello world';
      expect(removeBOM(withBOM)).toBe('hello world');
    });

    it('leaves content without BOM unchanged', () => {
      expect(removeBOM('hello world')).toBe('hello world');
    });

    it('only removes BOM at start, not mid-string', () => {
      const str = 'hello\uFEFFworld';
      expect(removeBOM(str)).toBe('hello\uFEFFworld');
    });

    it('handles empty string', () => {
      expect(removeBOM('')).toBe('');
    });
  });

  // =========================================================================
  // hashFile — requires real filesystem
  // =========================================================================
  describe('hashFile (sync)', () => {
    it('returns a 64-char hex hash for a text file', () => {
      const f = writeTmp('text-file.txt', 'hello world\n');
      const hash = hashFile(f);
      expect(hash).toHaveLength(64);
      expect(hash).toMatch(/^[a-f0-9]+$/);
    });

    it('returns the same hash when called twice on unchanged file', () => {
      const f = writeTmp('stable.txt', 'stable content');
      expect(hashFile(f)).toBe(hashFile(f));
    });

    it('returns different hash after file content changes', () => {
      const f = writeTmp('changing.txt', 'version 1');
      const h1 = hashFile(f);
      fs.writeFileSync(f, 'version 2', 'utf8');
      const h2 = hashFile(f);
      expect(h1).not.toBe(h2);
    });

    it('normalizes CRLF before hashing (cross-platform consistency)', () => {
      const lfFile = writeTmp('lf.txt', 'line1\nline2\n');
      const crlfFile = writeTmp('crlf.txt', 'line1\r\nline2\r\n');
      expect(hashFile(lfFile)).toBe(hashFile(crlfFile));
    });

    it('throws an error when file does not exist', () => {
      expect(() => hashFile('/nonexistent/path/file.txt')).toThrow('File not found');
    });

    it('throws an error when path is a directory', () => {
      expect(() => hashFile(tmpDir)).toThrow('Cannot hash directory');
    });
  });

  // =========================================================================
  // hashFileAsync — async version
  // =========================================================================
  describe('hashFileAsync', () => {
    it('returns a promise resolving to a 64-char hash', async () => {
      const f = writeTmp('async-text.txt', 'async content\n');
      const hash = await hashFileAsync(f);
      expect(hash).toHaveLength(64);
      expect(hash).toMatch(/^[a-f0-9]+$/);
    });

    it('matches sync hash for the same file', async () => {
      const f = writeTmp('sync-vs-async.txt', 'compare me');
      const syncHash = hashFile(f);
      const asyncHash = await hashFileAsync(f);
      expect(syncHash).toBe(asyncHash);
    });

    it('rejects when file does not exist', async () => {
      await expect(hashFileAsync('/nonexistent/async.txt')).rejects.toThrow('File not found');
    });

    it('rejects when path is a directory', async () => {
      await expect(hashFileAsync(tmpDir)).rejects.toThrow('Cannot hash directory');
    });
  });

  // =========================================================================
  // hashFilesParallel
  // =========================================================================
  describe('hashFilesParallel', () => {
    it('returns a Map of file → hash for multiple files', async () => {
      const f1 = writeTmp('parallel-1.txt', 'file one');
      const f2 = writeTmp('parallel-2.txt', 'file two');
      const f3 = writeTmp('parallel-3.txt', 'file three');
      const result = await hashFilesParallel([f1, f2, f3]);
      expect(result instanceof Map).toBe(true);
      expect(result.size).toBe(3);
      expect(result.get(f1)).toHaveLength(64);
      expect(result.get(f2)).toHaveLength(64);
      expect(result.get(f3)).toHaveLength(64);
    });

    it('returns empty Map for empty input', async () => {
      const result = await hashFilesParallel([]);
      expect(result instanceof Map).toBe(true);
      expect(result.size).toBe(0);
    });

    it('skips files that do not exist (no throw)', async () => {
      const f = writeTmp('exists.txt', 'i exist');
      const result = await hashFilesParallel([f, '/nonexistent/ghost.txt']);
      // Only the existing file should be in results
      expect(result.has(f)).toBe(true);
      expect(result.has('/nonexistent/ghost.txt')).toBe(false);
    });

    it('calls onProgress callback', async () => {
      const files = [
        writeTmp('progress-1.txt', 'one'),
        writeTmp('progress-2.txt', 'two'),
      ];
      const progressCalls = [];
      await hashFilesParallel(files, 50, (current, total) => {
        progressCalls.push({ current, total });
      });
      expect(progressCalls.length).toBeGreaterThan(0);
      expect(progressCalls[0].total).toBe(2);
    });
  });

  // =========================================================================
  // getFileMetadata
  // =========================================================================
  describe('getFileMetadata', () => {
    it('returns metadata object with path, hash, size, isBinary', () => {
      const f = writeTmp('metadata.txt', 'metadata content');
      const meta = getFileMetadata(f, tmpDir);
      expect(meta).toHaveProperty('path');
      expect(meta).toHaveProperty('hash');
      expect(meta).toHaveProperty('size');
      expect(meta).toHaveProperty('isBinary');
    });

    it('hash starts with sha256:', () => {
      const f = writeTmp('sha256-prefix.txt', 'prefix test');
      const meta = getFileMetadata(f, tmpDir);
      expect(meta.hash).toMatch(/^sha256:[a-f0-9]{64}$/);
    });

    it('isBinary is false for .txt files', () => {
      const f = writeTmp('not-binary.txt', 'text');
      const meta = getFileMetadata(f, tmpDir);
      expect(meta.isBinary).toBe(false);
    });

    it('path uses forward slashes (cross-platform)', () => {
      const f = writeTmp('forward-slashes.txt', 'slashes');
      const meta = getFileMetadata(f, tmpDir);
      expect(meta.path).not.toContain('\\');
    });
  });
});
