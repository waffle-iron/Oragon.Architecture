Ext.define('OragonAppServerManagement.view.HomeCenterRegion', {
	extend: 'Ext.tab.Panel',
	alias: 'view.HomeCenterRegion',
	region: 'center',

	id: 'centerRegionTabPanel',
	width: 400,
	height: 400,
	items: [{
		title: 'Dashboard',
		closable: false,
	}, {
		title: 'Bar',
		glyph: 42,
		closable: true,
		tabConfig: {
			title: 'Custom Title',
			tooltip: 'A button tooltip'
		}
	}]
});