namespace Identity.Domain.Entity
{
    public class Sys : BaseAuditableEntity, IAggregateRoot
    {
        //insert manually
        private Sys()
        {

        }
        public string SystemName { get; private set; }
        public string SystemCode { get; private set; }
    }
}