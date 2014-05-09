Ext.define('OragonAppServerManagement.view.HomeSouthRegion', {
	extend: 'Ext.panel.Panel',
	alias: 'view.HomeSouthRegion',
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

});