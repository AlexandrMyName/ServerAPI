using MessagePack.Formatters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ServerService.Abstracts;
using ServerService.Models;
using ServerService.Models.Views;
using ServerService.Protection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace ServerService.Controllers
{

     [ApiController]
     [Route("[controller]")]
    public class ServerController : Controller
    {
 
        private readonly ITreeService _treeService;
        private Guid _treeKey = new Guid("6f806b0f-bdd9-461b-b109-dfa94871e55c");
        private List<XmlBuisness> _buisnessList = new();    
        private string _fullPathToFile;


        public ServerController(ITreeService treeService, IWebHostEnvironment env)
        {

            Console.WriteLine("Server API");
           
            _treeService = treeService;

            var pathToData = env.ContentRootPath;
            _fullPathToFile = Path.Combine(pathToData, "static.xml");
        }
         

        #region trees api
        
        public ViewResult Interection() =>  View("InitialView");
        

        [HttpPost]
        [Route("{treekey}/treeitem")]
        public async Task<ActionResult<TreeItemView>> CreateTreeItem(Guid treekey, [FromBody] TreeItemInfo info)
        {
            Console.WriteLine("Server API");
            var result = await _treeService.CreateTreeItem(treekey, info);
            return Ok(result);
        }


        [HttpPost]
        [Route("tree")]
        public async Task<ActionResult> CreateTree([FromBody] CreationTreeInfo info)
        {
            Console.WriteLine("Server API");
            var result = await _treeService.CreateTree(info.TreeKey, info.SortType, info.Name);
             
            return Ok(result);
        }


        [HttpGet]
        [Route("treeitem/{treeitemid}")]
        public async Task<ActionResult<TreeItemView>> GetTreeItem(Guid treeitemid)
        {
            Console.WriteLine("Server API");
            var result = await _treeService.GetTreeItem(treeitemid);
            GetAndCheckFileData(result.EntityId, result.ParentEntityId, result.EntityValue);
            return Ok(result);
        }


        [HttpGet]
        [Route("treeitem/{treeitemid}/children")]
        public async Task<ActionResult<TreeItemView>> GetChildTreeItems(Guid treeitemid)
        {

            var results = await _treeService.GetChildTreeItems(treeitemid);

            foreach(var result in results)
            {
                GetAndCheckFileData(result.EntityId, result.ParentEntityId, result.EntityValue);
            }
            return Ok(results);
        }


        [HttpGet]
        [Route("treeitem/{treeitemid}/parents")]
        public async Task<ActionResult<TreeItemView>> GetParentTreeItems(Guid treeitemid)
        {

            var result = await _treeService.GetParentTreeItems(treeitemid);

            return Ok(result);
        }


        [HttpPost]
        [Route("FlatTree")]
        public async Task<ActionResult<TreeItemView>> GetTreeItemsFlatTree([FromBody] GetingTreeItemsByTreeInfo info)
        {

            var result = await _treeService.GetTreeItemsTree(info);

            return Ok(result);
        }
        #endregion


        private void GetAndCheckFileData(Guid id, Guid? parent, string name)
        {

            XmlConverter xml = XmlConverter.Create(_fullPathToFile);
            XmlBuisness buisness;
             
            if (!xml.HasFile)
            {
                buisness = CreateBuisnessNode(id, parent, name);

                _buisnessList.Add(buisness);
                xml.Save(_buisnessList, "serverAPI");

                SetToDataBase(buisness);
                 
            }
            else
            {
                _buisnessList =  xml.Load(_buisnessList, "serverAPI");

                 buisness = _buisnessList.Find(op => op.Id == id);

                if(buisness == null)
                {
                    buisness = CreateBuisnessNode(id, parent, name);
                    _buisnessList.Add(buisness);
                    xml.Save(_buisnessList, "serverAPI");
                }

                SetToDataBase(buisness);
            }
            
        }


        private static XmlBuisness CreateBuisnessNode(Guid id, Guid? parent, string name)
        {

            var buisness = new XmlBuisness();
            buisness.Id = id;
            buisness.Name = name;
            buisness.Parrent = parent;
             
            return buisness;
        }

         
        private async void SetToDataBase(XmlBuisness buisness)
        {

           var result =  await GetTreeItem(buisness.Id);

            if(result.Value == null)
            {
                TreeItemInfo treeItemInfo = new TreeItemInfo();
                
                if(buisness.Parrent.HasValue)
                    treeItemInfo.ParentId = buisness.Parrent;

                treeItemInfo.Value = buisness.Name;
                treeItemInfo.Id = buisness.Id;
                
                 await CreateTreeItem(_treeKey,treeItemInfo);

                return;
            }

            if(result.Value.ParentEntityId == buisness.Parrent)
            {

                return;
            }
            else
            {

                //Change
            } 
        }
    }
}
