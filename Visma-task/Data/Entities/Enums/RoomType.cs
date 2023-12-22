using System;
using System.ComponentModel;

namespace Visma_task.Data.Entities.Enums
{
    public enum RoomType
    {
        [Description("Meeting room")]
        MeetingRoom,
        [Description("Kitchen")]
        Kitchen,
        [Description("Bathroom")]
        Bathroom
    }
}

