using challange_mode;

namespace challange_mode.Tests;

public static class CustomerBehaviorHelpers
{
    public static float GetSuccessMultiplier(float enjoyment)
    {
        if (enjoyment < ChallengeConfig.ENJOYMENT_CRITICAL_LOW)
            return ChallengeConfig.MULTIPLIER_CRITICAL_LOW;
        
        if (enjoyment < ChallengeConfig.ENJOYMENT_LOW)
            return ChallengeConfig.MULTIPLIER_LOW;
        
        if (enjoyment < ChallengeConfig.ENJOYMENT_MEDIUM)
            return ChallengeConfig.MULTIPLIER_MEDIUM;
        
        return ChallengeConfig.MULTIPLIER_HIGH;
    }
}

