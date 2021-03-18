namespace Order.Shared.Dto
{
    public class ValueWrapperDto<T>
    {
        public T Value { get; set; }

        public ValueWrapperDto(T value) => Value = value;
    }
}
