
using System;

namespace BuccaneerBreaker
{
    public class Delegates
    {
        public delegate void CannonBallLaunchedEventHandler(object source, EventArgs args);
        public delegate void BurstEventHandler(object source, EventArgs args);
    }
}