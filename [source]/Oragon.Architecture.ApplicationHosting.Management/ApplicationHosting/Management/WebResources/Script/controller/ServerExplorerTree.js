Ext.define('OragonAppServerManagement.controller.ServerExplorerTree', {
	extend: 'Ext.app.Controller',
	alias: 'controller.ServerExplorerTree',

	//config: {
	//	control: {
	//		'#ServerExplorerTree': {
	//			itemcontextmenu: this.onItemContextMenu
	//		}
	//	}
	//},

	init: function () { },

	onLaunch: function (application) {
		var serverExplorerTree = Ext.getCmp('ServerExplorerTree')
		serverExplorerTree.on('itemcontextmenu', this.onItemContextMenu);
	},

	onItemContextMenu: function (sender, record, element, index, eventInfo, options) {
		eventInfo.stopEvent();
		
		var treeSelectedRecord = record;
		if (Ext.isEmpty(record.data.menuItems))
			return;
		var currentContextMenu = Ext.create('Ext.menu.Menu', {
			items: Enumerable.from(record.data.menuItems).select(function (menuItem) {
				if (menuItem.actionConfirmation == null) {
					menuItem.handler = function (widget, event) {
						Oragon.Architecture.Scripting.TreeViewManager.defaultMenuItemHandlers.handlerWithoutConfirmation(record.data, menuItem);
					}
				}
				else {
					menuItem.handler = function (widget, event) {
						Oragon.Architecture.Scripting.TreeViewManager.defaultMenuItemHandlers.handlerWithConfirmation(record.data, menuItem);
					}
				}
				console.log(menuItem);
				return menuItem;
			}).toArray()
		});//Ext.create('Ext.menu.Menu'
		currentContextMenu.showAt(eventInfo.getXY());		
	}
});