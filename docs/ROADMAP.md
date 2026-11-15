# Busy Beekeeper — Roadmap

> Living design document. Captures what has been **decided**, what is **still open**, and a
> phased plan for building it. Update as ideas firm up.

---

## 1. Vision

**Busy Beekeeper** is a **time management game with incremental progression**.

- **Time management** (Diner Dash, Delicious, Farm Frenzy): the beekeeper walks between
  stations, and each task takes time to do. The player queues up tasks, and choosing a good
  order with little walking is the core skill.
- **Incremental / idle** (Cookie Clicker, AdVenture Capitalist, Eatventure): earnings pay
  for upgrades, automation, and new mechanics. The game keeps opening new goals, and time
  spent away from the game still counts for something.

The player runs an apiary over a repeating **week**: five working days caring for hives and
processing honey, a **Saturday market** to sell goods, and a **Sunday** to restock supplies.
Weekly planning matters as much as playing each moment well.

---

## 2. Core Pillars

1. **Active play is rewarded, not required.** The beekeeper (player character) always has an
   edge over automated helpers. Idle progress exists, but hands-on play is the best way.
2. **Travel time is a resource.** Stations are spread out, and walking between them costs
   precious time. So does travelling between locations. Layout, routing, and the order of
   queued tasks matter.
3. **Plan the week, not just the minute.** Running out of jars on Wednesday, or making too
   little product before Saturday, should be real and recoverable mistakes.
4. **Introduce complexity slowly.** New pressures (seasons, pests, swarming…) unlock over time,
   and each one brings new upgrades to deal with it.
5. **Phone-first friendly.** Plays well on touch from day one, and is installable as a PWA.
6. **Almost every limit is an upgrade path.** Queue length, customer patience, storage, banked
   time, script length… Constraints are designed so the player can push past them.

---

## 3. Decided Design

### 3.1 The Beekeeper & the Task Queue

- The player controls one main character, **the beekeeper**.
- The beekeeper has a **task queue**. Tapping a station or action adds a task to the end.
- Each task = **travel to the station** + **perform the task** (has a duration).
- Queued tasks can be **cancelled** but **not reordered**. To reorder, cancel and re-queue.
- The queue holds tasks in the **current scene only**. The player never queues tasks in a scene
  they aren't looking at.
- **"Travel"** is a task that can be queued. It ends the scene's queue and opens the
  Overworld (§3.7).
- The queue length limit is a likely upgrade path (see Open Questions).
- Helper scripts **will** span scenes (§3.2), but the player's queue stays scoped to the current
  scene to keep the UX simple.

### 3.2 Helpers (Workers) & Scripts

- Helpers are unlocked over time and **automate** work.
- Helpers run **scripts**: repeating task lists the player sets up.
- Scripts are an upgrade path:
  - Max number of tasks per script
  - Which task types a helper can be scripted to do
  - (Possibly) conditional steps, helper speed, carry capacity, etc.
- Unlike the player's queue, helper scripts **can span multiple scenes** (e.g., pull supers in a
  yard → travel → extract at the Honey House).
- The beekeeper stays better than helpers (faster, more task types, bonuses, etc.) so active
  play stays the best option.

### 3.3 Banked Time (Offline Progress)

- While the game is closed, **time away is tracked and banked**, up to a **cap**.
- The game world does **not** advance while closed. Banked time is a resource instead.
- During active play, the player can **opt in** to spend banked time as a **speed boost**.
- Upgrade paths:
  - Banked time cap
  - Boost multiplier (how fast time runs while boosting)
  - (Possibly) how fast time is banked
  - **Unused day time → banked time**: an _upgrade_ (not available at the start) that turns
    the time left over from ending a day early into banked time

### 3.4 The Weekly Cycle

| Day     | Phase          | Focus                                                               | Length                          |
| ------- | -------------- | ------------------------------------------------------------------- | ------------------------------- |
| Mon–Fri | **Work days**  | Hive care, harvesting, extracting, bottling, and preparing products | 2 min each (starting value)     |
| Sat     | **Market day** | Fill customer orders at the market stall                            | 2 min (tuned separately)        |
| Sun     | **Supply day** | Buy inventory (jars, frames, supplies, equipment) for the next week | **No time limit** (menu screen) |

