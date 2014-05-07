﻿var HomeController = {

	init: function()
	{
		Ext.setGlyphFontFamily('Pictos');
	},

	beforeLoad: function () {

	},

	afterLoad: function()
	{
		HomeController.setupTreeViewContextMenu();
	},

	setupTreeViewContextMenu: function()
	{
		var applicationServerExplorer = Ext.getCmp('ApplicationServerExplorer');
		applicationServerExplorer.on(eventoMenuContexto, function (sender, record, element, index, eventInfo, options) {
			eventInfo.stopEvent();
			var treeSelectedRecord = record;

			if (Ext.isEmpty(record.data.menuItems))
				return;		

			var contextMenu = Ext.create('Ext.menu.Menu', {
				items: Enumerable.from(record.data.menuItems).select(function (menuItem) { 
				
					if(menuItem.actionConfirmation == null)
					{
						menuItem.handler = function (widget, event) {
							Oragon.Architecture.Scripting.TreeViewManager.defaultMenuItemHandlers.handlerWithoutConfirmation(record.data, menuItem);
						}
					}
					else
					{
						menuItem.handler = function (widget, event) {
							Oragon.Architecture.Scripting.TreeViewManager.defaultMenuItemHandlers.handlerWithConfirmation(record.data, menuItem);
						}
					}

					return menuItem;
				}).toArray()

			});//Ext.create('Ext.menu.Menu'

			contextMenu.showAt(eventInfo.getXY());
		});
	},

	refreshTreeNode: function (data, menuItem) {
		

	},

	stopApplication: function (data, menuItem) {
		

	}


}


