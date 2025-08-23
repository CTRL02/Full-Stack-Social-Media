using System.ComponentModel.DataAnnotations;

namespace socialmedia.Entities
{
    public class Group
    {
        public Group() { }
        public Group(string name)
        {
            this.name = name;
        }

        [Key]
        public string name { get; set; }
        public ICollection<Connection> connections { get; set; } = new List<Connection>();


    }
}
