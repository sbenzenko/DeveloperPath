using DeveloperPath.Application.Common.Mappings;
using DeveloperPath.Domain.Entities;

namespace DeveloperPath.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
