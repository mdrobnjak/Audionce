using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer
{
    public interface IVFX
    {
        void PreDraw();

        void Draw();

        void PostDraw();

        void Trigger1();

        void Trigger2();

        void Trigger3();
    }
}
