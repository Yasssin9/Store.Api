using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class BasketNotFoundException(string id) 
        : NotFoundExceptions($"Basket With Id Equal : {id} Not Found")
    {
      
    }
}
