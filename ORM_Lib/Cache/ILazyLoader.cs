using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace ORM_Lib.Cache
{
    // CallerMemberName is a CompilerService that hands you the name of the method that was called
    // ref means passing by reference so you can also set the variable to a new value (by reference not value reference)
    public interface ILazyLoader
    {
        ICollection<T> Load<T>(object poco, ref ICollection<T> loadTo, [CallerMemberName] string name = "");
        T Load<T>(object poco, ref T loadTo, [CallerMemberName] string name = "");

        bool ShouldLoad<T>(T loadTo);
    }
}
