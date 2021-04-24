using System.Linq;
using Application.Shared.Dtos.Models;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using WebUI.Blazor.Services;

namespace WebUI.Blazor.Pages
{
   
    public partial class PathAdminPage
    {
        [Inject] public PathService PathService { get; set; }

        public PathDto[] Paths { get; set; }
      
        protected override async Task OnInitializedAsync()
        {
            Paths = await PathService.GetListAsync();
        }
    }
}
