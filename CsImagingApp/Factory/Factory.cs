using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsImagingApp.Model;

namespace CsImagingApp.Factory
{
    public abstract class AbstractFactory
    {
        public abstract Product Create(EnumProcMode mode);
    }
}
