using System;
using System.Collections.Generic;
using System.Linq;
using Napp.Web.Navigation;

namespace Cvm.Web.Facade
{
    public class SiteHistory
    {
        private LinkedList<SiteHistoryItem> history=new LinkedList<SiteHistoryItem>();
        public void Add(SiteHistoryItem item)
        {
            //Remove existing items with the same name
            history.Where(_item => !_item.Name.Equals(item.Name));
            if (history.Count>5)
            {
                history.RemoveLast();
            }
            history.AddFirst(item);
        }
        public SiteHistoryItem[] GetHistoryItems()
        {
            return history.ToArray();
        }

        public void Add(PageLink link, string title)
        {
            Add(new SiteHistoryItem(){Link=link, Name=title});
        }
    }
}