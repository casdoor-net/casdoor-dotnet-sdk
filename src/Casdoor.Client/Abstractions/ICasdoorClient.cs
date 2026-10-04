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

public interface ICasdoorClient :
    ICasdoorUserClient, ICasdoorTokenClient, ICasdoorResourceClient, ICasdoorServiceClient, ICasdoorWebhookClient,
    ICasdoorApplicationClient, ICasdoorOrganizationClient, ICasdoorProviderClient, ICasdoorAccountClient, ICasdoorModelClient,
    ICasdoorEnforcerClient, ICasdoorGroupClient, ICasdoorPlanClient, ICasdoorSyncerClient, ICasdoorSubscriptionClient, ICasdoorPermissionClient,
    ICasdoorAdapterClient, ICasdoorCertClient, ICasdoorPaymentClient, ICasdoorPricingClient, ICasdoorProductClient, ICasdoorSessionClient,
    ICasdoorRoleClient, ICasdoorRecordClient, ICasdoorInvitationClient, ICasdoorOrderClient, ICasdoorTransactionClient,
    ICasdoorPolicyClient, ICasdoorMfaClient
{
    public ICasdoorClient WithAccessToken(string accessToken);
    public string GetId(string name, string? owner = null);
    public string GetSigninUrl(string redirectUrl);
    public string GetSigninUrl(string codeVerifier, bool noRedirect);

}
