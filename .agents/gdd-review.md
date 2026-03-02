# GDD Review - Bus Shift
**Reviewer Role:** Senior Game Developer (10+ years)  
**Date:** 2026-03-01  
**Status:** Detailed Analysis & Recommendations

---

## EXECUTIVE SUMMARY

**Current State:** GDD is conceptually strong with excellent narrative/horror theming, but technically incomplete for production.

**Overall Score:** 7/10
- ✅ Excellent horror atmosphere and narrative design
- ✅ Clear core loop (drive → manage sanity → avoid ghosts)
- ⚠️ **CRITICAL:** Missing engine/platform specification
- ⚠️ **CRITICAL:** Performance targets undefined
- ⚠️ **CRITICAL:** Multiplayer requirements unclear
- ⚠️ Control scheme needs latency testing
- ⚠️ Sanity/tension math incomplete

**Recommendation:** GDD is solid foundation. Proceed to **Technical Spec Phase** with focus on:
1. Engine selection (Unity/Unreal/Godot)
2. Performance targets & profiling strategy
3. State machine architecture for ghost AI
4. Physics/driving mechanics prototype

---

## SECTION-BY-SECTION ANALYSIS

### 1. CONCEPT & VISION ✅
**Quality: 8/10**

**Strengths:**
- Unique premise: psychological horror in school bus (FNAF-adjacent but original)
- 5-day narrative arc with clear escalation
- Three distinct endings tied to player sanity
- Strong atmosphere building (Day 1 introduction is excellent)

**Issues:**
- Genre label slightly inaccurate: should be **"Psychological Horror / Survival Horror"** (not combat-focused)
- "Terror de sobrevivência" is good, but clarify: **No combat. Only evasion/avoidance.**

**Recommendation:**
```
Genre: Psychological Horror / Survival Simulator
(Not "survival action" - player cannot fight. Only observe/avoid/manage sanity)
```

---

### 2. CORE MECHANICS ⚠️
**Quality: 6/10**

#### A. DRIVING MECHANICS
**Current State:**
- WASD controls
- Realistic bus physics implied but NOT specified
- No turn radius data, no weight simulation

**CRITICAL ISSUES:**
1. **No Physics Specification:**
   - What engine will handle bus physics?
   - Realistic (heavy vehicle + inertia) or arcade (responsive)?
   - **Recommendation:** "Realistic-lite" - heavier than cars but responsive enough for 1st-person tension

2. **Input Lag Risk:**
   - School bus physics = delay = player frustration
   - Need to test: can player see ghosts + drive smoothly + manage sanity?
   - **Recommendation:** Target <50ms input latency, smooth acceleration curves

3. **Route Design Impact:**
   - Document first route: distance, turns, obstacles
   - **Question:** Can player stall? Crash? Game Over or lose sanity?

**GDD Gap:** Missing turning system detail. Is it:
- Free-look + directed driving? (60° wheel turn = 15° vehicle turn)
- Or arcade steering?

**Recommendation:**
```
DRIVING SYSTEM SPEC NEEDED:
- Physics engine: (Unity: Rigidbody.drag=0.3, mass=5000kg)
- Max speed: 60 km/h (17 m/s)
- Steering response: 0.3s to max turn
- Acceleration: 0-60 in 8 seconds
- Test: Play while managing ghost + sanity simultaneously
```

---

#### B. BUS STOP INTERACTIONS
**Current:** Press 'T' to board/deboard children

**Issues:**
1. **No Animation Timing:** How long to deboard 10 children?
   - **Risk:** Dead time = boring gameplay
   - **Recommendation:** 3-5 second deboard, player must manage using camera/retrovisor during

2. **No Detection System:** What counts as "near" bus stop?
   - Radius check? Waypoint trigger?
   - **Specification needed**

3. **No NPC Behavior:** Do children walk to seats? Instant spawn?
   - **Recommendation:** Simple walk animation, 2-3 seconds per child, randomize seating (prevent pattern recognition)

