using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGame.Objects
{
    public class StyleFactoryDictionary : Dictionary<string, StyleFactoryAssignmentDictionary>
    {
        public new void Add(string collection_property, StyleFactoryAssignmentDictionary assignments) =>
            base.Add(collection_property, assignments);


        public new StyleFactoryAssignmentDictionary this[string collection_property]
        {
            get => base[collection_property];
            set => base[collection_property] = value;
        }
    }
}
