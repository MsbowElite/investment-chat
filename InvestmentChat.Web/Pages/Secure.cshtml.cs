using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace InvestmentChat.Web.Pages
{
    //[Authorize]
    public class SecureModel : PageModel
    {
        private readonly ILogger<SecureModel> _logger;

        public SecureModel(ILogger<SecureModel> logger)
        {
            var user = User;
            _logger = logger;
        }

        public async void OnGet()
        {
            ViewData["token"] = await HttpContext.GetTokenAsync("id_token");
        }
    }
}
