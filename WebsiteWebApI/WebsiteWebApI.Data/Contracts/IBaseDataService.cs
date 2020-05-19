using System;
using System.Collections.Generic;
using System.Text;
using WebsiteWebApI.Data.CrudAPIModels.Input;

namespace WebsiteWebApI.Data.Contracts
{
    public interface IBaseDataService<TCreate, TEdit, TEntityModel, TEntity, TContext>
    {
        TEntityModel CreateItem(TCreate model);
        void DeleteItem(DeleteInputModel item);
        TEntityModel EditItem(TEdit model);
        TEntityModel GetItemById(GetInputModel id);
        IList<TEntityModel> GetItems(IList<FilterModel> filters, SortModel sortModel, int page, int pageSize);

        void CreateItemWithNoReturn(TCreate model);

        void EditItemNoReturn(TEntity model);
        void SaveChanges();
    }
}
