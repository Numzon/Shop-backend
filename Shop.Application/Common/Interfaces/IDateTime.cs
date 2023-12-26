using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Application.Common.Interfaces;
public interface IDateTime
{
    public DateTime Now { get; }
}
