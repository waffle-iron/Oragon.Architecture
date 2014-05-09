Ext.define('OragonAppServerManagement.controller.ServerExplorerTreeController', {
	extend: 'Ext.app.Controller',
	alias: 'controller.ServerExplorerTreeController',

	//config: {
	//	control: {
	//		'#ServerExplorerTree': {
	//			itemcontextmenu: this.onItemContextMenu
	//		}
	//	}
	//},


	init: function () {
		
		radio('ServerExplorerTree|ItemContextMenu').subscribe(this.onItemContextMenu);
		radio('ContextMenu|Action|Click').subscribe(this.onContextMenuClick);
		radio('HOST|REGISTERED').subscribe(this.onHostRegistered);
		radio('HOST|UNREGISTERED').subscribe(this.onHostUnregistered);
	},	


	onLaunch: function (application) {
		
	},

	onHostRegistered: function (broadcastData)
	{
		//var idHost = Ext.String.format("/Host/{0}/", broadcastData.record.ContentID);
		//var serverExplorerTree = Ext.getCmp('ServerExplorerTree');
		//var serverExplorerTreeStore = serverExplorerTree.getStore();
		//var node = serverExplorerTreeStore.getNodeById(idHost);
		//if (Ext.isEmpty(node) == false)
		//	serverExplorerTreeStore.remove(node);
	},

	onHostUnregistered: function (broadcastData) {
		var idHost = Ext.String.format("/Host/{0}/", broadcastData.record.ContentID);
		var serverExplorerTree = Ext.getCmp('ServerExplorerTree');
		var serverExplorerTreeStore = serverExplorerTree.getStore();
		var node = serverExplorerTreeStore.getNodeById(idHost);
		if (Ext.isEmpty(node)==false)
			serverExplorerTreeStore.remove(node);

	},

	onItemContextMenu: function (broadcastData) {

		broadcastData.eventInfo.stopEvent();
		if (Ext.isEmpty(broadcastData.record.data.menuItems))
			return;
		var currentContextMenu = Ext.create('Ext.menu.Menu', {
			items: Enumerable.from(broadcastData.record.data.menuItems).select(function (menuItem) {
				if (Ext.isEmpty(  menuItem.actionConfirmation) == false) {
					menuItem.handler = function (widget, eventInfo) {
						var messageBoxConfig = {};
						messageBoxConfig.title = menuItem.actionConfirmation.title,
						messageBoxConfig.msg = menuItem.actionConfirmation.text,
						messageBoxConfig.icon = eval("(Ext.MessageBox." + menuItem.actionConfirmation.icon + ")");
						messageBoxConfig.buttons = eval("(Ext.MessageBox." + menuItem.actionConfirmation.buttons + ")");
						messageBoxConfig.fn = function (result) {
							var performActionOnClickIn = menuItem.actionConfirmation.performActionOnClickIn.toLowerCase();
							if (result == performActionOnClickIn) {								
								radio('ContextMenu|Action|Click').broadcast({
									sender: menuItem,
									record: broadcastData.record,
									actionRoute: menuItem.actionRoute,
									eventInfo: eventInfo
								});
							}
						};
						Ext.MessageBox.show(messageBoxConfig);
					}
				}
				else {
					menuItem.handler = function (widget, eventInfo) {
						radio('ContextMenu|Action|Click').broadcast({
							sender: menuItem,
							record: broadcastData.record,
							actionRoute: menuItem.actionRoute,
							eventInfo: eventInfo
						});
					}
				}
				return menuItem;
			}).toArray()
		});//Ext.create('Ext.menu.Menu'
		currentContextMenu.showAt(broadcastData.eventInfo.getXY());		
	},

	onContextMenuClick: function (broadcastData) {
		switch (broadcastData.sender.actionRoute) {
			case 'Node|Refresh':
				broadcastData.record.store.load({ node: broadcastData.record });
				break;
			case 'Repository|Add':
				break;
			case 'Application|Stop':
				break;
		}
	}
});