using Microsoft.AspNetCore.Http;
using ServerService.Abstracts;
using ServerService.Domain;
using ServerService.Models;
using ServerService.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ServerService.Services
{

    public class TreeService : ITreeService
    {

        private readonly TreeRepository _repository;

        public TreeService(TreeRepository repository) => _repository = repository;
         

        public async Task<Tree> CreateTree(Guid treeKey, string sortType, string name) =>  await _repository.CreateTree(treeKey, sortType, name);
       

        public async Task<TreeItemView> CreateTreeItem(Guid treeKey, TreeItemInfo treeItemInfo)
        {

            var tree = await _repository.GetTree(treeKey);

            if (tree == null)
                throw new BadHttpRequestException("Tree doesn't exists");

            var treeItem = await _repository.CreateTreeItem(treeKey, treeItemInfo);

            return MapToTreeItemInfo(treeItem);
        }

        public async Task<IReadOnlyCollection<TreeItemView>> GetTreeItemsTree(GetingTreeItemsByTreeInfo info)
        {

            return (await _repository.GetTreeItemsTree(
                                        info.TreeKey,
                                        info.FromIndex,
                                        info.ToIndex,
                                        info.ExpandedTreeItemIds))
                                        .Select(treeItem => MapToTreeItemInfo(treeItem))
                                        .ToArray();
        }


        public async Task<TreeItemView> GetTreeItem(Guid treeItemId)
        {

            var treeItem = await _repository.GetTreeItem(treeItemId);

            return MapToTreeItemInfo(treeItem);
        }


        public async Task<IReadOnlyCollection<TreeItemView>> GetParentTreeItems(Guid treeItemId)
        {

            await CheckTreeItemExist(treeItemId);

            var treeItems = await _repository.GetParentTreeItems(treeItemId);

            return treeItems.Select(x => MapToTreeItemInfo(x)).ToArray();
        }


        public async Task<IReadOnlyCollection<TreeItemView>> GetChildTreeItems(Guid treeItemId)
        {

            await CheckTreeItemExist(treeItemId);

            var treeItems = await _repository.GetChildTreeItems(treeItemId);

            return treeItems.Select(x => MapToTreeItemInfo(x)).ToArray();
        }

        private TreeItemView MapToTreeItemInfo(TreeItem treeItem) =>
             new TreeItemView
             {
                 Level = treeItem.Level,
                 Path = treeItem.MaterializedPath,
                 EntityId = treeItem.EntityId,
                 ParentEntityId = treeItem.ParentEntityId,
                 EntityValue = treeItem.EntityValue
             };

        private async Task CheckTreeItemExist(Guid treeItemId)
        {

            var treeItem = await _repository.GetTreeItem(treeItemId);

            if (treeItem == null)
                throw new BadHttpRequestException("TreeItem doesn't exists");
        }
    }
}
