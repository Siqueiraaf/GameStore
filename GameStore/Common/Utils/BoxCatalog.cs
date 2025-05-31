using GameStore.PackingService.Core.Models;

namespace GameStore.PackingService.Common.Utils;

public static class BoxCatalog
{
    public static List<Box> BoxesAvailable =
    [
        new Box { Id = 1, Type = "Caixa 1", Height = 30, Width = 40, Length = 80 },
        new Box { Id = 2, Type = "Caixa 2", Height = 80, Width = 50, Length = 40 },
        new Box { Id = 3, Type = "Caixa 3", Height = 50, Width = 80, Length = 60 },
    ];
}
