namespace AnimeApplication.Domain.Constraints;

public static class AnimeConstraints
{
    public const int MIN_TITLE_LENGTH = 1; // X
    public const int MAX_TITLE_LENGTH = 255; // Backstabbed in a Backwater Dungeon: My Trusted Companions Tried to Kill Me, But Thanks to the Gift of an Unlimited Gacha I Got Level 9999 Friends and I'm Out for Revenge on My Former Party Members and the World
    public const string TITLE_REGEXP = @"^[A-Za-z]+( [A-Za-z]+)*([:,] ([A-Za-z]+)+)?$";
    public const string MIN_DATE = "1917-06-30";
    public const string MAX_DATE = "2026-03-05";
    public const int MIN_EPISODES = 1; // Your name - full-length movie in 1 episode
    // no need in max value limit for episodes
    // Sazae-san counts 7542 episodes and still keeps on going
}
