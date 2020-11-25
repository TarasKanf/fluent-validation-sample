using System.Collections.Generic;

namespace SampleApi.Models
{
    public class ParentModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ChildModel MainChild { get; set; }
        public List<ChildModel> Children { get; set; }
    }
}