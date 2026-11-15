# Busy Beekeeper

Time-management + incremental beekeeping game. TypeScript, Vite and LittleJS, deployed as
static files to GitHub Pages (PWA). The design, decisions and phased plan are in `docs/ROADMAP.md`.

## Commands

- `npm run check`: typecheck, lint, format check, then tests. **Must pass before any change is done.**
- `npm run dev`: dev server. `npm run build`: production build to `dist/`.
- `npm run format` / `npm run lint:fix`: auto-fix formatting and fixable lint issues.

## Structure

```
src/
  sim/      Game rules and state: queue, clock, production, economy, saves.
  game/     LittleJS layer: rendering, input, scenes. Reads and drives sim/.
  config/   Typed tuning values (the central balance config).
  main.ts   Entry point: wires game/ to sim/.
```

**Boundary rule:** `sim/` and `config/` must never import from `game/` or DOM/browser APIs, and
may only use the **allowlisted pure LittleJS utilities** (`RandomGenerator`, `vec2`, `clamp`, …;
the list is `littleJsPureUtilities` in `eslint.config.js`). Engine-loop and stateful LittleJS
parts (`Timer`, `time`, `rand`, `EngineObject`, input, drawing) belong in `game/` only. The sim
owns the authoritative state, and `game/` objects only visualise it.

This is enforced by ESLint and by `tsconfig.sim.json`, which type-checks these folders without
DOM types. Don't work around these errors: move the code to `game/`, pass the value in, or (for a
genuinely pure LittleJS utility) add it to the allowlist.

## Rules

- **Deterministic sim.** No `Date.now()`, `performance.now()`, or `Math.random()` in `sim/`.
  Time arrives as an explicit `dt`. Randomness comes from a LittleJS `RandomGenerator` that is
  passed in, and its `seed` is saved so the sequence resumes after a load.
- **No magic numbers.** Balance values (durations, speeds, costs, limits) live in
  `src/config/defaultConfig.ts`, typed by `GameConfig`. Names carry their unit
  (`workDaySeconds`, `startingBeebucks`). Sim code receives a `GameConfig` as a parameter rather
  than importing `defaultConfig`, so tests can use tuned values.
- **Strict types.** No `any`, no non-null `!` assertions, and no lint-disable comments without a
  written reason on the same line.
- **Centralized theme.** Colours, fonts, font sizes and common spacing for `game/` live in
  `src/game/theme.ts`. Use its roles (`theme.colors.text`), not raw values. Lint blocks
  `rgb`/`hsl`/`new Color` outside `theme.ts`. Sizes and spacing can't be lint-checked, so never
  inline them: add a named entry to the theme.
- **Exhaustive unions.** Model variants (task types, scenes) as discriminated unions and handle
  them with exhaustive `switch`.
- **Saves are versioned.** Any change to `SaveData`'s shape bumps `CURRENT_SAVE_VERSION` and adds
  a migration in `src/sim/save/migrations.ts`, with a test that an old save still loads. Never edit
  a shipped migration.

## Conventions

- Standard TypeScript naming (camelCase locals, PascalCase types). Use `#private` class fields,
  not the `private` keyword.
- `import type` for type-only imports.
- One primary export per file, with the file named after it.

## Tests

- Vitest, placed next to the code: `foo.ts` gets `foo.test.ts`.
- **Purpose-driven only.** Test critical rules and risky logic (queue behaviour, end-of-day,
  production math, save migrations). Don't write tests just for coverage, and don't test the
  rendering layer.
- Name each test after the rule it protects, e.g. `drops queued tasks but finishes the active one at end of day`.

## Workflow

- Work in small, reviewable steps. Don't commit unless asked.
- **No self-run visual testing** (screenshots or headless browsers). It's token-expensive. Verify
  logic with tests and `npm run check`. When something visual needs checking, build or extend a
  test scene the user can open, and say exactly what feedback you need (e.g. "does the hive
  toggle colour when tapped on a phone?").
- When a design decision is made or changed, update `docs/ROADMAP.md`.

## Toolchain notes

- Dependencies are pinned to exact versions. Upgrade on purpose, not with `^` ranges.
- TypeScript is pinned to **6.0.3** because typescript-eslint doesn't support TS 7 yet.
- Line endings are LF (set in `.gitattributes`, and Prettier expects LF).
