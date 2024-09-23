using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

namespace SystemPOS.Client.Pages
{
    public partial class POSHome : ComponentBase
    {
        private bool isLoading = false;
        public string? alertMessage = null;
        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            isLoading = false;
            StateHasChanged();
        }
    }
}
