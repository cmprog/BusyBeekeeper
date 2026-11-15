import { defineConfig } from 'vitest/config';

export default defineConfig({
  test: {
    // Tests live next to the code they cover: src/**/foo.test.ts
    include: ['src/**/*.test.ts'],
    // Simulation tests run in plain Node — no DOM, no engine (see ROADMAP §3.11).
    environment: 'node',
    // Fail on accidental `.only` so a focused test can't silently skip the rest.
    allowOnly: false,
  },
});
