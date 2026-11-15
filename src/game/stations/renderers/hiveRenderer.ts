import { stationStateOf } from '../../../sim/stations/productionAccess';
import { drawFilledRect, drawProgressBar, insetRect } from '../../draw';
import { theme } from '../../theme';
import type { StationRenderer } from '../StationRenderer';
import { statusRowRect } from '../StationRenderer';

/** Each super on the hive as a box stacked on the brood box, with a fill bar inside. */
export const hiveRenderer: StationRenderer = {
  drawStatus({ station, rect, production }) {
    const hive = stationStateOf(production, station.id, 'hive');
    // A list, so stacking several supers later is a data change, not a drawing change.
    const superIds = hive.superId === null ? [] : [hive.superId];

    superIds.forEach((superId, index) => {
      const box = statusRowRect(rect, index, theme.world.superHeight);
      drawFilledRect(box, theme.colors.superBox);
      const contents = production.supers[superId]?.contents;
      const fill = contents?.stage === 'filling' ? contents.fill : 0;
      drawProgressBar(insetRect(box, theme.world.pipGap), fill);
    });
  },
};
