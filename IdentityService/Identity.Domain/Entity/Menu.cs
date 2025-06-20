﻿using Identity.Domain.Interfaces;

namespace Identity.Domain.Entity
{

    public class Menu : BaseEntity, IAggregateRoot
    {
        private Menu()
        {

        }
        public long Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public long ParentID { get; set; }
        public string Component { get; set; }
        public string icon { get; set; }
        public int Sort { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsShow { get; set; }
    }
}
