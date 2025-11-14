using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openGL2.Shaders.ShaderComAndElements
{
    public class CustomLayout : LayoutBase
    {
        string custom;
        public CustomLayout(string customString)
        {
            custom = customString;
        }
        public override string GetLayoutString()
        {
            return custom;
        }

      

    }
}
