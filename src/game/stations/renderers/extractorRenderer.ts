import { time } from 'littlejsengine';
import type { Color } from 'littlejsengine';
import { stationStateOf } from '../../../sim/stations/productionAccess';
import { drawPips } from '../../draw';
import { theme } from '../../theme';
import type { StationRenderer } from '../StationRenderer';
import { statusRowRect } from '../StationRenderer';

/** One pip per extractor slot, filled for each loaded frame. The icon spins while cranking. */
export const extractorRenderer: StationRenderer = {
  drawStatus({ station, rect, production, config }) {
    const extractor = stationStateOf(production, station.id, 'extractor');
    const slots = config.equipment.extractorFrames;
    const colors: Color[] = Array.from({ length: slots }, (_, index) =>
      index < extractor.loadedFrames ? theme.colors.frameUncapped : theme.colors.frameSlotEmpty,
    );
    drawPips(statusRowRect(rect, 0, theme.world.statusBarHeight), colors);
  },

  iconAngle({ activeAction }) {
    return activeAction?.kind === 'extract' ? time * theme.motion.extractorSpinRadiansPerSecond : 0;
  },
};
