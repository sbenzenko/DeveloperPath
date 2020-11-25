using DeveloperPath.Domain.Common;
using DeveloperPath.Domain.Enums;
using System;

namespace DeveloperPath.Domain.Entities
{
  //TODO: this class id from the template. Remove it
  public class TodoItem : AuditableEntityTemplate
  {
        public int Id { get; set; }

        public int ListId { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public bool Done { get; set; }

        public DateTime? Reminder { get; set; }

        public PriorityLevel Priority { get; set; }


        public TodoList List { get; set; }
    }
}
