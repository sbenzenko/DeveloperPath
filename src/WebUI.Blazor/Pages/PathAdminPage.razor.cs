using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Shared.ClientModels;
using WebUI.Blazor.Services;

namespace WebUI.Blazor.Pages
{
   
    public partial class PathAdminPage
    {
        [Inject] public PathService PathService { get; set; }

        public List<Path> Paths { get; set; }
      
        protected override async Task OnInitializedAsync()
        {
            ;
            try
            {
                Paths = await PathService.GetListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
