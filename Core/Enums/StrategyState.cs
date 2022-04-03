using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public enum StrategyState
    {
        DoNothing, // Valeur par défaut, ne pas déplacer
        Long,
        Short,
        ClosePositions,
    }
}
