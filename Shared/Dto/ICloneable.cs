namespace Order.Shared.Dto
{
    public interface ICloneable<T>
    {
        public T Clone();
    }
}
