
Ext.define('OragonAppServerManagement.view.ServerExplorerTree', {
	extend: 'Ext.tree.Panel',
	alias: 'view.ServerExplorerTree',
	region: 'west',

	//controller: 'controller.ServerExplorerTree',

	floatable: true,
	split: true,
	closable: true,
	autoDestroy: false,
	closeAction: 'hide',
	collapsible: true,
	width: 300,
	minWidth: 200,
	title: 'Application Server Explorer',
	iconCls: 'AppIcons-sitemap-color',
	tools: [
				{
					type: 'refresh',
					callback: function () {

					}
				},
				{
					type: 'search',
					callback: function () {

					}
				}

	],
	store: Ext.create('Ext.data.TreeStore', {
		proxy: {
			type: 'ajax',
			url: '/api/ApplicationServerExplorerTree/GetNodes/'
		},
		root: {
			text: 'Application Server Explorer',
			id: 'root',
			expanded: true
		}
	}),

	

	initComponent: function () {


		//var eventoMenuContexto = "itemcontextmenu";
		//this.on(eventoMenuContexto, function (sender, record, element, index, eventInfo, options) {
		//	eventInfo.stopEvent();
		//	var treeSelectedRecord = record;

		//	if (Ext.isEmpty(record.data.menuItems))
		//		return;

		//	var contextMenu = Ext.create('Ext.menu.Menu', {
		//		items: Enumerable.from(record.data.menuItems).select(function (menuItem) {

		//			if (menuItem.actionConfirmation == null) {
		//				menuItem.handler = function (widget, event) {
		//					Oragon.Architecture.Scripting.TreeViewManager.defaultMenuItemHandlers.handlerWithoutConfirmation(record.data, menuItem);
		//				}
		//			}
		//			else {
		//				menuItem.handler = function (widget, event) {
		//					Oragon.Architecture.Scripting.TreeViewManager.defaultMenuItemHandlers.handlerWithConfirmation(record.data, menuItem);
		//				}
		//			}

		//			return menuItem;
		//		}).toArray()

		//	});//Ext.create('Ext.menu.Menu'

		//	contextMenu.showAt(eventInfo.getXY());
		//});

		this.callParent();
	}
});