using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SystemPOS.Client;
using SystemPOS.Shared;

namespace Mitra10Logistic.Client.Pages.LoginPages
{
    public partial class LoginPage : ComponentBase
    {
        
        private bool isLoading = false;
        public string? alertMessage = null;


        private IJSObjectReference _jsModule;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            
            isLoading = false;
            StateHasChanged();
        }
        
        
    }
}
