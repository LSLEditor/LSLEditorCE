using System;
using System.Collections.Generic;
using System.Text;

namespace LSLEditor.Helpers
{
    class OutlineHelper
    {
        public KeyWordInfo info;
        public int line;
        public OutlineHelper(KeyWordInfo i, int l)
        {
            this.info = i;
            this.line = l;
        }
    }
}
