namespace Game.ModifiableParam
{
    public interface IParamModifier<T>
    {
        public int Priority { get; set; }
        public T Modify(T value);
    }
}