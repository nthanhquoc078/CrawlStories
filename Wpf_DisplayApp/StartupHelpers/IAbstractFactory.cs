namespace Wpf_DisplayApp.StartupHelpers
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }
}