**Recommendation:**
```
BOARDING SPEC:
- Detection: 10m radius from waypoint
- Deboard animation: 0.5s per child (5-10 children = 2.5-5s total)
- Children spawn in random seats (avoid predictable patterns)
- Player can use this time to check retrovisor/camera
```

---

#### C. AUXILIARY OBJECTS ⚠️
**Current Mechanics:**
- Map (TAB) - View only
- Rearview Mirror (Hold E) - View only
- Security Camera (C) - Manual static reduction (15s cooldown)
- Headlights (F) - 15s duration, 20s cooldown
- Wristwatch (Space) - View time/sanity, 20s cooldown

**CRITICAL ANALYSIS:**

**Positive:**
- Cooldown system prevents spam
- Thematic (tool-based, not inventory)
- Good risk/reward: looking = less focus on driving

**PROBLEMS:**

1. **Map Balance Issue:**
   - Current: TAB closes view (implies full-screen map)
   - **Problem:** Breaks immersion, too much information at once
   - **Better Solution:** Mini-map in corner, TAB = toggle on/off
   - OR: Paper map player must glance at (3-5 second window)

2. **Headlight Mechanic Undefined:**
   - Does it have uses? Or just decorative?
   - Ghosts avoid light? Need clarification
   - **Recommendation:** Remove OR make impactful (e.g., ghosts weaker in headlight FOV)

3. **Wristwatch Cooldown Too Long:**
   - 20s to check time seems long in horror game
   - **Recommendation:** 8-10s (allows ~2x per day period)

4. **Camera Static System:**
   - Current: "Tap C to reduce static for 5s, 15s cooldown"
   - **Question:** Static reduces by how much? 50%? 100%?
   - **Recommendation:** Be explicit (e.g., "Static becomes 50% less, revealing 1 ghost clearly")

**Recommendation:**
```
REVISED AUXILIARY OBJECT SPEC:

Map (TAB):
- Toggle mini-map corner (always on) OR
- Hold to view full-screen (closes on release)
- Decision needed based on playtest

Camera (C):
- Static reduces to 30% for 5s (reveals ghost silhouettes)
- Cooldown: 10s (more frequent use, higher tension)

Headlights (F):
- Ghosts take -15% damage when in headlight beam
- Duration: 20s, Cooldown: 30s

Wristwatch (Space):
- Shows current time + sanity %
- Cooldown: 8s (allows frequent checks without spam)
```

---

#### D. INTERVENTION OBJECTS ⚠️
**Current:**
- Microphone (Q hold 3s): Block Child 1 attacks, 20s cooldown
- Panel Lock (Shift): Block Child 2 attacks, 15s cooldown
- Radio (R): Block Child 3 attacks, 25s cooldown

**CRITICAL ISSUES:**

1. **Feedback Problem:** 
   - When does player know attempt worked?
   - **Recommendation:** Add visual/audio confirmation (HUD flash, sound effect)

2. **Effectiveness Question:**
   - "Block attacks" - does this prevent sanity damage?
   - Or just prevent dangerous action (door opening)?
   - **Clarification NEEDED**

3. **Cooldown Design:**
   - Why different timers (15/20/25s)?
   - Players won't memorize tiny differences
   - **Recommendation:** All 20s, or vary more (15s / 20s / 30s for strategy)

4. **Microphone Hold Mechanic:**
   - Hold Q for 3s - can player steer during?
   - **If Yes:** Easy (no trade-off)
   - **If No:** Great tension (must stop paying attention to road)
   - **Recommendation:** Clarify this is CRITICAL mechanic decision

5. **Skill Floor Issue:**
   - Players may not discover these mechanics
   - **Recommendation:** Tutorial/on-screen prompts (especially Day 1)

