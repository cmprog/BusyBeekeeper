import {
  drawCircle,
  drawText,
  drawTextScreen,
  mousePos,
  mousePosScreen,
  mouseWasPressed,
  timeDelta,
  Timer,
  vec2,
} from 'littlejsengine';
import type { GameConfig } from '../../config/GameConfig';
import type { ProductionState } from '../../sim/production/ProductionState';
import { createSaveData } from '../../sim/save/createSaveData';
import type { CalendarDay } from '../../sim/week/CalendarDay';
import type { GameState } from '../../sim/week/GameState';
import type { Station, StationId } from '../../sim/stations/Station';
import type { StationAction } from '../../sim/stations/StationAction';
import type { Task } from '../../sim/workday/Task';
import { WorkDay } from '../../sim/workday/WorkDay';
import type { EnqueueRefusal } from '../../sim/workday/WorkDay';
import type { ActiveTask, DayStatus, WorkDayState } from '../../sim/workday/WorkDayState';
import { actionVerbs } from '../actionVerbs';
import { shortDayName, weekdayName } from '../calendarText';
import { drawFilledRect, drawProgressBar } from '../draw';
import { drawButton } from '../hud/drawButton';
import { drawInventoryPanel } from '../hud/drawInventoryPanel';
import type { InventoryHudItem } from '../hud/drawInventoryPanel';
import { rectContains } from '../Rect';
import type { Rect } from '../Rect';
import { areaRect } from '../SceneArea';
import type { SceneArea } from '../SceneArea';
import { stationRenderers } from '../stations/stationRenderers';
import { stationVisuals } from '../stationVisuals';
import { taskFailureText } from '../taskFailureText';
import { theme } from '../theme';
import { viewport } from '../viewport';
import type { Scene } from './Scene';
import type { SceneContext } from './SceneContext';

const backyardAreas: readonly SceneArea[] = [
  { label: 'Bee Yard', color: theme.colors.yardArea, stationTypes: ['hive'] },
  {
    label: 'Honey House',
    color: theme.colors.honeyHouseArea,
    stationTypes: ['uncappingTable', 'extractor', 'bucket', 'bottlingTank'],
  },
];

/** What the backyard's inventory panel shows: bottling stock. */
function backyardInventory(production: ProductionState): readonly InventoryHudItem[] {
  const { filledBears, emptyBears, labels } = production.inventory;
  return [
    { icon: '🍯', count: filledBears },
    { icon: '🫙', count: emptyBears },
    { icon: '🏷️', count: labels },
  ];
}

/**
 * Phase 1 test scene: the backyard, with the Bee Yard and the Honey House. Tap a station to
 * queue its task, tap a queue slot to cancel it, and end the day early, then Sleep (stock
 * carries over) with the button. Menu saves and returns to the main menu. Saves when the day
 * ends and whenever the page is hidden (Sleep saves tomorrow).
 *
 * Owns no game rules: it forwards input to the sim's `WorkDay` and draws its state.
 * The HUD is drawn with `useWebGL = false` so it lands on the 2D layer above the scene.
 */
export class BackyardScene implements Scene {
  readonly #context: SceneContext;
  readonly #config: GameConfig;
  readonly #calendar: CalendarDay;
  readonly #day: WorkDay;
  #saveFailureShown = false;
  readonly #flashTimer = new Timer();
  #flashedStation: StationId | undefined;
  readonly #warningTimer = new Timer();
  #warningText = '';

  constructor(
    context: SceneContext,
    calendar: CalendarDay,
    production: ProductionState,
    workDay: WorkDayState,
  ) {
    this.#context = context;
    this.#config = context.config;
    this.#calendar = calendar;
    this.#day = new WorkDay(this.#config, production, workDay);
  }

  onPageHidden(): void {
    this.#saveNow();
  }

  get #game(): GameState {
    return {
      calendar: this.#calendar,
      production: this.#day.production,
      day: { kind: 'work', workDay: this.#day.state },
    };
  }

