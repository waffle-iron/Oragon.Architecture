Ext.define('OragonAppServerManagement.controller.MenuController', {
	extend: 'Ext.app.Controller',
	alias: 'controller.MenuController',

	init: function () {

		radio('MainMenu|Action|Click').subscribe(this.onMainMenuActionClick);
		
	},

	onLaunch: function (application) { },

	onMainMenuActionClick: function (broadcastData)
	{
		if (Ext.isEmpty(broadcastData.sender.initialConfig.purpose) == false)
		{
			switch (broadcastData.sender.initialConfig.purpose)
			{
				case 'show-hide|panel':
					var panel = Ext.getCmp(broadcastData.sender.initialConfig.target);
					if (panel.isHidden()) {
						panel.show();
					}
					else {
						panel.close();
					}
					break;
				case 'openLink|_blank':
					window.open(broadcastData.sender.initialConfig.target, "_blank");
					break;
			}
		}
	}

});