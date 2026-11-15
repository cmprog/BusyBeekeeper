import { defaultConfig } from './config/defaultConfig';
import { startGame } from './game/startGame';

startGame(defaultConfig).catch((error: unknown) => {
  console.error('Failed to start Busy Beekeeper', error);
});
