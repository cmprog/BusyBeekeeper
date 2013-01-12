using Microsoft.Xna.Framework;
using BusyBeekeeper.Data;
using BusyBeekeeper.Core;

namespace BusyBeekeeper
{
    internal interface IGameScreenManager
    {
        Game Game { get; }
        BeeWorldManager BeeWorldManager { get; }
        Player Player { get; }
        IGameScreen CurrentScreen { get; }

        void TransitionTo(IGameScreen gameScreen);
    }
}
