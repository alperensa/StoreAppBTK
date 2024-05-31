using System.ComponentModel.DataAnnotations;

namespace StoreApp.Models
{
	public class LoginModel
	{
        private string _returnurl;
        [Required(ErrorMessage = "Enter username")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Enter password")]
        public string? Password { get; set; }

        public string ReturnUrl
        {
            get
            {
                if(_returnurl is null)
                {
                    return "/";
                }
                else
                {
                    return _returnurl;
                }
            }

            set
            {
                _returnurl = value;
            }
        }
    }
}
