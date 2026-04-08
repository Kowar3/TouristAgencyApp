using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Patterns.Memento
{
    public interface IMemento<T>
    {
        T GetState();
    }
}
