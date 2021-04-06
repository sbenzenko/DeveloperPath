using System.Collections.Generic;

namespace IdentityProvider.Models.Error
{
    public class ErrorModel
    {
        public ErrorModel(string returnUrl)
        {
            ReturnUrl = returnUrl;
            Errors = new List<string>();
        }
        public List<string> Errors { get; set; }
        public string ReturnUrl { get; set; }
    }
}
