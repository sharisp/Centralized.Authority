using MediatR;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Domain
{
    public class BaseEntity
    {
      //  public long Id { get; set; }
        [NotMapped]
        public List<INotification> events = new();

      public  void AddDomainEvent(INotification eventItem)
        {
            events.Add(eventItem);
        }
        public void AddIfNotExists(INotification eventItem)
        {
            if (!events.Contains(eventItem))
            {
                events.Add(eventItem);
            }

        }
        public List<INotification> GetDomainEvents()
        {
            return events;
        }
        public void ClearDomainEvents()
        {
            events.Clear();
        }
    }
}
