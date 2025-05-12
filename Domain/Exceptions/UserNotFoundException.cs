using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNotFoundException(string email)
    :NotFoundExceptions($"No User With Email : {email} is not Found")
    {
    }
}
