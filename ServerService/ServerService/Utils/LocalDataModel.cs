using Microsoft.VisualBasic;
using Quartz;
using ServerService.Models;
using System.Collections.Generic;


namespace ServerService.Utils
{
 
    public static class LocalDataModel
    {

        internal static List<XmlBuisness> XmlBuisneses { get; private set; } 


        internal static void SetXmlBuisnessList(List<XmlBuisness> refList) => XmlBuisneses = refList;
         
         
    }
}
