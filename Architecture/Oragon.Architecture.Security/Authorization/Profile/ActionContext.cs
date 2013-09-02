using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.Security.Authorization.Profile
{
	[Serializable]
	public sealed class ActionContext
	{
		public string ID { get; set; }
		public string Type { get; set; }

		#region Equals/GetHashCode


		public bool EqualsTo(ActionContext other)
		{
			if (other == null ||  Object.ReferenceEquals(this, other))
				return true;

			bool returnValue = ((this.ID == other.ID) && (this.Type == other.Type));
			return returnValue;
		}

		#endregion		

		public override string ToString()
		{
			return "{0}/{1}".Formatar(this.Type, this.ID);
		}

        public class ComparadorPorIdentificador : IEqualityComparer<ActionContext>
        {
            public bool Equals(ActionContext x, ActionContext y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                return (x.ID == y.ID && x.Type == y.Type);
            }

            public int GetHashCode(ActionContext obj)
            {
                return obj.ID.GetHashCode();
            }
        }
	}
}
