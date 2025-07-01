using Identity.Domain.Enums;
using Identity.Domain.Interfaces;

namespace Identity.Domain.Entity
{

    public class Menu : BaseAuditableEntity, IAggregateRoot
    {
        private Menu()
        {

        }
        public string Title { get; private set; }
        public string Path { get; private set; }
        public long ParentId { get; private set; }
        public string? Component { get; private set; }
        public string? Icon { get; private set; }
        public int Sort { get; private set; }
        public MenuType Type { get; private set; }


        public bool IsShow { get; private set; }
        public Uri? ExternalLink { get; private set; }
        public string SystemName { get; private set; }//for multi-system support, e.g., "Identity", "Order", etc.

        public List<Role> Roles { get; private set; } = new List<Role>();

        public List<Permission> Permissions { get; private set; } = new List<Permission>();//only for menu type

        public Menu(string title, string path, long parentID, int sort, MenuType type,  string systemName, string? component = null, string? icon = null, Uri? externalLink = null)
        {
            Title = title;
            Path = path;
            ParentId = parentID;
            Component = component;
            this.Icon = icon;
            Sort = sort;
            Type = type;
            IsShow = true;
            this.ExternalLink = externalLink;
            SystemName = systemName;
      
        }
        public void ChangeTitle(string title)
        {
            Title = title;
        }
        public void ChangePath(string path)
        {
            Path = path;
        }
        public void ChangeParentID(long parentID)
        {
            ParentId = parentID;
        }
        public void ChangeComponent(string? component)
        {
            Component = component;
        }
        public void ChangeIcon(string? icon)
        {
            Icon = icon;
        }
        public void ChangeSort(int sort)
        {
            Sort = sort;
        }
        public void ChangeType(MenuType type)
        {
            Type = type;
        }
        public void ChangeIsShow(bool isShow)
        {
            IsShow = isShow;
        }
        public void ChangeExternalLink(Uri? externalLink)
        {
            ExternalLink = externalLink;
        }
        public void ChangeSystemName(string systemName)
        {
            SystemName = systemName;
        }
    }
}
