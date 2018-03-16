using System.Collections.ObjectModel;
using System.Linq;

namespace Abstractions.Models
{
    public class ObservableGroupCollection<S, T> : ObservableCollection<T>
    {
        public ObservableGroupCollection(IGrouping<S, T> group)
            : base(group) => Key = group.Key;

        public S Key { get; }
    }
}