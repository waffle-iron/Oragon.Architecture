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
					id: 'btnViewNotification',
					iconCls: 'AppIcons-application-view-list', //'AppIcons-application-put',
					purpose: 'show-hide|panel',
					target :  'NotificationCenter',
					handler: function () {
						radio('MainMenu|Action|Click').broadcast({ sender: this });
					}
				}),
				Ext.create('Ext.Action', {
					text: 'View/Hide Application Server Explorer',
					iconCls: 'AppIcons-sitemap-color',
					purpose: 'show-hide|panel',
					target :  'ServerExplorerTree',
					handler: function () {
						radio('MainMenu|Action|Click').broadcast({ sender: this });
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
					purpose: 'openLink|_blank',
					target: 'https://github.com/luizcarlosfaria/Oragon.Architecture/wiki/Oragon-Architecture',
					handler: function () {
						radio('MainMenu|Action|Click').broadcast({ sender: this });
					}
				}),
				Ext.create('Ext.Action', {
					text: 'Source Code @ GitHub',
					iconCls: 'AppIcons-script-code',
					purpose: 'openLink|_blank',
					target: 'https://github.com/luizcarlosfaria/Oragon.Architecture',
					handler: function () {
						radio('MainMenu|Action|Click').broadcast({ sender: this });
					}
				}),
				Ext.create('Ext.Action', {
					text: 'Sugestions and Feedback @ LuizCarlosFaria.net',
					iconCls: 'AppIcons-information',
					purpose: 'openLink|_blank',
					target: 'http://translate.google.com/translate?sl=pt&tl=en&js=y&prev=_t&hl=pt-BR&ie=UTF-8&u=http%3A%2F%2Fluizcarlosfaria.net%2Fcontato%2F&edit-text=&act=url',
					handler: function () {
						radio('MainMenu|Action|Click').broadcast({ sender: this });
					}
				})
			]
		}
	]
});