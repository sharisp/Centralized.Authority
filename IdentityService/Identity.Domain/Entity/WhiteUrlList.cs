namespace Identity.Domain.Entity
{
    public class WhiteUrl:BaseEntity,IAggregateRoot
    {
        private WhiteUrl()
        {
            
        }

        public  string Url { get; set; }
        public bool IsDel { get; set; }
        public string? Descriptions { get; set; }

        public DateTime? CreateDateTime { get; set; }

        public DateTime? DeleteDateTime { get; set; }
    }
}
