using Topshelf;

namespace Oragon.Architecture.ApplicationHosting
{
	public class Program
	{
		#region Public Methods

		public static void Main(string[] args)
		{
			HostProcessRunner host = new HostProcessRunner(args);
			TopshelfExitCode exitCode = host.Run();
			System.Environment.Exit((int)exitCode);
		}

		#endregion Public Methods
	}
}