**Day rules**

- **Any timed day can be ended early** if the player chooses. Ending early gives no reward at
  the start (see the banked time upgrade in §3.3).
- **When the day ends:** the task **in progress finishes**, and any **remaining queued tasks
  are dropped** (not run).
- Work day and market day lengths are **separate tuning values**, even though they start out equal.
- **Sunday** is a simple menu with no timer. The player decides when they're done shopping.

**Weekly economy**

- Supplies are limited, so running out mid-week is possible. The week has to be planned.
- Saturday income pays for Sunday orders and for upgrades.

### 3.5 Market Day (Saturday)

- **Active play**: customers arrive with **orders**, and the player fills them from the products
  they prepared during the week.
- **Orders can contain several items** (e.g., "2 jars of honey + 1 candle").
- **Customers have limited patience.** Upgrade paths: more patience, plus items or décor that
  slow the patience timer.
- **Unsold stock carries over** to the next week, up to **storage limits** (an upgrade path).
- The same queue / travel / task rules apply, inside the market scene.
- What was prepared Mon–Fri limits what can be sold, so Saturday rewards good weekly planning.

### 3.6 Pressures (gradually unlocked)

Real beekeeping challenges create pressure and give each upgrade something to solve. They
unlock **one at a time** so players learn each concept before the next arrives.

Candidate pressures (not final, see Open Questions):

- **Seasons**: nectar flow, winter, spring buildup
- **Weather**: rainy days limit hive work or foraging
- **Colony health**: queen condition, population, brood
- **Swarming**: crowded colonies leave unless managed
- **Pests & disease**: varroa mites, wax moths, hive beetles
- **Product spoilage**: honey crystallizing, demand changes at market

### 3.7 Scenes

The game is split into separate **scenes**. Moving between location scenes costs time.

| Scene           | Type               | Purpose                                                                                                            |
| --------------- | ------------------ | ------------------------------------------------------------------------------------------------------------------ |
| **Bee Yard**    | Location (several) | Hive care and harvesting. There can be **multiple bee yards**: one scene type, each with small visual differences. |
| **Honey House** | Location           | **Extraction and bottling** (uncap, extract, filter, bottle, label, value-added products)                          |
| **Market**      | Location (Sat)     | Fill customer orders at the stall                                                                                  |
| **Shop**        | Menu (Sun)         | Untimed shopping menu: supplies, equipment, upgrades                                                               |
| **Overworld**   | Navigation         | Map where the player picks which location to travel to                                                             |
| **Travel**      | Transition         | Plays while moving between locations, and stands for the travel time spent                                         |
| **Sleep**       | Transition         | Plays between days (end-of-day → next morning)                                                                     |

#### Travel flow

1. The player queues a **Travel** task in the current scene (e.g., walk to the truck or exit).
2. When the beekeeper reaches that task, the game switches to the **Overworld**.
3. The player picks a destination.
4. The **Travel** transition plays. Its length depends on the **destination's distance** and the
   **vehicle**.
5. The player arrives in the destination scene with an empty queue.

#### The Backyard (starting location)

- The game starts in the player's **backyard**, which holds both a small **bee yard** and the
  **Honey House** (extraction + bottling).
- Moving between the backyard yard and the Honey House takes **no travel time** and **needs no
  vehicle**.
- The backyard yard has **low capacity** (few hives/supers), so the player eventually has to
  expand to other yards.
- This keeps the early game simple: no travel at first, and travel arrives with the first
  outside yard.

#### Distance & Vehicles

- Each destination (Honey House, other bee yards, market) has its own **travel distance**
  (time taken).
- **Vehicles** are an upgrade path with two stats:
  - **Speed**: cuts travel time (e.g., a faster truck)
  - **Cargo capacity**: how many **supers** can be hauled from a yard back to the Honey House
    in one trip
- Faster vehicles and bigger cargo trade off against each other, which gives the player
  interesting purchase choices.

### 3.8 Screen Layout & Camera

- **Landscape** orientation (PWA manifest asks for landscape on phones).
- Each scene **starts as a single screen**. As the player unlocks or upgrades things, a scene
  can grow wider, and **horizontal scrolling** is used to move around inside it.
