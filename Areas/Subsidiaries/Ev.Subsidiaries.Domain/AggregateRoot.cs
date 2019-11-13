namespace Ev.Subsidiaries.Domain
{
    public abstract class AggregateRoot<TIdentifier>
    {
        public TIdentifier Identifier { get; }

        protected AggregateRoot(TIdentifier identifier)
        {
            Identifier = identifier;
        }
    }
}