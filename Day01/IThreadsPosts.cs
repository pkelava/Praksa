using System;
using System.Collections.Generic;
using System.Text;

namespace ForumLibrary
{
    public interface IThreadsPosts : IPosts
    {
        void PrintComments();
    }
}
