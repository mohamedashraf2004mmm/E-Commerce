using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Entities.Common
{
    public abstract class BaseEntity<T>
    {
        public T id { get; set; } = default!;

    }
}
