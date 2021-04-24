using System.Collections.Generic;
using System.Linq;

namespace Order.Shared.Dto
{
    public class CloneableList<T> : List<T>, ICloneable<CloneableList<T>> where T : ICloneable<T>
    {
        public CloneableList<T> Clone()
        {
            var newList = new CloneableList<T>();
            newList.AddRange(this.Select(e => e.Clone()));
            return newList;
        }
    }
}
