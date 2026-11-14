// @ts-check
import js from '@eslint/js';
import prettier from 'eslint-config-prettier';
import globals from 'globals';
import tseslint from 'typescript-eslint';

export default tseslint.config(
  {
    ignores: ['dist/', 'coverage/', 'node_modules/'],
  },

  js.configs.recommended,

  // Strictest typescript-eslint presets, using full type information.
  ...tseslint.configs.strictTypeChecked,
  ...tseslint.configs.stylisticTypeChecked,

  {
    languageOptions: {
      globals: globals.browser,
      parserOptions: {
        projectService: true,
        tsconfigRootDir: import.meta.dirname,
      },
    },
    rules: {
      // Exhaustive `switch` over unions (task types, scenes, …): adding a new member
      // without handling it becomes a lint error instead of a silent runtime gap.
      '@typescript-eslint/switch-exhaustiveness-check': [
        'error',
        { considerDefaultExhaustiveForUnions: false, requireDefaultForNonUnion: true },
      ],

      // Exported functions declare their return types, so module contracts are explicit.
      '@typescript-eslint/explicit-module-boundary-types': 'error',

      // Private fields that are never reassigned must be `readonly`.
      '@typescript-eslint/prefer-readonly': 'error',

      // `import type` for type-only imports (pairs with tsconfig `verbatimModuleSyntax`).
      '@typescript-eslint/consistent-type-imports': 'error',

      // Numbers in template strings are fine (`${count} jars`).
      '@typescript-eslint/restrict-template-expressions': ['error', { allowNumber: true }],

      eqeqeq: ['error', 'always'],
      'no-console': ['error', { allow: ['warn', 'error'] }],
    },
  },

  // Plain JS config files aren't part of the TS project — lint them without type info.
  {
    files: ['**/*.js'],
    ...tseslint.configs.disableTypeChecked,
    languageOptions: {
      ...tseslint.configs.disableTypeChecked.languageOptions,
      globals: globals.node,
    },
  },

  // Must be last: turns off stylistic rules that conflict with Prettier.
  prettier,
);
