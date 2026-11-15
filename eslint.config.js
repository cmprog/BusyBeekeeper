// @ts-check
import js from '@eslint/js';
import prettier from 'eslint-config-prettier';
import globals from 'globals';
import tseslint from 'typescript-eslint';

// Require JS `#private` fields instead of TypeScript's `private` keyword (which is only
// enforced at compile time). Shared because `no-restricted-syntax` options don't merge
// between config blocks: a later block replaces the whole list.
const noPrivateKeyword = [
  {
    selector:
      ":matches(PropertyDefinition, MethodDefinition, TSParameterProperty)[accessibility='private']",
    message: 'Use a #private member instead of the `private` keyword.',
  },
];

// Pure layers: game rules and config. Must stay deterministic and runnable in plain Node
// (see CLAUDE.md). Browser APIs are blocked separately by tsconfig.sim.json (no DOM lib).
const pureLayers = ['src/sim/**/*.ts', 'src/config/**/*.ts'];

// The ONLY LittleJS exports the pure layers may use: deterministic data/math utilities.
// Engine-loop or stateful parts (Timer, time/timeDelta, rand/randInt, EngineObject, input,
// drawing, audio) stay in game/. Add to this list deliberately when a new utility is needed.
const littleJsPureUtilities = [
  'RandomGenerator',
  'Vector2',
  'vec2',
  'clamp',
  'lerp',
  'percent',
  'smoothStep',
];

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
      'no-restricted-syntax': ['error', ...noPrivateKeyword],
    },
  },

  // sim/ + config/: LittleJS pure utilities only, no game/ layer, no hidden time or randomness.
  {
    files: pureLayers,
    rules: {
      'no-restricted-imports': [
        'error',
        {
          paths: [
            {
              name: 'littlejsengine',
              allowImportNames: littleJsPureUtilities,
              message:
                'sim/ and config/ may only use allowlisted LittleJS utilities (see eslint.config.js).',
            },
          ],
          patterns: [
            {
              regex: '^littlejsengine/',
              message: 'Import LittleJS from "littlejsengine" only (allowlisted names).',
            },
            {
              regex: '(^|/)game(/|$)',
              message: 'sim/ and config/ must not import from game/. Dependencies point inward.',
            },
          ],
        },
      ],
      'no-restricted-properties': [
        'error',
        { object: 'Math', property: 'random', message: 'Use the seeded Rng passed into the sim.' },
        { object: 'Date', property: 'now', message: 'Sim time comes in as an explicit dt.' },
        { object: 'performance', property: 'now', message: 'Sim time comes in as an explicit dt.' },
      ],
      'no-restricted-syntax': [
        'error',
        ...noPrivateKeyword,
        {
          selector: "NewExpression[callee.name='Date']",
          message: 'Sim time comes in as an explicit dt, not wall-clock dates.',
        },
      ],
    },
  },

  // game/: colours and engine-wide theme settings come only from theme.ts.
  {
    files: ['src/game/**/*.ts'],
    ignores: ['src/game/theme.ts'],
    rules: {
      'no-restricted-imports': [
        'error',
        {
          paths: [
            {
              name: 'littlejsengine',
              importNames: ['rgb', 'hsl', 'Color', 'setFontDefault', 'setCanvasClearColor'],
              allowTypeImports: true,
              message: 'Define colours and theme settings in src/game/theme.ts and use its roles.',
            },
          ],
        },
      ],
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
