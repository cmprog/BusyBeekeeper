import { defineConfig } from 'vite';

export default defineConfig({
  // Relative asset paths so the build works from a GitHub Pages sub-path
  // (https://<user>.github.io/<repo>/) without hard-coding the repo name.
  base: './',
  build: {
    target: 'es2022',
  },
});