**Recommendation:**
```
INTERVENTION SYSTEM SPEC:

Microphone (Q hold 3s):
- Effect: Child 1 stops moving for 5 seconds (frozen state)
- Feedback: BEEP sound + HUD highlight "Child 1: CONTAINED"
- Cooldown: 20s (shown via UI)
- Side-effect: Requires 1 hand off wheel (risk/reward)

Panel Lock (SHIFT press):
- Effect: Child 2 cannot interact with controls for 5s
- Feedback: CLICK sound + lights on panel flash
- Cooldown: 20s
- Side-effect: Instant (no holding time)

Radio (R toggle):
- Effect: Child 3 confused, attack delayed 7s
- Feedback: Music starts playing + HUD "RADIO: ON"
- Cooldown: 30s (longest, hardest effect)
- Side-effect: Audio masking (harder to hear other cues)

UI REQUIREMENT:
- Show remaining cooldown for each option
- Show which child is currently targeting player
```

---

### 3. GHOST AI SYSTEM ⚠️
**Quality: 5/10**

**Current State:**
- 3 children, each with 1 attack behavior
- Child 1: "changing seats" (mimics risk behavior)
- Child 2: Panel lock attack (mimics control interference)
- Child 3: Combination of 1+2

**MASSIVE GAP:** AI STATE MACHINE UNDEFINED

**Missing:**
1. **State Diagram:**
   - Spawn → Idle → Alert → Attack → Contained → Reset
   - No clarity on transitions
   - When does each child spawn? Day 1? Day 3?

2. **Behavior Tree:**
   - How do ghosts detect player looking at them?
   - Does Child 1 always laugh when spawning?
   - What triggers "stop moving when observed" for Child 1?

3. **Difficulty Curve:**
   - Day 1: Solo Child 1?
   - Day 2: Child 1 + Child 2?
   - Day 3: All 3?
   - **SPEC MISSING**

4. **Performance Risk:**
   - 3 ghosts with active AI = CPU load
   - Need LOD (Level of Detail) for ghost animation
   - Recommend: LOD 0 (close = full anim), LOD 1 (far = simplified)

**Recommendation:**
```
GHOST AI STATE MACHINE SPEC NEEDED:

Child 1 Configuration:
├─ Spawn: (Day 1, random seat)
├─ Idle: (wail softly, shift seat occasionally)
├─ Alert: (Player looks at camera for 3+ seconds)
├─ Attack: (Laugh + move seats rapidly + seek door)
├─ Contained: (Microphone used, frozen 5s)
└─ Reset: (5s after contained, resume attack)
   └─ [Escalation] After 3x contained attempts, Child 1 "smarter"
       (Delays attack, unpredictable movement)

Recommendation: Day 1 = Child 1 only (learning), Day 2+ = All 3 (stress)
```

---

### 4. SANITY SYSTEM ⚠️
**Quality: 4/10**

**Current State:**
- Table shows starting values: 5-35% depending on day/time
- Loss on seeing ghosts: -15 to -30%
- Recovery: "reset next day"
- Game Over: Sanity = 0%

**CRITICAL GAPS:**

1. **Sanity Loss Formula Undefined:**
   - Is it linear? Exponential?
   - Does 1 ghost cause -15%? All ghosts -15% each?
   - Does ignored ghost = higher loss?
   - **Math needed**

2. **Recovery Unclear:**
   - "Reset next day" means full recovery?
   - Or partial (reduces from Day 2 50% to Day 3 minimum 15%)?
   - **Recommendation:** NO full reset (prevents resets mechanic)
   - Instead: Final sanity value becomes start value for next day

3. **Threshold Effects Undefined:**
   - At 50% sanity: What changes?
   - At 25%: What changes?
   - At 10%: Game over dialogue?
   - **Spec needed**

4. **Visual Feedback Issue:**
   - GDD mentions "distortions" but no detail
   - Vignette at <30% is mentioned but vague
   - **Recommendation:** Clear spec:
     - 75-100%: Normal vision
     - 50-75%: Slight vignette (10% screen darkening)
     - 25-50%: Heavy vignette (30%) + slight desaturation
     - 0-25%: Red tint + visual distortion + hear heartbeat audio

