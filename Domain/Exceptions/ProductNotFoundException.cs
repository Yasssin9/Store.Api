using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public sealed class ProductNotFoundException :NotFoundExceptions   
    {
        public ProductNotFoundException(int id) :base($"Product With Id Equal : {id} Not Found")
        {
            
        }
    }
}
