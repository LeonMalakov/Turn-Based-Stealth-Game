namespace Stls.Constants
{
    public static class ManAnimatorController
    {
        public static class Parameters
        {
            public const string Move = nameof(Move);
            public const string Takedown = nameof(Takedown);
            public const string Die = nameof(Die);
            public const string TakedownIndex = nameof(TakedownIndex);
            public const string Attack = nameof(Attack);
        }

        public static class States
        {
            public const string Idle = nameof(Idle);
            public const string GrabBody = nameof(GrabBody);
            public const string GrabbedAsBody = nameof(GrabbedAsBody);
            public const string DropBody = nameof(DropBody);
            public const string DroppedAsBody = nameof(DroppedAsBody);
        }
    }
}