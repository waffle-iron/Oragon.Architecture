var Oragon = Oragon || {};
Oragon.Architecture = Oragon.Architecture || {};
Oragon.Architecture.Scripting = Oragon.Architecture.Scripting || {};

Oragon.Architecture.Scripting.WaitingManager = {
	init: function(){
		radio('AjaxManager:StartRequest').subscribe(function (configObject) {
			if (configObject.waiting != 'NoWait')
				configObject.waitingRef = Oragon.Architecture.Scripting.WaitingManager.showWaiting(configObject.waiting, configObject.timeout);
		});

		radio('AjaxManager:EndRequest').subscribe(function (resultInfo) {
			if (resultInfo.configObject.waiting != 'NoWait')
				Oragon.Architecture.Scripting.WaitingManager.stopWaiting(resultInfo.configObject.waitingRef);
		});
	},

	showWaiting: function (waitingMessage, duration) {
		var waitingRef = {};
		waitingRef.win = Ext.create('widget.window', {
			id: Ext.id(),
			title: waitingMessage.title,
			closable: false,
			modal: true,
			width: 300,
			height: 70,
			layout: {
				type: 'vbox',
				padding: '5',
				align: 'stretch'
			},
			bodyStyle: 'padding: 5px;',
			items: [
				Ext.create('Ext.ProgressBar', {
					id: Ext.id(),
					width: 300
				})
			]
		});
		waitingRef.win.show();
		waitingRef.progressBarWaiting = waitingRef.win.items.items[0];

		waitingRef.progressBarWaiting.wait({
			interval: 300,
			duration: Ext.Ajax.timeout,
			text: waitingMessage.message,
			increment: 10,
			fn: function () {
				Oragon.Architecture.Scripting.WaitingManager.stopWaiting(waitingRef);
			}
		});
		return waitingRef;
	},
	stopWaiting: function (waitingRef) {
		if (Ext.isEmpty(waitingRef) == false) {
			waitingRef.progressBarWaiting.destroy();
			waitingRef.win.close();
		}
	}
};
Oragon.Architecture.Scripting.WaitingManager.init();