5. **Ceiling Exploit:**
   - If sanity resets, can players "farm" lower difficulty?
   - **Recommendation:** GDD says "some carryover to next day" - enforce this

**Recommendation:**
```
SANITY SYSTEM FORMULA:

Starting Sanity by Day/Period:
Day 1 AM: 95%, PM: 90%
Day 2 AM: 90%, PM: 85%
Day 3 AM: 85%, PM: 80%
Day 4 AM: 75%, PM: 70%
Day 5 AM: 70%, PM: 65%

Loss Per Event:
- See Child 1 attacking: -10%
- Ignored Child 1 (attack succeeded): -20%
- Ignored Child 2: -25%
- Ignored Child 3: -30%
- Jumpscare (unexpected manifest): -15%

Recovery Per Day:
- Survive day with sanity >50%: -0% (no recovery)
- Survive day with sanity 25-50%: +5% (partial recovery)
- Survive day with sanity <25%: +10% (greater recovery)

Visual Effects Thresholds:
- >75%: Normal
- 50-75%: Vignette start (5% darkening)
- 25-50%: Heavy vignette (25%) + desaturate (20%)
- <25%: Red tint + heartbeat audio + visual glitches
- =0%: Game Over cutscene (transition to BAD END)
```

---

### 5. PROGRESSION SYSTEM ⚠️
**Quality: 6/10**

**Current:**
- 5 days = 5 level unlocks
- Endings gated by sanity thresholds
- No New Game+ or meta progression

**Strengths:**
- Simple, clear progression
- Sanity gating endings is smart (replay incentive)

**Issues:**

1. **Difficulty Scaling:**
   - GDD shows tension values but no NPC/spawn scaling
   - **Recommendation:** Add spawning logic:
     ```
     Day 1: Child 1 only
     Day 2: Child 1 + Child 2 (50% spawn chance each)
     Day 3: All 3 children (each has spawn chance)
     Day 4-5: All 3 guaranteed + "smarter" AI (faster attacks, harder counters)
     ```

2. **No Skill-Based Difficulty:**
   - All players see same ghosts on same days
   - No "easy/hard modes"
   - **This is intentional** = good design for horror

3. **Ending Balance Issue:**
   - Good End: >50% sanity (requires excellent play)
   - Bad End: <20% sanity (easy to achieve)
   - Medium End: 20-50% (wide range)
   - **Recommendation:** Retune:
     - Good End: >70% (very difficult)
     - Medium End: 40-70% (balanced challenge)
     - Bad End: <40% (struggle acceptable)

---

### 6. NARRATIVE & STORYTELLING ✅
**Quality: 8.5/10**

**Strengths:**
- Excellent Day 1 introduction (atmospheric, sets tone)
- Clear character establishment (Diretor Siqueira, mysterious bus)
- Good pacing (calm → jumpcare → revelation)
- Three endings with character contrast (cold/traumatized/cycled)

**Issues:**

1. **Days 2-5 Narrative Undefined:**
   - GDD only has Day 1 + Endings
   - **Gap:** What happens Days 2-4?
   - Need cutscenes/dialogue/story beats

2. **NPC Dialogue:**
   - Children are silent?
   - Other NPCs (teachers, parents)?
   - **Recommendation:** Minimal voice acting (budget-friendly):
     - Diretor Siqueira: 1 scene (opening)
     - Children NPCs: mumbles/ambient chatter
     - Player monologue: key story moments

3. **Lore Coherence:**
   - Accident happened "years ago"
   - How many? Why only 3 ghosts?
   - Previous motorist: dead or missing?
   - **Recommendation:** Define exact timeline in Technical Spec

4. **Ending Cinematography:**
   - Good End: Strong (walking away in daylight)
   - Medium End: Strong (paranoia reversal)
   - Bad End: Excellent (cycle repetition)
   - **No changes needed**

