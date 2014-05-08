var Oragon = Oragon || {};
Oragon.Architecture = Oragon.Architecture || {};
Oragon.Architecture.Scripting = Oragon.Architecture.Scripting || {};

Oragon.Architecture.Scripting.AjaxManager = {

	init: function () { 
		Ext.Ajax.timeout = 9 * 60 * 1000; //miliseconds
		Ext.Ajax.limit = 9999999; //miliseconds
	},

	sendAndReceiveUsingJson: function (configObject) {
		/*
			var configObject = {
				waiting : {
					title: "Aguardando...",
					message: "Aguarde a execução da tarefa!"
				},
				urlToSend : '/api/Notification/GetMessages/?clientID=' + clientID,
				params: {
					paramName: paramValue,
					paramName: paramValue,
					paramName: paramValue
				},
				timeout : 1000 * 10 //(10 seconds)
				listeners : {
					success: function(data){  },
					failure: function(resultInfo){  }
				}
			};			
		*/
		
		Ext.applyIf(configObject, configObject, {
			timeout : Ext.Ajax.timeout,
		});

		Ext.applyIf(configObject.waiting, {
			title: "Waiting...",
			message: "Wait the task ends!"
		});

		Ext.applyIf(configObject.params, {

		});

		Ext.applyIf(configObject.listeners, { 
			success: null,
			failure: null
		});


		radio('AjaxManager:StartRequest').broadcast(configObject);

		var handlerRequestResult = function(response, configObject)
		{
			var endExecution = new Date();			
			var objectFromServer = Ext.JSON.decode(response.responseText, true);				
			objectFromServer = objectFromServer || { };

			var resultInfo = {
				data : objectFromServer,
				requestTime : (endExecution.getTime() - startExecution.getTime()),
				httpStatusCode : response.status,
				configObject: configObject, 
				response : response,
				isSuccess : (response.status >= 200 && response.status < 400)
			};

			radio('AjaxManager:EndRequest').broadcast(resultInfo);

			if(resultInfo.isSuccess)
			{
				if(Ext.isEmpty(resultInfo.configObject.listeners.success) == false)
				{
					resultInfo.configObject.listeners.success(resultInfo.data);
				}				
			}
			else
			{
				if(Ext.isEmpty(resultInfo.configObject.listeners.failure) == false)
				{
					resultInfo.configObject.listeners.failure(resultInfo);
				}
			}
		};

		var startExecution = new Date();
		Ext.Ajax.request({
			url: configObject.urlToSend,
			jsonData: configObject.params,
			success: function(response)
			{
				handlerRequestResult(response, configObject);
			},
			failure: function(response)
			{
				handlerRequestResult(response, configObject);
			},
			timeout: configObject.timeout
		});
	},

	//configObject.urlToSend
	//configObject.params
	downloadFile: function (configObject) {
		var urlToSend = configObject.urlToSend;
		for (var paramName in configObject.params) {
			var paramValue = configObject.params[paramName];
			if (Ext.isEmpty(paramValue) == false)
				urlToSend = Ext.String.urlAppend(urlToSend, Ext.String.format("{0}={1}", paramName, paramValue));
		}
		Ext.core.DomHelper.append(document.body, {
			id: Ext.id(),
			cn: [{
				tag: 'iframe',
				src: urlToSend,
				style: 'display:none'
			}]
		});
	}
};
Oragon.Architecture.Scripting.AjaxManager.init();