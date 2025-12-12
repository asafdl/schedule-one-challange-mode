using MelonLoader;
using challenge_mode.Managers;

[assembly: MelonInfo(typeof(challenge_mode.Core), "Challenge Mode", "1.0.0", "Dixie", null)]
[assembly: MelonGame("TVGS", "Schedule I")]

namespace challenge_mode
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Challenge Mode initialized");

            ProductRequestTracker.LoadData();
            LoggerInstance.Msg("Loaded product request tracking data");
        }

        public override void OnApplicationQuit()
        {
            ProductRequestTracker.SaveData();
            LoggerInstance.Msg("Saved product request tracking data");
        }
    }
}