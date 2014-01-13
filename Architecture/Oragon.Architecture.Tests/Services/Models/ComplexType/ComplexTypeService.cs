using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.ComplexType
{
    public class ComplexTypeService : IComplexTypeService
    {
        public ParentEntity ComplexTypeTest(ParentEntity requestEntitie)
        {
            return requestEntitie;
        }
    }
}
