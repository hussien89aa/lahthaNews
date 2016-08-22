using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsApp.Classes
{
    public class myresources
    {  public  string  ResourcesName{get;set;}
    public string NesourceID { get; set; }
    public string ImageLink { get; set; }
    public string[] SubResourcesName { get; set; }
    public string[] SubNesourceID { get; set; }
    public string[] IsFollow { get; set; }
    public string[] NumberOfFollowers { get; set; }
    public string[] ChannelImage { get; set; } 
    }
}