import {
  drawCircle,
  drawRect,
  drawText,
  drawTextScreen,
  mousePos,
  mousePosScreen,
  mouseWasPressed,
  timeDelta,
  Timer,
  vec2,
} from 'littlejsengine';
import type { Color } from 'littlejsengine';
import type { GameConfig } from '../config/GameConfig';
import { stationIds } from '../sim/StationId';
import type { StationId } from '../sim/StationId';
import type { ProductionState } from '../sim/production/ProductionState';
import type { Task } from '../sim/workday/Task';
import { WorkDay } from '../sim/workday/WorkDay';
import type { EnqueueRefusal } from '../sim/workday/WorkDay';
import type { ActiveTask, DayStatus } from '../sim/workday/WorkDayState';
import { rectContains } from './Rect';
import type { Rect } from './Rect';
import { stationVisuals } from './stationVisuals';
import { taskFailureText } from './taskFailureText';
import { theme } from './theme';
import { viewport } from './viewport';

/**
 * Phase 1 test scene: the backyard with the honey production stations. Tap a station to queue
 * its task, tap a queue slot to cancel it, and end the day early or start a new one (stock
 * carries over) with the button.
 *
 * Owns no game rules: it forwards input to the sim's `WorkDay` and draws its state.
 * The HUD is drawn with `useWebGL = false` so it lands on the 2D layer above the scene.
 */
export class BackyardScene {
  readonly #config: GameConfig;
  #day: WorkDay;
  readonly #flashTimer = new Timer();
  #flashedStation: StationId | undefined;
  readonly #warningTimer = new Timer();
  #warningText = '';

  constructor(config: GameConfig) {
    this.#config = config;
    this.#day = new WorkDay(config);
  }

  /** Call from gameUpdate (fixed 60 Hz). */
  update(): void {
    // LittleJS maps touch to mouse button 0, so one code path covers both.
    if (mouseWasPressed(0)) {
      this.#handleTap();
    }
    for (const event of this.#day.tick(timeDelta)) {
      if (event.type === 'taskFailed') {
        this.#warn(taskFailureText[event.reason]);
      }
    }
  }

  /** Call from gameRender: everything in world space. */
  render(): void {
    for (const stationId of stationIds) {
      this.#renderStation(stationId);
    }
    this.#renderBeekeeper();
  }

