import { stationStateOf } from '../../../sim/stations/productionAccess';
import type { StationRenderer } from '../StationRenderer';
import { drawHoneyGauge } from './honeyGauge';

export const bottlingTankRenderer: StationRenderer = {
  drawStatus({ station, rect, production, config }) {
    const tank = stationStateOf(production, station.id, 'bottlingTank');
    drawHoneyGauge(rect, tank.honeyOz, config.equipment.bottlingTankCapacityOz);
  },
};
