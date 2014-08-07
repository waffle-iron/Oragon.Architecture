var Oragon = Oragon || {};
Oragon.Architecture = Oragon.Architecture || {};
Oragon.Architecture.Scripting = Oragon.Architecture.Scripting || {};

Oragon.Architecture.Scripting.Reflection = {
	init: function () { },

	getValue: function (dataObject, expression) {
		var returnValue = null;
		var canContinue = true;
		if (!Ext.isEmpty(dataObject)) {
			var parts = (new String(expression)).split('.');
			if (parts.length > 1) {
				var currentPartExpression = "dataObject";
				for (partIndex = 0; partIndex < parts.length; partIndex++) {
					currentPartExpression += "." + parts[partIndex];
					var tmpResult = eval(currentPartExpression);
					if (Ext.isEmpty(tmpResult)) {
						canContinue = false;
						break;
					}
				}
			}
			if (canContinue) {
				var evalExpression = Ext.String.format("dataObject.{0}", expression);
				returnValue = eval(evalExpression);
			}
		}
		return returnValue;
	},

	setValue: function (dataObject, expression, valueToSet) {
		var evalExpression = Ext.String.format("dataObject.{0} = valueToSet", expression);
		eval(evalExpression);
	}
};

Oragon.Architecture.Scripting.Reflection.init();