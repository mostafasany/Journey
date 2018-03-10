using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Abstractions.Models
{
    public class ObservableGroupCollection<S, T> : ObservableCollection<T>
    {
        private readonly S _key;

        public ObservableGroupCollection(IGrouping<S, T> group)
            : base(group)
        {
            _key = group.Key;
        }

        public S Key
        {
            get { return _key; }
        }
    }

    public class ObservableGroupCollection<T> : ObservableCollection<T>
    {
        public ObservableGroupCollection(string key, List<T> items) : base(items)
        {
            Key = key;
        }

        public string Key { get; set; }

    }
}
