import { drawText, vec2 } from 'littlejsengine';
import type { Rect } from '../../Rect';
import { drawProgressBar } from '../../draw';
import { theme } from '../../theme';
import { statusRowRect } from '../StationRenderer';

/** A fill gauge above a station with "honey / capacity oz" on it. Used by honey containers. */
export function drawHoneyGauge(stationRect: Rect, honeyOz: number, capacityOz: number): void {
  const gauge = statusRowRect(stationRect, 0, theme.world.statusBarHeight);
  drawProgressBar(gauge, capacityOz > 0 ? honeyOz / capacityOz : 0);
  drawText(
    `${String(honeyOz)}/${String(capacityOz)} oz`,
    gauge.center.subtract(vec2(0, theme.world.gaugeTextDrop)),
    theme.world.gaugeTextSize,
    theme.colors.text,
    theme.world.gaugeTextOutline,
    theme.colors.textOutline,
  );
}
