using Discord;

namespace DKPBot.Discord
{
    public class Activity : IActivity
    {
        public string Name { get; }
        public ActivityType Type { get; }
        public ActivityProperties Flags { get; }
        public string Details { get; }

        public Activity(string status, ActivityType type, ActivityProperties flags, string details)
        {
            Name = status;
            Type = type;
            Flags = flags;
            Details = details;
        }
    }
}