---

### 7. ART DIRECTION ✅
**Quality: 7/10**

**Current:**
- Low Poly visual style (excellent for horror + performance)
- "Worn-down" bus aesthetic
- Day/night lighting transitions

**Strengths:**
- Low poly = lower art cost + good indie aesthetic
- Horror + low poly is proven (see: Night in the Woods, Oxenfree)

**Specification Gaps:**

1. **Poly Count Targets Missing:**
   - Bus: 2000-3000 polys (mentioned in BACKLOG but not GDD)
   - Environment: "optimized but not specified"
   - **Recommendation:** Add to GDD:
     ```
     Poly Budget:
     - Bus: 3000 (exterior + interior)
     - NPCs: 400-600 each
     - Environment: 300-500 per building
     - Ghosts: 800-1200 (higher detail)
     ```

2. **Lighting Model:**
   - Day/night mentioned, no technical spec
   - **Recommendation:** Specify:
     - Baked lightmaps (performance)
     - Dynamic point lights for car headlights
     - Real-time shadows on ghosts (per-object)

3. **Animation Quality:**
   - No animation spec
   - **Recommendation:** (From GDD review):
     - NPC children: walk, sit, idle, deboard (simple)
     - Ghosts: float/glide, laugh animation, sudden stop
     - Player hands: steering, reaching for controls (visible in lower screen)

---

### 8. AUDIO DESIGN ⚠️
**Quality: 6/10**

**Current:**
- "Dynamic audio system" mentioned in BACKLOG
- No GDD audio spec
- Implies: tension music + ambient + SFX

**Missing Specifications:**

1. **Music Layering:**
   - How does music respond to sanity?
   - Example: Base layer plays always, second layer at <50% sanity?
   - **Recommendation:** 3-layer adaptive music:
     ```
     Layer 1: Ambient base (always on)
     Layer 2: Tension (triggers at 50% sanity or ghost alert)
     Layer 3: Panic (triggers at 25% sanity or 2+ ghosts attacking)
     Crossfade time: 2 seconds per layer
     ```

2. **SFX Budget:**
   - No mention of voice acting for ghosts (whispers, laughs)
   - Player radio should have AM static + possible voice?
   - **Recommendation:** Add:
     - Ghost whispers: 2-3 audio files (randomized)
     - Radio static: 1 base + 3 variations
     - Bus ambience: engine, seats, doors

