﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.WindowsServices.Model
{
	public enum AccountType
	{
		NetworkService,
		LocalSystem,
		LocalService,
		Custom,
		Prompt
	}
}
