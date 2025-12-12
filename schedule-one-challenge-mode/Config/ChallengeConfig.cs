namespace challenge_mode
{
    public static class ChallengeConfig
    {
        public const float DRUG_AFFINITY_MAX_IMPACT = 0.3f;
        
        public const float EFFECT_MATCH_NONE = 0.0f;
        public const float EFFECT_MATCH_ONE = 0.10f;
        public const float EFFECT_MATCH_TWO = 0.25f;
        public const float EFFECT_MATCH_THREE = 0.45f;
        
        public const float ENJOYMENT_CRITICAL_LOW = 0.20f;
        public const float ENJOYMENT_LOW = 0.35f;
        public const float ENJOYMENT_MEDIUM = 0.55f;
        
        public const float MULTIPLIER_CRITICAL_LOW = 0.3f;
        public const float MULTIPLIER_LOW = 0.5f;
        public const float MULTIPLIER_MEDIUM = 0.75f;
        public const float MULTIPLIER_HIGH = 1.0f;
        
        public const float COUNTEROFFER_MIN_ENJOYMENT = 0.4f;
        public const float COUNTEROFFER_MEDIOCRE_THRESHOLD = 0.6f;
        public const float COUNTEROFFER_MAX_REJECT_CHANCE = 0.5f;
        
        public const float MIN_APPEAL_FOR_REQUEST = 0.6f;

        public const float MIN_APPEAL_FOR_SUCCESS = 0.3f;
        public const int MESSAGE_COOLDOWN_DAYS = 3;
        public const int MIN_FAILED_REQUESTS_BEFORE_MESSAGE = 2;
        public const bool ENABLE_PRODUCT_REQUEST_MESSAGES = true;

        public static float GetSuccessMultiplier(float enjoyment)
        {
            if (enjoyment < ENJOYMENT_CRITICAL_LOW)
                return MULTIPLIER_CRITICAL_LOW;
            
            if (enjoyment < ENJOYMENT_LOW)
                return MULTIPLIER_LOW;
            
            if (enjoyment < ENJOYMENT_MEDIUM)
                return MULTIPLIER_MEDIUM;
            
            return MULTIPLIER_HIGH;
        }

        public static float GetStandardsMultiplier(int standardsValue)
        {
            return standardsValue switch
            {
                0 => 1.3f,
                1 => 1.15f,
                2 => 1.0f,
                3 => 0.85f,
                4 => 0.7f,
                _ => 1.0f
            };
        }
    }
}