3. **Audio Masking Risk:**
   - If player uses radio (intervention #3), does it properly mask other audio?
   - Players need to hear attacks still?
   - **Clarification needed**

---

### 9. TECHNICAL SPECIFICATION ❌
**Quality: 0/10 - CRITICAL GAP**

**Missing Everything:**

1. **Game Engine:**
   - ❌ No engine specified (Unity? Unreal? Godot?)
   - **Recommendation:** Choose Unity (lower barrier, good 3D horror)
   - **Decision driver:** Team skill, budget, timeline

2. **Platform Support:**
   - PC Windows only?
   - Mac/Linux/Console?
   - VR (1st person is VR-native)?
   - **Recommendation:** PC Windows only (lower scope, no console ports planned)

3. **Performance Targets:**
   - ❌ No target FPS (assume 60 FPS minimum)
   - ❌ No resolution targets (1080p? 4K support?)
   - ❌ No hardware specs (GTX 1660? RTX 4070?)
   - **Recommendation:**
     ```
     Target Performance:
     - Resolution: 1920x1080 @ 60 FPS minimum
     - Target GPU: GTX 1660 (2020-2022 era mid-range)
     - RAM: 8 GB minimum
     - CPU: Ryzen 5 3600 / Intel i5-10400
     - Storage: 4-6 GB SSD
     ```

4. **Multiplayer/Online:**
   - GDD implies single-player only
   - No multiplayer content
   - **Recommendation:** Confirm single-player, no multiplayer planned

5. **Camera System:**
   - First-person, but head physics?
   - Does steering wheel visible?
   - **Specification needed:**
     ```
     Camera Spec:
     - FOV: 90 degrees (immersive but not nauseating)
     - Head bob: Subtle (0.1 amplitude) during accel/decel
     - Visible element: Hands on wheel (lower screen)
     - No head tracking (traditional mouse look)
     ```

---

## RISK ANALYSIS

### HIGH RISK 🔴
1. **Driving + Ghost Management Balance:**
   - If driving is too hard, players can't manage ghosts
   - If driving is too easy, no tension
   - **Mitigation:** Heavy playtesting (10+ testers per build)

2. **Repetition Risk:**
   - 5 days × 2 periods = 10 loops
   - Can feel repetitive if ghost behavior is too predictable
   - **Mitigation:** Randomize ghost spawn locations/timing (do NOT expose randomness to player)

3. **Difficulty Ceiling:**
   - Day 5 might be unplayable for some players
   - Could lead to rage quits
   - **Mitigation:** Ensure Medium End is achievable (40-70% sanity is not punishing)

### MEDIUM RISK 🟡
1. **Skill Floor:** Players may not discover intervention mechanics
2. **Audio Masking:** Radio mechanic might obscure important cues
3. **Motion Sickness:** First-person driving + rotation = nausea risk

### LOW RISK 🟢
1. **Narrative appeal:** Story is strong
2. **Budget:** Low-poly + single level = manageable scope
3. **Performance:** Should be achievable on laptops

---

## RECOMMENDATIONS SUMMARY

### MUST DO (Before Production)
- [ ] **Specify game engine** (recommend: Unity 2022 LTS)
- [ ] **Define driving physics** (realistic-lite with responsive steering)
- [ ] **Create AI state diagrams** for all 3 ghosts
- [ ] **Define sanity formula** (exact loss values per ghost type)
- [ ] **Specify platform** (PC Windows, Mac optional, Console: NO)
- [ ] **Create camera spec** (FOV, head bob, hand visibility)

### SHOULD DO (Before Production)
- [ ] Define Days 2-5 narrative/cutscenes
- [ ] Specify audio layering system
- [ ] Create difficulty balancing sheet (ghost spawn rates per day)
- [ ] Define tutorial flow (teach mechanics without breaking immersion)

### NICE TO HAVE (During Production)
- [ ] Consider colorblind accessibility options
- [ ] Add difficulty settings (though single difficulty is fine for indie horror)
- [ ] Consider speedrun-friendly design (secret shortcuts?)

---

## FINAL SCORE CARD

| Category | Score | Notes |
|----------|-------|-------|
| **Core Concept** | 8.5/10 | Excellent premise, unique execution |
| **Mechanics** | 6/10 | Solid but incomplete specs |
| **Progression** | 6.5/10 | Clear but could use more variety |
| **Narrative** | 8.5/10 | Day 1 + Endings excellent; Days 2-4 missing |
| **Art Direction** | 7/10 | Low poly good choice; missing specs |
| **Audio Design** | 6/10 | Good intent; no specifications |
| **Technical Spec** | 0/10 | **CRITICAL GAP** - Must complete |
| **Playtesting Plan** | 0/10 | No mention of playtesting strategy |
| **Overall** | 6.2/10 | Solid foundation, needs technical polish |

---

## NEXT STEPS

**Phase 1: Technical Specification (2 weeks)**
1. Choose engine, platform, hardware targets
2. Create visual reference builds (camera, controls, physics)
3. Define exact AI state machines

**Phase 2: Prototype (4 weeks)**
1. Implement core driving + ghost behavior
2. Test control responsiveness
3. Playtest with 5+ testers

**Phase 3: Production (12-16 weeks)**
1. Full asset creation
2. Narrative implementation (D2-5)
3. Audio implementation
4. Optimization + polish

**Timeline Total:** ~20 weeks (5 months) for MVP

---

**Review Complete.** Ready for Production Technical Spec phase.
