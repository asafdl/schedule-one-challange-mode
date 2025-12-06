using Xunit;
using Xunit.Abstractions;
using challange_mode;

namespace challange_mode.Tests;

/// <summary>
/// Tests to ensure difficulty configuration is balanced
/// </summary>
public class ConfigBalanceTests
{
    private readonly ITestOutputHelper _output;

    public ConfigBalanceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void EffectMatchModifiers_AreBalanced()
    {
        Assert.True(ChallengeConfig.EFFECT_MATCH_NONE < 0);
        Assert.True(ChallengeConfig.EFFECT_MATCH_ONE <= 0);
        Assert.True(ChallengeConfig.EFFECT_MATCH_TWO > 0);
        Assert.True(ChallengeConfig.EFFECT_MATCH_THREE > ChallengeConfig.EFFECT_MATCH_TWO);

        float maxSwing = ChallengeConfig.EFFECT_MATCH_THREE - ChallengeConfig.EFFECT_MATCH_NONE;
        Assert.InRange(maxSwing, 0.5f, 0.8f);

        _output.WriteLine("Effect Match Modifiers:");
        _output.WriteLine($"  0 matches: {ChallengeConfig.EFFECT_MATCH_NONE:P0}");
        _output.WriteLine($"  1 match:   {ChallengeConfig.EFFECT_MATCH_ONE:P0}");
        _output.WriteLine($"  2 matches: {ChallengeConfig.EFFECT_MATCH_TWO:P0}");
        _output.WriteLine($"  3 matches: {ChallengeConfig.EFFECT_MATCH_THREE:P0}");
        _output.WriteLine($"  Total swing: {maxSwing:P0}");
    }

    [Fact]
    public void SuccessMultipliers_AreProgressive()
    {
        Assert.True(ChallengeConfig.MULTIPLIER_CRITICAL_LOW < ChallengeConfig.MULTIPLIER_LOW);
        Assert.True(ChallengeConfig.MULTIPLIER_LOW < ChallengeConfig.MULTIPLIER_MEDIUM);
        Assert.True(ChallengeConfig.MULTIPLIER_MEDIUM < ChallengeConfig.MULTIPLIER_HIGH);

        _output.WriteLine("Success Chance Multipliers:");
        _output.WriteLine($"  Critical Low (<{ChallengeConfig.ENJOYMENT_CRITICAL_LOW:P0}): {ChallengeConfig.MULTIPLIER_CRITICAL_LOW:P0}");
        _output.WriteLine($"  Low (<{ChallengeConfig.ENJOYMENT_LOW:P0}):          {ChallengeConfig.MULTIPLIER_LOW:P0}");
        _output.WriteLine($"  Medium (<{ChallengeConfig.ENJOYMENT_MEDIUM:P0}):       {ChallengeConfig.MULTIPLIER_MEDIUM:P0}");
        _output.WriteLine($"  High (≥{ChallengeConfig.ENJOYMENT_MEDIUM:P0}):        {ChallengeConfig.MULTIPLIER_HIGH:P0}");
    }

    [Fact]
    public void DrugAffinity_IsSecondaryFactor()
    {
        float maxImpact = ChallengeConfig.DRUG_AFFINITY_MAX_IMPACT;
        
        Assert.InRange(maxImpact, 0.2f, 0.35f);

        _output.WriteLine($"Drug Affinity Max Impact: ±{maxImpact:P0}");
    }
}

