using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Security.Authorization.Profile
{
	[Serializable]
	public class Action
	{
		public string ActionName { get; set; }
		public bool FullAccess { get; set; }
		private List<ActionContext> ValidContexts { get; set; }

		public Action(string actionName)
		{
			this.ActionName = actionName;
			this.FullAccess = false;
			this.ValidContexts = new List<ActionContext>();
		}

		public bool CanAccess(IActionContextConvertible convertibleContext)
		{
			ActionContext context = convertibleContext == null? null : convertibleContext.ToActionContext();
			bool returnValue = this.CanAccess(context);
			return returnValue;
		}

		public bool CanAccess(ActionContext context)
		{
			bool returnValue = this.FullAccess;
			if (returnValue == false)
			{
				if (context == null)
					returnValue = this.ValidContexts.Any();
				else
					returnValue = this.ExistsInContext(context);
			}
			return returnValue;
		}



        internal void Grant(IActionContextConvertible convertibleContext, bool isEditMode = false)
		{
			ActionContext context = convertibleContext.ToActionContext();
            if (isEditMode || (this.ExistsInContext(context) == false))
            {
                this.ValidContexts.Add(context);
            }
		}

		internal void Revoke(IActionContextConvertible convertibleContext)
		{
			ActionContext context = convertibleContext.ToActionContext();
			if (this.ExistsInContext(context))
			{
				ActionContext contextInList = this.ValidContexts.Single(it => it.EqualsTo(context));
				this.ValidContexts.Remove(contextInList);
			}
		}


		internal bool ExistsInContext(ActionContext context)
		{
			bool returnValue = this.ValidContexts.Any(it => it.EqualsTo(context));
			return returnValue;
		}

		internal List<ActionContext> GetContextsOf(string typeName)
		{
			List<ActionContext> returnValue = this.ValidContexts.Where(it => string.IsNullOrWhiteSpace(typeName) || it.Type == typeName).ToList();
			return returnValue;
		}

        internal void RemoveDuplicatedContexts()
        {
            this.ValidContexts = this.ValidContexts.Distinct(new ActionContext.ComparadorPorIdentificador()).ToList();
        }
	}

}
