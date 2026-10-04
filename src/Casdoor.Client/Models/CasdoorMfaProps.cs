using System.Text.Json.Serialization;

namespace Casdoor.Client;

public class CasdoorMfaProps
{
    [JsonPropertyName("enabled")]
    public bool? Enabled { get; set; }

    [JsonPropertyName("isPreferred")]
    public bool? IsPreferred { get; set; }

    [JsonPropertyName("mfaType")]
    public string? MfaType { get; set; }

    [JsonPropertyName("secret")]
    public string? Secret { get; set; }

    [JsonPropertyName("countryCode")]
    public string? CountryCode { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("recoveryCodes")]
    public string[]? RecoveryCodes { get; set; }

    [JsonPropertyName("mfaRememberInHours")]
    public int MfaRememberInHours { get; set; }
}
