using System;
using System.Collections.Generic;
using System.Text;

namespace ForumLibrary
{
    public interface IPosts 
    { 

        string PostCreator { get;}
        DateTime TimeOfCreation { get; }
        string Context { get; }


    }
}
