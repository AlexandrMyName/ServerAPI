using MessagePack.Formatters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Quartz;
using ServerService.Abstracts;
using ServerService.Models;
using ServerService.Models.Views;
using ServerService.Protection;
using ServerService.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
 

namespace ServerService.Controllers
{

     [ApiController]
     [Route("[controller]")]
    public class ServerController : Controller
    {
         
        private readonly ITreeService _treeService;

        private Guid _treeKey;
        private List<XmlBuisness> _buisnessList = new();    
        private string _fullPathToFile;

        private IConfigurationRoot _idKeyConfigurationr { get; }


        public ServerController(ITreeService treeService, IWebHostEnvironment env)
        {

            var jobKey = new JobKey("buisneses");
             
            Console.WriteLine("Server Started!");

            _idKeyConfigurationr = new ConfigurationBuilder()
              .SetBasePath(env.ContentRootPath)
              .AddJsonFile("appsettings.json")
              .Build();

            _treeKey = new Guid(_idKeyConfigurationr.GetConnectionString("Tree_ID"));
            _treeService = treeService;
             
            _buisnessList = LocalDataModel.XmlBuisneses;

            var pathToData = env.ContentRootPath;
            _fullPathToFile = Path.Combine(pathToData, "static.xml");
        }


        public ViewResult Interection() => View("InitialView");
         
        #region trees api
        [HttpPost]
        [Route("{treekey}/treeitem")]
        public async Task<ActionResult<TreeItemView>> CreateTreeItem(Guid treekey, [FromBody] TreeItemInfo info)
        {

            Console.WriteLine("CreateTreeItem request");

            var result = await _treeService.CreateTreeItem(treekey, info);
            return Ok(result);
        }


        [HttpPost]
        [Route("tree")]
        public async Task<ActionResult> CreateTree([FromBody] CreationTreeInfo info)
        {

            Console.WriteLine("Creation Tree request");

            var result = await _treeService.CreateTree(info.TreeKey, info.SortType, info.Name);
             
            return Ok(result);
        }


        [HttpGet]
        [Route("treeitem/{treeitemid}")]
        public async Task<ActionResult<TreeItemView>> GetTreeItem(Guid treeitemid)
        {

            Console.WriteLine("Get Tree Item request ");

            var result = await _treeService.GetTreeItem(treeitemid);
            GetAndCheckFileData(result.EntityId, result.ParentEntityId, result.EntityValue);
            return Ok(result);
        }


        [HttpGet]
        [Route("treeitem/{treeitemid}/children")]
        public async Task<ActionResult<TreeItemView>> GetChildTreeItems(Guid treeitemid)
        {

            Console.WriteLine("Get Chiled Tree Items request");

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
            
            var results = await _treeService.GetTreeItemsTree(info);
            results.ToList().ForEach(res => GetAndCheckFileData(res.EntityId, res.ParentEntityId, res.EntityValue));
            return Ok(results);
        }

         
        [HttpPost]
        [Route("CheckActivity")]
        public ActionResult<bool> GetActivityStatusItemTree([FromBody] TreeItemView view)
        {
            var data = LocalDataModel.XmlBuisneses.Find(item => item.Id == view.EntityId);
 
            if(data == null) return Ok(false); 

            return Ok(data.IsActive);
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
                 
            }

            var activityInfo = LocalDataModel.XmlBuisneses.Find(item => item.Name == buisness.Name);

            if (activityInfo == null)
            {
                Console.WriteLine("Не найден объект");
                LocalDataModel.XmlBuisneses.Add(buisness);
                Console.WriteLine(LocalDataModel.XmlBuisneses.Count + " кол-во");
            }
            else
            {
                Console.WriteLine(activityInfo.Name);
            }
            
            Console.WriteLine(activityInfo == null);
            LocalDataModel.XmlBuisneses.ForEach(xml => { Console.WriteLine(xml.IsActive); });
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

               
            } 
        }
    }
}
