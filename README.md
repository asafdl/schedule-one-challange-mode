# Challenge Mode - Schedule I Mod

A difficulty mod for Schedule I that makes customers significantly more selective about their purchases based on drug type preferences and product properties.


This mod significantly increases difficulty by:
- Making it nearly impossible to sell products customers don't want
- Requiring you to stock diverse drug types to satisfy different customers
- Forcing strategic product matching based on customer preferences
- Making counter-offers much harder to negotiate

## Features

### 1. Enhanced Product Enjoyment System
Customers now have much stronger preferences based on:
- **Drug Type Affinity**: Up to ±30% enjoyment modifier based on how much they like/dislike specific drug types (Marijuana, Cocaine, Meth, etc.)
- **Property Matching**: Up to ±20% enjoyment modifier based on how well product effects match their preferred properties
- Replaces the vanilla system with more impactful preference weighting

### 2. Stricter Direct Sales (Handover)
Success rates for direct sales are drastically reduced when offering mismatched products:
- **<30% enjoyment**: 90% success rate reduction (0.1x multiplier)
- **30-50% enjoyment**: 70% success rate reduction (0.3x multiplier)
- **50-70% enjoyment**: 40% success rate reduction (0.6x multiplier)
- **>70% enjoyment**: No penalty (1.0x multiplier)

### 3. Tougher Counter-Offers
When customers request specific products via messages, counter-offering is now much harder:
- **Price limit reduced**: From 3x to 2x average spend
- **Minimum enjoyment requirement**: 40% enjoyment threshold (rejects below)
- **Random rejection chance**: 50% chance to reject counter-offers with 40-60% enjoyment

### 4. Drug Affinity Display
Customer handover screens now show their drug type preferences:
- Color-coded affinity indicators:
  - **Green [+++]**: High affinity (>0.6)
  - **Light Green [++]**: Medium-high affinity (0.2-0.6)
  - **Yellow [+]**: Neutral (-0.2 to 0.2)
  - **Orange [-]**: Low affinity (-0.6 to -0.2)
  - **Red [--]**: Strong dislike (<-0.6)

## Installation

