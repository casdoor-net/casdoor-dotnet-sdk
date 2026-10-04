// Copyright 2022 The Casdoor Authors. All Rights Reserved.
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
    public virtual Task<CasdoorResponse?> SendSmsAsync(string content, IEnumerable<string> receivers, CancellationToken cancellationToken = default)
    {
        CasdoorSmsForm form = new()
        {
            OrganizationId = string.Concat("admin/", _options.OrganizationName),
            Content = content,
            Receivers = receivers as string[] ?? receivers.ToArray(),
        };
        string url = _options.GetActionUrl("send-sms");
        return PostAsJsonAsync(url, form, cancellationToken);
    }

    public virtual Task<CasdoorResponse?> SendEmailAsync(string title, string content, string sender,
        IEnumerable<string> receivers, CancellationToken cancellationToken = default)
    {
        CasdoorEmailForm form = new()
        {
            Title = title,
            Content = content,
            Receivers = receivers as string[] ?? receivers.ToArray(),
            Sender = sender
        };
        string url = _options.GetActionUrl("send-email");
        return PostAsJsonAsync(url, form, cancellationToken);
    }

    public virtual Task<CasdoorResponse?> SendNotification(string content, CancellationToken cancellationToken = default)
    {
        string url = _options.GetActionUrl("send-notification");
        CasdoorNotificationForm form = new()
        {
            Content = content,
        };
        return PostAsJsonAsync(url, form, cancellationToken);
    }

    /// <summary>
    ///     Sends the email by the given email provider instead of the application's default one.
    /// </summary>
    public virtual Task<CasdoorResponse?> SendEmailByProviderAsync(string title, string content, string sender, string provider,
        IEnumerable<string> receivers, CancellationToken cancellationToken = default)
    {
        CasdoorEmailForm form = new()
        {
            Title = title,
            Content = content,
            Receivers = receivers as string[] ?? receivers.ToArray(),
            Sender = sender
        };
        var queryMap = new QueryMapBuilder().Add("provider", provider).QueryMap;
        return PostAsJsonAsync(_options.GetActionUrl("send-email", queryMap), form, cancellationToken);
    }

    /// <summary>
    ///     Sends the SMS by the given SMS provider instead of the application's default one.
    /// </summary>
    public virtual Task<CasdoorResponse?> SendSmsByProviderAsync(string content, string provider, IEnumerable<string> receivers,
        CancellationToken cancellationToken = default)
    {
        CasdoorSmsForm form = new() { Content = content, Receivers = receivers as string[] ?? receivers.ToArray() };
        var queryMap = new QueryMapBuilder().Add("provider", provider).QueryMap;
        return PostAsJsonAsync(_options.GetActionUrl("send-sms", queryMap), form, cancellationToken);
    }

    /// <summary>
    ///     Sends the content to the recipient by the notification provider of the organization.
    /// </summary>
    public virtual Task<CasdoorResponse?> SendNotificationAsync(string content, string recipient,
        CancellationToken cancellationToken = default) =>
        PostAsJsonAsync(_options.GetActionUrl("send-notification"),
            new Dictionary<string, string> { ["content"] = content, ["recipient"] = recipient }, cancellationToken);
}
