import type { Color } from 'littlejsengine';
import { stationStateOf } from '../../../sim/stations/productionAccess';
import { drawFilledRect, drawPips, insetRect } from '../../draw';
import { theme } from '../../theme';
import type { StationRenderer } from '../StationRenderer';
import { statusRowRect } from '../StationRenderer';

/** The super on the table as one pip per frame: capped, uncapped, empty, or away in the extractor. */
export const uncappingTableRenderer: StationRenderer = {
  drawStatus({ station, rect, production, config }) {
    const table = stationStateOf(production, station.id, 'uncappingTable');
    if (table.superId === null) {
      return;
    }
    const contents = production.supers[table.superId]?.contents;
    if (contents?.stage !== 'harvested') {
      return;
    }

    const { capped, uncapped, extracted } = contents.frames;
    const away = config.shallowSuper.frames - capped - uncapped - extracted;
    const colors: Color[] = [
      ...Array<Color>(capped).fill(theme.colors.frameCapped),
      ...Array<Color>(uncapped).fill(theme.colors.frameUncapped),
      ...Array<Color>(extracted).fill(theme.colors.frameEmpty),
      ...Array<Color>(Math.max(0, away)).fill(theme.colors.frameSlotEmpty),
    ];

    const box = statusRowRect(rect, 0, theme.world.superHeight);
    drawFilledRect(box, theme.colors.superBox);
    drawPips(insetRect(box, theme.world.pipGap), colors);
  },
};
