using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Pattern.EF.DataContext
{
    public interface IDataContext
    {
        int SaveChange();      
    }
}
