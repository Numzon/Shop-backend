﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Common.Models;

[ExcludeFromCodeCoverage]
public sealed class RedisDto
{
    public string Uri { get; set; } = null!;
}
