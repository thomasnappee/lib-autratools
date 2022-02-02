using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Interfaces
{
    public interface IAction
    {
        int Id { get; set; }
        int Code { get; }
        double Reward { get; }
    }
}
