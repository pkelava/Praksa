using System;
using System.Collections.Generic;
using System.Text;

namespace ForumLibrary
{
    public interface IThreadsPosts : IPosts
    {
        string Title { get; set; }
        void PrintComments();
    }
}
