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

namespace Casdoor.Client.UnitTests.Utilities;

public static class TestUtils
{
    public static T AssertNotNull<T>(T? value) where T : class
    {
        Assert.NotNull(value);
        return value;
    }

    /// <summary>
    /// Generates a random numeric code of specified length
    /// </summary>
    /// <param name="length">Length of the random code</param>
    /// <returns>Random numeric string</returns>
    public static string GetRandomCode(int length)
    {
        const string digits = "0123456789";
        var result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = digits[Random.Shared.Next(digits.Length)];
        }
        return new string(result);
    }

    /// <summary>
    /// Generates a random name with the given prefix
    /// </summary>
    /// <param name="prefix">Prefix for the name</param>
    /// <returns>Random name in format "prefix_xxxxxx" where x is a random digit</returns>
    public static string GetRandomName(string prefix)
    {
        return $"{prefix}_{GetRandomCode(6)}";
    }
}
