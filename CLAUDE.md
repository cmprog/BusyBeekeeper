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

**Boundary rule:** `sim/` must never import from `game/`, LittleJS, or DOM/browser APIs.
This keeps the rules unit-testable in plain Node.

## Rules

- **Deterministic sim.** No `Date.now()`, `performance.now()`, or `Math.random()` in `sim/`.
  Time arrives as an explicit `dt`, and randomness comes from a seeded RNG that is passed in.
- **No magic numbers.** Balance values (durations, speeds, costs, limits) live in `src/config/`.
- **Strict types.** No `any`, no non-null `!` assertions, and no lint-disable comments without a
  written reason on the same line.
- **Exhaustive unions.** Model variants (task types, scenes) as discriminated unions and handle
  them with exhaustive `switch`.
- **Saves are versioned.** Any change to the saved state shape bumps the save version and adds a
  migration with a test.

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
- When a design decision is made or changed, update `docs/ROADMAP.md`.

## Toolchain notes

- Dependencies are pinned to exact versions. Upgrade on purpose, not with `^` ranges.
- TypeScript is pinned to **6.0.3** because typescript-eslint doesn't support TS 7 yet.
- Line endings are LF (set in `.gitattributes`, and Prettier expects LF).
