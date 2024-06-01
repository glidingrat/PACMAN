using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacMan
{
    internal interface IDAO<T> where T : IBaseClass
    {
        IEnumerable<T> GetAll();
        void Save(T element);
    }
}
