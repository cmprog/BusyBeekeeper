import { stationStateOf } from '../../../sim/stations/productionAccess';
import type { StationRenderer } from '../StationRenderer';
import { drawHoneyGauge } from './honeyGauge';

export const bucketRenderer: StationRenderer = {
  drawStatus({ station, rect, production, config }) {
    const bucket = stationStateOf(production, station.id, 'bucket');
    drawHoneyGauge(rect, bucket.honeyOz, config.equipment.bucketCapacityOz);
  },
};
