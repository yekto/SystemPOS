using Microsoft.AspNetCore.Components;
using SystemPOS.Shared.POS;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Forms;

namespace SystemPOS.Client.Pages
{
    public partial class Pos : ComponentBase
    {
        private IJSObjectReference _jsModule;

        List<respItem> listItem = new List<respItem>();
        List<Category> masterCategory = new List<Category>();
        POSmodel sell = new POSmodel();

        private bool isLoading = false;
        public string? alertMessage = null;

        private bool Poss = true;
        private bool Reports = false;
        private bool Stocks = false;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            StateHasChanged();
        }

        public async Task LoadData()
        {
            string username = iposServices.activeUser.Usernames;
            var f = await iposServices.GetCategory(username, iposServices.activeUser.Token);
            if (f != null)
            {
                foreach (var fd in f.Data)
                {
                    Category x = new Category();
                    x.Id = fd.Id;
                    x.CatName = fd.CatName;
                    masterCategory.Add(x);
                }
            }

            var asd = await iposServices.GetItem(username, iposServices.activeUser.Token);

            if (asd.Data == null)
            {

            }
            else
            {
                foreach (var x in asd.Data)
                {
                    respItem z = new respItem();
                    z.Id = x.Id;
                    z.ItemName = x.ItemName;
                    z.Category = x.Category;
                    z.Price = x.Price;
                    z.Qty = x.Qty;
                    z.Path = x.Path;
                    z.Mime = x.Mime;
                    listItem.Add(z);
                }
            }
            StateHasChanged ();
        }

        public async Task ActivePage(int Id)
        {
            Poss = false;
            Stocks = false;
            Reports = false;

            if (Id == 1)
            {
                Poss = true;
            }
            else if (Id == 2)
            {
                Reports = true;
            }
            else if(Id == 3)
            {
                Stocks = true;
            }
            StateHasChanged();
        }

        public async Task OnItemChange(ChangeEventArgs e, POSmodel t)
        {
            var selectedItemCode = e.Value?.ToString()?.Trim();
            if (int.TryParse(selectedItemCode, out int value))
            {
                var selectedItem = listItem.FirstOrDefault(x => x.Id == Convert.ToInt32(selectedItemCode));
                var selectCat = masterCategory.FirstOrDefault(x => x.Id == selectedItem.Category);
                if (selectedItem != null)
                {
                    sell.itemId = selectedItem.Id.ToString();
                    sell.itemName = selectedItem.ItemName;
                    sell.category = selectCat.CatName;
                    sell.price = Convert.ToDecimal(selectedItem.Price);
                    sell.qty = Convert.ToDecimal("0");
                    sell.totalPrice = 0;
                }
                else
                {
                    sell.itemId = "";
                    sell.itemName = "";
                    sell.category = "";
                    sell.price = 0;
                    sell.qty = Convert.ToDecimal("0");
                    sell.totalPrice = 0;
                }
            }
            else
            {
                sell.itemId = "";
                sell.itemName = "";
                sell.category = "";
                sell.price = 0;
                sell.qty = Convert.ToDecimal("0");
                sell.totalPrice = 0;
            }
            StateHasChanged();
        }

        public async Task OnQtyChange(ChangeEventArgs e, POSmodel t)
        {
            var selectedItemCode = e.Value?.ToString()?.Trim();
            var j = listItem.FirstOrDefault(x => x.Id == Convert.ToInt32(sell.itemId));
            if (int.TryParse(selectedItemCode, out int value))
            {
                if (Convert.ToDecimal(e.Value) <= j.Qty)
                {
                    sell.qty = Convert.ToDecimal(e.Value);
                    sell.totalPrice = t.price * Convert.ToDecimal(e.Value);
                }
                else
                {
                    alertMessage = "Qty more than stock";
                }
                
            }
        }

        public async Task Selling(POSmodel t)
        {
            t.username = iposServices.activeUser.Usernames;
            t.date = DateTime.Now;
            var a = await iposServices.InputSales(t, iposServices.activeUser.Token);
            if (a.isSuccess)
            {
                
            }
        }
    }
}