- Touch input has to tell "drag to scroll" apart from "tap to queue a task".

### 3.9 Central Tuning / Balance Config

All balance numbers live in **one central config**, a **plain TypeScript module** (typed, so a bad value fails the build), not spread
through the game code, so the game can be tuned and rebalanced over time without hunting
for magic numbers.

Examples of what belongs there:

- Day lengths (work day, market day)
- Walk speed, task durations, travel distances (in a scene and per destination)
- Vehicle speed and cargo capacity
- Yard limits (max hives, max hive size) and forage/risk factors
- Super sizes (frames, honey per frame, weight), carry limits, and how carrying slows walking
- Honey fill rates (yard × season × queen strength)
- Machine capacities (extractor frames, bucket volume, tank volume), flow rates, and maintenance
- Container volumes and prices
- Starting kit, including starting Beebucks (100) and onboarding gift contents
- Frame foundation lead time, inspection health cost, requeening risk
- Delivery lead time
- Queue length limits, script length limits
- Customer patience, order size ranges
- Storage limits
- Prices, costs, upgrade cost curves
- Banked time cap and boost multiplier
- When each pressure unlocks, and its rates

Nice extras (later): a debug panel to change values while the game runs, and a URL flag
or debug mode for fast-forwarding.

### 3.10 Platform & Tech

