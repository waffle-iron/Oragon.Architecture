Ext.require(['*']);
Ext.onReady(function () {

	var northRegion = {
		region: 'north',
		collapsible: false,
		title: 'Oragon Architecture Application Server',
		header: false,
		border: false,
		titleAlign: 'center',
		split: false,
		height: 28,
		tbar: [
			'Oragon Architecture',
			{
				text: 'Application Server',
				iconCls: 'AppIcons-application-osx',
			},
			{
				text: 'View',
				iconCls: 'AppIcons-cog',
				menu: [
					Ext.create('Ext.Action', {
						text: 'View/Hide Notification Center',
						iconCls: 'AppIcons-application-view-list', //'AppIcons-application-put',
						handler: function () {
							var panel = Ext.getCmp("NotificationCenter");
							if (panel.isHidden()) {
								panel.show();
							}
							else {
								panel.close();
							}
						}
					}),
					Ext.create('Ext.Action', {
						text: 'View/Hide Application Server Explorer',
						iconCls: 'AppIcons-sitemap-color',
						handler: function () {
							var panel = Ext.getCmp("ApplicationServerExplorer");
							if (panel.isHidden()) {
								panel.show();
							}
							else {
								panel.close();
							}
						}
					})
				]
			},
			{
				text: 'Help',
				iconCls: 'AppIcons-help',
				menu: [
					Ext.create('Ext.Action', {
						text: 'Wiki @ GitHub',
						iconCls: 'AppIcons-report',
						handler: function () {
							window.open("https://github.com/luizcarlosfaria/Oragon.Architecture/wiki/Oragon-Architecture", "_blank");
						}
					}),
					Ext.create('Ext.Action', {
						text: 'Source Code @ GitHub',
						iconCls: 'AppIcons-script-code',
						handler: function () {
							window.open("https://github.com/luizcarlosfaria/Oragon.Architecture", "_blank");
						}
					}),
					Ext.create('Ext.Action', {
						text: 'Sugestions and Feedback @ LuizCarlosFaria.net',
						iconCls: 'AppIcons-information',
						handler: function () {
							window.open("http://translate.google.com/translate?sl=pt&tl=en&js=y&prev=_t&hl=pt-BR&ie=UTF-8&u=http%3A%2F%2Fluizcarlosfaria.net%2Fcontato%2F&edit-text=&act=url", "_blank");
						}
					})
				]
			}
		]
	};

	var centerRegion = Ext.create('Ext.tab.Panel', {
		id: 'centerRegionTabPanel',
		region: 'center',
		width: 400,
		height: 400,
		renderTo: document.body,
		items: [{
			title: 'Dashboard',
			closable: false,
		}, {
			title: 'Bar',
			closable: true,
			tabConfig: {
				title: 'Custom Title',
				tooltip: 'A button tooltip'
			}
		}]
	});

	var appServerExplorerStore = Ext.create('Ext.data.TreeStore', {
		proxy: {
			type: 'ajax',
			url: '/api/ApplicationServerExplorerTree/'
		},
		root: {
			text: 'Application Server Explorer',
			id: 'root',
			expanded: true
		}
	});

	var westRegion = Ext.create('Ext.tree.Panel', {
		region: 'west',
		id: 'ApplicationServerExplorer',
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
		store: appServerExplorerStore
	});

	var southRegion = {
		region: 'south',
		iconCls: 'AppIcons-application-view-list',
		id: 'NotificationCenter',
		height: 100,
		split: true,
		closable: true,
		autoDestroy: false,
		closeAction: 'hide',
		collapsible: true,
		title: 'Notification Center',
		minHeight: 60,
		weight: -100,
		items: [
			{
				xtype: 'textareafield',
				grow: true,
				id: 'NotificationCenterTextArea',
				//anchor: '100%',
				border: false,
				readOnly: true,
				editable: false,
				style: {
					width: '100%',
					height: '100%'
				},
				value: ''
			}

		]
	};

	var viewport = Ext.create('Ext.Viewport', {
		layout: {
			type: 'border',
			padding: 5
		},
		defaults: {
			split: true
		},
		items: [
			northRegion,
			centerRegion,
			westRegion,
			southRegion
		],
		listeners: {
			'afterrender': function () {
				setInterval(function () {
					Ext.Ajax.request({
						url: '/api/Message/GetMessages/',
						timeout: 60000,
						params: {},
						success: function (response) {
							var lines = Ext.JSON.decode(response.responseText, true);
							var notificationCenterTextArea = Ext.getCmp("NotificationCenterTextArea");
							Ext.Array.forEach(lines, function (line, index) {
								notificationCenterTextArea.setValue(line + "\r\n" + notificationCenterTextArea.getValue());
							});							
						}
					});
				}, 1000);
			}
		}
	});

});