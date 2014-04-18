using System;
namespace Oragon.Architecture.Services
{
	public interface IService
	{
		string Name
		{
			get;
		}
		void Start();
		void Stop();
	}
}
