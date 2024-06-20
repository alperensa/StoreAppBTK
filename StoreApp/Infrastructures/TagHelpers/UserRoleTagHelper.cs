using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StoreApp.Infrastructures.TagHelpers
{
    [HtmlTargetElement("td",Attributes ="user-role")]
    public class UserRoleTagHelper : TagHelper
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        [HtmlAttributeName("user-name")]
        public String? Username { get; set; }

        public UserRoleTagHelper(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var user = await _userManager.FindByNameAsync(Username);
            TagBuilder ul = new TagBuilder("ul");
            ul.Attributes.Add("class", "list-unstyled");
            var roles = _roleManager.Roles.ToList().Select(r => r.Name);
            foreach(var role in roles)
            {
                if(await _userManager.IsInRoleAsync(user,role))
                {
                TagBuilder li = new TagBuilder("li");
                    li.InnerHtml.Append(role);
                ul.InnerHtml.AppendHtml(li);

                }

            }
            output.Content.AppendHtml(ul);

        }
    }
}