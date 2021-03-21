namespace API.BizLogic
{
	public interface IFactory<T>
	{
		T Create();
	}
}
