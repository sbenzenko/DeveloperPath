using DeveloperPath.Domain.Common;
using System.Collections.Generic;

namespace DeveloperPath.Domain.Entities
{
  //TODO: this class id from the template. Remove it
  public class TodoList : AuditableEntityTemplate
  {
        public TodoList()
        {
            Items = new List<TodoItem>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Colour { get; set; }

        public IList<TodoItem> Items { get; set; }
    }
}
