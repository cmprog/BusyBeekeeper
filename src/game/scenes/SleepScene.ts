import { drawTextScreen, mouseWasPressed, timeDelta, vec2 } from 'littlejsengine';
import type { CalendarDay } from '../../sim/week/CalendarDay';
import type { GameState } from '../../sim/week/GameState';
import { dayName, weekdayName } from '../calendarText';
import { drawFilledRect } from '../draw';
import { theme } from '../theme';
import { viewport } from '../viewport';
import type { Scene } from './Scene';
import type { SceneContext } from './SceneContext';

/**
 * The night between two days: the beekeeper walks home, the lights go out, and the Zs float up.
 * Tomorrow is already saved, so a reload here simply resumes tomorrow. Tap to skip.
 */
export class SleepScene implements Scene {
  readonly #context: SceneContext;
  readonly #night: CalendarDay;
  readonly #next: GameState;
  #elapsedSeconds = 0;

  constructor(context: SceneContext, night: CalendarDay, next: GameState) {
    this.#context = context;
    this.#night = night;
    this.#next = next;
  }

  update(): void {
    this.#elapsedSeconds += timeDelta;
    if (mouseWasPressed(0) || this.#elapsedSeconds >= theme.sleep.totalSeconds) {
      this.#context.goTo({ kind: 'day', game: this.#next });
    }
  }

  render(): void {
    // All drawn in screen space.
  }

  renderPost(): void {
    const { sleep } = theme;
    const t = this.#elapsedSeconds;
    const width = viewport.canvasWidthPx;
    const height = viewport.canvasHeightPx;
    const centerX = width / 2;
    const fullScreen = { center: vec2(centerX, height / 2), size: vec2(width, height) };

    drawFilledRect(fullScreen, theme.colors.night, true);
    drawFilledRect(
      {
        center: vec2(centerX, height - sleep.groundHeightPx / 2),
        size: vec2(width, sleep.groundHeightPx),
      },
      theme.colors.nightGround,
      true,
    );
    for (const star of sleep.stars) {
      drawTextScreen('✨', vec2(star.xPx, star.yPx), sleep.starSizePx);
    }
    // The moon rises a little through the scene.
    const moonRise = sleep.moonRisePx * Math.min(1, t / sleep.totalSeconds);
    drawTextScreen('🌙', vec2(sleep.moonXPx, sleep.moonYPx - moonRise), sleep.moonSizePx);
    drawTextScreen('🏠', vec2(centerX, sleep.houseYPx), sleep.houseSizePx);

    // Walk home, then disappear through the door.
    if (t < sleep.walkSeconds) {
      const progress = t / sleep.walkSeconds;
      const x = sleep.beekeeperStartXPx + (centerX - sleep.beekeeperStartXPx) * progress;
      drawTextScreen('🧑‍🌾', vec2(x, sleep.beekeeperYPx), sleep.beekeeperSizePx);
    }

    // Lights out once inside.
    const dim = clamp01((t - sleep.walkSeconds) / sleep.lightsOutSeconds);
    if (dim > 0) {
      drawFilledRect(fullScreen, theme.colors.lightsOut.scale(1, dim), true);
    }

    // Zs drift up from the roof, staggered, once the lights are out.
    const zTime = t - sleep.walkSeconds - sleep.lightsOutSeconds;
    for (let i = 0; i < sleep.zCount && zTime > 0; i++) {
      const phase = (zTime / sleep.zCycleSeconds + i / sleep.zCount) % 1;
      drawTextScreen(
        '💤',
        vec2(centerX + sleep.zDriftPx * phase, sleep.zStartYPx - sleep.zRisePx * phase),
        sleep.zSizePx,
      );
    }

    drawTextScreen(
      `Goodnight! ${weekdayName(this.#night.weekday)} is done.`,
      vec2(centerX, sleep.captionTopPx),
      theme.font.sizePx.menuPrompt,
      theme.colors.text,
    );
    drawTextScreen(
      `Next: ${dayName(this.#next.calendar)}`,
      vec2(centerX, sleep.nextTopPx),
      theme.font.sizePx.menuButton,
      theme.colors.text,
    );
    drawTextScreen(
      'Tap to skip',
      vec2(centerX, sleep.skipTopPx),
      theme.font.sizePx.hint,
      theme.colors.textMuted,
    );
  }
}

function clamp01(value: number): number {
  return Math.min(1, Math.max(0, value));
}
