var HomeController = {

	init: function()
	{
		
	},

	beforeLoad: function () {

	},

	afterLoad: function()
	{
		HomeController.setupTreeViewContextMenu();

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
					urlToSend: '/api/Notification/GetMessages/'+clientID,
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
	},

	setupTreeViewContextMenu: function()
	{
		var applicationServerExplorer = Ext.getCmp('ApplicationServerExplorer');
		applicationServerExplorer.on(eventoMenuContexto, function (sender, record, element, index, eventInfo, options) {
			eventInfo.stopEvent();
			var treeSelectedRecord = record;

			if (Ext.isEmpty(record.data.menuItems))
				return;		

			var contextMenu = Ext.create('Ext.menu.Menu', {
				items: Enumerable.from(record.data.menuItems).select(function (menuItem) { 
				
					if(menuItem.actionConfirmation == null)
					{
						menuItem.handler = function (widget, event) {
							Oragon.Architecture.Scripting.TreeViewManager.defaultMenuItemHandlers.handlerWithoutConfirmation(record.data, menuItem);
						}
					}
					else
					{
						menuItem.handler = function (widget, event) {
							Oragon.Architecture.Scripting.TreeViewManager.defaultMenuItemHandlers.handlerWithConfirmation(record.data, menuItem);
						}
					}

					return menuItem;
				}).toArray()

			});//Ext.create('Ext.menu.Menu'

			contextMenu.showAt(eventInfo.getXY());
		});
	},

	refreshTreeNode: function (data, menuItem) {
		

	},

	stopApplication: function (data, menuItem) {
		

	}


}


