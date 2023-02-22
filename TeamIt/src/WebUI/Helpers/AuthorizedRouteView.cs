using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using WebUI.Services;

namespace WebUI.Helpers
{
    public class AuthorizedRouteView : RouteView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public IUserService AccountService { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            var isProtected = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) is not null;
            if (isProtected && !AccountService.IsLoggedIn())
            {
                NavigationManager.NavigateTo("account/login");
            }
            else
            {
                base.Render(builder);
            }
        }
    }
}