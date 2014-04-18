using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Security.Authorization.Profile
{
	[Serializable]
	public class AuthorizationProfile
	{
		private volatile int _version;

		public AuthorizationProfile()
		{
			this.Actions = new List<Action>();
			this.isEditMode = false;
			this.Changed();
		}

		public int Version { get { return this._version; } }

		private List<Action> Actions { get; set; }

		private bool isEditMode; // { get; set; }

		public void BeginEdit()
		{
			this.isEditMode = true;
		}
		public void EndEdit()
		{
			this.isEditMode = false;
		}


		private Action GetAction(string actionName, bool thowIfNotFound = true)
		{
			if (string.IsNullOrEmpty(actionName))
				throw new ArgumentException("actionName");

			Action action = this.Actions.FirstOrDefault(it => it.ActionName.Trim().ToLower() == actionName.Trim().ToLower());
			if ((action == null) && (thowIfNotFound))
				throw new InvalidOperationException(string.Format("A ação '{0}' não foi encontrada no AuthorizationProfile", actionName));
			return action;
		}

		public bool IsEmpty
		{
			get
			{
				return (this.Actions.Count == 0);
			}
		}

		private void Changed()
		{
			this._version++;
		}

		public void Reset()
		{
			this.Actions.Clear();
			this.Changed();
		}

		public bool CanAccess(string actionName, IActionContextConvertible convertibleContext)
		{
			Action action = this.GetAction(actionName);
			return action.CanAccess(convertibleContext);
		}

		public List<ActionContext> GetValidContextsOf(string actionName, string typeName)
		{
			Action action = this.GetAction(actionName);
			List<ActionContext> returnValue = action.GetContextsOf(typeName);
			return returnValue;
		}

		public bool HasContextOf(string actionName = null, string typeName = null)
		{
			bool returnValue = this.Actions.Where(it =>
				(string.IsNullOrWhiteSpace(actionName) || it.ActionName == actionName)
				&&
				(it.GetContextsOf(typeName).Any())
			).Any();
			return returnValue;
		}

		public void Grant(string actionName, IActionContextConvertible convertibleContext)
		{
			Action action = this.GetAction(actionName);
			action.Grant(convertibleContext, this.isEditMode);
			this.Changed();
		}

		public void Grant(string actionName)
		{
			Action action = this.GetAction(actionName);
			action.FullAccess = true;
			this.Changed();
		}

		public void Revoke(string actionName, IActionContextConvertible convertibleContext)
		{
			Action action = this.GetAction(actionName);
			action.FullAccess = false;
			action.Revoke(convertibleContext);
			this.Changed();
		}

		public void CreateAction(string actionName)
		{
			Action action = this.GetAction(actionName, false);
			if (action == null)
			{
				this.Actions.Add(new Action(actionName));
				this.Changed();
			}
		}

		public void CleanActionContexts()
		{
			this.Actions.ForEach(it => it.RemoveDuplicatedContexts());
		}


	}
}
