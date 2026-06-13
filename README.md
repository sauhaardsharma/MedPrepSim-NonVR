# MedPrepSim — Medical Instrument Preparation Simulator (Non-VR)

A Unity-based interactive simulation of a surgical instrument preparation stage. The user identifies and places medical instruments into their designated slots on an instrument tray.

---

## Unity Version
Unity 6000.3.9f1 LTS

---

## Assets Used
- Medical Room Environment — [Sketchfab]
- Surgical Instrument Tray — [Sketchfab]
- Scalpel Model — [Sketchfab]
- Syringe Model — [Sketchfab]
- Forceps Model — [Sketchfab]
- Audio SFX — [Pixabay]

---

## Features Implemented

### Core Simulation
- Medical preparation room environment with surgical table and instrument tray
- 3 instruments: Scalpel, Syringe, Forceps
- Semi-transparent ghost models at each placement slot as visual guides
- Instrument selection via mouse click with yellow highlight feedback
- Instrument info panel showing name, description, and expected slot on selection
- Mouse drag-and-drop instrument movement
- Trigger-based slot detection for accurate placement

### Placement Feedback
- Ghost slot turns Green when correct instrument is in range
- Ghost slot turns Red when wrong instrument is in range
- Correct placement snaps instrument to slot, hides ghost, disables interaction
- Wrong placement returns instrument to original tray position
- Success and error feedback messages with color coding (green/red)
- Progress counter: 0/3 → 1/3 → 2/3 → 3/3 Instruments Placed

### Completion
- Completion panel shown when all 3 instruments placed
- Progress and feedback panels hidden on completion
- Final message: Procedure Preparation Complete

### Reset System
- Reset button restores all instruments to original tray positions
- Restores original rotations
- Re-enables all instrument interactions
- Restores all ghost slot objects
- Clears all progress, highlights, and messages

### Lighting Modes
- Mode A: Full room illumination with balanced lighting and shadows
- Mode B: Darker environment with spotlight focused on instrument tray
- Toggle button switches between modes at any point during simulation

### Navigation
- WASD camera movement
- Right mouse button hold for camera rotation
- Character Controller with collision preventing player from leaving the room

### UI (World Space)
- Instruction Panel
- Instrument Info Panel
- Progress Panel
- Feedback Panel
- Completion Panel
- Controls Panel (Reset, Lighting Toggle, Exit)

### Audio
- Correct placement sound
- Wrong placement sound
- Completion sound

---

## How To Run
1. Download the `Build` folder
2. Run `MedPrepSim.exe`
3. Use WASD to move, Right Mouse to look around
4. Left click and drag instruments to their matching ghost slots

---

## Time Taken
10 hrs

---

## Known Limitations
- VR mode not included in this build (see MedPrepSim-VR repository)
- Ghost color feedback relies on trigger colliders — very small or fast movements may occasionally miss trigger detection
- No audio for instrument selection or drag
