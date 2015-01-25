namespace Oragon.Architecture.Networking.Common
{
	public abstract class MessageProperty
	{
		#region Public Properties

		public string Name { get; set; }

		#endregion Public Properties
	}

	public class MessageProperty<T> : MessageProperty
	{
		#region Public Properties

		public T Data { get; set; }

		#endregion Public Properties
	}
}