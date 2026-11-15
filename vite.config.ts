import { defineConfig } from 'vite';
import { VitePWA } from 'vite-plugin-pwa';

// Manifest colours mirror src/game/theme.ts (palette.bark).
const backgroundColor = '#1f1a14';

export default defineConfig({
  // Relative asset paths so the build works from a GitHub Pages sub-path
  // (https://<user>.github.io/<repo>/) without hard-coding the repo name.
  base: './',
  build: {
    target: 'es2022',
  },
  plugins: [
    VitePWA({
      // New builds take over on next launch; saves live in localStorage so they're unaffected.
      registerType: 'autoUpdate',
      // globPatterns below already precaches the manifest icons; don't add them twice.
      includeManifestIcons: false,
      manifest: {
        name: 'Busy Beekeeper',
        short_name: 'Beekeeper',
        description: 'A time-management beekeeping game.',
        start_url: './',
        scope: './',
        display: 'standalone',
        display_override: ['fullscreen', 'standalone'],
        orientation: 'landscape',
        background_color: backgroundColor,
        theme_color: backgroundColor,
        icons: [
          { src: 'pwa-64x64.png', sizes: '64x64', type: 'image/png' },
          { src: 'pwa-192x192.png', sizes: '192x192', type: 'image/png' },
          { src: 'pwa-512x512.png', sizes: '512x512', type: 'image/png' },
          {
            src: 'maskable-icon-512x512.png',
            sizes: '512x512',
            type: 'image/png',
            purpose: 'maskable',
          },
        ],
      },
      workbox: {
        // Precache everything the game needs so it runs fully offline once installed.
        globPatterns: ['**/*.{js,css,html,png,svg,ico}'],
      },
    }),
  ],
});