  /** Call from gameRenderPost: the HUD, in screen space. */
  renderPost(): void {
    const status = this.#day.state.status;
    const centerX = viewport.canvasWidthPx / 2;

    drawTextScreen(
      formatClock(this.#day.remainingSeconds),
      vec2(centerX, theme.hud.clockTopPx),
      theme.font.sizePx.clock,
      theme.colors.text,
    );
    drawTextScreen(
      statusHint(status),
      vec2(centerX, theme.hud.hintTopPx),
      theme.font.sizePx.hint,
      theme.colors.textMuted,
    );
    if (this.#warningTimer.active()) {
      drawTextScreen(
        this.#warningText,
        vec2(centerX, theme.hud.warningTopPx),
        theme.font.sizePx.hud,
        theme.colors.warning,
      );
    }

    drawTextScreen(
      productionSummary(this.#day.state.production),
      vec2(centerX, theme.hud.debugTopPx),
      theme.font.sizePx.debug,
      theme.colors.text,
    );

    this.#renderQueue();
    this.#renderButton(status);
  }

  #handleTap(): void {
    const tasks = this.#tasksInRunOrder();
    const slot = this.#queueSlotRects().findIndex((rect) => rectContains(rect, mousePosScreen));
    if (slot >= 0) {
      const task = tasks[slot];
      if (task) {
        this.#day.cancel(task.id);
      }
      return;
    }

    if (rectContains(this.#buttonRect(), mousePosScreen)) {
      if (this.#day.state.status === 'ended') {
        const production = this.#day.state.production;
        this.#day = new WorkDay(this.#config, WorkDay.initialState(this.#config, production));
      } else {
        this.#day.endDayEarly();
      }
      return;
    }

    const stationId = stationIds.find((id) => rectContains(this.#stationRect(id), mousePos));
    if (stationId) {
      this.#flashedStation = stationId;
      this.#flashTimer.set(theme.flashSeconds);
      const result = this.#day.enqueue(stationId);
      if (!result.ok) {
        this.#warn(refusalText(result.reason));
      }
    }
  }

  #warn(text: string): void {
    this.#warningText = text;
    this.#warningTimer.set(theme.warningSeconds);
  }

  #renderStation(stationId: StationId): void {
    const visual = stationVisuals[stationId];
    const rect = this.#stationRect(stationId);
    const flashing = this.#flashedStation === stationId && this.#flashTimer.active();

    drawRect(rect.center, rect.size, flashing ? theme.colors.tapFlash : visual.color);
    drawText(visual.icon, rect.center, theme.world.stationIconSize);
    drawText(
      visual.label,
      rect.center.subtract(vec2(0, theme.world.stationLabelOffset)),
      theme.world.stationLabelSize,
      theme.colors.text,
    );
  }

  #renderBeekeeper(): void {
    const { beekeeperPosition, activeTask } = this.#day.state;
    const pos = vec2(beekeeperPosition.x, beekeeperPosition.y);

    drawCircle(pos, theme.world.beekeeperSize, theme.colors.beekeeper);
    drawText('🧑‍🌾', pos, theme.world.beekeeperIconSize);

    if (activeTask?.phase === 'working') {
      const barCenter = pos.add(vec2(0, theme.world.progressOffset));
      const barSize = vec2(theme.world.progressWidth, theme.world.progressHeight);
      drawProgressBar({ center: barCenter, size: barSize }, workProgress(activeTask), false);
    }
  }

  #renderQueue(): void {
    const tasks = this.#tasksInRunOrder();
    const activeTask = this.#day.state.activeTask;

    this.#queueSlotRects().forEach((rect, index) => {
      const task = tasks[index];
      drawScreenRect(rect, task ? theme.colors.hudPanel : theme.colors.hudSlotEmpty);
      if (!task) {
        return;
      }
      drawTextScreen(stationVisuals[task.stationId].icon, rect.center, theme.font.sizePx.slotIcon);

      if (index === 0 && activeTask) {
        // Thin bar along the slot's bottom edge: empty while walking, fills while working.
        const barHeight = theme.hud.queueProgressHeightPx;
        const bar: Rect = {
          center: rect.center.add(vec2(0, (rect.size.y - barHeight) / 2)),
          size: vec2(rect.size.x, barHeight),
        };
        const progress = activeTask.phase === 'working' ? workProgress(activeTask) : 0;
        drawProgressBar(bar, progress, true);
      }
    });
  }

  #renderButton(status: DayStatus): void {
    const rect = this.#buttonRect();
    drawScreenRect(
      rect,
      status === 'windingDown' ? theme.colors.hudSlotEmpty : theme.colors.button,
    );
    drawTextScreen(buttonLabel(status), rect.center, theme.font.sizePx.button, theme.colors.text);
  }

  /** The active task first, then the queued tasks: the order the queue slots show them. */
  #tasksInRunOrder(): readonly Task[] {
    const { activeTask, queuedTasks } = this.#day.state;
    return activeTask ? [activeTask.task, ...queuedTasks] : queuedTasks;
  }

  #stationRect(stationId: StationId): Rect {
    const { x, y } = this.#config.backyard.stations[stationId];
    return { center: vec2(x, y), size: vec2(theme.world.stationSize) };
  }

  /** One slot per task the queue can hold, along the bottom-left edge. */
  #queueSlotRects(): Rect[] {
    const { edgePx, queueSlotSizePx, queueSlotGapPx } = theme.hud;
    const y = viewport.canvasHeightPx - edgePx - queueSlotSizePx / 2;
    return Array.from({ length: this.#config.queue.maxTasks }, (_, index) => ({
      center: vec2(edgePx + queueSlotSizePx / 2 + index * (queueSlotSizePx + queueSlotGapPx), y),
      size: vec2(queueSlotSizePx),
    }));
  }

  #buttonRect(): Rect {
    const { edgePx, buttonWidthPx, buttonHeightPx } = theme.hud;
    return {
      center: vec2(
        viewport.canvasWidthPx - edgePx - buttonWidthPx / 2,
        viewport.canvasHeightPx - edgePx - buttonHeightPx / 2,
      ),
      size: vec2(buttonWidthPx, buttonHeightPx),
    };
  }
}

