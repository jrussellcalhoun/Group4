using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WordGame.Objects
{
    public struct Assignment
    {
        public DependencyProperty Dependency { get; set; }
        public object Value { get; set; }

        public Assignment(DependencyProperty property, object value) { Dependency = property; Value = value; }
    }

    public class StyleFactoryAssignmentDictionary : Dictionary<object, Assignment>
    {
        public new void Add(object compare, Assignment assignment) =>
            base.Add(compare, assignment);

        public new Assignment this[object compare]
        {
            get => base[compare];
            set => base[compare] = value;
        }
    }
}
