// Copyright 2026 The Casdoor Authors. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Casdoor.Client;

public partial class CasdoorClient
{
    /// <summary>
    ///     Starts setting up the MFA of the user, the data of the response contains the secret and the recovery codes.
    /// </summary>
    public virtual Task<CasdoorResponse?> InitiateMfaAsync(string owner, string mfaType, string name,
        CancellationToken cancellationToken = default) =>
        PostFormAsync(_options.GetActionUrl("mfa/setup/initiate"), Form(("owner", owner), ("mfaType", mfaType), ("name", name)), cancellationToken);

    public virtual Task<CasdoorResponse?> VerifyMfaAsync(string owner, string mfaType, string name, string secret, string passcode,
        CancellationToken cancellationToken = default) =>
        PostFormAsync(_options.GetActionUrl("mfa/setup/verify"),
            Form(("owner", owner), ("mfaType", mfaType), ("name", name), ("secret", secret), ("passcode", passcode)), cancellationToken);

    public virtual Task<CasdoorResponse?> EnableMfaAsync(string owner, string mfaType, string name, string secret, string recoveryCode,
        CancellationToken cancellationToken = default) =>
        PostFormAsync(_options.GetActionUrl("mfa/setup/enable"),
            Form(("owner", owner), ("mfaType", mfaType), ("name", name), ("secret", secret), ("recoveryCode", recoveryCode)), cancellationToken);

    public virtual Task<CasdoorResponse?> SetPreferredMfaAsync(string owner, string mfaType, string name, string secret = "",
        CancellationToken cancellationToken = default) =>
        PostFormAsync(_options.GetActionUrl("set-preferred-mfa"),
            Form(("owner", owner), ("mfaType", mfaType), ("name", name), ("secret", secret)), cancellationToken);

    /// <summary>
    ///     Deletes all the MFA settings of the user.
    /// </summary>
    public virtual Task<CasdoorResponse?> DeleteMfaAsync(string owner, string name, CancellationToken cancellationToken = default)
    {
        var queryMap = new QueryMapBuilder().Add("owner", owner).Add("name", name).QueryMap;
        return PostAsJsonAsync(_options.GetActionUrl("delete-mfa", queryMap), string.Empty, cancellationToken);
    }

    private static List<KeyValuePair<string, string?>> Form(params (string key, string? value)[] fields) =>
        fields.Select(field => new KeyValuePair<string, string?>(field.key, field.value)).ToList();
}
