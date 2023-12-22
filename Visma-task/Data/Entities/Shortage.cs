using System;
using Visma_task.Data.Entities.Enums;

namespace Visma_task.Data.Entities
{
	public class Shortage
	{
        public string Title { get; set; } = "";
        public string Name { get; set; } = "";
        public RoomType Room { get; set; }
        public CategoryType Category { get; set; }
        public int Priority { get; set; }
        public DateOnly CreatedOn { get; set; }
    }
}

