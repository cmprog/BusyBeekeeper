import type { GameConfig } from '../../config/GameConfig';
import type { Task } from './Task';

/** How long a task takes once the beekeeper has arrived at its station. */
export function taskWorkSeconds(task: Task, config: GameConfig): number {
  switch (task.kind) {
    // eslint-disable-next-line @typescript-eslint/no-unnecessary-condition -- Task has one variant until step 3; the switch keeps the exhaustiveness check in place.
    case 'placeholder':
      return config.tasks.placeholderWorkSeconds;
  }
}
