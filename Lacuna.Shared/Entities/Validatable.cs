namespace Lacuna.Shared.Entities
{
    public abstract class Validatable
    {
        protected bool isInvalid;
        public bool IsValid() => !isInvalid;
        public abstract void Validate();
    }
}