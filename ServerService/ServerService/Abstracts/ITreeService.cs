using ServerService.Models;
using System.Threading.Tasks;
using System;
using ServerService.Models.Views;
using System.Collections.Generic;


namespace ServerService.Abstracts
{

    public interface ITreeService
    {

        Task<Tree> CreateTree(Guid treeKey, string sortType, string name);

        Task<TreeItemView> CreateTreeItem(Guid treeKey, TreeItemInfo treeItemInfo);

        Task<IReadOnlyCollection<TreeItemView>> GetTreeItemsTree(GetingTreeItemsByTreeInfo info);

        Task<TreeItemView> GetTreeItem(Guid treeItemId);

        Task<IReadOnlyCollection<TreeItemView>> GetParentTreeItems(Guid treeItemId);

        Task<IReadOnlyCollection<TreeItemView>> GetChildTreeItems(Guid treeItemId);
         
    }
}
