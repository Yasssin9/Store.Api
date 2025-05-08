using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.IdentityDtos
{
    public class LoginDto
    {
        [EmailAddress]
        [Required]
        public string MyEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
