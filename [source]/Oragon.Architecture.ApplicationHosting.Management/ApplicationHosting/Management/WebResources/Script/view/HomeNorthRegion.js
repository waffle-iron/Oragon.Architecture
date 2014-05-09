Ext.define('OragonAppServerManagement.view.HomeNorthRegion', {
	extend: 'Ext.panel.Panel',
	alias: 'view.HomeNorthRegion',
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

});