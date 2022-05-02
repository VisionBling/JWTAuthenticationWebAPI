using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuth.Data.Entities
{
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUser User { get; set; }

    }

}
