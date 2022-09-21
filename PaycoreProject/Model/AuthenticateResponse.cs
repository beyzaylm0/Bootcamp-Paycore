using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace PaycoreProject.Model
{
    public class AuthenticateResponse
    {
        [Display(Name = "Expire Time")]
        public DateTime ExpireTime { get; set; }


        [Display(Name = "Access Token")]
        public string AccessToken { get; set; }



        public string Email { get; set; }

        public int SessionTimeInSecond { get; set; }
    }
}
