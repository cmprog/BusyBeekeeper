import { defineConfig, minimal2023Preset } from '@vite-pwa/assets-generator/config';

// Generates the PWA icon PNGs (favicon, 64/192/512, maskable, apple-touch) into public/
// from public/icon.svg. Run `npm run icons` after changing the SVG and commit the output.
const background = '#1f1a14'; // theme.ts palette.bark

export default defineConfig({
  preset: {
    ...minimal2023Preset,
    maskable: { ...minimal2023Preset.maskable, resizeOptions: { background } },
    apple: { ...minimal2023Preset.apple, resizeOptions: { background } },
  },
  images: ['public/icon.svg'],
});