### Prerequisites
- **Schedule I** - MONO version (not IL2CPP)
- **MelonLoader** - Must be installed in your game directory ([Installation Guide](https://melonwiki.xyz/#/?id=automated-installation))
- **.NET SDK 6.0+** - Required only for building from source ([Download](https://dotnet.microsoft.com/download))

### Option 1: Install Pre-built

1. Download the latest `challenge_mode.dll` from the [`dist/`](https://github.com/asafdl/schedule-one-challenge-mode/tree/master/dist) folder
2. Navigate to your Schedule I installation directory
3. Place the DLL in the `Mods` folder:
   ```
   Schedule I/
   └── Mods/
       └── challenge_mode.dll
   ```
4. Launch the game - MelonLoader will automatically load the mod

### Option 2: Build from Source

#### Initial Setup
1. **Clone the repository**
   ```bash
   git clone https://github.com/asafdl/schedule-one-challenge-mode.git
   cd schedule-one-challenge-mode
   ```

2. **Configure game path**
   
   Open `schedule-one-challenge-mode/schedule-one-challenge-mode.csproj` and update the game directory path:
   ```xml
   <PropertyGroup Condition="'$(Configuration)' == 'MONO'">
       <S1Dir>E:\SteamLibrary\steamapps\common\Schedule I</S1Dir>
   </PropertyGroup>
   ```
   
   Replace with your actual Schedule I installation path.

3. **Verify dependencies**
   
   Ensure the following exist in your game directory:
   - `Schedule I_Data/Managed/` - Contains game DLLs
   - `MelonLoader/` - MelonLoader installation

#### Build Commands

**Build the mod:**
```bash
./build.sh
```

**Run tests and simulations:**
```bash
./test.sh
```

The tests will show difficulty impact for various scenarios.

#### Development Workflow

**Quick development build:**
```bash
cd schedule-one-challenge-mode
dotnet build -c MONO
```
This will auto-launch the game for testing.

**Production build:**
```bash
./build.sh
```
This builds without launching the game and copies to `dist/`.

### Verify Installation

Launch Schedule I and check the MelonLoader console for:
```
[ChallengeMode] Challenge Mode initialized - success rates reduced by 70%
[ChallengeMode] Harmony patches applied successfully
```

## Configuration

### Difficulty Tuning

All difficulty settings are in `Patches/CustomerPatches.cs` under the `ChallengeConfig` class:

```csharp
public static class ChallengeConfig
{
    // Modify these values to adjust difficulty
    public const float DRUG_AFFINITY_MAX_IMPACT = 0.3f;      // Drug type preference impact
    public const float PROPERTY_MATCH_MAX_IMPACT = 0.4f;     // Effect matching impact
    public const float ENJOYMENT_CRITICAL_LOW = 0.3f;        // Harsh penalty threshold
    // ... more settings
}
```

After modifying, rebuild the mod with `dotnet build -c MONO`.

### Game Path

If your Schedule I installation is in a non-standard location, update the path in `schedule-one-challenge-mode.csproj`:

```xml
<S1Dir>YOUR_PATH_HERE\Schedule I</S1Dir>
```

Common locations:
- Steam (default): `C:\Program Files (x86)\Steam\steamapps\common\Schedule I`
- Steam (custom): Check Steam → Right-click Schedule I → Properties → Local Files → Browse

## Technical Details

### Patched Methods
- `Customer.GetProductEnjoyment()` - Modified enjoyment calculation
- `Customer.GetOfferSuccessChance()` - Added enjoyment-based multipliers
- `Customer.EvaluateCounteroffer()` - Stricter acceptance criteria
- `HandoverScreenDetailPanel.Open()` - Added drug affinity UI

### Dependencies
- HarmonyLib (for runtime patching)
- MelonLoader (mod loader)
- Schedule I game assemblies

## Compatibility

- **Game Version**: Tested on current Schedule I MONO build
- **Other Mods**: Should be compatible with most mods unless they also modify customer behavior
- **IL2CPP**: Not currently supported (MONO only)

## Known Issues

None reported. Please submit issues on the GitHub repository.

## Future Features (Planned)

### 1. Customer Feedback Messages
Add messaging system where customers periodically send messages when they can't find products they like in your current offerings.

### 2. Enhanced Addiction & Satisfaction Scaling
Rework the addiction and satisfaction systems so customers offer increasingly higher prices and product amounts as their addiction grows, with much steeper scaling than vanilla.

### 3. Dynamic Pricing System
Replace manual price configuration with customer-driven pricing based on deal success rates and demand. Prices adjust automatically based on market forces.

### 4. Increased Penalties
Implement harsher consequences for:
- Rejected deals (larger relationship penalties)
- Customers unable to find desired products
- Failed contract acceptances
- Unmet customer needs

### 5. Dynamic Customer Preferences
Make customer affinities and preferences change over time to prevent the game from becoming stale. Customers will gradually shift their drug type preferences and desired effects, requiring players to adapt their product offerings continuously.

## Release Process (for maintainers)

1. Make changes and test thoroughly
2. Run the build script:
   ```bash
   ./build.sh
   ```
3. The script will build and copy the DLL to `dist/`
4. Commit and push:
   ```bash
   git add dist/challenge_mode.dll
   git commit -m "Release: version description"
   git push origin master
   ```

Users can always download the latest build from the `dist/` folder.

## Contributing

We welcome contributions! Here's how you can help:

### Reporting Issues

Found a bug or have a suggestion? [Open an issue](https://github.com/asafdl/schedule-one-challenge-mode/issues) with:
- Clear description of the problem
- Steps to reproduce
- Expected vs actual behavior
- MelonLoader console logs (if applicable)

### Pull Requests

1. **Fork the repository**
2. **Create a feature branch**
   ```bash
   git checkout -b feature/your-feature-name
   ```
3. **Make your changes**
   - Follow existing code style
   - Add XML documentation for new methods
   - Update README if adding features
4. **Test thoroughly**
   - Build and test in-game
   - Verify no console errors
5. **Submit PR**
   - Describe what your changes do
   - Reference any related issues

## License

This project uses the Schedule I game assemblies for modding purposes only. The mod code itself is provided as-is for community use.
