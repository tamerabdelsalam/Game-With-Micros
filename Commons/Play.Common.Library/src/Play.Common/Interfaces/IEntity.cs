using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Play.Common.Interfaces;

public interface IEntity
{
    Guid Id { get; set; }
}
