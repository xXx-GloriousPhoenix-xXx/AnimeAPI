namespace AnimeApplication.Domain.Constraints;

public static class WaifuConstraints
{
    public const int MIN_NAME_LENGTH = 3; // Aoi
    public const int MAX_NAME_LENGTH = 255; // Jugemu Jugemu Goko-no surikire Kaijarisuigyo-no Suigyomatsu Unraimatsu Furaimatsu Kuunerutokoro-ni Sumutokoru Yaburakoji-no burakoji Paipopaipo Paipo-no-shuringan Shuringan-no Gurindai Gurindai-no Ponpokopi-no Ponpokona-no Chosuke
    public const string NAME_REGEXP = @"^([A-Z][a-z]+(-[a-z]+)?)(\s+([A-Z][a-z]+(-[a-z]+)?))*$";
    public const int MIN_AGE = 1; // Spy Family - Anya Forger 2 years old
    public const int MAX_AGE = 1_000; // The Helpful Fox Senko-San 800 years old
}
