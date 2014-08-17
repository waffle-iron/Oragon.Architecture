namespace Oragon.Architecture.ApplicationHosting
{
	public interface IContainerFactory<out T>
	{
		#region Public Methods

		T CreateContainer();

		#endregion Public Methods
	}
}