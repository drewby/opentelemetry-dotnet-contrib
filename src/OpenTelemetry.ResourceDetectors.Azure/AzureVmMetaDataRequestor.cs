// <copyright file="AzureVmMetaDataRequestor.cs" company="OpenTelemetry Authors">
// Copyright The OpenTelemetry Authors
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
// </copyright>

using System;
using System.Net.Http;
using System.Text.Json;

namespace OpenTelemetry.ResourceDetectors.Azure;

internal static class AzureVmMetaDataRequestor
{
    private const string AzureVmMetadataEndpointURL = "http://169.254.169.254/metadata/instance/compute?api-version=2021-12-13&format=json";

    public static Func<AzureVmMetadataResponse?> GetAzureVmMetaDataResponse { get; internal set; } = GetAzureVmMetaDataResponseDefault!;

    public static AzureVmMetadataResponse? GetAzureVmMetaDataResponseDefault()
    {
        using var httpClient = new HttpClient();

        httpClient.DefaultRequestHeaders.Add("Metadata", "True");
        var res = httpClient.GetStringAsync(AzureVmMetadataEndpointURL).ConfigureAwait(false).GetAwaiter().GetResult();

        if (res != null)
        {
            return JsonSerializer.Deserialize<AzureVmMetadataResponse>(res);
        }

        return null;
    }
}
