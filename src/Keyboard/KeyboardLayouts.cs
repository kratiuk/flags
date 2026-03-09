using System.Globalization;

internal sealed record KeyboardLayoutInfo(
    string DisplayName,
    string FlagCode,
    FlagDefinition Flag);

internal static class KeyboardLayouts
{
    private static readonly Dictionary<ushort, KeyboardLayoutInfo> Layouts = new()
    {
        [0x0409] = Create("English (United States)", "US", FlagCatalog.UnitedStates),
        [0x0809] = Create("English (United Kingdom)", "GB", FlagCatalog.UnitedKingdom),
        [0x0422] = Create("Ukrainian", "UA", FlagCatalog.Ukraine),
        [0x0407] = Create("German", "DE", FlagCatalog.Germany),
        [0x040C] = Create("French", "FR", FlagCatalog.France),
        [0x0C0A] = Create("Spanish", "ES", FlagCatalog.Spain),
        [0x0410] = Create("Italian", "IT", FlagCatalog.Italy),
        [0x0415] = Create("Polish", "PL", FlagCatalog.Poland),
        [0x0405] = Create("Czech", "CZ", FlagCatalog.Czechia),
        [0x0411] = Create("Japanese", "JP", FlagCatalog.Japan)
    };

    public static KeyboardLayoutInfo FromLanguageId(ushort languageId)
    {
        if (Layouts.TryGetValue(languageId, out var layout))
        {
            return layout;
        }

        return CreateUnknown(languageId);
    }

    public static IReadOnlyList<KeyboardLayoutInfo> GetSupportedLayouts() =>
        Layouts.Values
            .OrderBy(layout => layout.DisplayName, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    private static KeyboardLayoutInfo Create(string displayName, string flagCode, FlagDefinition flag) =>
        new(displayName, flagCode, flag);

    private static KeyboardLayoutInfo CreateUnknown(ushort languageId)
    {
        var languageCode = GetLanguageCode(languageId);
        return new KeyboardLayoutInfo($"Unknown ({languageId:X4})", languageCode, FlagCatalog.Code(languageCode));
    }

    private static string GetLanguageCode(ushort languageId)
    {
        if (languageId == 0)
        {
            return "UN";
        }

        try
        {
            var code = CultureInfo.GetCultureInfo(languageId).TwoLetterISOLanguageName.ToUpperInvariant();
            return code.Length == 2 ? code : "UN";
        }
        catch (CultureNotFoundException)
        {
            return "UN";
        }
    }
}
