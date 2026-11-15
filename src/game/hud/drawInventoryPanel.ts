import { drawTextScreen, vec2 } from 'littlejsengine';
import { drawFilledRect } from '../draw';
import { theme } from '../theme';
import { viewport } from '../viewport';

export interface InventoryHudItem {
  readonly icon: string;
  readonly count: number;
}

/**
 * The inventory panel in the top-right corner. Each scene decides which items it shows (the
 * backyard shows bottling stock; the market will show what's for sale).
 */
export function drawInventoryPanel(items: readonly InventoryHudItem[]): void {
  const { edgePx, inventoryItemWidthPx, inventoryPanelHeightPx } = theme.hud;
  const width = items.length * inventoryItemWidthPx;
  const left = viewport.canvasWidthPx - edgePx - width;
  const centerY = edgePx + inventoryPanelHeightPx / 2;

  drawFilledRect(
    { center: vec2(left + width / 2, centerY), size: vec2(width, inventoryPanelHeightPx) },
    theme.colors.hudPanel,
    true,
  );
  items.forEach((item, index) => {
    drawTextScreen(
      `${item.icon} ${String(item.count)}`,
      vec2(left + inventoryItemWidthPx * (index + 0.5), centerY),
      theme.font.sizePx.inventory,
      theme.colors.text,
    );
  });
}
