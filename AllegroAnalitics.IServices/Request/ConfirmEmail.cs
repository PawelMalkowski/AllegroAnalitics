using System;
using System.Collections.Generic;
using System.Text;

namespace AllegroAnalitics.IServices.Request
{
    public class ConfirmEmail
    {
        public string Token { get; set; }
        public string UserName { get; set; }

    }
}
