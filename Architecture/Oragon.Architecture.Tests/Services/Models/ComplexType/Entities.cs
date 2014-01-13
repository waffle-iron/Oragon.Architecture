using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests.Services.Models.ComplexType
{
    public class ParentEntity
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IList<Pet> Pets { get; set; }

        public Exception ExceptionToSerialize { get; set; }
    }

    public class Pet
    {
        public string Name { get; set; }
    }
    public class Dog : Pet
    {
    }
    public class Cockatiel : Pet
    {
    }
}
