namespace Anime.DAL.Validation;

public static class Constraints
{
    // Anime
    public const int MIN_TITLE_LENGTH = 1; // X
    public const int MAX_TITLE_LENGTH = 255; // Backstabbed in a Backwater Dungeon: My Trusted Companions Tried to Kill Me, But Thanks to the Gift of an Unlimited Gacha I Got Level 9999 Friends and I'm Out for Revenge on My Former Party Members and the World
    public const string TITLE_REGEXP = @"^([A-Z][a-z]*)|([A-Z][a-z]*[:,]((( [A-Za-z][a-z]*)+)|(( [A-Za-z][a-z]*[:,])+ [A-Za-z][a-z]*)))$";
    public const string MIN_DATE = "1917-06-30";
    public const string MAX_DATE = "2026-03-05";
    public const int MIN_EPISODES = 1; // Your name - full-length movie in 1 episode
    // no need in max value limit for episodes
    // Sazae-san counts 7542 episodes and still keeps on going

    // Waifu
    public const int MIN_NAME_LENGTH = 3; // Aoi
    public const int MAX_NAME_LENGTH = 255; // Jugemu Jugemu Goko-no surikire Kaijarisuigyo-no Suigyomatsu Unraimatsu Furaimatsu Kuunerutokoro-ni Sumutokoru Yaburakoji-no burakoji Paipopaipo Paipo-no-shuringan Shuringan-no Gurindai Gurindai-no Ponpokopi-no Ponpokona-no Chosuke
    public const string NAME_REGEXP = @"^([A-Z][a-z]+(-[a-z]+)?)(\s+([A-Z][a-z]+(-[a-z]+)?))*$";
    public const int MIN_AGE = 1; // Spy Family - Anya Forger 2 years old
    public const int MAX_AGE = 1_000; // The Helpful Fox Senko-San 800 years old
}
