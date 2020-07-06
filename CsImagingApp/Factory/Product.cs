using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsImagingApp.Factory
{
    public abstract class Product
    {
        public abstract Bitmap Action(Bitmap src);
    }
}
