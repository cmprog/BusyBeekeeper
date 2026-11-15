import { startGame } from './game/startGame';

startGame().catch((error: unknown) => {
  console.error('Failed to start Busy Beekeeper', error);
});
