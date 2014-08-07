namespace Oragon.Architecture.ApplicationHosting
{
	public interface IContainerFactory<T>
	{
		#region Public Methods

		T CreateContainer();

		#endregion Public Methods
	}
}