- **Engine:** [LittleJS](https://github.com/KilledByAPixel/LittleJS)
- **Delivery:** a static HTML file, hosted on **GitHub Pages**
- **PWA:** installable on phones (web manifest + service worker for offline play)
- **Input:** desktop (mouse) and mobile (touch) supported from the start
- **Config:** plain module (§3.9), written in TypeScript so it's type-checked
- **Language:** **TypeScript** (strict mode)
- **Build / dev server:** **Vite** (output is still static files for GitHub Pages)
- **Tests:** **Vitest** unit tests for critical paths only (see §3.11)
- **Saves:** `localStorage`, holding one **versioned JSON** save. Export/import isn't
  needed at first (easy to add later because the save is a single JSON string).
- **Art (prototype):** **coloured shapes + emoji**. Gameplay first, no time spent on art yet.
- **Playtesting:** mostly desktop at first. Friends & family will play on phones.

### 3.11 Engineering Principles

The goal is to **catch problems at build time, not at runtime**, and to give coding agents
**deterministic tooling feedback** (types, lint, tests) they can check their work against.

- **Strict typing.** TypeScript `strict` plus extra strictness flags (e.g.,
  `noUncheckedIndexedAccess`). No `any` without a written reason.
- **Simulation is separate from the engine.** Game rules (queue, clock, production, economy,
  saves) live in **plain TypeScript with no DOM imports**, so they can be unit tested in Node.
  The sim may use **allowlisted pure LittleJS utilities** (`RandomGenerator`, `Vector2`, math
  helpers), but not engine-loop or stateful parts (`Timer`, engine time, `EngineObject`, input,
  drawing). The sim owns the authoritative state, and LittleJS objects only visualise it.
- **Deterministic simulation.** Time and randomness are **passed in**: the simulation advances by
  an explicit `dt`, and randomness comes from a seeded LittleJS `RandomGenerator`. The same inputs
  always give the same results, so tests are reliable and bugs can be replayed.
- **Purpose-driven tests.** Tests cover **critical rules and risky logic** (e.g., queue
  cancel/end-of-day behaviour, production math, save migrations), not coverage for its own sake.
  Every test should protect a rule we'd be upset to see broken.
- **One check command.** A single `npm run check` (typecheck + lint + test) is the gate every
  change must pass.
- **Versioned saves with migrations.** Each save format version has a migration path, and
  migrations are tested.

---

## 4. Beekeeping & Production

### 4.1 Production Chain Overview

```
BEE YARD                         HONEY HOUSE                                         MARKET
Hive fills supers ──carry──▶ Uncap frames ──▶ Extractor ──▶ Bucket ──pour──▶ Bottling tank ──▶ Containers ──▶ Orders
(over time)       super(s)   (per frame)      (N frames)    (honey vol.)     (capacity)        (+ label)
      ▲                                           │
      └──── empty supers / frames go back ◀───────┘
```

Units change along the chain: **supers** (transport) → **frames** (processing) → **honey
volume** (bucket/tank) → **containers** (products).

### 4.2 Hives, Supers & Bees

- **Hives are not bought.** The player **builds a hive** in a yard from parts bought in the Shop:
  - **Brood box** (required, one per hive): where the colony lives. **Not harvested.** Bought
    separately and **includes a queen excluder** (no separate excluder item).
  - **Honey supers** stacked on top: these are what get filled and harvested.
- **Super sizes:** **deep**, **medium**, **shallow**. Each counts as one unit, but bigger
  supers hold more frames/honey and are heavier (§4.3).
- **The hive (its supers) sets capacity.** **The bees set productivity.**
- **Bees are bought in the Shop** and tracked **separately** from the hive structure.
- A colony is modelled by its **queen**, not by individual bees:
  - **Strong queen** → healthy, productive, sturdy colony
  - **Weak queen** → weak colony, slower honey, less resilient
  - **Queens don't age.**
  - **Requeening** (replacing the queen) is possible, but carries **some risk** (e.g., the colony
    rejects the new queen).
  - Room for more queen traits later (temperament, swarm tendency, disease resistance…)

**Frame state** (kept simple on purpose):

- Each frame is either **bare foundation** or **drawn comb**.
- Frames on bare foundation need **extra lead time** before they start filling with honey (the bees
  must build comb first).
- **Extracted** frames go back as **drawn comb**, ready to refill.
- **Comb honey** removes the comb, so frames go back **bare** (that's the comb honey trade-off).
- The Shop sells both **foundation frames** (cheap) and **drawn comb frames** (more expensive,
  but no lead time).

**Inspections:**

- Inspecting a hive **reveals problems** (mites, disease, weak queen…).
- **Over-inspecting hurts colony health**, so the player must choose how often to inspect.
- **Hives fill supers on their own over time.** The rate depends on:
  - **Yard characteristics**: local forage/pollen yield
  - **Season**
  - **Colony strength** (queen)
- **The player's job in the yard:**
  - Keep **empty supers** on hives so the bees have room to store honey
  - **Inspect** hives for colony health and dangers (mites and other natural problems)
  - Pull full supers and carry them back for processing

### 4.3 Carrying Supers

- The beekeeper can carry supers by hand. **Bigger supers are heavier** and affect:
  - How many can be carried at once
  - **Walk speed inside the scene** while carrying
- **Beekeeper strength** is a possible upgrade path.
- Between scenes, supers travel in **vehicle cargo** (§3.7).

### 4.4 Yards

- Each yard has **hard limits**:
  - **Max hives** (hive slots)
  - **Max hive size** (supers per hive)
- Yard limits **cannot be upgraded directly**. The player grows output by:
  - Using **bigger supers** (up to the max hive size)
  - Buying **better yards**
- Yards differ in **forage/pollen yield** (honey rate) and **risk factors** (pests, weather, etc.).
- **Backyard:** low limits, no travel time (§3.7).

### 4.5 Honey House Processing

1. **Uncap**: done **frame by frame** with an uncapping tool.
2. **Extract**: the extractor holds a **limited number of frames** per run. Honey drains into a
   **bucket**.
3. **Pour**: buckets are poured into the **bottling tank** (limited **capacity**, in honey volume).
4. **Bottle**: fill containers from the tank. **Label** them (labels are a consumable).

Upgrades for the bottling tank: **higher capacity**, and **heated tanks** for faster flow
(quicker bottling).

### 4.6 Machine Progression (general pattern)

Most machines follow the same upgrade arc:

1. **Manual**: the beekeeper must work the machine (e.g., hand-crank extractor)
2. **Manual, more capacity**
3. **Automated**: runs on its own once loaded, so the beekeeper can walk away
4. **Automated, more capacity / scale**

Automated machines still need **maintenance**, so there's always some action required.
**Higher-end machines need less maintenance.**

### 4.7 Containers & Products

**Containers** (each holds a different amount of honey):

| Tier  | Container                | Notes                          |
| ----- | ------------------------ | ------------------------------ |
| Start | **Honey bear**           | Starting product, lid included |
| Early | **Pint jar**             | Bigger volume                  |
| Early | **Quart jar**            | Bigger volume                  |
| Late  | **Gallon jug**           | Bulk                           |
| Late  | **Commercial contracts** | Large fulfilment orders        |

**Other products** (unlocked over time):

- _Mid:_ **beeswax products** from cappings (candles, lip balm)
- _Mid:_ **comb honey**: skips extraction, but the **comb is lost**, so the bees must **rebuild
  it**, which slows that super's next fill
- _Late:_ creamed honey (time-based processing), varietal honeys tied to specific yards, mead?

### 4.8 Starting Kit

- **1 hive** in the backyard: **1 brood box** + **1 shallow super**, with **1 queen/colony**
- **100 Beebucks** (starting money, a config value to tune in playtesting)
- **Honey bears** (lids included)
- **Some labels** (separate consumable)
- **Simple uncapping tool**
- **4-frame manual extractor**
- **Small basic bottling tank**
- Enough starting consumables to cover the two-week delivery lead time (§4.9)
- _(TBD: number of buckets, exact consumable counts, frame state of the starting super)_

### 4.9 Shop & Ordering

- The **Shop** (Sunday menu) sells **everything**: consumables, supers, bees/queens, equipment,
  machine upgrades, vehicles, and yards.
- **Currency:** **Beebucks**.
- **Delivery:** an order placed on Sunday arrives the **following Saturday, after the market
  closes**. The player is always **buying for the week after next**.
  - This is a deliberately strong planning pressure (pillar #3).
  - The delay opens up later mechanics such as **rush orders** and delivery upgrades.
- **Helping new players with the lead time:**
  - **Generous starting consumables** (§4.8)
  - **Gifts**: a character gifts extra "starter" supplies after the player's **first and second
    orders**, to keep them going while they learn how delivery works
  - The **Shop screen clearly shows when an order will arrive**
- **Bulk discounts**: possible, but **late game** only.

### 4.10 Stations by Scene (draft)

| Scene             | Stations / tasks                                                                                                 |
| ----------------- | ---------------------------------------------------------------------------------------------------------------- |
| **Bee Yard**      | Hives (inspect, add empty super, pull full super, treat/feed later), supply drop                                 |
| **Honey House**   | Super drop-off, uncapping station, extractor, buckets, bottling tank, bottling/labeling station, storage shelves |
| **Market**        | Display/stock table, customer counter, cash box                                                                  |
| **Shop** _(menu)_ | Everything for sale (§4.9)                                                                                       |

---

## 5. Open Questions

### Core loop & pacing

- [x] ~~How long is a game day?~~ → 2 min work days and market day, tuned separately (§3.4)
- [x] ~~How does a day end?~~ → on a timer, or early if the player chooses (§3.4)
- [x] ~~Is Sunday timed?~~ → No, an untimed menu (§3.4)
- [x] ~~Unfinished tasks at end of day?~~ → the task in progress finishes, the rest are dropped (§3.4)
- [x] ~~Reward for ending early?~~ → None at the start. Unused time → banked time is an upgrade (§3.3)
- [ ] Starting queue length, and how far it can be upgraded?
- [ ] Are there carry limits (e.g., the beekeeper can only carry N frames at once)?

### Scenes & travel

- [x] ~~Queue scope?~~ → Current scene only, plus a queueable Travel task (§3.1)
- [x] ~~Overworld?~~ → Destination picker, opened when a Travel task is reached (§3.7)
- [x] ~~Travel time?~~ → Per-destination distance, reduced by vehicle speed (§3.7)
- [x] ~~Starting location?~~ → The backyard: a small yard plus the Honey House, no travel time (§3.7)
- [ ] Are the backyard bee yard and the Honey House **one scene** or **two scenes** joined with zero
      travel time?
- [ ] Does the day clock pause while the player is choosing a destination in the Overworld?
- [ ] Does each work day start in the backyard, or wherever the player was the day before?
- [ ] Is a vehicle needed to reach the market on Saturday, or is it handled automatically?
- [ ] Does vehicle cargo apply only to supers, or also to supplies hauled out to yards (empty
      supers, feed, treatments)?
- [ ] How does the beekeeper get to the market on Saturday: automatically, or by choice?
- [ ] What does the Sleep scene show: a day summary, saving, upcoming events (weather)?
- [ ] How are new bee yards acquired, and do they differ in gameplay (forage type → honey
      varieties, size, risks) or only in looks?

### Market day (Saturday)

- [x] ~~Active or simulated?~~ → Active: fill customer orders (§3.5)
- [x] ~~Customer patience?~~ → Limited, with upgrades and items to help (§3.5)
- [x] ~~Multi-item orders?~~ → Yes (§3.5)
- [x] ~~Unsold stock?~~ → Carries over, up to storage limits (§3.5)
- [ ] Tips or bonuses for filling orders fast? Penalty (e.g., reputation) when customers leave?
- [ ] How does demand work: fixed, random, or reputation-based? Preferences per product?
- [ ] Can helpers run the stall?

### Supply day (Sunday)

- [x] ~~What can be ordered?~~ → Everything: consumables, supers, bees, equipment, vehicles, yards (§4.9)
- [x] ~~Delivery timing?~~ → Arrives Saturday after market. Bulk discounts late game (§4.9)
- [x] ~~Confirm lead time?~~ → Confirmed: Sunday orders arrive the following Saturday, with
      onboarding gifts + clear delivery messaging (§4.9)
- [ ] Who is the gifting character (mentor beekeeper, neighbour, family)? Could they also act as
      the tutorial guide?
- [ ] Is the Shop only on Sunday, or can upgrades be bought at other times?

### Hives, bees & yards

- [x] ~~Brood box?~~ → Yes, a separate purchase that includes a queen excluder, not harvested (§4.2)
- [x] ~~Requeening / aging?~~ → Requeening possible with some risk. Queens don't age (§4.2)
- [x] ~~Frame state?~~ → Bare foundation vs. drawn comb. Both sold in the Shop (§4.2)
- [x] ~~Inspections?~~ → Reveal problems. Over-inspecting hurts health (§4.2)
- [ ] How are bees added to a new hive: buy a "package/nuc + queen" and assign it to a brood box?
- [ ] Does the brood box come in sizes too, or is it a single item?
- [ ] Requeening risk: what can go wrong (rejection → queenless colony?) and how is it fixed?
- [ ] How much inspection is "too much": a cooldown per hive, or health damage per inspection?
- [ ] Do problems get worse when nobody looks (e.g., mites grow unseen until inspected)?
- [ ] How do the three super sizes differ in numbers (frames each, honey per frame, weight)?
- [ ] Are frames tracked per frame, or as a per-super state ("this super is drawn / bare")?

### Products, inventory & economy

- [x] ~~Product list?~~ → Honey bear → pint → quart → gallon/contracts, plus wax products,
      comb honey, creamed, varietals (§4.7)
- [ ] Do products need quality or variety (e.g., floral sources by season or bee yard)?
- [x] ~~Main currency?~~ → Beebucks (start with 100, tunable) (§4.8, §4.9)
- [ ] Any secondary currencies (reputation, royal jelly, etc.)?
- [ ] Storage: one shared limit, or separate limits per kind (supers, buckets, supplies, finished
      goods)? Where does stock physically live (Honey House shelves)?
- [ ] Are labels required to sell, or do unlabeled products just sell for less?
- [x] ~~Starting hive & money?~~ → 1 brood box + 1 shallow super + 1 queen, 100 Beebucks (§4.8)
- [ ] Starting consumable counts (bears, labels) and buckets: tune during playtesting

### Machines

- [x] ~~Manual vs. automated?~~ → Manual first, then capacity, then automation, then scale,
      with maintenance (§4.6)
- [ ] What does maintenance look like: a periodic task, a breakdown chance, or a slow drop in
      efficiency?
- [ ] Which machines follow the arc: extractor, uncapper, bottling, others?

### Helpers

- [ ] How are helpers acquired: hired with money, unlocked by milestones?
- [ ] Do helpers need wages, rest, or skill growth?
- [ ] What can a script do: plain loops only, or conditions like "if jars < 5, restock"?
- [ ] Is a helper assigned to one scene (e.g., Honey House helper), or can they travel?
- [ ] How many helpers at most, and do they get in each other's way?

### Pressures

- [ ] Which pressures come first, and in what order?
- [ ] How punishing should failure be (e.g., losing a colony)? Cozy vs. challenging tone?
- [ ] Are the pressures simulated (hidden state) or shown clearly to the player?

### Long-term progression

- [ ] Is there a prestige/reset layer (e.g., seasons or years, new apiary locations)?
- [ ] Is there an end goal or win state, or does it go on forever?
- [ ] How does the operation grow: more hives per yard, more yards, bigger Honey House?

### Banked time

- [ ] Starting cap (e.g., 1 hour? 8 hours?) and starting boost multiplier?
- [ ] Does boosting speed up helpers too, or only the beekeeper?
- [ ] Is banked time 1:1 with real time away, or scaled?

### Tech

- [x] ~~Camera?~~ → Single screen per scene at first, horizontal scrolling as it grows (§3.8)
- [x] ~~Orientation?~~ → Landscape (§3.8)
- [x] ~~Market scene?~~ → Its own scene (§3.7)
- [x] ~~Build step?~~ → Vite + TypeScript (§3.10)
- [x] ~~Save storage?~~ → `localStorage`, versioned JSON, no export/import yet (§3.10)
- [x] ~~Art?~~ → Coloured shapes + emoji for the prototype (§3.10)
- [x] ~~Config format?~~ → Typed TS module (§3.9)

---

## 6. Phased Roadmap (draft)

Phases are rough and will shift as open questions are answered.

### Phase 0 — Foundations

- Project skeleton: Vite + TypeScript (strict) + LittleJS
- Tooling: ESLint, Prettier, Vitest, and a single `npm run check` gate
- Project `CLAUDE.md` describing conventions and the check command for coding agents
- Folder layout that enforces the simulation/engine split (§3.11)
- GitHub Pages deployment
- PWA basics: manifest, icons, service worker
- Mouse + touch input, responsive landscape canvas
- Central tuning config module (all balance values come from it from the start)
- Scene manager (switch scenes, transition scenes)
- Save/load scaffold with versioning

### Phase 1 — Core Loop Prototype ("one work day")

- The **backyard**: a small bee yard and the **Honey House**, with no travel time
- Beekeeper movement + walking time inside a scene
- Task queue: add, run in order, cancel
- Simplest production chain: **hive fills super → carry super → uncap frames → manual extractor
  → bucket → bottling tank → honey bears (+ labels)**
- Starting kit only (§4.8). One super size is enough to begin with
- Basic HUD: queue display, inventory, day clock
- End-of-day rules: the task in progress finishes and the rest of the queue is dropped, plus End Day early
- **Goal:** answer "is queueing tasks fun?"

### Phase 2 — The Week

- Day/week cycle (Mon–Fri work, Sat market, Sun supply) with the **Sleep** transition
- **Market** scene: customers with multi-item orders and patience timers
- **Shop** menu: untimed Sunday purchasing, with **next-Saturday delivery** shown clearly on screen
- Onboarding gifts after the first and second orders
- Inventory carry-over + storage limits
- Limited supplies, so running out mid-week is possible
- First upgrades (queue length, task speed, walk speed, patience, storage)

### Phase 3 — Incremental Layer

- Upgrade tree
- Banked offline time + opt-in speed boost, with cap/multiplier upgrades
- "Unused day time → banked time" upgrade
- More hives, **additional bee yards**
- **Overworld** + Travel task + **Travel** transition with per-destination distances
- **Vehicles** (speed + super cargo capacity)
- Horizontal scrolling for scenes that grow wider than one screen

### Phase 4 — Helpers & Scripts

- Hire the first helper
- Script editor (repeating task lists) with a touch-friendly UI
- Script upgrades (length, task types)

### Phase 5 — Pressures

- First pressure added and balanced (likely seasons/nectar flow or colony health)
- Later pressures unlocked one at a time
- Upgrades and tools that counter each pressure

### Phase 6 — Content & Polish

- More products and recipes
- Art, animation, sound
- Tutorial / onboarding
- Balancing pass, friends & family playtest on phones

### Later / Maybe

- Prestige layer (new seasons/years/locations)
- Achievements
- Save export/import, cloud sync
