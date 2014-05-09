Ext.application({
	name: 'OragonAppServerManagement',

	//models: ['Post', 'Comment'],

	controllers: ['ServerExplorerTree'],

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
				Ext.create('OragonAppServerManagement.view.HomeSouthRegion', {})
			]
		});



		Oragon.Architecture.Scripting.TimerManager.run({
			intervalInSeconds: 0,
			autoReschedule: true,
			fn: function (timerConfig) {
				timerConfig.intervalInSeconds = 15;

				Oragon.Architecture.Scripting.AjaxManager.sendAndReceiveUsingJson({
					waiting: {
						title: "Aguardando...",
						message: "Aguarde a execução da tarefa!"
					},
					//urlToSend: '/api/Notification/GetMessages/?clientID=' + clientID,
					urlToSend: '/api/Notification/GetMessages/' + clientID,
					params: {
						clientID: clientID
					},
					listeners: {
						success: function (data) {
							var notificationCenterTextArea = Ext.getCmp("NotificationCenterTextArea");
							Enumerable.from(data).forEach(function (itemOfData) {
								notificationCenterTextArea.setValue(
									itemOfData.Date + "\t" +
									itemOfData.Message + "\t" +
									itemOfData.MessageType + "\t" +
									"\r\n" +
									notificationCenterTextArea.getValue()
								);
							});

						},
						failure: function (resultInfo) {

							console.log(resultInfo);
						},
					}
				});
			}
		});

	}
});