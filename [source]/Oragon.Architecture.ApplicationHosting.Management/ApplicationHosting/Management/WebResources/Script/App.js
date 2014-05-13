Ext.application({
	name: 'OragonAppServerManagement',

	//models: ['Post', 'Comment'],

	controllers: ['ServerExplorerTreeController', 'MenuController'],

	appFolder: '/dynRes/ApplicationHosting/Management/WebResources/Script',

	launch: function() {		
		Ext.create('Ext.container.Viewport', {
			layout: {
				type: 'border',
				padding: 5
			},
			defaults: {
				split: true
			},
			items: [
				Ext.create('OragonAppServerManagement.view.HomeNorthRegion', {}),
				Ext.create('OragonAppServerManagement.view.HomeCenterRegion', {}),
				Ext.create('OragonAppServerManagement.view.ServerExplorerTree', { id: 'ServerExplorerTree' }),
				Ext.create('OragonAppServerManagement.view.HomeSouthRegion', { id: 'NotificationCenter'})
			]
		});


		$.connection.hub.url = (hostUrl + "signalr");

		var NotificationHub = $.connection.NotificationHub;

		NotificationHub.client.receiveMessages = function (messages) {

			var notificationCenterTextArea = Ext.getCmp("NotificationCenterTextArea");
			Enumerable.from(messages).forEach(function (itemOfData) {
				notificationCenterTextArea.setValue(
					itemOfData.Date + "\t" +
					itemOfData.Message + "\t" +
					itemOfData.MessageType + "\t" +
					"\r\n" +
					notificationCenterTextArea.getValue()
				);

				radio(itemOfData.MessageType).broadcast({ sender: null, record: itemOfData });
			});

		};

		$.connection.hub.start().done(function () {
			NotificationHub.server.RegisterWebManagement(clientID);
		});



		//Oragon.Architecture.Scripting.TimerManager.run({
		//	intervalInSeconds: 0,
		//	fn: function (timerConfig) {
		//		Oragon.Architecture.Scripting.AjaxManager.sendAndReceiveUsingJson({
		//			waiting: 'NoWait',
		//			//urlToSend: '/api/Notification/GetMessages/?clientID=' + clientID,
		//			urlToSend: '/api/Notification/GetMessages/' + clientID,
		//			params: {
		//				clientID: clientID
		//			},
		//			listeners: {
		//				success: function (data) {
		//					var notificationCenterTextArea = Ext.getCmp("NotificationCenterTextArea");
		//					Enumerable.from(data).forEach(function (itemOfData) {
		//						notificationCenterTextArea.setValue(
		//							itemOfData.Date + "\t" +
		//							itemOfData.Message + "\t" +
		//							itemOfData.MessageType + "\t" +
		//							"\r\n" +
		//							notificationCenterTextArea.getValue()
		//						);

		//						radio(itemOfData.MessageType).broadcast({ sender: null, record: itemOfData });
		//					});
		//					timerConfig.intervalInSeconds = 5;
		//					timerConfig.ScheduleNext();
		//				},
		//				failure: function (resultInfo) {
		//					timerConfig.intervalInSeconds = 30;
		//					timerConfig.ScheduleNext();
		//				},
		//			}
		//		});
		//	}
		//});

	}
});


$(function () {
	
	


});