function workProgress(activeTask: Extract<ActiveTask, { phase: 'working' }>): number {
  const { workTotalSeconds, workRemainingSeconds } = activeTask;
  return workTotalSeconds > 0 ? 1 - workRemainingSeconds / workTotalSeconds : 1;
}

/** Temporary one-line readout of the production state, until the inventory HUD (step 4). */
function productionSummary(production: ProductionState): string {
  const { honeySuper, extractor, bucketHoneyOz, tankHoneyOz, inventory } = production;
  const superText =
    honeySuper.location === 'hive'
      ? `on hive ${String(Math.floor(honeySuper.fill * 100))}%`
      : `${honeySuper.location === 'carried' ? 'carried' : 'on table'} ` +
        `(capped ${String(honeySuper.frames.capped)}, uncapped ${String(honeySuper.frames.uncapped)}, ` +
        `empty ${String(honeySuper.frames.extracted)})`;
  return [
    `Super: ${superText}`,
    `Extractor: ${String(extractor.loadedFrames)} frames`,
    `Bucket: ${String(bucketHoneyOz)} oz`,
    `Tank: ${String(tankHoneyOz)} oz`,
    `Bears: ${String(inventory.filledBears)} filled, ${String(inventory.emptyBears)} empty, ${String(inventory.labels)} labels`,
  ].join('  ·  ');
}

function drawScreenRect(rect: Rect, color: Color): void {
  drawRect(rect.center, rect.size, color, 0, false, true);
}

/** Left-to-right fill. `screenSpace` picks the HUD (pixels) or the scene (world units). */
function drawProgressBar(rect: Rect, progress: number, screenSpace: boolean): void {
  const fillWidth = rect.size.x * Math.min(1, Math.max(0, progress));
  const fillCenter = vec2(rect.center.x - (rect.size.x - fillWidth) / 2, rect.center.y);
  // HUD bars must skip WebGL to draw above the scene; world bars use the engine default.
  const useWebGL = screenSpace ? false : undefined;
  drawRect(rect.center, rect.size, theme.colors.progressTrack, 0, useWebGL, screenSpace);
  drawRect(
    fillCenter,
    vec2(fillWidth, rect.size.y),
    theme.colors.progressFill,
    0,
    useWebGL,
    screenSpace,
  );
}

/** Remaining day time as m:ss, rounded up so the clock reads 0:00 only when time is up. */
function formatClock(seconds: number): string {
  const whole = Math.ceil(seconds);
  const minutes = Math.floor(whole / 60);
  const rest = whole % 60;
  return `${String(minutes)}:${String(rest).padStart(2, '0')}`;
}

function statusHint(status: DayStatus): string {
  switch (status) {
    case 'running':
      return 'Tap a station to queue a task. Tap a queued task to cancel it.';
    case 'windingDown':
      return "Time's up: finishing the current task.";
    case 'ended':
      return 'Day over.';
  }
}

function buttonLabel(status: DayStatus): string {
  switch (status) {
    case 'running':
      return 'End day';
    case 'windingDown':
      return 'Ending…';
    case 'ended':
      return 'New day';
  }
}

function refusalText(reason: EnqueueRefusal): string {
  switch (reason) {
    case 'queueFull':
      return 'Queue is full';
    case 'dayOver':
      return 'The day is over';
  }
}
