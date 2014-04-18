using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.ComplexType
{
    public interface IComplexTypeService
    {
        ParentEntity ComplexTypeTest(ParentEntity requestEntitie);        
    }
}
