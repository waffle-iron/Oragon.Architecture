var Oragon = Oragon || {};
Oragon.Architecture = Oragon.Architecture || {};
Oragon.Architecture.Scripting = Oragon.Architecture.Scripting || {};

Oragon.Architecture.Scripting.TreeViewManager = {
	defaultMenuItemHandlers: {

		handlerWithConfirmation: function (data, menuItem) {
			var messageBoxConfig = {};
			messageBoxConfig.title = menuItem.actionConfirmation.title,
			messageBoxConfig.msg = menuItem.actionConfirmation.text,
			messageBoxConfig.icon = eval("(Ext.MessageBox." + menuItem.actionConfirmation.icon + ")");
			messageBoxConfig.buttons = eval("(Ext.MessageBox." + menuItem.actionConfirmation.buttons + ")");
			messageBoxConfig.fn = function (result) {
				var performActionOnClickIn = menuItem.actionConfirmation.performActionOnClickIn.toLowerCase();
				if (result == performActionOnClickIn) {
					eval(menuItem.handlerFunction + "(data, menuItem)");
				}
			};
			Ext.MessageBox.show(messageBoxConfig);
		},
		handlerWithoutConfirmation: function (data, menuItem) {

		},
	}

};
