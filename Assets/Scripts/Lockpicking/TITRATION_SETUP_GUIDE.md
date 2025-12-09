# Titration Minigame Setup Guide

## What I Created

### 1. **ElementButton.cs**
Attach this to each element icon button. When clicked, it adds that element to the beaker.

**Properties:**
- `elementSymbol`: Chemical symbol (e.g., "H", "O", "Na", "Cl")
- `elementCharge`: Charge value (+1, -1, +2, -2, etc.)
- `elementName`: Optional display name
- `titrationManager`: Reference to the TitrationManager (auto-finds if not set)

### 2. **TitrationManager.cs**
The brain of the minigame. Tracks elements, calculates balance, handles win/loss.

**Key Settings:**
- `targetCharge`: What charge to balance to (usually 0)
- `timeToWin`: How long to maintain balance (default 3 seconds)
- `chargeDeviation`: Allowed margin of error (default 0 = exact)

**UI References:**
- `currentChargeText`: TextMeshPro showing current charge
- `elementsInBeakerText`: TextMeshPro showing elements added
- `timerText`: TextMeshPro showing balance timer
- `winPanel`/`losePanel`: UI panels to show on game end

### 3. **BalanceBeam.cs** (Enhanced)
Visual feedback - rotates or moves based on charge balance.

**Settings:**
- `useRotation`: true = rotate beam, false = slide horizontally
- `chargeRange`: How many charge units = full tilt (default 10)
- `smoothSpeed`: Animation smoothness
- `useColorFeedback`: Green when balanced, red when not

---

## Unity Setup Steps

### Step 1: Create the Scene Hierarchy
```
TitrationMinigame (Empty GameObject)
├── TitrationManager (Empty GameObject + TitrationManager.cs)
├── BeakerDisplay (Empty GameObject)
│   └── BalanceBeam (Sprite + BalanceBeam.cs)
├── UI Canvas
│   ├── ElementButtons (Empty)
│   │   ├── BoronButton (Button + Image + ElementButton.cs)
│   │   ├── OxygenButton (Button + Image + ElementButton.cs)
│   │   ├── NitrogenButton (Button + Image + ElementButton.cs)
│   │   ├── CarbonButton (Button + Image + ElementButton.cs)
│   │   └── HydrogenButton (Button + Image + ElementButton.cs)
│   ├── InfoPanel
│   │   ├── ChargeText (TextMeshProUGUI)
│   │   ├── BeakerText (TextMeshProUGUI)
│   │   └── TimerText (TextMeshProUGUI)
│   ├── WinPanel (Panel with "You Win!" text)
│   └── LosePanel (Panel with "Try Again" text)
```

### Step 2: Configure Each Element Button

For each element button (e.g., BoronButton):
1. Add **Button** component (already has it)
2. Add **ElementButton.cs** script
3. Set properties in Inspector:
   - **Element Symbol**: "B"
   - **Element Charge**: +3
   - **Element Name**: "Boron" (optional)
   - **Titration Manager**: Drag TitrationManager object here (or leave empty to auto-find)

Example configurations for your elements:
- **Boron (B³⁺)**: symbol = "B", charge = +3
- **Oxygen (O²⁻)**: symbol = "O", charge = -2
- **Nitrogen (N³⁻)**: symbol = "N", charge = -3
- **Carbon (C⁴⁺/C⁴⁻)**: symbol = "C", charge = +4 or -4 (you choose based on compound)
- **Hydrogen (H⁺)**: symbol = "H", charge = +1

Note: Carbon can have variable charges depending on the compound. For simplicity, you might use +4 for cations or create two separate buttons (C⁺⁴ and C⁻⁴) if needed.

### Step 3: Configure TitrationManager

1. Select the TitrationManager GameObject
2. In Inspector, set:
   - **Target Charge**: 0 (neutral)
   - **Time To Win**: 3 (seconds)
   - **Charge Deviation**: 0 (must be exact)
3. Drag UI elements:
   - **Current Charge Text**: ChargeText object
   - **Elements In Beaker Text**: BeakerText object
   - **Timer Text**: TimerText object
   - **Win Panel**: WinPanel object
   - **Lose Panel**: LosePanel object
   - **Balance Beam**: BalanceBeam object

### Step 4: Configure BalanceBeam

1. Create a sprite for the balance beam (a horizontal bar/beam)
2. Attach **BalanceBeam.cs** to it
3. Set in Inspector:
   - **Use Rotation**: ✓ (check for rotating beam)
   - **Charge Range**: 10 (adjust to taste)
   - **Beam Renderer**: Auto-assigned or drag SpriteRenderer
   - **Use Color Feedback**: ✓
   - **Balanced Color**: Green
   - **Unbalanced Color**: Red

---

## Example Game Flow

1. **Player clicks "B" button** → Adds B³⁺ (charge +3) → Total charge: +3 → Beam tilts right
2. **Player clicks "N" button** → Adds N³⁻ (charge -3) → Total charge: 0 → Beam centers, turns green
3. **Timer starts**: 0.0s / 3.0s
4. **After 3 seconds balanced** → Win panel appears!

---

## Optional Features

### Add an Undo Button
Create a button and hook it to `TitrationManager.RemoveLastElement()`

### Add a Reset Button
Create a button and hook it to `TitrationManager.ClearBeaker()`

### Use Events
In TitrationManager → Events section:
- **On Win**: Play sound, trigger animation, advance level
- **On Lose**: Show retry message
- **On Charge Changed**: Update particle effects, shake screen

---

## Example Titration Challenges

### Challenge 1: Simple Balance (Easy)
- **Starting charge**: 0
- **Goal**: Balance using Boron and Nitrogen
- **Available elements**: B (+3), N (-3)
- **Solution**: 1 Boron + 1 Nitrogen = 0 charge

### Challenge 2: Water Formation (Medium)
- **Starting charge**: 0
- **Goal**: Balance H₂O components
- **Available elements**: H (+1), O (-2)
- **Solution**: 2 Hydrogen + 1 Oxygen = 0 charge

### Challenge 3: Complex Compound (Hard)
- **Starting charge**: 0
- **Goal**: Balance using multiple elements
- **Available elements**: B (+3), O (-2), N (-3), C (+4), H (+1)
- **Solution**: Multiple valid combinations (e.g., 2B + 3O = 0, or 3C + 4N = 0, etc.)

---

## Troubleshooting

**Buttons don't respond:**
- Check EventSystem exists in scene (GameObject → UI → Event System)
- Verify Button component is on the same object as ElementButton.cs

**Balance beam doesn't move:**
- Check BalanceBeam reference in TitrationManager
- Verify BalanceBeam.cs is attached to the beam sprite

**Text doesn't update:**
- Make sure you're using TextMeshPro, not legacy Text
- Check references in TitrationManager Inspector

**Can't find TitrationManager:**
- Either drag manually in ElementButton Inspector
- Or ensure TitrationManager.cs is in the scene (it auto-finds)
