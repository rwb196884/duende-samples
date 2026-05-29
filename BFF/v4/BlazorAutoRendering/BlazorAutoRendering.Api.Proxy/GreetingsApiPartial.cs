// Copyright (c) Duende Software. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlazorAutoRendering.Api.Proxy;

public partial class GreetingsApi
{
    public Uri? ClientBaseAddress
    {
        get
        {
            return _httpClient.BaseAddress;
        }
    }
}