  get #layout(): readonly Station[] {
    return this.#config.backyard.stations;
  }

  update(): void {
    const statusBefore = this.#day.state.status;
    // LittleJS maps touch to mouse button 0, so one code path covers both.
    if (mouseWasPressed(0)) {
      this.#handleTap();
    }
    for (const event of this.#day.tick(timeDelta)) {
      if (event.type === 'taskFailed') {
        this.#warn(taskFailureText[event.reason]);
      }
    }
    if (statusBefore !== 'ended' && this.#day.state.status === 'ended') {
      this.#saveNow();
    }
  }

  render(): void {
    for (const area of backyardAreas) {
      this.#renderArea(area);
    }
    for (const station of this.#layout) {
      this.#renderStation(station);
    }
    this.#renderBeekeeper();
  }

  renderPost(): void {
    const status = this.#day.state.status;
    const centerX = viewport.canvasWidthPx / 2;

    drawTextScreen(
      shortDayName(this.#calendar),
      vec2(theme.hud.edgePx, theme.hud.clockTopPx),
      theme.font.sizePx.day,
      theme.colors.text,
      0,
      undefined,
      'left',
    );
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

    drawInventoryPanel(backyardInventory(this.#day.production));
    this.#renderQueue();
    this.#renderButtons(status);
    if (status === 'ended') {
      this.#renderDaySummary();
    }
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

    if (rectContains(this.#menuButtonRect(), mousePosScreen)) {
      this.#saveNow();
      this.#context.goTo({ kind: 'mainMenu' });
      return;
    }

    if (rectContains(this.#buttonRect(), mousePosScreen)) {
      if (this.#day.state.status === 'ended') {
        this.#context.endDay(this.#game);
      } else {
        this.#day.endDayEarly();
      }
      return;
    }

    const station = this.#layout.find((candidate) =>
      rectContains(stationRect(candidate), mousePos),
    );
    if (station) {
      this.#flashedStation = station.id;
      this.#flashTimer.set(theme.flashSeconds);
      const result = this.#day.enqueue(station.id);
      if (!result.ok) {
        this.#warn(refusalText(result.reason));
      }
    }
  }

  /** Saves the game now. Shows a notice the first time saving fails. */
  #saveNow(): void {
    const saved = this.#context.persist(createSaveData(this.#game));
    if (!saved && !this.#saveFailureShown) {
      this.#saveFailureShown = true;
      this.#warn(
        "Progress couldn't be saved (storage may be full or blocked).",
        theme.noticeSeconds,
      );
    }
  }

  #warn(text: string, seconds: number = theme.warningSeconds): void {
    this.#warningText = text;
    this.#warningTimer.set(seconds);
  }

  /** The end-of-day panel, centred over the scene until the next day starts. */
  #renderDaySummary(): void {
    const { stats } = this.#day.state;
    const lines = [
      `🍯 Bears bottled: ${String(stats.bearsBottled)}`,
      `Honey extracted: ${String(stats.honeyExtractedOz)} oz`,
      `Frames uncapped: ${String(stats.framesUncapped)}`,
      `Unused time: ${formatClock(this.#day.remainingSeconds)}`,
    ];
    const { summaryCenterYPx, summaryWidthPx, summaryPaddingPx, summaryLineHeightPx } = theme.hud;
    // One line for the title, then one per stat.
    const height = summaryPaddingPx * 2 + summaryLineHeightPx * (lines.length + 1);
    const centerX = viewport.canvasWidthPx / 2;
    const top = summaryCenterYPx - height / 2;

    drawFilledRect(
      { center: vec2(centerX, summaryCenterYPx), size: vec2(summaryWidthPx, height) },
      theme.colors.hudPanel,
      true,
    );
    const lineY = (index: number): number =>
      top + summaryPaddingPx + summaryLineHeightPx * (index + 0.5);
    drawTextScreen(
      `${weekdayName(this.#calendar.weekday)} complete`,
      vec2(centerX, lineY(0)),
      theme.font.sizePx.summaryTitle,
      theme.colors.text,
    );
    lines.forEach((line, index) => {
      drawTextScreen(
        line,
        vec2(centerX, lineY(index + 1)),
        theme.font.sizePx.summaryLine,
        theme.colors.text,
      );
    });
  }

  #renderArea(area: SceneArea): void {
    const rect = areaRect(area, this.#layout);
    if (!rect) {
      return;
    }
    drawFilledRect(rect, area.color);
    const labelY = rect.center.y + rect.size.y / 2 - theme.world.areaLabelInset;
    drawText(area.label, vec2(rect.center.x, labelY), theme.world.areaLabelSize, theme.colors.text);
  }

  #renderStation(station: Station): void {
    const visual = stationVisuals[station.type];
    const renderer = stationRenderers[station.type];
    const rect = stationRect(station);
    const flashing = this.#flashedStation === station.id && this.#flashTimer.active();
    const context = {
      station,
      rect,
      production: this.#day.production,
      config: this.#config,
      activeAction: this.#activeActionAt(station.id),
    };

    drawFilledRect(rect, flashing ? theme.colors.tapFlash : visual.color);
    drawText(
      visual.icon,
      rect.center,
      theme.world.stationIconSize,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      undefined,
      renderer.iconAngle?.(context) ?? 0,
    );
    drawText(
      visual.label,
      rect.center.subtract(vec2(0, theme.world.stationLabelOffset)),
      theme.world.stationLabelSize,
      theme.colors.text,
    );
    renderer.drawStatus(context);
  }

  #renderBeekeeper(): void {
    const { beekeeperPosition, activeTask } = this.#day.state;
    const { production } = this.#day;
    const pos = vec2(beekeeperPosition.x, beekeeperPosition.y);

    drawCircle(pos, theme.world.beekeeperSize, theme.colors.beekeeper);
    drawText('🧑‍🌾', pos, theme.world.beekeeperIconSize);

    if (production.carriedSuperId !== null) {
      drawFilledRect(
        {
          center: pos.add(vec2(theme.world.carriedSuperOffsetX, 0)),
          size: vec2(theme.world.carriedSuperWidth, theme.world.carriedSuperHeight),
        },
        theme.colors.superBox,
      );
    }

    if (activeTask?.phase === 'working') {
      drawProgressBar(
        {
          center: pos.add(vec2(0, theme.world.progressOffset)),
          size: vec2(theme.world.progressWidth, theme.world.progressHeight),
        },
        workProgress(activeTask),
      );
    }
  }

  #renderQueue(): void {
    const tasks = this.#tasksInRunOrder();
    const activeTask = this.#day.state.activeTask;

    this.#queueSlotRects().forEach((rect, index) => {
      const task = tasks[index];
      drawFilledRect(rect, task ? theme.colors.hudPanel : theme.colors.hudSlotEmpty, true);
      if (!task) {
        return;
      }

      const working = index === 0 && activeTask?.phase === 'working' ? activeTask : undefined;
      const iconCenter = working
        ? rect.center.subtract(vec2(0, theme.hud.slotIconLiftPx))
        : rect.center;
      drawTextScreen(this.#stationIcon(task.stationId), iconCenter, theme.font.sizePx.slotIcon);
      if (working) {
        drawTextScreen(
          actionVerbs[working.planned.action.kind],
          rect.center.add(vec2(0, theme.hud.slotVerbOffsetPx)),
          theme.font.sizePx.slotVerb,
          theme.colors.text,
        );
      }

      if (index === 0 && activeTask) {
        // Thin bar along the slot's bottom edge: empty while walking, fills while working.
        const barHeight = theme.hud.queueProgressHeightPx;
        const bar: Rect = {
          center: rect.center.add(vec2(0, (rect.size.y - barHeight) / 2)),
          size: vec2(rect.size.x, barHeight),
        };
        drawProgressBar(bar, working ? workProgress(working) : 0, true);
      }
    });
  }

  #renderButtons(status: DayStatus): void {
    drawButton(this.#menuButtonRect(), 'Menu');
    drawButton(this.#buttonRect(), buttonLabel(status), { enabled: status !== 'windingDown' });
  }

  /** The active task first, then the queued tasks: the order the queue slots show them. */
  #tasksInRunOrder(): readonly Task[] {
    const { activeTask, queuedTasks } = this.#day.state;
    return activeTask ? [activeTask.task, ...queuedTasks] : queuedTasks;
  }

  #activeActionAt(stationId: StationId): StationAction | undefined {
    const activeTask = this.#day.state.activeTask;
    return activeTask?.phase === 'working' && activeTask.task.stationId === stationId
      ? activeTask.planned.action
      : undefined;
  }

  #stationIcon(stationId: StationId): string {
    const station = this.#layout.find((candidate) => candidate.id === stationId);
    return station ? stationVisuals[station.type].icon : '?';
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

  /** Left of the day button, along the bottom edge. */
  #menuButtonRect(): Rect {
    const { edgePx, buttonWidthPx, buttonHeightPx, smallButtonWidthPx, queueSlotGapPx } = theme.hud;
    return {
      center: vec2(
        viewport.canvasWidthPx - edgePx - buttonWidthPx - queueSlotGapPx - smallButtonWidthPx / 2,
        viewport.canvasHeightPx - edgePx - buttonHeightPx / 2,
      ),
      size: vec2(smallButtonWidthPx, buttonHeightPx),
    };
  }
}

type WorkingTask = Extract<ActiveTask, { phase: 'working' }>;

function stationRect(station: Station): Rect {
  const { x, y } = station.position;
  return { center: vec2(x, y), size: vec2(theme.world.stationSize) };
}

function workProgress(activeTask: WorkingTask): number {
  const { workTotalSeconds, workRemainingSeconds } = activeTask;
  return workTotalSeconds > 0 ? 1 - workRemainingSeconds / workTotalSeconds : 1;
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
      return 'Sleep';
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
