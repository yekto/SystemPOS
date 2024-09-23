using SystemPOS.Shared;
using SystemPOS.Client;
using Microsoft.AspNetCore.Components;

using System.IdentityModel.Tokens.Jwt;
using SystemPOS.Shared.POS;
using Microsoft.JSInterop;
using SystemPOS.Client.Services;
using Blazored.SessionStorage;
using System.ComponentModel;
using Microsoft.AspNetCore.Components.Forms;

namespace SystemPOS.Client.Pages
{
    public partial class ManageItem : ComponentBase
    {

        private bool isLoading = false;
        public string? alertMessage = null;

        public int deleteId = 0;
        public int updateId = 0;

        public bool modalUploadItem = false;
        public bool modalUpdateItem = false;
        public bool modalAlertDeleteItem = false;

        private Guid inputFileId = Guid.NewGuid();

        public int selectIdCat;
        

        List<respItem> listItem = new List<respItem>();
        List<respItem> addItem = new List<respItem>();
        List<Category> masterCategory = new List<Category>();
        respItem editItem = new respItem();

        private IJSObjectReference _jsModule;
        protected override async Task OnInitializedAsync()
        {
            await LoadData();
            StateHasChanged();
        }

        public async Task LoadData()
        {
            listItem.Clear();
            masterCategory.Clear();
            addItem.Add(new respItem());
            string username = iposServices.activeUser.Usernames;
            var fgh = await iposServices.GetCategory(username, iposServices.activeUser.Token);
            if (fgh != null)
            {
                foreach (var f in fgh.Data)
                {
                    Category x = new Category();
                    x.Id = f.Id;
                    x.CatName = f.CatName;
                    masterCategory.Add(x);
                }
            }

            var asd = await iposServices.GetItem(username);
            
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
            StateHasChanged();
        }

        public async Task triggerpopupUpload()
        {
            modalUploadItem = !modalUploadItem;
            StateHasChanged() ;
        }

        void OnStoreCodeChanged(ChangeEventArgs e, respItem item)
        {
            var selectedItemCode = e.Value?.ToString()?.Trim();
            if (int.TryParse(selectedItemCode, out int value))
            {
                var selectedItem = masterCategory.FirstOrDefault(x => x.Id == Convert.ToInt32(selectedItemCode));
                if (selectedItem != null)
                {
                    item.Id = selectedItem.Id;
                }
                else
                {
                    item.Id = 0;
                }
            }
            else
            {
                item.Id = 0;
            }
        }

        public void addingItem()
        {
            addItem.Add(new respItem());
        }
        private void DeleteRow(respItem item)
        {
            addItem.Remove(item);
        }

        public async Task UploadItem(List<respItem> data)
        {
            if (data.Count > 0)
            {
                try
                {
                    var result = await iposServices.PostItem(data);
                    if (result.isSuccess)
                    {
                        addItem.Clear();
                        modalUploadItem = false;
                        await LoadData();
                    }
                }
                catch (Exception ex)
                {
                    alertMessage = ex.Message;
                }
            }
            else
            {
                alertMessage = "no data upload";
            }
            StateHasChanged();
        }


        public async Task triggerpopupdelete(int id)
        {
            modalAlertDeleteItem = !modalAlertDeleteItem;
            if (modalAlertDeleteItem)
            {
                deleteId = id;
            }
            else
            {
                deleteId=0;
            }
            StateHasChanged() ;
        }
        public async Task deleteItembyId(int id)
        {
            try
            {
                var result = await iposServices.DeleteItem(id, iposServices.activeUser.Token);
                if (result.isSuccess)
                {
                    modalAlertDeleteItem = false;
                }
                await LoadData();
            }
            catch(Exception ex)
            {
                alertMessage = ex.Message;
            }
            StateHasChanged();
        }

        public async Task triggerpopupedit(int id)
        {
            modalUpdateItem = !modalUpdateItem;
            editItem = new respItem();
            editItem = listItem.Where(x => x.Id == id).FirstOrDefault();

            if (modalUpdateItem)
            {
                updateId = id;
            }
            else
            {
                updateId = 0;
            }
            StateHasChanged();
        }

        public async Task OnInputFileChange(InputFileChangeEventArgs e, respItem item)
        {
            try
            {

                //alertMessageUpload = string.Empty;

                if (e.File.Size <= 5242880)
                {

                    var datafile = e.File.OpenReadStream(5242880);
                    var fileBytes = new byte[datafile.Length];
                    await datafile.ReadAsync(fileBytes, 0, (int)fileBytes.Length);

                    item.Path = Convert.ToBase64String(fileBytes);
                    item.Mime = e.File.ContentType;
                    

                }
                else
                {
                    alertMessage = "The data file must not exceed 5 MB.";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            StateHasChanged();
        }

        public async Task updateItem(respItem file)
        {
            var a = await iposServices.UpdateItem(file);
            Console.WriteLine(a);
            Console.WriteLine(editItem);
            StateHasChanged();
        }
    }
}
