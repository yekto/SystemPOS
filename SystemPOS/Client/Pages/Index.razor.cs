using SystemPOS.Shared;
using SystemPOS.Client;
using Microsoft.AspNetCore.Components;

using System.IdentityModel.Tokens.Jwt;
using SystemPOS.Shared.POS;
using Microsoft.JSInterop;
using SystemPOS.Client.Services;
using Blazored.SessionStorage;

namespace SystemPOS.Client.Pages
{
    public partial class Index : ComponentBase
    {
        private reqLogin log {  get; set; } = new reqLogin();
        public reqLog lo {  get; set; } = new reqLog();
        private respLogin user {  get; set; } = new respLogin();

        private bool isLoading = false;
        public string? alertMessage = null;

        private IJSObjectReference _jsModule;

        protected override async Task OnInitializedAsync()
        {
            StateHasChanged();
        }
        private async Task checkLogin()
        {
            await checkdulu();

        }
        private async Task checkdulu()
        {
            
            try
            {
                reqLog reqLogin = new reqLog();
                reqLogin.Uname = lo.Uname;
                reqLogin.Pass = lo.Pass;
                var a = await iPosServices.Login(reqLogin);
                if (a.isSuccess)
                {
                    if (a.Data.Usernames != "" && a.Data.Usernames != "")
                    {
                        iPosServices.activeUser = new respLogin();
                        iPosServices.activeUser.Names = a.Data.Names;
                        iPosServices.activeUser.Usernames = a.Data.Usernames;
                        iPosServices.activeUser.Token = a.Data.Token;


                        var handler = new JwtSecurityTokenHandler();
                        var jsonToken = handler.ReadToken((string)a.Data.Token);
                        var tokenz = jsonToken as JwtSecurityToken;
                        sessionStorage.SetItem("Name", a.Data.Names);
                        sessionStorage.SetItem("Username", a.Data.Usernames);
                        sessionStorage.SetItem("Token", a.Data.Token);
                        navigate.NavigateTo("POSHome");

                    }
                    else
                    {
                        alertMessage = "Username or Password is wrong";
                    }
                }
            }
            catch (Exception ex)
            {
                alertMessage = ex.Message;
                isLoading = false;
                StateHasChanged();
                throw;
            }

            StateHasChanged();
        }
        private async void Clear()
        {
            log.Username = "";
            log.Password = "";
            alertMessage = null;
            StateHasChanged();
        }
    }
}
